﻿using System;
using System.Linq;
using System.Reflection;

namespace Refactoring
{
    public class RemoteController
    {
        private readonly TvCommand[] _tvCommands;

        public RemoteController()
        {
            var tvOptionsController = new TvOptionsController();

            _tvCommands = new TvCommand[]
            {
                new TvTurnOnCommand(tvOptionsController),
                new TvTurnOffCommand(tvOptionsController),
                new TvIncreaseVolumeCommand(tvOptionsController),
                new TvDecreaseVolumeCommand(tvOptionsController),
                new TvIncreaseBrightnessCommand(tvOptionsController),
                new TvDecreaseBrightnessCommand(tvOptionsController),
                new TvIncreaseContrastCommand(tvOptionsController),
                new TvDecreaseContrastCommand(tvOptionsController),
                new TvGetOptionsToShowCommand(tvOptionsController),
            
                new TvMuteVolumeCommand(tvOptionsController),
                new TvUnmuteVolumeCommand(tvOptionsController),
                new TvSetDefaultOptionsCommand(tvOptionsController)
            };
        }

        public string Call(string command)
        {
            var currentCommand = _tvCommands.FirstOrDefault(cmd => cmd.Name == command);
            
            currentCommand?.Execute();

            return (currentCommand is TvGetOptionsToShowCommand tvGetOptionsCommand)
                ? tvGetOptionsCommand.Result
                : string.Empty;
        }
    }

    internal abstract class TvCommand
    {
        protected TvOptionsController TvOptionsController;

        public abstract string Name { get; }

        protected TvCommand(TvOptionsController tvOptionsController) => TvOptionsController = tvOptionsController;

        public abstract void Execute();
    }

    internal class TvTurnOnCommand : TvCommand
    {
        public TvTurnOnCommand(TvOptionsController tvOptionsController) : base(tvOptionsController) { }

        public override string Name { get; } = "Tv On";
        public override void Execute() => TvOptionsController.TurnOn();
    }

    internal class TvTurnOffCommand : TvCommand
    {
        public TvTurnOffCommand(TvOptionsController tvOptionsController) : base(tvOptionsController) { }

        public override string Name { get; } = "Tv Off";
        public override void Execute() => TvOptionsController.TurnOff();
    }

    internal class TvIncreaseVolumeCommand : TvCommand
    {
        public TvIncreaseVolumeCommand(TvOptionsController tvOptionsController) : base(tvOptionsController) { }

        public override string Name { get; } = "Volume Up";
        public override void Execute() => TvOptionsController.IncreaseVolume();
    }

    internal class TvDecreaseVolumeCommand : TvCommand
    {
        public TvDecreaseVolumeCommand(TvOptionsController tvOptionsController) : base(tvOptionsController) { }

        public override string Name { get; } = "Volume Down";
        public override void Execute() => TvOptionsController.DecreaseVolume();
    }

    internal class TvIncreaseBrightnessCommand : TvCommand
    {
        public TvIncreaseBrightnessCommand(TvOptionsController tvOptionsController) : base(tvOptionsController) { }

        public override string Name { get; } = "Options change brightness up";
        public override void Execute() => TvOptionsController.IncreaseBrightness();
    }

    internal class TvDecreaseBrightnessCommand : TvCommand
    {
        public TvDecreaseBrightnessCommand(TvOptionsController tvOptionsController) : base(tvOptionsController) { }

        public override string Name { get; } = "Options change brightness down";
        public override void Execute() => TvOptionsController.DecreaseBrightness();
    }

    internal class TvIncreaseContrastCommand : TvCommand
    {
        public TvIncreaseContrastCommand(TvOptionsController tvOptionsController) : base(tvOptionsController) { }

        public override string Name { get; } = "Options change contrast up";
        public override void Execute() => TvOptionsController.IncreaseContrast();
    }

    internal class TvDecreaseContrastCommand : TvCommand
    {
        public TvDecreaseContrastCommand(TvOptionsController tvOptionsController) : base(tvOptionsController) { }

        public override string Name { get; } = "Options change contrast down";
        public override void Execute() => TvOptionsController.DecreaseContrast();
    }

    internal class TvMuteVolumeCommand : TvCommand
    {
        public TvMuteVolumeCommand(TvOptionsController tvOptionsController) : base(tvOptionsController) { }

        public override string Name { get; } = "Volume Mute";
        public override void Execute() => TvOptionsController.MuteVolume();
    }

    internal class TvUnmuteVolumeCommand : TvCommand
    {
        public TvUnmuteVolumeCommand(TvOptionsController tvOptionsController) : base(tvOptionsController) { }

        public override string Name { get; } = "Volume Unmute";
        public override void Execute() => TvOptionsController.UnmuteVolume();
    }

    internal class TvSetDefaultOptionsCommand : TvCommand
    {
        public TvSetDefaultOptionsCommand(TvOptionsController tvOptionsController) : base(tvOptionsController) { }

        public override string Name { get; } = "Options set default";
        public override void Execute() => TvOptionsController.SetDefaultOptions();
    }

    internal class TvGetOptionsToShowCommand : TvCommand
    {
        public TvGetOptionsToShowCommand(TvOptionsController tvOptionsController) : base(tvOptionsController) { }

        public override string Name { get; } = "Options show";

        public string Result { get; set; }
        
        public override void Execute() => Result = TvOptionsController.GetOptionsToShow();
        
    }

    public class TvOptionsController
    {
        private const int MinValue = 0;
        private const int MaxValue = 100;
        private const int ValueOfOptionChange = 10;

        private readonly TvOptions _tvOptions = new TvOptions();

        private bool _isVolumeMuted;
        private int _volumeBeforeMute;

        public TvOptionsController() => SetDefaultOptions();

        public void SetDefaultOptions()
        {
            _tvOptions.Volume = 30;
            _tvOptions.Brightness = 20;
            _tvOptions.Contrast = 20;

            _isVolumeMuted = false;
            _volumeBeforeMute = _tvOptions.Volume;
        }

        public void TurnOn() => _tvOptions.IsOnline = true;

        public void TurnOff() => _tvOptions.IsOnline = false;

        public void IncreaseVolume()
        {
            if (!_tvOptions.IsOnline) return;
            
            if (_isVolumeMuted)
                UnmuteVolume();

            _tvOptions.Volume += ValueOfOptionChange;
            _tvOptions.Volume = Normalize(_tvOptions.Volume);
        }

        public void DecreaseVolume()
        {
            if (!_tvOptions.IsOnline) return;
            
            if (_isVolumeMuted)
                UnmuteVolume();

            _tvOptions.Volume -= ValueOfOptionChange;
            _tvOptions.Volume = Normalize(_tvOptions.Volume);
        }

        public void MuteVolume()
        {
            _isVolumeMuted = true;
            _volumeBeforeMute = _tvOptions.Volume;
            _tvOptions.Volume = MinValue;
        }

        public void UnmuteVolume()
        {
            _isVolumeMuted = false;
            _tvOptions.Volume = _volumeBeforeMute;
        }

        public void IncreaseBrightness()
        {
            if (!_tvOptions.IsOnline) return;
            
            _tvOptions.Brightness += ValueOfOptionChange;
            _tvOptions.Brightness = Normalize(_tvOptions.Brightness);
        }

        public void DecreaseBrightness()
        {
            if (!_tvOptions.IsOnline) return;
            
            _tvOptions.Brightness -= ValueOfOptionChange;
            _tvOptions.Brightness = Normalize(_tvOptions.Brightness);
        }

        public void IncreaseContrast()
        {
            if (!_tvOptions.IsOnline) return;
            
            _tvOptions.Contrast += ValueOfOptionChange;
            _tvOptions.Contrast = Normalize(_tvOptions.Contrast);
        }

        public void DecreaseContrast()
        {
            if (!_tvOptions.IsOnline) return;
            
            _tvOptions.Contrast -= ValueOfOptionChange;
            _tvOptions.Contrast = Normalize(_tvOptions.Contrast);
        }

        private int Normalize(int value)
        {
            if (value < MinValue)
                value = MinValue;
            if (value > MaxValue)
                value = MaxValue;

            return value;
        }
        
        public string GetOptionsToShow()
        {
            var propsWithTitle = _tvOptions.GetType().GetProperties()
                .Where(prop => prop.IsDefined(typeof(TitleToShowAttribute)));

            var titlesAndValues = propsWithTitle
                .Select(prop =>
                {
                    var title = prop.GetCustomAttribute<TitleToShowAttribute>().Name;
                    var value = prop.GetValue(_tvOptions);
                    return $"{title} {value}";
                });

            return $"Options:\n{string.Join("\n", titlesAndValues)}";
        }
        
        private class TvOptions
        {
            [TitleToShow("IsOnline")] 
            public bool IsOnline { get; set; }

            [TitleToShow("Volume")] 
            public int Volume { get; set; }

            [TitleToShow("Brightness")] 
            public int Brightness { get; set; }

            [TitleToShow("Contrast")] 
            public int Contrast { get; set; }
        }
        
        [AttributeUsage(AttributeTargets.Property)]
        public class TitleToShowAttribute : Attribute
        {
            public string Name { get; }
        
            public TitleToShowAttribute(string name) => Name = name;
        }
    }
}
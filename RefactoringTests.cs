using NUnit.Framework;

namespace Refactoring
{
    [TestFixture]
    public class RefactoringTests
    {
        [Test]
        public void ShowOptions_DefaultValues()
        {
            var rc = new RemoteController();
            var optionsShowResult = rc.Call("Options show");
            CollectionAssert.AreEquivalent(
                new[] {"Options:", "IsOnline False", "Volume 30", "Brightness 20", "Contrast 20"},
                optionsShowResult.Split('\n'));
        }

        [Test]
        public void BrightnessUp()
        {
            var rc = new RemoteController();
            rc.Call("Tv On");
            rc.Call("Options change brightness up");
            var optionsShowResult = rc.Call("Options show");
            CollectionAssert.AreEquivalent(
                new[] {"Options:", "IsOnline True", "Volume 30", "Brightness 30", "Contrast 20"},
                optionsShowResult.Split('\n'));
        }

        [Test]
        public void SetDefaultAfterChange()
        {
            var rc = new RemoteController();

            rc.Call("Tv On");
            rc.Call("Options change brightness up");
            rc.Call("Options change contrast up");
            rc.Call("Volume up");

            rc.Call("Options set default");
            var defaultOptions = rc.Call("Options show");

            CollectionAssert.AreEquivalent(
                new[] {"Options:", "IsOnline True", "Volume 30", "Brightness 20", "Contrast 20"},
                defaultOptions.Split('\n'));
        }

        [Test]
        public void MuteVolume()
        {
            var rc = new RemoteController();
            rc.Call("Tv On");

            rc.Call("Volume Mute");
            var optionsWithMutedVolume = rc.Call("Options show");

            CollectionAssert.AreEquivalent(
                new[] {"Options:", "IsOnline True", "Volume 0", "Brightness 20", "Contrast 20"},
                optionsWithMutedVolume.Split('\n'));
        }

        [Test]
        public void MuteAndUnmuteVolume()
        {
            var rc = new RemoteController();
            rc.Call("Tv On");

            rc.Call("Volume Mute");
            var optionsWithMutedVolume = rc.Call("Options show");

            CollectionAssert.AreEquivalent(
                new[] {"Options:", "IsOnline True", "Volume 0", "Brightness 20", "Contrast 20"},
                optionsWithMutedVolume.Split('\n'));

            rc.Call("Volume Unmute");
            var defaultOptions = rc.Call("Options show");

            CollectionAssert.AreEquivalent(
                new[] {"Options:", "IsOnline True", "Volume 30", "Brightness 20", "Contrast 20"},
                defaultOptions.Split('\n'));
        }
    }
}
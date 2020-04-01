using NUnit.Framework;

namespace Refactoring
{
    [TestFixture]
    public class RefactoringTests
    {
        void TestRemoteController(string[] commands, string[] expectedOutput)
        {
            var rc = new RemoteController();

            foreach (var command in commands)
            {
                rc.Call(command);
            }

            var optionsShowParts = rc.Call("Options show").Split('\n');

            Assert.AreEqual(expectedOutput.Length, optionsShowParts.Length);
            Assert.AreEqual(expectedOutput, optionsShowParts);
        }

        [Test]
        public void ShowDefaultOptions()
        {
            TestRemoteController(new string[0],
                new[]
                {
                    "Options:",
                    "IsOnline False",
                    "Volume 30",
                    "Brightness 20",
                    "Contrast 20"
                });
        }

        [Test]
        public void TurnOnTv()
        {
            TestRemoteController(new[]
                {
                    "Tv On"
                },
                new[]
                {
                    "Options:",
                    "IsOnline True",
                    "Volume 30",
                    "Brightness 20",
                    "Contrast 20"
                });
        }

        [Test]
        public void TurnOffTv()
        {
            TestRemoteController(new[]
                {
                    "Tv On",
                    "Tv Off"
                },
                new[]
                {
                    "Options:",
                    "IsOnline False",
                    "Volume 30",
                    "Brightness 20",
                    "Contrast 20"
                });
        }

        [Test]
        public void UpVolume()
        {
            TestRemoteController(new[]
                {
                    "Tv On",
                    "Volume Up"
                },
                new[]
                {
                    "Options:",
                    "IsOnline True",
                    "Volume 40",
                    "Brightness 20",
                    "Contrast 20"
                });
        }

        [Test]
        public void DownVolume()
        {
            TestRemoteController(new[]
                {
                    "Tv On",
                    "Volume Down"
                },
                new[]
                {
                    "Options:",
                    "IsOnline True",
                    "Volume 20",
                    "Brightness 20",
                    "Contrast 20"
                });
        }

        [Test]
        public void DontUpVolume_When_TvIsOff()
        {
            TestRemoteController(new[]
                {
                    "Volume Up"
                },
                new[]
                {
                    "Options:",
                    "IsOnline False",
                    "Volume 30",
                    "Brightness 20",
                    "Contrast 20"
                });
        }

        [Test]
        public void DontDownVolume_When_TvIsOff()
        {
            TestRemoteController(new[]
                {
                    "Volume Down"
                },
                new[]
                {
                    "Options:",
                    "IsOnline False",
                    "Volume 30",
                    "Brightness 20",
                    "Contrast 20"
                });
        }

        [Test]
        public void UpBrightness()
        {
            TestRemoteController(new[]
                {
                    "Tv On",
                    "Options change brightness up"
                },
                new[]
                {
                    "Options:",
                    "IsOnline True",
                    "Volume 30",
                    "Brightness 30",
                    "Contrast 20"
                });
        }

        [Test]
        public void DownBrightness()
        {
            TestRemoteController(new[]
                {
                    "Tv On",
                    "Options change brightness down"
                },
                new[]
                {
                    "Options:",
                    "IsOnline True",
                    "Volume 30",
                    "Brightness 10",
                    "Contrast 20"
                });
        }

        [Test]
        public void DontUpBrightness_When_TvIsOff()
        {
            TestRemoteController(new[]
                {
                    "Options change brightness up"
                },
                new[]
                {
                    "Options:",
                    "IsOnline False",
                    "Volume 30",
                    "Brightness 20",
                    "Contrast 20"
                });
        }

        [Test]
        public void DontDownBrightness_When_TvIsOff()
        {
            TestRemoteController(new[]
                {
                    "Options change brightness down"
                },
                new[]
                {
                    "Options:",
                    "IsOnline False",
                    "Volume 30",
                    "Brightness 20",
                    "Contrast 20"
                });
        }

        [Test]
        public void UpContrast()
        {
            TestRemoteController(new[]
                {
                    "Tv On",
                    "Options change contrast up"
                },
                new[]
                {
                    "Options:",
                    "IsOnline True",
                    "Volume 30",
                    "Brightness 20",
                    "Contrast 30"
                });
        }

        [Test]
        public void DownContrast()
        {
            TestRemoteController(new[]
                {
                    "Tv On",
                    "Options change contrast down"
                },
                new[]
                {
                    "Options:",
                    "IsOnline True",
                    "Volume 30",
                    "Brightness 20",
                    "Contrast 10"
                });
        }

        [Test]
        public void DontUpContrast_When_TvIsOff()
        {
            TestRemoteController(new[]
                {
                    "Options change contrast up"
                },
                new[]
                {
                    "Options:",
                    "IsOnline False",
                    "Volume 30",
                    "Brightness 20",
                    "Contrast 20"
                });
        }

        [Test]
        public void DontDownContrast_When_TvIsOff()
        {
            TestRemoteController(new[]
                {
                    "Options change contrast down"
                },
                new[]
                {
                    "Options:",
                    "IsOnline False",
                    "Volume 30",
                    "Brightness 20",
                    "Contrast 20"
                });
        }

        [Test]
        public void SetOptionsDefaultAfterChange()
        {
            TestRemoteController(new[]
                {
                    "Tv On",
                    "Options change brightness up",
                    "Options change contrast up",
                    "Volume up",

                    "Options set default"
                },
                new[]
                {
                    "Options:",
                    "IsOnline True",
                    "Volume 30",
                    "Brightness 20",
                    "Contrast 20"
                });
        }

        [Test]
        public void MuteVolume()
        {
            TestRemoteController(new[]
            {
                "Tv On",
                "Volume Mute"
            }, new[]
            {
                "Options:",
                "IsOnline True",
                "Volume 0",
                "Brightness 20",
                "Contrast 20"
            });
        }

        [Test]
        public void MuteAndUnmuteVolume()
        {
            TestRemoteController(new[]
            {
                "Tv On",
                "Volume Mute",
                "Volume Unmute"
            }, new[]
            {
                "Options:",
                "IsOnline True",
                "Volume 30",
                "Brightness 20",
                "Contrast 20"
            });
        }
        
        [Test]
        public void MuteVolumeAndUpVolume()
        {
            TestRemoteController(new[]
            {
                "Tv On",
                "Volume Mute",
                "Volume Up"
            }, new[]
            {
                "Options:",
                "IsOnline True",
                "Volume 40",
                "Brightness 20",
                "Contrast 20"
            });
        }
        
        [Test]
        public void MuteVolumeAndDownVolume()
        {
            TestRemoteController(new[]
            {
                "Tv On",
                "Volume Mute",
                "Volume Down"
            }, new[]
            {
                "Options:",
                "IsOnline True",
                "Volume 20",
                "Brightness 20",
                "Contrast 20"
            });
        }
        
        [Test]
        public void UpVolumeManyTimes()
        {
            TestRemoteController(new[]
            {
                "Tv On",
                "Volume Up", "Volume Up", "Volume Up", "Volume Up", 
                "Volume Up", "Volume Up", "Volume Up", "Volume Up", 
                "Volume Up", "Volume Up", "Volume Up", "Volume Up",
            }, new[]
            {
                "Options:",
                "IsOnline True",
                "Volume 100",
                "Brightness 20",
                "Contrast 20"
            });
        }
        
        [Test]
        public void DownVolumeManyTimes()
        {
            TestRemoteController(new[]
            {
                "Tv On",
                "Volume Down", "Volume Down", "Volume Down", 
                "Volume Down", "Volume Down", "Volume Down", 
                "Volume Down", "Volume Down", "Volume Down",
            }, new[]
            {
                "Options:",
                "IsOnline True",
                "Volume 0",
                "Brightness 20",
                "Contrast 20"
            });
        }
    }
}
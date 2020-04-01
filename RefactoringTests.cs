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
        public void BrightnessUp()
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
    }
}
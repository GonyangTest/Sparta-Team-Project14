using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{
    public class SoundManager
    {
        private static SoundManager instance;

        public static SoundManager Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new SoundManager();
                }
                return instance;
            }
        }

        private SoundManager() { }

        private Music backgroundMusic;

        public void StartMainMusic()
        {
            backgroundMusic = new Music("Sounds/Main.mp3");
            backgroundMusic.SetVolume(0.05f);
            backgroundMusic.PlayLooping();
        }

        public void StartDungeonMusic()
        {
            backgroundMusic = new Music("Sounds/Dungeon.mp3");
            backgroundMusic.SetVolume(0.05f);
            backgroundMusic.PlayLooping();
        }


        public void StopMusic()
        {
            if (backgroundMusic != null)
            {
                backgroundMusic.Stop();
                backgroundMusic.Dispose();
                backgroundMusic = null;
            }
        }
    }
}

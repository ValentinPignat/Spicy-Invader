using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace Spicy_Invader
{
    internal class SoundManager
    {
        /// <summary>
        /// Toggle sound on/off
        /// </summary>
        public bool SoundOn { get; set; } = true;

        /// <summary>
        /// Retro Game SFX Explosion by suntemple -- https://freesound.org/s/253169/ -- License: Creative Commons 0
        /// </summary>
        SoundPlayer _collisionSound = new SoundPlayer("253169__suntemple__retro-game-sfx-explosion.wav");

        /// <summary>
        /// Retro Blaster Fire by astrand -- https://freesound.org/s/328011/ -- License: Creative Commons 0
        /// </summary>
        SoundPlayer _firingSound = new SoundPlayer("328011__astrand__retro-blaster-fire.wav");

        /// <summary>
        /// Retro laser 2 by MaoDin204 -- https://freesound.org/s/717687/ -- License: Creative Commons 0
        /// </summary>
        SoundPlayer _gameOverSound = new SoundPlayer("717687__maodin204__retro-laser-2.wav");

        /// <summary>
        /// Retro video game sfx - Tiny warble by OwlStorm -- https://freesound.org/s/404780/ -- License: Creative Commons 0
        /// </summary>
        SoundPlayer _menuSound = new SoundPlayer("404780__owlstorm__retro-video-game-sfx-tiny-warble.wav");

        public SoundManager() {
            _gameOverSound.Load();
            _firingSound.Load();
            _collisionSound.Load();
            _menuSound.Load();
        }

        public void MenuSound() {
            if (SoundOn) { _menuSound.Play(); }
        }
        public void FiringSound()
        {
            if (SoundOn) { _firingSound.Play(); }
        }
        public void CollisionSound()
        {
            if (SoundOn) { _collisionSound.Play(); }
        }
        public void GameOverSound()
        {
            if (SoundOn) { _gameOverSound.Play(); }
        }
    }
}

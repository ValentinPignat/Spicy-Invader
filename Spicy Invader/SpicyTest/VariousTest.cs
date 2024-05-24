/// ETML
/// Author : Valentin Pignat
/// Date (creation): 24.05.2024
/// Description: Test Class for various tests executable without display or sound

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spicy_Invader;
using static Spicy_Invader.Game;

namespace SpicyTest
{
    /// <summary>
    /// Test Class for various tests executable without display or sound
    /// </summary>
    [TestClass]
    public class VariousTest
    {

        /// <summary>
        /// Check CastOutOfBounds method by trying to cast an enemy out of bounds
        /// </summary>
        [TestMethod]
        public void StayInbound()
        {
            // Arrange 
            const int COL = 1;
            const int ROW = 2;
            const bool EXP_VALID = false;

            SoundManager soundManager = new SoundManager();
            EnemyBlock enemyBlock = new EnemyBlock(easymode: true, soundManager: soundManager);
            Enemy enemy = new Enemy(x: Game.WIDTH/2 + Game.MARGIN_SIDE, y: Game.HEIGHT/ 2 + Game.MARGIN_TOP_BOTTOM, col: COL, row: ROW, owner: enemyBlock);

            // Act
            bool valid = enemy.CastOutOfBounds(Game.WIDTH, Game.HEIGHT);


            // Assert 
            Assert.AreEqual(EXP_VALID, valid);
            
        }
    }
}

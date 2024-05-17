/// ETML
/// Author : Valentin Pignat
/// Date (creation): 17.05.2024
/// Description: Test Class for constructors used in SpicyInvader

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spicy_Invader;


namespace SpicyTest
{
    /// <summary>
    /// Test Class for constructors used in SpicyInvader
    /// </summary>
    [TestClass]
    public class ConstructorTest
    {
        /// <summary>
        /// Test brick Constructor
        /// </summary>
        [TestMethod]
        public void BrickConstructor()
        {
            // Arrange 
            const int EXP_X = 100;
            const int EXP_Y= 50; 
            Brick brick = new Brick(x: EXP_X, y: EXP_Y);

            // Act

            // Assert 
            Assert.AreEqual(EXP_X, brick.X);
            Assert.AreEqual(EXP_Y, brick.Y);
        }

        /// <summary>
        /// Test Enemy Constructor
        /// </summary>
        [TestMethod]
        public void EnemyConstructor()
        {
            // Arrange 
            const int EXP_X = 100;
            const int EXP_Y = 50;
            const int EXP_COL = 1;
            const int EXP_ROW = 2;

            SoundManager soundManager = new SoundManager();
            EnemyBlock enemyBlock = new EnemyBlock(easymode: true, soundManager:soundManager) ;
            Enemy enemy = new Enemy(x: EXP_X, y:EXP_Y, col:EXP_COL, row: EXP_ROW, owner: enemyBlock);

            // Act

            // Assert 
            Assert.AreEqual(EXP_X, enemy.X);
            Assert.AreEqual(EXP_Y, enemy.Y);
            Assert.AreEqual(EXP_COL , enemy.Col);
            Assert.AreEqual(EXP_ROW, enemy.Row);
            Assert.AreEqual(enemyBlock, enemy.Owner);
        }

        /// <summary>
        /// Test EnemyBlock constructor on easy mode
        /// </summary>
        [TestMethod]
        public void EnemyBlockEasy()
        {
            // Arrange 
            const bool EXP_EASY_MODE = true;

            SoundManager soundManager = new SoundManager();
            EnemyBlock enemyBlock = new EnemyBlock(easymode: EXP_EASY_MODE, soundManager: soundManager);

            // Act

            // Assert 
            Assert.AreEqual(soundManager, enemyBlock.SoundManager);
            Assert.AreEqual(EnemyBlock.MAX_ACTIVE_MISSILES, enemyBlock.MaxActiveMissile);
        }

        /// <summary>
        /// Test EnemyBlock constructor on easy mode
        /// </summary>
        [TestMethod]
        public void EnemyBlockHard()
        {
            // Arrange 
            const bool EXP_EASY_MODE = false;

            SoundManager soundManager = new SoundManager();
            EnemyBlock enemyBlock = new EnemyBlock(easymode: EXP_EASY_MODE, soundManager: soundManager);

            // Act

            // Assert 
            Assert.AreEqual(soundManager, enemyBlock.SoundManager);
            Assert.AreEqual(EnemyBlock.MAX_ACTIVE_MISSILES_HARD, enemyBlock.MaxActiveMissile);
        }

        /// <summary>
        /// Test Highscore
        /// </summary>
        [TestMethod]
        public void HighscoreConstructor()
        {
            // Arrange 
            const int EXP_SCORE = 100;
            const string EXP_USERNAME = "Player one";

            Highscore highscore = new Highscore(score:EXP_SCORE, username: EXP_USERNAME);

            // Act

            // Assert 
            Assert.AreEqual(EXP_SCORE, highscore.Score);
            Assert.AreEqual(EXP_USERNAME, highscore.Username);
        }
    }
}

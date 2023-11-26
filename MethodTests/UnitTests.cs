using ADS;
using NUnit.Framework;

namespace MethodTests
{
    public class UnitTests
    {
        private Game _game;

        [SetUp]
        public void Setup()
        {
            _game = new Game();
        }

        [Test]
        public void CalculateDamage_WithPositiveMultiplier_ReturnsPositiveDamage()
        {
            // Arrange
            int winner = 1;
            int multiplier = 1;

            // Act
            int damage = _game.CalculateDamage(winner, multiplier);

            // Assert
            Assert.Greater(damage, 0, "Damage should be positive when multiplier is positive.");
        }

        [Test]
        public void CalculateDamage_WithZeroMultiplier_ReturnsZero()
        {
            // Arrange
            int winner = 1;
            int multiplier = 0;

            // Act
            int damage = _game.CalculateDamage(winner, multiplier);

            // Assert
            Assert.AreEqual(0, damage, "Damage should be zero when multiplier is zero.");
        }

        [Test]
        public void CalculateDamage_WithNegativeMultiplier_ReturnsNonNegativeDamage()
        {
            // Arrange
            int winner = 1;
            int multiplier = -1;

            // Act
            int damage = _game.CalculateDamage(winner, multiplier);

            // Assert
            Assert.GreaterOrEqual(damage, 0, "Damage should be non-negative even when multiplier is negative.");
        }

        [Test]
        public void CalculateDamage_WithDifferentWinnerValues_ReturnsInteger()
        {
            // Arrange
            int multiplier = 1;

            // Act & Assert
            Assert.IsInstanceOf(typeof(int), _game.CalculateDamage(0, multiplier), "Damage should be an integer for winner = 0.");
            Assert.IsInstanceOf(typeof(int), _game.CalculateDamage(2, multiplier), "Damage should be an integer for winner = 2.");
        }

        [Test]
        public void CalculateDamage_WithMaxPossibleRandomValue_ReturnsExpectedMaxDamage()
        {
            // Arrange
            int winner = 1;
            int multiplier = 1;
            int maxRandomValue = 14; 
            int expectedMaxDamage = maxRandomValue * multiplier;

            // Act
            int damage = _game.CalculateDamage(winner, multiplier);

            // Assert
            Assert.LessOrEqual(damage, expectedMaxDamage, "Damage should not exceed the expected maximum value.");
        }

        [Test]
        public void CalculateDamage_WithMinPossibleRandomValue_ReturnsExpectedMinDamage()
        {
            // Arrange
            int winner = 1;
            int multiplier = 1;
            int minRandomValue = 5; 
            int expectedMinDamage = minRandomValue * multiplier;

            // Act
            int damage = _game.CalculateDamage(winner, multiplier);

            // Assert
            Assert.GreaterOrEqual(damage, expectedMinDamage, "Damage should not be less than the expected minimum value.");
        }
    }


    // INTEGRATION TESTS
    public class IntegrationTests
    {
        private Game _game;

        [SetUp]
        public void Setup()
        {
            _game = new Game();
            _game.Start();  
        }

        [Test]
        public void CalculateDamage_AppliesDamageToEnemy_WhenWinnerIsPlayer()
        {
            // Arrange
            int initialEnemyHealth = _game.GetEnemyHealth();
            int winner = 1;  
            int multiplier = 1;

            // Act
            _game.CalculateDamage(winner, multiplier);

            // Assert
            Assert.Less(_game.GetEnemyHealth(), initialEnemyHealth, "Enemy health should decrease after player wins.");
        }

        [Test]
        public void CalculateDamage_AppliesDamageToPlayer_WhenWinnerIsEnemy()
        {
            // Arrange
            int initialPlayerHealth = _game.GetPlayerHealth();
            int winner = 0;  
            int multiplier = 1;

            // Act
            _game.CalculateDamage(winner, multiplier);

            // Assert
            Assert.Less(_game.GetPlayerHealth(), initialPlayerHealth, "Player health should decrease after enemy wins.");
        }

        [Test]
        public void CalculateDamage_BehavesCorrectly_InDifferentGameStates()
        {
            // Arrange
            _game.SetPlayerHealth(10); 
            int winner = 0;  
            int multiplier = 1;

            // Act
            _game.CalculateDamage(winner, multiplier);

            // Assert
            Assert.IsFalse(_game.GetFightingStatus(), "Game should end when player health reaches 0 or below.");
        }

       
    }


}
using System;
using System.Collections.Generic;
using System.IO;

namespace TextBasedGame
{
    enum GameState
    {
        MainMenu,
        Battle,
        City,
        Dead
    }

    class Program
    {
        static bool isScoreSaved; // New flag to track if the score has been saved
        static GameState gameState;
        static Player player;
        static List<Enemy> enemies;
        static int currentRound;

        static void Main(string[] args)
        {
            gameState = GameState.MainMenu;
            isScoreSaved = false; // Initialize the flag

            while (true)
            {
                switch (gameState)
                {
                    case GameState.MainMenu:
                        DisplayMainMenu();
                        break;
                    case GameState.Battle:
                        StartBattle();
                        break;
                    case GameState.City:
                        VisitCity();
                        break;
                    case GameState.Dead:
                        if (!isScoreSaved) // Check if the score has already been saved
                        {
                            GameOver();
                            isScoreSaved = true; // Set the flag to indicate that the score has been saved
                        }
                        break;
                }

                // Check game state to exit the loop
                if (gameState == GameState.MainMenu)
                {
                    break;
                }
            }
        }

        private static void VisitCity()
        {
            throw new NotImplementedException();
        }

        private static void GameOver()
        {
            throw new NotImplementedException();
        }

        static void DisplayMainMenu()
        {
            Console.Clear();
            Console.WriteLine("Welcome to the Text-Based Game!");
            Console.WriteLine("1. Start New Game");
            Console.WriteLine("2. Quit");

            int choice = GetNumericInput(1, 2);

            if (choice == 1)
            {
                NewGame();
                StartBattle();
            }
            else
            {
                Environment.Exit(0);
            }
        }

private static int GetNumericInput(int minValue, int maxValue)
{
    int input;
    while (true)
    {
        Console.Write("Enter your choice: ");
        if (int.TryParse(Console.ReadLine(), out input))
        {
            if (input >= minValue && input <= maxValue)
            {
                return input;
            }
        }
        Console.WriteLine("Invalid input. Please try again.");
    }
}
        static void NewGame()
        {
            Console.Clear();
            Console.WriteLine("Enter your name: ");
            string playerName = Console.ReadLine();
            player = new Player(playerName);

            currentRound = 1;
            gameState = GameState.Battle;
        }

        static void StartBattle()
{
    Console.Clear();
    Console.WriteLine("Round " + currentRound);
    Console.WriteLine(player.Name + " vs Enemies");

    // Initialize enemies for the current round
    enemies = new List<Enemy>();
    enemies.Add(new Goblin("Goblin 1"));
    enemies.Add(new Dragon("Dragon 1"));

            while (player.IsAlive && enemies.Count > 0)
            {
                bool enemyAttacked = false; // Flag to track if an enemy has attacked

                Console.WriteLine();
                Console.WriteLine("What would you like to do?");
                Console.WriteLine("1. Attack");
                Console.WriteLine("2. Check Player Stats");

                int choice = GetNumericInput(1, 2);

                if (choice == 1)
                {
                    player.Attack(enemies);

                    // Remove defeated enemies in reverse order
                    for (int i = enemies.Count - 1; i >= 0; i--)
                    {
                        if (enemies[i].IsDefeated)
                        {
                            Console.WriteLine("You defeated " + enemies[i].Name + "!");
                            player.Score += 10; // Increase the score by 10
                            enemies.RemoveAt(i);
                        }
                    }
                }
                else
                {
                    player.DisplayStats();
                }

                // Perform enemy turn
                foreach
(Enemy enemy in enemies)
{
    if (enemy.IsDefeated)
    {
        continue; // Skip the defeated enemy
    }

    enemy.Attack(player);
    enemyAttacked = true;
    break; // Only allow one enemy to attack per round
}

if (!enemyAttacked)
{
    Console.WriteLine("Enemies cannot attack at the moment.");
}

if (player.IsAlive && enemies.Count == 0)
{
    Console.WriteLine("Congratulations! You have defeated all enemies in round " + currentRound);
    currentRound++;

    Console.WriteLine();
    Console.WriteLine("What would you like to do?");
    Console.WriteLine("1. Continue to the next round");
    Console.WriteLine("2. Visit the City");


        if (GetNumericInput(1, 2) == 1)
    {
        StartBattle();
    }
    else
    {
        gameState = GameState.City;
    }
}
else if (!player.IsAlive)
{
    gameState = GameState.Dead;
}
}

static void VisitCity()
{
    Console.Clear();
    Console.WriteLine("Welcome to the City!");
    Console.WriteLine("1. Rest and Recover Health");
    Console.WriteLine("2. Save and Quit");

    int choice = GetNumericInput(1, 2);

    if (choice == 1)
    {
        player.Rest();
        gameState = GameState.Battle;
    }
    else
    {
        SaveGame();
        gameState = GameState.MainMenu;
    }
}

static void GameOver()
{
    Console.Clear();
    Console.WriteLine("Game Over!");
    Console.WriteLine("Your final score: " + player.Score);

    Console.WriteLine();
    Console.WriteLine("1. Save and Quit");
    Console.WriteLine("2. Quit without saving");

    int choice = GetNumericInput(1, 2);
static void GameOver()
{
    Console.Clear();
    Console.WriteLine("Game Over!");
    Console.WriteLine("Your final score: " + player.Score);

    Console.WriteLine();
    Console.WriteLine("1. Save and Quit");
    Console.WriteLine("2. Quit without saving");

    int choice = GetNumericInput(1, 2);

    if (choice == 1)
    {
        SaveGame();
    }

    Environment.Exit(0);
}
    if (choice == 1)
    {
        SaveGame();
    }

    Environment.Exit(0);
}

static void SaveGame()
{
    string saveData = player.Name + "," + player.Score;
    File.WriteAllText("save.txt", saveData);
    Console.WriteLine("Game saved successfully!");
}

static void LoadGame()
{
    if (File.Exists("save.txt"))
    {
        string saveData = File.ReadAllText("save.txt");
        string[] values = saveData.Split(',');

        if (values.Length == 2)
        {
            string playerName = values[0];
            int score;

            if (int.TryParse(values[1], out score))
            {
                player = new Player(playerName);
                player.Score = score;
                Console.WriteLine("Game loaded successfully!");
                gameState = GameState.City;
            }
        }
    }
    else
    {
        Console.WriteLine("No saved game found.");
    }
}
}

class Player
{
    public string Name { get; private set; }
    public int Health { get; private set; }
    public int AttackPower { get; private set; }
    public int Defense { get; private set; }
    public bool IsAlive { get; private set; }
    public int Score { get; set; }

    public Player(string name)
    {
        Name = name;
        Health = 100;
        AttackPower = 10;
        Defense = 5;
        IsAlive = true;
        Score = 0;
    }

    public void Attack(List<Enemy> enemies)
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.TakeDamage(AttackPower);
        }
    }

    public void TakeDamage(int damage)
    {
        int inflictedDamage = Math.Max(damage - Defense, 0);
        Health -= inflictedDamage;

        Console.WriteLine("You take " + inflictedDamage + " damage!");

        if (Health <= 0)
        {
            Console.WriteLine
            ("You have been defeated!");
            IsAlive = false;
        }
        else
        {
            Console.WriteLine("Remaining health: " + Health);
        }
    }

    public void TakeDamage(int damage, Enemy enemy)
    {
        int inflictedDamage = Math.Max(damage - Defense, 0);
        Health -= inflictedDamage;

        Console.WriteLine("You take " + inflictedDamage + " damage!");

        if (Health <= 0)
        {
            Console.WriteLine("You have been defeated!");
            IsAlive = false;
        }
        else
        {
            Console.WriteLine("Remaining health: " + Health);
        }

        if (enemy.IsDefeated)
        {
            Console.WriteLine("You defeated " + enemy.Name + "!");
            Score += 10;
        }
    }

    public void Rest()
    {
        Health = 100;
        Console.WriteLine("Health has been restored to full.");
    }

    public void DisplayStats()
    {
        Console.WriteLine("Player Stats:");
        Console.WriteLine("Player Name: " + Name);
        Console.WriteLine("Health: " + Health);
        Console.WriteLine("Attack Power: " + AttackPower);
        Console.WriteLine("Defense: " + Defense);
    }
}

class Enemy
{
    public string Name { get; private set; }
    public int Health { get; private set; }
    public int AttackPower { get; private set; }
    public int Defense { get; private set; }
    public bool IsDefeated => Health <= 0;

    protected static readonly Random random = new Random();

    public Enemy(string name)
    {
        Name = name;
        Health = random.Next(10, 31);
        AttackPower = random.Next(10, 31);
        Defense = 3;
    }

    public void TakeDamage(int damage)
    {
        int inflictedDamage = Math.Max(damage - Defense, 0);
        Health -= inflictedDamage;

        Console.WriteLine("You dealt " + inflictedDamage + " damage to " + Name);

        if (Health <= 0)
        {
            Console.WriteLine(Name + " has been defeated!");
        }
        else
        {
            Console.WriteLine(Name + " remaining health: " + Health);
        }
    }

    public void Attack(Player player)
    {
        player.TakeDamage(AttackPower, this);
    }
}

class Goblin : Enemy
{
    public Goblin(string name) : base(name)
    {
        // Customize Goblin properties if needed
    }
}

class Dragon : Enemy
{
    public Dragon(string name) : base(name)
    {
        // Customize Dragon properties if needed
                }
            }
        }    
    }
}

namespace XTorGame.GameEngine;

public class XTorGameEngine
{
    public Player Player { get; private set; }
    public List<Enemy> Enemies { get; private set; } = [];
    public List<Laser> Lasers { get; private set; } = [];
    public float GameWidth { get; private set; }
    public float GameHeight { get; private set; }
    public int Score { get; private set; }
    public bool GameOver { get; private set; }

    // Background management
    public string CurrentBackground { get; private set; } = "skycloud.png";
    private float _backgroundTimer = 0;
    private const float BACKGROUND_SWAP_INTERVAL = 5.0f; // Swap every 5 seconds

    private Random _random = new();
    private float _enemySpawnTimer = 0;
    private const float ENEMY_SPAWN_INTERVAL = 2.0f; // Spawn enemy every 2 seconds

    public XTorGameEngine(float width, float height)
    {
        GameWidth = width;
        GameHeight = height;

        Player = new Player
        {
            X = width / 2 - 40, // Center horizontally
            Y = height - 100    // Near bottom
        };
    }

    public void Update(float deltaTime)
    {
        if (GameOver) return;

        // Update background timer
        _backgroundTimer += deltaTime;
        if (_backgroundTimer >= BACKGROUND_SWAP_INTERVAL)
        {
            _backgroundTimer = 0;
            CurrentBackground = CurrentBackground == "skycloud.png" ? "skycloud2.jpg" : "skycloud.png";
        }

        // Update player
        Player.Update(deltaTime);

        // Keep player on screen
        if (Player.X < 0) Player.X = 0;
        if (Player.X + Player.Width > GameWidth) Player.X = GameWidth - Player.Width;
        if (Player.Y < 0) Player.Y = 0;
        if (Player.Y + Player.Height > GameHeight) Player.Y = GameHeight - Player.Height;

        // Update lasers
        for (int i = Lasers.Count - 1; i >= 0; i--)
        {
            Lasers[i].Update(deltaTime);
            if (!Lasers[i].IsActive)
            {
                Lasers.RemoveAt(i);
            }
        }

        // Update enemies
        for (int i = Enemies.Count - 1; i >= 0; i--)
        {
            var enemy = Enemies[i];
            enemy.Update(deltaTime);

            // Remove enemies that go off screen
            if (enemy.Y > GameHeight + enemy.Height)
            {
                if (enemy.Type == EnemyType.Chauve)
                {
                    // Add 10 points if Chauve escapes
                    Score += 10;
                }
                else if (enemy.Type == EnemyType.Chx)
                {
                    // Game over if Chx escapes
                    GameOver = true;
                    return;
                }
                Enemies.RemoveAt(i);
                continue;
            }

            // Check collision with player
            if (enemy.Type == EnemyType.Chx && enemy.CollidesWith(Player))
            {
                GameOver = true;
                return;
            }

            // Handle enemy multiplication (for Chauve type)
            if (enemy.Type == EnemyType.Chx && enemy.ShouldMultiply && Enemies.Count < 20) // Only Chx can multiply
            {
                enemy.ShouldMultiply = false; // Reset flag
                Enemies.Add(enemy.CreateCopy());
            }
        }

        // Check laser-enemy collisions
        for (int i = Lasers.Count - 1; i >= 0; i--)
        {
            var laser = Lasers[i];
            for (int j = Enemies.Count - 1; j >= 0; j--)
            {
                var enemy = Enemies[j];
                if (laser.CollidesWith(enemy))
                {
                    if (enemy.Type == EnemyType.Chx)
                    {
                        // Only Chx enemies can be killed by lasers
                        Enemies.RemoveAt(j);
                        Lasers.RemoveAt(i);
                        Score += 10;
                        break; // Exit inner loop since laser is removed
                    }
                    else if (enemy.Type == EnemyType.Chauve)
                    {
                        // When Chauve is hit, spawn 2 Chx at its position
                        float baseX = enemy.X;
                        float baseY = enemy.Y;
                        float velocityY = enemy.VelocityY;
                        Enemies.RemoveAt(j);
                        Lasers.RemoveAt(i);
                        // Spawn 2 Chx
                        var chx1 = new Enemy(EnemyType.Chx) { X = baseX, Y = baseY, VelocityY = velocityY };
                        var chx2 = new Enemy(EnemyType.Chx) { X = baseX + 30, Y = baseY, VelocityY = velocityY };
                        Enemies.Add(chx1);
                        Enemies.Add(chx2);
                        break;
                    }
                }
            }
        }

        // Spawn new enemies
        _enemySpawnTimer += deltaTime;
        if (_enemySpawnTimer >= ENEMY_SPAWN_INTERVAL)
        {
            _enemySpawnTimer = 0;
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        var enemyType = _random.Next(2) == 0 ? EnemyType.Chx : EnemyType.Chauve;
        var enemy = new Enemy(enemyType)
        {
            X = _random.Next(0, (int)(GameWidth - 60)),
            Y = -60 // Start above screen
        };
        Enemies.Add(enemy);
    }

    public void FireLaser()
    {
        var laser = Player.FireLaser();
        Lasers.Add(laser);
    }

    public void MovePlayerLeft(float deltaTime)
    {
        Player.MoveLeft(deltaTime);
    }

    public void MovePlayerRight(float deltaTime)
    {
        Player.MoveRight(deltaTime);
    }

    public void MovePlayerUp(float deltaTime)
    {
        Player.MoveUp(deltaTime);
    }

    public void MovePlayerDown(float deltaTime)
    {
        Player.MoveDown(deltaTime);
    }

    public void StopPlayer()
    {
        Player.StopMoving();
    }

    public void ResetGame()
    {
        GameOver = false;
        Score = 0;
        Player.X = GameWidth / 2 - 40;
        Player.Y = GameHeight - 100;
        Player.StopMoving();
        Enemies.Clear();
        Lasers.Clear();
        _enemySpawnTimer = 0;
        _backgroundTimer = 0;
        CurrentBackground = "skycloud.png";
    }
}
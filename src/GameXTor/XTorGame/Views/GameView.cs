using XTorGame.GameEngine;
using IImage = Microsoft.Maui.Graphics.IImage;

namespace XTorGame.Views;

public class GameView : GraphicsView
{
    private XTorGameEngine _gameEngine;
    private Dictionary<string, IImage> _imageCache = [];
    private DateTime _lastUpdate = DateTime.Now;

    public GameView()
    {
        Drawable = new GameDrawable(this);
        StartInteraction += OnStartInteraction;
        EndInteraction += OnEndInteraction;

        // Start game loop
        Dispatcher.StartTimer(TimeSpan.FromMilliseconds(16), GameLoop); // ~60 FPS
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        if (width > 0 && height > 0)
        {
            _gameEngine = new XTorGameEngine((float)width, (float)height);
            LoadImages();
        }
    }

    private async void LoadImages()
    {
        try
        {
            var imageNames = new[] { "superman.png", "chx.png", "chauve.png", "skycloud.png", "skycloud2.jpg" };

            foreach (var imageName in imageNames)
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync(imageName);
#if WINDOWS
                var image = Microsoft.Maui.Graphics.Platform.PlatformImage.FromStream(stream);
#elif ANDROID
                var image = Microsoft.Maui.Graphics.Platform.PlatformImage.FromStream(stream);
#elif IOS || MACCATALYST
                var image = Microsoft.Maui.Graphics.Platform.PlatformImage.FromStream(stream);
#endif
                _imageCache[imageName] = image;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading images: {ex.Message}");
        }
    }

    private bool GameLoop()
    {
        if (_gameEngine != null)
        {
            var now = DateTime.Now;
            var deltaTime = (float)(now - _lastUpdate).TotalSeconds;
            _lastUpdate = now;

            _gameEngine.Update(deltaTime);
            Invalidate(); // Trigger redraw
        }

        return true; // Continue timer
    }

    private void OnStartInteraction(object? sender, TouchEventArgs e)
    {
        // Handle touch/click for firing lasers
        _gameEngine?.FireLaser();
    }

    private void OnEndInteraction(object? sender, TouchEventArgs e)
    {
        // Stop player movement when touch ends
        _gameEngine?.StopPlayer();
    }

    public XTorGameEngine? GameEngine => _gameEngine;
    public Dictionary<string, IImage> ImageCache => _imageCache;

    private class GameDrawable(GameView gameView) : IDrawable
    {
        private readonly GameView _gameView = gameView;

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            var gameEngine = _gameView.GameEngine;
            if (gameEngine == null) return;

            // Clear canvas
            canvas.FillColor = Colors.Black;
            canvas.FillRectangle(dirtyRect);

            // Draw background
            if (_gameView.ImageCache.TryGetValue(gameEngine.CurrentBackground, out var backgroundImage))
            {
                canvas.DrawImage(backgroundImage, 0, 0, dirtyRect.Width, dirtyRect.Height);
            }

            // Draw player
            if (_gameView.ImageCache.TryGetValue(gameEngine.Player.ImageSource, out var playerImage))
            {
                canvas.DrawImage(playerImage,
                    gameEngine.Player.X,
                    gameEngine.Player.Y,
                    gameEngine.Player.Width,
                    gameEngine.Player.Height);
            }

            // Draw enemies
            foreach (var enemy in gameEngine.Enemies)
            {
                if (_gameView.ImageCache.TryGetValue(enemy.ImageSource, out var enemyImage))
                {
                    canvas.DrawImage(enemyImage,
                        enemy.X,
                        enemy.Y,
                        enemy.Width,
                        enemy.Height);
                }
            }

            // Draw lasers
            canvas.FillColor = Color.FromArgb("#00ef81");
            foreach (var laser in gameEngine.Lasers)
            {
                canvas.FillRectangle(laser.X, laser.Y, laser.Width, laser.Height);
            }

            // Draw UI
            canvas.FontSize = 20;
            canvas.FontColor = Colors.White;
            canvas.DrawString($"Score: {gameEngine.Score}", 10, 30, HorizontalAlignment.Left);

            if (gameEngine.GameOver)
            {
                canvas.FontSize = 48;
                canvas.FontColor = Colors.Red;
                var gameOverText = "GAME OVER";
                var centerX = dirtyRect.Width / 2;
                var centerY = dirtyRect.Height / 2;

                // Draw GAME OVER centered
                var gameOverSize = canvas.GetStringSize(gameOverText, null, 48);
                canvas.DrawString(gameOverText,
                    centerX - gameOverSize.Width / 2,
                    centerY - gameOverSize.Height / 2,
                    HorizontalAlignment.Left);

                // Draw Tap to restart centered below
                canvas.FontSize = 20;
                canvas.FontColor = Colors.White;
                var restartText = "Tap to restart";
                var restartSize = canvas.GetStringSize(restartText, null, 20);
                canvas.DrawString(restartText,
                    centerX - restartSize.Width / 2,
                    centerY + gameOverSize.Height / 2 + 16, // 16px below GAME OVER
                    HorizontalAlignment.Left);
            }
        }
    }
}
using XTorGame.Views;

namespace XTorGame;

public partial class MainPage : ContentPage
{
    private bool _isMovingLeft = false;
    private bool _isMovingRight = false;
    private bool _isMovingUp = false;
    private bool _isMovingDown = false;

    public MainPage()
    {
        InitializeComponent();

        // Start input handling timer
        Dispatcher.StartTimer(TimeSpan.FromMilliseconds(16), HandleInput);
    }

    private bool HandleInput()
    {
        var gameView = this.FindByName<GameView>("GameView");
        if (gameView?.GameEngine == null)
            return true;

        var deltaTime = 0.016f; // ~60fps

        // Handle continuous movement
        if (_isMovingLeft)
            gameView.GameEngine.MovePlayerLeft(deltaTime);
        else if (_isMovingRight)
            gameView.GameEngine.MovePlayerRight(deltaTime);
        else if (_isMovingUp)
            gameView.GameEngine.MovePlayerUp(deltaTime);
        else if (_isMovingDown)
            gameView.GameEngine.MovePlayerDown(deltaTime);
        else
            gameView.GameEngine.StopPlayer();

        return true; // Continue timer
    }

    private void OnLeftPressed(object sender, EventArgs e)
    {
        _isMovingLeft = true;
        _isMovingRight = false;
        _isMovingUp = false;
        _isMovingDown = false;
    }

    private void OnRightPressed(object sender, EventArgs e)
    {
        _isMovingRight = true;
        _isMovingLeft = false;
        _isMovingUp = false;
        _isMovingDown = false;
    }

    private void OnUpPressed(object sender, EventArgs e)
    {
        _isMovingUp = true;
        _isMovingDown = false;
        _isMovingLeft = false;
        _isMovingRight = false;
    }

    private void OnDownPressed(object sender, EventArgs e)
    {
        _isMovingDown = true;
        _isMovingUp = false;
        _isMovingLeft = false;
        _isMovingRight = false;
    }

    private void OnMoveReleased(object sender, EventArgs e)
    {
        _isMovingLeft = false;
        _isMovingRight = false;
        _isMovingUp = false;
        _isMovingDown = false;
    }

    private void OnFireClicked(object sender, EventArgs e)
    {
        var gameView = this.FindByName<GameView>("GameView");
        gameView?.GameEngine?.FireLaser();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Handle game restart when tapping after game over
        var gameView = this.FindByName<GameView>("GameView");
        if (gameView != null)
        {
            gameView.StartInteraction += OnGameViewTapped;
        }
    }

    private void OnGameViewTapped(object sender, TouchEventArgs e)
    {
        var gameView = this.FindByName<GameView>("GameView");
        if (gameView?.GameEngine?.GameOver == true)
        {
            gameView.GameEngine.ResetGame();
        }
    }
}
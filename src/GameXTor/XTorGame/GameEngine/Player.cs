namespace XTorGame.GameEngine;

public class Player : GameObject
{
    public Player()
    {
        Width = 80;
        Height = 80;
        ImageSource = "superman.png";
    }

    public void MoveLeft(float deltaTime)
    {
        VelocityX = -300; // pixels per second
    }

    public void MoveRight(float deltaTime)
    {
        VelocityX = 300;
    }

    public void MoveUp(float deltaTime)
    {
        VelocityY = -300;
    }

    public void MoveDown(float deltaTime)
    {
        VelocityY = 300;
    }

    public void StopMoving()
    {
        VelocityX = 0;
        VelocityY = 0;
    }

    public Laser FireLaser()
    {
        return new Laser
        {
            X = X + Width / 2 - 2,
            Y = Y - 10,
            VelocityY = -500 // Move upward
        };
    }
}
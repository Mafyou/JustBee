namespace XTorGame.GameEngine;

public class Laser : GameObject
{
    public Laser()
    {
        Width = 4;
        Height = 20;
        ImageSource = ""; // We'll draw this as a green rectangle
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        
        // Remove laser if it goes off screen
        if (Y < -Height)
        {
            IsActive = false;
        }
    }
}
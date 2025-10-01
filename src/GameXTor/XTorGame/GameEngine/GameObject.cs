namespace XTorGame.GameEngine;

public class GameObject
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }
    public float VelocityX { get; set; }
    public float VelocityY { get; set; }
    public bool IsActive { get; set; } = true;
    public string ImageSource { get; set; } = string.Empty;

    public virtual void Update(float deltaTime)
    {
        X += VelocityX * deltaTime;
        Y += VelocityY * deltaTime;
    }

    public virtual bool CollidesWith(GameObject other)
    {
        return X < other.X + other.Width &&
               X + Width > other.X &&
               Y < other.Y + other.Height &&
               Y + Height > other.Y;
    }
}
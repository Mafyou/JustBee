namespace XTorGame.GameEngine;

public class Enemy : GameObject
{
    public EnemyType Type { get; set; }

    public Enemy(EnemyType type)
    {
        Type = type;
        Width = 60;
        Height = 60;
        VelocityY = 100; // Move down slowly
        
        switch (type)
        {
            case EnemyType.Chx:
                ImageSource = "chx.png";
                break;
            case EnemyType.Chauve:
                ImageSource = "chauve.png";
                break;
        }
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
    }

    public bool ShouldMultiply { get; set; } // Will not be set for Chauve anymore

    public Enemy CreateCopy()
    {
        return new Enemy(Type)
        {
            X = X + Width + 10, // Spawn next to original
            Y = Y,
            VelocityY = VelocityY
        };
    }
}

public enum EnemyType
{
    Chx,
    Chauve
}
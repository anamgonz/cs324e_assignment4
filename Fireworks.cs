using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace assignment_6_fireworks;

public class Firework
{
    // properties 
    private Vector2 position;
    private Vector2 velocity;
    private Vector2 acceleration;
    private Vector2 origin;
    

    private Texture2D _sprite;
    
    public  Firework(Texture2D sprite, float x, float y)
    {
        _sprite = sprite;
        origin = new Vector2(x, y);
        position = new Vector2(x, y);
        velocity = new Vector2(0, (float)(Random.Shared.NextDouble() - 2f));
    }

    public void Ignite(float fx, float fy, int screenHeight)
    {
        acceleration = new Vector2(fx, fy);
        velocity += acceleration;
        position += velocity;
        
        if (position.Y  > screenHeight + 200)
        {
            // reset position within screen
            position = origin;
            velocity.X = 0;
            velocity.Y = (float)(Random.Shared.NextDouble() - 2f);
        }
    }
    

    public void Display(SpriteBatch spriteBatch)
    {
        
        spriteBatch.Draw(
            _sprite, 
            position, 
            null, 
            Color.White,
            0.0f,
            Vector2.Zero,
            0.2f,
            SpriteEffects.None,
            0.0f);
        
    }
    
    

}
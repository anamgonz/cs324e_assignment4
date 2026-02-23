using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Assignment4_ana;

public class Starfish
{
    private Texture2D starCore;      // my starfish body
    private Texture2D starfish;      // mmy starfish arms
    private Vector2 origin;       // where am I rotating around
    private Vector2 position;       // my position on the screen
    private float rotation = 0f;         // my rotation for the star movement
    private float scale = 0.25f;          // how big my star is(variety)
    private float armWiggle = 0f;        // for my second level transform
    private float armAmplitude = 0.2f;           //wiggle angle
    
    public Starfish(Texture2D body, Texture2D arms, Vector2 start)
    {
        starCore = body;
        starfish = arms;
        position = start;
        origin = new Vector2(starCore.Width / 2, starCore.Height / 2);

    }

    public void Update(GameTime gameTime)
    {
        // update rotation angle
        rotation += 0.01f;
        
        // update the wiggle
        armWiggle = MathF.Sin((float)gameTime.TotalGameTime.TotalSeconds) * armAmplitude;
        
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        // update the body transformation of a spin in-place
        Matrix Scale = Matrix.CreateScale(scale);
        Matrix bodyspin = Matrix.CreateRotationZ(rotation);
        Matrix movetoOrigin = Matrix.CreateTranslation(-origin.X, -origin.Y, 0f);
        Matrix moveback = Matrix.CreateTranslation(origin.X, origin.Y, 0f);
        Matrix bodyTranslation = Matrix.CreateTranslation(position.X, position.Y, 0f);
        // Rotate --> Translate
        Matrix bodyTransform =  movetoOrigin *  Scale * bodyspin * moveback * bodyTranslation;
        
            
        // update the body transformation of an arm wiggle
        //Matrix Scale = Matrix.CreateScale(scale);
        Matrix arm2center = Matrix.CreateTranslation(-origin.X, -origin.Y, 0f);
        Matrix rotateArm = Matrix.CreateRotationZ(armWiggle);
        Matrix armfromcenter = Matrix.CreateTranslation(origin.X, origin.Y, 0f);
        
        
        // all together now: my arm offset --> rotate arm with body --> rotate body
        Matrix armTransform = arm2center * rotateArm * armfromcenter * bodyTransform;
        
        //draw the starfish
        spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, bodyTransform);
        spriteBatch.Draw(
            starCore, 
            Vector2.Zero,
            Color.White);
        spriteBatch.End();
        
        //draw the arms 
        spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, armTransform);
        spriteBatch.Draw(
            starfish,
            Vector2.Zero, 
            Color.White);
        spriteBatch.End();
    }

}

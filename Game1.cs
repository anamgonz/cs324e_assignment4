using System;
using System.Net.Http;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace assignment_6_fireworks;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Texture2D spark;
    private Firework[] fireworks1;
    private Random random = new Random();
    
    private Firework[] fireworks2;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        spark = Content.Load<Texture2D>("spark3"); 
        fireworks1 = new Firework[40];
        fireworks2 = new Firework[40];

        for (int i = 0; i < fireworks1.Length; i++)
        {
            fireworks1[i] = new Firework(spark, 500, 80);
            fireworks2[i] = new Firework(spark, 150, 125);
        }
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        for (int i = 0; i < fireworks1.Length; i++)
        {
            // expansion force for each spark
            float fx = (float)(random.NextDouble() * 0.4 - 0.2f);
            
            //gravity
            float fy = 0.098f;
            int screenHeight = Window.ClientBounds.Height;
            fireworks1[i].Ignite(fx, fy, screenHeight );
            fireworks2[i].Ignite(fx, fy, screenHeight );
        }
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        
        _spriteBatch.Begin();
        
        for (int i = 0; i < fireworks1.Length; i++)
        {
            fireworks1[i].Display(_spriteBatch);
            fireworks2[i].Display(_spriteBatch);
        }
        
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace particle_simulation_rocket;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // Rocket
    private Texture2D _rocketTex;
    private Vector2   _rocketPos;
    private float     _rocketAngle = 0f; // radians; 0 = pointing up
    private const float RocketSpeed = 200f;

    // Smoke
    private SmokeEmitter _smokeEmitter;
    
    // Fireworks
    private Texture2D spark;
    private Firework[] fireworks1;
    private Firework[] fireworks2;
    private Random random = new Random();

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth  = 800;
        _graphics.PreferredBackBufferHeight = 600;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _rocketPos = new Vector2(
            _graphics.PreferredBackBufferWidth / 2f,
            _graphics.PreferredBackBufferHeight * 0.65f);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _rocketTex   = CreateRocketTexture();
        _smokeEmitter = new SmokeEmitter(GraphicsDevice);
        
        spark = Content.Load<Texture2D>("spark3"); 
        fireworks1 = new Firework[40];
        fireworks2 = new Firework[40];

        // same length for both fireworks
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

        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        var   kb = Keyboard.GetState();

        var movement = Vector2.Zero;
        if (kb.IsKeyDown(Keys.Left))  movement.X -= 1;
        if (kb.IsKeyDown(Keys.Right)) movement.X += 1;
        if (kb.IsKeyDown(Keys.Up))    movement.Y -= 1;
        if (kb.IsKeyDown(Keys.Down))  movement.Y += 1;

        if (movement != Vector2.Zero)
        {
            movement.Normalize();
            _rocketPos += movement * RocketSpeed * dt;
            
            _rocketAngle = (float)Math.Atan2(movement.Y, movement.X) + MathHelper.PiOver2;
        }

        // Clamp to screen bounds
        _rocketPos.X = MathHelper.Clamp(_rocketPos.X, _rocketTex.Width / 2f,
            _graphics.PreferredBackBufferWidth  - _rocketTex.Width  / 2f);
        _rocketPos.Y = MathHelper.Clamp(_rocketPos.Y, _rocketTex.Height / 2f,
            _graphics.PreferredBackBufferHeight - _rocketTex.Height / 2f);

        // Nozzle trails opposite the facing direction
        float nozzleOffset = _rocketTex.Height / 2f;
        var nozzlePos = _rocketPos + new Vector2(
            (float)Math.Sin(_rocketAngle),
            -(float)Math.Cos(_rocketAngle)) * -nozzleOffset;

        _smokeEmitter.Update(gameTime, nozzlePos, _rocketAngle);
        
        
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
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin(blendState: BlendState.AlphaBlend);
        
        // fireworks 
        for (int i = 0; i < fireworks1.Length; i++)
        {
            fireworks1[i].Display(_spriteBatch);
            fireworks2[i].Display(_spriteBatch);
        }

        // Smoke drawn first so it appears behind the rocket
        _smokeEmitter.Draw(_spriteBatch);

        var origin = new Vector2(_rocketTex.Width / 2f, _rocketTex.Height / 2f);
        _spriteBatch.Draw(_rocketTex, _rocketPos, null, Color.White,
                          _rocketAngle, origin, 1f, SpriteEffects.None, 0f);

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    // Draws a simple rocket shape procedurally — no external asset needed
    private Texture2D CreateRocketTexture()
    {
        const int W = 30, H = 70;
        var tex  = new Texture2D(GraphicsDevice, W, H);
        var data = new Color[W * H];

        for (int y = 0; y < H; y++)
        for (int x = 0; x < W; x++)
        {
            // Nose cone: top 25%, tapers from centre outward
            bool inNose = y < H * 0.25f &&
                          x >= (int)MathHelper.Lerp(W / 2f, 0,  (float)y / (H * 0.25f)) &&
                          x <  (int)MathHelper.Lerp(W / 2f, W,  (float)y / (H * 0.25f));

            // Body: middle 60%, fixed width with a 4px margin each side
            bool inBody = y >= H * 0.25f && y < H * 0.85f && x >= 4 && x < W - 4;

            // Fins: bottom 15%, full width
            bool inFin  = y >= H * 0.85f && (x < 8 || x >= W - 8);

            if      (inNose) data[y * W + x] = new Color(220, 220, 235);
            else if (inBody) data[y * W + x] = new Color(200, 200, 215);
            else if (inFin)  data[y * W + x] = new Color(180, 50, 50);
            else             data[y * W + x] = Color.Transparent;
        }

        tex.SetData(data);
        return tex;
    }
}
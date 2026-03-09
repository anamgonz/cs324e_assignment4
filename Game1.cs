using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Ana_Amaton_Tree_A5;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    private Matrix view;
    private Matrix projection;
    private float aspectRatio;

    private Tree tree1;
    private Tree tree2;
    private Model _tree1;
    private Model _tree2;
    private Model _leaf;
    

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        view = Matrix.CreateLookAt(new Vector3(0.0f, 4000.0f, 2500f), 
            new Vector3(0.0f, 2000.0f, 0.0f),
            Vector3.Up);
        aspectRatio = GraphicsDevice.Viewport.AspectRatio;
        projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f),
            aspectRatio, 1.0f, 10000.0f);
        
        
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _tree1 = Content.Load<Model>("meshes/Tree_1");
        _tree2 = Content.Load<Model>("meshes/tree_obj");
        _leaf  = Content.Load<Model>("meshes/Fall_leaf_OBJ");
        tree1 = new Tree(_tree1, _leaf, new Vector3(-1300.0f, 0.0f, 0.0f));
        tree2 = new Tree(_tree2, _leaf, new Vector3(1800.0f, 0.0f, 0.0f));
        
        
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        tree1.TreeDance(gameTime);
        tree1.UpdateLeaves(gameTime);
        
        tree2.TreeDance(gameTime);
        tree2.UpdateLeaves(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        tree1.draw(view, projection);
        tree2.draw(view, projection);

        base.Draw(gameTime);
    }
    
    
}
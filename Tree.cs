using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Ana_Amaton_Tree_A5;

public class Tree
{
    private Model _tree;
    private Vector3 position;
    private float rotationX;
    private Matrix treeRotation;
    private bool goingRight = true;
    
    // Leaves
    private float t = 0.0f;
    private Model _leaf1;
    private Vector3 pleaf1;
    private Matrix leafTranslation1;
    private bool isFalling1 = true;
    private Vector3 start1;
    private Vector3 end1;
    
    private Model _leaf2;
    private Vector3 pleaf2;
    private Matrix leafTranslation2;
    private bool isFalling2 = true;
    private Vector3 start2;
    private Vector3 end2;
    
    
    private Model _leaf3;
    private Vector3 pleaf3;
    private Matrix leafTranslation3;
    private bool isFalling3 = true;
    private Vector3 start3;
    private Vector3 end3;


    public Tree(Model tree, Model leaf, Vector3 pos)
    {
        _tree = tree;
        position = pos;
        _leaf1 = leaf;
        pleaf1 = new Vector3(pos.X - 100.0f, pos.Y + 2500.0f, pos.Z);
        _leaf2 = leaf;
        pleaf2 = new Vector3(pos.X + 100.0f, pos.Y + 2400.0f, pos.Z);
        _leaf3 = leaf;
        pleaf3 = new Vector3(pos.X + 200.0f, pos.Y + 2300.0f, pos.Z);
        rotationX = 0.1f;
        
        // leaves
        start1 = pleaf1;
        end1 = new Vector3(pleaf1.X, position.Y, pleaf1.Z);
        
        start2 = pleaf2;
        end2 = new Vector3(pleaf2.X, position.Y, pleaf2.Z);
        
        start3 = pleaf3;
        end3 = new Vector3(pleaf3.X, position.Y, pleaf3.Z);

    }

    public void TreeDance(GameTime gameTime)
    {
        // Rotation Logic
        if (goingRight)
        {
            rotationX += gameTime.ElapsedGameTime.Milliseconds * 0.003f;
            if (rotationX > 1.0f)
                goingRight = false;
        }
        else
        {
            rotationX -= gameTime.ElapsedGameTime.Milliseconds * 0.003f;
            if (rotationX < -1.0f)
            {
                goingRight = true;
            }
        }

        treeRotation = Matrix.CreateRotationZ(rotationX);

    }
    
    public void UpdateLeaves(GameTime gameTime)
    {
        // Leaves Translation down and wiggle ;
        t += (float)gameTime.ElapsedGameTime.TotalSeconds * 0.2f;
        t = MathHelper.Clamp(t, 0f, 1f);
        
        pleaf1 = Vector3.Lerp(start1, end1, t);
        
        pleaf2 = Vector3.Lerp(start2, end2, t);
        
        
        
        pleaf3 = Vector3.Lerp(start3, end3, t);
        
        
        // Position the leaf
        leafTranslation1 = Matrix.CreateTranslation(pleaf1);
        leafTranslation2 = Matrix.CreateTranslation(pleaf2);
        leafTranslation3 = Matrix.CreateTranslation(pleaf3);

        // reset
        if (t >= 1f)
        {
            t = 0f;

            pleaf1 = start1;
            pleaf2 = start2;
            pleaf3 = start3;
        }
    }

    public void DrawMesh(Model m, Matrix movement, Matrix view, Matrix projection, Color color)
    {
        Matrix[] transforms = new Matrix[m.Bones.Count];
        m.CopyAbsoluteBoneTransformsTo(transforms);

        foreach (ModelMesh mesh in m.Meshes)
        {
            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.EnableDefaultLighting();

                effect.View = view;
                effect.Projection = projection;
                effect.World = movement * transforms[mesh.ParentBone.Index];

                effect.DiffuseColor = color.ToVector3();

            }
            mesh.Draw();
        }
    }

    public void draw(Matrix view, Matrix projection)
    {
        // Tree
        Matrix treeMovement =  Matrix.CreateRotationX(MathHelper.ToRadians(-65)) * treeRotation * Matrix.CreateTranslation(position);
        DrawMesh(_tree, treeMovement, view, projection, Color.SaddleBrown );
        
        // Leaves
        DrawMesh(_leaf1,   treeRotation * leafTranslation1, view, projection, Color.Orange);
        DrawMesh(_leaf2, treeRotation * leafTranslation2, view, projection, Color.Yellow);
        DrawMesh(_leaf3,  treeRotation * leafTranslation3, view, projection, Color.Red);
    }
    
}
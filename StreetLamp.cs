using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Assignment5new
{
    public class StreetLamp
    {
        private Model model;
        private float lerpTimer = 0f;
        private float swingAngle = 0f;
        private Vector3 lerpedColor;

        public StreetLamp(Model model)
        {
            this.model = model;
        }

        public void Update(GameTime gameTime)
        {
            // track time to keep the animation moving
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            lerpTimer += dt;

            // create a smooth looping value between 0 and 1
            float lerpPercent = (MathF.Sin(lerpTimer) + 1f) / 2f; 
            
            // use the lerp function to smoothly change the light color
            lerpedColor = Vector3.Lerp(new Vector3(0.5f), new Vector3(0.8f), lerpPercent);

            // calculate the back and forth swinging motion
            swingAngle = MathF.Sin(lerpTimer * 2.0f) * 0.15f; 
        }

        public void Draw(Matrix view, Matrix projection)
        {
            // level one transform to turn the model right side up
            Matrix baseTransform = Matrix.CreateRotationX(MathHelper.Pi);

            // level two transform to make the child part swing
            Matrix swingTransform = Matrix.CreateRotationZ(swingAngle);

            for (int i = 0; i < model.Meshes.Count; i++)
            {
                ModelMesh mesh = model.Meshes[i];

                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.LightingEnabled = true;
                    
                    // set the light color using the lerp result
                    effect.AmbientLightColor = lerpedColor; 
                    
                    // combine the swing and the base flip to show hierarchical grouping
                    if (i > -100)
                    {
                        effect.World = swingTransform * baseTransform;
                    }
                    else
                    {
                        effect.World = baseTransform;
                    }

                    effect.View = view;
                    effect.Projection = projection;
                }
                mesh.Draw();
            }
        }
    }
}
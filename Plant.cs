using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AquariumAnimation
{
    public class Plant
    {
        private Vector2 basePosition;           // Where the plant is rooted (bottom)
        private float totalHeight;              // Total height of the plants
        private float segmentWidth;             // Width of each segment
        
        private float swayAngle;                // Current sway angle for whole plant
        private float swaySpeed;                // Speed of swaying animation
        private float swayAmount;               // Maximum sway angle in radians
        private float animationOffset;          // Phase offset for varied timing
        
        private int numSegments;                // Number of segments in the plant
        private float[] segmentAngles;          // Current rotation of each segment
        private float segmentHeight;            // Height of each individual segment
        
        private Color plantColor;               // Color of the plant
        private Texture2D pixelTexture;         // 1x1 white texture for drawing rectangles
        
        private Matrix rootTransform;           // Level 1: Root transformation matrix

        public Plant(Vector2 position, Color color, int segments, float height, 
                       float width, float speed, float offset, GraphicsDevice graphicsDevice)
        {
            basePosition = position;
            plantColor = color;
            numSegments = segments;
            totalHeight = height;
            segmentWidth = width;
            swaySpeed = speed;
            animationOffset = offset;
            
            segmentHeight = totalHeight / numSegments;
            swayAmount = 0.15f;
            
            segmentAngles = new float[numSegments];
            
            pixelTexture = new Texture2D(graphicsDevice, 1, 1);
            pixelTexture.SetData(new[] { Color.White });
            
            rootTransform = Matrix.Identity;
        }

        public void Update(GameTime gameTime)
        {
            float time = (float)gameTime.TotalGameTime.TotalSeconds;
            
            swayAngle = (float)Math.Sin(time * swaySpeed + animationOffset) * swayAmount;
            

            rootTransform = Matrix.CreateRotationZ(swayAngle) * 
                           Matrix.CreateTranslation(basePosition.X, basePosition.Y, 0);
            

            for (int i = 0; i < numSegments; i++)
            {

                float phase = time * swaySpeed * 1.5f + i * 0.4f + animationOffset;
                segmentAngles[i] = (float)Math.Sin(phase) * 0.1f * (i + 1) / numSegments;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            
            Vector2 currentPosition = Vector2.Zero;
            float cumulativeRotation = 0f;
            
            for (int i = 0; i < numSegments; i++)
            {
                cumulativeRotation += segmentAngles[i];
                

                Matrix segmentTransform = 
                    Matrix.CreateTranslation(-segmentWidth / 2, 0, 0) *  // Center the segment
                    Matrix.CreateRotationZ(cumulativeRotation) *          // Apply rotation
                    Matrix.CreateTranslation(currentPosition.X, currentPosition.Y, 0) * // Position
                    rootTransform;                                         // Apply root transform
                
                Vector2 drawPosition = new Vector2(segmentTransform.M41, segmentTransform.M42);
                float drawRotation = (float)Math.Atan2(segmentTransform.M21, segmentTransform.M11);
                
                Color segmentColor = Color.Lerp(plantColor, 
                    new Color(plantColor.R / 2, plantColor.G / 2, plantColor.B / 2), 
                    i / (float)numSegments * 0.3f);
                
                spriteBatch.Draw(
                    pixelTexture,
                    drawPosition,
                    null,
                    segmentColor,
                    drawRotation,
                    Vector2.Zero,
                    new Vector2(segmentWidth, segmentHeight),
                    SpriteEffects.None,
                    0f
                );
                
                currentPosition.X += (float)Math.Sin(cumulativeRotation) * segmentHeight;
                currentPosition.Y -= (float)Math.Cos(cumulativeRotation) * segmentHeight;
            }
        }
    }
}

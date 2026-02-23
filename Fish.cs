// dt27238 daniela Taborda

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace aquarium_fish
{
    public class Fish
    {
        // Textures for each part of the fish
        private Texture2D bodyTexture;
        private Texture2D tailTexture;
        private Texture2D topFinTexture;
        private Texture2D bottomFinTexture;

        // Movement settings
        private Vector2 position;
        private float speed;
        private float scale;
        private float direction;

        // Animation values
        private float tailRotation;
        private float finRotation;
        private float tailWiggleSpeed;
        private float finWiggleSpeed;
        private float animationOffset;

        // Root transform for scaling and positioning
        private Matrix rootTransform;

        public Fish(Texture2D body, Texture2D tail, Texture2D topFin, Texture2D bottomFin,
                    Vector2 startPos, float moveSpeed, float scale,
                    float tailSpeed, float finSpeed, float offset)
        {
            bodyTexture = body;
            tailTexture = tail;
            topFinTexture = topFin;
            bottomFinTexture = bottomFin;

            position = startPos;
            speed = moveSpeed;
            this.scale = scale;

            // Fish swims left because the head faces left
            direction = -1f;

            tailWiggleSpeed = tailSpeed;
            finWiggleSpeed = finSpeed;
            animationOffset = offset;

            rootTransform = Matrix.Identity;
        }

        public void Update(GameTime gameTime)
        {
            float time = (float)gameTime.TotalGameTime.TotalSeconds;

            // Move the fish horizontally
            position.X += speed * direction * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Wrap the fish around the screen
            if (position.X < -150) position.X = 900;
            if (position.X > 900) position.X = -100;

            // Apply scale and position
            rootTransform =
                Matrix.CreateScale(scale) *
                Matrix.CreateTranslation(position.X, position.Y, 0);

            // Update tail and fin rotation
            tailRotation = (float)Math.Sin(time * tailWiggleSpeed + animationOffset) * 0.12f;
            finRotation  = (float)Math.Sin(time * finWiggleSpeed  + animationOffset) * 0.06f;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Tail position
            Vector2 tailOffset = new Vector2(1, 0);

            spriteBatch.Draw(
                tailTexture,
                position + tailOffset * scale,
                null,
                Color.White,
                tailRotation,
                new Vector2(tailTexture.Width / 2f, tailTexture.Height / 2f),
                scale,
                SpriteEffects.None,
                0f
            );

            // Top fin position
            Vector2 topFinOffset = new Vector2(0, 9);

            spriteBatch.Draw(
                topFinTexture,
                position + topFinOffset * scale,
                null,
                Color.White,
                finRotation,
                new Vector2(topFinTexture.Width / 2f, topFinTexture.Height / 2f),
                scale,
                SpriteEffects.None,
                0f
            );

            // Bottom fin position
            Vector2 bottomFinOffset = new Vector2(0, -10);

            spriteBatch.Draw(
                bottomFinTexture,
                position + bottomFinOffset * scale,
                null,
                Color.White,
                -finRotation,
                new Vector2(bottomFinTexture.Width / 2f, bottomFinTexture.Height / 2f),
                scale,
                SpriteEffects.None,
                0f
            );

            // Body position
            spriteBatch.Draw(
                bodyTexture,
                position,
                null,
                Color.White,
                0f,
                new Vector2(bodyTexture.Width / 2f, bodyTexture.Height / 2f),
                scale,
                SpriteEffects.None,
                0f
            );
        }
    }
}

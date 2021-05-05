using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MacApp05Game.Models
{
    /// <summary>
    /// A bullet sprite used whenever the player fires their
    /// weapon in the game by holding down the space bar. 
    /// </summary>
    public class Bullet : Sprite
    {
        internal AnimatedPlayer Parent;
        private float RotationVelocity;
        private float LinearVelocity;

        public float Timer { get; set; }

        public float LifeSpan { get; set; } = 2.0f;

        public Bullet(Texture2D texture) : base(texture, 0, 0) { }

        public override void Update(GameTime gameTime)
        {
            RotationVelocity = 3f;
            LinearVelocity = 4f;

            Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Timer > LifeSpan)
            {
                IsVisible = false;
                IsActive = false;
                IsAlive = false;
            }
            else
            {
                Position += Direction * LinearVelocity;
            }
        }
    }
}
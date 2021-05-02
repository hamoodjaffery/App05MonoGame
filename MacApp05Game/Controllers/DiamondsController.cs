using MacApp05Game.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace MacApp05Game.Controllers
{
    public enum DiamondColours
    {
        blue = 100,
        red = 200,
        violet = 500
    }

    /// <summary>
    /// This class creates a list of coins which
    /// can be updated and drawn and checked for
    /// collisions with the player sprite
    /// </summary>
    /// <authors>
    /// Derek Peacock & Andrei Cruceru
    /// </authors>
    public class DiamondsController
    {
        private SoundEffect diamondEffect;

        private readonly List<AnimatedSprite> Diamonds;

        public DiamondsController()
        {
            Diamonds = new List<AnimatedSprite>();
        }
        /// <summary>
        /// Create an animated sprite of a copper coin
        /// which could be collected by the player for a score
        /// </summary>
        public void CreateDiamond(GraphicsDevice graphics, Texture2D Sheet)
        {
            diamondEffect = SoundController.GetSoundEffect("Diamondpickup");
            Animation animation = new Animation("diamond", diamondSheet, 8);

            AnimatedSprite diamond = new AnimatedSprite()
            {
                Animation = animation,
                Image = animation.SetMainFrame(graphics),
                Scale = 2.0f,
                Position = new Vector2(600, 100),
                Speed = 0,
            };

            Diamonds.Add(diamond);
        }

        public void HasCollided(AnimatedPlayer player)
        {
            foreach (AnimatedSprite diamond in Diamonds)
            {
                if (diamond.HasCollided(player) && diamond.IsAlive)
                {
                    diamondEffect.Play();

                    diamond.IsActive = false;
                    diamond.IsAlive = false;
                    diamond.IsVisible = false;
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (AnimatedSprite diamond in Diamonds)
            {
                diamond.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (AnimatedSprite diamond in Diamonds)
            {
                diamond.Draw(spriteBatch);
            }
        }
    }
}

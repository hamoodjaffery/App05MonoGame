using MacApp05Game.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
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
        public const float MaxTime = 3.0f;

        private Random generator = new Random();

        private SoundEffect diamondEffect;

        private readonly List<Sprite> Diamonds;

        public Texture2D redDiamond { get; set; }
        public Texture2D blueDiamond { get; set; }
        public Texture2D violetDiamond { get; set; }

        private Texture2D diamondImage;

        private ContentManager content;

        private double timer;

        public DiamondsController()
        {
            Diamonds = new List<Sprite>();
        }

        /// <summary>
        /// Create an animated sprite of a copper coin
        /// which could be collected by the player for a score
        /// </summary>
        public void CreateDiamonds(GraphicsDevice graphics, ContentManager content)
        {
            //diamondEffect = SoundController.GetSoundEffect("Diamondpickup");

            this.content = content;

            CreateDiamond(DiamondColours.red);
            CreateDiamond(DiamondColours.blue);
            CreateDiamond(DiamondColours.violet);

        }

        private void CreateDiamond(DiamondColours colour)
        {
            if(colour == DiamondColours.red)
            {
                diamondImage = content.Load<Texture2D>("images/diamond red");
            } 
            else if (colour == DiamondColours.blue)
            {
                diamondImage = content.Load<Texture2D>("images/diamond blue");
            }
            else if (colour == DiamondColours.violet)
            {
                diamondImage = content.Load<Texture2D>("images/diamond violet");
            }

            int x = generator.Next(1000) + 100;
            int y = generator.Next(520) + 100;

            Sprite sprite = new Sprite(diamondImage, x, y);
            sprite.Scale = 0.05f;

            Diamonds.Add(sprite);
        }

        public void HasCollided(AnimatedPlayer player)
        {
            foreach (Sprite diamond in Diamonds)
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

        /// <summary>
        /// When the timer has reached zero from say 2 seconds
        /// then create new diamond of random colour.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            // decrease timer by gametime
            // when = 0 call create diamond

            foreach (Sprite diamond in Diamonds)
            {
                diamond.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Sprite diamond in Diamonds)
            {
                diamond.Draw(spriteBatch);
            }
        }
    }
}

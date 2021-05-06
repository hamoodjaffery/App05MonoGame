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
    /// This class creates three different colour diamond
    /// which can be updated and drawn and checked for
    /// collisions with the player sprite
    /// </summary>
    /// <author>
    /// Hamood Jaffery
    /// </author>
    public class DiamondsController
    {
        public const float MaxTime = 3.0f;
        public const float TimeofCollision = 3f;

        private Random generator = new Random();

        private SoundEffect diamondEffect;

        private readonly List<Sprite> Diamonds;

        public Texture2D redDiamond { get; set; }
        public Texture2D blueDiamond { get; set; }
        public Texture2D violetDiamond { get; set; }

        private Texture2D diamondImage;

        private ContentManager content;

        public DiamondsController()
        {
            Diamonds = new List<Sprite>();
        }

        /// <summary>
        /// Create a list of diamonds and store the diamond images
        /// </summary>
        public void CreateDiamonds(ContentManager content)
        {
            //diamondEffect = SoundController.GetSoundEffect("Diamond");

            this.content = content;

            CreateDiamond(DiamondColours.red);
            CreateDiamond(DiamondColours.blue);
            CreateDiamond(DiamondColours.violet);

        }

        /// <summary>
        /// Generate three different colour diamonds at random positions
        /// </summary>
        /// <param name="colour"></param>
        private void CreateDiamond(DiamondColours colour)
        {
            if(colour == DiamondColours.red)
            {
                diamondImage = content.Load<Texture2D>("images/diamond red");
            } 
            else if (colour == DiamondColours.blue)
            {
                diamondImage = content.Load<Texture2D>("images/diamond violet");
            }
            else if (colour == DiamondColours.violet)
            {
                diamondImage = content.Load<Texture2D>("images/diamond blue");
            }

            int x = generator.Next(1000) + 100;
            int y = generator.Next(520) + 100;

            Sprite sprite = new Sprite(diamondImage, x, y);
            sprite.Scale = 0.9f;

            Diamonds.Add(sprite);
        }

        /// <summary>
        /// This method checks if the diamond is picked up/collided
        /// by the player.
        /// </summary>
        /// <param name="player"></param>
        public void HasCollided(AnimatedPlayer player)
        {
            foreach (Sprite diamond in Diamonds)
            {
                if (diamond.HasCollided(player) && diamond.IsAlive)
                {
                    //diamondEffect.Play();

                    diamond.IsActive = false;
                    diamond.IsAlive = false;
                    diamond.IsVisible = false;

                    player.health += 10;

                    if(player.health >= 100)
                    {
                        player.health = 100;
                    }
                }
            }
        }

       /// <summary>
       /// This method creates an array of diamonds to delete that will be
       /// picked up by the player and then empty that array.
       /// It will also regenerate the diamonds at random position
       /// once the player has picked up
       /// the available diamonds
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

            Diamonds.RemoveAll(x => !x.IsAlive);

            if (Diamonds.Count == 0)
            {
                CreateDiamonds(content);
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

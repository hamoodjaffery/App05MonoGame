using System;
using System.Collections.Generic;
using System.Text;
using MacApp05Game.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace MacApp05Game.Controllers
{
    /// <summary>
    /// This class creates a list of bullets which will be fired
    /// when the player presses down on the space bar in the game.
    /// </summary>
    public class BulletController
    {
        public SoundEffect killEffect { get; set; }

        public Texture2D BulletTexture { get; set; }

        public List<Bullet> Bullets { get; set; }

        /// <summary>
        /// Create a list of bullets and store the bullet image
        /// </summary>
        public BulletController(Texture2D bulletTexture)
        {
            Bullets = new List<Bullet>();
            this.BulletTexture = bulletTexture;
        }

        public void UpdateBullets(GameTime gameTime)
        {
            foreach (Bullet bullet in Bullets)
            {
                if(bullet.IsActive)
                    bullet.Update(gameTime);
            }

            Bullets.RemoveAll(x => !x.IsAlive);
        }

        public void DrawBullets(SpriteBatch spriteBatch)
        {
            foreach (Bullet bullet in Bullets)
            {
                if(bullet.IsVisible)
                    bullet.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Clones the bullet when the player fires their
        /// weapon by holding down the space bar. 
        /// </summary>
        public void AddBullet(AnimatedPlayer player)
        {
            Bullet bullet = new Bullet(BulletTexture);

            bullet.Direction = player.Direction;

            bullet.Position = new Vector2(
                player.Position.X + 60, player.Position.Y + 40);

            bullet.Parent = player;

            Bullets.Add(bullet);
        }

        /// <summary>
        /// Checks if a fired bullet has hit an enemy and
        /// if it has, the enemy will be killed and
        /// removed from the screen. The bullet will
        /// also disappear.
        /// </summary>
        public void HasCollided(AnimatedSprite enemy)
        {
            foreach (Bullet bullet in Bullets)
            {
                if (bullet.IsActive && bullet.HasCollided(enemy))
                {
                    if (killEffect != null)
                        killEffect.Play();

                    enemy.IsActive = false;
                    enemy.IsAlive = false;
                    enemy.IsVisible = false;

                    bullet.IsVisible = false;
                    bullet.IsActive = false;
                    bullet.IsAlive = false;

                    bullet.Parent.score += 20;
                }
            }

        }

        internal static void Draw(SpriteBatch spriteBatch)
        {
            //throw new NotImplementedException();
        }
    }
}
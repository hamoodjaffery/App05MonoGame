using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MacApp05Game.Controllers;

namespace MacApp05Game.Models
{
    /// <summary>
    /// This class is an AnimatedSprite whose direction
    /// can be controlled by the keyboard in four
    /// directions, up, down, left and right
    /// </summary>
    /// <authors>
    /// Hamood Jaffery
    /// </author>
    public class AnimatedPlayer : AnimatedSprite
    {
        public bool CanWalk { get; set; }
        public BulletController bulletController { get; set; }

        private const double healthLoss = 0.1;
        private KeyboardState Previous;
        private KeyboardState Current;


        private readonly MovementController movement;
        internal int score = 0;
        public double health { get; set; } = 100;

        public AnimatedPlayer() : base()
        {
            CanWalk = false;
            movement = new MovementController();
        }

        /// <summary>
        /// If the sprite has animations for walking in the
        /// four directions then it switches between the
        /// animations depending on the direction
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            if (health == 0)
            {
                IsActive = false;
                IsAlive = false;
                IsVisible = false;
            }
            KeyboardState keyState = Keyboard.GetState();
            Previous = Current;
            Current = keyState;
            IsActive = false;

            Vector2 newDirection = movement.ChangeDirection(keyState);

            if (newDirection != Vector2.Zero)
            {
                Direction = newDirection;
                IsActive = true;

                health -= healthLoss;

                if(health <= 0)
                {
                    IsActive = false;
                    IsAlive = false;
                }
            }

            if (CanWalk) Walk();

            if (Current.IsKeyDown(Keys.Space) &&
                Previous.IsKeyUp(Keys.Space) && (bulletController != null))
            {
                bulletController.AddBullet(this);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Switch between the four walk animations depending
        /// on the direction.  Will not look quite right
        /// with 45 degree directions
        /// </summary>
        private void Walk()
        {
            if (Animations.Count >= 4)
            {
                if (Direction.X > 0 && Direction.Y < Direction.X)
                {
                    Animation = Animations["Right"];
                }
                else if (Direction.Y > 0 && Direction.X < Direction.Y)
                {
                    Animation = Animations["Down"];
                }
                else if (Direction.X < 0 && Direction.X < Direction.Y)
                {
                    Animation = Animations["Left"];
                }
                else if (Direction.Y < 0 && Direction.Y < Direction.X)
                {
                    Animation = Animations["Up"];
                }
            }
        }
    }
}

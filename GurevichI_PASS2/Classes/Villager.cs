using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GurevichI_PASS2
{
    public class Villager : Mob
    {
        public float offScreenTimer;
        private Vector2 position;
        private Texture2D texture;
        private GraphicsDevice graphicsDevice;

        // Villager constructor
        public Villager(ContentManager content, Texture2D texture, Vector2 position, float speed, GraphicsDevice graphicsDevice, int hp) : base(texture, position, (int)4.5f, 1)
        {
            this.position = position;
            this.graphicsDevice = graphicsDevice;
            this.texture = content.Load<Texture2D>("Sized/Villager_64");
            offScreenTimer = 5f;
        }

        public new Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, Texture.Width, Texture.Height);
            }
        }

        public override void Update(GameTime gameTime, Vector2 playerPosition, GraphicsDevice graphicsDevice)
        {
            // Update the Villager's position and handle off-screen timer
            position.X += Speed;
            if (Position.X < 0 || Position.X > graphicsDevice.Viewport.Width || Position.Y < 0 || Position.Y > graphicsDevice.Viewport.Height)
            {
                offScreenTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                offScreenTimer = 5f;
            }
        }

        //Pre: arrow, position, texture, and damage 
        //Post:After executing this subprogram, it will check whether the bounding box of the arrow intersects with the bounding box of the object. If there is a collision,
        //the Hp value of the object will be decreased by the damage value of the arrow. If the Hp value becomes less than or equal to 0,
        //the "ToRemove" bool of the object will be set to true. 
        //The new mob object will have its texture, position, speed, and health points set appropriately.
        //Desc: This method is used to handle collisions between an object and an arrow.
        public bool HandleCollisionWithArrow(Arrow arrow)
        {
            if (BoundingBox.Intersects(arrow.BoundingBox))
            {
                Hp -= arrow.damage;
                if (Hp <= 0)
                {
                    ToRemove = true;
                    return true;
                }
            }
            return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw the Villager
            spriteBatch.Draw(Texture, position, Color.White);
        }
    }
}
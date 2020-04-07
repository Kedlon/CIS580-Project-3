using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameWindowsStarter
{
    public class Hazard
    {
        public Vector2 position { get; set; }

        public BoundingRectangle bounds { get; set; }

        Texture2D texture;

        public Hazard(Texture2D texture, float width, float height)
        {
            this.texture = texture;
            position = new Vector2(0,0);
            bounds = new BoundingRectangle(position, width, height);
        }

        public Hazard(Texture2D texture, Vector2 position, float width, float height)
        {
            this.texture = texture;
            this.position = position;
            bounds = new BoundingRectangle(position, width, height);
        }

        /// <summary>
        /// Draws the sprite using the provided SpriteBatch.  This
        /// method should be invoked between SpriteBatch.Begin() 
        /// and SpriteBatch.End() calls.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

    }
}

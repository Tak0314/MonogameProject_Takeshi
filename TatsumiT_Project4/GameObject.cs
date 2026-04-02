using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace TatsumiT_Project4
{
    internal abstract class GameObject
    {
        protected Texture2D asset;
        protected Rectangle rect;
        protected int windowWidth;
        protected int windowHeight;

        public Texture2D Asset
        {
            get { return asset; }
        }

        public Rectangle Rect
        {
            get { return rect; }
        }

        public int X
        {
            get { return rect.X; }
            set { rect.X = value; }
        }

        public int Y
        {
            get { return rect.Y; }
            set { rect.Y = value; }
        }

        public int Width
        {
            get { return rect.Width; }
            set { rect.Width = value; }
        }

        public int Height
        {
            get { return rect.Height; }
            set { rect.Height = value; }
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="asset">asset of object</param>
        /// <param name="rect">rectangle of object</param>
        /// <param name="windowWidth">width of window</param>
        /// <param name="windowHeight">height of window</param>
        public GameObject(Texture2D asset, Rectangle rect, int windowWidth, int windowHeight)
        {
            this.asset = asset;
            this.rect = rect;
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
        }

        /// <summary>
        /// draw object
        /// </summary>
        /// <param name="sb">sprite batch</param>
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(
                asset,
                rect,
                Color.White);
        }

        /// <summary>
        /// update 
        /// </summary>
        /// <param name="gameTime">game time</param>
        public abstract void Update(GameTime gameTime);

    }
}

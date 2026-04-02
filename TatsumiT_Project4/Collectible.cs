using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TatsumiT_Project4
{
    internal class Collectible : GameObject
    {
        private bool move;
        private Random rng;
        private int moveX;
        private int moveY;
        private int xDir;
        private int yDir;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="asset">asset of collectible</param>
        /// <param name="rect">rectangle of collectible</param>
        /// <param name="windowWidth">width of window</param>
        /// <param name="windowHeight">height of window</param>
        /// <param name="move">bool if the collectible move or not</param>
        public Collectible(Texture2D asset, Rectangle rect, int windowWidth, int windowHeight, bool move)
            :base (asset,rect, windowWidth, windowHeight)
        {
            this.move = move;
            if (move)
            {
                rng = new Random();

                //make random speed
                moveX = rng.Next(0, 6);
                moveY = rng.Next(0, 6);

                //make random direction
                if (rng.Next(0, 2) == 0)
                {
                    xDir = -1;
                }
                else
                {
                    xDir = 1;
                }
                if (rng.Next(0, 2) == 0)
                {
                    yDir = -1;
                }
                else
                {
                    yDir = 1;
                }
            }
        }

        public bool CheckCollision(GameObject otherObject)
        {
            return rect.Intersects(otherObject.Rect);
        }

        /// <summary>
        /// update collectible
        /// </summary>
        /// <param name="gameTime">game time</param>
        public override void Update(GameTime gameTime)
        {
            if (move)
            {
                //move
                X += moveX*xDir;
                Y += moveY*yDir;

                //move to other side of the window when it reached the edge
                if (X >= windowWidth)
                {
                    X = 0 - Width;
                }
                else if (X + Width <= 0)
                {
                    X = windowWidth;
                }

                if (Y >= windowHeight)
                {
                    Y = 0 - Height;
                }
                else if (Y + Height <= 0)
                {
                    Y = windowHeight;
                }
            }
        }
    }
}

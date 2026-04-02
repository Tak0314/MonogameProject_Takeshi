using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatsumiT_Project4
{
    internal class Player : GameObject
    {
        private int levelScore;
        private int totalScore;

        private int speed;

        /// <summary>
        /// score of current level
        /// </summary>
        public int LevelScore
        {
            get { return levelScore; }
            set { levelScore = value; }
        }

        /// <summary>
        /// total score you got
        /// </summary>
        public int TotalScore
        {
            get { return totalScore; }
            set { totalScore = value; }
        }

        /// <summary>
        /// constructor for player
        /// </summary>
        /// <param name="asset">asset of player</param>
        /// <param name="rect">rectangle of player</param>
        /// <param name="windowWidth">width of window</param>
        /// <param name="windowHeight">height of window</param>
        public Player(Texture2D asset, Rectangle rect, int windowWidth, int windowHeight)
            : base(asset, rect, windowWidth, windowHeight)
        {
            levelScore = 0;
            totalScore = 0;
            speed = 4;
        }

        /// <summary>
        /// chek collision
        /// </summary>
        /// <param name="otherObject">other object</param>
        /// <returns></returns>
        public bool CheckCollision(GameObject otherObject)
        {
            return rect.Intersects(otherObject.Rect);
        }

        /// <summary>
        /// update
        /// </summary>
        /// <param name="gameTime">game time</param>
        public override void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.D)) 
            {
                X += speed;
            }
            if (keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.A))
            {
                X -= speed;
            }
            if (keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.W))
            {
                Y -= speed;
            }
            if (keyState.IsKeyDown(Keys.Down) || keyState.IsKeyDown(Keys.S))
            {
                Y += speed;
            }

            if (X >= windowWidth)
            {
                X = 0 - Width;
            }
            else if (X + Width <= 0)
            {
                X = windowWidth;
            }
            else if (Y >= windowHeight)
            {
                Y = 0 - Height;
            }
            else if (Y + Height <= 0)
            {
                Y = windowHeight;
            }

        }

        /// <summary>
        /// move player to canter of window
        /// </summary>
        public void Center()
        {
            X = (windowWidth / 2) - (Width / 2);
            Y = (windowHeight / 2) - (Width / 2);
        }
    }
}

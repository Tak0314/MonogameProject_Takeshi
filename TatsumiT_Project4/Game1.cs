using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace TatsumiT_Project4
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        /// <summary>
        /// FSM
        /// </summary>
        private enum GameModeStates
        {
            Menu,
            Game,
            GameOver
        }

        //fields
        private Player player;
        private List<Collectible> collectibles;
        private Texture2D playerImage;
        private Texture2D collectibleImage;
        private SpriteFont arial36;
        private KeyboardState keyboardState;
        private KeyboardState previousKeyState;
        private GameModeStates currentStates;
        private int windowWidth;
        private int windowHeight;
        private int level;
        private double timer;
        private Random rng;

        private bool debugMode;
        private Texture2D debugPixel;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
        
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            rng = new Random();
            // TODO: use this.Content to load your game content here

            //images
            playerImage = Content.Load<Texture2D>("player_proj4");
            collectibleImage = Content.Load<Texture2D>("gear_collectable");
            arial36 = Content.Load<SpriteFont>("arial36");
            //window size
            windowWidth = GraphicsDevice.Viewport.Width;
            windowHeight = GraphicsDevice.Viewport.Height;
            //other fields
            level = 1;
            timer = 15;
            currentStates = GameModeStates.Menu;

            debugMode = false;

            player = new Player(playerImage, new Rectangle((windowWidth / 2) - 50, (windowHeight / 2) - 50, 100, 100), windowWidth, windowHeight);
            collectibles = new List<Collectible>();

            debugPixel = new Texture2D(GraphicsDevice, 1, 1);
            debugPixel.SetData(new[] { Color.White });
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            previousKeyState = keyboardState;
            keyboardState = Keyboard.GetState();

            //turns on/off debug mode
            if (SingleKeyPress(Keys.Tab, keyboardState, previousKeyState))
            {
                debugMode = !debugMode;
            }


            switch (currentStates)
            {
                case GameModeStates.Menu:
                    //go to Game when enter key pressed
                    if(SingleKeyPress(Keys.Enter,keyboardState,previousKeyState))
                    {
                        currentStates = GameModeStates.Game;
                        level = 1;
                        timer = 15;
                        GenerateCollectibles(level);
                    }
                    break;

                case GameModeStates.Game:
                    //update player
                    player.Update(gameTime);

                    //when collectibles all collected, level up, reset levelScore, reset timer
                    if (collectibles.Count == 0) 
                    {
                        level++;
                        player.LevelScore = 0;
                        GenerateCollectibles(level);
                        timer = 15 + (2 * (level - 1));
                    }

                    //check collision between player and collectible
                    for (int i = collectibles.Count - 1; i >= 0; i--)
                    {
                        collectibles[i].Update(gameTime);
                        //if it is collected, remove the collectible and increase score.
                        if (collectibles[i].CheckCollision(player))
                        {
                            collectibles.Remove(collectibles[i]);
                            player.LevelScore++;
                            player.TotalScore++;
                        }
                    }
                    //update timer
                    timer -= gameTime.ElapsedGameTime.TotalSeconds;

                    //when timer reaches 0, go to GameOver
                    if(timer <= 0)
                    {
                        currentStates = GameModeStates.GameOver;
                    }
                    break;

                case GameModeStates.GameOver:
                    //when enter key pressed, go to Menu
                    if (SingleKeyPress(Keys.Enter, keyboardState, previousKeyState))
                    {
                        currentStates = GameModeStates.Menu;
                        ResetGame();
                    }
                    break;
            }



            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            switch (currentStates)
            {
                case GameModeStates.Menu:
                    //print menu message
                    _spriteBatch.DrawString(
                        arial36,
                        "Welcome to The Factory!",
                        new Vector2((windowWidth/2)-100,(windowHeight/2)),
                        Color.Black);
                    _spriteBatch.DrawString(
                        arial36,
                        "Press Enter to begin\n" +
                        "Use the arrow keys to collect gears!",
                        new Vector2((windowWidth/2)-100,(windowHeight/2)+20),
                        Color.Gray);
                    break;
                case GameModeStates.Game:
                    player.Draw(_spriteBatch);

                    //print level and score, total score, time left
                    _spriteBatch.DrawString(
                        arial36,
                        $"Level {level}: {player.LevelScore}\n" +
                        $"Total Score: {player.TotalScore}\n" +
                        $"Time Left: {timer:F2}",
                        new Vector2(0, 0),
                        Color.Black);

                    //draw all collectibles
                    foreach (Collectible c in collectibles)
                    {
                        c.Draw(_spriteBatch);
                    }

                    //draw debug rect and messages
                    if (debugMode)
                    {
                        DrawBox(player.Rect, Color.Red);

                        foreach (Collectible c in collectibles)
                        {
                            DrawBox(c.Rect, Color.Yellow);
                        }

                        _spriteBatch.DrawString(
                            arial36,
                            $"Pos: ({player.X}, {player.Y})\nState: {currentStates}\nLeft: {collectibles.Count}",
                            new Vector2(10, 100),
                            Color.Black);
                    }
                    break;

                case GameModeStates.GameOver:

                    //print game over message
                    _spriteBatch.DrawString(
                        arial36,
                        "Game Over!",
                        new Vector2((windowWidth / 2) - 100, (windowHeight / 2)),
                        Color.Black);
                    _spriteBatch.DrawString(
                        arial36,
                        "Press Enter to return to main menu.\n\n" +
                        $"Highest Level {level}\n" +
                        $"Total Score {player.TotalScore}",
                        new Vector2((windowWidth / 2) - 100, (windowHeight / 2) + 20),
                        Color.Gray);
                    break;
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
        
        /// <summary>
        /// resets all scores, removes collectibles
        /// </summary>
        private void ResetGame()
        {
            player.LevelScore = 0;
            player.TotalScore = 0;
            collectibles.Clear();
            player.Center();
        }

        /// <summary>
        /// only return true when key is not pressed previous frame
        /// </summary>
        /// <param name="key">key you want to check</param>
        /// <param name="currentState">current keyboard state</param>
        /// <param name="previousState">previous keyboard state</param>
        /// <returns></returns>
        private bool SingleKeyPress(Keys key, KeyboardState currentState, KeyboardState previousState)
        {
            return currentState.IsKeyDown(key) && previousState.IsKeyUp(key);
        }

        /// <summary>
        /// generate collectibles in random place, amount change by level
        /// </summary>
        /// <param name="level">number of level</param>
        private void GenerateCollectibles(int level)
        {
            int count = level*3 + 2;
            for (int i = 0; i < count; i++)
            {
                bool move = rng.Next(0, 2) == 0;
                collectibles.Add(new Collectible(collectibleImage, new Rectangle(rng.Next(0, windowWidth - 25), rng.Next(0, windowHeight - 25), 25, 25), windowWidth, windowHeight, move));
            }
        }

        /// <summary>
        /// draw boxes for debug
        /// </summary>
        /// <param name="rect">rect of object</param>
        /// <param name="color">color of rect</param>
        private void DrawBox(Rectangle rect, Color color)
        {
            _spriteBatch.Draw(debugPixel, new Rectangle(rect.X, rect.Y, rect.Width, 2), color);
            _spriteBatch.Draw(debugPixel, new Rectangle(rect.X, rect.Y + rect.Height - 2, rect.Width, 2), color);
            _spriteBatch.Draw(debugPixel, new Rectangle(rect.X, rect.Y, 2, rect.Height), color);
            _spriteBatch.Draw(debugPixel, new Rectangle(rect.X + rect.Width - 2, rect.Y, 2, rect.Height), color);
        }
    }
}

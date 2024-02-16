using Maze.Input;
using Maze.Components;
using MazeClass = Maze.Components.Maze;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace Maze
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager m_graphics;
        private SpriteBatch m_spriteBatch;

        private KeyboardInput m_inputKeyboard;

        private MazeClass m_maze;
        private Stack<Cell> m_shortestPath;
        private HashSet<Cell> m_breadcrumbs;
        private int mazeStartX;
        private int m_maze_length;
        private int mazeStartY;
        private bool displayHighScores;
        private bool displayCredits;
        private bool displayShortestPath;
        private bool displayBreadcrumbs;
        private bool displayHint;
        private bool isMazeWon;

        private List<Score> m_scores;

        private Character m_character;
        private Texture2D m_texCharacter;

        private Texture2D m_texFinish;
        private Texture2D m_texBreadcrumbs;
        private Texture2D m_texShortestPath;
        private Texture2D m_texBackground;

        // fonts
        private SpriteFont m_fontFoulFiend24;

        // tiles
        private Texture2D m_texBoarder;
        private Texture2D m_texTile;
        private Texture2D m_texTileN;
        private Texture2D m_texTileNS;
        private Texture2D m_texTileNSE;
        private Texture2D m_texTileNSEW;
        private Texture2D m_texTileNSW;
        private Texture2D m_texTileNE;
        private Texture2D m_texTileNEW;
        private Texture2D m_texTileNW;
        private Texture2D m_texTileS;
        private Texture2D m_texTileSE;
        private Texture2D m_texTileSEW;
        private Texture2D m_texTileSW;
        private Texture2D m_texTileE;
        private Texture2D m_texTileEW;
        private Texture2D m_texTileW;

        public Game1()
        {
            m_graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            m_graphics.PreferredBackBufferWidth = 1920;
            m_graphics.PreferredBackBufferHeight = 1080;


            m_graphics.ApplyChanges();

            m_maze_length = m_graphics.PreferredBackBufferWidth / 2;
            mazeStartX = (m_graphics.PreferredBackBufferWidth - m_maze_length) / 2;
            mazeStartY = (m_graphics.PreferredBackBufferHeight - m_maze_length) / 2 + 30;

            m_shortestPath = new Stack<Cell> { };
            m_scores = new List<Score>();
            m_breadcrumbs = new HashSet<Cell> { };

            // Setup input handlers
            m_inputKeyboard = new KeyboardInput();

            m_inputKeyboard.registerCommand(Keys.W, true, new IInputDevice.CommandDelegate(onMoveUp));
            m_inputKeyboard.registerCommand(Keys.S, true, new IInputDevice.CommandDelegate(onMoveDown));
            m_inputKeyboard.registerCommand(Keys.A, true, new IInputDevice.CommandDelegate(onMoveLeft));
            m_inputKeyboard.registerCommand(Keys.D, true, new IInputDevice.CommandDelegate(onMoveRight));

            m_inputKeyboard.registerCommand(Keys.I, true, new IInputDevice.CommandDelegate(onMoveUp));
            m_inputKeyboard.registerCommand(Keys.K, true, new IInputDevice.CommandDelegate(onMoveDown));
            m_inputKeyboard.registerCommand(Keys.J, true, new IInputDevice.CommandDelegate(onMoveLeft));
            m_inputKeyboard.registerCommand(Keys.L, true, new IInputDevice.CommandDelegate(onMoveRight));

            m_inputKeyboard.registerCommand(Keys.Up, true, new IInputDevice.CommandDelegate(onMoveUp));
            m_inputKeyboard.registerCommand(Keys.Down, true, new IInputDevice.CommandDelegate(onMoveDown));
            m_inputKeyboard.registerCommand(Keys.Left, true, new IInputDevice.CommandDelegate(onMoveLeft));
            m_inputKeyboard.registerCommand(Keys.Right, true, new IInputDevice.CommandDelegate(onMoveRight));

            m_inputKeyboard.registerCommand(Keys.H, true, new IInputDevice.CommandDelegate(onToggleHint));
            m_inputKeyboard.registerCommand(Keys.B, true, new IInputDevice.CommandDelegate(onToggleBreadcrumbs));
            m_inputKeyboard.registerCommand(Keys.P, true, new IInputDevice.CommandDelegate(onTogglePathToFinish));

            m_inputKeyboard.registerCommand(Keys.F1, true, new IInputDevice.CommandDelegate(onNew5x5));
            m_inputKeyboard.registerCommand(Keys.F2, true, new IInputDevice.CommandDelegate(onNew10x10));
            m_inputKeyboard.registerCommand(Keys.F3, true, new IInputDevice.CommandDelegate(onNew15x15));
            m_inputKeyboard.registerCommand(Keys.F4, true, new IInputDevice.CommandDelegate(onNew20x20));

            m_inputKeyboard.registerCommand(Keys.F5, true, new IInputDevice.CommandDelegate(onToggleHighScores));
            m_inputKeyboard.registerCommand(Keys.F6, true, new IInputDevice.CommandDelegate(onToggleCredits));

            this.m_maze = new MazeClass(5);
            initAfterMazeCreation();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            m_spriteBatch = new SpriteBatch(GraphicsDevice);

            // fonts
            m_fontFoulFiend24 = this.Content.Load<SpriteFont>("Fonts/foul_fiend");

            // textures
            m_texBoarder = this.Content.Load<Texture2D>("Images/boarder");
            m_texCharacter = this.Content.Load<Texture2D>("Images/zombie");
            m_texFinish = this.Content.Load<Texture2D>("Images/brain");
            m_texBreadcrumbs = this.Content.Load<Texture2D>("Images/bootprint");
            m_texShortestPath = this.Content.Load<Texture2D>("Images/skull");
            m_texBackground = this.Content.Load<Texture2D>("Images/graveyard");
            // tile textures
            m_texTile = this.Content.Load<Texture2D>("Images/Tiles/tile");
            m_texTileN = this.Content.Load<Texture2D>("Images/Tiles/tileN");
            m_texTileNS = this.Content.Load<Texture2D>("Images/Tiles/tileNS");
            m_texTileNSE = this.Content.Load<Texture2D>("Images/Tiles/tileNSE");
            m_texTileNSEW = this.Content.Load<Texture2D>("Images/Tiles/tileNSEW");
            m_texTileNSW = this.Content.Load<Texture2D>("Images/Tiles/tileNSW");
            m_texTileNE = this.Content.Load<Texture2D>("Images/Tiles/tileNE");
            m_texTileNEW = this.Content.Load<Texture2D>("Images/Tiles/tileNEW");
            m_texTileNW = this.Content.Load<Texture2D>("Images/Tiles/tileNW");
            m_texTileS = this.Content.Load<Texture2D>("Images/Tiles/tileS");
            m_texTileSE = this.Content.Load<Texture2D>("Images/Tiles/tileSE");
            m_texTileSEW = this.Content.Load<Texture2D>("Images/Tiles/tileSEW");
            m_texTileSW = this.Content.Load<Texture2D>("Images/Tiles/tileSW");
            m_texTileE = this.Content.Load<Texture2D>("Images/Tiles/tileE");
            m_texTileEW = this.Content.Load<Texture2D>("Images/Tiles/tileEW");
            m_texTileW = this.Content.Load<Texture2D>("Images/Tiles/tileW");
        }

        protected void processInput(GameTime gameTime)
        {
            m_inputKeyboard.Update(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            processInput(gameTime);

            if (m_maze != null && !isMazeWon) m_maze.updateTime(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            m_spriteBatch.Begin();

            m_spriteBatch.Draw(
                m_texBackground, 
                GraphicsDevice.Viewport.Bounds, 
                Color.Cyan);

            if (m_maze != null)
            {
                int tileSize = m_maze_length / m_maze.size;
            
                int objectsOnMazeSizing = (int)(tileSize * 0.7);

                foreach (Cell cell in m_maze.grid)
                {
                    Texture2D texWallToRender;
                    if (cell.isNotWalled()) { texWallToRender = m_texTile; }
                    else if (cell.isN()) { texWallToRender = m_texTileN; }
                    else if (cell.isNS()) { texWallToRender = m_texTileNS; }
                    else if (cell.isNSE()) { texWallToRender = m_texTileNSE; }
                    else if (cell.isNSEW()) { texWallToRender = m_texTileNSEW; }
                    else if (cell.isNSW()) { texWallToRender = m_texTileNSW; }
                    else if (cell.isNE()) { texWallToRender = m_texTileNE; }
                    else if (cell.isNEW()) { texWallToRender = m_texTileNEW; }
                    else if (cell.isNW()) { texWallToRender = m_texTileNW; }
                    else if (cell.isS()) { texWallToRender = m_texTileS; }
                    else if (cell.isSE()) { texWallToRender = m_texTileSE; }
                    else if (cell.isSEW()) { texWallToRender = m_texTileSEW; }
                    else if (cell.isSW()) { texWallToRender = m_texTileSW; }
                    else if (cell.isE()) { texWallToRender = m_texTileE; }
                    else if (cell.isEW()) { texWallToRender = m_texTileEW; }
                    else { texWallToRender = m_texTileW; }

                    m_spriteBatch.Draw(
                        texWallToRender, 
                        new Rectangle(mazeStartX + cell.x * tileSize, mazeStartY + cell.y * tileSize, tileSize, tileSize),
                        null,
                        Color.White,
                        0,
                        new Vector2(0, 0),
                        SpriteEffects.None,
                        0);
                }

                m_spriteBatch.Draw(
                        m_texFinish,
                        new Rectangle(mazeStartX + (m_maze.size - 1) * tileSize + (tileSize / 2), mazeStartY + (m_maze.size - 1) * tileSize + (tileSize / 2), objectsOnMazeSizing, objectsOnMazeSizing),
                        null,
                        Color.White,
                        0,
                        new Vector2(m_texFinish.Width / 2, m_texFinish.Height / 2),
                        SpriteEffects.None,
                        0);

                if (displayBreadcrumbs)
                {
                    if (m_breadcrumbs.Count > 0)
                    {
                        foreach (Cell cell in m_breadcrumbs)
                        { 
                            m_spriteBatch.Draw(
                                m_texBreadcrumbs,
                                new Rectangle(mazeStartX + cell.x * tileSize + (tileSize / 2), mazeStartY + cell.y * tileSize + (tileSize / 2), (int)(objectsOnMazeSizing * 0.5), (int)(objectsOnMazeSizing * 0.5)),
                                null,
                                Color.Gray,
                                0,
                                new Vector2(m_texBreadcrumbs.Width / 2, m_texBreadcrumbs.Height / 2),
                                SpriteEffects.FlipHorizontally,
                                0);
                        }
                    }
                }

                if (displayShortestPath)
                {
                    foreach (Cell cell in m_shortestPath)
                    {
                        m_spriteBatch.Draw(
                            m_texShortestPath,
                            new Rectangle(mazeStartX + cell.x * tileSize + (tileSize / 2), mazeStartY + cell.y * tileSize + (tileSize / 2), (int)(objectsOnMazeSizing * 0.5), (int)(objectsOnMazeSizing * 0.5)),
                            null,
                            Color.White,
                            0,
                            new Vector2(m_texShortestPath.Width / 2, m_texShortestPath.Height / 2),
                            SpriteEffects.None,
                            0);
                    }
                }

                if (displayHint)
                {
                    Cell cell;
                    if (m_shortestPath.Count > 0)
                    { 
                        cell = m_shortestPath.Peek();
                        m_spriteBatch.Draw(
                            m_texShortestPath,
                            new Rectangle(mazeStartX + cell.x * tileSize + (tileSize / 2), mazeStartY + cell.y * tileSize + (tileSize / 2), (int)(objectsOnMazeSizing * 0.5), (int)(objectsOnMazeSizing * 0.5)),
                            null,
                            Color.White,
                            0,
                            new Vector2(m_texShortestPath.Width / 2, m_texShortestPath.Height / 2),
                            SpriteEffects.None,
                            0);
                    }
                }

                if (m_character != null)
                {
                    float scale = tileSize * 0.9f / (float)m_texCharacter.Height;
                    int scaledWidth = (int)(m_texCharacter.Width * scale);
                    int scaledHeight = (int)(m_texCharacter.Height * scale);

                    if (m_character.location != null)
                    {
                        m_spriteBatch.Draw(
                            m_texCharacter,
                            new Rectangle(mazeStartX + m_character.location.x * tileSize + (tileSize / 2), mazeStartY + m_character.location.y * tileSize + (tileSize / 2), scaledWidth, scaledHeight),
                            null,
                            Color.White,
                            0,
                            new Vector2(m_texCharacter.Width / 2, m_texCharacter.Height / 2),
                            SpriteEffects.None,
                            0);
                    }
                }

            }

            // boarder
            int borderWidth = 6;
            m_spriteBatch.Draw(
                m_texBoarder,
                new Rectangle(mazeStartX - borderWidth, mazeStartY - borderWidth, m_maze_length + (borderWidth * 2), m_maze_length + (borderWidth * 2)),
                Color.Black
            );

            // Score
            string strScore;
            if (m_maze != null) strScore = "Score: " + m_maze.score.count.ToString();
            else strScore = "Score: 0";
            float scaleOutlineScore = 0.75f;
            Vector2 stringSizeScore = m_fontFoulFiend24.MeasureString(strScore) * scaleOutlineScore;
            drawOutlineText(
                m_spriteBatch,
                m_fontFoulFiend24, strScore,
                Color.Black, Color.White,
                new Vector2(
                    mazeStartX,
                    mazeStartY - stringSizeScore.Y),
                scaleOutlineScore);

            // Time
            string strTime;
            if (m_maze != null) strTime = "Time: " + m_maze.totalTime.ToString(@"mm\:ss");
            else strTime = "Time: 00:00";
            float scaleOutlineTime = 0.75f;
            Vector2 stringSizeTime = m_fontFoulFiend24.MeasureString(strTime) * scaleOutlineTime;
            drawOutlineText(
                m_spriteBatch,
                m_fontFoulFiend24, strTime,
                Color.Black, Color.White,
                new Vector2(
                    m_graphics.PreferredBackBufferWidth - mazeStartX - stringSizeTime.X,
                    mazeStartY - stringSizeTime.Y),
                scaleOutlineTime);

            // Controls
            const string strControls = "Controls:\n" +
                "  F1:  5x5 Maze\n" +
                "  F2: 10x10 Maze\n" +
                "  F3: 15x15 Maze\n" +
                "  F4: 20x20 Maze\n" +
                "  F5: Display High Scores\n" +
                "  F6: Display Credits\n" +
                "  H: Toggle Hint\n" +
                "  B: Toggle Breadcrumbs\n" +
                "  P: Toggle Path to Finish";
            float scaleOutlineControls = 0.5f;
            Vector2 stringSizeControls = m_fontFoulFiend24.MeasureString(strControls) * scaleOutlineControls;
            drawOutlineText(
                m_spriteBatch,
                m_fontFoulFiend24, strControls,
                Color.Black, Color.White,
                new Vector2(
                    mazeStartX + m_maze_length + 50,
                    mazeStartY),
                scaleOutlineControls);

            // Credits
            if (displayCredits)
            {
                const string strCredit = "Credits:\n" +
                    "  Developed by:\n" +
                    "    Jonathan Gates\n" +
                    "  Foul Fiend Font:\n" +
                    "    Chad Savage\n";
                float scaleOutlineCredits = 0.5f;
                Vector2 stringSizeCredits = m_fontFoulFiend24.MeasureString(strCredit) * scaleOutlineCredits;
                drawOutlineText(
                    m_spriteBatch,
                    m_fontFoulFiend24, strCredit,
                    Color.Black, Color.White,
                    new Vector2(
                        50,
                        mazeStartY),
                    scaleOutlineCredits);
            }

            // High Scores
            if (displayHighScores)
            {
                m_scores.Sort();
                const int MAX_SCORES = 20;
                string strHighScores = "Top " + MAX_SCORES.ToString() + " High Scores:\n";
                int numScores = 0;
                if (m_scores.Count > 0)
                { 
                    foreach (Score score in m_scores)
                    {
                        strHighScores += "  Score: " + score.count.ToString() + " : Size: " + score.mazeSize +  "\n";
                        numScores++;
                        if (numScores >= MAX_SCORES) break;
                    }
                }
                float scaleOutlineHighScores = 0.5f;
                Vector2 stringSizeHighScores = m_fontFoulFiend24.MeasureString(strHighScores) * scaleOutlineHighScores;
                drawOutlineText(
                    m_spriteBatch,
                    m_fontFoulFiend24, strHighScores,
                    Color.Black, Color.White,
                    new Vector2(
                        50,
                        mazeStartY),
                    scaleOutlineHighScores);
            }


            m_spriteBatch.End();

            base.Draw(gameTime);
        }

        protected static void drawOutlineText(SpriteBatch spriteBatch, SpriteFont font, string text, Color outlineColor, Color frontColor, Vector2 position, float scale)
        {
            const float PIXEL_OFFSET = 1.0f;
            //
            // Offset to the upper left and lower right - faster, but not as good
            //spriteBatch.DrawString(font, text, position - new Vector2(PIXEL_OFFSET * scale, PIXEL_OFFSET * scale), outlineColor, 0, Vector2.Zero, scale, SpriteEffects.None, 1f);
            //spriteBatch.DrawString(font, text, position + new Vector2(PIXEL_OFFSET * scale, PIXEL_OFFSET * scale), outlineColor, 0, Vector2.Zero, scale, SpriteEffects.None, 1f);

            //
            // Offset in each of left,right, up, down directions - slower, but good
            spriteBatch.DrawString(font, text, position - new Vector2(PIXEL_OFFSET * scale, 0), outlineColor, 0, Vector2.Zero, scale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, text, position + new Vector2(PIXEL_OFFSET * scale, 0), outlineColor, 0, Vector2.Zero, scale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, text, position - new Vector2(0, PIXEL_OFFSET * scale), outlineColor, 0, Vector2.Zero, scale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, text, position + new Vector2(0, PIXEL_OFFSET * scale), outlineColor, 0, Vector2.Zero, scale, SpriteEffects.None, 1f);

            //
            // This sits inside the text rendering done just above
            spriteBatch.DrawString(font, text, position, frontColor, 0, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        private void handleMove(Cell locationNew)
        {
            if (m_character.location.x == m_maze.size - 1 && m_character.location.y == m_maze.size - 1) 
            {
                return;
            }

            if (!m_breadcrumbs.Contains(locationNew))
            {
                this.m_breadcrumbs.Add(locationNew);

                // change score
                if (m_shortestPath.Count > 0 && locationNew == m_shortestPath.Peek())
                {
                    m_maze.score.count += 5;
                }
                else if (locationNew.x == m_maze.size - 1 && locationNew.y == m_maze.size - 1)
                {
                    m_maze.score.count += 5;
                    isMazeWon = true;
                }
                else if (m_maze.adjacentShortestPath.Contains(locationNew))
                {
                    m_maze.score.count -= 1;
                }
                else
                {
                    m_maze.score.count -= 2;
                }

            }

            if (m_shortestPath.Count > 0 && m_shortestPath.Peek() == locationNew)
            {
                m_shortestPath.Pop();
            }
            else
            {
                if (!(locationNew.x == m_maze.size - 1 && locationNew.y == m_maze.size - 1))
                {
                    m_shortestPath.Push(m_character.location);
                }
                else
                {
                    m_scores.Add(m_maze.score);
                    displayCredits = false;
                    displayHighScores = true;
                }
            }

            m_character.location = locationNew;
        }

        #region Input Handlers
        /// <summary>
        /// The various move methods for the character, creation of the maze and its size, and other controls.
        /// </summary>
        private void onMoveUp(GameTime gameTime, float scale)
        {
            if (m_character != null && m_character.location.n != null)
            {
                handleMove(m_character.location.n);
            }
        }

        private void onMoveDown(GameTime gameTime, float scale)
        {
            if (m_character != null && m_character.location.s != null)
            {
                handleMove(m_character.location.s);
            }
        }

        private void onMoveLeft(GameTime gameTime, float scale)
        {
            if (m_character != null && m_character.location.w != null)
            {
                handleMove(m_character.location.w);
            }
        }

        private void onMoveRight(GameTime gameTime, float scale)
        {
            if (m_character != null && m_character.location.e != null)
            {
                handleMove(m_character.location.e);
            }
        }


        private void onToggleHint(GameTime gameTime, float scale)
        {
            displayShortestPath = false;
            displayHint = !displayHint;
        }

        private void onToggleBreadcrumbs(GameTime gameTime, float scale)
        {
            displayBreadcrumbs = !displayBreadcrumbs;
        }

        private void onTogglePathToFinish(GameTime gameTime, float scale)
        {
            displayHint = false;
            displayShortestPath = !displayShortestPath;
        }

        private void initAfterMazeCreation()
        {
            this.isMazeWon = false;
            this.m_character = new Character(this.m_maze.grid[0, 0]);
            this.m_breadcrumbs = new HashSet<Cell> { };
            this.m_breadcrumbs.Add(this.m_maze.grid[0, 0]);
            this.m_character.breadcrumbs.Add(this.m_maze.grid[0, 0]);
            this.m_shortestPath.Clear();

            // Set shortest path
            foreach (Cell cell in m_maze.shortestPath)
            {
                if (!(cell.x == 0 && cell.y == 0 || cell.x == m_maze.size - 1 && cell.y == m_maze.size - 1))
                { 
                    this.m_shortestPath.Push(cell);
                }
            }
        }

        private void onNew5x5(GameTime gameTime, float scale)
        {
            this.m_maze = new MazeClass(5);
            initAfterMazeCreation();
        }

        private void onNew10x10(GameTime gameTime, float scale)
        {
            this.m_maze = new MazeClass(10);
            initAfterMazeCreation();
        }

        private void onNew15x15(GameTime gameTime, float scale)
        {
            this.m_maze = new MazeClass(15);
            initAfterMazeCreation();
        }

        private void onNew20x20(GameTime gameTime, float scale)
        {
            this.m_maze = new MazeClass(20);
            initAfterMazeCreation();
        }

        private void onToggleHighScores(GameTime gameTime, float scale)
        {
            this.displayCredits = false;
            this.displayHighScores = !this.displayHighScores;
        }

        private void onToggleCredits(GameTime gameTime, float scale)
        {
            this.displayHighScores = false;
            this.displayCredits = !this.displayCredits;
        }

        #endregion
    }
    
}
using Maze.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Maze
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager m_graphics;
        private SpriteBatch m_spriteBatch;

        private KeyboardInput m_inputKeyboard;

        private Character m_character;
        private Texture2D m_texCharacter;

        private Texture2D m_texBrain;

        private Maze m_maze;

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

            base.Initialize();
        }

        protected override void LoadContent()
        {
            m_spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            m_texCharacter = this.Content.Load<Texture2D>("Images/zombie");
            m_texBrain = this.Content.Load<Texture2D>("Images/brain");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            m_inputKeyboard.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            m_spriteBatch.Begin();

            // m_spriteBatch.Draw(/////);
            m_spriteBatch.Draw(
                m_texCharacter,
                 new Rectangle(1000, 1000, 400, 400),
                null,
                Color.White,
                0,
                new Vector2(m_texCharacter.Width / 2, m_texCharacter.Height / 2),
                SpriteEffects.None,
                0);

            m_spriteBatch.End();

            base.Draw(gameTime);
        }

        #region Input Handlers
        /// <summary>
        /// The various moveX methods subtract half of the height/width because the rendering is performed
        /// from the center of the rectangle because it can rotate
        /// </summary>
        private void onMoveUp(GameTime gameTime, float scale)
        {
            if (m_character != null && m_character.location.n != null)
            {
                m_character.location = m_character.location.n;
                if (!m_character.location.visited)
                {
                    m_character.location.visited = true;
                    // add to breadcrumbs
                }
                // edit scores / edit shortest path
            }
        }

        private void onMoveDown(GameTime gameTime, float scale)
        {

        }

        private void onMoveLeft(GameTime gameTime, float scale)
        {

        }

        private void onMoveRight(GameTime gameTime, float scale)
        {

        }

        private void onToggleHint(GameTime gameTime, float scale)
        {

        }

        private void onToggleBreadcrumbs(GameTime gameTime, float scale)
        {

        }

        private void onTogglePathToFinish(GameTime gameTime, float scale)
        {

        }

        private void onNew5x5(GameTime gameTime, float scale)
        {
            // TODO: reset score on maze regen
            // give character to 0,0?
            this.m_maze = new Maze(5);
        }

        private void onNew10x10(GameTime gameTime, float scale)
        {
            this.m_maze = new Maze(5);
        }

        private void onNew15x15(GameTime gameTime, float scale)
        {
            this.m_maze = new Maze(5);
        }

        private void onNew20x20(GameTime gameTime, float scale)
        {
            this.m_maze = new Maze(5);
        }

        private void onDisplayHighScores(GameTime gameTime, float scale)
        {

        }

        private void onDisplayCredits(GameTime gameTime, float scale)
        {

        }

        #endregion
    }

    public class Character
    {
        public Cell location;

        public Character(Cell location)
        {
            this.location = location;
        }

        public void moveUp()
        { 
            if (location.n != null) 
            {
                location = location.n;
            }
        }

        public void moveDown()
        {
            if (location.s != null)
            {
                location = location.s;
            }
        }

        public void moveLeft()
        {
            if (location.w != null)
            {
                location = location.w;
            }
        }

        public void moveRight()
        {
            if (location.e != null)
            {
                location = location.e;
            }
        }

    }

    public class Maze
    {
        public int size { get; private set; }
        public Cell[,] grid { get; private set; }


        public Maze(int size)
        {
            this.size = size;
            this.grid = new Cell[size, size];

            initializePrims();

        }

        private void initializePrims() 
        {
            for (int i = 0; i < this.size; i++)
            {
                for (int j = 0; j < this.size; j++)
                {
                    grid[i, j] = new Cell(i, j);
                }
            }

            HashSet<Cell> maze = new HashSet<Cell> { };
            List<Cell> frontier = new List<Cell> { };

            Cell startCell = grid[0,0];
            maze.Add(startCell);
            foreach (var cell in getNeighbors(startCell))
            {
                frontier.Add(cell);
            }

            Random random = new Random();
            while (frontier.Count > 0)
            {
                // randomly pick cell from frontier
                int index = random.Next(frontier.Count);
                maze.Add(frontier[index]);
                List<Cell> neighbors = getNeighbors(frontier[index]);
                foreach (var cell in neighbors)
                {
                    if (!maze.Contains(cell)) frontier.Add(cell);
                }

                randomRemoveWall(frontier[index], neighbors, maze);

                frontier.RemoveAt(index);
            }

        }

        private List<Cell> getNeighbors(Cell cell)
        {
            List<Cell> cells = new List<Cell>();
            if (cell.y != 0) cells.Add(grid[cell.x, cell.y - 1]);
            if (cell.x != 0) cells.Add(grid[cell.x, cell.y - 1]);
            if (cell.y < size - 1) cells.Add(grid[cell.x, cell.y + 1]);
            if (cell.x < size - 1) cells.Add(grid[cell.x + 1, cell.y]);
            return cells;
        }

        private void randomRemoveWall(Cell mainCell, List<Cell> neightbors, HashSet<Cell> maze)
        {
            List<Cell> options = new List<Cell>();
            foreach (var cell in neightbors)
            { 
                if (maze.Contains(cell)) options.Add(cell);
            }
            Random random= new Random();
            int index = random.Next(options.Count);

            if (mainCell.x > neightbors[index].x)
            {
                mainCell.n = neightbors[index];
                neightbors[index].s = mainCell;
            }
            else if (mainCell.x < neightbors[index].x)
            {
                mainCell.s = neightbors[index];
                neightbors[index].n = mainCell;
            }
            else if (mainCell.y > neightbors[index].y)
            {
                mainCell.w = neightbors[index];
                neightbors[index].e = mainCell;
            }
            else
            {
                mainCell.e = neightbors[index];
                neightbors[index].w = mainCell;
            }
        }

    }

    public class Cell
    {
        public int x { get; set; }
        public int y { get; set; }
        public Cell n { get; set; }
        public Cell s { get; set; }
        public Cell e { get; set; }
        public Cell w { get; set; }
        public bool visited { get; set; }

        public Cell(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.visited = false;
        }

    }

    public class Score
    {

    }
}
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

        private Maze m_maze;
        private Queue<Cell> m_shortestPath;
        private int m_maze_length;
        private int m_mazeCenterX;
        private int m_mazeCenterY;

        private Character m_character;
        private Texture2D m_texCharacter;

        private Texture2D m_texBrain;

        // tiles
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
            m_mazeCenterX = m_graphics.PreferredBackBufferWidth / 2;
            m_mazeCenterY = m_graphics.PreferredBackBufferHeight / 2;

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

            m_texCharacter = this.Content.Load<Texture2D>("Images/zombie");
            m_texBrain = this.Content.Load<Texture2D>("Images/brain");
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
                SpriteEffects.FlipHorizontally,
                0);

            if (m_maze != null)
            {
                int tileSize = m_maze_length / m_maze.size;

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
                        new Rectangle( cell.x * tileSize + (m_mazeCenterX / m_maze.size), cell.y * tileSize + (m_mazeCenterY / m_maze.size), tileSize, tileSize),
                        null,
                        Color.White,
                        0,
                        new Vector2(texWallToRender.Width / 2, texWallToRender.Height / 2),
                        SpriteEffects.None,
                        0);
                }
            }

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
                    // change score
                    // edit shortest path
                }
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
            //this.m_maze = new Maze(5);

            Maze maze = new Maze(3);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    maze.grid[i, j].n = null;
                    maze.grid[i, j].s = null;
                    maze.grid[i, j].e = null;
                    maze.grid[i, j].w = null;
                }
            }
            maze.grid[0, 0].s = maze.grid[1, 0];

            maze.grid[0, 1].s = maze.grid[1, 1];
            maze.grid[0, 1].e = maze.grid[0, 2];

            maze.grid[0, 2].w = maze.grid[0, 1];
            maze.grid[0, 2].s = maze.grid[1, 2];

            maze.grid[1, 0].n = maze.grid[0, 0];
            maze.grid[1, 0].e = maze.grid[1, 1];
            maze.grid[1, 0].s = maze.grid[2, 0];

            maze.grid[1, 1].n = maze.grid[0, 1];
            maze.grid[1, 1].s = maze.grid[2, 1];
            maze.grid[1, 1].w = maze.grid[1, 0];

            maze.grid[1, 2].n = maze.grid[0, 2];

            maze.grid[2, 0].n = maze.grid[1, 0];

            maze.grid[2, 1].n = maze.grid[1, 1];
            maze.grid[2, 1].e = maze.grid[2, 2];

            maze.grid[2, 2].w = maze.grid[2, 1];

            this.m_maze = maze;
        }

        private void onNew10x10(GameTime gameTime, float scale)
        {
            this.m_maze = new Maze(10);
        }

        private void onNew15x15(GameTime gameTime, float scale)
        {
            this.m_maze = new Maze(15);
        }

        private void onNew20x20(GameTime gameTime, float scale)
        {
            this.m_maze = new Maze(20);
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

                List<Cell> neighbors = getNeighbors(frontier[index]);

                randomRemoveWall(frontier[index], neighbors, maze);
                maze.Add(frontier[index]);

                foreach (var cell in neighbors)
                {
                    if (!maze.Contains(cell) && !frontier.Contains(cell)) frontier.Add(cell);
                }


                frontier.RemoveAt(index);
            }

        }

        private List<Cell> getNeighbors(Cell cell)
        {
            List<Cell> cells = new List<Cell>();
            if (cell.y != 0) cells.Add(grid[cell.x, cell.y - 1]);
            if (cell.x != 0) cells.Add(grid[cell.x - 1, cell.y]);
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
            Random random = new Random();
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

        public bool isNotWalled()
        {
            return n != null && s != null && e != null && w != null;
        }

        public bool isN()
        {
            return n == null && s != null && e != null && w != null;
        }

        public bool isNS()
        {
            return n == null && s == null && e != null && w != null;
        }

        public bool isNSE()
        {
            return n == null && s == null && e == null && w != null;
        }

        public bool isNSEW()
        {
            return n == null && s == null && e == null && w == null;
        }

        public bool isNSW()
        {
            return n == null && s == null && e != null && w == null;
        }

        public bool isNE()
        {
            return n == null && s != null && e == null && w != null;
        }

        public bool isNEW()
        {
            return n == null && s != null && e == null && w == null;
        }

        public bool isNW()
        {
            return n == null && s != null && e != null && w == null;
        }

        public bool isS()
        {
            return n != null && s == null && e != null && w != null;
        }

        public bool isSE()
        {
            return n != null && s == null && e == null && w != null;
        }

        public bool isSEW()
        {
            return n != null && s == null && e == null && w == null;
        }

        public bool isSW()
        {
            return n != null && s == null && e != null && w == null;
        }

        public bool isE()
        {
            return n != null && s != null && e == null && w != null;
        }

        public bool isEW()
        {
            return n != null && s != null && e == null && w == null;
        }

        public bool isW()
        {
            return n != null && s != null && e != null && w == null;
        }

    }

    public class Score
    {

    }
}
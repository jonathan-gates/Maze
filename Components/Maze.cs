using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Maze.Components
{
    public class Maze
    {
        public int size { get; private set; }
        public Cell[,] grid { get; private set; }
        public List<Cell> shortestPath { get; private set; }
        public HashSet<Cell> adjacentShortestPath { get; private set; }
        public Score score { get; private set; }
        public TimeSpan totalTime { get; private set; }

        private Random random;


        public Maze(int size)
        {
            this.size = size;
            this.grid = new Cell[size, size];
            this.random = new Random();
            this.score = new Score(size);
            this.adjacentShortestPath = new HashSet<Cell>();
            totalTime = new TimeSpan();

            initializePrims();
            shortestPath = FindPathBFS(grid[0, 0], grid[size - 1, size - 1]);
            foreach (Cell cell in shortestPath)
            {
                foreach (Cell spCell in getAccessibleNeighbors(cell))
                {
                    adjacentShortestPath.Add(spCell);
                }
            }

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
            HashSet<Cell> frontier = new HashSet<Cell> { };

            Cell startCell = grid[0, 0];
            maze.Add(startCell);
            foreach (var cell in getAllNeighbors(startCell))
            {
                frontier.Add(cell);
            }

            while (frontier.Count > 0)
            {
                Cell ranCell = frontier.ElementAt(this.random.Next(frontier.Count));

                List<Cell> neighbors = getAllNeighbors(ranCell);

                maze.Add(ranCell);
                randomRemoveWall(ranCell, neighbors, maze);

                foreach (var cell in neighbors)
                {
                    if (!maze.Contains(cell) && !frontier.Contains(cell)) frontier.Add(cell);
                }

                frontier.Remove(ranCell);

            }

        }

        private List<Cell> getAllNeighbors(Cell cell)
        {
            List<Cell> cells = new List<Cell>();
            if (cell.y > 0) cells.Add(grid[cell.x, cell.y - 1]);
            if (cell.x > 0) cells.Add(grid[cell.x - 1, cell.y]);
            if (cell.y < size - 1) cells.Add(grid[cell.x, cell.y + 1]);
            if (cell.x < size - 1) cells.Add(grid[cell.x + 1, cell.y]);
            return cells;
        }

        private void randomRemoveWall(Cell mainCell, List<Cell> neightbors, HashSet<Cell> maze)
        {
            HashSet<Cell> options = new HashSet<Cell>();
            foreach (var cell in neightbors)
            {
                if (maze.Contains(cell)) options.Add(cell);
            }

            Cell randomCell = options.ElementAt(this.random.Next(options.Count));

            if (mainCell.x > randomCell.x)
            {
                mainCell.w = randomCell;
                randomCell.e = mainCell;
            }
            else if (mainCell.x < randomCell.x)
            {
                mainCell.e = randomCell;
                randomCell.w = mainCell;
            }
            else if (mainCell.y > randomCell.y)
            {
                mainCell.n = randomCell;
                randomCell.s = mainCell;
            }
            else
            {
                mainCell.s = randomCell;
                randomCell.n = mainCell;
            }
        }

        // chatgpt for BFS
        public List<Cell> FindPathBFS(Cell start, Cell end)
        {
            Queue<Cell> queue = new Queue<Cell>();
            Dictionary<Cell, Cell> predecessors = new Dictionary<Cell, Cell>();
            HashSet<Cell> visited = new HashSet<Cell>
            {
                start
            };

            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                Cell current = queue.Dequeue();
                if (current == end) // Path found
                {
                    return ReconstructPath(predecessors, end);
                }

                foreach (Cell neighbor in getAccessibleNeighborsBFSHelper(current, visited))
                {
                    if (visited.Add(neighbor))
                    {
                        predecessors[neighbor] = current; // Record the path
                        queue.Enqueue(neighbor);
                    }
                }
            }

            return null; // No path found
        }

        private List<Cell> ReconstructPath(Dictionary<Cell, Cell> predecessors, Cell end)
        {
            List<Cell> path = new List<Cell>();
            for (Cell at = end; at != null; at = predecessors.ContainsKey(at) ? predecessors[at] : null)
            {
                path.Add(at);
            }
            // path.Reverse();
            return path;
        }

        private List<Cell> getAccessibleNeighborsBFSHelper(Cell cell, HashSet<Cell> visited)
        {
            List<Cell> neighbors = new List<Cell>();
            if (cell.n != null && !visited.Contains(cell.n)) neighbors.Add(cell.n);
            if (cell.s != null && !visited.Contains(cell.s)) neighbors.Add(cell.s);
            if (cell.e != null && !visited.Contains(cell.e)) neighbors.Add(cell.e);
            if (cell.w != null && !visited.Contains(cell.w)) neighbors.Add(cell.w);
            return neighbors;
        }

        private List<Cell> getAccessibleNeighbors(Cell cell)
        {
            List<Cell> neighbors = new List<Cell>();
            if (cell.n != null) neighbors.Add(cell.n);
            if (cell.s != null) neighbors.Add(cell.s);
            if (cell.e != null) neighbors.Add(cell.e);
            if (cell.w != null) neighbors.Add(cell.w);
            return neighbors;
        }

        public void updateTime(GameTime gameTime)
        {
            totalTime += gameTime.ElapsedGameTime;
        }



    }
}

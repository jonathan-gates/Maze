using System.Collections.Generic;

namespace Maze.Components
{
    public class Character
    {
        public Cell location;
        public List<Cell> breadcrumbs { get; set; }

        public Character(Cell location)
        {
            this.location = location;
            this.breadcrumbs = new List<Cell>();
        }

    }
}

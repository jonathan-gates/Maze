using System;

namespace Maze.Components
{
    public class Score : IComparable<Score>
    {
        public int count { get; set; }
        public int mazeSize { get; private set; }

        public Score(int mazeSize)
        {
            this.count = 0;
            this.mazeSize = mazeSize;
        }

        public int CompareTo(Score other)
        {
            // Returns the one with the greater count or greater size
            int countComparison = other.count.CompareTo(count);
            if (countComparison != 0)
            {
                return countComparison;
            }
            else
            {
                return other.mazeSize.CompareTo(mazeSize);
            }
        }
    }
}

namespace Maze.Components
{
    public class Cell
    {
        public int x { get; set; }
        public int y { get; set; }
        public Cell n { get; set; }
        public Cell s { get; set; }
        public Cell e { get; set; }
        public Cell w { get; set; }

        public Cell(int x, int y)
        {
            this.x = x;
            this.y = y;
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
}

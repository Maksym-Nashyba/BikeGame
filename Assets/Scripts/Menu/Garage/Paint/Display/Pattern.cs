using Array2DEditor;
using UnityEngine;

namespace Menu.Garage.Paint.Display
{
    public sealed class Pattern
    {
        public readonly Vector2Int Size;
        public readonly bool[,] Grid;

        public Pattern(Vector2Int size)
        {
            Size = size;
            Grid = new bool[size.x, size.y];
        }
        
        public Pattern(Vector2Int size, Array2DBool grid)
        {
            Size = size;
            Grid = new bool[Size.x, Size.y];
            FillGrid(Grid, grid);
        }

        private void FillGrid(bool[,] target, Array2DBool source)
        {
            for (int x = 0; x < source.GridSize.x; x++)
            {
                for (int y = 0; y < source.GridSize.y; y++)
                {
                    target[x, y] = source.GetCell(x, y);
                }
            }
        }

        public void Insert(Vector2Int location, Pattern pattern)
        {
            for (int x = 0; x < pattern.Size.x; x++)
            {
                if(location.x + x < 0 || location.x + x > Size.x-1) continue;
                for (int y = 0; y < pattern.Size.y; y++)
                {
                    if(location.y + y < 0 || location.y + y > Size.y-1) continue;
                    Grid[location.x + x, location.y + y] = pattern.Grid[x, y];
                }
            }
        }
    }
}
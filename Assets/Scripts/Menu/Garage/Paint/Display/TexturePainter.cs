using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace Menu.Garage.Paint.Display
{
    public class TexturePainter
    {
        private readonly Vector2Int _resolution;
        private readonly Vector2Int _cellSize;
        private readonly Texture2D _texture;

        public TexturePainter(Vector2Int resolution, MeshRenderer renderer)
        {
            _resolution = resolution;
            _cellSize = CalculateCellSize(resolution);
            _texture = InitializeTexture(resolution, renderer);
        }

        #region Initialization
        private Texture2D InitializeTexture(Vector2Int size, MeshRenderer renderer)
        {
            Texture2D texture = new Texture2D(size.x, size.y, GraphicsFormat.R8G8B8A8_SRGB, TextureCreationFlags.None);
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;
            renderer.material.SetTexture("_Image", texture);
            return texture;
        }
        
        private Vector2Int CalculateCellSize(Vector2Int Resolution)
        {
            return new Vector2Int(Resolution.x / 2, Resolution.y / 3);
        }
        #endregion

        public void PaintPatternInCell(Vector2Int cellIndex, Pattern pattern)
        {
            Vector2Int topLeft = cellIndex * _cellSize;
            PaintPattern(topLeft, pattern);
        }
        
        public void PaintPattern(Vector2Int topLeft, Pattern pattern)
        {
            for (int x = 0; x < pattern.Size.x; x++)
            {
                if(topLeft.x+x < 0 || topLeft.x+x > _resolution.x-1)continue;
                for (int y = 0; y < pattern.Size.y; y++)
                {
                    if(topLeft.y+y < 0 || topLeft.y+y > _resolution.y-1)continue;
                    Color color = pattern.Grid[x,y] ? Color.green : Color.clear;
                    PaintPixel(topLeft + new Vector2Int(x, y), color);
                }
            }
        }
        
        public void Apply()
        {
            _texture.Apply();
        }
        
        private void PaintPixel(Vector2Int position, Color color)
        {
            _texture.SetPixel(position.x, _resolution.y - position.y - 1, color);
        }
    }
}
using System.Threading.Tasks;
using Misc;
using Misc.Extensions;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace Menu.Garage.Paint.Display
{
    public class TexturePainter
    {
        private readonly Vector2Int _resolution;
        private readonly Vector2Int _cellSize;
        private readonly Texture2D _texture;
        private readonly AsyncExecutor _asyncExecutor;

        public TexturePainter(Vector2Int resolution, MeshRenderer renderer, AsyncExecutor asyncExecutor)
        {
            _resolution = resolution;
            _cellSize = CalculateCellSize(resolution);
            _texture = InitializeTexture(resolution, renderer);
            _asyncExecutor = asyncExecutor;
        }

        #region Initialization
        private Texture2D InitializeTexture(Vector2Int size, MeshRenderer renderer)
        {
            Texture2D texture = BuildTexture(size);
            renderer.material.SetTexture("_Image", texture);
            return texture;
        }

        private Texture2D BuildTexture(Vector2Int size)
        {
            return new Texture2D(size.x, size.y, 
                GraphicsFormat.R8G8B8A8_SRGB, TextureCreationFlags.None)
            {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };
        }
        
        private Vector2Int CalculateCellSize(Vector2Int resolution)
        {
            return new Vector2Int(resolution.x / 2, resolution.y / 3);
        }
        #endregion

        public void PaintPatternInCell(Vector2Int cell, Pattern pattern, Color color)
        {
            Vector2Int topLeft = cell * _cellSize;
            PaintPattern(topLeft, pattern, color);
        }
        
        public void PaintPattern(Vector2Int topLeft, Pattern pattern, Color color)
        {
            for (int x = 0; x < pattern.Size.x; x++)
            {
                if(topLeft.x+x < 0 || topLeft.x+x > _resolution.x-1) continue;
                for (int y = 0; y < pattern.Size.y; y++)
                {
                    if(topLeft.y+y < 0 || topLeft.y+y > _resolution.y-1) continue;
                    Color pixelColor = pattern.Grid[x,y] ? color : Color.clear;
                    PaintPixel(topLeft + new Vector2Int(x, y), pixelColor);
                }
            }
        }

        public void PaintFromTexture(Texture2D source)
        {
            Graphics.CopyTexture(source, _texture);
        }

        public Task CleanAnimated()
        {
            Texture2D copy = BuildTexture(_resolution);
            Graphics.CopyTexture(_texture, copy);
            Clear();
            return _asyncExecutor.EachFrame(1.2f, t =>
            {
                Clear();
                Color[] columnBuffer = new Color[_resolution.y];
                for (int x = 0; x < _resolution.x; x++)
                {
                    float horizontalPosition = (float)x / _resolution.x;
                    int yPatternPosition = (int)(_resolution.y*horizontalPosition*t + _resolution.y*t);
                    copy.GetPixelsNonAlloc(x, yPatternPosition, 1, _resolution.y, columnBuffer);
                    _texture.SetPixels(x,0,1,_resolution.y, columnBuffer);
                }
                Apply();
            }, EaseFunctions.EaseInCirc);
        }
        
        public void Clear()
        {
            for (int x = 0; x < _resolution.x; x++)
            {
                for (int y = 0; y < _resolution.y; y++)
                {
                    _texture.SetPixel(x,y, Color.clear);
                }
            }
            Apply();
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
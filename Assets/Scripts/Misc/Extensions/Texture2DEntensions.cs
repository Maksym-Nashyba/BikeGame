using UnityEngine;

namespace Misc.Extensions
{
    public static class Texture2DEntensions
    {
        public static void GetPixelsNonAlloc(this Texture2D texture, int xPosition, int yPosition, int width, int height, Color[] buffer)
        {
            int i = 0;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    buffer[i] = texture.GetPixel(xPosition + x, yPosition + y);
                    i++;
                }
            }            
        }
    }
}
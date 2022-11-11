using UnityEngine;

namespace Misc
{
    public static class RectTransformExtensions
    {
        public static bool ContainsPoint(this RectTransform rectTransform, Vector2 offset, Vector2 position)
        {
            Rect rect = rectTransform.rect;
            rect.size *= rectTransform.localScale;
            rect.center =  offset;
            return rect.Contains(position);
        }
    }
}
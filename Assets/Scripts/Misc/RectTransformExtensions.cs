using UnityEngine;

namespace Misc
{
    public static class RectTransformExtensions
    {
        public static bool ContainsPoint(this RectTransform rectTransform, Vector2 postion)
        {
            Rect rect = rectTransform.rect;
            rect.size *= rectTransform.localScale;
            rect.center = rectTransform.anchoredPosition;
            return rect.Contains(postion);
        }
    }
}
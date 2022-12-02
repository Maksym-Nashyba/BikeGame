using System;
using IGUIDResources;
using Misc;
using UnityEngine;

namespace Garage.Paint
{
    public class PaintContainer : ClickTarget<GameObject>
    {
        public override event Action<GameObject> Clicked;
        [SerializeField] private Skin _skin;

        public Skin GetSkin()
        {
            return _skin;
        }
        
        protected override void OnClicked()
        {
            Clicked?.Invoke(gameObject.transform.parent.gameObject);
        }
    }
}
using System;
using IGUIDResources;
using Misc;
using UnityEngine;

namespace Garage.Paint
{
    public class PaintContainer : ClickTarget<GameObject>
    {
        public override event Action<GameObject> Clicked;
        public Skin Skin { get; set; }

        protected override void OnClicked()
        {
            Clicked?.Invoke(gameObject.transform.parent.gameObject);
        }
    }
}
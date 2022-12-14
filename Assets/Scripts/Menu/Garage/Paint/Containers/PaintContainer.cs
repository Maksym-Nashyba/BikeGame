using System;
using System.Threading.Tasks;
using IGUIDResources;
using Misc;
using UnityEngine;

namespace Menu.Garage.Paint.Containers
{
    public class PaintContainer : ClickTarget<PaintContainer>
    {
        public override event Action<PaintContainer> Clicked;
        public Skin Skin { get; private set; }
        public Vector2Int Cell { get; private set; }
        
        [SerializeField] private PaintContainerAnimator _paintContainerAnimator;

        protected override void OnClicked() 
        {
            Clicked?.Invoke(this);
        }
        
        public void SetUp(Vector2Int cell)
        {
            Cell = cell;
        }
        
        public Task Fill(Skin skin)
        {
            Skin = skin;
            _paintContainerAnimator.ApplySkin(skin);
            return _paintContainerAnimator.PlayFillAnimation();
        }

        public Task Clean()
        {
            if(Skin == null) return Task.CompletedTask;
            Skin = null;
            return _paintContainerAnimator.PlayCleanAnimation();
        }
    }
}
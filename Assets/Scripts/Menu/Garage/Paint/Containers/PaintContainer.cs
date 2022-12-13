using System;
using System.Threading;
using System.Threading.Tasks;
using IGUIDResources;
using Misc;
using UnityEngine;
using UnityEngine.Serialization;

namespace Menu.Garage.Paint.Containers
{
    public class PaintContainer : ClickTarget<PaintContainer>
    {
        public override event Action<PaintContainer> Clicked;
        public Skin Skin { get; private set; }
        public Vector2Int Cell { get; private set; }
        
        [SerializeField] private Transform _paintTransform;
        [SerializeField] private Transform _leakTransfrm;
        [SerializeField] private MeshRenderer _paintRenderer;
        [SerializeField] private MeshRenderer _leakRenderer;
        
        private AsyncExecutor _asyncExecutor;

        protected override void Awake()
        {
            base.Awake();
            _asyncExecutor = new AsyncExecutor();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _asyncExecutor.Dispose();
        }

        public void SetUp(Vector2Int cell)
        {
            Cell = cell;
        }
        
        protected override void OnClicked()
        {
            Clicked?.Invoke(this);
        }
        
        public Task Fill(Skin skin)
        {
            Skin = skin;
            _paintRenderer.material = skin.Material;
            _leakRenderer.material = skin.Material;
            return PlayFillAnimation();
        }

        public Task Clean()
        {
            Skin = null;
            return PlayCleanAnimation();
        }

        private Task PlayFillAnimation()
        {
            throw new NotImplementedException();
        }

        private Task PlayCleanAnimation()
        {
            throw new NotImplementedException();
        }
    }
}
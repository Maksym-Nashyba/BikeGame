using System;
using System.Threading;
using IGUIDResources;
using Misc;
using UnityEngine;

namespace Menu.Garage.Paint
{
    public class PaintContainer : ClickTarget<GameObject>
    {
        public override event Action<GameObject> Clicked;
        public Skin Skin { get; set; }
        [SerializeField] private GameObject _paintMesh;
        [SerializeField] private GameObject _sprayMesh;
        [SerializeField] private MeshRenderer _paintMeshRenderer;
        [SerializeField] private MeshRenderer _sprayMeshRenderer;
        private AsyncExecutor _asyncExecutor;
        private CancellationToken _cancellationToken;

        protected override void Awake()
        {
            base.Awake();
            _asyncExecutor = new AsyncExecutor();
            _cancellationToken = new CancellationToken();
        }

        public void PlayFillAnimation()
        {
            PlayStrayAnimation();
            PlayPaintAnimation();
        }        
        
        private async void PlayStrayAnimation()
        {
            _sprayMeshRenderer.material = Skin.Material;
            _sprayMesh.SetActive(true);
            await _asyncExecutor.EachFrame(2f, t =>
            {
                _sprayMesh.transform.localScale = Vector3.LerpUnclamped(Vector3.up * 0.1f, new Vector3(0.025f, 0.1f, 0.025f), t);
            }, EaseFunctions.ZeroOneZeroQuad, _cancellationToken);
        }
        
        private void PlayPaintAnimation()
        {
            _paintMeshRenderer.material = Skin.Material;
            Vector3 startPosition = new Vector3(-3.041449f, 2.9f, 0.7072f);
            Vector3 targetPosition = new Vector3(-3.041449f, 2.79f, 0.7072f);
            Vector3 startScale = new Vector3(1f, 0f, 1f);
            PlayAnimation(startPosition, targetPosition, startScale, Vector3.one);
        }
        
        public void PlayCleanAnimation()
        {
            _sprayMesh.SetActive(false);
            Vector3 startPosition = new Vector3(-3.041449f, 2.79f, 0.7072f);
            Vector3 targetPosition = new Vector3(-3.041449f, 2.9f, 0.7072f);
            Vector3 startScale = new Vector3(1f, 1f, 1f);
            PlayAnimation(startPosition, targetPosition, startScale, new Vector3(1f, 0f, 1f));
        }

        private async void PlayAnimation(Vector3 startPosition, Vector3 targetPosition, Vector3 startScale, Vector3 targetScale)
        {
            await _asyncExecutor.EachFrame(2f, t =>
            {
                _paintMesh.transform.localPosition = Vector3.LerpUnclamped(startPosition, targetPosition, t);
                _paintMesh.transform.localScale = Vector3.LerpUnclamped(startScale, targetScale, t);
            }, EaseFunctions.InOutQuad, _cancellationToken);
        }

        protected override void OnClicked()
        {
            Clicked?.Invoke(gameObject.transform.parent.gameObject);
        }
    }
}
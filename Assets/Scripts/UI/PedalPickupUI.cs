using System.Threading.Tasks;
using LevelObjectives.LevelObjects;
using Misc;
using UnityEngine;

namespace UI
{
    public sealed class PedalPickupUI : MonoBehaviour
    {
        [SerializeField] private Animator _uiPedalAnimator;
        [SerializeField] private Transform _uiPedal;
        [SerializeField] private Transform _uiPedalHolder;
        [SerializeField] private GameObject _pedalPrefab;
        private readonly Vector2 ReferenceResolution = new Vector2(2560, 1440);
        private AsyncExecutor _asyncExecutor;
        
        private void Awake()
        {
            _asyncExecutor = new AsyncExecutor();
            ServiceLocator.Pedal.PedalPickedUp += OnPedalPickedUp;
        }

        private void Start()
        {
            _uiPedal.gameObject.SetActive(false);
            _uiPedalAnimator.Play("Awaiting");
            AdjustResolution();
        }

        private void OnDestroy()
        {
            ServiceLocator.Pedal.PedalPickedUp -= OnPedalPickedUp;
            _asyncExecutor.Dispose();
        }
        
        private async void OnPedalPickedUp(Pedal.PedalPickedUpArgs args)
        {
            Transform worldPedal = CreateWorldPedal(args.GFXTransformation);
            await MoveToPoint(worldPedal, _uiPedal, 0.8f);
            Destroy(worldPedal.gameObject);
            _uiPedal.gameObject.SetActive(true);
            _uiPedalAnimator.Play("Collected");
        }

        private Transform CreateWorldPedal(Transformation transformation)
        {
            Transform worldPedal = Instantiate(_pedalPrefab).transform;
            worldPedal.SetPositionAndRotation(transformation.Position, transformation.Rotation);
            worldPedal.localScale = transformation.Scale;
            return worldPedal;
        }

        private Task MoveToPoint(Transform body, Transform targetTransform, float duration)
        {
            body.SetParent(targetTransform.parent);
            Vector3 startPosition = body.position;
            Quaternion startRotation = body.rotation;
            Vector3 startScale = body.localScale;

            return _asyncExecutor.EachFrame(duration, t =>
            {
                Vector3 nextPosition = Vector3.Lerp(startPosition, targetTransform.position, t);
                Quaternion nextRotation = Quaternion.Lerp(startRotation, targetTransform.rotation, t);
                body.localScale = Vector3.Lerp(startScale, targetTransform.localScale, t);
                body.SetPositionAndRotation(nextPosition, nextRotation);
            }, EaseFunctions.Lerp);
        }

        private void AdjustResolution()
        {
            Vector2 ratio = new Vector2(Screen.width, Screen.height) / ReferenceResolution;
            _uiPedalHolder.localScale *= ratio;
        }
    }
}
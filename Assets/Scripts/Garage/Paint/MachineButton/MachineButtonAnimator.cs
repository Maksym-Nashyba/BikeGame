using System.Threading;
using Misc;
using UnityEngine;

namespace Garage.Paint.MachineButton
{
    public class MachineButtonAnimator: MonoBehaviour
    {
        private AsyncExecutor _asyncExecutor;
        private CancellationToken _cancellationToken;

        private void OnEnable()
        {
            _asyncExecutor = new AsyncExecutor();
            _cancellationToken = new CancellationToken();
        }

        public async void ChangeButtonState(ButtonSides buttonSide)
        {
            Quaternion startRotation = transform.localRotation;
            Quaternion targetRotation = Quaternion.AngleAxis((float)buttonSide, Vector3.forward);
            
            await _asyncExecutor.EachFrame(1.2f, t =>
            {
                transform.localRotation = Quaternion.SlerpUnclamped(startRotation, targetRotation, t);
            }, EaseFunctions.InOutQuad, _cancellationToken);
        }

        private void OnDestroy()
        {
            _asyncExecutor.Dispose();
        }
    }

    public enum ButtonSides
    {
        Empty = 0,
        Paint = 90,
        Purchase = 180,
        Confirm = 270
    }
}
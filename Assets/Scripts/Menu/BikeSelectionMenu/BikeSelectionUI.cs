using System.Threading.Tasks;
using Misc;
using UnityEngine;
using UnityEngine.UI;

namespace Menu.BikeSelectionMenu
{
    [RequireComponent(typeof(Animator))]
    public class BikeSelectionUI : MonoBehaviour
    {
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _previousButton;
        [SerializeField] private Button _selectButton;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void SetButtonState(Direction1D buttonSide, bool enabled)
        {
            Button button = buttonSide == Direction1D.Right ? _nextButton : _previousButton;
            button.interactable = enabled;
        }

        public void SetUIState(bool enabled)
        {
            _nextButton.interactable = enabled;
            _previousButton.interactable = enabled;
            _selectButton.interactable = enabled;
        }

        public Task ShowUI()
        {
            _animator.Play("Appear");
            return Task.Delay((int)_animator.GetCurrentAnimatorStateInfo(0).length * 1000);
        }

        public Task HideUI()
        {
            _animator.Play("Hide");
            return Task.Delay((int)_animator.GetCurrentAnimatorStateInfo(0).length * 1000);
        }
    }
}
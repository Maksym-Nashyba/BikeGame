using System;
using Effects.TransitionCover;
using Misc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Menu.Garage.Computer
{
    public class ExitSign : ClickTarget<ExitSign>
    {
        public UnityEvent EditorClicked;
        public override event Action<ExitSign> Clicked;
        [SerializeField] private SceneTransitionCover _transitionCover;
        
        protected override async void OnClicked()
        {
            EditorClicked.Invoke();
            await _transitionCover.TransitionToState(SceneTransitionCover.State.Covered);
            SceneManager.LoadScene("MainMenu");
        }
    }
}
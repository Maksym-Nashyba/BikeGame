using System;
using Effects.TransitionCover;
using Misc;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu.Garage.Computer
{
    public class ExitSign : ClickTarget<ExitSign>
    {
        public override event Action<ExitSign> Clicked;
        [SerializeField] private SceneTransitionCover _transitionCover;
        protected override async void OnClicked()
        {
            await _transitionCover.TransitionToState(SceneTransitionCover.State.Covered);
            SceneManager.LoadScene("MainMenu");
        }
    }
}
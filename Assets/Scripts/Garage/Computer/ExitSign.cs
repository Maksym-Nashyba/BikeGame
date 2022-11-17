using System;
using Misc;
using UnityEngine.SceneManagement;

namespace Garage.Computer
{
    public class ExitSign : ClickTarget<ExitSign>
    {
        public override event Action<ExitSign> Clicked;
        protected override void OnClicked()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
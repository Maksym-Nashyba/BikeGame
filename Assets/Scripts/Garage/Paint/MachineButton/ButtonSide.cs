using System;
using Misc;
using UnityEngine;

namespace Garage.Paint.MachineButton
{
    public class ButtonSide : ClickTarget<ButtonSides>
    {
        public override event Action<ButtonSides> Clicked;
        [SerializeField] private ButtonSides buttonSide;

        protected override void OnClicked()
        {
            Clicked?.Invoke(buttonSide);
        }
    }
}
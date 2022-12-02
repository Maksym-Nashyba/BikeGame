using System;
using Misc;
using UnityEngine;
using UnityEngine.Serialization;

namespace Garage.Paint.MachineButton
{
    public class ButtonSide : ClickTarget<ButtonSides>
    {
        public override event Action<ButtonSides> Clicked;
        [SerializeField] private ButtonSides _buttonSide;

        protected override void OnClicked()
        {
            Clicked?.Invoke(_buttonSide);
        }
    }
}
using Array2DEditor;
using UnityEngine;

namespace Menu.Garage.Paint.Display
{
    [CreateAssetMenu(fileName = "PaintDisplayPatterns", menuName = "ScriptableObjects/PaintDisplayPatterns")]
    public class PaintDisplayPatterns : ScriptableObject
    {
        public Array2DBool SelectionFrame => _selectionFrame;
        [SerializeField] private Array2DBool _selectionFrame;
        public Array2DBool ThreeHundrerBucks => _threeHundrerBucks;
        [SerializeField] private Array2DBool _threeHundrerBucks;
    }
}
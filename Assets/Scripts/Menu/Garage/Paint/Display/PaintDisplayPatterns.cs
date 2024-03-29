﻿using System.Collections.Generic;
using Array2DEditor;
using Misc;
using UnityEngine;

namespace Menu.Garage.Paint.Display
{
    [CreateAssetMenu(fileName = "PaintDisplayPatterns", menuName = "ScriptableObjects/PaintDisplayPatterns")]
    public class PaintDisplayPatterns : ScriptableObject
    {
        private Dictionary<Array2DBool, Pattern> _bakedPatterns = new Dictionary<Array2DBool, Pattern>();
        [SerializeField] private Vector2Int _cellSize;

        public Pattern SelectionFrame => GetBaked(_selectionFrame);
        [SerializeField] private Array2DBool _selectionFrame;
        public Pattern ArrowRight => GetBaked(_arrowRight);
        [SerializeField] private Array2DBool _arrowRight;
        public Pattern ArrowLeft => GetBaked(_arrowLeft);
        [SerializeField] private Array2DBool _arrowLeft;
        public Pattern ArrowDown => GetBaked(_arrowDown);
        [SerializeField] private Array2DBool _arrowDown;

        private Dictionary<char, Pattern> _characterPatterns = new Dictionary<char, Pattern>();
        [SerializeField] private Vector2Int _characterPatternSize;
        #region Characters
        public Pattern Zero => GetBaked(_zero);
        [Space]
        [Header("Numbers")]
        [SerializeField] private Array2DBool _zero;
        public Pattern One => GetBaked(_one);
        [SerializeField] private Array2DBool _one;
        public Pattern Two => GetBaked(_two);
        [SerializeField] private Array2DBool _two;
        public Pattern Three => GetBaked(_three);
        [SerializeField] private Array2DBool _three;
        public Pattern Four => GetBaked(_four);
        [SerializeField] private Array2DBool _four;
        public Pattern Five => GetBaked(_five);
        [SerializeField] private Array2DBool _five;
        public Pattern Six => GetBaked(_six);
        [SerializeField] private Array2DBool _six;
        public Pattern Seven => GetBaked(_seven);
        [SerializeField] private Array2DBool _seven;
        public Pattern Eight => GetBaked(_eight);
        [SerializeField] private Array2DBool _eight;
        public Pattern Nine => GetBaked(_nine);
        [SerializeField] private Array2DBool _nine;

        public Pattern Dollans => GetBaked(_dollans);
        [Header("Chars")]
        [SerializeField] private Array2DBool _dollans;
        
        #endregion

        public void Bake()
        {
            _characterPatterns.Add('0', Zero);
            _characterPatterns.Add('1', One);
            _characterPatterns.Add('2', Two);
            _characterPatterns.Add('3', Three);
            _characterPatterns.Add('4', Four);
            _characterPatterns.Add('5', Five);
            _characterPatterns.Add('6', Six);
            _characterPatterns.Add('7', Seven);
            _characterPatterns.Add('8', Eight);
            _characterPatterns.Add('9', Nine);
            _characterPatterns.Add('$', Dollans);
        }
        
        private Pattern GetBaked(Array2DBool raw)
        {
            if (_bakedPatterns.ContainsKey(raw)) return _bakedPatterns[raw];
            _bakedPatterns.Add(raw, new Pattern(raw.GridSize, raw));
            return _bakedPatterns[raw];
        }
        
        public Pattern FromString(string text)
        {
            Pattern pattern = new Pattern(new Vector2Int(text.Length * _characterPatternSize.x + (text.Length-1), _characterPatternSize.y));
            int xOffset = 0;
            for (var i = 0; i < text.Length; i++)
            {
                char character = text[i];
                pattern.Insert(new Vector2Int(xOffset, 0), _characterPatterns[character]);
                xOffset += _characterPatternSize.x + 1;
            }
            return pattern;
        }
        
        public Pattern BuildPricePattern(uint price, Direction1D direction)
        {
            Pattern result = new Pattern(_cellSize);
            Pattern arrowPattern = direction == Direction1D.Left ? ArrowLeft : ArrowRight;
            Pattern priceText = FromString($"{price}$");
            Vector2Int priceTextLocation = new Vector2Int((_cellSize.x-priceText.Size.x)/2, _cellSize.y/2-priceText.Size.y);
            result.Insert(priceTextLocation, priceText);
            Vector2Int arrowPatternLocation = new Vector2Int((_cellSize.x-arrowPattern.Size.x)/2, (int)(_cellSize.y - (1.5f * arrowPattern.Size.y)));
            result.Insert(arrowPatternLocation, arrowPattern);
            return result;
        }
    }
}
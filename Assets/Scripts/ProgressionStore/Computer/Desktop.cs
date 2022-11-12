using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProgressionStore.Computer
{
    public class Desktop : MonoBehaviour
    {
        [SerializeField] private ComputerUI _computerUI;
        [SerializeField] private GameObject _iconPrefab;
        [SerializeField] private Transform _iconsHolder;

        private Dictionary<Program, DesktopIcon> _icons;

        private void Awake()
        {
            _icons = new Dictionary<Program, DesktopIcon>();
        }

        private void Start()
        {
            foreach (Program program in _computerUI.ProgramsCopy)
            {
                CreateIcon(program);
            }
        }

        private void CreateIcon(Program program)
        {
            if (_icons.ContainsKey(program)) throw new InvalidOperationException("Icon for this program already exists");

            DesktopIcon icon = Instantiate(_iconPrefab, _iconsHolder).GetComponent<DesktopIcon>();
            icon.SetUp(program);
            icon.Clicked += _computerUI.Launch;
        }

        private void OnDestroy()
        {
            foreach (Program program in _icons.Keys)
            {
                _icons[program].Clicked -= _computerUI.Launch;
            }
        }
    }
}
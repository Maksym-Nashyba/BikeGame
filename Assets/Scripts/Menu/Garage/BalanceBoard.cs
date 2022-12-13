using SaveSystem.Front;
using TMPro;
using UnityEngine;

namespace Menu.Garage
{
    public class BalanceBoard : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _balanceOnBoard;
        private Saves _saves;

        private void Awake()
        {
            _saves = FindObjectOfType<Saves>();
            _saves.Currencies.Changed += OnCurrenciesChanged;
        }

        private void Start()
        {
            OnCurrenciesChanged();
        }

        private void OnCurrenciesChanged()
        {
            _balanceOnBoard.text = _saves.Currencies.GetDollans() + "$";
        }

        private void OnDestroy()
        {
            _saves.Currencies.Changed -= OnCurrenciesChanged;
        }
    }
}
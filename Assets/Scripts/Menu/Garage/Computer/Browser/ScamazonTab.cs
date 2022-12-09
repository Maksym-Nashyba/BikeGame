using IGUIDResources;
using SaveSystem.Front;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu.Garage.Computer.Browser
{
    public class ScamazonTab : MonoBehaviour
    {
        [SerializeField] private BikeModel _bike;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private Button _buyButton;
        [SerializeField] private GameObject _soldOutText;
        private Saves _saves;

        private void Awake()
        {
            _saves = FindObjectOfType<Saves>();
        }

        private void Start()
        {
            SetUpUI(BikeBought());
        }

        public void OnBuyButton()
        {
            if(BikeBought()) return;

            if (TryBuy())
            {
                SetUpUI(true);
            }
            else
            {
                FindObjectOfType<Computer>().ShowDialog($"YOU_ARE_TOO_POOR\nBALANCE_{_saves.Currencies.GetDollans()}");
            }
        }

        private void SetUpUI(bool bikeBought)
        {
            _priceText.SetText($"{_bike.Cost}$");
            _nameText.SetText($"{_bike.Name}");
            _buyButton.interactable = !bikeBought;
            _soldOutText.SetActive(bikeBought);
        }

        private bool TryBuy()
        {
            if (_saves.Currencies.GetDollans() < _bike.Cost) return false;
            
            _saves.Currencies.SubtractDollans(_bike.Cost);
            _saves.Bikes.UnlockBike(_bike.GetGUID());
            return true;
        }

        private bool BikeBought()
        {
            return _saves.Bikes.IsBikeUnlocked(_bike);
        }
    }
}
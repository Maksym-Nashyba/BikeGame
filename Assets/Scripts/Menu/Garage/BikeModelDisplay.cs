using Effects;
using Garage;
using IGUIDResources;
using UnityEngine;

namespace Menu.Garage
{
    public class BikeModelDisplay : MonoBehaviour
    {
        public BikeModel CurrentBike { get; private set; }
        public GarageBikeModelHolder Holder => _holder;
        [SerializeField] private GarageBikeModelHolder _holder;
        private GameObject _currentDisplayModel;

        public void Display(BikeModel bike, Skin skin)
        {
            CleanHolder();
            CurrentBike = bike;
            _currentDisplayModel = Instantiate(bike.EmptyPrefab, _holder.HolderTransform);
            ApplySkin(skin);
        }

        public void ApplySkin(Skin skin)
        {
            _currentDisplayModel.GetComponent<BikeSkinApplier>().ApplySkin(skin);
        }

        private void CleanHolder()
        {
            for (int i = _holder.HolderTransform.childCount -1; i >= 0; i--)
            {
                Destroy(_holder.HolderTransform.GetChild(i).gameObject);
            }            
        }
    }
}
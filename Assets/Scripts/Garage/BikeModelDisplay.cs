using IGUIDResources;
using Misc;
using UnityEngine;

namespace Garage
{
    public class BikeModelDisplay : MonoBehaviour
    {
        public GarageBikeModelHolder Holder => _holder;
        [SerializeField] private GarageBikeModelHolder _holder;
        private GameObject _currentDisplayModel;

        public void Display(BikeModel bike, Skin skin)
        {
            CleanHolder();
            _currentDisplayModel = Instantiate(bike.EmptyPrefab, _holder.HolderTransform);
            ApplySkin(skin);
        }

        public void ApplySkin(Skin skin)
        {
            for (int i = _currentDisplayModel.transform.childCount -1; i >= 0; i--)
            {
                if (_currentDisplayModel.transform.GetChild(i).TryGetComponent(out MeshRenderer renderer))
                {
                    renderer.material = skin.Material;
                }
            }  
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
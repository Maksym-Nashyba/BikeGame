using IGUIDResources;
using ProgressionStore.PaintShop;
using UnityEngine;

namespace ProgressionStore
{
    public class BikeDisplay : MonoBehaviour
    {
        [SerializeField] private Transform _bikeSpawnPoint;
        [SerializeField] private Transform _bikePaintPoint;
        [SerializeField] private GameObject _bikeParentGameObject;
        private GameObject _currentBike;
        private Garage _garage;
        private PaintGarageShop _paintGarageShop;
        private GUIDResourceLocator _resourceLocator;

        private void Awake()
        { 
            _garage = FindObjectOfType<Garage>();
            _paintGarageShop = FindObjectOfType<PaintGarageShop>();
            _garage.NewBikeSelected += Show;
            _garage.NewSkinSelected += ChangeSkin;
            _paintGarageShop.Opened += MoveBikeToVendingMachine;
        }

        private void Show(BikeModel bikeModel)
        {
            if(_currentBike is not null) Destroy(_currentBike); 
            _currentBike = Instantiate(bikeModel.Prefab, _bikeSpawnPoint.position, _bikeSpawnPoint.rotation, _bikeParentGameObject.transform);
            _currentBike.transform.localScale = new Vector3(1f,1f,1f);
        }

        private void ChangeSkin(Skin skin)
        {
            MeshRenderer[] bikeMeshRenderers = _currentBike.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < bikeMeshRenderers.Length; i++)
            {
                bikeMeshRenderers[i].material = skin.Material;
            }
        }

        private void MoveBikeToVendingMachine()
        {
            float distance = (_bikePaintPoint.position - _currentBike.transform.position).magnitude;
            while (distance > 0.2f)
            {
                _currentBike.transform.position = Vector3.Lerp(_currentBike.transform.position, _bikePaintPoint.position, 0.1f);
                distance = (_bikePaintPoint.position - _currentBike.transform.position).magnitude;
            }
        }
            

        private void OnDestroy()
        {
            _garage.NewBikeSelected -= Show;
            _garage.NewSkinSelected -= ChangeSkin;
            _paintGarageShop.Opened -= MoveBikeToVendingMachine;
        }
    }
}
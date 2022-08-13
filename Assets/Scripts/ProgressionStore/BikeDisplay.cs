using System;
using IGUIDResources;
using UnityEditor;
using UnityEngine;

namespace ProgressionStore
{
    public class BikeDisplay : MonoBehaviour
    {
        [SerializeField] private Transform _bikeSpawnPoint;
        private GameObject _currentBike;
        private Garage _garage;
        private GUIDResourceLocator _resourceLocator;

        private void Awake()
        { 
            _garage = FindObjectOfType<Garage>();
            _garage.NewBikeSelected += Show;
            _garage.NewSkinSelected += ChangeSkin;
        }

        private void Show(BikeModel bikeModel)
        {
            if(_currentBike is not null) Destroy(_currentBike); 
            _currentBike = Instantiate(bikeModel.Prefab,_bikeSpawnPoint.position,_bikeSpawnPoint.rotation);
            _currentBike.transform.localScale = new Vector3(4.07f,4.07f,4.07f);
        }

        private void ChangeSkin(Skin skin)
        {
            MeshRenderer[] bikeMeshRenderers = _currentBike.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < bikeMeshRenderers.Length; i++)
            {
                bikeMeshRenderers[i].material = skin.Material;
            }
            
        }

        private void OnDestroy()
        {
            _garage.NewBikeSelected -= Show;
            _garage.NewSkinSelected -= ChangeSkin;
        }
    }
}
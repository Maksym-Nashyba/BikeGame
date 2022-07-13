using UnityEngine;

namespace IGUIDResources
{
    [CreateAssetMenu(fileName = "newBikeModel", menuName = "ScriptableObjects/Bikes/Model")]
    public class BikeModel : ScriptableObject, IGUIDResource
    {
        public GameObject Prefab;
        public Skin[] AllSkins;
        public float stat1;
        public float stat2;
        public float stat3;
        
        public string GetGUID()
        {
            return name;
        }
    }
}
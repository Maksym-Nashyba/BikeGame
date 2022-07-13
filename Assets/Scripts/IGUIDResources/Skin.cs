using UnityEngine;

namespace IGUIDResources
{
    [CreateAssetMenu(fileName = "newSkin", menuName = "ScriptableObjects/Bikes/Skin")]
    public class Skin : ScriptableObject, IGUIDResource
    {
        public Material Material;
        
        public string GetGUID()
        {
            return name;
        }
    }
}
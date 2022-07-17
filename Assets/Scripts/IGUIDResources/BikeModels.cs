using System.Linq;
using UnityEngine;

namespace IGUIDResources
{
    [CreateAssetMenu(fileName = "newBikeModels", menuName = "ScriptableObjects/Bikes/List")]
    public class BikeModels : ScriptableObject
    {
        public BikeModel[] Models;

        public BikeModel Get(string GUID)
        {
            return Models.First(model => model.GetGUID() == GUID);
        }
        
        public BikeModel GetDefault()
        {
            return Models[0];
        }
    }
}
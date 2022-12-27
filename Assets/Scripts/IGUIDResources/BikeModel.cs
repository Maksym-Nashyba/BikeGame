using SaveSystem.Models;
using UnityEngine;

namespace IGUIDResources
{
    [CreateAssetMenu(fileName = "newBikeModel", menuName = "ScriptableObjects/Bikes/Model")]
    public class BikeModel : ScriptableObject, IGUIDResource
    {
        public string Name;
        public GameObject Prefab;
        public GameObject EmptyPrefab;
        public Skin[] AllSkins;
        public int Cost;
        public float stat1;
        public float stat2;
        public float stat3;

        public string GetGUID()
        {
            return name;
        }

        public Skin GetSkinFor(string GUID)
        {
            Skin selectedSkin = AllSkins[0];
            foreach (Skin skin in AllSkins)
            {
                if (skin.GetGUID() == GUID) selectedSkin = skin;
            }

            return selectedSkin;
        }

        public PersistentBike MakeCleanSaveObject()
        {
            return new PersistentBike(
                AllSkins[0].GetGUID(),
                new []{AllSkins[0].GetGUID()},
                GetGUID());
        }
    }
}
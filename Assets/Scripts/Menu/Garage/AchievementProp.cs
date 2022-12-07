using IGUIDResources;
using SaveSystem.Front;
using UnityEngine;

namespace Menu.Garage
{
    public class AchievementProp : MonoBehaviour
    {
        [SerializeField] private Level _level;

        private void Start()
        {
            Saves saves = FindObjectOfType<Saves>();
            if(!saves.Career.IsCompleted(_level)) Destroy(gameObject); 
        }
    }
}
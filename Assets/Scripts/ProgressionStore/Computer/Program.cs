using UnityEngine;

namespace ProgressionStore.Computer
{
    [CreateAssetMenu(fileName = "FILENAME", menuName = "ScriptableObjects/Garage/ComputerProgram")]
    public class Program : ScriptableObject
    {
        public string PresentableName => _presentableName;
        [SerializeField] private string _presentableName;
        
        public Sprite TaskBarProcessSprite => _taskBarProcessSprite;
        [SerializeField] private Sprite _taskBarProcessSprite;
    }
}
using System.Threading.Tasks;
using SaveSystem.Front;
using UnityEngine;

namespace SetUp
{
    public class SaveSystemSetUp : MonoBehaviour
    {
        [SerializeField] private SavesInitializer _savesInitializer;

        private void Awake()
        {
            FindObjectOfType<GameSetUp>().RegisterSetUpTask(SetUpSaves());
        }

        private Task SetUpSaves()
        {
            return _savesInitializer.InitializeOnDemand();
        }
    }
}
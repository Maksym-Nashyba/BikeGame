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
            SetUpOperation setUpOperation = new SetUpOperation(SetUpSaves, "Saves Loaded", true);
            FindObjectOfType<GameSetUp>().RegisterSetUpTask(setUpOperation);
        }

        private Task SetUpSaves()
        {
            return _savesInitializer.InitializeOnDemand();
        }
    }
}
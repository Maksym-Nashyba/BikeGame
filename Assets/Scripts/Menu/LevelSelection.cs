using System.Threading.Tasks;
using IGUIDResources;
using SaveSystem.Front;
using SaveSystem.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class LevelSelection : MonoBehaviour
    {
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private GameObject[] _levelGameObjects;
        private Saves _saves;
        private GUIDResourceLocator _resourceLocator;
        private Career _career;
        private PersistentLevel[] _persistentLevels;
        private Level _currentLevel;
        private TaskCompletionSource<Level> _taskCompletionSource;

        private void Awake()
        {
            _saves = FindObjectOfType<Saves>();
            _persistentLevels = _saves.Career.GetAllCompletedLevels();
            _resourceLocator = GUIDResourceLocator.Initialize();
            _career = _resourceLocator.Career;
        }

        private void Start()
        {
            DisplayLevel(_persistentLevels[0]);
        }

        private void DisplayLevel(PersistentLevel persistentLevel)
        {
            TransformCamera(_lineRenderer.GetPosition(1), _levelGameObjects[1].transform);
            _currentLevel = _career.GetLevelWithGUID(persistentLevel.GUID);
        }

        private void TransformCamera(Vector3 linePointPosition, Transform levelTransform)
        {
            _cameraTransform.position = _lineRenderer.transform.position + linePointPosition;
            _cameraTransform.LookAt(levelTransform);
        }

        public void SelectLevel()
        {
            _taskCompletionSource.SetResult(_currentLevel);
        }
        
        public static async Task<LevelSelection> DisplayLevelSelection()
        {
            AsyncOperation loadingProcess = SceneManager.LoadSceneAsync("LevelSelection", LoadSceneMode.Additive);
            while (!loadingProcess.isDone)
            {
                await Task.Yield();
            }
            return FindObjectOfType<LevelSelection>();
        }
    }
}
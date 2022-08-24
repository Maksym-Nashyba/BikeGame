using System.Threading.Tasks;
using IGUIDResources;
using SaveSystem.Front;
using SaveSystem.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu
{
    public class LevelSelection : MonoBehaviour
    {
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _previousButton;
        
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private GameObject[] _levelGameObjects;

        private int _currentLevelNumber;
        private Vector3 _currentPointPosition;
        private Vector3 _currentLevelPosition;
        
        private Saves _saves;
        private Career _career;
        private Level _currentLevel;
        private GUIDResourceLocator _resourceLocator;
        private PersistentLevel[] _persistentLevels;
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
            DisplayLevel();
        }

        private void Update()
        {
            UpdateButtonsStatus(_currentLevelNumber, _persistentLevels.Length);
        }

        private void FixedUpdate()
        {
            _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, _currentPointPosition, 0.03f);
            _cameraTransform.LookAt(_currentLevelPosition);
        }
        
        private void UpdateButtonsStatus(int levelNumber, int levelsCount)
        {
            _nextButton.interactable = levelNumber < levelsCount - 1;
            _previousButton.interactable = levelNumber > 0;
        }
        
        public void ChangeLevel(bool nextOrPrevious) 
        {
            _currentLevelNumber = nextOrPrevious ? ++_currentLevelNumber : --_currentLevelNumber;
            DisplayLevel();
        }
        
        private void DisplayLevel()
        {
            UpdatePointInfo(_currentLevelNumber);
            UpdatePickedLevel(_persistentLevels[_currentLevelNumber]);
        }
        
        private void UpdatePointInfo(int levelNumber)
        {
            _currentPointPosition = _lineRenderer.GetPosition(levelNumber);
            _currentLevelPosition = _levelGameObjects[levelNumber].transform.position;
        }
        
        private void UpdatePickedLevel(PersistentLevel persistentLevel)
        {
            _currentLevel = _career.GetLevelWithGUID(persistentLevel.GUID);
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
        
        public void RegisterTaskCompletionSource(TaskCompletionSource<Level> taskCompletionSource)
        {
            _taskCompletionSource = taskCompletionSource;
        }
    }
}
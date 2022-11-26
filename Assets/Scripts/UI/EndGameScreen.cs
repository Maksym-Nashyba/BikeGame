using System;
using System.Threading.Tasks;
using GameCycle;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class EndGameScreen : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _timeValueText;
        [SerializeField] private TextMeshProUGUI _scoreValueText;
        [SerializeField] private TextMeshProUGUI _fallCountValueText;
        [SerializeField] private Button _continueButton;

        private void Start()
        {
            _continueButton.interactable = false;
        }

        public void OnContinueButton()
        {
            SceneManager.LoadScene("MainMenu");
        }
        
        public async Task Show(CareerLevelAchievements levelAchievements)
        {
            await ShowScoreCount(levelAchievements);
            _continueButton.interactable = transform;
        }

        private async Task ShowScoreCount(CareerLevelAchievements levelAchievements)
        {
            await ExecuteOverTime(1f, async t =>
            {
                DisplayTimeCount(t);
            });
        }

        private void DisplayTimeCount(float t)
        {
            throw new NotImplementedException();
        }

        private async Task ExecuteOverTime(float seconds, Action<float> action)
        {
            float timeElapsed = 0f;
            while (timeElapsed < seconds)
            {
                await Task.Yield();
                action.Invoke(timeElapsed/seconds);
                timeElapsed += Time.deltaTime;
            }
        }
    }
}
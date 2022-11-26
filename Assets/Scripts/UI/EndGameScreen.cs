using System;
using System.Threading.Tasks;
using GameCycle;
using Misc;
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
        
        public async Task Show(ScoreCount scoreCount)
        {
            await ShowScoreCount(scoreCount);
            _scoreValueText.SetText($"{scoreCount.Score}");
            _timeValueText.SetText(Format.FormatSeconds(scoreCount.TimeSeconds));
            _continueButton.interactable = true;
        }

        private async Task ShowScoreCount(ScoreCount scoreCount)
        {
            await ExecuteOverTime(10f, t =>
            {
                DisplayTimeCount(t, scoreCount.TimeSeconds, scoreCount.ExpectedTimeSeconds);
            });
        }

        private void DisplayTimeCount(float t, int playerTimeSeconds, int expectedTimeSeconds)
        {
            //TODO math is fucking broken
            float secondsPassed = playerTimeSeconds * t;
            float value = ScoreCount.BaseScore + (expectedTimeSeconds - secondsPassed) / expectedTimeSeconds * 100f * ScoreCount.OnePercentTimeCost;
            _scoreValueText.SetText($"{value}");
            _timeValueText.SetText(Format.FormatSeconds((int)secondsPassed));
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
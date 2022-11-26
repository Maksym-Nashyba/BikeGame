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
        [SerializeField] private TextMeshProUGUI _expectedTimeValueText;
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
            _timeValueText.transform.parent.gameObject.SetActive(false);
            _fallCountValueText.transform.parent.gameObject.SetActive(false);
            await ShowScoreCount(scoreCount);
            _scoreValueText.SetText($"{scoreCount.Score}");
            _timeValueText.SetText(Format.FormatSeconds(scoreCount.TimeSeconds));
            _continueButton.interactable = true;
        }

        private async Task ShowScoreCount(ScoreCount scoreCount)
        {
            _timeValueText.transform.parent.gameObject.SetActive(true);
            _expectedTimeValueText.SetText(Format.FormatSeconds(scoreCount.ExpectedTimeSeconds));
            await ExecuteOverTime(10f, t =>
            {
                DisplayTimeCount(t, scoreCount.TimeSeconds, scoreCount.ExpectedTimeSeconds);
            });
        }

        private void DisplayTimeCount(float t, int playerTimeSeconds, int expectedTimeSeconds)
        {
            float secondsPassed = playerTimeSeconds * t;
            float minValue = ScoreCount.BaseScore - Mathf.Abs(0 - expectedTimeSeconds) * ScoreCount.SecondsTimeCost;
            float maxValue = ScoreCount.BaseScore - Mathf.Abs(playerTimeSeconds - expectedTimeSeconds) * ScoreCount.SecondsTimeCost;
            float value = ScoreCount.BaseScore - Mathf.Abs(secondsPassed - expectedTimeSeconds) * ScoreCount.SecondsTimeCost;
            value = value.Remap(minValue, maxValue, 0f, maxValue);
            _scoreValueText.SetText($"{(int)value}");
            _timeValueText.SetText(Format.FormatSeconds((int)secondsPassed));
            _timeValueText.color = expectedTimeSeconds > secondsPassed ? Color.green : Color.red;
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
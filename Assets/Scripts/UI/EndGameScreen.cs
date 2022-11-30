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
        [SerializeField] private AnimatableText _scoreValueText;
        [SerializeField] private AnimatableText _fallCountValueText;
        [SerializeField] private TextMeshProUGUI _expectedTimeValueText;
        [SerializeField] private Button _continueButton;
        private AsyncExecutor _executor;
        private int _displayedScore;
        private int _displayedFallCount;

        private void Awake()
        {
            _executor = new AsyncExecutor();
        }

        private void Start()
        {
            _continueButton.interactable = false;
        }

        private void OnDestroy()
        {
            _executor.Dispose();
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
            await Task.Delay(1000);
            await ShowFallCount(scoreCount);

            _continueButton.interactable = true;
        }

        private async Task ShowScoreCount(ScoreCount scoreCount)
        {
            _timeValueText.transform.parent.gameObject.SetActive(true);
            _expectedTimeValueText.SetText(Format.FormatSeconds(scoreCount.ExpectedTimeSeconds));
            await _executor.EachFrame(5f, t =>
            {
                UpdateTimeCount(t, scoreCount.TimeSeconds, scoreCount.ExpectedTimeSeconds);
            });
        }
        
        private void UpdateTimeCount(float t, int playerTimeSeconds, int expectedTimeSeconds)
        {
            float secondsPassed = playerTimeSeconds * t;
            float minValue = ScoreCount.BaseScore - Mathf.Abs(0 - expectedTimeSeconds) * ScoreCount.SecondsTimeCost;
            float maxValue = ScoreCount.BaseScore - Mathf.Abs(playerTimeSeconds - expectedTimeSeconds) * ScoreCount.SecondsTimeCost;
            float value = ScoreCount.BaseScore - Mathf.Abs(secondsPassed - expectedTimeSeconds) * ScoreCount.SecondsTimeCost;
            
            ApplyScoreValue((int)value.Remap(minValue, maxValue, 0f, maxValue));
            _timeValueText.SetText(Format.FormatSeconds((int)secondsPassed));
            _timeValueText.color = expectedTimeSeconds > secondsPassed ? Color.green : Color.red;
        }

        private async Task ShowFallCount(ScoreCount scoreCount)
        {
            _fallCountValueText.Text.SetText("0");
            _fallCountValueText.transform.parent.gameObject.SetActive(true);
            for (int i = 0; i < scoreCount.Falls; i++)
            {
                ApplyFallCountIncrement();
                ApplyScoreValue(_displayedScore - ScoreCount.FallCost, true);
                await _fallCountValueText.Kick(Color.red);
                _fallCountValueText.ReturnToDefault();
                await Task.Delay(500);
            }
        }

        private async Task ApplyScoreValue(int nextValue, bool withShake = false)
        {
            Color color;
            if (nextValue == _displayedScore) color = _scoreValueText.Text.color;
            else color = nextValue >= _displayedScore ? Color.green : Color.red;
            
            _scoreValueText.Text.color = color;
            _displayedScore = nextValue;
            _scoreValueText.Text.SetText($"{_displayedScore}");
            if (withShake) await _scoreValueText.Kick(color);
            _scoreValueText.ReturnToDefault();
        }

        private void ApplyFallCountIncrement()
        {
            _displayedFallCount++;
            _fallCountValueText.Text.SetText($"{_displayedFallCount}");
        }
    }
}
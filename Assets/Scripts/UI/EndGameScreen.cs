using System.Threading.Tasks;
using Effects.TransitionCover;
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
        [SerializeField] private SceneTransitionCover _blackoutTransitionCover;
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

        public async void OnContinueButton()
        {
            await _blackoutTransitionCover.TransitionToState(SceneTransitionCover.State.Covered);
            SceneManager.LoadScene("MainMenu");
        }
        
        public async Task Show(LevelAchievements achievements)
        {
            _timeValueText.transform.parent.gameObject.SetActive(false);
            _fallCountValueText.transform.parent.gameObject.SetActive(false);
            
            await ShowScoreCount(achievements);
            await Task.Delay(1000);
            await ShowFallCount(achievements);

            ApplyScoreValue(achievements.FinalScore);
            _continueButton.interactable = true;
        }

        private async Task ShowScoreCount(LevelAchievements achievements)
        {
            _timeValueText.transform.parent.gameObject.SetActive(true);
            _expectedTimeValueText.SetText(Format.FormatSeconds(achievements.ExpectedTimeSeconds));
            await _executor.EachFrame(5f, t =>
            {
                UpdateTimeCount(t, achievements.TimeSeconds, achievements.ExpectedTimeSeconds);
            });
        }
        
        private void UpdateTimeCount(float t, int playerTimeSeconds, int expectedTimeSeconds)
        {
            float secondsPassed = playerTimeSeconds * t;
            ApplyScoreValue(CalculateScore(t, playerTimeSeconds, expectedTimeSeconds));
            _timeValueText.SetText(Format.FormatSeconds((int)secondsPassed));
            _timeValueText.color = expectedTimeSeconds > secondsPassed ? Color.green : Color.red;
        }

        private int CalculateScore(float t, int playerTimeSeconds, int expectedTimeSeconds)
        {
            float breakTime = Mathf.Clamp01(expectedTimeSeconds / (float)playerTimeSeconds);
            float realScore = ScoreForTime(playerTimeSeconds, expectedTimeSeconds);

            if (playerTimeSeconds < expectedTimeSeconds)
            {
                return (int)Mathf.Lerp(0, ScoreForTime(playerTimeSeconds, expectedTimeSeconds), t);
            }
            if (t < breakTime)
            {
                return (int)Mathf.Lerp(0, LevelAchievements.BaseScore, t / breakTime);
            }
            else
            {
                return (int)Mathf.Lerp(LevelAchievements.BaseScore, realScore, (t - breakTime) /(1f-breakTime));
            }

            float ScoreForTime(float real, float expected)
            {
                return LevelAchievements.BaseScore + (expected - real) * LevelAchievements.SecondTimeCost;
            }
        }

        private async Task ShowFallCount(LevelAchievements achievements)
        {
            _fallCountValueText.Text.SetText("0");
            _fallCountValueText.transform.parent.gameObject.SetActive(true);
            for (int i = 0; i < achievements.FallCount; i++)
            {
                ApplyFallCountIncrement();
                ApplyScoreValue(_displayedScore - LevelAchievements.FallCost, true);
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
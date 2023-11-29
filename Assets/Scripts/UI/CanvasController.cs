using System.Collections;
using TMPro;
using UnityEngine;

namespace GamePatron.IndividualGames.UI
{
    /// <summary>
    /// Controls canvas elements.
    /// Contains Actions for game events regarding the UI.
    /// Handles the countdown.
    /// </summary>
    public class CanvasController : MonoBehaviour
    {
        public static CanvasController Instance;
        private int _currentScore;
        private int _countdown = 60;

        private WaitForSeconds _tickWait = new(1);

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            GameManagerScript.Instance.GameWon += GameWin;
            GameManagerScript.Instance.GameLost += GameLost;
            GameManagerScript.Instance.PointGained += OnScoreChanged;

            StartCoroutine(Tick());
        }

        [SerializeField] private GameObject _gamewin;
        [SerializeField] private GameObject _gamelose;
        [SerializeField] private GameObject _gamedata;
        [SerializeField] private TextMeshProUGUI _scoreLabel;
        [SerializeField] private TextMeshProUGUI _countdownLabel;

        /// <summary> Game is won. </summary>
        private void GameWin()
        {
            _gamewin.SetActive(true);
            _gamedata.SetActive(false);
        }

        /// <summary> Game is lost. </summary>
        private void GameLost()
        {
            _gamelose.SetActive(true);
            _gamedata.SetActive(false);
        }

        /// <summary> Score is changed in UI. </summary>
        private void OnScoreChanged(int addScore)
        {
            _currentScore += addScore;
            _scoreLabel.text = _currentScore.ToString();
        }

        /// <summary> Tick each second for countdown. </summary>
        private IEnumerator Tick()
        {
            while (_countdown > 0)
            {
                _countdown--;
                _countdownLabel.text = _countdown.ToString();
                yield return _tickWait;
            }
            GameLost();
        }

        /// <summary> Quit game on quit button clicked. </summary>
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
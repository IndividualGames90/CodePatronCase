using System.Collections;
using TMPro;
using UnityEngine;

namespace GamePatron.IndividualGames.UI
{
    /// <summary>
    /// Controls canvas elements.
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

        private void GameWin()
        {
            _gamewin.SetActive(true);
            _gamedata.SetActive(false);
        }

        private void GameLost()
        {
            _gamelose.SetActive(true);
            _gamedata.SetActive(false);
        }

        private void OnScoreChanged(int addScore)
        {
            _currentScore += addScore;
            _scoreLabel.text = _currentScore.ToString();
        }

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
    }
}
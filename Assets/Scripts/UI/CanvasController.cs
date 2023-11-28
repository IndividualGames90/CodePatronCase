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

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            GameManagerScript.Instance.GameWon += GameWin;
            GameManagerScript.Instance.GameLost += GameLost;
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
    }
}
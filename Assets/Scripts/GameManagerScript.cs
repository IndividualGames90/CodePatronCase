using GamePatron.IndividualGames.ScriptableObjects;
using System;
using UnityEngine;

/// <summary>
/// Controls game state.
/// </summary>
public class GameManagerScript : MonoBehaviour
{
    public Points Points => _points;
    [SerializeField] private Points _points;

    public static GameManagerScript Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManagerScript>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("GameManager");
                    _instance = go.AddComponent<GameManagerScript>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }
    private static GameManagerScript _instance;

    public Action GameWon;
    public Action GameLost;
    public Action<int> PointGained;

    private void Awake()
    {
        if (Instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    /// <summary> Game is won. </summary>
    public void OnVictory()
    {
        GameWon?.Invoke();
    }

    /// <summary> Game is lost. </summary>
    public void OnLost()
    {
        GameLost?.Invoke();
    }

    /// <summary> Player gained points. </summary>
    public void OnPointGained(int point)
    {
        PointGained?.Invoke(point);
    }
}

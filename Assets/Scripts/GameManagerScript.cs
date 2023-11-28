using GamePatron.IndividualGames.ScriptableObjects;
using System;
using UnityEngine;

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

    public void OnVictory()
    {
        GameWon?.Invoke();
    }

    public void OnLost()
    {
        GameLost?.Invoke();
    }

    public void OnPointGained(int point)
    {
        PointGained?.Invoke(point);
    }
}

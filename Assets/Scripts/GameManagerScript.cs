using System;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
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

    private void Awake()
    {
        if (Instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void Victory()
    {
        GameWon?.Invoke();
    }

    public void Lost()
    {
        GameLost?.Invoke();
    }
}

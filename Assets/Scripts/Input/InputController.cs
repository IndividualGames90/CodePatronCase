using UnityEngine;

public class InputController : MonoBehaviour
{
    /// <summary>
    /// Access to player input.
    /// </summary>
    public static PlayerInput Input
    {
        get
        {
            if (_input == null)
            {
                _input = new();//CEM: Access initialization.
                _input.Enable();
            }
            return _input;
        }
    }
    private static PlayerInput _input;

    private void Awake()
    {
        _input = new();//CEM: Initialize in awake.
        _input.Enable();
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }
}

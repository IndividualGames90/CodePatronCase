using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _healthLabel;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _moveDistance = 5;
    [SerializeField] private float _moveStep = .1f;

    private float _currentMoveDistance;
    private int _health = 100;
    private const int _damage = 25;

    private bool _moveDirection = false;
    private Vector3 _initialDirection;
    private Vector3 _reverseDirection;

    private void Awake()
    {
        _initialDirection = transform.forward;
        _reverseDirection = transform.forward;

        _animator.SetFloat("Blend", 1f);

        _currentMoveDistance = _moveDistance;
    }

    private void Update()
    {
        float newX;
        if (_moveDirection)
        {
            newX = transform.position.x + _moveSpeed;
        }
        else
        {
            newX = transform.position.x - _moveSpeed;
        }
        _currentMoveDistance -= _moveStep;

        float newY = transform.position.y;
        float newZ = transform.position.z;
        Vector3 newPosition = new Vector3(newX, newY, newZ);
        transform.position = newPosition;

        if (_currentMoveDistance <= 0)
        {
            _currentMoveDistance = _moveDistance;
            _moveDirection = !_moveDirection;
        }
    }

    public void DamageNPC()
    {
        _health -= _damage;
        _healthLabel.text = _health.ToString();

        if (_health <= 0)
        {
            Killed();
        }
    }

    private void Killed()
    {
        GameManagerScript.Instance.OnPointGained(GameManagerScript.Instance.Points.Enemy);
        Destroy(gameObject);
    }
}

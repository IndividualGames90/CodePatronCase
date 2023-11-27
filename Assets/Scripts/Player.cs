using GamePatron.IndividualGames.Unity;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [Header("Related Components")]
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private CharacterController characterController; //Contoller componenti
    [SerializeField] private Animator _animator; //Animator componenti
    [SerializeField] private GameObject KameraObjesi; //Kamera objesi (Kamera oyuncunun sað ve sol hareketlerine göre takip edecek. Sahnedeki kamera transformunun tüm deðerleri sabit kalacak, yalnýzca position.x deðeri karakterin position.x deðerini takip edecek.)
    [SerializeField] private GameObject mermiPrefab; //Duvar ya da npc objelerine týklandýðýnda spawnlanacak olan mermi prefabý
    [SerializeField] private Transform _cameraLookAt;
    [SerializeField] private Transform _playerMesh;

    [Header("Player Data")]
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _moveSpeedMax = 10f;
    [SerializeField] private float _gravityValue = -1f;

    private bool _grounded;
    private const float _bottomDistance = .2f;
    private const float _slowDownFactor = .1f;

    private Vector2 _movement => InputController.Input.PlayerControls.Movement.ReadValue<Vector2>();

    private bool _jumpLocked;
    private bool _jumped;
    private bool _shooting;

    private Vector3 _initialForward;
    private Vector3 _initialBackward;
    private Vector3 _playerVelocity;

    private Vector3 _bottomPosition;
    private Vector3 _cameraPlayerDistance;

    private RaycastHit _hit;
    private Camera _mainCamera;

    private WaitForEndOfFrame _jumpWait;

    // Use this for initialization
    void Start()
    {
        _mainCamera = Camera.main;
        _cameraPlayerDistance = _mainCamera.transform.position - transform.position;
        _mainCamera.transform.LookAt(_cameraLookAt.position);

        _initialForward = transform.forward;
        _initialBackward = -transform.forward;

        InputController.Input.PlayerControls.Jump.started += (C) => { _jumped = true; };
        InputController.Input.PlayerControls.Shoot.started += (C) => { _shooting = true; };
        InputController.Input.PlayerControls.Shoot.canceled += (C) => { _shooting = false; };
    }

    // Update is called once per frame
    void Update()
    {
        AnimasyonVeRotasyon();
        YerCekimiVeZiplama();
        KameraTakip();
        KarakterHareket();
    }

    void AnimasyonVeRotasyon()
    {
        if (_movement.x == 0)
        {
            _playerVelocity = Vector3.Lerp(_playerVelocity, Vector3.zero, _slowDownFactor);
            return;
        }

        if (_movement.x > 0)
        {
            _playerMesh.forward = _initialForward;
            _playerVelocity.x += _initialForward.x * _moveSpeed;
        }
        else if (_movement.x < 0)
        {
            _playerMesh.forward = _initialBackward;
            _playerVelocity.x += _initialBackward.x * _moveSpeed;
        }

        _playerVelocity.x = Mathf.Clamp(_playerVelocity.x, -_moveSpeedMax, _moveSpeedMax);
    }

    void YerCekimiVeZiplama()
    {
        if (_jumpLocked)
        {
            return;
        }

        CheckGrounded();

        if (_grounded && _jumped)//Jump.
        {
            StartCoroutine(Jump());

            _jumped = false;
            _grounded = false;
        }
        else if (!_grounded)//Fall down.
        {
            _playerVelocity.y += _gravityValue * Time.deltaTime;
        }
    }

    void KameraTakip()//Genelde cinemachinele yaparım, bu fonksiyon önceden olduğundan scriptle yaptım.
    {
        _mainCamera.transform.position = Vector3.Lerp(transform.position, transform.position + _cameraPlayerDistance, 10f); ;
    }

    void KarakterHareket()
    {
        _animator.SetFloat("Blend", Mathf.Abs(_playerVelocity.x) * 10);

        characterController.Move(100 * Time.deltaTime * _playerVelocity);
    }

    private void CheckGrounded()
    {
        _bottomPosition = transform.position;
        _grounded = Physics.Raycast(_bottomPosition, Vector3.down, out _hit, _bottomDistance, 1 << Layers.Ground);
    }

    private IEnumerator Jump()
    {
        _jumpLocked = true;
        float remainingJumpForce = _jumpForce;
        float jumpForceDiminish = .1f;
        while (remainingJumpForce > jumpForceDiminish)
        {
            _playerVelocity.y += remainingJumpForce;
            remainingJumpForce = Mathf.Lerp(remainingJumpForce, 0f, jumpForceDiminish);
            yield return _jumpWait;
        }
        _jumpLocked = false;
    }
}

using GamePatron.IndividualGames.Unity;
using System.Collections;
using UnityEngine;

namespace GamePatron.IndividualGames.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Related Components")]
        [SerializeField] private CharacterController characterController; //Contoller componenti
        [SerializeField] private Animator _animator; //Animator componenti
        [SerializeField] private GameObject mermiPrefab; //Duvar ya da npc objelerine týklandýðýnda spawnlanacak olan mermi prefabý
        [SerializeField] private Transform _cameraLookAt;
        [SerializeField] private Transform _playerMesh;

        [Header("Player Data")]
        [SerializeField] private float _movementMultiplier = 50f;
        [SerializeField] private float _jumpForce = 5f;
        [SerializeField] private float _jumpSpeedMax = 10f;
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _moveSpeedMax = 10f;
        [SerializeField] private float _gravityValue = -1f;

        private bool _grounded;
        private const float _bottomDistance = .2f;
        private const float _slowDownFactor = .1f;

        private Vector2 _movement => InputController.Input.PlayerControls.Movement.ReadValue<Vector2>();

        private bool _jumpInProgress;
        private bool _jumped;
        private bool _shooting;

        private Vector3 _initialForward;
        private Vector3 _initialBackward;
        private Vector3 _playerVelocity;

        private Vector3 _bottomPosition;
        private Vector3 _cameraPlayerDistance;

        private RaycastHit _hit;
        private Camera _mainCamera;

        private WaitForEndOfFrame _jumpWait = new();

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
                _playerVelocity.x = 0;
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
            if (_jumpInProgress)
            {
                return;
            }

            CheckGrounded();

            if (_grounded)
            {
                _playerVelocity.y = 0;
            }

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

        void KameraTakip()
        {
            _mainCamera.transform.position = Vector3.Lerp(transform.position, transform.position + _cameraPlayerDistance, 10f); ;
        }

        void KarakterHareket()
        {
            _animator.SetFloat("Blend", Mathf.Abs(_playerVelocity.x) * 10);

            characterController.Move(_movementMultiplier * Time.deltaTime * _playerVelocity);
        }

        private void CheckGrounded()
        {
            _bottomPosition = transform.position;
            _grounded = Physics.Raycast(_bottomPosition, Vector3.down, out _hit, _bottomDistance, 1 << Layers.Ground);
        }

        private IEnumerator Jump()
        {
            _jumpInProgress = true;

            float remainingJumpForce = _jumpForce;
            float jumpForceDiminish = .1f;
            while (remainingJumpForce > jumpForceDiminish)
            {
                _playerVelocity.y += remainingJumpForce;
                remainingJumpForce = Mathf.Lerp(remainingJumpForce, 0f, jumpForceDiminish);
                yield return _jumpWait;
            }

            _jumpInProgress = false;
        }
    }
}
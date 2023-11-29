using GamePatron.IndividualGames.Bullet;
using GamePatron.IndividualGames.Mouse;
using GamePatron.IndividualGames.Unity;
using IndividualGames.Pool;
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
        [SerializeField] private Transform _shootTransform;
        [SerializeField] private GameObjectPool _bulletPool;
        [SerializeField] private MouseController _mouseController;

        [Header("Player Data")]
        [SerializeField] private float _movementMultiplier = 50f;
        [SerializeField] private float _jumpForce = 5f;
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _moveSpeedMax = 10f;
        [SerializeField] private float _gravityValue = -1f;

        private Vector2 _movement => InputController.Input.PlayerControls.Movement.ReadValue<Vector2>();

        private bool _grounded;
        private bool _jumpInProgress;
        private bool _jumped;
        private bool _shootLocked;

        private Vector3 _initialForward;
        private Vector3 _initialBackward;
        private Vector3 _playerVelocity;
        private Vector3 _bottomPosition;
        private Vector3 _cameraPlayerDistance;

        private Camera _mainCamera;
        private readonly WaitForEndOfFrame _jumpWait = new();
        private readonly WaitForSeconds _shootWait = new(.5f);

        private const float _bottomDistance = .2f;

        // Use this for initialization
        void Start()
        {
            _mainCamera = Camera.main;
            _cameraPlayerDistance = _mainCamera.transform.position - transform.position;
            _mainCamera.transform.LookAt(_cameraLookAt.position);

            _initialForward = transform.forward;
            _initialBackward = -transform.forward;

            InputController.Input.PlayerControls.Jump.started += (C) => { _jumped = true; };

            _mouseController.ShootBullet += Shoot;
            GameManagerScript.Instance.GameWon += OnGameWin;
        }

        private void OnGameWin()
        {
            _animator.SetTrigger("Dance");
        }

        // Update is called once per frame
        void Update()
        {
            AnimasyonVeRotasyon();
            YerCekimiVeZiplama();
            KameraTakip();
            KarakterHareket();
        }

        /// <summary> Shoot a bullet. </summary>
        private void Shoot()
        {
            if (!_shootLocked)
            {
                _animator.SetTrigger("Punch");
                var go = _bulletPool.Retrieve();
                go.transform.position = _shootTransform.position;
                go.transform.rotation = _shootTransform.rotation;
                go.GetComponent<BulletController>().Retrieved(_bulletPool);
                StartCoroutine(ShootInterval());
            }
        }

        /// <summary> Rotates to move direction. </summary>
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

        /// <summary> Jump and fall down physics. </summary>
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

        /// <summary> Camera follow for player. </summary>
        void KameraTakip()
        {
            _mainCamera.transform.position = Vector3.Lerp(transform.position, transform.position + _cameraPlayerDistance, 10f); ;
        }

        /// <summary> Final character movement after other changes combined. </summary>
        void KarakterHareket()
        {
            _animator.SetFloat("Blend", Mathf.Abs(_playerVelocity.x) * 10);

            characterController.Move(_movementMultiplier * Time.deltaTime * _playerVelocity);
        }

        /// <summary> Check if player is grounded. </summary>
        private void CheckGrounded()
        {
            _bottomPosition = transform.position;
            _grounded = Physics.Raycast(_bottomPosition, Vector3.down, _bottomDistance, 1 << Layers.Ground);
        }

        /// <summary> Jump in progress. </summary>
        private IEnumerator Jump()
        {
            _jumpInProgress = true;
            _animator.SetTrigger("Jump");

            float remainingJumpForce = _jumpForce;
            float jumpForceDiminish = .1f;
            while (remainingJumpForce > jumpForceDiminish)
            {
                _playerVelocity.y += remainingJumpForce;
                remainingJumpForce = Mathf.Lerp(remainingJumpForce, 0f, jumpForceDiminish);
                yield return _jumpWait;
            }

            _animator.SetTrigger("Land");
            _jumpInProgress = false;
        }

        /// <summary> Interrupt jumping event. </summary>
        public void InterruptJump()
        {
            StopAllCoroutines();
            _shootLocked = false;
            _jumpInProgress = false;
        }

        /// <summary> Wait between shots. </summary>
        private IEnumerator ShootInterval()
        {
            _shootLocked = true;
            yield return _shootWait;
            _shootLocked = false;
        }
    }
}
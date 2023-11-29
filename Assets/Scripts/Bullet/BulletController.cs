using IndividualGames.CaseLib.Behavior.GameSystem;
using IndividualGames.Pool;
using UnityEngine;

namespace GamePatron.IndividualGames.Bullet
{
    /// <summary>
    /// Controls bullet movement.
    /// </summary>
    public class BulletController : MonoBehaviour, IPooledObject
    {
        [SerializeField] private float _moveSpeed = 5;
        [SerializeField] private float _destroyDistance = .05f;

        public IPool Pool { get; set; }

        private Ray _ray = new();
        private RaycastHit _hit = new();
        private bool _moving;

        private void Update()
        {
            Move();
            CheckDistance();
        }

        public void Retrieved(IPool returnPool)
        {
            _moving = true;
            Pool = returnPool;
        }

        public void ReturnToPool()
        {
            _moving = false;
            var pool = (GameObjectPool)Pool;
            pool.ReturnToPool(gameObject);
        }

        /// <summary> Check distance with raycast and destroy walls and enemies. </summary>
        private void CheckDistance()
        {
            _ray.origin = transform.position;
            _ray.direction = transform.forward;
            Debug.DrawRay(_ray.origin, _ray.direction.normalized * _destroyDistance, Color.red, .1f);
            if (Physics.Raycast(_ray, out _hit, _destroyDistance))
            {
                if (_hit.collider.gameObject.CompareTag(Tags.Wall))
                {
                    _hit.collider.gameObject.GetComponentInParent<KirilacakObje>().Broken();
                    GameManagerScript.Instance.OnPointGained(GameManagerScript.Instance.Points.Wall);
                    ReturnToPool();
                }
                else if (_hit.collider.gameObject.CompareTag(Tags.NPC))
                {
                    _hit.collider.gameObject.GetComponentInParent<NPC>().DamageNPC();
                    ReturnToPool();
                }
            }
        }

        /// <summary> Move forward in each call. </summary>
        private void Move()
        {
            if (!_moving)
            {
                return;
            }

            Vector3 newMovement = new Vector3(_moveSpeed * Time.deltaTime * transform.position.y, 0, 0);
            transform.position += newMovement;
        }
    }
}
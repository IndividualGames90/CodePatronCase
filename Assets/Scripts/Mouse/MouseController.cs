using System;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public Action ShootBullet;

    private Camera _mainCamera;
    private Ray _ray;
    private RaycastHit _hit;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out _hit))
            {
                if (_hit.collider.gameObject.CompareTag(Tags.Wall))
                {
                    ShootBullet?.Invoke();
                }
                else if (_hit.collider.gameObject.CompareTag(Tags.NPC))
                {
                    ShootBullet?.Invoke();
                }
            }
        }
    }
}

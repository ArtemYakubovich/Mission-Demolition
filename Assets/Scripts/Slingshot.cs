using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    static private Slingshot S;

    [Header("Set in Inspector")]
    [SerializeField] private GameObject _prefabProjectile;
    [SerializeField] private float _velocityMult = 8f;

    private GameObject _launchPoint;
    private Vector3 _launchPosition;
    private GameObject _projectile;
    private bool _aimingMode;
    private Rigidbody _projectileRigidbody;

    static public Vector3 LAUNCH_POS
    {
        get { if (S == null) return Vector3.zero;
            return S._launchPosition;
        }
    }

    private void Awake()
    {
        S = this;
        Transform launchPointTransform = transform.Find("LaunchPoint");
        _launchPoint = launchPointTransform.gameObject;
        _launchPoint.SetActive(false);
        _launchPosition = launchPointTransform.position;
    }

    private void OnMouseEnter()
    {
        _launchPoint.SetActive(true);
    }

    private void OnMouseExit()
    {
        _launchPoint.SetActive(false);
    }

    private void OnMouseDown()
    {
        _aimingMode = true;
        _projectile = Instantiate(_prefabProjectile);
        _prefabProjectile.transform.position = _launchPosition;
        _projectileRigidbody = _projectile.GetComponent<Rigidbody>();
        _projectileRigidbody.isKinematic = true;
    }

    private void Update()
    {
        if(_aimingMode)
        {
            Vector3 mousePos2D = Input.mousePosition;
            mousePos2D.z = -Camera.main.transform.position.z;
            Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

            Vector3 mouseDelta = mousePos3D - _launchPosition;
            float maxMagnitude = GetComponent<SphereCollider>().radius;
            if(mouseDelta.magnitude > maxMagnitude)
            {
                mouseDelta.Normalize();
                mouseDelta *= maxMagnitude;
            }

            Vector3 projPos = _launchPosition + mouseDelta;
            _projectile.transform.position = projPos;

            if(Input.GetMouseButtonUp(0))
            {
                _aimingMode = false;
                _projectileRigidbody.isKinematic = false;
                _projectileRigidbody.velocity = -mouseDelta * _velocityMult;
                FollowCam.POI = _projectile;
                _projectile = null;
                MissionDemolition.ShotFired();
                ProjectileLine.S.Poi = _projectile;
            }
        }
    }
}

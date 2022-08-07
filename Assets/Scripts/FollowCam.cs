using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [Header("Set in Inspector")]
    [SerializeField] private float _easing = 0.05f;
    [SerializeField] private Vector2 _minXY = Vector2.zero;

    static public GameObject POI;

    private float _camZ;

    private void Awake()
    {
        _camZ = transform.position.z;
    }

    private void FixedUpdate()
    {
        Vector3 destination;

        if(POI == null) {
            destination = Vector3.zero;
        } else {
            destination = POI.transform.position;
            if(POI.CompareTag("Projectile")){
                if(POI.GetComponent<Rigidbody>().IsSleeping()){
                    POI = null;
                    return;
                }
            }
        }

        destination.x = Mathf.Max(_minXY.x, destination.x);
        destination.y = Mathf.Max(_minXY.y, destination.y);
        destination = Vector3.Lerp(transform.position, destination, _easing);
        destination.z = _camZ;
        transform.position = destination;
        Camera.main.orthographicSize = destination.y + 10f;
    }
}

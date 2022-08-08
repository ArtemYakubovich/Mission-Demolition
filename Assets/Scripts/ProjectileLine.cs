using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour {
    static public ProjectileLine S;

    public GameObject Poi;

    [Header("Set in Inspector")]
    [SerializeField] private float _minDist = 0.1f;

    private LineRenderer _line;
    private List<Vector3> _points;

    private void Awake() {
        S = this;
        _line = GetComponent<LineRenderer>();
        _line.enabled = false;
        _points = new List<Vector3>();
    }

    private GameObject poi {
        get { return Poi; }
        set {
            Poi = value;
            if (Poi != null) {
                _line.enabled = false;
                _points = new List<Vector3>();
                AddPoint();
            }
        }
    }

    public void Clear() {
        Poi = null;
        _line.enabled = false;
        _points = new List<Vector3>();
    }

    private void AddPoint() {
        Vector3 pt = Poi.transform.position;
        if (_points.Count > 0 && (pt - LastPoint).magnitude < _minDist) {
            return;
        }
        if (_points.Count == 0) {
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS;
            _points.Add(pt + launchPosDiff);
            _points.Add(pt);
            _line.positionCount = 2;
            _line.SetPosition(0, _points[0]);
            _line.SetPosition(1, _points[1]);
            _line.enabled = true;
        } else {
            _points.Add(pt);
            _line.positionCount = _points.Count;
            _line.SetPosition(_points.Count - 1, LastPoint);
            _line.enabled = true;
        }
    }

    private Vector3 LastPoint {
        get {
            if (_points == null) {
                return Vector3.zero;
            }
            return (_points[_points.Count - 1]);
        }
    }

    private void FixedUpdate() {
        if (poi == null) {
            if (FollowCam.POI != null) {
                if (FollowCam.POI.CompareTag("Projectile")) {
                    poi = FollowCam.POI;
                } else {
                    return;
                }
            } else {
                return;
            }
        }

        AddPoint();
        if (FollowCam.POI == null) {
            poi = null;
        }
    }
}
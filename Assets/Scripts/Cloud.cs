using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour {
    [Header("Set in Inspector")]
    [SerializeField] private GameObject _cloudSphere;
    [SerializeField] private int _numSphereMin = 6;
    [SerializeField] private int _numSphereMax = 10;
    [SerializeField] private Vector3 _sphereOffsetScale = new Vector3(5f, 2f, 1f);
    [SerializeField] private Vector2 _sphereScaleRangeX = new Vector2(4f, 8f);
    [SerializeField] private Vector2 _sphereScaleRangeY = new Vector2(3f, 4f);
    [SerializeField] private Vector2 _sphereScaleRangeZ = new Vector2(2f, 4f);
    [SerializeField] private float scaleYMin = 2f;

    private List<GameObject> _spheres;

    private void Start() {
        _spheres = new List<GameObject>();

        int num = Random.Range(_numSphereMin, _numSphereMax);
        for (int i = 0; i < num; i++) {
            GameObject sp = Instantiate(_cloudSphere);
            _spheres.Add(sp);
            Transform spTrans = sp.transform;
            spTrans.SetParent(transform);

            Vector3 offset = Random.insideUnitSphere;
            offset.x *= _sphereOffsetScale.x;
            offset.y *= _sphereOffsetScale.y;
            offset.z *= _sphereOffsetScale.z;
            spTrans.localPosition = offset;

            Vector3 scale = Vector3.one;
            scale.x = Random.Range(_sphereScaleRangeX.x, _sphereScaleRangeX.y);
            scale.y = Random.Range(_sphereScaleRangeY.x, _sphereScaleRangeY.y);
            scale.z = Random.Range(_sphereScaleRangeZ.x, _sphereScaleRangeZ.y);

            scale.y *= 1 - (Mathf.Abs(offset.x) / _sphereOffsetScale.x);
            scale.y = Mathf.Max(scale.y, scaleYMin);

            spTrans.localScale = scale;
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Restart();
        }
    }

    private void Restart() {
        foreach (GameObject sp in _spheres) {
            Destroy(sp);
        }
        Start();
    }
}
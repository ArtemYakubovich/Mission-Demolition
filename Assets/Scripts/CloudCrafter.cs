using UnityEngine;

public class CloudCrafter : MonoBehaviour {
    [Header("Set in Inspector")]
    [SerializeField] private int _numClouds = 40;
    [SerializeField] private GameObject _cloudPrefab;
    [SerializeField] private Vector3 _cloudPosMin = new Vector3(-50f, -5f, 10f);
    [SerializeField] private Vector3 _cloudPosMax = new Vector3(150f, 100f, 10f);
    [SerializeField] public float _cloudScaleMin = 1f;
    [SerializeField] private float _cloudScaleMax = 3f;
    [SerializeField] private float _cloudSpeedMult = 0.5f;

    private GameObject[] _cloudInstances;

    private void Awake() {
        _cloudInstances = new GameObject[_numClouds];
        GameObject anchor = GameObject.Find("CloudAnchor");

        GameObject cloud;
        for (int i = 0; i < _numClouds; i++) {
            cloud = Instantiate(_cloudPrefab);
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(_cloudPosMin.x, _cloudPosMax.x);
            cPos.y = Random.Range(_cloudPosMin.y, _cloudPosMax.y);

            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(_cloudScaleMin, _cloudScaleMax, scaleU);

            cPos.y = Mathf.Lerp(_cloudPosMin.y, cPos.y, scaleU);
            cPos.z = 100f - 90f * scaleU;

            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;

            cloud.transform.SetParent(anchor.transform);
            _cloudInstances[i] = cloud;
        }
    }

    private void Update() {
        foreach (GameObject cloud in _cloudInstances) {
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;
            cPos.x -= scaleVal * Time.deltaTime * _cloudSpeedMult;

            if (cPos.x <= _cloudPosMin.x) {
                cPos.x = _cloudPosMax.x;
            }
            cloud.transform.position = cPos;
        }
    }
}
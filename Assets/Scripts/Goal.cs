using UnityEngine;

public class Goal : MonoBehaviour {
    static public bool goalMet = false;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Projectile")) {
            Goal.goalMet = true;
            Material mat = GetComponent<Renderer>().material;
            Color c = mat.color;
            c.g = 1;
            mat.color = c;
        }
    }
}
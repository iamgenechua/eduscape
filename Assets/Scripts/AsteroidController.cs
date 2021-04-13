using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AsteroidController : MonoBehaviour {

    private Rigidbody rb;

    [SerializeField] private Vector3 movementDirection;
    [SerializeField] private Vector3 rotationDirection;
    [SerializeField] private float speed;

    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private Transform explosionSource;
    [SerializeField] private Transform destroyer;
    [SerializeField] private GameObject[] objectsToDestroy;

    [Space(10)]

    [SerializeField] private UnityEvent bigExplosionEvent;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start() {
        rb.velocity = movementDirection * speed;
        rb.angularVelocity = rotationDirection * rb.velocity.magnitude * Mathf.Deg2Rad;
    }

    public void Explode() {
        Instantiate(explosionPrefab, explosionSource.position, Quaternion.identity);

        foreach (GameObject obj in objectsToDestroy) {
            obj.transform.parent = destroyer;
        }

        bigExplosionEvent.Invoke();

        Destroy(destroyer.gameObject);
    }

    private void OnCollisionEnter(Collision collision) {
        Explode();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementHeld : Element {

    protected AudioSource audioSource;

    [SerializeField] private AudioClip switchSound;
    public AudioClip SwitchSound { get => switchSound; }

    [SerializeField] private AudioClip shootSound;
    public AudioClip ShootSound { get => shootSound; }

    [SerializeField] protected ElementProjectile projectilePrefab;

    protected override void Awake() {
        base.Awake();
        audioSource = GetComponentInChildren<AudioSource>();
    }

    protected override void OnEnable() {
        base.OnEnable();
        audioSource.Play();
    }

    // Update is called once per frame
    void Update() {

    }

    public void Shoot(Vector3 direction, float force) {
        ElementProjectile projectile = Instantiate(projectilePrefab, transform.position, projectilePrefab.transform.rotation);
        projectile.GetComponent<Rigidbody>().velocity = direction * force;
    }
}

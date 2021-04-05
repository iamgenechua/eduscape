using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFxSource : MonoBehaviour {

    private AudioSource audioSource;

    [Tooltip("The sphere within which the sound plays at its default volume.")]
    [SerializeField] private SphereCollider defaultVolumeSphere;

    [Tooltip("The sphere within which the volume will decrease towards zero as the player moves towards its edge." +
        "This should be larger than the default volume sphere.")]
    [SerializeField] private SphereCollider decreasingVolumeSphere;

    private float defaultVolume;

    /// <summary>
    /// The distance between the edge of the area of default volume and the area of zero volume.
    /// </summary>
    private float decreasingVolumeDistance;

    void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start() {
        if (decreasingVolumeSphere.radius < defaultVolumeSphere.radius) {
            decreasingVolumeSphere.radius = defaultVolumeSphere.radius;
        }

        decreasingVolumeDistance = decreasingVolumeSphere.radius - defaultVolumeSphere.radius;

        defaultVolume = audioSource.volume;
    }

    // Update is called once per frame
    void Update() {
        if (audioSource.isPlaying) {
            ModulateVolume();
        }
    }

    /// <summary>
    /// Adjusts the volume of the sound based on proximity to the player.
    /// </summary>
    private void ModulateVolume() {
        float scale = Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);

        float distanceToPlayer = Vector3.Distance(transform.position, LevelManager.Instance.Player.transform.position);
        if (distanceToPlayer <= defaultVolumeSphere.radius * scale) {
            // maintain volume at default volume
            audioSource.volume = defaultVolume;
            if (!audioSource.isPlaying) {
                audioSource.Play();
            }

            return;
        }

        float decreasingVolumeRadius = decreasingVolumeSphere.radius * scale;
        if (distanceToPlayer >= decreasingVolumeRadius) {
            // exceeded total radius
            audioSource.volume = 0f;
            return;
        }

        // decrease volume
        audioSource.volume = (decreasingVolumeRadius - distanceToPlayer) / (decreasingVolumeDistance * scale) * defaultVolume;
        if (!audioSource.isPlaying) {
            audioSource.Play();
        }
    }
}

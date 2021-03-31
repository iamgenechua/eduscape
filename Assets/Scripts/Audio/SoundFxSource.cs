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

    /// <summary>
    /// The distance between the edge of the area of default volume and zero volume.
    /// </summary>
    private float decreasingVolumeDistance;

    public float DefaultVolume { private get; set; }

    public bool IsPlaying { private get; set; }

    // Start is called before the first frame update
    void Start() {
        audioSource = GetComponent<AudioSource>();

        if (decreasingVolumeSphere.radius < defaultVolumeSphere.radius) {
            decreasingVolumeSphere.radius = defaultVolumeSphere.radius;
        }

        decreasingVolumeDistance = decreasingVolumeSphere.radius - defaultVolumeSphere.radius;
    }

    // Update is called once per frame
    void Update() {
        if (IsPlaying) {
            ModulateVolume();
        }
    }

    /// <summary>
    /// Adjusts the volume of the sound based on proximity to the player.
    /// </summary>
    private void ModulateVolume() {
        float distanceToPlayer = Vector3.Distance(transform.position, GameManager.Instance.Player.transform.position);
        if (distanceToPlayer <= defaultVolumeSphere.radius) {
            // maintain volume at default volume
            audioSource.volume = DefaultVolume;
            if (!audioSource.isPlaying) {
                audioSource.Play();
            }

            return;
        }

        if (distanceToPlayer >= decreasingVolumeSphere.radius) {
            // exceeded total radius; stop ambient sound
            audioSource.Stop();
            return;
        }

        // decrease volume
        audioSource.volume = (decreasingVolumeSphere.radius - distanceToPlayer) / decreasingVolumeDistance * DefaultVolume;
        if (!audioSource.isPlaying) {
            audioSource.Play();
        }
    }
}

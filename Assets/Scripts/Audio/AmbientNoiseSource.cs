using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientNoiseSource : MonoBehaviour {

    [Tooltip("The sphere within which the ambient noise plays at its default volume.")]
    [SerializeField] private SphereCollider defaultVolumeSphere;

    [Tooltip("The sphere within which the volume will decrease towards zero as the player moves towards its edge." +
        "This should be larger than the default volume sphere.")]
    [SerializeField] private SphereCollider decreasingVolumeSphere;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public float GetDefaultVolumeRadius() {
        return defaultVolumeSphere.radius;
    }

    /// <summary>
    /// Gets the distance between the edge of the area of default volume and zero volume.
    /// </summary>
    /// <returns></returns>
    public float GetDecreasingVolumeDistance() {
        return Mathf.Max(decreasingVolumeSphere.radius - defaultVolumeSphere.radius, 0f);
    }

    public float GetTotalRadius() {
        return decreasingVolumeSphere.radius;
    }
}

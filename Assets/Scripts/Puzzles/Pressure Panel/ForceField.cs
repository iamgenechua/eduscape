using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour {

    [SerializeField] private float scaleSpeed = 1f;
    [SerializeField] private float minScale = 0.5f;
    [SerializeField] private float maxScale = 2f;
    
    public bool IsScalingUp { get; private set; }
    public bool IsScalingDown { get; private set; }
    public bool HasReachedMinSize { get => transform.localScale.x <= minScale; }
    public bool HasReachedMaxSize { get => transform.localScale.x >= maxScale; }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if (IsScalingUp && !IsScalingDown && !HasReachedMaxSize) {
            transform.localScale += Vector3.one * scaleSpeed * Time.deltaTime;
        } else if (!IsScalingUp && IsScalingDown && !HasReachedMinSize) {
            transform.localScale -= Vector3.one * scaleSpeed * Time.deltaTime;
        }
    }

    public void ScaleUp() {
        if (HasReachedMaxSize) {
            return;
        }

        IsScalingUp = true;
    }

    public void StopScalingUp() {
        IsScalingUp = false;
    }

    public void ScaleDown() {
        if (HasReachedMinSize) {
            return;
        }

        IsScalingDown = true;
    }

    public void StopScalingDown() {
        IsScalingDown = false;
    }
}

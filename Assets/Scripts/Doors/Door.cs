using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour {

    private Animator anim;

    [SerializeField] private string animOpenBool;

    [Tooltip("Object, with colliders, to activate when door is closed.")]
    [SerializeField] private GameObject closedColliders;

    [Tooltip("The action blocker in the negative direction of the x axis.")]
    [SerializeField] private BoxCollider actionBlockerLeft;
    [Tooltip("The action blocker in the positive direction of the x axis.")]
    [SerializeField] private BoxCollider actionBlockerRight;

    public bool IsOpen { get => anim.GetBool(animOpenBool); private set => anim.SetBool(animOpenBool, value); }

    private bool isPlayerInDoorway = false;
    public bool IsPlayerInDoorway { get => isPlayerInDoorway; }

    [SerializeField] private UnityEvent openEvent;
    public UnityEvent OpenEvent { get => openEvent; }

    [SerializeField] private UnityEvent closeEvent;
    public UnityEvent CloseEvent { get => closeEvent; }

    void Awake() {
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start() {
        closedColliders.SetActive(!anim.GetBool(animOpenBool));
        ActivateActionBlocker();
    }

    private void ActivateActionBlocker() {
        Vector3 localPos = transform.InverseTransformPoint(LevelManager.Instance.PlayerBody.transform.position);

        // use z axis because all doors are rotated 90 degrees to the right about the y axis
        if (localPos.z < 0f) {
            actionBlockerLeft.gameObject.SetActive(false);
            actionBlockerRight.gameObject.SetActive(true);
        } else {
            actionBlockerLeft.gameObject.SetActive(true);
            actionBlockerRight.gameObject.SetActive(false);
        }
    }

    public void OpenDoor() {
        if (IsOpen) {
            return;
        }

        IsOpen = true;
        closedColliders.SetActive(false);
        actionBlockerLeft.gameObject.SetActive(false);
        actionBlockerRight.gameObject.SetActive(false);

        openEvent.Invoke();
    }

    public void CloseDoor() {
        if (!IsOpen) {
            return;
        }

        IsOpen = false;
        closedColliders.SetActive(true);
        ActivateActionBlocker();

        closeEvent.Invoke();
    }

    public void ToggleDoor(Element element) {
        if (element.ElementType != Element.Type.FIRE) {
            if (IsOpen) {
                CloseDoor();
            } else {
                OpenDoor();
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            isPlayerInDoorway = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            isPlayerInDoorway = false;
        }
    }

    private void OnDestroy() {
        openEvent.RemoveAllListeners();
        closeEvent.RemoveAllListeners();
    }
}

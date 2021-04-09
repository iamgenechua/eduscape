using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour {

    protected Animator anim;
    protected AudioSource audioSource;

    [SerializeField] protected string animOpenBool;
    [SerializeField] protected string animOpeningStateName;
    [SerializeField] protected string animClosingStateName;

    [SerializeField] protected AudioClip openSound;
    [SerializeField] protected AudioClip closeSound;

    [Tooltip("Object, with colliders, to activate when door is closed.")]
    [SerializeField] protected GameObject closedColliders;

    [Tooltip("The action blocker in the negative direction of the x axis.")]
    [SerializeField] protected ActionBlocker actionBlockerLeft;
    [Tooltip("The action blocker in the positive direction of the x axis.")]
    [SerializeField] protected ActionBlocker actionBlockerRight;

    public bool IsOpen { get => anim.GetBool(animOpenBool); protected set => anim.SetBool(animOpenBool, value); }

    protected bool isPlayerInDoorway = false;
    public bool IsPlayerInDoorway { get => isPlayerInDoorway; }

    [SerializeField] protected UnityEvent openEvent;
    public UnityEvent OpenEvent { get => openEvent; }

    [SerializeField] protected UnityEvent closeEvent;
    public UnityEvent CloseEvent { get => closeEvent; }

    protected virtual void Awake() {
        anim = GetComponent<Animator>();
        audioSource = GetComponentInChildren<AudioSource>();
    }

    // Start is called before the first frame update
    protected virtual void Start() {
        closedColliders.SetActive(!anim.GetBool(animOpenBool));
        ActivateActionBlocker();
    }

    protected virtual void ActivateActionBlocker() {
        Vector3 localPos = transform.InverseTransformPoint(LevelManager.Instance.PlayerBody.transform.position);

        // use z axis because all doors are rotated 90 degrees to the right about the y axis
        if (localPos.z < 0f) {
            actionBlockerLeft.Deactivate();
            actionBlockerRight.Activate();
        } else {
            actionBlockerLeft.Activate();
            actionBlockerRight.Deactivate();
        }
    }

    public virtual void OpenDoor() {
        if (IsOpen) {
            return;
        }

        IsOpen = true;
        closedColliders.SetActive(false);
        actionBlockerLeft.Deactivate();
        actionBlockerRight.Deactivate();

        audioSource.PlayOneShot(openSound);

        openEvent.Invoke();
    }

    public virtual void CloseDoor() {
        if (!IsOpen) {
            return;
        }

        IsOpen = false;
        closedColliders.SetActive(true);
        ActivateActionBlocker();

        audioSource.PlayOneShot(closeSound);

        closeEvent.Invoke();
    }

    public virtual void ToggleDoor(Element element) {
        if (element.ElementType != Element.Type.FIRE) {
            if (IsOpen) {
                CloseDoor();
            } else {
                OpenDoor();
            }
        }
    }

    public virtual bool IsOpeningOrClosing() {
        AnimatorStateInfo animStateInfo = anim.GetCurrentAnimatorStateInfo(0);
        return animStateInfo.IsName(animOpeningStateName) || animStateInfo.IsName(animClosingStateName);
    }

    protected virtual void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            isPlayerInDoorway = true;
        }
    }

    protected virtual void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            isPlayerInDoorway = false;
        }
    }

    protected virtual void OnDestroy() {
        openEvent.RemoveAllListeners();
        closeEvent.RemoveAllListeners();
    }
}

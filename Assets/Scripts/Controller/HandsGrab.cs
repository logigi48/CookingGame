using System.Collections;
using UnityEngine;

public class HandsGrab : MonoBehaviour
{
    // 1. A clean State Machine replaces the confusing boolean "_check"
    public enum GrabState { Idle, Reaching, Holding }

    [Header("Core Settings")]
    public HandCollide.ArmType armType;
    [SerializeField] private Transform hand;
    [SerializeField] private Transform tip;
    
    [Header("Controls")]
    [SerializeField] private KeyCode grabKey;
    [SerializeField] private KeyCode actionKey;

    [Header("Reach Settings")]
    [SerializeField] private float raycastDistance = 50f;
    [SerializeField] private float reachSpeed = 10f;
    [SerializeField] private float reachSnapDistance = 0.3f; // How close the hand must be to snap
    [SerializeField] private float reachTimeout = 1.5f; // Failsafe so the hand doesn't get stuck

    // Expose this for HandCollide.cs
    public bool isGrabbed => currentState == GrabState.Holding;

    private GrabState currentState = GrabState.Idle;
    
    // We cache these to avoid using GetComponent every single frame!
    private Interfaces.IInteractable currentInteractable;
    private Transform grabbedTransform;
    private Rigidbody grabbedRigidbody;

    private void Update()
    {
        HandleInput();

        if (currentState == GrabState.Holding && grabbedTransform != null)
        {
            // FIX: Lerp the LOCAL rotation to Quaternion.identity.
            // Quaternion.identity means "0,0,0" in local space, forcing it to 
            // perfectly and smoothly align with its parent (the tip) over time.
            grabbedTransform.localRotation = Quaternion.Lerp(grabbedTransform.localRotation, Quaternion.identity, 5f * Time.deltaTime);
        }
    }

    private void HandleInput()
    {
        // Block player input while the hand is doing the automated reaching animation
        if (currentState == GrabState.Reaching) return;

        if (Input.GetKeyDown(grabKey))
        {
            if (currentState == GrabState.Holding)
                DropObject();
            else
                TryLocateObject();
        }

        if (Input.GetKeyDown(actionKey) && currentState == GrabState.Holding && currentInteractable != null)
        {
            currentInteractable.Action();
        }
    }

    private void TryLocateObject()
    {
        if (Physics.Raycast(hand.position, Vector3.down, out RaycastHit hit, raycastDistance))
        {
            Interfaces.IInteractable interactable = hit.collider.GetComponentInParent<Interfaces.IInteractable>();

            if (interactable != null)
            {
                Transform targetTransform = (interactable as Component).transform;
                // Pass the collider so we can calculate the exact top-center volume
                StartCoroutine(ReachForObjectRoutine(targetTransform, hit.collider, interactable));
            }
        }
    }

    private IEnumerator ReachForObjectRoutine(Transform target, Collider targetCollider, Interfaces.IInteractable interactable)
    {
        currentState = GrabState.Reaching;
        bool isLeft = armType == HandCollide.ArmType.LeftArm;

        if (isLeft) HandsMovement.Instance.LLocked = true;
        else HandsMovement.Instance.RLocked = true;

        float timer = 0f;

        while (timer < reachTimeout && target != null)
        {
            timer += Time.deltaTime;

            // 1. Get the exact center of the object (X and Z) and the highest point (Y)
            Bounds bounds = targetCollider.bounds;
            Vector3 topCenter = new Vector3(bounds.center.x, bounds.max.y, bounds.center.z);
            
            // Optional: Add a tiny offset if you want it slightly ABOVE the highest point
            float hoverHeight = topCenter.y + 0.02f; 

            float distX = topCenter.x - hand.position.x;
            float distZ = topCenter.z - hand.position.z;

            if (isLeft)
            {
                HandsMovement.Instance.AdjustMoveVector(distX, distZ, 0f, 0f);
                HandsMovement.Instance.Lmove.y = hoverHeight;
            }
            else
            {
                HandsMovement.Instance.AdjustMoveVector(0f, 0f, distX, distZ);
                HandsMovement.Instance.Rmove.y = hoverHeight;
            }

            // 2. Use Vector3 to properly check X, Y, AND Z distance!
            if (Vector3.Distance(hand.position, topCenter) <= reachSnapDistance)
            {
                break;
            }

            yield return null; 
        }

        ProcessReachedObject(target, interactable);
    }
    

    private void ProcessReachedObject(Transform target, Interfaces.IInteractable interactable)
    {
        if (target == null)
        {
            ResetArmLock();
            currentState = GrabState.Idle;
            return;
        }

        if (interactable.Grabable)
        {
            currentInteractable = interactable;
            grabbedTransform = target;
            grabbedRigidbody = target.GetComponent<Rigidbody>();

            if (grabbedRigidbody != null) grabbedRigidbody.isKinematic = true;

            grabbedTransform.parent = tip;
            
            // Center it on the hand
            grabbedTransform.localPosition = Vector3.zero;
            
            // FIX: Force the rotation to perfectly align with the hand tip!
            // This cancels out any weird physics tumbling that happened on the floor.
            grabbedTransform.localRotation = Quaternion.identity; 

            currentInteractable.Grabed = true;
            currentState = GrabState.Holding;
        }
        else
        {
            interactable.Action();
            currentState = GrabState.Idle;
        }

        ResetArmLock();
    }

    public void DropObject()
    {
        if (grabbedTransform == null) return;

        grabbedTransform.parent = null;

        if (currentInteractable != null)
        {
            currentInteractable.Grabed = false;
        }

        if (grabbedRigidbody != null)
        {
            grabbedRigidbody.isKinematic = false;
        }

        // Clear all cached references cleanly
        currentInteractable = null;
        grabbedTransform = null;
        grabbedRigidbody = null;
        
        currentState = GrabState.Idle;
    }

    private void ResetArmLock()
    {
        if (armType == HandCollide.ArmType.LeftArm)
            HandsMovement.Instance.LLocked = false;
        else
            HandsMovement.Instance.RLocked = false;
    }
}
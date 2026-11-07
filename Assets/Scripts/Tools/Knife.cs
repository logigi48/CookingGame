using System.Collections;
using UnityEngine;

public class Knife : MonoBehaviour, Interfaces.IInteractable
{
    public bool Grabable { get; set; } = true;
    
    private bool _grabed;
    public bool Grabed 
    { 
        get => _grabed; 
        set 
        { 
            _grabed = value; 
            KnifeCollider.isTrigger = _grabed; 
        } 
    }

    [Header("Game Feel Settings")]
    public float progressPerCut = 1f; 
    [Tooltip("How far down the wrist plunges when you chop. Tweak this until the blade perfectly hits the board!")]
    public float chopDropDistance = 0.35f; 
    public float downwardSlamSpeedMult = 10f; // Multiplies your normal LerpSpeed for a violent strike

    public Collider KnifeCollider;

    private Coroutine chopCoroutine;

    public void Action()
    {
        // 1. THE INTERRUPT: If we are already swinging, kill the Coroutine instantly!
        if (chopCoroutine != null)
        {
            StopCoroutine(chopCoroutine);
        }
        
        // Start a fresh, aggressive chop
        chopCoroutine = StartCoroutine(ChopRoutine());
    }

    private IEnumerator ChopRoutine()
    {
        HandsGrab holdingHand = GetComponentInParent<HandsGrab>();
        
        if (holdingHand != null)
        {
            bool isLeft = holdingHand.armType == HandCollide.ArmType.LeftArm;
            
            // Lock the arm so we can control it
            if (isLeft) HandsMovement.Instance.LLocked = true;
            else HandsMovement.Instance.RLocked = true;

            float originalLerpSpeed = HandsMovement.Instance.LerpSpeed;

            // Calculate the target by simply subtracting our drop distance from the hand's current hovering height
            float currentHoverY = isLeft ? HandsMovement.Instance.Lmove.y : HandsMovement.Instance.Rmove.y;
            float strikeTargetY = currentHoverY - chopDropDistance;

            // --- THE SLAM (Downwards) ---
            // Crank the speed to make the strike near-instant
            HandsMovement.Instance.LerpSpeed = originalLerpSpeed * downwardSlamSpeedMult;
            
            if (isLeft) HandsMovement.Instance.Lmove.y = strikeTargetY;
            else HandsMovement.Instance.Rmove.y = strikeTargetY;
            
            // Wait just 0.05 seconds. We don't want the knife resting on the board; it should bounce!
            yield return new WaitForSeconds(0.05f); 

            // --- THE RECOVERY (Upwards) ---
            // Reset the speed back to normal
            HandsMovement.Instance.LerpSpeed = originalLerpSpeed;
            
            // Unlocking the arm automatically tells HandsMovement to float the arm back up to its original height!
            if (isLeft) HandsMovement.Instance.LLocked = false;
            else HandsMovement.Instance.RLocked = false;
            
            // Give it a tiny moment before clearing the coroutine memory
            yield return new WaitForSeconds(0.1f);
        }
        
        chopCoroutine = null;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!Grabed) return;

        FoodItem food = other.GetComponentInParent<FoodItem>();
        if (food != null)
        {
            food.ApplyProcess(FoodAction.Slice, progressPerCut);
        }
    }
}
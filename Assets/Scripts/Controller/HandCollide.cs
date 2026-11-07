using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCollide : MonoBehaviour
{
    public enum ArmType
    {
        LeftArm,
        RightArm,
    }

    [SerializeField] private ArmType armType;
    [SerializeField] private float HorizontalBounce;
    [SerializeField] private float VerticalBounce;

    public void OnTriggerStay(Collider other)
    {
        
        if (!other.CompareTag("Lower Arm") && !other.CompareTag("Upper Arm")) 
            return;

        var handsGrab = other.GetComponentInParent<HandsGrab>();
        if (handsGrab != null && handsGrab.isGrabbed)
        {
            handsGrab.DropObject();
        }
        
        // Lower Arm gets negative bounce, Upper Arm gets positive bounce
        float verticalDir = other.CompareTag("Lower Arm") ? -VerticalBounce : VerticalBounce;

        // Apply the movement based on the arm type.
        if (armType == ArmType.RightArm)
        {
            HandsMovement.Instance.AdjustMoveVector(-HorizontalBounce,verticalDir,0f,0f);
        }
        else if (armType == ArmType.LeftArm)
        {
            HandsMovement.Instance.AdjustMoveVector(0f,0f,HorizontalBounce,verticalDir);
        }

    }
}

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

    public ArmType armType;
    public float HorizontalBounce;
    public float VerticalBounce;

    public void OnTriggerStay(Collider other)
    {
        if(other.tag == "Lower Arm")
        {

            if (armType == ArmType.RightArm)
                HandsMovement.Instance.AddMovement(-HorizontalBounce, -VerticalBounce, 0f, 0f);
            if (armType == ArmType.LeftArm)
                HandsMovement.Instance.AddMovement(0f, 0f, HorizontalBounce, -VerticalBounce);
        }

        if (other.tag == "Upper Arm")
        {

            if (armType == ArmType.RightArm)
                HandsMovement.Instance.AddMovement(-HorizontalBounce, VerticalBounce, 0f, 0f);
            if (armType == ArmType.LeftArm)
                HandsMovement.Instance.AddMovement(0f, 0f, HorizontalBounce, VerticalBounce);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsGrab : MonoBehaviour
{
    public bool IsGrab;
    public Transform Hand;
    public HandCollide.ArmType ArmType;
    public KeyCode Grab_DropKey;
    public KeyCode ActionKey;

    RaycastHit hit;
    bool Check;

    private void Update()
    {
        if (Input.GetKeyDown(Grab_DropKey))
            LocateTheObject();

        if (Check)
        {
            if (ArmType == HandCollide.ArmType.LeftArm)
            {
                HandsMovement.Instance.LLocked = true;
                HandsMovement.Instance.AddMovement(hit.transform.position.x - Hand.position.x, hit.transform.position.z - Hand.position.z, 0f, 0f);
            }

            if (ArmType == HandCollide.ArmType.RightArm)
            {
                HandsMovement.Instance.RLocked = true;
                HandsMovement.Instance.AddMovement(0f, 0f, hit.transform.position.x - Hand.position.x, hit.transform.position.z - Hand.position.z);
            }
        }
    }

    public void LocateTheObject()
    {
        if (Physics.Raycast(Hand.position, Vector3.down, out hit, 5f))
        {
            if (hit.transform.GetComponent<Food>() != null)
                Check = true;
            else
                Check = false;
        }

        else
            Check = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsMovement : MonoBehaviour
{
    public static HandsMovement Instance;

    private void Awake()
    {
        Instance = this;
    }

    public float HandsSensitivity;
    public Transform RArmTarget;
    public Transform LArmTarget;

    Vector3 Lmove;
    Vector3 Rmove;

    [Header("Limits")]

    public Vector2 HorizontalMinMax;
    public Vector2 VerticalMinMax;

    void Update()
    {
        float Lh = Input.GetAxis("Horizontal");
        float Lv = Input.GetAxis("Vertical");

        float Rh = Input.GetAxis("Arrow Horizontal");
        float Rv = Input.GetAxis("Arrow Vertical");

        Lmove += new Vector3(Lh, 0f, Lv) * HandsSensitivity * Time.deltaTime;
        Rmove += new Vector3(Rh, 0f, Rv) * HandsSensitivity * Time.deltaTime;

        Lmove.x = Mathf.Clamp(Lmove.x, HorizontalMinMax.x, HorizontalMinMax.y);
        Rmove.x = Mathf.Clamp(Rmove.x, HorizontalMinMax.x, HorizontalMinMax.y);

        Lmove.z = Mathf.Clamp(Lmove.z, VerticalMinMax.x, VerticalMinMax.y);
        Rmove.z = Mathf.Clamp(Rmove.z, VerticalMinMax.x, VerticalMinMax.y);

        LArmTarget.localPosition = new Vector3(Lmove.x, LArmTarget.localPosition.y, Lmove.z);
        RArmTarget.localPosition = new Vector3(Rmove.x, RArmTarget.localPosition.y, Rmove.z);
    }
}

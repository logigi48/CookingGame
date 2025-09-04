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

    public bool RLocked;
    public bool LLocked;

    [Header("")]

    public float HandsSensitivity;
    public float LerpSpeed;
    public Transform RArmTarget;
    public Transform LArmTarget;

    Vector3 Lmove;
    Vector3 Rmove;

    float Lh;
    float Lv;
    float Rh;
    float Rv;

    [Header("Limits")]

    public Vector2 HorizontalMinMax;
    public Vector2 VerticalMinMax;
    public float MaxRad;

    void Update()
    {
        if (!LLocked)
        {
            Lh = Input.GetAxis("Horizontal");
            Lv = Input.GetAxis("Vertical");
        }

        if (!RLocked)
        {
            Rh = Input.GetAxis("Arrow Horizontal");
            Rv = Input.GetAxis("Arrow Vertical");
        }

        AddMovement(Lh, Lv, Rh, Rv);

        Lmove.x = Mathf.Clamp(Lmove.x, HorizontalMinMax.x, HorizontalMinMax.y);
        Rmove.x = Mathf.Clamp(Rmove.x, HorizontalMinMax.x, HorizontalMinMax.y);

        Lmove.z = Mathf.Clamp(Lmove.z, VerticalMinMax.x, VerticalMinMax.y);
        Rmove.z = Mathf.Clamp(Rmove.z, VerticalMinMax.x, VerticalMinMax.y);

        float Ldist = Lmove.magnitude;
        float Rdist = Rmove.magnitude;

        if (Ldist > MaxRad)
            Lmove = Lmove.normalized * MaxRad;

        if (Rdist > MaxRad)
            Rmove = Rmove.normalized * MaxRad;

        LArmTarget.localPosition = Vector3.Lerp(LArmTarget.localPosition, new Vector3(Lmove.x, LArmTarget.localPosition.y, Lmove.z), LerpSpeed * Time.deltaTime);
        RArmTarget.localPosition = Vector3.Lerp(RArmTarget.localPosition, new Vector3(Rmove.x, RArmTarget.localPosition.y, Rmove.z), LerpSpeed * Time.deltaTime);
    }

    public void AddMovement(float LH, float LV, float RH, float RV, float LY = 0f, float RY = 0f)
    {
        Lmove += new Vector3(LH, LY, LV) * HandsSensitivity * Time.deltaTime;
        Rmove += new Vector3(RH, RY, RV) * HandsSensitivity * Time.deltaTime;
    }
}

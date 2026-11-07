using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsMovement : MonoBehaviour
{
    public static HandsMovement Instance { get; private set; }

    public bool RLocked;
    public bool LLocked;
    
    [HideInInspector] public Vector3 Lmove;
    [HideInInspector] public Vector3 Rmove;
    
    [Header("")]

    public float LerpSpeed;
    
    [SerializeField] float HandsSensitivity;
    [SerializeField] Transform RArmTarget;
    [SerializeField] Transform LArmTarget;
    
    [Header("Limits")]

    [SerializeField] Vector2 HorizontalMinMax;
    [SerializeField] Vector2 VerticalMinMax;
    [SerializeField] float MaxRad;
    
    
    [Header("Oriantation")]

    [SerializeField] Transform LLowerArm;
    [SerializeField] Transform RLowerArm;
    
    
    float Lh;
    float Lv;
    float Rh;
    float Rv;

    
    Vector3 LOriginalPos;
    Vector3 ROriginalPos;
    
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Duplicate HandsMovement found and destroyed!");
            Destroy(this.gameObject); 
            
            return; 
        }
        
        Instance = this;
    }
    
    private void Start()
    {
        LOriginalPos = LArmTarget.localPosition;
        ROriginalPos = RArmTarget.localPosition;

        Lmove = LOriginalPos;
        Rmove = ROriginalPos;
    }

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


        AdjustMoveVector(Lh, Lv, Rh, Rv);

        LArmTarget.localPosition = Vector3.Lerp(LArmTarget.localPosition, new Vector3(Lmove.x, LArmTarget.localPosition.y, Lmove.z), LerpSpeed * Time.deltaTime);
        RArmTarget.localPosition = Vector3.Lerp(RArmTarget.localPosition, new Vector3(Rmove.x, RArmTarget.localPosition.y, Rmove.z), LerpSpeed * Time.deltaTime);
    }
    
    public void AdjustMoveVector(float LH, float LV, float RH, float RV)
    {
        Lmove += new Vector3(LH, 0f, LV) * (HandsSensitivity * Time.deltaTime);
        Rmove += new Vector3(RH, 0f, RV) * (HandsSensitivity * Time.deltaTime);
        
        
        if (!LLocked)
            Lmove.y = LOriginalPos.y;
        if (!RLocked)
            Rmove.y = ROriginalPos.y;

        Lmove.x = Mathf.Clamp(Lmove.x, HorizontalMinMax.x, HorizontalMinMax.y);
        Rmove.x = Mathf.Clamp(Rmove.x, HorizontalMinMax.x, HorizontalMinMax.y);
        
        Lmove.z = Mathf.Clamp(Lmove.z, VerticalMinMax.x, VerticalMinMax.y);
        Rmove.z = Mathf.Clamp(Rmove.z, VerticalMinMax.x, VerticalMinMax.y);
        
        float ldist = Lmove.magnitude;
        float rdist = Rmove.magnitude;

        if (ldist > MaxRad)
            Lmove = Lmove.normalized * MaxRad;

        if (rdist > MaxRad)
            Rmove = Rmove.normalized * MaxRad;
        
    }
}

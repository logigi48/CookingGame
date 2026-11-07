using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform Player;
    [SerializeField] Transform Cam;
    [SerializeField] Transform CamPos;
    [SerializeField] float StepDistance;
    [SerializeField] float StepSpeed;
    [SerializeField] float CamSpeed;
    [SerializeField] KeyCode RightKey;
    [SerializeField] KeyCode LeftKey;
    Vector3 StepVector;

    private void Start()
    {
        StepVector = Player.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(RightKey))
            Step(StepDistance);
        if(Input.GetKeyDown(LeftKey))
            Step(-StepDistance);


        Player.position = Vector3.Lerp(Player.position, StepVector, StepSpeed * Time.deltaTime); 
        Cam.position = Vector3.Lerp(Cam.position, CamPos.position, CamSpeed * Time.deltaTime);
    }

    public void Step(float distance)
    {
        StepVector += new Vector3(distance, 0f, 0f); 
    }
}

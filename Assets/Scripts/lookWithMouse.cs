using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookWithMouse : MonoBehaviour
{
    public enum RotationDirection{
        None,
        Horizontal = (1<<0),
        Vertical = (1<<1)
    }
    public Vector2 sens;
    public Vector2 acceleration;
    public RotationDirection rotationDirections;
    public float inputLagPeriod;
    public float maxVerticalAngleFromHorizon;
    private Vector2 rotation;
    private Vector2 velocity;
    private Vector2 lastInputEvent;
    private float inputLagTimer; 
    private PlayerController playerMove;
    private float ClampVerticalAngle(float angle){
        return Mathf.Clamp(angle, -maxVerticalAngleFromHorizon, maxVerticalAngleFromHorizon);
    }
    private Vector2 GetInput(){
        inputLagTimer+=Time.deltaTime;
        Vector2 input = new Vector2(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y")
        );
        if((Mathf.Approximately(0,input.x) && Mathf.Approximately(0,input.y))==false|| inputLagTimer>=inputLagPeriod){
            lastInputEvent = input;
            inputLagTimer=0;
        }
        return lastInputEvent;
    }
    private void playerRotation(){
        Vector2 wantedVelocity = GetInput() * sens;
        if((rotationDirections & RotationDirection.Horizontal) == 0){
            wantedVelocity.x = 0;
        }
        if((rotationDirections & RotationDirection.Vertical) == 0){
            wantedVelocity.y = 0;
        }
        rotation += wantedVelocity * Time.deltaTime;
        transform.localEulerAngles = new Vector3(rotation.y, rotation.x, 0);
        velocity = new Vector2(
            Mathf.MoveTowards(velocity.x, wantedVelocity.x, acceleration.x * Time.deltaTime),
            Mathf.MoveTowards(velocity.y, wantedVelocity.y, acceleration.y * Time.deltaTime));
            rotation += velocity * Time.deltaTime;
            rotation.y = ClampVerticalAngle(rotation.y);
    }
    void Start(){
        playerMove = GetComponent<PlayerController>();
    }
    void Update(){
            playerRotation();
    }
}

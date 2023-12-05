using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPun{
    public int id;
    public Rigidbody rig;
    public Collider _col;
    public Player photonPlayer;
    public GameObject flashLight;
    public float speed;
    public float jumpForce;
    public float distanceToGround;

    private float hInput;
    private float vInput;
    private bool isFlashlight;

    public static PlayerController me;

    [PunRPC]
    public void Initialize(Player player){
        id = player.ActorNumber;
        photonPlayer = player;
        GameManager.instance.players[id - 1] = this;
        if(!photonView.IsMine){
            GetComponentInChildren<Camera>().gameObject.SetActive(false);
            GetComponent<lookWithMouse>().enabled = false;
        }
    }

    void Update(){
        if(photonView.IsMine){
            hInput = Input.GetAxis("Horizontal") * speed;
            vInput = Input.GetAxis("Vertical") * speed;
            if(Input.GetKeyDown(KeyCode.F)){
                isFlashlight = !isFlashlight;
                flashLight.SetActive(isFlashlight);
            }
        }
    }

    void FixedUpdate(){
        if(photonView.IsMine){
            Move();
        }
    }

    public void Move(){
        rig.MovePosition(transform.position + transform.forward * vInput * Time.deltaTime + transform.right * hInput * Time.fixedDeltaTime);
    }
}

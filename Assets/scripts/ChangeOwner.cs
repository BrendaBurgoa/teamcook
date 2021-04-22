using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeOwner : Photon.PunBehaviour
{
    private bool  isGrabbed = false;
    private bool collisionFlag = false;

    private void OnMouseDown(){
        this.photonView.TransferOwnership(PhotonNetwork.player);
        isGrabbed = true;
    }
    void Update(){
        if (base.photonView.owner == PhotonNetwork.player && collisionFlag == true && isGrabbed == true)
        {
            float moveSpeed = 2f;
            float vertical = Input.GetAxis("Vertical");
            float horizontal = Input.GetAxis("Horizontal");

            transform.position += transform.forward * (vertical * moveSpeed * Time.deltaTime);
            transform.position += transform.right * (horizontal * moveSpeed * Time.deltaTime);
        }
        if(Input.GetKeyDown("space") && isGrabbed == true){
           this.photonView.TransferOwnership(0);
        }
        else if (Input.GetKeyDown("space") && isGrabbed == false && collisionFlag == true){
            Debug.Log("agarra");
            this.photonView.TransferOwnership(PhotonNetwork.player);
            isGrabbed = true;
        }
    }
    
    void OnCollisionEnter(Collision other)
    {
        collisionFlag = true;

    }
    void OnCollisionExit(Collision other)
    {
        isGrabbed = false;
        collisionFlag = false;

    }
}


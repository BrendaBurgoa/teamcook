using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class extinguisher : MonoBehaviour
{
    public PhotonView photonView;
    private bool collided;
    private GameObject character;
    void Update(){
        Data.Instance.fireExists = true; 
        if(collided==true && Input.GetKeyDown("space") && character.GetComponent<PhotonView>().owner == PhotonNetwork.player){
            photonView.RPC("extinguishFire", PhotonTargets.All);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "extinguisher"){
            collided=true;
        }
        if(collision.gameObject.tag == "character"){
            character= collision.gameObject;
        }
    }
    [PunRPC]
    public void extinguishFire()
    {
        Data.Instance.fireExists = false;
        Destroy(gameObject);
    }
}

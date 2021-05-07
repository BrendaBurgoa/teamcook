using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class extinguisher : MonoBehaviour
{
    //clase en los fuegos no en el matafuegos
    public PhotonView photonView;
    private bool collided;
    private GameObject character;
    void Update(){
        //se comunica que existe el fuego para no poder entregar ordenes antes de que se apaguen
        Data.Instance.fireExists = true; 
        //si se colisiono con un matafuegos y se es el que tiene el personae y se presionó espacio, se extiingue el fuego
        if(collided==true && Input.GetKeyDown("space") && character.GetComponent<PhotonView>().isMine){
            photonView.RPC("extinguishFire", PhotonTargets.All);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        //si se colisiona con un matafuegos se registra la colision
        if(collision.gameObject.tag == "extinguisher"){
            collided=true;
        }
        //se guarda quien es el que tiene el matafuegos
        if(collision.gameObject.tag == "character"){
            character= collision.gameObject;
        }
    }
//antes de destruir el fuego se comunica que no queda el fuego 
    [PunRPC]
    public void extinguishFire()
    {
        Data.Instance.fireExists = false;
        Destroy(gameObject);
    }
}

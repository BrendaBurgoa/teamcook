using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObj : Photon.MonoBehaviour
{
    public GameObject character;
    public PhotonView photonView;
    private bool contact;
    private Vector3 initialPosition;
    public string[] includedTags;
    public bool allowed;
    void Start(){
        initialPosition = gameObject.transform.position;
    }

    void Update()
    {
            if(transform.childCount >= 2){
                Destroy(transform.GetChild(0));
            }
            if (Input.GetKeyDown("space") && contact == true && gameObject.transform.childCount == 0 && character.transform.GetChild(0).transform.childCount >=1 && character.GetComponent<PhotonView>().isMine)
                {
//si apreto la barra, un personaje con manos llenas está en contacto y no tengo algo, y el objeto no está excluido, tomo lo que me dan 
                    for(var i =0; i<includedTags.Length; i++){
                        if (character.transform.GetChild(0).transform.GetChild(0).tag == includedTags[i]){
                            allowed = true;
                        }
                    }
                    if(allowed == true)
                    {                       
                        photonView.RPC("pickDrop", PhotonTargets.All, false, character.name, character.transform.GetChild(0).transform.GetChild(0).name);
                    }
                }
            else if (Input.GetKeyDown("space") && contact == true && gameObject.transform.childCount >= 1 && character.transform.GetChild(0).transform.childCount <=1 && character.GetComponent<PhotonView>().isMine)
                {
                    photonView.RPC("pickDrop", PhotonTargets.All, true, character.name, gameObject.transform.GetChild(0).name);
                    //si apreto la barra, un personaje con manos vacias esta en contacto y tengo algo, se lo doy 
                }
    }
    void OnCollisionEnter(Collision other)
    {
        //si entro en colision con un personaje, lo guardo y ficho el contacto
            if (other.gameObject.tag=="character"){
                contact=true;
                character= other.gameObject;
            }
    }

    void OnCollisionExit(Collision other){
        if(other.gameObject.tag == "character"){
            contact=false;
            character= null;
            allowed=false;
        }
    }


    [PunRPC]
    private void pickDrop(bool pickLeave, string name, string objectToPick){
        var objectPicked = GameObject.Find(objectToPick);
        if (objectPicked != null)
        {        
            var chara = GameObject.Find(name);
            var objPhtn = objectPicked.GetComponent<PhotonView>();
            if(pickLeave == false){
                //deja en muebles, transfiere ownership e indica en la clase del objeto que fue dejado
                objPhtn.TransferOwnership(0);
                objectPicked.transform.SetParent(gameObject.transform);                
                objectPicked.transform.localPosition= new Vector3(0f,0f,0f);
                objectPicked.GetComponent<pick_drop>().isLeft = false;
            } else{
                 if(chara.transform.GetChild(0).transform.childCount == 0)
                 {
                    //se transfiere ownership al personaje y se pasa objeto como hijo
                    objPhtn.TransferOwnership(chara.GetComponent<PhotonView>().viewID);
                    var dest = chara.transform.GetChild(0);
                    objectPicked.transform.SetParent(dest.transform);
                    objectPicked.transform.localPosition= new Vector3(0f,0f,0f);
                }
            }    
           objectPicked.GetComponent<pick_drop>().enabled = false;
        }
    }

}

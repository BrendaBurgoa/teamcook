using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pick_drop : Photon.PunBehaviour
{
    public GameObject character;
    public PhotonView charaPhotonView;
    public bool  isGrabbed = false;
    private bool collisionFlag = false;
    private bool leaveOnCounter = false;
  //  public bool takingback = false;
    private string collisionName;
    public bool isLeft = false;
    public string leftWhere;
    public PhotonView photonView;
    public string who;
    private Vector3 lastPosition;
    void Update(){
    if(charaPhotonView != null)
    {

        if(gameObject.tag == "onion" || gameObject.tag == "tomato" || gameObject.tag == "patty" || gameObject.tag == "chopped_bread" )
        {
          if(transform.parent == null)
          {  
                this.photonView.TransferOwnership(PhotonNetwork.player);
                PhotonNetwork.Destroy(gameObject);
            }
        }
            if(transform.parent == null)
            {
                if(isLeft==true){
                    //SI el counter activa que se quiere soltar, llama para soltar
                    photonView.RPC("pickDrop", PhotonTargets.All, "leave");
                    this.photonView.TransferOwnership(0);
                }
                if (Input.GetKeyDown("space") && isGrabbed == false && collisionFlag == true && character.GetComponent<PhotonView>().owner == PhotonNetwork.player){
                    //si apreto barra y no esta agarrado pero si estoy cerca me hago dueño y el agarre se activa
                     this.photonView.TransferOwnership(PhotonNetwork.player);
                    isGrabbed = true;
                }
                if (base.photonView.owner == PhotonNetwork.player && collisionFlag == true && isGrabbed == true)
                {
                    //si el  objeto me pertenece,  Y estoy en contacto Y el objeto esta agarrado
                    if(character.transform.GetChild(0).transform.childCount <=0)
                    //Y si yo no tengo objetos ya agarrrados, puedo llamar para agarrar
                    photonView.RPC("pickDrop", PhotonTargets.All, who);
                }
            }

    }
    }
    void OnCollisionEnter(Collision other)
    {
            if(other.gameObject.tag == "character" ){
                //si la colision es con un personaje character ahora es ese personaje
                character  =other.gameObject;
//                if(character.GetComponent<character>().isGrabbing == false){
                if(character.transform.GetChild(0).transform.childCount <= 0){
                    //si personaje no está agarrando nada se toma su photonview, se reconoce el contacto y quien es 
                    charaPhotonView=character.GetComponent<PhotonView>();
                    collisionFlag = true;
        //            gameObject.transform.position=new Vector3(transform.position.x, transform.position.y+0.01f , transform.position.z);
                    who=other.gameObject.name;
                }
            }
        
    }
    void OnCollisionExit(Collision other)
    {
        if(other.gameObject.tag == "character"){
            isGrabbed = false;
            collisionFlag = false;
            character = null;
        }
    }

    [PunRPC]
    private void pickDrop(string name){
            if(name != "leave"){
                var chara = GameObject.Find(name);
                var dest = chara.transform.GetChild(0);
                gameObject.transform.SetParent(dest.transform);
                transform.localPosition= new Vector3(0f,0f,0f);
                Debug.Log("entra mediante pickdrop");
            }
        }

}

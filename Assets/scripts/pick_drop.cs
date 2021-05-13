using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pick_drop : Photon.PunBehaviour
{
   // public character character;
   // public PhotonView charaPhotonView;
   // public bool  isGrabbed = false;
   // private bool collisionFlag = false;
   // private string collisionName;
   //// public bool isLeft = false;
   // public PhotonView photonView;
   //// public string who;
   // private Vector3 lastPosition;

    //void _____________Update(){
    //    if (charaPhotonView == null) return;

    //    // if(gameObject.tag == "onion" || gameObject.tag == "tomato" || gameObject.tag == "patty" || gameObject.tag == "chopped_bread" )
    //    // {
    //    //   if(transform.parent == null)
    //    //   {  
    //    //       //si el objeto no tiene padre para alguien, esa pesona pide ownership y le dice a todos que lo borren
    //    //         photonView.RPC("DeleteThis", PhotonTargets.All);
    //    //     }
    //    // }
    //    if(transform.parent == null)
    //    {
    //        //if(isLeft==true){
    //        //    //SI el counter activa que se quiere soltar, llama para soltar
    //        //    photonView.RPC("pickDrop", PhotonTargets.All, "leave");
    //        //    this.photonView.TransferOwnership(0);
    //        //}
    //        //if (Input.GetKeyDown("space") && isGrabbed == false && collisionFlag == true && character.GetComponent<PhotonView>().isMine){
    //        //    //si apreto barra y no esta agarrado pero si estoy cerca me hago dueño y el agarre se activa
    //        //        this.photonView.TransferOwnership(PhotonNetwork.player);
    //        //    isGrabbed = true;
    //        //}
    //        if (base.photonView.owner == PhotonNetwork.player && collisionFlag == true && isGrabbed == true)
    //        {
    //            //si el  objeto me pertenece,  Y estoy en contacto Y el objeto esta agarrado
    //            if(character.transform.GetChild(0).transform.childCount <=0)
    //            //Y si yo no tengo objetos ya agarrrados, puedo llamar para agarrar
    //            if(who!= "")
    //                photonView.RPC("pickDrop", PhotonTargets.All, who);
    //        }
    //    }
    //}
    //[PunRPC]
    //private void DeleteThis (){
    //    Destroy(gameObject);
    //}
    //void OnCollisionEnter(Collision other)
    //{
    //    character _character = other.gameObject.GetComponent<character>();
    //    if(_character != null && _character.IsMe())
    //    {
    //        character = _character;
    //        if (!character.HasSomething())
    //        {
    //            charaPhotonView=character.GetComponent<PhotonView>();
    //            collisionFlag = true;
    //            who=other.gameObject.name;
    //        }
    //    }
        
    //}
    //void OnCollisionExit(Collision other)
    //{
    //    character _character = other.gameObject.GetComponent<character>();
    //    if (_character != null && _character.IsMe())
    //    {
    //        isGrabbed = false;
    //        collisionFlag = false;
    //        character = null;
    //    }
    //}
    //[PunRPC]
    //private void pickDrop(string name)
    //{
    //    var chara = GameObject.Find(name);
    //    var dest = chara.transform.GetChild(0);
    //    gameObject.transform.SetParent(dest.transform);
    //    transform.localPosition= new Vector3(0f,0f,0f);
    //    Debug.Log("entra mediante pickdrop");
    //}

}

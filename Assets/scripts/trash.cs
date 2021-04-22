using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trash : Photon.MonoBehaviour
{
    public PhotonView photonView;
    public GameObject pan;
    public GameObject pot;
    private bool collided;
    private GameObject character;
    void Update(){
        //si un personaje se acerca, se chequea que quien apreta espacio sea este
        if (collided && Input.GetKeyDown("space") && character.GetComponent<PhotonView>().owner == PhotonNetwork.player){
            //si el pesonaje tiene un obj agarrado, chequea que no sea un objeto no desechable
            if( character.transform.GetChild(0).transform.GetChild(0) != null){
                var objTrash = character.transform.GetChild(0).transform.GetChild(0).gameObject.tag;
                    if(  
                    objTrash != "extinguisher" && 
                    objTrash != "floor" && 
                    objTrash != "counter" && 
                    objTrash != "stove" )
                    {
                    //chequea si es una olla con, la elimina, crea unanueva y se la da al psje
                    photonView.RPC("throwToBin", PhotonTargets.All, character.transform.GetChild(0).transform.GetChild(0).gameObject.name);
                    if(objTrash == "burnt_pot" 
                        || objTrash == "pot"
                        || objTrash == "potOnionSoup"
                        || objTrash == "potTomatoSoup" )
                    {
                        var newpot =PhotonNetwork.Instantiate(pot.name, transform.position, Quaternion.identity,0);
                        photonView.RPC("GiveToChara", PhotonTargets.All, character.GetComponent<PhotonView>().viewID, newpot.GetComponent<PhotonView>().viewID);
                    }else if(
                        objTrash == "burnt_pan" 
                        || objTrash == "pan"  
                        || objTrash == "cooked_patty" 
                    ){
                        var newpot =PhotonNetwork.Instantiate(pan.name, transform.position, Quaternion.identity,0);
                        photonView.RPC("GiveToChara", PhotonTargets.All, character.GetComponent<PhotonView>().viewID, newpot.GetComponent<PhotonView>().viewID);
                    }
                    }
            }
        }
    }
    void OnCollisionEnter(Collision other)
    {
        //registra la colision con personaje
        if(other.gameObject.tag == "character"){
            collided=true;
            character=other.gameObject;
        }

    }
    void OnCollisionExit(Collision other)
    {
        //resetea la colision con psje
        if(other.gameObject.tag == "character"){
            collided=false;
            character=null;
        }
    }
    
    [PunRPC]
    private void GiveToChara(int charaid, int id){
        //encuentra al objeto creado (olla o sarten), le cambia el nombre y se lo da al personaje deseado
        var obj = PhotonView.Find(id);
        obj.name = obj.name + id; 
        var chara = PhotonView.Find(charaid);
        var dest = chara.transform.GetChild(0);
        obj.transform.SetParent(dest.transform, true);
        obj.transform.localPosition= new Vector3(0f,0f,0f);
    }

    [PunRPC]
    private void throwToBin(string name){
        //desecha el objeto para todos
        Destroy(GameObject.Find(name));
    }
    
}

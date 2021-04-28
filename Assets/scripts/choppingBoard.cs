using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class choppingBoard : MonoBehaviour
{
    public float necessaryTime = 12f;
    float elapsed;
    public Animator animation;
    public GameObject chopped_lettuce;
    public GameObject chopped_bread;
    public GameObject chopped_tomato;
    public GameObject chopped_onion;
    public PhotonView photonView;
    private bool nowChop = false;
    public float val1;
    public float val2;
    private GameObject ingredientToChop;
    public AudioSource chopping;
    private bool startChop = false;
    private bool isPlaying = false;

void Start(){
    chopping = GetComponent<AudioSource>();

}
void Update(){
    if(startChop ==true && Data.Instance.Rol == 0){
        //solo el manager activa el audio de todos, y lleva cuenta de que se está cortando 
            if (gameObject.transform.childCount >= 1){
                elapsed += Time.deltaTime;
                photonView.RPC("PlayChop", PhotonTargets.All, true);
           //     photonView.RPC("ObjectEnablingWhenPlaced", PhotonTargets.All, false);
                if (elapsed > necessaryTime)
                {
                photonView.RPC("ChopForAll", PhotonTargets.All);
              //  photonView.RPC("ObjectEnablingWhenPlaced", PhotonTargets.All, true);
                }
            }
    }
    if(transform.childCount <=0){
        //si no tiene nada adentro se pueden poner cosas
        gameObject.GetComponent<PlaceObj>().enabled =true;
    }
        if(nowChop==true){
            if(Data.Instance.Rol == 0){
                //cuando el manager registra que paso el tiempo para el cambio se instancia lo cortado y destruye el original, se calla el audio y se reactiva el timer
                    ChopChange(ingredientToChop.name, ingredientToChop.tag);  
                    this.photonView.TransferOwnership(0);
                    photonView.RPC("ReActivate", PhotonTargets.All);
                    PhotonNetwork.Destroy(ingredientToChop);
                    photonView.RPC("PlayChop", PhotonTargets.All, false);
                }
            }
}
[PunRPC]
private void ObjectEnablingWhenPlaced(bool enablingState){
                gameObject.GetComponent<PlaceObj>().enabled =enablingState;
}
    void OnCollisionEnter(Collision other)
    {
     if (other.gameObject.tag == "lettuce" || other.gameObject.tag == "tomato" || other.gameObject.tag == "onion")
        {       
            //cuando entra en colision empieza la animacion 
            ingredientToChop = other.gameObject;
        }
    }
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "lettuce" || other.gameObject.tag == "bread" || other.gameObject.tag == "tomato" || other.gameObject.tag == "onion")
        {
            //si no lo dejas para cortar se para la animacion
            startChop=false;
        }
    }
    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "lettuce" || other.gameObject.tag == "bread" || other.gameObject.tag == "tomato" || other.gameObject.tag == "onion")
        {
            startChop=true;
            //mientras este en colision con algo se corta, se anima y se inamilita que se pueda volver a agarrar
             if (gameObject.transform.childCount >= 1){
                photonView.RPC("animateChop", PhotonTargets.All);
             }
            
        }
    }

[PunRPC]
private void ChopForAll(){
    nowChop = true;
    elapsed = 0;
    startChop = false;
}
[PunRPC]
private void PlayChop(bool playstop){
    if(playstop && isPlaying==false)
    {
        chopping.Play();
        isPlaying=true;
    }else if(playstop == false && isPlaying==true){
        chopping.Stop();
        nowChop=false;
        ingredientToChop = null;
        isPlaying=false;
    }
}

    [PunRPC]
    private void animateChop()
    {
            animation.SetTrigger("chop");
    }

    
    [PunRPC]
    private void ReActivate()
    {
            gameObject.GetComponent<PlaceObj>().enabled =true;
        
    }

    private void ChopChange(string name, string tag)
    {
        
        float randomValue = Random.Range(val1, val2);
        if (tag == "lettuce")
        { 
            var ingredient = PhotonNetwork.Instantiate(chopped_lettuce.name, new Vector3(randomValue , transform.position.y, transform.position.z), Quaternion.identity,0); 
            photonView.RPC("DeleteChange", PhotonTargets.All, name, tag, ingredient.GetComponent<PhotonView>().viewID);
        }
        else if (tag == "tomato")
        {
            var ingredient = PhotonNetwork.Instantiate(chopped_tomato.name, new Vector3(randomValue, transform.position.y, transform.position.z), Quaternion.identity,0); 
            photonView.RPC("DeleteChange", PhotonTargets.All, name, tag, ingredient.GetComponent<PhotonView>().viewID);
        }
        else if (tag == "onion")
        {
            var ingredient = PhotonNetwork.Instantiate(chopped_onion.name, new Vector3(randomValue, transform.position.y, transform.position.z), Quaternion.identity,0);  
            photonView.RPC("DeleteChange", PhotonTargets.All, name, tag, ingredient.GetComponent<PhotonView>().viewID);
        }
        else if (tag == "bread")
        {
            var ingredient = PhotonNetwork.Instantiate(chopped_bread.name, new Vector3(randomValue, transform.position.y, transform.position.z), Quaternion.identity,0);  
            photonView.RPC("DeleteChange", PhotonTargets.All, name, tag, ingredient.GetComponent<PhotonView>().viewID);
        }
    }

    

    [PunRPC]
    private void DeleteChange(string name, string tag, int id)
    {
       // Destroy(GameObject.Find(name));
        var ingredient = PhotonView.Find(id);
        ingredient.name = ingredient.name + id; 
        
    }



}

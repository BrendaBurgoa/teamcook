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
void Update(){
    if(startChop ==true && Data.Instance.Rol == 0){
            if (gameObject.transform.childCount >= 1){
                elapsed += Time.deltaTime;
                photonView.RPC("PlayChop", PhotonTargets.All, true);
                if (elapsed > necessaryTime)
                {
                photonView.RPC("ChopForAll", PhotonTargets.All);
                }
            }
    }
    if(transform.childCount <=0){
        gameObject.GetComponent<PlaceObj>().enabled =true;
    }
        if(nowChop==true){
            if(Data.Instance.Rol == 0){
                    ChopChange(ingredientToChop.name, ingredientToChop.tag);  
                    photonView.RPC("ReActivate", PhotonTargets.All);
                    PhotonNetwork.Destroy(ingredientToChop);
                    photonView.RPC("PlayChop", PhotonTargets.All, false);
                }
            }
}
    void OnCollisionEnter(Collision other)
    {
     if (other.gameObject.tag == "lettuce" || other.gameObject.tag == "tomato" || other.gameObject.tag == "onion")
        {        Debug.Log("colisionó con ingrediente y llama animacion");
            ingredientToChop = other.gameObject;
        }
    }
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "lettuce" || other.gameObject.tag == "bread" || other.gameObject.tag == "tomato" || other.gameObject.tag == "onion")
        {
            startChop=false;
        }
    }
    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "lettuce" || other.gameObject.tag == "bread" || other.gameObject.tag == "tomato" || other.gameObject.tag == "onion")
        {
            startChop=true;
            
             if (gameObject.transform.childCount >= 1){
            photonView.RPC("animateChop", PhotonTargets.All);
                other.gameObject.GetComponent<pick_drop>().enabled =false;
                gameObject.GetComponent<PlaceObj>().enabled =false;

//                 elapsed += Time.fixedDeltaTime;
//                 if (elapsed > necessaryTime)
//                 {
//                    chopping.Play();
//                     nowChop = true;
//                     elapsed = 0;
//                 }
             }
            
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
            // nowChop=false;
            // ingredientToChop = null;
            // elapsed = 0;
            // startChop = false;
        
    }

    private void ChopChange(string name, string tag)
    {
        
        float randomValue = Random.Range(val1, val2);
        if (tag == "lettuce")
        { 
//            var ingredient = PhotonNetwork.Instantiate(chopped_lettuce.name, new Vector3(transform.position.x+randomValue , transform.position.y, randomValue), Quaternion.identity,0); 
            var ingredient = PhotonNetwork.Instantiate(chopped_lettuce.name, new Vector3(randomValue , transform.position.y, transform.position.z), Quaternion.identity,0); 
            photonView.RPC("DeleteChange", PhotonTargets.All, name, tag, ingredient.GetComponent<PhotonView>().viewID);
        }
        else if (tag == "tomato")
        {
//            var ingredient = PhotonNetwork.Instantiate(chopped_tomato.name, new Vector3(transform.position.x+randomValue , transform.position.y, randomValue), Quaternion.identity,0); 
            var ingredient = PhotonNetwork.Instantiate(chopped_tomato.name, new Vector3(randomValue, transform.position.y, transform.position.z), Quaternion.identity,0); 
            photonView.RPC("DeleteChange", PhotonTargets.All, name, tag, ingredient.GetComponent<PhotonView>().viewID);
        }
        else if (tag == "onion")
        {
//            var ingredient = PhotonNetwork.Instantiate(chopped_onion.name, new Vector3(transform.position.x+randomValue , transform.position.y, randomValue), Quaternion.identity,0);  
            var ingredient = PhotonNetwork.Instantiate(chopped_onion.name, new Vector3(randomValue, transform.position.y, transform.position.z), Quaternion.identity,0);  
            photonView.RPC("DeleteChange", PhotonTargets.All, name, tag, ingredient.GetComponent<PhotonView>().viewID);
        }
        else if (tag == "bread")
        {
//            var ingredient = PhotonNetwork.Instantiate(chopped_bread.name, new Vector3(transform.position.x+randomValue , transform.position.y, randomValue), Quaternion.identity,0);  
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

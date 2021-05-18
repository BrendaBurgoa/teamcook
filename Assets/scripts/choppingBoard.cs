using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class choppingBoard : MonoBehaviour
{
    public float necessaryTime = 2f;
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
                Debug.Log(elapsed +"."+ necessaryTime);
            }
    }
        if (elapsed >= necessaryTime && Data.Instance.Rol == 0)
        {
            //pasado el tiempo se instancia la version cortada, y se borra el ingrediente sin cortar
            Debug.Log("termina tiempo");
            photonView.RPC("ChopForAll", PhotonTargets.All);
            ChopChange();  
            ingredientToChop.GetComponent<PhotonView>().TransferOwnership(0);
            PhotonNetwork.Destroy(GetComponent<Transform>().GetChild(0).gameObject);
            photonView.RPC("PlayChop", PhotonTargets.All, false);

        }
    if(transform.childCount ==0){
        //si no tiene nada adentro se pueden poner cosas
        gameObject.GetComponent<PlaceObj>().enabled =true;
    }
}

    void OnCollisionEnter(Collision other)
    {
     if (other.gameObject.tag == "lettuce" || other.gameObject.tag == "tomato" || other.gameObject.tag == "onion")
        {   
            startChop=true;    
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
            elapsed = 0;
        }
    }
    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "lettuce" || other.gameObject.tag == "bread" || other.gameObject.tag == "tomato" || other.gameObject.tag == "onion")
        {

            //mientras este en colision con algo se corta, se anima y se inamilita que se pueda volver a agarrar
             if (gameObject.transform.childCount >= 1){
                photonView.RPC("animateChop", PhotonTargets.All);
             }
            
        }
    }

[PunRPC]
private void ChopForAll(){
    //indicar que terminó el tiempo y ahora se tiene q instanciar la version cortada y reiniciar el tiempo
    startChop = false;
    nowChop = true;
    elapsed = 0;
    Debug.Log("ENTRA A CHOP FOR ALL" + startChop);
}
[PunRPC]
private void PlayChop(bool playstop){
    //animar o parar la animación del cuchillo
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

    private void ChopChange()
    {
        //se ve el tag del objeto en la tabla y dependiendo se instancia la version cortada correspondiente en un lugar random cerca de la tabla de cortar
        Debug.Log("entra a chopchange");
        var tag = ingredientToChop.tag;
        float randomValue = Random.Range(val1, val2);
        if (tag == "lettuce")
        { 
            var ingredient = PhotonNetwork.Instantiate(chopped_lettuce.name, new Vector3(randomValue , transform.position.y, transform.position.z), Quaternion.identity,0); 
             photonView.RPC("DeleteChange", PhotonTargets.All, ingredient.GetComponent<PhotonView>().viewID);

        }
        else if (tag == "tomato")
        {
            var ingredient = PhotonNetwork.Instantiate(chopped_tomato.name, new Vector3(randomValue, transform.position.y, transform.position.z), Quaternion.identity,0); 
             photonView.RPC("DeleteChange", PhotonTargets.All, ingredient.GetComponent<PhotonView>().viewID);

        }
        else if (tag == "onion")
        {
            var ingredient = PhotonNetwork.Instantiate(chopped_onion.name, new Vector3(randomValue, transform.position.y, transform.position.z), Quaternion.identity,0);  
             photonView.RPC("DeleteChange", PhotonTargets.All, ingredient.GetComponent<PhotonView>().viewID);

        }
        else if (tag == "bread")
        {
            var ingredient = PhotonNetwork.Instantiate(chopped_bread.name, new Vector3(randomValue, transform.position.y, transform.position.z), Quaternion.identity,0);  
             photonView.RPC("DeleteChange", PhotonTargets.All, ingredient.GetComponent<PhotonView>().viewID);

        }
    }

    

    [PunRPC]
    private void DeleteChange(int id)
    {
        //se le asigna un nombre unico al ingrediente cortado
        Debug.Log(GetComponent<Transform> ().GetChild (0).gameObject.name);
        var ingredient = PhotonView.Find(id);
        ingredient.name = ingredient.name + id; 
        
    }



}

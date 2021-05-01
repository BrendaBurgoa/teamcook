using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pot : MonoBehaviour
{
    private int tomatoes; 
    private int onions;
    public GameObject tomato_soup;
    public GameObject onion_soup;
    public GameObject burntPot;
    public GameObject newPot;
    public GameObject onionUI;
    public GameObject tomatoUI;
    public GameObject filledTomato;
    public GameObject filledOnion;
    public GameObject showBurnt;
    public bool beginTimer = false;
    private bool collided = false;
    private bool ready = false;
    public bool onstove = false;
    public Image timer;
    public Text quantity;
    public GameObject timerCanvas;
	public float waitTime = 25.0f;
    public float cookingTime;
    public PhotonView photonView;
    public GameObject character;
    public GameObject buttonCook;
    public AudioSource burner;
    private bool isPlaying = false;
   void Start(){
       //se desactivan los elementos UI
            timerCanvas.SetActive(false);
            buttonCook.SetActive(false);
            timer.enabled=false;
   }
    void Update(){
        //si comienza la cocción y está sobre el horno se desactiva el boton de cocción, se activa el timer y se enciende el sonido
        if (beginTimer == true && onstove==true){
            buttonCook.SetActive(false);
                cookingTime = cookingTime + Time.deltaTime;
                if(cookingTime > 15){
                    //si se excede el tiempo de cocción se desactiva el marcador de tiempo y se para el sonido, si los ingredientes alcanzan se llama a la función que crea lo cocido y se destruye la olla actual
                    beginTimer = false;
                    photonView.RPC("PutSound", PhotonTargets.All, false);
                    if(tomatoes >= 3 || onions >= 3 ){
                        CookedVersion();
                        Destroy(gameObject);
                    }
                }
                //timer visual se actualiza
                photonView.RPC("timerFunction", PhotonTargets.All, (1.0f/20 * Time.deltaTime));
        }

        //si la olla tiene los elementos necesarios se activa el boton para comenzar la cocción
        if(onions >= 3 || tomatoes >= 3 ){
            buttonCook.SetActive(true);
        }
        //si se introduce un ingrediente se muestra la pista visual
        if(onions >= 1 ){
            filledOnion.SetActive(true);
        }else if(tomatoes >= 1 ){
            filledTomato.SetActive(true);
        }
        // if(ready){
        //     if(tomatoes >= 3 || onions >= 3 ){
        //         CookedVersion();
        //         Destroy(gameObject);
        //     }
        // }
    }

    public void CookSoup()
    {
        if(collided && character.GetComponent<PhotonView>().isMine){
            beginTimer = true;
            photonView.RPC("PutSound", PhotonTargets.All, true);
            photonView.RPC("showTimers", PhotonTargets.All);
            }
    }

[PunRPC] 
private void PutSound(bool playstop){
    if(playstop && isPlaying==false)
    {
        burner.Play();
        isPlaying=true;
    }else if(playstop == false && isPlaying==true){
        burner.Stop();
        isPlaying=false;
    }
}    
[PunRPC]
    private void showTimers(){
            timer.enabled = true;
    }

    private void CookedVersion()
    {   
        //se fija que sopa instanciar e inicializa las variables
        if(tomatoes == 3){
            this.photonView.TransferOwnership(PhotonNetwork.player);
                instantiateFood("TSoup");
        }
        if(onions == 3){
            this.photonView.TransferOwnership(PhotonNetwork.player);
                instantiateFood("OSoup");
        }
        photonView.RPC("initializeVars", PhotonTargets.All);
    }



    void OnCollisionEnter(Collision other)
    {
        if(onstove==true)
        { 
            //si no tiene ingrediente acepta tomate o cebolla, sino acepta solo el que ya esté en la olla
            if(onions == 0 && tomatoes == 0){
                cookIngredient(other.gameObject.tag, other.gameObject.name);
            }
            else if(other.gameObject.tag == "chopped_onion" && tomatoes == 0){             
                cookIngredient(other.gameObject.tag, other.gameObject.name);
            }
            else if(other.gameObject.tag == "chopped_tomato" && onions == 0){             
                cookIngredient(other.gameObject.tag, other.gameObject.name);
            }

}           

        if(other.gameObject.tag=="character"){
                collided=true;
                character = other.gameObject;
            }
        if(other.gameObject.tag == "stove"){
            onstove=true;
            gameObject.transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
        }
    }
    
     void OnCollisionStay(Collision other)
    {
        if(other.gameObject.tag == "stove"){
           onstove = true;

        }
    }
    void OnCollisionExit(Collision other){
        
        if(other.gameObject.tag == "character"){
            collided=false;
            character = null;
        }
        if(other.gameObject.tag == "stove"){
            onstove=false;
        }
    }
    [PunRPC]
    private void timerFunction( float filler)
    {
			timer.fillAmount -= filler;
    }
    [PunRPC]
    private void SetIngredient(int ingredientCount, string name, int which){
            switch (which){
            case 1: onions= ingredientCount;
                    quantity.text = "X"+onions;
                break;
            case 2: tomatoes= ingredientCount;
                    quantity.text = "X"+tomatoes;
                break;
            }
            timerCanvas.SetActive(true);
            Destroy(GameObject.Find(name));
    }
    private void cookIngredient(string tag, string name)
    {    
        //aumenta el contador y llama ala funcion que muestra cuandos hay en UI
            if (tag == "chopped_onion")
            {
                onions = onions + 1;
                 photonView.RPC("SetIngredient", PhotonTargets.All, onions, name, 1);
            }
            else if (tag == "chopped_tomato")
            {
                tomatoes = tomatoes +1;
                photonView.RPC("SetIngredient", PhotonTargets.All, tomatoes, name, 2);
            }
    }
    private void instantiateFood(string which)
    {
        float randomValue = Random.Range(0.5f, 1f);
        if (which == "OSoup")
        {
            var newPot = PhotonNetwork.Instantiate(onion_soup.name, new Vector3(transform.position.x, transform.position.y, transform.position.z-randomValue ), transform.rotation,0);
            photonView.RPC("UniquePot", PhotonTargets.All, newPot.GetComponent<PhotonView>().viewID);
        }
        else if (which == "TSoup")
        {
            var newPot = PhotonNetwork.Instantiate(tomato_soup.name, new Vector3(transform.position.x, transform.position.y, transform.position.z-randomValue ), transform.rotation,0);
            photonView.RPC("UniquePot", PhotonTargets.All, newPot.GetComponent<PhotonView>().viewID);
        }else if (which == "burnt"){
            PhotonNetwork.Instantiate(burntPot.name, new Vector3(transform.position.x , transform.position.y, transform.position.z-randomValue), transform.rotation,0);
        }
        
        PhotonNetwork.Destroy(gameObject);
    }

    [PunRPC]
    private void UniquePot(int id){
        var createdPot = PhotonView.Find(id);
        createdPot.name = createdPot.name + PhotonView.Find(id); 
    }

    [PunRPC]
    private void initializeVars ()
    {
        tomatoes=0;
        onions=0;
        cookingTime=0;
        beginTimer = false;
        timer.fillAmount= 1;
        quantity.text = "";
    }
    

}

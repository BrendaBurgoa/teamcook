using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pan : MonoBehaviour
{
    public GameObject cooked_patty;
    public GameObject showPatty;
    [SerializeField]public bool beginTimer = false;
    private bool cookingPatty = false;
    private bool isCooking = false;
    private bool collided = false;
    public bool onstove = false;
    public Image timer;
	public float waitTime = 13.0f;
    public float cookingTime;
    public PhotonView photonView;
    public GameObject timerCanvas;
    public GameObject buttonCook;
    public AudioSource burner;
    public GameObject character;
    private bool isPlaying = false;

      void Start(){
            timerCanvas.SetActive(false);
            buttonCook.SetActive(false);
            timer.enabled=false;
   }
    void Update(){
        if (beginTimer == true && onstove==true){
            //si ya se puede comenzar el tiempo y está sobre el honro se activa el timer y se incrementa el tiempo
            timerCanvas.SetActive(true);
            cookingTime += Time.deltaTime;
            if (cookingTime > 10)
            {
                //si el tiempo de coccion supera el tiempo predeterminado se apaga el timer y se instancia la versión cocinada
                beginTimer=false;
                CookedVersion();
                photonView.RPC("PutSound", PhotonTargets.All, false);
            }
            photonView.RPC("timerFunction", PhotonTargets.All, (1.0f/waitTime * Time.deltaTime));

        }
    }
    
    public void CookBtn()
    {
        //si ya se colocó un patty y está sobre el horno se activa el timer
        if (onstove==true && cookingPatty == true){
            if(collided && character.GetComponent<PhotonView>().isMine){
                beginTimer = true;
                photonView.RPC("PutSound", PhotonTargets.All, true);
                photonView.RPC("showTimers", PhotonTargets.All);
            }
         }
    }
[PunRPC]
    private void showTimers(){
            timer.enabled = true;
    }
    private void CookedVersion()
    {   
        //si se colocó un patty se instancia un patty cocido y se destruye el crudo
        if(cookingPatty){
                instantiateFood("patty");
                this.photonView.TransferOwnership(PhotonNetwork.player);
                PhotonNetwork.Destroy(gameObject); 
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
    void OnCollisionEnter(Collision other)
    {
        //se chequea que esté sobre el horno
        if(other.gameObject.tag == "stove"){
            onstove=true;
            gameObject.transform.rotation = Quaternion.Euler(0.0f, -90.0f, 0.0f); 
            timerCanvas.SetActive(false);
        }
        //se chequea quien es el personaje
        if(other.gameObject.tag=="character"){
                collided=true;
                character = other.gameObject;
        }
        //se muestra el patty en la sarten y se borra el patty como ingrediente en sí
        if(other.gameObject.tag == "patty" && isCooking == false){
            photonView.RPC("ShowPatty", PhotonTargets.All, other.gameObject.name);
        }
    }
     void OnCollisionStay(Collision other)
    {
        if(other.gameObject.tag == "stove"){
           onstove = true;
            gameObject.transform.rotation = Quaternion.Euler(0.0f, -90.0f, 0.0f); 
        }
    }
    void OnCollisionExit(Collision other)
    {
        if(other.gameObject.tag == "stove"){
            onstove=false;
        }
        if(other.gameObject.tag== "character"){
            collided=false;
            character = null;
        }
    }
    [PunRPC]
    private void timerFunction(float filler)
    {
			timer.fillAmount -= filler;
            
    }

    private void CookIngredient()
    {    
        //se muestra el patty y se activa el boton para comenzar a cocinar
                timerCanvas.SetActive(true);
                buttonCook.SetActive(true);
                cookingPatty = true;
    }
    [PunRPC]
    private void ShowPatty(string name){
                    Destroy(GameObject.Find(name));
                    isCooking = true;
                    showPatty.SetActive(true);
                    CookIngredient();
    }
    private void instantiateFood(string which)
    {
        float randomValue = Random.Range(0.5f, 1f);
        if(which == "patty")
        {
            var newPan = PhotonNetwork.Instantiate(cooked_patty.name, transform.position, transform.rotation,0);
            photonView.RPC("UniquePan", PhotonTargets.All, newPan.GetComponent<PhotonView>().viewID);

        }
    }

    [PunRPC]
    private void UniquePan(int id){
        //se da un nombre unico al patty cocido instanciado
        var createdPot = PhotonView.Find(id);
        createdPot.name = createdPot.name + PhotonView.Find(id); 
    }

    [PunRPC]
    private void initializeVars ()
    {
        cookingTime=0;
        beginTimer = false;
        timer.fillAmount= 1;
        isCooking = false;
        cookingPatty = false;
    }
    

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pan : MonoBehaviour
{
    public GameObject cooked_patty;
    public GameObject burntPot;
    public GameObject showPatty;
    [SerializeField]public bool beginTimer = false;
    private bool isBurnt = false;
    private bool halfBurnt = false;
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
     //       buttonCook.SetActive(false);
            timerCanvas.SetActive(true);
            cookingTime += Time.deltaTime;
            if (cookingTime > 10)
            {
                beginTimer=false;
                CookedVersion();
                photonView.RPC("PutSound", PhotonTargets.All, false);
            }
            photonView.RPC("timerFunction", PhotonTargets.All, (1.0f/waitTime * Time.deltaTime));

        }
    }
    
    public void CookBtn()
    {
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
        if(other.gameObject.tag == "stove"){
            onstove=true;
            gameObject.transform.rotation = Quaternion.Euler(0.0f, -90.0f, 0.0f); 
        }
        if(other.gameObject.tag=="character"){
                collided=true;
                character = other.gameObject;
        }
        if(other.gameObject.tag == "patty" && isCooking == false){
            photonView.RPC("ShowPatty", PhotonTargets.All, other.gameObject.name);
        }
    }
     void OnCollisionStay(Collision other)
    {
        if(other.gameObject.tag == "stove"){
           onstove = true;
        }
    }
    void OnCollisionExit(Collision other)
    {
        if(other.gameObject.tag == "stove"){
            onstove=false;
        //    timerCanvas.SetActive(false);
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
        else if (which == "burnt"){
            PhotonNetwork.Instantiate(burntPot.name, transform.position, Quaternion.identity,0);

        }
    }

    [PunRPC]
    private void UniquePan(int id){
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


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class orders : MonoBehaviour
{
    public int simpleBurger;
    public int fullBurger;
    public int simpleBurgerFries;
    public int fullBurgerFries;
    public int tomatoSoup;
    public int onionSoup;
    public int simpleSalad;
    public int fullSalad;

    public Text text_points;

    public PhotonView photonView;
    public AudioSource clap;
    void Awake()
    {
        Events.NewOrder += NewOrder;
    }
    void Start(){
        clap = GetComponent<AudioSource>();
    }
    void OnDestroy()
    {
        Events.NewOrder -= NewOrder;
    }
    void NewOrder()
    {
        GetOrdersQty();
    }
    void GetOrdersQty(){

        simpleBurger = GameObject.FindGameObjectsWithTag("SBurgerOrder").Length;
        fullBurger= GameObject.FindGameObjectsWithTag("FBurgerOrder").Length;
        simpleBurgerFries= GameObject.FindGameObjectsWithTag("SBurgerFriesOrder").Length;
        fullBurgerFries= GameObject.FindGameObjectsWithTag("FBurgerFriesOrder").Length;
        tomatoSoup= GameObject.FindGameObjectsWithTag("TSoupOrder").Length;
        onionSoup= GameObject.FindGameObjectsWithTag("OSoupOrder").Length;
        simpleSalad= GameObject.FindGameObjectsWithTag("SSaladOrder").Length;
        fullSalad= GameObject.FindGameObjectsWithTag("FSaladOrder").Length;
    }

    public void OnSelect(character character)
    {
        print("OnSelect orders " + Data.Instance.fireExists);
        if (Data.Instance.fireExists == true) return;

        PhotonView pv = character.HasSomethingToDrop();
        if (pv != null)
            RecognizeOrder(pv.tag, pv);
    }
    
    private void RecognizeOrder(string collisionTag, PhotonView plate){

        GetOrdersQty();
        print("RecognizeOrder " + collisionTag);

        if (collisionTag == "simpleBurger" && simpleBurger>0)
        {
            photonView.RPC("ChangePoints", PhotonTargets.All, plate.name);
            photonView.RPC("DeleteLatest", PhotonTargets.All, "SBurgerOrder");
        }
        else if(collisionTag == "fullBurger" && fullBurger>0)
        {
            photonView.RPC("ChangePoints", PhotonTargets.All, plate.name);
            photonView.RPC("DeleteLatest", PhotonTargets.All, "FBurgerOrder");
        }
        else if(collisionTag == "fullBurgerFries" && fullBurgerFries>0)
        {
            photonView.RPC("ChangePoints", PhotonTargets.All, plate.name);
            photonView.RPC("DeleteLatest", PhotonTargets.All, "FBurgerFriesOrder"); 
        }
        else if(collisionTag == "simpleBurgerFries" && simpleBurgerFries>0)
        {
            photonView.RPC("ChangePoints", PhotonTargets.All, plate.name);
            photonView.RPC("DeleteLatest", PhotonTargets.All, "SBurgerFriesOrder");
        }

        else if(collisionTag == "simpleSalad" && simpleSalad>0)
        { 
            photonView.RPC("ChangePoints", PhotonTargets.All,  plate.name);
            photonView.RPC("DeleteLatest", PhotonTargets.All, "SSaladOrder");
        }
        else if(collisionTag == "fullSalad" && fullSalad>0)
        {
            photonView.RPC("ChangePoints", PhotonTargets.All,   plate.name);
            photonView.RPC("DeleteLatest", PhotonTargets.All, "FSaladOrder");
        }
        else if(collisionTag == "onionSoup" && onionSoup>0)
        {
            photonView.RPC("ChangePoints", PhotonTargets.All, plate.name);
            photonView.RPC("DeleteLatest", PhotonTargets.All, "OSoupOrder");
        }
        else if(collisionTag == "tomatoSoup" && tomatoSoup>0)
        {         
            photonView.RPC("ChangePoints", PhotonTargets.All,   plate.name);   
            photonView.RPC("DeleteLatest", PhotonTargets.All, "TSoupOrder");
        }
    }

    [PunRPC]
    private void ChangePoints(string receivedplate){
        clap.Play();        
        Destroy(GameObject.Find(receivedplate));
        Data.Instance.TimelyOrders++;
        Events.OnRefreshPoints();
    }

    [PunRPC]    
    private void DeleteLatest(string tag){
        var listButton = GameObject.FindGameObjectsWithTag(tag);
        Destroy(listButton[0].gameObject);
    }
    

}

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

    void Start(){
        clap = GetComponent<AudioSource>();
    }

    void Update(){
        simpleBurger = GameObject.FindGameObjectsWithTag("SBurgerOrder").Length;
        fullBurger= GameObject.FindGameObjectsWithTag("FBurgerOrder").Length;
        simpleBurgerFries= GameObject.FindGameObjectsWithTag("SBurgerFriesOrder").Length;
        fullBurgerFries= GameObject.FindGameObjectsWithTag("FBurgerFriesOrder").Length;
        tomatoSoup= GameObject.FindGameObjectsWithTag("TSoupOrder").Length;
        onionSoup= GameObject.FindGameObjectsWithTag("OSoupOrder").Length;
        simpleSalad= GameObject.FindGameObjectsWithTag("SSaladOrder").Length;
        fullSalad= GameObject.FindGameObjectsWithTag("FSaladOrder").Length;

    }

    void OnCollisionEnter(Collision other)
    {
        if(Data.Instance.fireExists == false && other.gameObject.tag == "character"){
            if(other.gameObject.GetComponent<PhotonView>().owner == PhotonNetwork.player && other.gameObject.transform.GetChild(0).transform.GetChild(0).gameObject != null){
                var plate = other.gameObject.transform.GetChild(0).transform.GetChild(0).gameObject;
                RecognizeOrder(plate.tag, plate);
            }
        }
    }
    
private void RecognizeOrder(string collisionTag, GameObject plate){
            if(collisionTag == "simpleBurger" && simpleBurger>0)
            {
                    photonView.RPC("ChangePoints", PhotonTargets.All, Data.Instance.TimelyOrders+1, plate.name);
                    photonView.RPC("DeleteLatest", PhotonTargets.All, "SBurgerOrder");
                    // simpleBurger=simpleBurger-1;
            }
            else if(collisionTag == "fullBurger" && fullBurger>0)
            {
                    photonView.RPC("ChangePoints", PhotonTargets.All, Data.Instance.TimelyOrders+1, plate.name);
                    photonView.RPC("DeleteLatest", PhotonTargets.All, "FBurgerOrder");
                    // fullBurger = fullBurger-1;
            }
            else if(collisionTag == "fullBurgerFries" && fullBurgerFries>0)
            {
                photonView.RPC("ChangePoints", PhotonTargets.All, Data.Instance.TimelyOrders+1, plate.name);
                    photonView.RPC("DeleteLatest", PhotonTargets.All, "FBurgerFriesOrder"); 
                    // fullBurgerFries=fullBurgerFries-1;
            }
            else if(collisionTag == "simpleBurgerFries" && simpleBurgerFries>0)
            {
                    photonView.RPC("ChangePoints", PhotonTargets.All, Data.Instance.TimelyOrders+1, plate.name);
                    photonView.RPC("DeleteLatest", PhotonTargets.All, "SBurgerFriesOrder");
                    // simpleBurgerFries=simpleBurgerFries-1;
            }

            else if(collisionTag == "simpleSalad" && simpleSalad>0)
            { 
                        photonView.RPC("ChangePoints", PhotonTargets.All, Data.Instance.TimelyOrders+1, plate.name);
                        photonView.RPC("DeleteLatest", PhotonTargets.All, "SSaladOrder");
                        // simpleSalad=simpleSalad-1;
            }
            else if(collisionTag == "fullSalad" && fullSalad>0)
            {
                    photonView.RPC("ChangePoints", PhotonTargets.All, Data.Instance.TimelyOrders+1, plate.name);
                    photonView.RPC("DeleteLatest", PhotonTargets.All, "FSaladOrder");
                    // fullSalad=fullSalad-1;
            }
            else if(collisionTag == "onionSoup" && onionSoup>0)
            {
                    photonView.RPC("ChangePoints", PhotonTargets.All, Data.Instance.TimelyOrders+1, plate.name);
                    photonView.RPC("DeleteLatest", PhotonTargets.All, "OSoupOrder");
                    // onionSoup=onionSoup-1;
            }
            else if(collisionTag == "tomatoSoup" && tomatoSoup>0)
            {         
                    photonView.RPC("ChangePoints", PhotonTargets.All, Data.Instance.TimelyOrders+1, plate.name);   
                    photonView.RPC("DeleteLatest", PhotonTargets.All, "TSoupOrder");
                    // tomatoSoup=tomatoSoup-1;
            }
}

    [PunRPC]
    private void ChangePoints(int newPoints, string receivedplate){
        clap.Play();
        Data.Instance.TimelyOrders=newPoints;
        Destroy(GameObject.Find(receivedplate));
    }
    [PunRPC]
    public void PickOrder(int order){
        // if(order == 1)
        // {
        //     simpleBurger = simpleBurger + 1;
        // }
        // else if(order == 2)
        // {
        //     fullBurger = fullBurger + 1;
        // }
        // else if(order == 3)
        // {
        //     tomatoSoup = tomatoSoup + 1;
        // }
        // else if(order == 4)
        // {
        //     onionSoup = onionSoup + 1;
        // }
        // else if(order == 5)
        // {
        //     simpleSalad = simpleSalad + 1;
        // }
        // else if(order == 6)
        // {
        //     fullSalad = fullSalad + 1;
        // }
        // else if(order == 7)
        // {
        //     fullBurgerFries = fullBurgerFries + 1;
        // }
        // else if(order == 8)
        // {
        //     simpleBurgerFries = simpleBurgerFries + 1;
        // }
        Debug.Log("simpleBurger"+simpleBurger+"fullBurger"+fullBurger+"tomatoSoup"+tomatoSoup+"onionSoup"+onionSoup+"simpleSalad" +simpleSalad + "fullSalad"+ fullSalad);
    
    }
    [PunRPC]    
    private void DeleteLatest(string tag){
        var listButton = GameObject.FindGameObjectsWithTag(tag);
        Destroy(listButton[0].gameObject);
    }
    
    public void MakeOrder(int order)
    {
       photonView.RPC("PickOrder", PhotonTargets.All, order);
    }

}

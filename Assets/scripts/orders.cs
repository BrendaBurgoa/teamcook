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
        Events.NewOrder += NewOrder;
    }
    void OnDestroy()
    {
        Events.NewOrder -= NewOrder;
    }
    void NewOrder()
    {
        _Update();
    }
    void _Update(){
        //la cantidad de ordenes se define segun la cantidad de objetos con esa tag que haya
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
        //se deja entregar si no existe fuego y existe una orden para ese plato
        if(Data.Instance.fireExists == false && other.gameObject.tag == "character"){
            if(other.gameObject.GetComponent<PhotonView>().owner == PhotonNetwork.player && other.gameObject.transform.GetChild(0).transform.GetChild(0).gameObject != null){
                var plate = other.gameObject.transform.GetChild(0).transform.GetChild(0).gameObject;
                RecognizeOrder(plate.tag, plate);
            }
        }
    }
    
    private void RecognizeOrder(string collisionTag, GameObject plate){
        //se corrobora que exista el pedido y se elimina una de las ordenes para entregar
            if(collisionTag == "simpleBurger" && simpleBurger>0)
            {
                    photonView.RPC("ChangePoints", PhotonTargets.All, Data.Instance.TimelyOrders+1, plate.name);
                    photonView.RPC("DeleteLatest", PhotonTargets.All, "SBurgerOrder");
            }
            else if(collisionTag == "fullBurger" && fullBurger>0)
            {
                    photonView.RPC("ChangePoints", PhotonTargets.All, Data.Instance.TimelyOrders+1, plate.name);
                    photonView.RPC("DeleteLatest", PhotonTargets.All, "FBurgerOrder");
            }
            else if(collisionTag == "fullBurgerFries" && fullBurgerFries>0)
            {
                photonView.RPC("ChangePoints", PhotonTargets.All, Data.Instance.TimelyOrders+1, plate.name);
                    photonView.RPC("DeleteLatest", PhotonTargets.All, "FBurgerFriesOrder"); 
            }
            else if(collisionTag == "simpleBurgerFries" && simpleBurgerFries>0)
            {
                    photonView.RPC("ChangePoints", PhotonTargets.All, Data.Instance.TimelyOrders+1, plate.name);
                    photonView.RPC("DeleteLatest", PhotonTargets.All, "SBurgerFriesOrder");
            }

            else if(collisionTag == "simpleSalad" && simpleSalad>0)
            { 
                        photonView.RPC("ChangePoints", PhotonTargets.All, Data.Instance.TimelyOrders+1, plate.name);
                        photonView.RPC("DeleteLatest", PhotonTargets.All, "SSaladOrder");
            }
            else if(collisionTag == "fullSalad" && fullSalad>0)
            {
                    photonView.RPC("ChangePoints", PhotonTargets.All, Data.Instance.TimelyOrders+1, plate.name);
                    photonView.RPC("DeleteLatest", PhotonTargets.All, "FSaladOrder");
            }
            else if(collisionTag == "onionSoup" && onionSoup>0)
            {
                    photonView.RPC("ChangePoints", PhotonTargets.All, Data.Instance.TimelyOrders+1, plate.name);
                    photonView.RPC("DeleteLatest", PhotonTargets.All, "OSoupOrder");
            }
            else if(collisionTag == "tomatoSoup" && tomatoSoup>0)
            {         
                    photonView.RPC("ChangePoints", PhotonTargets.All, Data.Instance.TimelyOrders+1, plate.name);   
                    photonView.RPC("DeleteLatest", PhotonTargets.All, "TSoupOrder");
            }
    }

    [PunRPC]
    private void ChangePoints(int newPoints, string receivedplate){
    //cuando se entrega se modifica el numero de entregas, se reproduce un audio y se destuye el plato entregado
        clap.Play();
        Data.Instance.TimelyOrders=newPoints;
        Destroy(GameObject.Find(receivedplate));
        Events.OnRefreshPoints();
    }

    [PunRPC]    
    private void DeleteLatest(string tag){
        var listButton = GameObject.FindGameObjectsWithTag(tag);
        Destroy(listButton[0].gameObject);
    }
    

}

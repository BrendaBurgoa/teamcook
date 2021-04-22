using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ordersViejo : MonoBehaviour
{
    /*
    private int simpleBurger;
    private int fullBurger;
    private int simpleBurgerFries;
    private int fullBurgerFries;
    private int tomatoSoup;
    private int onionSoup;
    private int simpleSalad;
    private int fullSalad;
    
    // public Image img_simpleBurger;
    // public Image img_fullBurger;
    // public Image img_tomatoSoup;
    // public Image img_onionSoup;
    // public Image img_simpleSalad;
    // public Image img_fullSalad;
    
    public Text text_points;

    public PhotonView photonView;

    public int LateOsoup;
    public int LateTsoup;
    public int LateSburger;
    public int LateFburger;
    public int LateSsalad;
    public int LateFsalad;
    public int LateSburgerFries;
    public int LateFburgerFries;

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "simpleBurger")
        {
            if(simpleBurger>0){
                if(Data.Instance.LateSburger>=1){
                    photonView.RPC("ChangePoints", PhotonTargets.All, Data.Instance.Point+10);
                    LateSburger=Data.Instance.LateSburger-1;
                    photonView.RPC("setDataValues", PhotonTargets.All, LateSburger,"SimpleBurger");
                }
                else if (Data.Instance.LateSburger<=0){
                    photonView.RPC("ChangePoints", PhotonTargets.All, Data.Instance.Point+20);
                }
                PhotonNetwork.Destroy(other.gameObject);
//                DeleteLatest("SBurgerOrder");
                photonView.RPC("DeleteLatest", PhotonTargets.All, "SBurgerOrder");
            }
        }
        else if(other.gameObject.tag == "fullBurger")
        {
            if(fullBurger>0){
                if(Data.Instance.LateFburger>=1){
                    photonView.RPC("ChangePoints", PhotonTargets.All, Data.Instance.Point+10);
                    LateFburger=Data.Instance.LateFburger-1;
                    photonView.RPC("setDataValues", PhotonTargets.All, LateFburger,"FullBurger");
                }
                else if (Data.Instance.LateFburger<=0){
                    photonView.RPC("ChangePoints", PhotonTargets.All, Data.Instance.Point+20);
                }
                PhotonNetwork.Destroy(other.gameObject);
                    //DeleteLatest("FBurgerOrder");
                photonView.RPC("DeleteLatest", PhotonTargets.All, "FBurgerOrder");
            }
            
        }
        else if(other.gameObject.tag == "fullBurgerFries")
        {
            if(fullBurger>0){
                if(Data.Instance.LateFburger>=1){
                    photonView.RPC("ChangePoints", PhotonTargets.All, Data.Instance.Point+10);
                    LateFburgerFries=Data.Instance.LateFburgerFries-1;
                    photonView.RPC("setDataValues", PhotonTargets.All, LateFburgerFries,"FullBurgerFries");
                }
                else if (Data.Instance.LateFburgerFries<=0){
                    photonView.RPC("ChangePoints", PhotonTargets.All, Data.Instance.Point+20);
                }
                PhotonNetwork.Destroy(other.gameObject);
                photonView.RPC("DeleteLatest", PhotonTargets.All, "FBurgerFriesOrder");
            }
            
        }


        else if(other.gameObject.tag == "simpleBurgerFries")
        {
            if(fullBurger>0){
                if(Data.Instance.LateFburger>=1){
                    photonView.RPC("ChangePoints", PhotonTargets.All, Data.Instance.Point+10);
                    LateSburgerFries=Data.Instance.LateSburgerFries-1;
                    photonView.RPC("setDataValues", PhotonTargets.All, LateFburgerFries,"SimpleBurgerFries");
                }
                else if (Data.Instance.LateFburgerFries<=0){
                    photonView.RPC("ChangePoints", PhotonTargets.All, Data.Instance.Point+20);
                }
                PhotonNetwork.Destroy(other.gameObject);
                photonView.RPC("DeleteLatest", PhotonTargets.All, "SBurgerFriesOrder");
            }
            
        }

        else if(other.gameObject.tag == "simpleSalad")
        { 
            if(simpleSalad>0){
                    Debug.Log("Late salad" + Data.Instance.LateSsalad);
                if(Data.Instance.LateSsalad>=1){
                    Debug.Log("capta q  hay tarde");
                    photonView.RPC("ChangePoints", PhotonTargets.All, Data.Instance.Point+10);
                    LateSsalad=Data.Instance.LateSsalad-1;
                    photonView.RPC("setDataValues", PhotonTargets.All, LateSsalad,"SimpleSalad");
                }
                else if (Data.Instance.LateSsalad<=0){
                    Debug.Log("capta q no hay tarde");
                    photonView.RPC("ChangePoints", PhotonTargets.All, Data.Instance.Point+20);
                }
                PhotonNetwork.Destroy(other.gameObject);
//                    DeleteLatest("SSaladOrder");
                    photonView.RPC("DeleteLatest", PhotonTargets.All, "SSaladOrder");
            }
        }
        else if(other.gameObject.tag == "fullSalad")
        {
            if(fullSalad>0){
                if(Data.Instance.LateFsalad>=1){
                    photonView.RPC("ChangePoints", PhotonTargets.All, Data.Instance.Point+10);
                    LateFsalad=Data.Instance.LateFsalad-1;
                    photonView.RPC("setDataValues", PhotonTargets.All, LateFsalad,"FullSalad");
                }
                else if (Data.Instance.LateFsalad<=0){
                    photonView.RPC("ChangePoints", PhotonTargets.All, Data.Instance.Point+20);
                }
                PhotonNetwork.Destroy(other.gameObject);
//                DeleteLatest("FSaladOrder");
                photonView.RPC("DeleteLatest", PhotonTargets.All, "FSaladOrder");
            }
        }
        else if(other.gameObject.tag == "onionSoup")
        {
            if(onionSoup>0){
                if(Data.Instance.LateOsoup>=1){
                    photonView.RPC("ChangePoints", PhotonTargets.All, Data.Instance.Point+10);
                    LateOsoup=Data.Instance.LateOsoup-1;
                    photonView.RPC("setDataValues", PhotonTargets.All, LateOsoup,"OSoup");
                }
                else if (Data.Instance.LateOsoup<=0){
                    photonView.RPC("ChangePoints", PhotonTargets.All, Data.Instance.Point+20);
                }
                PhotonNetwork.Destroy(other.gameObject);
  //              DeleteLatest("OSoupOrder");
                    photonView.RPC("DeleteLatest", PhotonTargets.All, "OSoupOrder");
            }
        }
        else if(other.gameObject.tag == "tomatoSoup")
        {
            if(tomatoSoup>0){
                if(Data.Instance.LateTsoup>=1){
                    photonView.RPC("ChangePoints", PhotonTargets.All, Data.Instance.Point+10);
                    LateTsoup=Data.Instance.LateTsoup-1;
                    photonView.RPC("setDataValues", PhotonTargets.All, LateTsoup,"TSoup");
                }
                else if (Data.Instance.LateTsoup<=0){
                    photonView.RPC("ChangePoints", PhotonTargets.All, Data.Instance.Point+20);
                }
                PhotonNetwork.Destroy(other.gameObject);
                photonView.RPC("DeleteLatest", PhotonTargets.All, "TSoupOrder");
            }
        }
    }
    
    [PunRPC]
    private void setDataValues(int newValue, string which){
        switch(which) {
            case "SimpleBurger" : Data.Instance.LateSburger=LateSburger;
            break;
            case "FullBurger" : Data.Instance.LateFburger=LateFburger;
            break;
            case "OSoup" : Data.Instance.LateOsoup=LateOsoup;
            break;
            case "TSoup" : Data.Instance.LateTsoup=LateTsoup;
            break;
            case "SimpleSalad" : Data.Instance.LateSsalad=LateSsalad;
            break;
            case "FullSalad" : Data.Instance.LateFsalad=LateFsalad;
            break;
        }
        text_points.text="puntos: " + Data.Instance.Point;
        Debug.Log("orders llama a setDataValus"+Data.Instance.Point);
    }


    [PunRPC]
    private void ChangePoints(int newPoints){
        Debug.Log("entra a setDataValues");
        Data.Instance.Point=newPoints;
    }
    [PunRPC]
    public void PickOrder(int order){
        if(order == 1)
        {
            simpleBurger = simpleBurger + 1;
        }
        else if(order == 2)
        {
            fullBurger = fullBurger + 1;
        }
        else if(order == 3)
        {
            tomatoSoup = tomatoSoup + 1;
        }
        else if(order == 4)
        {
            onionSoup = onionSoup + 1;
        }
        else if(order == 5)
        {
            simpleSalad = simpleSalad + 1;
        }
        else if(order == 6)
        {
            fullSalad = fullSalad + 1;
        }
        else if(order == 7)
        {
            fullBurgerFries = fullBurgerFries + 1;
        }
        else if(order == 8)
        {
            simpleBurgerFries = simpleBurgerFries + 1;
        }
        Debug.Log("simpleBurger"+simpleBurger+"fullBurger"+fullBurger+"tomatoSoup"+tomatoSoup+"onionSoup"+onionSoup+"simpleSalad" +simpleSalad + "fullSalad"+ fullSalad);
    
    }
    [PunRPC]    
    private void DeleteLatest(string tag){
        var listButton = GameObject.FindGameObjectsWithTag(tag);
//         int all = listButton.Length;
//  for(int i = 0; i<listButton.Length; i++){
//      Debug.Log(listButton[i].name);
//  }
//         Destroy(listButton[all-1].gameObject);
    Destroy(listButton[0].gameObject);
    }
    
    public void MakeOrder(int order)
    {
       photonView.RPC("PickOrder", PhotonTargets.All, order);
    }
*/
}

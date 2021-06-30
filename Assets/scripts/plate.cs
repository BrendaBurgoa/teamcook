using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plate : Photon.MonoBehaviour
{
    public GameObject button;
    public Transform uiContainer;
    public int lettuce;
    public int tomato;
    public int bread;
    public int patty;
    public int fries;
    public int osoup;
    public int tsoup;

    public GameObject newPot;
    public GameObject newPan;

    public GameObject emptyPlate;
    public GameObject trashPlate;
    public GameObject tomatoSoup;
    public GameObject onionSoup;
    public GameObject simpleSalad;
    public GameObject fullSalad;
    public GameObject simpleBurger;
    public GameObject fullBurger;
    public GameObject simpleBurgerFries;
    public GameObject fullBurgerFries;
    public GameObject pattyUI;
    public GameObject tomatoUI;
    public GameObject friesUI;
    public GameObject breadUI;
    public GameObject tsoupUI;
    public GameObject osoupUI;
    public GameObject lettuceUI;
    public AudioSource ding;
    public AudioSource dong;

    void Start(){
        button.SetActive(false);
        initializeVars();
    }
    public void OnCharacterNear(bool isNear)
    {
        button.SetActive(isNear);
    }
    public void OnSelect(PhotonView pv, character character)
    {       
        string tag = pv.tag;
      //  print(pv.name);
        if (tag == "potOnionSoup" || tag == "potTomatoSoup"){
            if (tag == "potTomatoSoup")
            {
                tsoup++;
                photonView.RPC("setIngredients", PhotonTargets.All, 7, tsoup);
                photonView.RPC("GiveToChara", PhotonTargets.MasterClient, character.GetComponent<PhotonView>().viewID, newPot.name);
                gameManager.Instance.DeleteItem(pv.viewID);
            }
            else if (tag == "potOnionSoup")
            {
                osoup++;
                photonView.RPC("setIngredients", PhotonTargets.All, 6, osoup);
                photonView.RPC("GiveToChara", PhotonTargets.MasterClient, character.GetComponent<PhotonView>().viewID, newPot.name);
                gameManager.Instance.DeleteItem(pv.viewID);
            }
            makeDish();
            photonView.RPC("initializeVars", PhotonTargets.All);
        }else{ 
            checkIngredients(pv, character);
        }
    }

    public void makeDish(){
        if (patty == 0 && bread == 0 && tomato == 0 && lettuce == 0 && fries == 0 && tsoup == 0 && osoup == 0)
            return;
      

       // Debug.Log("tomato: " + tomato + ", bread: " + bread + ", lettuce" + lettuce + ", patty: " + patty + ", fries" + fries + ", tsoup" + tsoup + ", osoup" + osoup);
        if (patty==1 && bread == 1 && tomato == 0 && lettuce == 0 && fries == 0 && tsoup ==0 && osoup ==0 ){
            photonView.RPC("AddPlate", PhotonTargets.MasterClient, simpleBurger.name);
            initializeVars();
        }
        else if(patty==1 && bread == 1 && tomato == 1 && lettuce == 1 && fries == 0 && tsoup ==0 && osoup ==0){
            photonView.RPC("AddPlate", PhotonTargets.MasterClient, fullBurger.name);
            initializeVars();
        } else if(patty==1 && bread == 1 && tomato == 0 && lettuce == 0 && fries == 1 && tsoup ==0 && osoup ==0){
            photonView.RPC("AddPlate", PhotonTargets.MasterClient, simpleBurgerFries.name);
            initializeVars();
        }
        else if(tsoup ==1 && osoup ==0 && patty==0 && bread == 0 && tomato == 0 && lettuce == 0 && fries == 0)
        {
            photonView.RPC("AddPlate", PhotonTargets.MasterClient, tomatoSoup.name);
            initializeVars();
        }
        else if(osoup ==1 && tsoup ==0 && patty==0 && bread == 0 && tomato == 0 && lettuce == 0 && fries == 0){
            photonView.RPC("AddPlate", PhotonTargets.MasterClient, onionSoup.name);
            initializeVars();
        }
        else if(patty==1 && bread == 1 && tomato == 1 && lettuce == 1 && fries == 1 && tsoup ==0 && osoup ==0){
            photonView.RPC("AddPlate", PhotonTargets.MasterClient, fullBurgerFries.name);
            initializeVars();
        }
        else if(patty==0 && bread == 0 && tomato == 0 && lettuce == 3 && fries == 0 && tsoup ==0 && osoup ==0){
            photonView.RPC("AddPlate",  PhotonTargets.MasterClient, simpleSalad.name);
            initializeVars();
        }
        else if(patty==0 && bread == 0 && tomato == 1 && lettuce == 2 && fries == 0 && tsoup ==0 && osoup ==0){
            photonView.RPC("AddPlate", PhotonTargets.MasterClient, fullSalad.name);
            initializeVars();
        }
        else{  //borra todo:
            Debug.Log("tomato: "+tomato+", bread: "+bread+", lettuce" + lettuce +", patty: "+patty+", fries"+fries + ", tsoup" + tsoup + ", osoup" + osoup);
            dong.Play();
            photonView.RPC("initializeVars", PhotonTargets.All);
        }        
    }
    [PunRPC]
    public void AddPlate(string  plateName)
    {
        Vector3 dest = new Vector3(2.95f, transform.position.y, 4.42f);
        PhotonNetwork.Instantiate(plateName, dest, Quaternion.identity, 0);
        ding.Play();
        photonView.RPC("initializeVars", PhotonTargets.All);
    }
    [PunRPC]
    public void setIngredients(int which, int count){
        //gameManager.Instance.DeleteItem(viewID);
            switch (which){
            case 1: tomato= count;
                break;
            case 2: lettuce= count;
                break;
            case 3: patty= count;
                break;
            case 4: bread= count;
                break;
            case 5: fries= count;
                break;
            case 6: osoup= count;
                break;
            case 7: tsoup= count;
                break;
            }
    }
    [PunRPC]
    void SetNewIngredient(string ingredientName, int which, int qty)
    {
        photonView.RPC("setIngredients", PhotonTargets.All, which, qty);
        var currentUI = PhotonNetwork.Instantiate(ingredientName, transform.position, Quaternion.Euler(7.0f, 0f, -5f), 0);
        Debug.Log(currentUI + "currentUI");
        photonView.RPC("SetNewParent", PhotonTargets.All, currentUI.GetComponent<PhotonView>().viewID);
    }
    private void checkIngredients(PhotonView pv, character character){
        string tag = pv.gameObject.tag;
        if (tag == "chopped_tomato")
        {
            tomato++;
            photonView.RPC("SetNewIngredient", PhotonTargets.MasterClient, tomatoUI.name, 1, tomato);
            gameManager.Instance.DeleteItem(pv.viewID);
        }
        else if (tag == "fries")
        {
            fries++;
            photonView.RPC("SetNewIngredient", PhotonTargets.MasterClient, friesUI.name, 5, fries);
            gameManager.Instance.DeleteItem(pv.viewID);
        }
        else if (tag == "chopped_lettuce")
        {
            lettuce++;
            photonView.RPC("SetNewIngredient", PhotonTargets.MasterClient, lettuceUI.name, 2, lettuce);
            gameManager.Instance.DeleteItem(pv.viewID);
        }
        else if (tag == "cooked_patty")
        {
            patty++;            
            photonView.RPC("SetNewIngredient", PhotonTargets.MasterClient, pattyUI.name, 3, patty);
            gameManager.Instance.DeleteItem(pv.viewID);
            photonView.RPC("GiveToChara", PhotonTargets.MasterClient, character.GetComponent<PhotonView>().viewID, newPan.name);
        }
        else if (tag == "chopped_bread")
        {
            bread++;
            photonView.RPC("SetNewIngredient", PhotonTargets.MasterClient, breadUI.name, 4, bread);
            gameManager.Instance.DeleteItem(pv.viewID);
        }
    }

    [PunRPC]
    private void GiveToChara(int charaid, string name){
        var createdPot = PhotonNetwork.Instantiate(name,  new Vector3((transform.position.x - 2f), transform.position.y, (transform.position.z - 2f)), Quaternion.identity, 0);
        PhotonView obj = PhotonView.Find(createdPot.GetComponent<PhotonView>().viewID);
        photonView.RPC("CharacterGet", PhotonTargets.All, charaid, obj.viewID);
        character character = PhotonView.Find(charaid).GetComponent<character>();
        //  obj.TransferOwnership(charaid);
        character.GetObject(obj);
    }
    [PunRPC]
    private void CharacterGet(int charaid, int viewID)
    {
        PhotonView obj = PhotonView.Find(viewID);
        character chara = PhotonView.Find(charaid).GetComponent<character>();
        chara.GetObject(obj);
    }
    [PunRPC]
    private void SetNewParent(int viewID){
        var order =  PhotonView.Find(viewID).gameObject; 
        order.transform.SetParent(uiContainer);
        order.transform.localScale = Vector3.one;
    }
    public void RemoveAllChildsIn(Transform container)
    {
        int num = container.transform.childCount;
        for (int i = 0; i < num; i++) UnityEngine.Object.DestroyImmediate(container.transform.GetChild(0).gameObject);
    }

    [PunRPC]
    private void initializeVars ()
    {
        tomato=0;
        lettuce=0;
        bread=0;
        patty=0;
        fries=0;
        tsoup =0;
        osoup=0;
        RemoveAllChildsIn(uiContainer);
    }
}

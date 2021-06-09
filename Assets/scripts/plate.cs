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
        print(pv.name);
        if (tag == "potOnionSoup" || tag == "potTomatoSoup"){
            if (tag == "potTomatoSoup")
            {
                tsoup++;
                photonView.RPC("setIngredients", PhotonTargets.All, 7, tsoup);
               // var createdPot = PhotonNetwork.Instantiate(newPot.name, new Vector3((transform.position.x-2f),transform.position.y, (transform.position.z-2f) ), Quaternion.identity,0);                
                photonView.RPC("GiveToChara", PhotonTargets.MasterClient, character.GetComponent<PhotonView>().viewID, newPot.name);
                gameManager.Instance.DeleteItem(pv.viewID);
            }
            else if (tag == "potOnionSoup")
            {
                osoup++;
                photonView.RPC("setIngredients", PhotonTargets.All, 6, osoup);
               // var createdPot = PhotonNetwork.Instantiate(newPot.name, new Vector3((transform.position.x-0.5f),transform.position.y, (transform.position.z-0.5f) ), Quaternion.identity,0);
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
        Vector3 dest = new Vector3(2.9f, transform.position.y, 4.4f);

        Debug.Log("tomato: " + tomato + ", bread: " + bread + ", lettuce" + lettuce + ", patty: " + patty + ", fries" + fries + ", tsoup" + tsoup + ", osoup" + osoup);
        if (patty==1 && bread == 1 && tomato == 0 && lettuce == 0 && fries == 0 && tsoup ==0 && osoup ==0 ){
            PhotonNetwork.Instantiate(simpleBurger.name, dest, Quaternion.identity,0);
            photonView.RPC("initializeVars", PhotonTargets.All);
              ding.Play();
        }
        else if(patty==1 && bread == 1 && tomato == 1 && lettuce == 1 && fries == 0 && tsoup ==0 && osoup ==0){
            PhotonNetwork.Instantiate(fullBurger.name, dest, Quaternion.identity,0);
            photonView.RPC("initializeVars", PhotonTargets.All);
              ding.Play();
        } else if(patty==1 && bread == 1 && tomato == 0 && lettuce == 0 && fries == 1 && tsoup ==0 && osoup ==0){
            PhotonNetwork.Instantiate(simpleBurgerFries.name, dest, Quaternion.identity,0);
            photonView.RPC("initializeVars", PhotonTargets.All);
              ding.Play();
        }
        else if(tsoup ==1 && osoup ==0 && patty==0 && bread == 0 && tomato == 0 && lettuce == 0 && fries == 0)
        {
            PhotonNetwork.Instantiate(tomatoSoup.name, dest, Quaternion.Euler(-90.0f, 0f, 0.0f),0);
              ding.Play();
        }
        else if(osoup ==1 && tsoup ==0 && patty==0 && bread == 0 && tomato == 0 && lettuce == 0 && fries == 0){
            PhotonNetwork.Instantiate(onionSoup.name, dest, Quaternion.Euler(-90.0f, 0f, 0.0f),0);
              ding.Play();
        }
        else if(patty==1 && bread == 1 && tomato == 1 && lettuce == 1 && fries == 1 && tsoup ==0 && osoup ==0){
            PhotonNetwork.Instantiate(fullBurgerFries.name, dest, Quaternion.identity,0);
              ding.Play();
            photonView.RPC("initializeVars", PhotonTargets.All);
        }
        else if(patty==0 && bread == 0 && tomato == 0 && lettuce == 3 && fries == 0 && tsoup ==0 && osoup ==0){
            PhotonNetwork.Instantiate(simpleSalad.name, dest, Quaternion.identity,0);
              ding.Play();
            photonView.RPC("initializeVars", PhotonTargets.All);
        }
        else if(patty==0 && bread == 0 && tomato == 1 && lettuce == 2 && fries == 0 && tsoup ==0 && osoup ==0){
            PhotonNetwork.Instantiate(fullSalad.name, dest, Quaternion.identity,0);
              ding.Play();
            photonView.RPC("initializeVars", PhotonTargets.All);
        }
        else{  //borra todo:
            Debug.Log("tomato: "+tomato+", bread: "+bread+", lettuce" + lettuce +", patty: "+patty+", fries"+fries + ", tsoup" + tsoup + ", osoup" + osoup);
            dong.Play();
            photonView.RPC("initializeVars", PhotonTargets.All);
        }        
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

    private void checkIngredients(PhotonView pv, character character){
        string tag = pv.gameObject.tag;
        if (tag == "chopped_tomato")
        {
            tomato = tomato +1;
            photonView.RPC("setIngredients", PhotonTargets.All, 1, tomato);
            var currentUI = PhotonNetwork.Instantiate(tomatoUI.name, transform.position, Quaternion.Euler(7.0f, 0f, -5f),0);
            Debug.Log(currentUI + "currentUI");
            photonView.RPC("SetNewParent", PhotonTargets.All, currentUI.GetComponent<PhotonView>().viewID );
            gameManager.Instance.DeleteItem(pv.viewID);
        }
        else if (tag == "fries")
        {
            fries = fries + 1;
            photonView.RPC("setIngredients", PhotonTargets.All, 5, fries);
            var currentUI = PhotonNetwork.Instantiate(friesUI.name, transform.position, Quaternion.Euler(7.0f, 0f, -5f),0);
            photonView.RPC("SetNewParent", PhotonTargets.All, currentUI.GetComponent<PhotonView>().viewID );
            gameManager.Instance.DeleteItem(pv.viewID);
        }
        else if (tag == "chopped_lettuce")
        {
            lettuce = lettuce + 1;
            photonView.RPC("setIngredients", PhotonTargets.All, 2, lettuce);
            var currentUI = PhotonNetwork.Instantiate(lettuceUI.name, transform.position, Quaternion.Euler(7.0f, 0f, -5f),0);
            photonView.RPC("SetNewParent", PhotonTargets.All, currentUI.GetComponent<PhotonView>().viewID );
            gameManager.Instance.DeleteItem(pv.viewID);
        }
        else if (tag == "cooked_patty")
        {
            patty++;
            photonView.RPC("setIngredients", PhotonTargets.All, 3, patty);
            var currentUI = PhotonNetwork.Instantiate(pattyUI.name, transform.position, Quaternion.Euler(7.0f, 0f, -5f),0);
            photonView.RPC("SetNewParent", PhotonTargets.All, currentUI.GetComponent<PhotonView>().viewID );
           photonView.RPC("GiveToChara", PhotonTargets.MasterClient, character.GetComponent<PhotonView>().viewID, "pan");
            gameManager.Instance.DeleteItem(pv.viewID);
        }
        else if (tag == "chopped_bread")
        {
            bread++;
            photonView.RPC("setIngredients", PhotonTargets.All, 4, bread);
            var currentUI = PhotonNetwork.Instantiate(breadUI.name, transform.position, Quaternion.Euler(7.0f, 0f, -5f),0);
            photonView.RPC("SetNewParent", PhotonTargets.All, currentUI.GetComponent<PhotonView>().viewID );
            gameManager.Instance.DeleteItem(pv.viewID);
        }
    }

    [PunRPC]
    private void GiveToChara(int charaid, string name){
        var createdPot = PhotonNetwork.Instantiate(name,  new Vector3((transform.position.x - 2f), transform.position.y, (transform.position.z - 2f)), Quaternion.identity, 0);
      
        PhotonView obj = PhotonView.Find(createdPot.GetComponent<PhotonView>().viewID);

        photonView.RPC("CharacterGet", PhotonTargets.All, charaid, obj.viewID);

        character chara = PhotonView.Find(charaid).GetComponent<character>();
        obj.TransferOwnership(charaid);
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

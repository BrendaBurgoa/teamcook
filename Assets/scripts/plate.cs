using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plate : Photon.MonoBehaviour
{
    public int lettuce;
    public int tomato;
    public int bread;
    public int patty;
    public int fries;
    public int osoup;
    public int tsoup;
    private bool collided; 
    public GameObject newPot;
    public GameObject newPan;
    public GameObject character;
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
    public PhotonView photonView;
    public GameObject pattyUI;
    public GameObject tomatoUI;
    public GameObject friesUI;
    public GameObject breadUI;
    public GameObject tsoupUI;
    public GameObject osoupUI;
    public GameObject lettuceUI;
    private GameObject ingredientOnCollision;
    public AudioSource ding;
    public AudioSource dong;
    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        
    }
    void Start(){
        tomato=0;
        lettuce=0;
        bread=0;
        patty=0;
        fries=0;
        tsoup=0;
        osoup=0;
    }
    
    void OnMouseDown()
    {
        //si un personaje esta cerca y clickea el plato se crea lo correspondiente y se inicializan variables
        if (collided==true && character.transform.GetChild(0).transform.childCount <=0){
            makeDish();
              photonView.RPC("initializeVars", PhotonTargets.All);
              photonView.RPC("deleteChildren", PhotonTargets.All);
        }
    }
    void Update (){
        //si un personaje esta cerca y hace espacio se suma la imagen correspondiente, si tiene una olla se crea la olla y se devuelve
        if(Input.GetKeyDown("space") && collided == true && character.GetComponent<PhotonView>().isMine){
          if(ingredientOnCollision.tag == "potOnionSoup" || ingredientOnCollision.tag == "potTomatoSoup"){
              if (ingredientOnCollision.tag == "potTomatoSoup")
                {
                    tsoup=tsoup+1;
                    photonView.RPC("setIngredients", PhotonTargets.All, 7, ingredientOnCollision.name, tsoup);
                    var createdPot = PhotonNetwork.Instantiate(newPot.name, new Vector3((transform.position.x-2f),transform.position.y, (transform.position.z-2f) ), Quaternion.identity,0);                
                    photonView.RPC("GiveToChara", PhotonTargets.All, character.GetComponent<PhotonView>().viewID, createdPot.GetComponent<PhotonView>().viewID);                
                }
                else if (ingredientOnCollision.tag == "potOnionSoup")
                {
                    osoup=osoup+1;
                     photonView.RPC("setIngredients", PhotonTargets.All, 6, ingredientOnCollision.name, osoup);
                    var createdPot = PhotonNetwork.Instantiate(newPot.name, new Vector3((transform.position.x-0.5f),transform.position.y, (transform.position.z-0.5f) ), Quaternion.identity,0);
                    photonView.RPC("GiveToChara", PhotonTargets.All, character.GetComponent<PhotonView>().viewID, createdPot.GetComponent<PhotonView>().viewID);
                }
              makeDish();
              photonView.RPC("initializeVars", PhotonTargets.All);
              photonView.RPC("deleteChildren", PhotonTargets.All);
          }else{
          checkIngredients(ingredientOnCollision.name);}
        }
    }

    public void makeDish(){
        Debug.Log("tomato: "+tomato+", bread: "+bread+", lettuce" + lettuce +", patty: "+patty+", fries"+fries + ", tsoup" + tsoup + ", osoup" + osoup);
//se chequean las recetas y se instancia el plato logrado o basura
        if(patty==1 && bread == 1 && tomato == 0 && lettuce == 0 && fries == 0 && tsoup ==0 && osoup ==0 ){
            PhotonNetwork.Instantiate(simpleBurger.name, new Vector3(3.6f,transform.position.y, 4.6f ), Quaternion.identity,0);
            photonView.RPC("initializeVars", PhotonTargets.All);
              ding.Play();
        }
        else if(patty==1 && bread == 1 && tomato == 1 && lettuce == 1 && fries == 0 && tsoup ==0 && osoup ==0){
            PhotonNetwork.Instantiate(fullBurger.name, new Vector3(3.6f,transform.position.y, 4.6f ), Quaternion.identity,0);
            photonView.RPC("initializeVars", PhotonTargets.All);
              ding.Play();
        } else if(patty==1 && bread == 1 && tomato == 0 && lettuce == 0 && fries == 1 && tsoup ==0 && osoup ==0){
            PhotonNetwork.Instantiate(simpleBurgerFries.name, new Vector3(3.6f,transform.position.y, 4.6f ), Quaternion.identity,0);
            photonView.RPC("initializeVars", PhotonTargets.All);
              ding.Play();
        }
        else if(tsoup ==1 && osoup ==0 && patty==0 && bread == 0 && tomato == 0 && lettuce == 0 && fries == 0)
        {
            PhotonNetwork.Instantiate(tomatoSoup.name, new Vector3(3.6f,transform.position.y, 4.6f ), Quaternion.Euler(-90.0f, 0f, 0.0f),0);
              ding.Play();
        }
        else if(osoup ==1 && tsoup ==0 && patty==0 && bread == 0 && tomato == 0 && lettuce == 0 && fries == 0){
            PhotonNetwork.Instantiate(onionSoup.name, new Vector3(3.6f,transform.position.y, 4.6f ), Quaternion.Euler(-90.0f, 0f, 0.0f),0);
              ding.Play();
        }
        else if(patty==1 && bread == 1 && tomato == 1 && lettuce == 1 && fries == 1 && tsoup ==0 && osoup ==0){
            PhotonNetwork.Instantiate(fullBurgerFries.name,new Vector3(3.6f,transform.position.y, 4.6f ), Quaternion.identity,0);
              ding.Play();
            photonView.RPC("initializeVars", PhotonTargets.All);
        }
        else if(patty==0 && bread == 0 && tomato == 0 && lettuce == 3 && fries == 0 && tsoup ==0 && osoup ==0){
            PhotonNetwork.Instantiate(simpleSalad.name, new Vector3(3.6f,transform.position.y, 4.6f ), Quaternion.identity,0);
              ding.Play();
            photonView.RPC("initializeVars", PhotonTargets.All);
        }
        else if(patty==0 && bread == 0 && tomato == 1 && lettuce == 2 && fries == 0 && tsoup ==0 && osoup ==0){
            PhotonNetwork.Instantiate(fullSalad.name, new Vector3(3.6f,transform.position.y, 4.6f ), Quaternion.identity,0);
              ding.Play();
            photonView.RPC("initializeVars", PhotonTargets.All);
        }
        else{
        Debug.Log("tomato: "+tomato+", bread: "+bread+", lettuce" + lettuce +", patty: "+patty+", fries"+fries + ", tsoup" + tsoup + ", osoup" + osoup);
              dong.Play();
            photonView.RPC("initializeVars", PhotonTargets.All);
        }
        
    }

    [PunRPC]
    public void setIngredients(int which, string name, int count){
        //se comparten a todos los ingredientes y cantidades modificados
            Destroy(GameObject.Find(name));
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

    private void checkIngredients(string name){
        //cuando colisiona un ingrediente se reconoce cual es, se suma su contador y se muestra el UI
            var currentIngredient= GameObject.Find(name);
                if (currentIngredient.tag == "chopped_tomato")
                {
                    tomato = tomato +1;
                    photonView.RPC("setIngredients", PhotonTargets.All, 1, currentIngredient.name, tomato);
                    var currentUI = PhotonNetwork.Instantiate(tomatoUI.name, transform.position, Quaternion.Euler(7.0f, 0f, -5f),0);
                    Debug.Log(currentUI + "currentUI");
                    photonView.RPC("SetNewParent", PhotonTargets.All, currentUI.GetComponent<PhotonView>().viewID );
                }
                else if (currentIngredient.tag == "fries")
                {
                    fries = fries + 1;
                    photonView.RPC("setIngredients", PhotonTargets.All, 5,  currentIngredient.name, fries);
                    var currentUI = PhotonNetwork.Instantiate(friesUI.name, transform.position, Quaternion.Euler(7.0f, 0f, -5f),0);
                    photonView.RPC("SetNewParent", PhotonTargets.All, currentUI.GetComponent<PhotonView>().viewID );
                }
                else if (currentIngredient.tag == "chopped_lettuce")
                {
                    lettuce = lettuce + 1;
                    photonView.RPC("setIngredients", PhotonTargets.All, 2,  currentIngredient.name, lettuce);
                    var currentUI = PhotonNetwork.Instantiate(lettuceUI.name, transform.position, Quaternion.Euler(7.0f, 0f, -5f),0);
                    photonView.RPC("SetNewParent", PhotonTargets.All, currentUI.GetComponent<PhotonView>().viewID );
                }
                else if (currentIngredient.tag == "cooked_patty")
                {
                    patty = patty +1;
                    photonView.RPC("setIngredients", PhotonTargets.All, 3, currentIngredient.name, patty);
                    var currentUI = PhotonNetwork.Instantiate(pattyUI.name, transform.position, Quaternion.Euler(7.0f, 0f, -5f),0);
                    photonView.RPC("SetNewParent", PhotonTargets.All, currentUI.GetComponent<PhotonView>().viewID );
                    var createdPot=PhotonNetwork.Instantiate(newPan.name, new Vector3((transform.position.x-2f),transform.position.y, (transform.position.z-2f) ), Quaternion.identity,0);
                    photonView.RPC("GiveToChara", PhotonTargets.All, character.GetComponent<PhotonView>().viewID, createdPot.GetComponent<PhotonView>().viewID);                
                }
                else if (currentIngredient.tag == "chopped_bread")
                {
                    bread = bread +1;
                    photonView.RPC("setIngredients", PhotonTargets.All, 4, currentIngredient.name, bread);
                    var currentUI = PhotonNetwork.Instantiate(breadUI.name, transform.position, Quaternion.Euler(7.0f, 0f, -5f),0);
                    photonView.RPC("SetNewParent", PhotonTargets.All, currentUI.GetComponent<PhotonView>().viewID );
                }

    }

    [PunRPC]
    private void GiveToChara(int charaid, int id){
        //se dan las ollas/sartenes vacias al personaje
        var obj = PhotonView.Find(id);
        obj.name = obj.name + id; 
        var chara = PhotonView.Find(charaid);
        var dest = chara.transform.GetChild(0);
        obj.transform.SetParent(dest.transform, true);
        obj.transform.localPosition= new Vector3(0f,0f,0f);
    }

    void OnCollisionEnter(Collision other)
        {
            //reconoce la colision con el personaje y que ingrediente tiene
            if(other.gameObject.tag == "character"){
                collided=true;
                character = other.gameObject;
            } else{
                   ingredientOnCollision=other.gameObject;
            }
        }

    void OnCollisionExit(Collision other)
    {
        //se resetean las colisiones 
        collided = false;
        ingredientOnCollision=null;
        character=null;
    }
    
    [PunRPC]
    private void SetNewParent(int id){
        //a la imagen UI instanciada se la coloca en el canvas adecuado
        var canvas = gameObject.transform.GetChild(0);
        var panel = canvas.transform.GetChild(0);
        var content = panel.transform.GetChild(0);
        var order =  PhotonView.Find(id).gameObject; 
        order.transform.SetParent(content);
        order.transform.localScale = new Vector3(1f, 1f, 1f);
    }
    [PunRPC]
    private void deleteChildren(){
        //cuando se resetea el plato se borran los elementos UI
        var canvas = gameObject.transform.GetChild(0);
        var panel = canvas.transform.GetChild(0);
        var content = panel.transform.GetChild(0);
        content.DetachChildren();
    }

    [PunRPC]
    private void initializeVars ()
    {
        tomato=0;
        lettuce=0;
        bread=0;
        patty=0;
        fries=0;
        collided = false;
        tsoup =0;
        osoup=0;
    }
}

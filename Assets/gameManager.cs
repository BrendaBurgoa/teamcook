using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : Photon.MonoBehaviour
{
    static gameManager mInstance = null;
    public GameObject manager;
    PhotonView photonView;
    public GameObject ConnectingCanvas;
    private float valX;
    private float valZ;
    private string side;

    public static gameManager Instance
    {
        get
        {
            return mInstance;
        }
    }
    void Awake()
    {
        if (!mInstance)
            mInstance = this;
    }
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (Data.Instance.Rol == 0)
        {
            manager.SetActive(true);
        }else if (Data.Instance.Rol == 1)
        {
            manager.SetActive(false);
            ConnectingCanvas.SetActive(true);
        }
    }

    public GameObject PlayerPrefab;

    public void SpawnPlayer(){

        GameObject[] all = GameObject.FindGameObjectsWithTag("character");
        if (all.Length == 0)
        {
            if (PhotonNetwork.player.ID %2==0){
                valX = Random.Range(-2f, -1f);
                side = "left";
            }        
            else if (PhotonNetwork.player.ID %2==1){
                valX = Random.Range(1f, 3f);
                side = "right";
            }
        }
        else
        {
            int total_in_left = 0;
            int total_in_right = 0;
            foreach (GameObject go in all)
            {
                if (go.transform.localPosition.x > 0)
                    total_in_right++;
                else total_in_left++;
            }
            bool inLeft = true;
            if (total_in_left > total_in_right)
                inLeft = false;

            if (inLeft)
            {
                valX = Random.Range(-2f, -1f);
                side = "left";
            }
            else
            {
                valX = Random.Range(1f, 3f);
                side = "right";
            }
        }
        print("Agrega user id: " + PhotonNetwork.player.ID + "  (ya hay " + all.Length + ")");
        valZ = Random.Range(1f, 3f);
        var newPlayer = PhotonNetwork.Instantiate(PlayerPrefab.name, new Vector3(valX, 0, valZ), Quaternion.identity, 0);
        photonView.RPC("changeName", PhotonTargets.All, newPlayer.GetComponent<PhotonView>().viewID, side);
    }

    [PunRPC]
    public void changeName(int id, string rightOrLeft)
    {    
        var ownAvatar = PhotonView.Find(id);
        ownAvatar.GetComponent<character>().playerId=PhotonNetwork.player.ID;
        ownAvatar.name = ownAvatar.name + PhotonView.Find(id); 
        ownAvatar.GetComponent<character>().currentSide=rightOrLeft;
    }
    public void DeleteItem(int viewID)
    {
        GameObject go = PhotonView.Find(viewID).gameObject;
        if (go == null) return;

        foreach (MeshRenderer sr in go.GetComponents<MeshRenderer>())  sr.enabled = false; //lo apaga para que se oculte rápido (estético)
        photonView.RPC("DeleteItemByMaster", PhotonTargets.MasterClient, viewID);
    }
    [PunRPC]
    void DeleteItemByMaster(int viewID)
    {
        PhotonView pv = PhotonView.Find(viewID);
        if(pv != null)
            PhotonNetwork.Destroy(pv.gameObject);
    }

    public void PickObject(int id, int characterID)
    {
        photonView.RPC("PickObjectMasterFilter", PhotonTargets.MasterClient, id, characterID);
    }
    [PunRPC]
    public void PickObjectMasterFilter(int id, int characterID)
    {
        Debug.Log("Pick up item id: " + id + " characterID: " + characterID);
        PhotonView ingredient = PhotonView.Find(id);
        character characterThaHasItem = ingredient.GetComponentInParent<character>();
        if (characterThaHasItem == null)
        {
            // se fija que el objeto no este ya agarrado:
            Picked(id, characterID);
            photonView.RPC("PickObjectForOthers", PhotonTargets.Others, id, characterID);
        }
    }
    [PunRPC]
    public void PickObjectForOthers(int id, int characterID)
    {
        Picked(id, characterID);
    }
    public void Picked(int id, int characterID)
    {
        PhotonView ingredient = PhotonView.Find(id);
        if (ingredient == null) return;
        ingredient.GetComponent<ShowCollision>().SetCollision(false);

        var _character = PhotonView.Find(characterID);
        character characterThatCatch = _character.GetComponent<character>();
        characterThatCatch.GetObject(ingredient.GetComponent<PhotonView>());
    }

}

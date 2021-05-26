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
           
        if (PhotonNetwork.player.ID %2==0){
            valX = Random.Range(-2f, -1f);
            side = "left";
        }        
        else if (PhotonNetwork.player.ID %2==1){
            valX = Random.Range(1f, 3f);
            side = "right";
        }
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : Photon.MonoBehaviour
{
    public GameObject manager;
    public PhotonView photonView;
    public GameObject ConnectingCanvas;
    private float valX;
    private float valZ;
    private string side;
    // Start is called before the first frame update
    void Start()
    {             
        if (Data.Instance.Rol == 0)
        {
          //  SpawnPlayer();
            manager.SetActive(true);
        }else if (Data.Instance.Rol == 1)
        { 
            ConnectingCanvas.SetActive(true);
          //  SpawnPlayer();
        }
    }


    public GameObject PlayerPrefab;
    public void SpawnPlayer(){
        ConnectingCanvas.SetActive(false);        
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
}

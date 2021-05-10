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

    void Start()
    {             
        if (Data.Instance.Rol == 0)
        {
            manager.SetActive(true);
        }else if (Data.Instance.Rol == 1)
        { 
            ConnectingCanvas.SetActive(true);
        }
    }


    public GameObject PlayerPrefab;

    //esta funcion esta en este script para que pueda ser ejecutada por todos los jugadores, pero es llamda por el manager en MatchTime
    public void SpawnPlayer(){
        //se deshabilita la pantalla de carga y dependiendo de si el Id del jugador es par o impar se le asigna un lado de la cocina, luego se spawnea
           
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
        //para luego poder buscar a cada jugador, se le asigna un nombre unico segun su ID       
        var ownAvatar = PhotonView.Find(id);
        ownAvatar.GetComponent<character>().playerId=PhotonNetwork.player.ID;
        ownAvatar.name = ownAvatar.name + PhotonView.Find(id); 
        ownAvatar.GetComponent<character>().currentSide=rightOrLeft;
    }
}

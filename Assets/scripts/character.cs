using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class character : Photon.MonoBehaviour
{
    //los scripts de movimiento son propios del prefab del personaje y vinieron del store, Simple Sample Charact (editados para que solo si la vista del personaje es mia se pueda mover)
    public PhotonView photonView;
    public GameObject avatar;
    public Text PlayerName;
    public GameObject playerCanvas;
    public int playerId;
    public string currentSide;

    private void Awake()
    {
        //se muestra el nombre del personaje y si es propio es de otro color
        if (photonView.isMine){
            PlayerName.text = PhotonNetwork.playerName;
        }
        else{
            PlayerName.text = photonView.owner.name;
            PlayerName.color = Color.blue;
        }
    }

    void Update()
    {   
        //en el caso de error que el empty que contiene lo agarrado tenga mas de un elemento, el segundo elemento se elimina
        if(transform.GetChild(0).childCount >= 2){
            Destroy(transform.GetChild(0).transform.GetChild(0).gameObject);
        }
        if (photonView.isMine){
            //si se excede el limite según el lado en el que se encuentre, se lo retrocede
            Vector3 pos = transform.position;

            if(pos.x < -3.5f)
            pos.x = -3.3f;
            else if (pos.x > 3.7f)
            pos.x = 3.6f;

            if(pos.z < 0.1f)
            pos.z = 0.2f;
            else if (pos.z > 4.1f)
            pos.z = 4;

            if(currentSide == "left"){
                if(pos.x > -0.3f){
                    pos.x = -0.5f;
                }
            } else if(currentSide == "right"){
                if(pos.x < 0.5f){
                    pos.x = 0.6f;
                }
            }

            transform.position = pos;
            playerCanvas.transform.rotation = Quaternion.identity;
        }

        
        
    }


}

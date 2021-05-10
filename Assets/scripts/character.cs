using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class character : Photon.MonoBehaviour
{
    //los scripts de movimiento son propios del prefab del personaje y vinieron del store, Simple Sample Charact (editados para que solo si la vista del personaje es mia se pueda mover)
    public PhotonView photonView;
    public GameObject avatar;
    public string playerName;
   // public GameObject playerCanvas;
    public int playerId;
    public string currentSide;
    Vector2 limits_x = new Vector2(3.45f,0.6f);
    Vector2 limits_z = new Vector2(3.93f, 0.35f);

    private void Start()
    {
        Events.OnNewPlayer(this);
    }
    void Update()
    {   
        //en el caso de error que el empty que contiene lo agarrado tenga mas de un elemento, el segundo elemento se elimina
        if(transform.GetChild(0).childCount >= 2){
            Destroy(transform.GetChild(0).transform.GetChild(0).gameObject);
        }
        if (photonView.isMine){
            Vector3 pos = transform.position;

            if(currentSide == "left"){
                if (pos.x < -limits_x.x) pos.x = -limits_x.x;
                if (pos.x > -limits_x.y) pos.x = -limits_x.y;
            } else if(currentSide == "right"){
                if (pos.x > limits_x.x) pos.x = limits_x.x;
                if (pos.x < limits_x.y) pos.x = limits_x.y;
            }

            if (pos.z > limits_z.x) pos.z = limits_z.x - (0.01f);
            if (pos.z < limits_z.y) pos.z = limits_z.y + (0.01f);

            pos.y = 0;
            transform.position = pos;
        }
    }


}

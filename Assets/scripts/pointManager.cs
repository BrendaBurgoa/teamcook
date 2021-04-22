using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pointManager : Photon.MonoBehaviour
{
    public Text Points;
    public PhotonView photonView;
    void Update()
    {
        photonView.RPC("markPoints", PhotonTargets.All);
        //actualiza el texto UI de las ordenes en tiempo y vencidas
    }
    [PunRPC]
    public void markPoints ()
    {
        Points.text= "entregadas: "+Data.Instance.TimelyOrders +", vencidas: " + Data.Instance.LateOrders; 
    }
}

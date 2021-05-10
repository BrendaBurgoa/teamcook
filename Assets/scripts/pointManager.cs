using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pointManager : Photon.MonoBehaviour
{
    public Text Points;
    public PhotonView photonView;
    private void Start()
    {
        Events.OnRefreshPoints += OnRefreshPoints;
    }
    private void OnDestroy()
    {
        Events.OnRefreshPoints -= OnRefreshPoints;
    }
    void OnRefreshPoints()
    {
        if(Data.Instance.Rol == 0)
            photonView.RPC("markPoints", PhotonTargets.All);
    }
    [PunRPC]
    public void markPoints ()
    {
        Points.text= "entregadas: "+Data.Instance.TimelyOrders +", vencidas: " + Data.Instance.LateOrders; 
    }
}

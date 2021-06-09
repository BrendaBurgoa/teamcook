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
        markPoints(0, 0);
    }
    private void OnDestroy()
    {
        Events.OnRefreshPoints -= OnRefreshPoints;
    }
    void OnRefreshPoints()
    {
        if(Data.Instance.Rol == 0)
            photonView.RPC("markPoints", PhotonTargets.All, Data.Instance.TimelyOrders, Data.Instance.LateOrders);
    }
    [PunRPC]
    public void markPoints (int TimelyOrders, int LateOrders)
    {
        Points.text= "entregadas: "+TimelyOrders +", vencidas: " + LateOrders; 
    }
}

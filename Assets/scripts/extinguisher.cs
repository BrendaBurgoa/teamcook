using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class extinguisher : MonoBehaviour
{
    //clase en los fuegos no en el matafuegos
    public PhotonView photonView;
    private bool collided;
    private GameObject character;

    private void Start()
    {
        Data.Instance.fireExists = true;
        photonView = GetComponent<PhotonView>();
    }
    public void OnSelect()
    {
        photonView.RPC("extinguishFire", PhotonTargets.MasterClient, photonView.viewID);
    }   
    [PunRPC]
    public void extinguishFire(int viewID)
    {
        photonView.RPC("FireOff", PhotonTargets.All);
        GameObject go = PhotonView.Find(viewID).gameObject;
        PhotonNetwork.Destroy(go);
    }
    [PunRPC]
    public void FireOff()
    {
        Data.Instance.fireExists = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fires : Photon.MonoBehaviour
{
    public GameObject myPrefab;
    public PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }
    public void MakeFires()
    {                
       photonView.RPC("instantiateFires", PhotonTargets.MasterClient);
    }
    
    [PunRPC]
    private void instantiateFires()
    {
        float _x = Random.Range(0.8f, 3.0f);
        if (Random.Range(0, 10) < 5)
            _x *= -1;
        Vector3 position = new Vector3(_x, 0, Random.Range(0.9f, 3.5f));
        PhotonNetwork.Instantiate(myPrefab.name, position, Quaternion.identity, 0);
    }
}

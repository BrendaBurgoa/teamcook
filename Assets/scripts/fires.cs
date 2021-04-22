using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fires : Photon.MonoBehaviour
{
    public GameObject myPrefab;
    public PhotonView photonView;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void MakeFires()
    {                

       photonView.RPC("instantiateFires", PhotonTargets.All);
       // this.GetComponent<PhotonView>().RPC("instantiateFires", RpcTarget.All);
    }
    
    [PunRPC]
    private void instantiateFires()
    {
        Vector3 position = new Vector3(Random.Range(-3f, 3.0f), 0, Random.Range(0f, 4.0f));
        PhotonNetwork.Instantiate(myPrefab.name, position, Quaternion.identity, 0); 
    }
}

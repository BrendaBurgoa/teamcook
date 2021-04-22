using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instantiateAnim : MonoBehaviour
{
    
    public GameObject myPrefab;
    public PhotonView photonView;
    
    public void ChopChange()
    {
            var ingredient = PhotonNetwork.Instantiate(myPrefab.name, new Vector3(-0.89f , 0.5f, 4.3f), Quaternion.identity,0); 
            photonView.RPC("DeleteChange", PhotonTargets.All, ingredient.GetComponent<PhotonView>().viewID);
        
    }

    [PunRPC]
    private void DeleteChange(int id)
    {
        var ingredient = PhotonView.Find(id);
        ingredient.name = ingredient.name + id; 
        
    }
}

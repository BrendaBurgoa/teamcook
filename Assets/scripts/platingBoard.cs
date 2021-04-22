using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platingBoard : Photon.MonoBehaviour
{
    public PhotonView photonView;
    public GameObject plate;
    // void OnCollisionExit(Collision other)
    //     {
    //         if (other.gameObject.tag == "plate" || other.gameObject.tag == "trashplate")
    //         {
    //             PhotonNetwork.Instantiate(plate.name, new Vector3(transform.position.x , transform.position.y+0.3f, transform.position.z), Quaternion.identity,0);
    //         }
    //     }
        
}

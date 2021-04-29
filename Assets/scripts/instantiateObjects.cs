using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instantiateObjects : Photon.MonoBehaviour
{
    public GameObject myPrefab;
    public PhotonView photonView;
    private bool buttonFlag = false;
    public float val1;
    public float val2;
    private GameObject chara;

    void Update()
    {
        if (Input.GetKeyDown("space") && buttonFlag == true && chara.transform.GetChild(0).transform.childCount == 0 && chara.GetComponent<PhotonView>().isMine)
            {
                // Debug.Log(chara.GetComponent<PhotonView>().owner);
                if(transform.parent.position.x < 0){
                    val1=2f;
                    val2=3f;
                }else{
                    val1= -2f;
                    val2 = -3f;
                }
                float randomValue = Random.Range(val1, val2);
                var ingredient = PhotonNetwork.Instantiate(myPrefab.name, new Vector3(transform.position.x+randomValue , 0.5f, transform.position.z), Quaternion.identity, 0); 
                photonView.RPC("instantiateIngredient", PhotonTargets.All, ingredient.GetComponent<PhotonView>().viewID);
            }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "character" && collision.gameObject.transform.GetChild(0).transform.childCount == 0){
            photonView.RPC("WhoGets", PhotonTargets.All, collision.gameObject.name);
            buttonFlag=true;    
        }
    }

    void OnCollisionExit(Collision collision){
        if(collision.gameObject.tag == "character"){
            buttonFlag=false;
            chara=null;
        }
    }
    [PunRPC]
    private void WhoGets(string name){
        chara = GameObject.Find(name);
    }
    [PunRPC]
    public void instantiateIngredient(int id)
    {
       var ingredient = PhotonView.Find(id);
       ingredient.name = ingredient.name + PhotonView.Find(id); 
      if(chara != null){
        if(chara.transform.GetChild(0).transform.childCount <= 0){
            var dest = chara.transform.GetChild(0);
            ingredient.transform.SetParent(dest.transform, true);
            ingredient.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.player);
            ingredient.GetComponent<pick_drop>().character = chara;
            ingredient.GetComponent<pick_drop>().isGrabbed=true;
            ingredient.transform.localPosition=new Vector3(0, 0, 0);
        }else{
            Debug.Log("se eliminaría");
            Destroy(ingredient);
        }
       }
       
    }
    
}
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trash : Photon.MonoBehaviour
{

    public void OnSelect(PhotonView pv, character character)
    {
        string tag = pv.gameObject.tag;
        print("trash " + tag);
        if (tag != "pan" && tag != "pot" && tag != "extinguisher" && tag != "counter" && tag != "stove")
        {
            gameManager.Instance.DeleteItem(pv.viewID);
        }
        
        if (    tag == "burnt_pot"
            ||  tag == "potOnionSoup"
            ||  tag == "potTomatoSoup")
        {
            photonView.RPC("GiveToChara", PhotonTargets.MasterClient, character.GetComponent<PhotonView>().viewID, "pot");

          //  var newpot = PhotonNetwork.Instantiate(pot.name, new Vector3(1000,0,0), Quaternion.identity, 0);
          //  photonView.RPC("GiveToChara", PhotonTargets.All, character.photonView.viewID, newpot.GetComponent<PhotonView>().viewID);
        }
        else if (
                tag == "burnt_pan"
        //  ||    tag == "pan"
          ||    tag == "cooked_patty"
        )
        {
            photonView.RPC("GiveToChara", PhotonTargets.MasterClient, character.GetComponent<PhotonView>().viewID, "pan");

            //var newpot = PhotonNetwork.Instantiate(pan.name, new Vector3(1000, 0, 0), Quaternion.identity, 0);
            //photonView.RPC("GiveToChara", PhotonTargets.All, character.photonView.viewID, newpot.GetComponent<PhotonView>().viewID);
        }
    }

    [PunRPC]
    private void GiveToChara(int charaid, string name)
    {
        Debug.Log("TRASH ____Give " + name);
        var newObj = PhotonNetwork.Instantiate(name, new Vector3(1000,0,0), Quaternion.identity, 0);
        PhotonView obj = PhotonView.Find(newObj.GetComponent<PhotonView>().viewID);
        Debug.Log("TRASH NEW obj.viewID; " + obj.viewID);
        photonView.RPC("CharacterGet", PhotonTargets.All, charaid, obj.viewID);

       // character character = PhotonView.Find(charaid).GetComponent<character>();
        //obj.TransferOwnership(charaid);
       // if (character == null) return;
       // character.GetObject(photonView);
    }
    [PunRPC]
    private void CharacterGet(int charaid, int viewID)
    {
        PhotonView obj = PhotonView.Find(viewID);
        character character = PhotonView.Find(charaid).GetComponent<character>();
        if (character == null) return;
        character.GetObject(obj);
    }

    //[PunRPC]
    //private void GiveToChara(int charaid, int viewID)
    //{
    //    PhotonView obj = PhotonView.Find(viewID);
    //    obj.name = obj.name + viewID; 
    //    character chara = PhotonView.Find(charaid).GetComponent<character>();
    //    chara.GetObject(obj);
    //    //var dest = chara.transform.GetChild(0);
    //    //obj.transform.SetParent(dest.transform, true);
    //    //obj.transform.localPosition= new Vector3(0f,0f,0f);
    //}



}

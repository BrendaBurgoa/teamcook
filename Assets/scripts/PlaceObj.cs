using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObj : Photon.MonoBehaviour
{
    private Vector3 initialPosition;
    public string[] includedTags;
    public Vector3 offset;

    void Start(){
        initialPosition = gameObject.transform.position;
    }
    [PunRPC]
    private void deleteItem(int viewID)
    {
        var pv = PhotonView.Find(viewID);
        if (pv == null) return;
        if(Data.Instance.Rol == 0)
            PhotonNetwork.Destroy(pv);
        Destroy(pv.gameObject);
    }
    public void OnSelect(PhotonView pv)
    {
        photonView.RPC("deleteItem", PhotonTargets.All, pv.viewID);
        string _name = pv.gameObject.name;
        string[] arr = pv.gameObject.name.Split("(Clone)"[0]);
        if (arr.Length > 1)
            _name = arr[0];
        PhotonNetwork.Instantiate(_name, transform.position + offset, Quaternion.identity, 0);
    }
    [PunRPC]
    private void drop(int viewID)
    {
        var obj = PhotonView.Find(viewID);
        if (obj == null) return;
        obj.transform.position = gameObject.transform.position;
        obj.transform.SetParent(gameObject.transform);
    }
   
}

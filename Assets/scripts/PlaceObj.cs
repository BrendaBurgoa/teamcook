using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObj : Photon.MonoBehaviour
{
    public string[] includedTags;
    public Vector3 offset;
    public Vector3 rotationToPlace;
    float lastTimeItemPlaced;

    public void OnSelect(PhotonView pv)
    {        
        string _name = pv.gameObject.name;
        string[] arr = pv.gameObject.name.Split("(Clone)"[0]);
        if (arr.Length > 1)
            _name = arr[0];
        photonView.RPC("PlaceNewGO", PhotonTargets.MasterClient, _name, pv.viewID);
    }
    [PunRPC]
    private void PlaceNewGO(string _name, int viewID)
    {
        if (lastTimeItemPlaced != 0 && lastTimeItemPlaced + 1.5f > Time.time)
            return;

        //Hack para no cocinar en cualquier lado:
        if ( 
            (includedTags[0] == "patty" && transform.localPosition.x < 1)
            || 
            (includedTags[0] == "chopped_tomato" && transform.localPosition.x > -1)
           )
            return;
        

        lastTimeItemPlaced = Time.time;
        gameManager.Instance.DeleteItem(viewID);        
        GameObject go = PhotonNetwork.Instantiate(_name, transform.position + offset, Quaternion.Euler(rotationToPlace), 0);
        
        Coocker coocker = GetComponent<Coocker>();
        if (coocker != null)
            coocker.Added(go.GetComponent<PhotonView>());
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

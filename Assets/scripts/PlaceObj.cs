using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObj : Photon.MonoBehaviour
{
    character character;
    private Vector3 initialPosition;
    public string[] includedTags;

    void Start(){
        initialPosition = gameObject.transform.position;
    }

    void Update()
    {
        if (character == null)
            return;
        if (Input.GetKeyDown("space"))
        {
            PhotonView pv;
            if (character.HasSomething())
            {
                pv = HasSomethingToDrop();
                if (pv != null)
                    photonView.RPC("drop", PhotonTargets.All, pv.viewID);
            }
            else
            {
                if (gameObject.transform.childCount >= 1)
                {
                    pv = gameObject.transform.GetChild(0).GetComponent<PhotonView>();
                    if (pv != null)
                        photonView.RPC("pick", PhotonTargets.All, pv.viewID, character.photonView.viewID);
                }
            }
        }
    }
    PhotonView HasSomethingToDrop()
    {
        for (var i = 0; i < includedTags.Length; i++)
        {
            PhotonView pv = character.container.GetComponentInChildren<PhotonView>();
            if (pv.tag == includedTags[i])
                return pv;
        }
        return null;
    }
    void OnCollisionEnter(Collision other)
    {
        character _character = other.gameObject.GetComponent<character>();
        if (_character != null && _character.IsMe())
        {
            character= _character;
        }
    }

    void OnCollisionExit(Collision other){
        character _character = other.gameObject.GetComponent<character>();
        if (_character != null && _character.IsMe()) { 
            character= null;
        }
    }
    [PunRPC]
    private void drop(int viewID)
    {
        var obj = PhotonView.Find(viewID);
        if (obj == null) return;
        {
            obj.transform.SetParent(gameObject.transform);
            obj.transform.localPosition = Vector3.zero;
        }
    }
    [PunRPC]
    private void pick(int id, int characterID)
    {
        var ingredient = PhotonView.Find(id);   
        if (Data.Instance.Rol == 0)
            ingredient.GetComponent<PhotonView>().TransferOwnership(characterID);

        var _character = PhotonView.Find(characterID);
        character characterThatCatch = _character.GetComponent<character>();
        characterThatCatch.GetObject(ingredient.GetComponent<PhotonView>());
    }
}

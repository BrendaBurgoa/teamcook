using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class character : Photon.MonoBehaviour
{
    //los scripts de movimiento son propios del prefab del personaje y vinieron del store, Simple Sample Charact (editados para que solo si la vista del personaje es mia se pueda mover)
    public PhotonView photonView;
    public GameObject avatar;
    public int playerId;
    public string currentSide;
    Vector2 limits_x = new Vector2(3.47f,0.55f);
    Vector2 limits_z = new Vector2(4f, 0.32f);
    public Transform container;
    public PhotonView photonViewActive;

    public bool HasSomething()
    {
        return container.childCount > 0;
    }
    private void Start()
    {
        Events.OnNewPlayer(this);
    }
    public void GetObject(PhotonView objectPicked)
    {     
        ShowCollision sc = objectPicked.GetComponent<ShowCollision>();
        if (sc != null)
        {
            sc.ResetCollision();
            sc.SetCollision(false);
        }
        objectPicked.transform.SetParent(container);
        objectPicked.transform.localPosition = Vector3.zero;
    }
    public bool IsMe()
    {
        return photonView.isMine;
    }
    void Update()
    {
        if (!photonView.isMine) return;

        Vector3 pos = transform.position;

        if (currentSide == "left")
        {
            if (pos.x < -limits_x.x) pos.x = -limits_x.x;
            if (pos.x > -limits_x.y) pos.x = -limits_x.y;
        }
        else if (currentSide == "right")
        {
            if (pos.x > limits_x.x) pos.x = limits_x.x;
            if (pos.x < limits_x.y) pos.x = limits_x.y;
        }

        if (pos.z > limits_z.x) pos.z = limits_z.x - (0.01f);
        if (pos.z < limits_z.y) pos.z = limits_z.y + (0.01f);

        pos.y = 0;
        transform.position = pos;
       
        if (photonViewActive != null && Input.GetKeyDown("space"))
        {
            Coocker coocker = photonViewActive.GetComponent<Coocker>();
            if (coocker != null && !coocker.CanBeGrabbed())
                return;

            pick_drop pd = photonViewActive.GetComponent<pick_drop>();
            if (pd != null && !HasSomething())
            {
                photonView.RPC("pick", PhotonTargets.All, pd.photonView.viewID, photonView.viewID);
                photonViewActive = null;
            }
            else if (HasSomething())
            {
                PlaceObj po = photonViewActive.GetComponent<PlaceObj>();
                if(po != null)
                {
                    PhotonView pv = HasSomethingToDrop(po);
                    if (pv != null) po.OnSelect(pv);
                }
                orders o = photonViewActive.GetComponent<orders>();
                if (o != null) o.OnSelect(this);
            } else
            {
                instantiateObjects io = photonViewActive.GetComponent<instantiateObjects>();
                if (io != null) InstantiateObject(io);                
            }
        }
    }

    PhotonView HasSomethingToDrop(PlaceObj po)
    {
        for (var i = 0; i < po.includedTags.Length; i++)
        {
            PhotonView pv = container.GetComponentInChildren<PhotonView>();
            if (pv.tag == po.includedTags[i])
                return pv;
        }
        return null;
    }
    public PhotonView HasSomethingToDrop()
    {
        return container.GetComponentInChildren<PhotonView>();
    }

    void OnCollisionEnter(Collision other)
    {
        if (!photonView.isMine) return;
        if (photonViewActive == null || photonViewActive.GetComponent<pick_drop>() == null)
        {
            PhotonView pv = other.gameObject.GetComponent<PhotonView>();
            if (pv != null)
            {
                photonViewActive = pv;
                ShowCollision sc = photonViewActive.GetComponent<ShowCollision>();
                if (sc != null) sc.OnCharacterOver(true);
            }
        }
    }
    void OnCollisionExit(Collision other)
    {
        if (!photonView.isMine) return;
        PhotonView pv = other.gameObject.GetComponent<PhotonView>();
        if (pv != null)
        {
            ShowCollision sc = pv.GetComponent<ShowCollision>();
            if (sc != null) sc.OnCharacterOver(false);
            if(photonViewActive != null && photonViewActive == pv)
                photonViewActive = null;
        }
    }

    [PunRPC]
    private void pick(int id, int characterID)
    {
        PhotonView ingredient = PhotonView.Find(id);
        if (Data.Instance.Rol == 0)
            ingredient.GetComponent<PhotonView>().TransferOwnership(characterID);

        ingredient.GetComponent<ShowCollision>().SetCollision(false);

        var _character = PhotonView.Find(characterID);
        character characterThatCatch = _character.GetComponent<character>();
        characterThatCatch.GetObject(ingredient.GetComponent<PhotonView>());
    }
    public void InstantiateObject(instantiateObjects io)
    {
        var ingredient = PhotonNetwork.Instantiate(io.myPrefab.name, new Vector3(1000,0,0), Quaternion.identity, 0);
        photonView.RPC("instantiateIngredient", PhotonTargets.All, ingredient.GetComponent<PhotonView>().viewID, photonView.viewID);
    }
    [PunRPC]
    void instantiateIngredient(int id, int characterID)
    {
        var ingredient = PhotonView.Find(id);

        if (ingredient == null) return;
        ingredient.name = ingredient.name + PhotonView.Find(ingredient.GetComponent<PhotonView>().viewID);

        var _character = PhotonView.Find(characterID);
        if (_character == null)
        {
            Destroy(ingredient.gameObject);
        }
        else
        {
            if (Data.Instance.Rol == 0)
                ingredient.GetComponent<PhotonView>().TransferOwnership(characterID);

            character characterThatCatch = _character.GetComponent<character>();
            characterThatCatch.GetObject(ingredient.GetComponent<PhotonView>());
        }
    }
}

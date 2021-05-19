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
    SimpleSampleCharacterControl control;
    public List<ShowCollision> colliders;

    public bool HasSomething()
    {
        return container.childCount > 0;
    }
    private void Start()
    {
        control = GetComponent<SimpleSampleCharacterControl>();
        Events.OnNewPlayer(this);
    }
    public void GetObject(PhotonView objectPicked)
    {     
        ShowCollision sc = objectPicked.GetComponent<ShowCollision>();
        if (sc != null)
        {
            PickUp();
            sc.ResetCollision();
            sc.SetCollision(false);
        }
        objectPicked.transform.SetParent(container);
        objectPicked.transform.localPosition = Vector3.zero;
        ResetColliders();
    }
    public void PickUp()
    {
        control.PickUp();
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
          

            pick_drop pd = photonViewActive.GetComponent<pick_drop>();
            if (pd != null && !HasSomething())
            {
                Coocker coocker = photonViewActive.GetComponent<Coocker>();
                if (coocker != null && !coocker.CanBeGrabbed())
                    return;

                photonView.RPC("pick", PhotonTargets.All, pd.photonView.viewID, photonView.viewID);
                photonViewActive = null;
            }
            else if (HasSomething())
            {
                PlaceObj po = photonViewActive.GetComponent<PlaceObj>();
                if(po != null)
                {
                    PhotonView pov = HasSomethingToDropInside(po);
                    if (pov != null) po.OnSelect(pov);
                    PickUp();
                }
                Coocker coocker = photonViewActive.GetComponent<Coocker>();
                if (coocker != null && !coocker.CanBeGrabbed())
                    return;

                orders o = photonViewActive.GetComponent<orders>();
                if (o != null)
                {
                    PickUp();
                    o.OnSelect(this);
                }
                plate p = photonViewActive.GetComponent<plate>();
                PhotonView pv = HasSomethingToDrop();
                if (p != null && pv != null)
                {
                    PickUp();
                    p.OnSelect(pv, this);
                }
                trash t = photonViewActive.GetComponent<trash>();
                if (t != null && pv != null)
                {
                    PickUp();
                    t.OnSelect(pv, this);
                }
                extinguisher e = photonViewActive.GetComponent<extinguisher>();
                if (e != null && pv != null)
                {
                    PickUp();
                    e.OnSelect();
                }
            } else
            {
                instantiateObjects io = photonViewActive.GetComponent<instantiateObjects>();
                if (io != null) InstantiateObject(io);
                PickUp();   
            }
        }
    }

    PhotonView HasSomethingToDropInside(PlaceObj po)
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
    void ResetColliders()
    {
        if (photonViewActive != null)
            photonViewActive.GetComponent<ShowCollision>().OnCharacterOver(false);
        colliders.Clear();
        photonViewActive = null;
    }
    void AddCollider(ShowCollision showCollision, bool add)
    {
        if(add)
            colliders.Add(showCollision);
        else
        {
            showCollision.OnCharacterOver(false);
            colliders.Remove(showCollision);
        }
        SetColliderActive();
    }
    void SetColliderActive()
    {
        if (colliders.Count <= 0)
        {
            photonViewActive = null;
            return;
        }

        photonViewActive = GetPhotonviewActive();
        if (photonViewActive == null)
        {
            ResetColliders();
            return;
        }
        foreach (ShowCollision sc in colliders)
        {
            if(photonViewActive.viewID == sc.photonView.viewID)
                sc.OnCharacterOver(true);
            else
                sc.OnCharacterOver(false);
        }
    }
    PhotonView GetPhotonviewActive()  // jerarquiza los objetos en over y devuelve segun orden de importancia:
    {
        ShowCollision _sc = null;
        foreach (ShowCollision sc in colliders)
        {
            if (sc != null)
            {
                if (sc.GetComponent<Coocker>())
                    return sc.photonView;
                else if ( sc.GetComponent<pick_drop>())
                    _sc = sc;
                else if (_sc == null || _sc.GetComponent<pick_drop>() == null)
                    _sc = sc;
            }
        }
        if (_sc == null)
            return null;
        return _sc.photonView;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.isMine) return;

        PhotonView pv = other.gameObject.GetComponent<PhotonView>();
        if (pv == null) return;
        ShowCollision sc = pv.GetComponent<ShowCollision>();
        if (sc != null) AddCollider(sc, true);

        //if (photonViewActive == null || photonViewActive.GetComponent<pick_drop>() == null)
        //{
        //    PhotonView pv = other.gameObject.GetComponent<PhotonView>();
        //    if (pv != null)
        //    {
        //        photonViewActive = pv;
        //        ShowCollision sc = photonViewActive.GetComponent<ShowCollision>();
        //        if (sc != null) sc.OnCharacterOver(true);
        //    }
        //}
    }
    private void OnTriggerExit(Collider other)
    {
        if (!photonView.isMine) return;

        PhotonView pv = other.gameObject.GetComponent<PhotonView>();
        if (pv == null) return;
        ShowCollision sc = pv.GetComponent<ShowCollision>();
        if (sc != null) AddCollider(sc, false);


        //if (!photonView.isMine) return;
        //PhotonView pv = other.gameObject.GetComponent<PhotonView>();
        //if (pv != null)
        //{
        //    ShowCollision sc = pv.GetComponent<ShowCollision>();
        //    if (sc != null) sc.OnCharacterOver(false);
        //    if(photonViewActive != null && photonViewActive == pv)
        //        photonViewActive = null;
        //}
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
        var ingredient = PhotonNetwork.Instantiate(io.myPrefab.name, new Vector3(1000,0,0), io.myPrefab.transform.rotation, 0);
        photonView.RPC("instantiateIngredient", PhotonTargets.All, ingredient.GetComponent<PhotonView>().viewID, photonView.viewID);
    }
    [PunRPC]
    void instantiateIngredient(int viewID, int characterID)
    {
        var ingredient = PhotonView.Find(viewID);

        if (ingredient == null) return;

        var _character = PhotonView.Find(characterID);
        if (_character == null)
            gameManager.Instance.DeleteItem(viewID);
        else
        {
            if (Data.Instance.Rol == 0)
                ingredient.GetComponent<PhotonView>().TransferOwnership(characterID);

            character characterThatCatch = _character.GetComponent<character>();
            characterThatCatch.GetObject(ingredient.GetComponent<PhotonView>());
        }
    }
}

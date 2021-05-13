using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instantiateObjects : Photon.MonoBehaviour
{
    public GameObject myPrefab;
    public PhotonView photonView;
    public float val1;
    public float val2;
    private character character;
    GameObject ingredient = null;

    void Update()
    {
        if (character == null) return;

        if (Input.GetKeyDown("space") && !character.HasSomething())
        {
            if(transform.parent.position.x < 0){
                val1=2f;
                val2=3f;
            } else {
                val1= -2f;
                val2 = -3f;
            }
            float randomValue = Random.Range(val1, val2);
            var ingredient = PhotonNetwork.Instantiate(myPrefab.name, new Vector3(Random.Range(-2, -3), 0.5f, transform.position.z), Quaternion.identity, 0);
            photonView.RPC("instantiateIngredient", PhotonTargets.All, ingredient.GetComponent<PhotonView>().viewID, character.photonView.viewID);
            character = null;
        }
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
            //ingredient.GetComponent<pick_drop>().character = characterThatCatch;
            //ingredient.GetComponent<pick_drop>().isGrabbed = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        character _character = collision.gameObject.GetComponent<character>();
        if (_character != null && _character.IsMe() && !_character.HasSomething())
            character = _character;
    }

    void OnCollisionExit(Collision collision){
        character _character = collision.gameObject.GetComponent<character>();
        if (_character != null && _character.IsMe()){
            character=null;
        }
    }

    public void instantiateIngredient(int id)
    {
        //se le da nombre unico al ingrediente instanciado

        var ingredient = PhotonView.Find(id);
        ingredient.name = ingredient.name + PhotonView.Find(id);

        if(character != null){
            if(!character.HasSomething())
            {
                //si el personaje existe y no tinene nada el ingrediente instanciado se pone como hijo del empty del personaje
                character.GetObject(ingredient);
                ingredient.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.player);
                //ingredient.GetComponent<pick_drop>().character = character;
                //ingredient.GetComponent<pick_drop>().isGrabbed=true;
                ingredient.transform.localPosition=new Vector3(0, 0, 0);
            }
            else {
                    Debug.Log("se eliminaría");
                    Destroy(ingredient);
            }
        }
     
    }
    
}
 
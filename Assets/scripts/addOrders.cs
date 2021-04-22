using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class addOrders : Photon.MonoBehaviour
{
    public GameObject orderSimpleSalad;
    public GameObject orderFullSalad;
    public GameObject orderSimpleBurger;
    public GameObject orderFullBurger;
    public GameObject orderSimpleBurgerFries;
    public GameObject orderFullBurgerFries;
    public GameObject orderOnionSoup;
    public GameObject orderTomatoSoup;
    public PhotonView photonView;

    public void InstantiateOrder(int which){
        if (which == 1){
            var order = PhotonNetwork.Instantiate(orderSimpleSalad.name, transform.position, Quaternion.identity,0);
            photonView.RPC("setParent", PhotonTargets.All, order.GetComponent<PhotonView>().viewID );
        } 
        else if (which == 2){
            var order = PhotonNetwork.Instantiate(orderFullSalad.name, transform.position, Quaternion.identity,0);
            photonView.RPC("setParent", PhotonTargets.All, order.GetComponent<PhotonView>().viewID );
        }
        else if (which == 3){
            var order = PhotonNetwork.Instantiate(orderSimpleBurger.name, transform.position, Quaternion.identity,0);
            photonView.RPC("setParent", PhotonTargets.All, order.GetComponent<PhotonView>().viewID );
        }
        else if (which == 4){
            var order = PhotonNetwork.Instantiate(orderFullBurger.name, transform.position, Quaternion.identity,0);
            photonView.RPC("setParent", PhotonTargets.All, order.GetComponent<PhotonView>().viewID );
        }
        else if (which == 5){
            var order = PhotonNetwork.Instantiate(orderOnionSoup.name, transform.position, Quaternion.identity,0);
            photonView.RPC("setParent", PhotonTargets.All, order.GetComponent<PhotonView>().viewID );
        }
        else if (which == 6){
            var order = PhotonNetwork.Instantiate(orderTomatoSoup.name, transform.position, Quaternion.identity,0);
            photonView.RPC("setParent", PhotonTargets.All, order.GetComponent<PhotonView>().viewID );
        }
        else if (which == 7){
            var order = PhotonNetwork.Instantiate(orderFullBurgerFries.name, transform.position, Quaternion.identity,0);
            photonView.RPC("setParent", PhotonTargets.All, order.GetComponent<PhotonView>().viewID );
        }
        else if (which == 8){
            var order = PhotonNetwork.Instantiate(orderSimpleBurgerFries.name, transform.position, Quaternion.identity,0);
            photonView.RPC("setParent", PhotonTargets.All, order.GetComponent<PhotonView>().viewID );
        }
    }
    [PunRPC]
    private void setParent(int id){
      var order =  PhotonView.Find(id).gameObject; 
      order.transform.SetParent(gameObject.transform);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class addOrders : Photon.MonoBehaviour
{
    //lamadas por botones UI del manager, segun el numero instancia la tarjeta en la lista de ordenes
    public GameObject orderSimpleSalad;
    public GameObject orderFullSalad;
    public GameObject orderSimpleBurger;
    public GameObject orderFullBurger;
    public GameObject orderSimpleBurgerFries;
    public GameObject orderFullBurgerFries;
    public GameObject orderOnionSoup;
    public GameObject orderTomatoSoup;

    public void InstantiateOrder(int which){
        object[] data = new object[2];
        data[0] = "OrdersContent";
        GameObject order = null;
        if (which == 1){
            order = PhotonNetwork.Instantiate(orderSimpleSalad.name, transform.position, Quaternion.identity,0, data);
            photonView.RPC("setParent", PhotonTargets.All, order.GetComponent<PhotonView>().viewID );
        } 
        else if (which == 2){
            order = PhotonNetwork.Instantiate(orderFullSalad.name, transform.position, Quaternion.identity,0);
            photonView.RPC("setParent", PhotonTargets.All, order.GetComponent<PhotonView>().viewID );
        }
        else if (which == 3){
            order = PhotonNetwork.Instantiate(orderSimpleBurger.name, transform.position, Quaternion.identity,0);
            photonView.RPC("setParent", PhotonTargets.All, order.GetComponent<PhotonView>().viewID );
        }
        else if (which == 4){
            order = PhotonNetwork.Instantiate(orderFullBurger.name, transform.position, Quaternion.identity,0);
            photonView.RPC("setParent", PhotonTargets.All, order.GetComponent<PhotonView>().viewID );
        }
        else if (which == 5){
            order = PhotonNetwork.Instantiate(orderOnionSoup.name, transform.position, Quaternion.identity,0);
            photonView.RPC("setParent", PhotonTargets.All, order.GetComponent<PhotonView>().viewID );
        }
        else if (which == 6){
            order = PhotonNetwork.Instantiate(orderTomatoSoup.name, transform.position, Quaternion.identity,0);
            photonView.RPC("setParent", PhotonTargets.All, order.GetComponent<PhotonView>().viewID );
        }
        else if (which == 7){
            order = PhotonNetwork.Instantiate(orderFullBurgerFries.name, transform.position, Quaternion.identity,0);
            photonView.RPC("setParent", PhotonTargets.All, order.GetComponent<PhotonView>().viewID );
        }
        else if (which == 8){
            order = PhotonNetwork.Instantiate(orderSimpleBurgerFries.name, transform.position, Quaternion.identity,0);
            photonView.RPC("setParent", PhotonTargets.All, order.GetComponent<PhotonView>().viewID );
        }
        if(order != null)
            order.transform.localScale = Vector3.one;
        Events.NewOrder();
    }
    [PunRPC]
    private void setParent(int id){
          GameObject order =  PhotonView.Find(id).gameObject; 
          order.transform.SetParent(gameObject.transform);
          order.transform.localScale = Vector3.one;
    }


}

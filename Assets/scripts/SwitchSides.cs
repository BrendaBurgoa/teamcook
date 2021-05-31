using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSides : MonoBehaviour
{
    public GameObject[] AllPlayers;
    public GameObject OnionPattyButtons;
    public GameObject TomatoLettuceBreadButtons;
    private int whichSideChef;
    private int whichSideIngredient;
    public PhotonView photonView;

    
    public void SwitchChef(){
        //contador de lados se incrementa y dependiendo de que sea par o impar, se asigna la nueva posición
        whichSideChef=whichSideChef+1;
        photonView.RPC("SwitchChefToOtherSide", PhotonTargets.All, whichSideChef);
    }    
    
    public void SwitchIngredient(){
        //contador de lados se incrementa y dependiendo de que sea par o impar, se asigna la nueva posición
        whichSideIngredient=whichSideIngredient+1;
        photonView.RPC("SwitchIngredientToOtherSide", PhotonTargets.All, whichSideIngredient);
    }

    [PunRPC]
    public void SwitchChefToOtherSide(int side){
        //array con todos los jugadores se fija en su id y según sea par o impar, se fija de la clase character el lado actual y lo cambia. Y le asigna una posicion aleatoria nueva en el lado nuevo
        AllPlayers= GameObject.FindGameObjectsWithTag("character");
        for(int i=0; i<AllPlayers.Length; i++){
            var randomValuez = Random.Range(0.3f, 3.5f);
            if(AllPlayers[i].GetComponent<character>().playerId%2==1){
                if(side%2==1){
                    var randomValuex = Random.Range(-3f, -0.5f);
                    AllPlayers[i].transform.localPosition = new Vector3(randomValuex, 0 , randomValuez);
                    AllPlayers[i].GetComponent<character>().currentSide="left";
                }else{
                    var randomValuex = Random.Range(0.3f, 3f);
                    AllPlayers[i].transform.localPosition = new Vector3(randomValuex, 0 , randomValuez);
                    AllPlayers[i].GetComponent<character>().currentSide="right";
                }
            }else{
                if(side%2==1){
                    var randomValuex = Random.Range(0.3f, 3f);
                    AllPlayers[i].transform.localPosition = new Vector3(randomValuex, 0, randomValuez);
                    AllPlayers[i].GetComponent<character>().currentSide="right";
                }else{
                    var randomValuex = Random.Range(-3f, -0.5f);
                    AllPlayers[i].transform.localPosition = new Vector3(randomValuex, 0, randomValuez);
                    AllPlayers[i].GetComponent<character>().currentSide="left";
                }
            }
        }
    }
    [PunRPC]
     public void SwitchIngredientToOtherSide(int side){
        if(side%2==1){
            OnionPattyButtons.transform.localPosition = new Vector3(1.81f,0.5f,-6.8f);
            TomatoLettuceBreadButtons.transform.localPosition = new Vector3(-18.85f, 0.5f,-3f);
        }else{
            OnionPattyButtons.transform.localPosition = new Vector3(-18.7f, 0.5f,-4.41f);
            TomatoLettuceBreadButtons.transform.localPosition = new Vector3(1.72f, 0.5f,-5.5f);
        }
        }
}

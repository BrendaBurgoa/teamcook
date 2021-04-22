using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelfDestruct : MonoBehaviour
{
    public Image timer;
	private float waitTime = 120.0f;
    public float cookingTime;
    public PhotonView photonView;
    private bool Late;
    public System.DateTime createdDate;
    public bool finished;
    private string tag;

    void Start(){
        //al crearse la orden se renombra y añade el tiempo para ser distinguible
        createdDate=System.DateTime.Now;
        Late=false;
        finished=false;
        gameObject.name="ordered"+createdDate;
        tag = gameObject.tag;
    }
    void Update()
    {
        //si se es el manager y todavía no se venció, se incrementa el tiempo
        if(Data.Instance.Rol == 0){
            if(Late==false){
                cookingTime += Time.deltaTime;
                if (cookingTime > waitTime)
                {
                    //si el tiempo es mayor al limite se marca la orden como vencida
                    Late=true;
                }
            }
            //se disminuye el reloj visual de la orden
            photonView.RPC("timerFunction", PhotonTargets.All, (1.0f/waitTime * Time.deltaTime));
        }
      if (Late==true && Data.Instance.Rol==0){
          //si se vence aumenta el contador de ordenes vencidas 
             photonView.RPC("setLates", PhotonTargets.All, (Data.Instance.LateOrders+1));
      }
    }

    [PunRPC]
    private void timerFunction(float filler)
    {
                timer.fillAmount -= filler;
    }
    [PunRPC]
    private void setLates(int lates){
        Data.Instance.LateOrders = lates;
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowPoints : Photon.MonoBehaviour
{
    public Text points;
    public GameObject reStartBtn;
    public PhotonView photonView;
    private int results;
    public Image red;
    public Image yellow;
    public Image green;

    void Start()
    {
        //en la pantalla final toma el porcentaje de ordenes completadas y las que no se llegaron a hacer y reemplaza el texto por el porcentaje
        //de acuerdo al porcentaje resalta la imagen correcta
        if(Data.Instance.Rol == 0){        //si el jugador es manager se le muestra el boton de reinicio
            reStartBtn.SetActive(true);
        }
    }

    void Update(){
        if (Data.Instance.Rol ==0){
            photonView.RPC("WritePoints", PhotonTargets.All, Data.Instance.TimelyOrders, Data.Instance.LateOrders);
}
    }
    public void ReStart(){        
        photonView.RPC("goBack", PhotonTargets.All);
    }
    [PunRPC]
    private void WritePoints(int onTime, int late){
        results = ((onTime)*100)/(late + onTime);
        points.text=results+"%";
        if(results < 33.3f)
            red.GetComponent<Image>().color = Color.white;
        else if (results > 33.3f && results < 66.6f)
            yellow.GetComponent<Image>().color = Color.white;
        else if (results > 66.6f)
            green.GetComponent<Image>().color = Color.white;
    }
    [PunRPC]
    private void goBack(){
        //se resetean los valores necesarios y se cambia de escena al juego
	    Data.Instance.Point = 0;
	    Data.Instance.Time = 0;   
	    Data.Instance.LateOrders = 0;
	    Data.Instance.TimelyOrders = 0;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");  
    }
}

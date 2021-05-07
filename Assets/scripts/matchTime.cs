using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class matchTime : MonoBehaviour
{
    public GameObject StartCanvas;
    public GameObject stopCanvas;
    public Image timer;
    public GameObject[] AllPlayers;
    private int numplayers;
    public bool beginTimer = false;
    private bool changeColors = true;
    public float playTime;
    public PhotonView photonView;
    public GameObject StartButton;
    public gameManager gameManager;
    public Text connected;
    public Text roomName;
    public Text timeleft;
    private int playstate;
    private float fill;
     Color[] colors = { new Color(0,1,0,1), new Color(1,0,0,1), new Color(1,1,1,1), new Color(0,0,1,1),  new Color(1,1,0,1), new Color(0, 0, 0, 1)};

    void Start(){
        //cuando un usuario nuevo se conecta llama para todos (así le llega al manager) la funcion que indica su conexión 
            photonView.RPC("ShowConnected", PhotonTargets.All);    
            playstate = 1;    
            roomName.text=Data.Instance.roomName;
    }
    void Update(){

        if(beginTimer==true){
            //si la partida comienza se corre el tiempo
            photonView.RPC("timerMatch", PhotonTargets.All);
            if(changeColors==true && Data.Instance.Rol == 0){
            //se spawnean la primera vez con todos sombreros de color blanco, si se es manager y todavía no se cambió de color, se recorre cada jugador y mediante RPC se le cambia de color a un color asignado distinto
                AllPlayers= GameObject.FindGameObjectsWithTag("character");
                for(int i=0; i<AllPlayers.Length; i++){
                    photonView.RPC("SetColors", PhotonTargets.All, AllPlayers[i].name, i);        
                }
            }

        }
    }

    [PunRPC]
    private void SetColors(string name, int which){
        //busca al personaje del jugador indicado, busca el sombrero y le cambia de color
         GameObject.Find(name).transform.GetChild(1).transform.GetChild(0).GetComponent<Renderer>().material.color =colors[which];
    }

    public void StartTimer(){
    //es llamada por UI despues de seleccionar el largo de partida, comienza el tiempo y spawnea a jugadores mediante el script de game manager (porque lo tienen todos disponible y si se llamara de este no se ejecutaría en los jugadores)
        StartCanvas.SetActive(false);
        beginTimer=true;
        photonView.RPC("SpawnUsers", PhotonTargets.All);
        AllPlayers= GameObject.FindGameObjectsWithTag("character");    
    }

    [PunRPC]
    private void ShowConnected(){
        //es ejecutada cada vez que alguien se conecta pero solo es relevante al manager a quien se le muestra la lista de jugadores conectados 
            connected.text = "";
            foreach (var player in PhotonNetwork.otherPlayers)
                {
                    connected.text = connected.text +" " + player.name + ",";
                }
    }

    private void OnPhotonPlayerDisconnected(PhotonPlayer player){
        //si el manager pierde conexión o se desconecta, el juego temrina y se redirecciona al menu 
        //se utiliza en nombre en vez de Data.Rol porque el evento de pun no lo puede acceder
        if(player.name == "manager"){
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");            
        }
    }

    [PunRPC]
    private void timerMatch()
    {
        //se incrementa el tiempo y se toma el tiempo del manager (para asegurarse de que todos tengan el mismo dato) para modificar el reloj digital del countdown
            playTime += Time.deltaTime;
            if(Data.Instance.Rol ==0){
                photonView.RPC("FillTimer", PhotonTargets.All, playTime);        
                if (playTime > Data.Instance.Time)
                {
                    //si el tiempo actual excede el tiempo máximo establecido se cambia a la escena final
                    photonView.RPC("GoToEnd", PhotonTargets.All);        
                }
            }
    }


    [PunRPC]
    private void FillTimer(float lefttime){
        //modificación del reloj UI
        if((Mathf.Floor((Data.Instance.Time-lefttime)%60))>=10)
        {            
            timeleft.text = Mathf.Floor((Data.Instance.Time-lefttime)/60 )+":"+Mathf.Floor((Data.Instance.Time-lefttime)%60);
        }
        else{
            //para que aparezca tipo 9:07 en vez de 9:7 
            timeleft.text = Mathf.Floor((Data.Instance.Time-lefttime)/60 )+": 0"+Mathf.Floor((Data.Instance.Time-lefttime)%60);
        }  
    }
    
    [PunRPC]
    private void GoToEnd(){
                beginTimer=false;
                GameObject go = GameObject.Find("OrdersContent");
                Data.Instance.LateOrders = Data.Instance.LateOrders + go.transform.childCount;
                UnityEngine.SceneManagement.SceneManager.LoadScene("End");            
    }




    public void Reload(){
        photonView.RPC("ReloadLogic", PhotonTargets.All);
    }

    [PunRPC]
    public void ReloadLogic(){
        //si se reinicia el juego se va a ir a otra escena y volver a la actual para asegurar que los elementos no perduren en la partida nueva
        Data.Instance.LateOrders=0;
        Data.Instance.TimelyOrders=0;
        Data.Instance.fireExists=false;
        Data.Instance.Point=0;
        UnityEngine.SceneManagement.SceneManager.LoadScene("restart");            
    }
    

 public void SetTiming(int time){
        StartButton.SetActive(true);
        photonView.RPC("SetTime", PhotonTargets.All, time);
    }

    //seteo del largo de la partida
    [PunRPC]
    private void SetTime(int time){
        Data.Instance.Time=time;
    }


    [PunRPC]
    private void SpawnUsers(){
        if (Data.Instance.Rol == 1){
            //el manager no tiene personaje propio
            gameManager.SpawnPlayer();
        }

    }

//se cambia el estado del juego para pausar la partida
    public void ManagerMatchPause(){
        if (playstate == 1){
            playstate = 0;
        }else{
            playstate = 1;
        }
        photonView.RPC("PauseResume", PhotonTargets.All, playstate);
    }
    [PunRPC]
    void PauseResume (int value)
        {
            Time.timeScale = value;
            if(Data.Instance.Rol==1)
            {   if(value==0){
                 stopCanvas.SetActive(true);
                }else{
                 stopCanvas.SetActive(false);
                }
            }
        }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class matchTime : MonoBehaviour
{
    bool gameStarted;
    public GameObject StartCanvas;
    public GameObject stopCanvas;
    public Image timer;
    public PhotonView photonView;
    public GameObject StartButton;
    public gameManager gameManager;
    public Text connected;
    public Text roomName;
    public Text timeleft;
    private int playstate;
    private float fill;
     Color[] colors = { new Color(0,1,0,1), new Color(1,0,0,1), new Color(1,1,1,1), new Color(0,0,1,1),  new Color(1,1,0,1), new Color(0, 0, 0, 1)};
    private bool canStart;

    void Start(){
        StartButton.SetActive(false);
        StartButton.GetComponent<Button>().interactable = false;
        //cuando un usuario nuevo se conecta llama para todos (así le llega al manager) la funcion que indica su conexión 
        photonView.RPC("ShowConnected", PhotonTargets.All);    
            playstate = 1;    
            roomName.text=Data.Instance.roomName;
    }
    

    [PunRPC]
    private void SetColors(string name, int which){
        //busca al personaje del jugador indicado, busca el sombrero y le cambia de color
         GameObject.Find(name).transform.GetChild(1).transform.GetChild(0).GetComponent<Renderer>().material.color =colors[which];
    }
    float initialTime = 0;

    public void StartTimer()
    {
        if (Data.Instance.Rol == 0)
        {
            if (!canStart)
                return;
            initialTime = Time.time;
            OnTick();
        }
        InitGame();
    }
    void InitGame()
    {
        if (gameStarted) return;

        gameManager.ConnectingCanvas.SetActive(false);

        gameStarted = true;

        if (Data.Instance.Rol == 1)
            photonView.RPC("NewUser", PhotonTargets.All);
        StartCanvas.SetActive(false);
    }
    int lastPlayTime = 0;

    //only the manager:
    void OnTick()
    {
        int playTime = (int)(Time.time - initialTime);
        if (lastPlayTime < playTime)
        {
            lastPlayTime = playTime;

            if (playTime > Data.Instance.Time)
                photonView.RPC("GoToEnd", PhotonTargets.All);
            else
            {
                string timer;
                int diff = (Data.Instance.Time - playTime);
                if ((Mathf.Floor(diff % 60)) >= 10)
                    timer = Mathf.Floor(diff / 60) + ":" + Mathf.Floor(diff % 60);
                else
                    timer = Mathf.Floor(diff / 60) + ":0" + Mathf.Floor(diff % 60);

                photonView.RPC("FillTimer", PhotonTargets.All, timer);
            }
            CheckColors();
        }
        //photonView.RPC("timerMatch", PhotonTargets.All);
        Invoke("OnTick", 0.1f);
    }
    int lastTotalCharacters;
    void CheckColors()
    {
        GameObject[] AllPlayers = GameObject.FindGameObjectsWithTag("character");
        if(AllPlayers.Length>0 && lastTotalCharacters != AllPlayers.Length)
        {
            lastTotalCharacters = AllPlayers.Length;
            for (int i = 0; i < AllPlayers.Length; i++)
                photonView.RPC("SetColors", PhotonTargets.All, AllPlayers[i].name, i);
        }
    }
    void Update____()
    {

        //  if(beginTimer==true){
        //si la partida comienza se corre el tiempo
        photonView.RPC("timerMatch", PhotonTargets.All);
        //if (changeColors == true && Data.Instance.Rol == 0)
        //{
        //    //se spawnean la primera vez con todos sombreros de color blanco, si se es manager y todavía no se cambió de color, se recorre cada jugador y mediante RPC se le cambia de color a un color asignado distinto
        //    AllPlayers = GameObject.FindGameObjectsWithTag("character");
        //    for (int i = 0; i < AllPlayers.Length; i++)
        //    {
        //        photonView.RPC("SetColors", PhotonTargets.All, AllPlayers[i].name, i);
        //    }
        //}

        //   }
    }
    [PunRPC]
    private void ShowConnected(){
        //es ejecutada cada vez que alguien se conecta pero solo es relevante al manager a quien se le muestra la lista de jugadores conectados 
        connected.text = "";
        foreach (var player in PhotonNetwork.otherPlayers)
        {
            canStart = true;
            connected.text = connected.text +" " + player.name + ",";

            StartButton.GetComponent<Button>().interactable = true;
        }
    }

    private void OnPhotonPlayerDisconnected(PhotonPlayer player){
        //si el manager pierde conexión o se desconecta, el juego temrina y se redirecciona al menu 
        //se utiliza en nombre en vez de Data.Rol porque el evento de pun no lo puede acceder
        if(player.name == "manager"){
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");            
        }
    }

    //[PunRPC]
    //private void timerMatch()
    //{
    //    //se incrementa el tiempo y se toma el tiempo del manager (para asegurarse de que todos tengan el mismo dato) para modificar el reloj digital del countdown
    //        //playTime += Time.deltaTime;
    //  //  if(Data.Instance.Rol ==0){
    //        photonView.RPC("FillTimer", PhotonTargets.All, playTime);        
    //        if (playTime > Data.Instance.Time)
    //        {
    //            //si el tiempo actual excede el tiempo máximo establecido se cambia a la escena final
    //            photonView.RPC("GoToEnd", PhotonTargets.All);        
    //        }
    //  //  }
    //}


    [PunRPC]
    private void FillTimer(string timerString){
        if(!gameStarted)
            InitGame();
        timeleft.text = timerString;
    }
    
    [PunRPC]
    private void GoToEnd(){
        CancelInvoke();
               // beginTimer=false;
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

    bool alreadyAdded;
    [PunRPC]
    private void NewUser(){
        if(Data.Instance.Rol == 1 && !alreadyAdded)
        {
            alreadyAdded = true;
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

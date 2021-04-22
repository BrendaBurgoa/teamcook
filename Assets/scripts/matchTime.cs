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

    public void StartTimer(){
        StartCanvas.SetActive(false);
        beginTimer=true;
        photonView.RPC("SpawnUsers", PhotonTargets.All);
                AllPlayers= GameObject.FindGameObjectsWithTag("character");
        if(Data.Instance.Rol == 0){ 
            GameObject.Find("managerThings").GetComponent<ManagerTimer>().beginTimer = true;
        }          
    
    }
    void Start(){
            photonView.RPC("ShowConnected", PhotonTargets.All);    
            playstate = 1;    
            roomName.text=Data.Instance.roomName;
    }
    [PunRPC]
    private void ShowConnected(){
            connected.text = "";
            foreach (var player in PhotonNetwork.otherPlayers)
                {
                    connected.text = connected.text +" " + player.name + ",";
                }
    }
    void Update(){

        if(beginTimer==true){
            photonView.RPC("timerMatch", PhotonTargets.All);
            
            if(changeColors==true && Data.Instance.Rol == 0){
                AllPlayers= GameObject.FindGameObjectsWithTag("character");
                for(int i=0; i<AllPlayers.Length; i++){
                    photonView.RPC("SetColors", PhotonTargets.All, AllPlayers[i].name, i);        
                }
            }

        }
    }
    private void OnPhotonPlayerDisconnected(PhotonPlayer player){
        if(player.name == "manager"){
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");            
        }
    }

    // private void TimerMatch()
    // {
    //         playTime += Time.deltaTime;
    //         fill=(timer.fillAmount -= 1.0f/Data.Instance.Time * Time.deltaTime);
    //         photonView.RPC("FillTimer", PhotonTargets.All, fill, playTime);        
    //         if (playTime > Data.Instance.Time)
    //         {
    //                 photonView.RPC("GoToEnd", PhotonTargets.All);        
    //         }
    // }


    [PunRPC]
    private void timerMatch()
    {
            playTime += Time.deltaTime;
			// timer.fillAmount -= 1.0f/Data.Instance.Time * Time.deltaTime;
            if(Data.Instance.Rol ==0){
            photonView.RPC("FillTimer", PhotonTargets.All, playTime);        

            }
            if (playTime > Data.Instance.Time)
            {
                photonView.RPC("GoToEnd", PhotonTargets.All);        
            }
    }




    [PunRPC]
    private void FillTimer(float lefttime){
            timeleft.text = Mathf.Floor((Data.Instance.Time-lefttime)/60 )+":"+Mathf.Floor((Data.Instance.Time-lefttime)%60);
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
        Data.Instance.LateOrders=0;
        Data.Instance.TimelyOrders=0;
        Data.Instance.fireExists=false;
        Data.Instance.Point=0;
        // Scene scene = SceneManager.GetActiveScene(); 
        // SceneManager.LoadScene(scene.name);          
        UnityEngine.SceneManagement.SceneManager.LoadScene("restart");            
    }
    
    public void SetTiming(int time){
        StartButton.SetActive(true);
        photonView.RPC("SetTime", PhotonTargets.All, time);
    }

    [PunRPC]
    private void SetTime(int time){
        Data.Instance.Time=time;
    }

    [PunRPC]
    private void SpawnUsers(){
        if (Data.Instance.Rol == 1){
            gameManager.SpawnPlayer();
        }

    }

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


    [PunRPC]
    private void SetColors(string name, int which){
         GameObject.Find(name).transform.GetChild(1).transform.GetChild(0).GetComponent<Renderer>().material.color =colors[which];
    }
}

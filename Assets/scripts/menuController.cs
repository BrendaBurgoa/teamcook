using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menuController : MonoBehaviour
{
    [SerializeField] private string VersionName = "0.1";

    [SerializeField] private InputField UsernameInput;
    [SerializeField] private InputField CreateGameinput;
    [SerializeField] private InputField JoinGameinput;
    [SerializeField] private GameObject Create;
    [SerializeField] private GameObject Join;
    [SerializeField] private Text alert;
    [SerializeField] private GameObject masker;

    private string username = "";

    private void Awake(){
        PhotonNetwork.ConnectUsingSettings(VersionName);
        masker.SetActive(true);
    }

    private void Start()
    {
        Init();
    }
    public void Init()
    {
        Data.Instance.Reset();
        if (Data.Instance.isAdmin)
        {
            Create.SetActive(true);
            Join.SetActive(false);
        }
        else
        {
            Create.SetActive(false);
            Join.SetActive(true);
        }
    }
    public void CreateGame(){
        PhotonNetwork.CreateRoom(CreateGameinput.text, new RoomOptions(){maxPlayers = 7}, null);
        PhotonNetwork.playerName="manager";
        Data.Instance.Rol=0;
        Data.Instance.roomName=CreateGameinput.text;
        alert.text="creando nueva sala";
    }
    public void JoinGame()
    {   if(UsernameInput.text.Length >= 3 && UsernameInput.text.Length <= 10 && JoinGameinput.text.Length >= 1){
            PhotonNetwork.playerName = UsernameInput.text;
            PhotonNetwork.JoinRoom(JoinGameinput.text);
            Data.Instance.Rol=1;
            alert.text="buscando la sala";
        }else{
            alert.text="verifique lo ingresado";
        }
    }
    private void OnJoinedRoom(){
        alert.text="Se unió a la sala exitosamente";
        PhotonNetwork.LoadLevel("Game");
    }
    private void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        masker.SetActive(false);
        alert.text="Conectado al servidor";
    }
    private void OnCreateRoomFailed()
    {
        alert.text="Error al crear la sala";
    }
    private void OnCreateRoomFailed	(short returnCode, string message)
    {
        alert.text="Error al unirse a la sala";
    }
    private void OnCreatedRoom()
    {
        alert.text="Sala creada exitosamente";
    }
}

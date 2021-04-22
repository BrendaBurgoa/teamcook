using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class character : Photon.MonoBehaviour
{
   // public bool isGrabbing;
    public PhotonView photonView;
    public float speed;
    public float rotationSpeed;
    public GameObject avatar;
    public Animator animation;
    public Text PlayerName;
    public GameObject playerCanvas;
    private float moveSpeed = 2f;
    public int playerId;
    public string currentSide;
    void Start()
    {
            animation = avatar.GetComponent<Animator>();

    }
    private void Awake()
    {
        if (photonView.isMine){
            PlayerName.text = PhotonNetwork.playerName;
        }
        else{
            PlayerName.text = photonView.owner.name;
            PlayerName.color = Color.blue;
        }
    }

    void Update()
    {   if(transform.GetChild(0).childCount >= 2){
            Destroy(transform.GetChild(0).transform.GetChild(0).gameObject);
        }
        if (photonView.isMine){
            
            Vector3 pos = transform.position;

            if(pos.x < -3.5f)
            pos.x = -3.3f;
            else if (pos.x > 3.7f)
            pos.x = 3.6f;

            if(pos.z < 0.1f)
            pos.z = 0.2f;
            else if (pos.z > 4.1f)
            pos.z = 4;

            if(currentSide == "left"){
                if(pos.x > -0.3f){
                    pos.x = -0.5f;
                }
            } else if(currentSide == "right"){
                if(pos.x < 0.5f){
                    pos.x = 0.6f;
                }
            }

            transform.position = pos;
            playerCanvas.transform.rotation = Quaternion.identity;
        }

        
        
    }


}

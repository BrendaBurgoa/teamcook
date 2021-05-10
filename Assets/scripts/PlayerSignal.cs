using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerSignal : MonoBehaviour
{
    public Text usernameField;
    Transform target;

    public void Init(character character)
    {
        this.target = character.transform;
        //se muestra el nombre del personaje y si es propio es de otro color
        if (character.photonView.isMine)
        {
            usernameField.text = PhotonNetwork.playerName;
        }
        else
        {
            usernameField.text = character.photonView.owner.name;
            usernameField.color = Color.blue;
        }
        this.gameObject.SetActive(true);
    }

    void Update()
    {
        if (target)
        {
            Vector3 pos = target.transform.position;
            Vector2 viewportPoint = Camera.main.WorldToScreenPoint(pos);
            transform.position = viewportPoint;
        }
        else
            transform.localPosition = new Vector3(1000, 1000, 0);
    }
}

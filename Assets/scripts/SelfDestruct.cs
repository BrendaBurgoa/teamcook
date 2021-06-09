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
    private bool isReady;
    public System.DateTime createdDate;
    private string tag;

    void Start(){
        GameObject container = GameObject.Find("OrdersContent");
        transform.SetParent(container.transform);
        createdDate = System.DateTime.Now;
        gameObject.name="ordered"+createdDate;
        tag = gameObject.tag;
    }
    void Update()
    {
        cookingTime += Time.deltaTime;
        timerFunction(1.0f / waitTime * Time.deltaTime);
        if (Data.Instance.Rol == 0 && cookingTime > waitTime)
        {
            isReady = true;           
            photonView.RPC("setLates", PhotonTargets.All);
        }          
    }
    void timerFunction(float filler)
    {
        timer.fillAmount -= filler;
        if (timer.fillAmount < 0)
            timer.fillAmount = 0;
    }
    [PunRPC]
    private void setLates(){
        Destroy(gameObject);
        Data.Instance.LateOrders++;
        Events.OnRefreshPoints();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Coocker : MonoBehaviour
{
    public IngredientData[] ingredients;

    [Serializable]
    public class IngredientData
    {
        public string ingredientTag;
        public GameObject target;
    }
    public float duration = 13.0f;
    bool isCooking;
    public Image fillImage;
    float timer;
    public GameObject bar;
    public GameObject finalState;
    PhotonView photonView;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        Reset();
    }
    public void Reset()
    {
        timer = 0;
        fillImage.fillAmount = 0;
        bar.SetActive(false);
    }
    public bool CanBeGrabbed()
    {
        if (isCooking) return false;
        return true;
    }
    public void Added(PhotonView pv)
    {
        photonView.RPC("AddIngredient", PhotonTargets.All, pv.viewID);
    }
    private void Update()
    {
        if (!isCooking) return;
        timer += Time.deltaTime;
        if (timer >= duration)
            photonView.RPC("Ready", PhotonTargets.MasterClient);
        else
            fillImage.fillAmount = timer/ duration;
    }
    [PunRPC]
    private void StartCookingForAll()
    {
        bar.SetActive(true);
        isCooking = true;
        GetComponent<AudioSource>().Play();
    }
    [PunRPC]
    private void AddIngredient(int viewID)
    {
        PhotonView pv = PhotonView.Find(viewID);
        int totalIngredients = 0;
        foreach (IngredientData d in ingredients)
        {
            if (d.ingredientTag == pv.gameObject.tag)
                d.target = pv.gameObject;
            if (d.target != null)
                totalIngredients++;
        }
        if (totalIngredients >= ingredients.Length)
            photonView.RPC("StartCookingForAll", PhotonTargets.All);
    }
    [PunRPC]
    private void Ready()
    {
        print("Cook Ready " + Data.Instance.Rol);
        PhotonNetwork.Instantiate(finalState.name, transform.position, transform.rotation, 0);
        isCooking = false;            
        photonView.RPC("DestroyForAll", PhotonTargets.All);
    }
    [PunRPC]
    private void DestroyForAll()
    {
        GetComponent<AudioSource>().Stop();
        isCooking = false;

        print("ingredients: " + ingredients.Length);

        foreach (IngredientData id in ingredients)
        {
            print("id: " + id.target);
            if(id.target != null)
                Destroy(id.target.gameObject);
            id.target = null;
        }

        print("Destroy Parent" + ingredients.Length);
        Destroy(this.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Coocker : MonoBehaviour
{
    public Food[] foods;
    public Text qty_field;
    
    [Serializable]
    public class Food
    {
        public IngredientData[] ingredients;
        public GameObject finalState;
    }
    [Serializable]
    public class IngredientData
    {
        public string ingredientTag;
        public GameObject target;
    }
    public float duration = 13.0f;
    public bool isCooking;
    public Image fillImage;
    float timer;
    public GameObject bar;
    PhotonView photonView;
    string finalStateName;
    public bool replaceGO = true;
    public Animator anim;
    public Vector3 offset;
    public float offset_random_x;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        Reset();
    }
    public void Reset()
    {
        if(qty_field != null) qty_field.text = "";
        timer = 0;
        fillImage.fillAmount = 0;
        bar.SetActive(false);
    }
    public bool CanBeGrabbed()
    {
        foreach (Food f in foods)
            foreach (IngredientData d in f.ingredients)
                if (d.target != null)
                    return false;

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
        {
            if(Data.Instance.Rol == 0)
            {
                Vector3 newOffset = offset;
                newOffset.x += (float)UnityEngine.Random.Range(0, offset_random_x);
                PhotonNetwork.Instantiate(finalStateName, transform.position + newOffset, transform.rotation, 0);
                photonView.RPC("DestroyForAll", PhotonTargets.All);
            }
            isCooking = false;
        }
        else
            fillImage.fillAmount = timer / duration;
    }
    [PunRPC]
    private void StartCookingForAll()
    {
        if (anim != null)
            anim.enabled = true;
        bar.SetActive(true);
        isCooking = true;
        GetComponent<AudioSource>().Play();
    }
    [PunRPC]
    private void AddIngredient(int viewID)
    {
        PhotonView pv = PhotonView.Find(viewID);
       
        foreach (Food food in foods)
        {
            bool foodAdded = false;
            int totalIngredients = 0;
            foreach (IngredientData d in food.ingredients)
            {
                if (d.ingredientTag == pv.gameObject.tag && d.target == null && !foodAdded)
                {
                    foodAdded = true;
                    d.target = pv.gameObject;                   
                    finalStateName = food.finalState.name;
                }
                if (d.target != null)
                    totalIngredients++;
            }
            if (totalIngredients > 0)
            {
                if (qty_field != null) qty_field.text = "x" + totalIngredients;
            }
            if (totalIngredients >= food.ingredients.Length)
            {
                if (qty_field != null) qty_field.text = "";
                photonView.RPC("StartCookingForAll", PhotonTargets.All);
                return;
            }
        }
    }
    [PunRPC]
    private void DestroyForAll()
    {
        if (anim != null)
            anim.enabled = false;

        GetComponent<AudioSource>().Stop();
        isCooking = false;

        foreach (Food food in foods)
        {
            foreach (IngredientData id in food.ingredients)
            {
                if (id.target != null) gameManager.Instance.DeleteItem(id.target.gameObject.GetComponent<PhotonView>().viewID);
                id.target = null;
            }
        }
        if(replaceGO) gameManager.Instance.DeleteItem(photonView.viewID);
        Reset();
    }
}

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
    bool beginTimer;
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
        if (beginTimer)
            return false;
        return true;
    }
    public void Added(GameObject go)
    {
        int totalIngredients = 0;
        foreach (IngredientData d in ingredients)
        {
            if (d.ingredientTag == go.tag)
                d.target = go;
            if (d.target != null)
                totalIngredients++;
        }
        if (totalIngredients >= ingredients.Length)
            StartCooking();
    }
    void StartCooking()
    {
        bar.SetActive(true);
        beginTimer = true;
    }
    private void Update()
    {
        if (!beginTimer) return;
        timer += Time.deltaTime;
        if (timer >= duration)
            photonView.RPC("Ready", PhotonTargets.All);
        else
            fillImage.fillAmount = timer/ duration;
    }
    [PunRPC]
    private void Ready()
    {
        if(Data.Instance.Rol == 0)
            PhotonNetwork.Instantiate(finalState.name, transform.position, transform.rotation, 0);
        foreach(IngredientData id in ingredients)
            Destroy(id.target.gameObject);
        Destroy(this.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCollision : MonoBehaviour
{
    public Renderer ren;
    public Material[] mat;
    public Material[] shownMat;
    public Color matcolor;
    pick_drop pick_drop_go;
    Collider colliders;

    void Start () {
        ren = gameObject.GetComponent<Renderer>();
        mat = ren.materials;       
        matcolor =mat[0].color;
        pick_drop_go = gameObject.GetComponent<pick_drop>();
        colliders = GetComponent<Collider>();
    }
    public void ResetCollision()
    {
        if (ren == null) Start();
        SetColor(matcolor);
    }
    public void SetCollision(bool isOn)
    {
        if(colliders == null)
            colliders = GetComponent<Collider>();
        colliders.enabled = isOn;
    }
    public void OnCharacterOver(bool isOver)
    {
        if(isOver)
            SetColor(Color.green);
        else
            SetColor(matcolor);
    }
    void SetColor(Color matcolor)
    {
        shownMat = ren.materials;
        shownMat[0].color = matcolor;
        ren.materials = shownMat;
    }

}

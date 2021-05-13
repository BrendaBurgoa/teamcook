using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCollision : MonoBehaviour
{
    public Renderer ren;
    public Material[] mat;
    public Material[] shownMat;
    public Color matcolor;
    private bool grabbed;
    character character;
    pick_drop pick_drop_go;

    void Start () {
        ren = gameObject.GetComponent<Renderer>();
        mat = ren.materials;       
        matcolor =mat[0].color;
        pick_drop_go = gameObject.GetComponent<pick_drop>();
    }
    public void ResetCollision()
    {
        ren = gameObject.GetComponent<Renderer>();
        SetColor(matcolor);
    }
    //void Update(){
    //    if (pick_drop_go == null) return;
    //    if(transform.parent != null && transform.parent.tag == "destination")
    //        SetColor(matcolor);
    //}
    void OnCollisionEnter(Collision other){
        character _character = other.gameObject.GetComponent<character>();
        if (_character != null && _character.IsMe())
        {
            SetColor(Color.green);
            character = _character;
         }
     }
    void OnCollisionExit(Collision other){
        character _character = other.gameObject.GetComponent<character>();
        if (_character != null && _character.IsMe())
        {
            character = null;
            SetColor(matcolor);
        }
    }
    void SetColor(Color matcolor)
    {
        shownMat = ren.materials;
        shownMat[0].color = matcolor;
        ren.materials = shownMat;
    }

}

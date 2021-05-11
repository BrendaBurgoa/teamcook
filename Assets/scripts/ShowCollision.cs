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

    void Start () {
        ren = gameObject.GetComponent<Renderer>();
        mat = ren.materials;       
        matcolor =mat[0].color;
    }
    void Update(){
        if (gameObject.GetComponent<pick_drop>() != null && transform.parent != null){            
            if (transform.parent.tag == "destination"){
                SetColor(matcolor);
            }
        }
    }
    void OnCollisionEnter(Collision other){
         if (other.gameObject.tag == "character"){
            SetColor(Color.green);
            character = other.gameObject.GetComponent<character>();
         }
     }
    void OnCollisionExit(Collision other){
        if (other.gameObject.tag == "character"){
            if (character != null && character != other.gameObject) return;
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

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
private GameObject character;
    void Start () {
        //reconoce los materiales del objeto
        ren = gameObject.GetComponent<Renderer>();
        mat = ren.materials;
       
        matcolor =mat[0].color;
    }
    void Update(){
        if (gameObject.GetComponent<pick_drop>() != null && transform.parent != null){
            //si tiene padre y no clase para ser agarrado (elementos chopped) se chequea que el padre sea un personaje y se vuelven a poner los materiales originales 
            if (transform.parent.tag == "destination"){
                    shownMat=ren.materials;
                    shownMat[0].color = matcolor;
                    ren.materials=shownMat;
            }
        }
    }
    void OnCollisionEnter(Collision other){
         if (other.gameObject.tag == "character"){
             //si colisiona con un personaje se cambia el color del material por verde
                shownMat=ren.materials;
                shownMat[0].color = Color.green;
                ren.materials=shownMat;
                character = other.gameObject;
         }
     }
    void OnCollisionExit(Collision other){
        //si un personaje se aleja se retoman los materiales originales
        if (other.gameObject.tag == "character"){
                shownMat=ren.materials;
                shownMat[0].color = matcolor;
                ren.materials=shownMat;
        }
    }

}

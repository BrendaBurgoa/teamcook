using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ManagerTimer : MonoBehaviour
{
    public Image timer;
    public bool beginTimer = false;
    public float playTime;
    
    void Update(){
        if(beginTimer==true){
            if(Data.Instance.Rol == 0){           
                playTime += Time.deltaTime;
                timer.fillAmount -= 1.0f/Data.Instance.Time * Time.deltaTime;
            }            
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goback : MonoBehaviour
{
// lamado cuando se pasa a la escena de transicion RESTART
    void Start()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");            
    }
}

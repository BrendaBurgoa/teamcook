using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScene : MonoBehaviour
{
    public void init(bool isAdmin)
    {
        Data.Instance.isAdmin = isAdmin;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu"); 
    }
}

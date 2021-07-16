using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScene : MonoBehaviour
{
    void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        Loop();
#endif
    }
    void Loop()
    {
        if (Data.Instance.loaded)
            init(Data.Instance.isAdmin);
        else
            Invoke("Loop", 0.05f);
    }
    public void init(bool isAdmin)
    {
        Data.Instance.isAdmin = isAdmin;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu"); 
    }
}

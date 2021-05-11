using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DebugBox : MonoBehaviour
{
    public Text field;
    void Start()
    {
        Events.Log += Log;   
    }
    void OnDestroy()
    {
        Events.Log -= Log;
    }
    void Log(string text)
    {
        CancelInvoke();
        field.text += text;
        Invoke("Reset", 4);
    }
    void Reset()
    {
        field.text = "";
    }
}

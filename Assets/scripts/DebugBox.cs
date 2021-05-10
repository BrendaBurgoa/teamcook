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
        field.text = text;
    }
}

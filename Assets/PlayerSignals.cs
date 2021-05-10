using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSignals : MonoBehaviour
{
    public Camera mainCamera;
    public PlayerSignal playerSignal;

    void Start()
    {
        Events.OnNewPlayer += OnNewPlayer;
    }
    void OnDestroy()
    {
        Events.OnNewPlayer -= OnNewPlayer;
    }
    void OnNewPlayer(character ch)
    {
        PlayerSignal ps = Instantiate(playerSignal, transform);
        ps.transform.localScale = Vector2.one;
        ps.Init(ch);
    }
}

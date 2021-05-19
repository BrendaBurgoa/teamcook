using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pick_drop : Photon.PunBehaviour
{
    public Vector3 rotation = Vector3.zero;
    private void Start()
    {
        if(rotation != Vector3.zero)
            transform.localEulerAngles = rotation;
    }
}

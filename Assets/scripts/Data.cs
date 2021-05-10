using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public bool isAdmin;
    //información que persiste en las escenas

    const string PREFAB_PATH = "Data";    
    static Data mInstance = null;
	public int Rol;
	public int Point;
	public int Time;
	public string roomName;
    public int LateOrders;
    public int TimelyOrders;
    public bool fireExists;
	
	public static Data Instance
	{
		get
		{
			return mInstance;
		}
	}
    void Awake()
    {
        if (!mInstance)
            mInstance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}

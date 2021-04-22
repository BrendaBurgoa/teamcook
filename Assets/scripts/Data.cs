using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    const string PREFAB_PATH = "Data";    
    static Data mInstance = null;
	public int Rol;
	public int Point;
	public int Time;
	public string roomName;


    // public int LateOsoup;
    // public int LateTsoup;
    // public int LateSburger;
    // public int LateFburger;
    // public int LateSburgerFries;
    // public int LateFburgerFries;
    // public int LateSsalad;
    // public int LateFsalad;
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

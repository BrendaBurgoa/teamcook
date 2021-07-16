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
    public menuController menuController;
    public bool loaded;
	
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
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
        loaded = false;

#if UNITY_WEBGL && !UNITY_EDITOR
        URLParameters.Instance.RegisterOnDone((url) => {
           // Debug.Log("search parameters: " + url.Search);
           // Debug.Log("hash parameters: " + url.Hash);
            if (url.SearchParameters == null  || url.SearchParameters.Count == 0) return;
            string _isAdmin = url.SearchParameters["admin"];
            Debug.Log("_isAdmin: " + _isAdmin);
            if (_isAdmin == "yes")
                isAdmin = true;
            else
                isAdmin = false;
            loaded = true;
            menuController.Init();
        });
#endif
    }
    public void Reset()
    {
        LateOrders = 0;
        TimelyOrders = 0;
        fireExists = false;
        Point = 0;
    }
}

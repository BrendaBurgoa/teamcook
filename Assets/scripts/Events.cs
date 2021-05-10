using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Events
{
    public static System.Action<string> Log = delegate { };
    public static System.Action OnRefreshPoints = delegate { };
    public static System.Action NewOrder = delegate { };
    public static System.Action<character> OnNewPlayer = delegate { };

}

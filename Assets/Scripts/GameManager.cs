using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance== null) _instance = new GameManager();
            return _instance;
        }
    }
    private bool _isActive = false;
    public bool IsActive
    {
        get => _isActive;
        set => _isActive =  value;
    }
    
}

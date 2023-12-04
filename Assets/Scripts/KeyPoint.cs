using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class KeyPoint : MonoBehaviour
{
    [SerializeField]
    //List<CircleCollider2D> _touchObjects= new List<CircleCollider2D>(60);
    GameObject[] _touchObjects = new GameObject[10];
    int num = 0;
    [SerializeField]
    bool isTouching = false;
    [SerializeField]
    CircleCollider2D _collider;
    [SerializeField]
    CircleCollider2D[] _result = new CircleCollider2D[10];
    [SerializeField]
    private int _id = 0;
    [SerializeField]
    ContactFilter2D filter2D;
    public int ID
    {
        get => _id; 
        set => _id = value;
    }
    //public List<CircleCollider2D> TouchObjects
    //{
    //    get { return _touchObjects; }
    //}
    public GameObject[] TouchObjects
    {
        get { return _touchObjects; }
    }
    public bool IsTouching
    {
        get => isTouching;
        set => isTouching = value;
    }
    void Awake()
    {
        _collider= GetComponent<CircleCollider2D>();
        //IsTouching = CheckOverLap();
    }

    public void Check()
    {
        IsTouching = CheckOverLap();
    }

    public bool CheckOverLap()
    {
        int count = _collider.OverlapCollider(filter2D, _result);
        if(count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                if (_result[i].GetComponent<KeyPoint>() is KeyPoint)
                {
                    _touchObjects[num] = _result[i].gameObject;
                    num++;
                }
            }
            //foreach (var key in _result)
            //{
            //    if (key.GetComponent<KeyPoint>() != null)
            //    {
            //        _touchObjects.Add(key);
            //    }
            //}
        }
        if (_touchObjects.Length > 0)
        {
            return true;
        }
        return false;

    }
}

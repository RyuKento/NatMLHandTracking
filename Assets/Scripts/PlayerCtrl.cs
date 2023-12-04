using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    [SerializeField]
    CircleCollider2D _collider2D;
    [SerializeField]
    CircleCollider2D[] _results  = new CircleCollider2D[10];
    [SerializeField]
    ContactFilter2D filter2D;

    // Start is called before the first frame update
    void Start()
    {
        _collider2D= GetComponent<CircleCollider2D>();
       // CheckOverlapCollider();
    }

    // Update is called once per frame
    void Update()
    {
        //CheckOverlapCollider();
    }

    void CheckOverlapCollider()
    {
        int count = _collider2D.OverlapCollider(filter2D, _results);
        for(int i =0;i<count;i++) 
        {
            if (_results[i].GetComponent<PlayerCtrl>() == null)
            {
                Destroy(_results[i].gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.gameObject.transform.position.y <= -500)
        {
            Destroy(gameObject);
        }
    }
}

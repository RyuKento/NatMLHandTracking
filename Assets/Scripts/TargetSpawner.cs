using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject _canvas;
    [SerializeField]
    Image _targetDust;
    [SerializeField]
    Image[] _targetsMoney;
    [SerializeField]
    int _frequency = 100;
    [SerializeField]
    private float _interval = 1;
    [SerializeField]
    bool isMode = false; //false = dust ,true = money
    [SerializeField]
    bool isSpawn = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Run());
        isSpawn = true;
    }
    private void Update()
    {

    }

    IEnumerator Run()
    {
        if(isMode == true && _canvas.transform.childCount >= 0) { yield return null; }
        while (isSpawn)
        {
            isSpawn = false;
            yield return new WaitForSeconds(_interval);
            for (int i = 0; i < _frequency; i++)
            {
                var x = Random.Range(-900f, 900f);
                var y = Random.Range(-500f, 500f);
                var size = Random.Range(1f, 3f);
                if (isMode == false)
                {
                    SpawnDust(new(x, y, 0), size);
                    isSpawn= true;
                }
                else if (isMode == true)
                {
                    SpawnMoney(new(x, y, 0));
                    isSpawn= true;
                }
            }
        }
    }

    void SpawnDust(Vector3 vec,float size)
    {
        var pos = _canvas.transform.position+vec;
        var target = Instantiate(_targetDust, pos,_canvas.transform.rotation,_canvas.transform);
        target.transform.localScale*= size;
    }
    void SpawnMoney(Vector3 vec )
    {
        var pos = _canvas.transform.position + vec;
        var id = Random.Range(0,_targetsMoney.Length-1);
        var target = Instantiate(_targetsMoney[id], pos, _canvas.transform.rotation, _canvas.transform);
    }
}

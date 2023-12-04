using NatML.Visualizers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    [SerializeField]
    BlazePalmVisualizer _visualizer = default;
    [SerializeField]
    Image _konImage = default;
    [SerializeField]
    Image _etImage = default;
    GameManager _instance => GameManager.Instance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))        
        {
            //StartCoroutine(Activation_KON_Coroutine());
            UnityEditor.EditorApplication.isPaused = true;
        }
    }
    public void CheckActivationSkill()
    {
        if (!_instance.IsActive)
        {
            CheckActivation_KON();
            CheckActivation_ET();
        }
    }
    public bool JudgeTouch(KeyPoint keys,int id)
    {
        foreach (var key in keys.TouchObjects)
        {
            if (key.gameObject.GetComponent<KeyPoint>().ID == id)
            {
                return true;
            }
        }
        return false;
    }
    void JudgeActive(bool isMiddle, bool isThumb,bool isIndex)
    {
        if (isMiddle && isThumb && isIndex)
        {
            Debug.Log("Start");
            StartCoroutine(Activation_KON_Coroutine());
            _instance.IsActive = true;
        }
    }
    public void CheckActivation_KON()
    {
        bool isMiddle = false;
        bool isThumb = false;
        bool isIndex = false;
        Debug.Log("Check");
        var ringKey = _visualizer.CurrentHands[16].GetComponent<KeyPoint>();
        if (ringKey.IsTouching)
        {
            isMiddle = JudgeTouch(ringKey, 12);
            isThumb = JudgeTouch(ringKey, 4);
            var littleKey = _visualizer.CurrentHands[20].GetComponent<KeyPoint>();
            if (littleKey.IsTouching)
            {
                isIndex = JudgeTouch(littleKey, 8);
            }
            JudgeActive(isMiddle,isThumb,isIndex);
        }

    }
    public void CheckActivation_ET()
    {
        if (_visualizer.CurrentHands.Count > 21)
        {
            Debug.Log("CheckET");
            var leftIndexKey = _visualizer.CurrentHands[29].GetComponent<KeyPoint>();
            if (leftIndexKey.IsTouching)
            {
                foreach (var key in leftIndexKey.TouchObjects)
                {
                    if (key.gameObject.GetComponent<KeyPoint>().ID == 8)
                    {
                        StartCoroutine(Activation_ET_Coroutine());
                    }
                }
            }

        }
    }
    public IEnumerator Activation_KON_Coroutine()
    {
        if (_visualizer.CurrentHands[5])
        {
            _konImage.transform.position = _visualizer.CurrentHands[5].transform.position;
        }
        _konImage.gameObject.SetActive(true);

        yield return FadeOut(_konImage);

        yield return FadeIn(_konImage);
        _konImage.gameObject.SetActive(false);
        _instance.IsActive= false;
    }
    public IEnumerator Activation_ET_Coroutine()
    {
        Debug.Log("ET");
        _etImage.gameObject.SetActive(true);

        yield return FadeOut(_etImage);

        yield return FadeIn(_etImage);
        _etImage.gameObject.SetActive(false);
        _instance.IsActive = false;
    }
    private IEnumerator FadeOut(Image image)
    {
        var color = image.color;
        color.a = 0;
        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            color.a = 1 * (i / 1);
            image.color = color;
            yield return null;
        }
    }
    private IEnumerator FadeIn(Image image)
    {
        var color = image.color;
        color.a = 1;
        for (float i = 1; i > 0; i -= Time.deltaTime)
        {
            color.a = 1 * (i / 1);
            image.color = color;
            yield return null;
        }
    }
}

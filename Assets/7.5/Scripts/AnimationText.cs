using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationText : MonoBehaviour
{
    public Text text;
    public float maxSize;
    public float minSize;
    public float speed;

    private RectTransform _rectTransform;
    private float _newSize;
    
    void Start()
    {
        _rectTransform = text.GetComponent<RectTransform>();
    }

    void Update()
    {
        _newSize = Mathf.PingPong(Time.time * speed, maxSize - minSize);
        if (_newSize >= maxSize)
        {
            
            _rectTransform.localScale = new Vector3(_newSize, _newSize, 1);
        }
        else if (_newSize <= minSize)
        {
            _rectTransform.localScale = new Vector3(maxSize - _newSize, maxSize - _newSize, 1);
        }

        
    }
}

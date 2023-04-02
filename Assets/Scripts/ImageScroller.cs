using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageScroller : MonoBehaviour
{
    [SerializeField] private RawImage _img;
    [SerializeField] private float _imgX, _imgY;

    void Update()
    {
        _img.uvRect = new Rect(_img.uvRect.position + new Vector2(_imgX, _imgY) * Time.deltaTime, _img.uvRect.size);
    }
}

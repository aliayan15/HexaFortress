using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ToolTip : MonoBehaviour
{
    [SerializeField] LayoutElement layoutElement;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] RectTransform rectTransform;
    [SerializeField] int characterLimit;


    public void SetText(string text)
    {
        this.text.text = text;

        //int length = text.Length;
        //layoutElement.enabled = length > characterLimit ? true : false;

        AdjustPosition();
    }

    private void Update()
    {
        AdjustPosition();
    }

    private void AdjustPosition()
    {
        Vector2 mousePos = Input.mousePosition;
        float pivotX = mousePos.x / Screen.width;

        rectTransform.pivot = new Vector2(pivotX, 0);
        transform.position = mousePos;
    }
}


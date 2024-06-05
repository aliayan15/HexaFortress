using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ToolTip : MonoBehaviour
{
    [SerializeField] LayoutElement layoutElement;
    [SerializeField] TextMeshProUGUI content;
    [SerializeField] TextMeshProUGUI header;
    [SerializeField] RectTransform rectTransform;
    [Header("Settings")]
    [SerializeField] int characterLimit;
    [SerializeField] int posOffset = 20;

    private float _posYBorder;

    private void Awake()
    {
        _posYBorder = Screen.height - (Screen.height / 3f);
    }

    public void SetText(string text)
    {
        this.content.text = text;
        this.header.gameObject.SetActive(false);

        int length = text.Length;
        layoutElement.enabled = length > characterLimit ? true : false;

        Init();
    }
    public void SetText(string content, string header)
    {
        this.content.text = content;
        this.header.gameObject.SetActive(true);
        this.header.text = header;

        int length = content.Length;
        layoutElement.enabled = length > characterLimit ? true : false;

        Init();
    }

    private void Update()
    {
        Init();
    }

    private void Init()
    {
        Vector2 mousePos = Input.mousePosition;
        float pivotX = mousePos.x / Screen.width;
        bool PosYCon = mousePos.y > _posYBorder;
        float pivotY = PosYCon ? 1 : 0;
        mousePos.y += PosYCon ? -posOffset : posOffset;

        rectTransform.pivot = new Vector2(pivotX, pivotY);
        transform.position = mousePos;
    }
}


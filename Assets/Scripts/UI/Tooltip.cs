using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tooltip : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI HeaderText;
    [SerializeField]
    public TextMeshProUGUI ContentText;
    [SerializeField]
    private LayoutElement layoutElement;

    public int characterWrapLimit;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        SetTooltip();
    }

    private void Update()
    {
        Vector2 position = Input.mousePosition;
        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;
        rectTransform.pivot = new Vector2(pivotX, pivotY);
        transform.position = position;
    }

    private void SetTooltip()
    {
        int headerLength = HeaderText.text.Length;
        int contentLength = ContentText.text.Length;

        layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit) ? true : false;
    }

    public void SetTooltipText(string header = "", string content = "")
    {
        if(string.IsNullOrEmpty(header))
        {
            HeaderText.gameObject.SetActive(false);
        } else
        {
            HeaderText.gameObject.SetActive(true);
            HeaderText.text = header;
        }
        
        if(string.IsNullOrEmpty(content))
        {
            ContentText.text = "Content text is missing or empty";
        } else
        {
            ContentText.text = content;
        }
    }


}

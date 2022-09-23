using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTip : MonoBehaviour
{
    [SerializeField] private Vector2 tooltipOffset;
    private Vector2 tempOffset;
    private RectTransform rectTransform;
    
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void Update()
    {
        ToolTipFollowMouse();
    }
    
    private void ToolTipFollowMouse()
    {
        if(Input.mousePosition.y < tooltipOffset.y + rectTransform.rect.height)
            tempOffset.y = tooltipOffset.y + rectTransform.rect.height;
        else
            tempOffset.y = tooltipOffset.y;
        gameObject.transform.position = Input.mousePosition + new Vector3(tooltipOffset.x, tempOffset.y, 0);
    }
}

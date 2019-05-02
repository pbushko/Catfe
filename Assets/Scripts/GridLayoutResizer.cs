using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridLayoutResizer : MonoBehaviour
{
    public RectTransform fullPanel;
    private float width;
    private float height;
 
    // Use this for initialization
    void Start ()
    {
        width = fullPanel.rect.width;
        height = fullPanel.rect.height;
        Vector2 newSize = new Vector2(width / 3f, height / 4);
        this.gameObject.GetComponent<GridLayoutGroup>().cellSize = newSize;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

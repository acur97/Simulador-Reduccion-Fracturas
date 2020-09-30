using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridAutoSize : MonoBehaviour {

    public enum scenas {Historial_clinico, xRays, Puertos};
    public scenas scn;
    
    private GridLayoutGroup grid;
    private RectTransform rect;
    private ScrollRect Srect;

    private void Awake()
    {
        if (scn == scenas.Historial_clinico)
        {
            grid = GetComponent<GridLayoutGroup>();
            float cellS = Screen.height * 0.799f;
            float spa = Screen.height * 0.01f;
            float pad1 = Screen.height * 0.012f;
            float pad2 = Screen.height * 0.004f;
            grid.cellSize = new Vector2(cellS, cellS);
            grid.spacing = new Vector2(0, spa);
            grid.padding.left = Mathf.RoundToInt(pad1);
            grid.padding.top = Mathf.RoundToInt(pad2);
            grid.padding.bottom = Mathf.RoundToInt(pad2);
        }
        if (scn == scenas.xRays)
        {
            grid = GetComponent<GridLayoutGroup>();
            float cellS1 = Screen.height * 1.3112f;
            float cellS2 = Screen.height * 0.78f;
            float spa = Screen.height * 0.014f;
            float pad1 = Screen.height * 0.002f;
            float pad2 = Screen.height * 0.007f;
            grid.cellSize = new Vector2(cellS1, cellS2);
            grid.spacing = new Vector2(0, spa);
            grid.padding.left = Mathf.RoundToInt(pad1);
            grid.padding.top = Mathf.RoundToInt(pad2);
            grid.padding.bottom = Mathf.RoundToInt(pad2);
        }
        if (scn == scenas.Puertos)
        {
            grid = GetComponent<GridLayoutGroup>();
            float cellS = Screen.height * 0.231f;
            float cellS2 = Screen.height * 0.047f;
            float spa = Screen.height * 0.019f;
            float pad = Screen.height * 0.003f;
            grid.cellSize = new Vector2(cellS, cellS2);
            grid.spacing = new Vector2(0, pad);
            grid.padding.top = Mathf.RoundToInt(pad);
        }
    }
}

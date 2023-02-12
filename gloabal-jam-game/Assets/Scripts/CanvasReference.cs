using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasReference : MonoBehaviour
{
    public GameObject colorFilter;
    public GameObject coinDisplay;
    public TextMeshProUGUI coinText;
    public RectTransform selector;
    public GameObject uiDisplayTemplate;

    public static CanvasReference Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}

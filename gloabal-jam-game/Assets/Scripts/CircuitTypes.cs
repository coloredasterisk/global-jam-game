using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitTypes : MonoBehaviour
{
    private CircuitTile circuitTile;
    public enum circuits {
        TShape,
        LShape,
        PlusShape,
        Line,
    }
    // Start is called before the first frame update
    void Start()
    {
        circuitTile = GetComponent<CircuitTile>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

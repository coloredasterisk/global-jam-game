using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitTypes : MonoBehaviour
{
    private CircuitTile circuitTile;

    private GridComponent gridComponent;
    private GridManager gridManager;
    // Start is called before the first frame update
    void Start()
    {
        circuitTile = GetComponent<CircuitTile>();
        gridComponent = GetComponent<GridComponent>();
        gridManager = GetComponent<GridManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

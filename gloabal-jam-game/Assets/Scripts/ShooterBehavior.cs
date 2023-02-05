using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterBehavior : MonoBehaviour
{
    public Vector2Int directionFacing;
    public GridComponent gridComponent;

    private void Awake()
    {
        gridComponent = GetComponent<GridComponent>();
    }


  /*  void CreateLaser()
    {
        GridManager.CheckItemAtPosition(gridComponent.GetComponentsInParent<ParentState>(), );
    } */
}

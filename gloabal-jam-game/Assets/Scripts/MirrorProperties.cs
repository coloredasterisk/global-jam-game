using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorProperties : MonoBehaviour
{
    private GridComponent gc;
    // Start is called before the first frame update
    void Start()
    {
        gc = GetComponent<GridComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void reflectLaser(TileType mirror) {
        Vector3 leftPosition = transform.position + new Vector3 (-1,0,0);
        Vector2 rightPosition = transform.position + new Vector3 (1,0,0);
        Vector2 upPosition = transform.position + new Vector3 (0,1,0);
        Vector2 downPosition = transform.position + new Vector3 (0,-1,0);
        if (mirror == TileType.Mirror45 ) {
            if (GridManager.CheckItemAtPosition(gc, TileType.LaserHorizontal, leftPosition) != null){ //check if laser is on the left
                //spawn laser upwards

            } else if (GridManager.CheckItemAtPosition(gc, TileType.LaserVertical, upPosition) != null) {
                //spawn laser leftwards
            }else if (GridManager.CheckItemAtPosition(gc, TileType.LaserHorizontal, rightPosition) != null) {
                //spawn laser downwards
            }
            else if (GridManager.CheckItemAtPosition(gc, TileType.LaserVertical, downPosition) != null) {
                //spawn laser rightwards
            }
        } else if (mirror == TileType.Mirror135) {
            if (GridManager.CheckItemAtPosition(gc, TileType.LaserHorizontal, leftPosition) != null){ //check if laser is on the left
                //spawn laser downwards
            } else if (GridManager.CheckItemAtPosition(gc, TileType.LaserVertical, downPosition) != null) {
                //spawn laser leftwards
            }else if (GridManager.CheckItemAtPosition(gc, TileType.LaserHorizontal, rightPosition) != null) {
                //spawn laser upwards
            }
            else if (GridManager.CheckItemAtPosition(gc, TileType.LaserVertical, upPosition) != null) {
                //spawn laser rightwards
            }
        }

    }

}

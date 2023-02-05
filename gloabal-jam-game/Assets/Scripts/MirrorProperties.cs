using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorProperties : MonoBehaviour
{

    private GridComponent gc;
    private SpriteRenderer sr;

    public Sprite intialSprite;
    public Sprite topleft;
    public Sprite topRight;
    public Sprite bottomleft;
    public Sprite bottomright;

    // Start is called before the first frame update
    void Awake()
    {
        gc = GetComponent<GridComponent>();
        sr = GetComponent<SpriteRenderer>();
        intialSprite = sr.sprite;
    }
    public void ResetMirror()
    {
        sr.sprite = intialSprite;
    }

    public Vector2Int reflectLaser(TileType mirror, GridComponent cause) {
        Vector3 leftPosition = transform.position + new Vector3 (-1,0,0);
        Vector2 rightPosition = transform.position + new Vector3 (1,0,0);
        Vector2 upPosition = transform.position + new Vector3 (0,1,0);
        Vector2 downPosition = transform.position + new Vector3 (0,-1,0);
        if (mirror == TileType.Mirror45 ) {
            if (GridManager.CheckItemAtPosition(gc, TileType.LaserHorizontal, leftPosition) != null){ //check if laser is on the left
                //spawn laser upwards
                sr.sprite = topleft;
                return Vector2Int.up;
                

            } else if (GridManager.CheckItemAtPosition(gc, TileType.LaserVertical, upPosition) != null) {
                //spawn laser leftwards
                sr.sprite = topleft;
                return Vector2Int.left;

            }
            else if (GridManager.CheckItemAtPosition(gc, TileType.LaserHorizontal, rightPosition) != null) {
                //spawn laser downwards
                sr.sprite = bottomleft;
                return Vector2Int.down;
            }
            else if (GridManager.CheckItemAtPosition(gc, TileType.LaserVertical, downPosition) != null) {
                //spawn laser rightwards
                sr.sprite = bottomleft;
                return Vector2Int.right;
            }
        } else if (mirror == TileType.Mirror135) {
            if (GridManager.CheckItemAtPosition(gc, TileType.LaserHorizontal, leftPosition) != null){ //check if laser is on the left
                //spawn laser downwards
                sr.sprite = bottomright;
                return Vector2Int.down;

            } else if (GridManager.CheckItemAtPosition(gc, TileType.LaserVertical, downPosition) != null) {
                //spawn laser leftwards
                sr.sprite = bottomright;
                return Vector2Int.left;

            }
            else if (GridManager.CheckItemAtPosition(gc, TileType.LaserHorizontal, rightPosition) != null) {
                //spawn laser upwards
                sr.sprite = topRight;
                return Vector2Int.up;
            }
            else if (GridManager.CheckItemAtPosition(gc, TileType.LaserVertical, upPosition) != null) {
                //spawn laser rightwards
                sr.sprite = topRight;
                return Vector2Int.right;
            }

        }
        return Vector2Int.zero;

    }

    public Vector2Int toVector2(Vector2 v)
    {
        return new Vector2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
    }

}

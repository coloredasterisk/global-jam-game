using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarBehaviour : MonoBehaviour
{


    public PressurePlateBehavior pressure;
    private ShooterBehavior laser;
    private GridComponent gc;

    public Sprite offSprite;
    public Sprite onSprite;

    private List<TileType> horizontal = new List<TileType> {TileType.PressurePlate};
    private List<TileType> vertical = new List<TileType> {TileType.LaserVertical};



    void Start()
    {
        pressure = GetComponent<PressurePlateBehavior>();
        laser = GetComponent<ShooterBehavior>();
        gc = GetComponent<GridComponent>();
    }

    public void SwitchSprites(bool on)
    {
        if (on)
        {
            GetComponent<SpriteRenderer>().sprite = onSprite;
           
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = offSprite;
        }
    }


    
    public void checkLaser(){
        Vector2Int leftPosition = gc.gridPosition + new Vector2Int (-1,0);
        Vector2Int rightPosition = gc.gridPosition +  new Vector2Int (1,0);
        Vector2Int upPosition = gc.gridPosition +  new Vector2Int (0,1);
        Vector2Int downPosition = gc.gridPosition + new Vector2Int (0,-1);

        
        if ((GridManager.CheckItemAtPosition(gc, TileType.LaserHorizontal, leftPosition)!= null )
        || (GridManager.CheckItemAtPosition(gc, TileType.LaserHorizontal, rightPosition)!= null) 
        || (GridManager.CheckItemAtPosition(gc, TileType.LaserHorizontal, upPosition)!= null) || 
        (GridManager.CheckItemAtPosition(gc, TileType.LaserHorizontal, downPosition)!= null)) {
            Debug.Log("true");
            pressure.SwitchOn();
            
            

        } else {
            
            pressure.SwitchOff();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEditor.Tilemaps;
using UnityEngine;

public enum TileType
{
    Empty,
    Player,
    Block,
    Wall
}

public class GridComponent : MonoBehaviour
{
    //Attach this script to a game object to track items
    public TileType tileType;
    public Vector2Int gridPosition;
    public bool isLerping = false;
    private float lerpSpeed = 10f;


    // Start is called before the first frame update
    void Start()
    {
        if(gridPosition == null)
        {
            gridPosition = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        }
        else
        {
            transform.position = new Vector3(gridPosition.x, gridPosition.y, 0);
        }
        GridManager.InsertSelf(this);
    }

    public void MovePosition(int x, int y)
    {
        Vector2Int newPos = gridPosition + new Vector2Int(x,y); 
        if(GridManager.MoveSelf(this, newPos))
        {
            gridPosition = newPos;
        } else
        {
            //Check if object is pushable 
            GridComponent pushable = GridManager.CheckItemAtPosition(TileType.Block, newPos);
            if(pushable != null)
            {
                if(GridManager.MoveSelf(pushable, newPos + new Vector2Int(x, y)))
                {
                    pushable.gridPosition = newPos + new Vector2Int(x, y);
                    pushable.animatePosition();

                    //try moving again
                    if(GridManager.MoveSelf(this, newPos))
                    {
                        gridPosition = newPos;
                    }
                }
            }

        }
        animatePosition();
    }



    public void animatePosition()
    {
        if (!isLerping)
        {
            StartCoroutine(LerpPosition());
        }
    }

    private IEnumerator LerpPosition()
    {
        float timeScale = 0f;
        isLerping = true;
        while (timeScale < 1)
        {
            timeScale += Time.deltaTime * lerpSpeed;
            float x = Mathf.Lerp(transform.position.x , gridPosition.x, timeScale);
            float y = Mathf.Lerp(transform.position.y, gridPosition.y, timeScale);
            transform.position = new Vector3(x, y, 0);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        isLerping = false;
    }
}

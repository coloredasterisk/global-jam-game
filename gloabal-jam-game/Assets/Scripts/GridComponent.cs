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
    Wall,
    SpawnPoint,
    PlayerForcefield,
    PushableForcefield,

}

public class GridComponent : MonoBehaviour
{
    [Header("Let grid position use the world position")]
    public bool useWorldPosition = true;
    [Header("")]
    //Attach this script to a game object to track items
    public TileType tileType;
    public Vector2Int gridPosition;
    public bool isLerping = false;
    
    private float lerpSpeed = 10f;


    // Start is called before the first frame update
    void Awake()
    {
        if(useWorldPosition)
        {
            gridPosition = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        }
        else
        {
            transform.position = new Vector3(gridPosition.x, gridPosition.y, 0);
        }
        GridManager.InsertSelf(this);
    }
    public void MovePosition(Vector2Int v)
    {
        MovePosition(v.x, v.y);
    }
    public void MovePosition(int x, int y)
    {
        Vector2Int moveDirection = new Vector2Int(x, y);
        Vector2Int newPos = gridPosition + moveDirection;

        bool moved = AttemptToMove(this, newPos);

        if(!moved){
            
            StateType state = GetComponentInParent<ParentState>().stateType;
            bool similarPush = CheckPushable(state, newPos, moveDirection);
        }

        
        if(GetComponent<PlayerController>() != null)
        {
            LevelBehavior level = FindObjectOfType<GameManager>().currentLevel;
            //check if player is out of level bounds
            level.checkBoundsBehavior(this);
        }
    }
    public void RemoveFromGrid()
    {
        GridManager.RemoveSelf(this);
    }

    public void animatePosition()
    {
        if (!isLerping)
        {
            StartCoroutine(LerpPosition());
        }
    }
    private bool AttemptToMove(GridComponent component, Vector2Int moveTo)
    {
        if(GridManager.MoveSelf(component, moveTo))
        {
            gridPosition = moveTo;
            animatePosition();
            return true;
        }
        return false;
    }
    private bool CheckPushable(StateType state, Vector2Int newPos, Vector2Int moveDirection)
    {
        //Check if object is pushable 
        GridComponent pushable = GridManager.CheckItemAtPosition(state, TileType.Block, newPos);
        if (pushable != null)
        {
            pushable.MovePosition(moveDirection);
            AttemptToMove(this, newPos);
            return true;
        }
        return false;
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
    
    private void OnEnable()
    {
        isLerping = false;
        transform.position = new Vector3(gridPosition.x, gridPosition.y, 0);
    }
}

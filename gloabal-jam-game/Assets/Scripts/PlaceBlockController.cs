using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceBlockController : MonoBehaviour
{
    private PlayerController player;
    private GridComponent gridComponent;
    public SpriteRenderer placementDisplay;

    public List<GameObject> blocks;
    private List<bool> blockEquipped = new List<bool>(); 

    public int selection = 1;

    
     
    //private GameObject[] blocks = {block1,block2,block3,block4,block5}; 
    //attempting to combine both 
    //private Tuple<GameObject,bool>[] blockPair = {Tuple.Create(block1, true)};
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < blocks.Count; i++)
        {
            blockEquipped.Add(false);
        }
        blockEquipped[0] = true;

        player = GetComponent<PlayerController>();
        gridComponent = GetComponent<GridComponent>();
        placementDisplay.sprite = blocks[0].GetComponent<SpriteRenderer>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        changeBlock();
        PlaceBlock();
        UpdateDisplay();
    }

    void changeBlock()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ChangeSelection();
        }

        if (Input.GetKeyUp(KeyCode.Alpha1)) {
            ChangeSelection(1);
        } else if (Input.GetKeyUp(KeyCode.Alpha2)){
            ChangeSelection(2);
        } else if (Input.GetKeyUp(KeyCode.Alpha3)){
            ChangeSelection(3);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha4)){
            ChangeSelection(4);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha5)){
            ChangeSelection(5);
        } else if (Input.GetKeyUp(KeyCode.Alpha6)){
            ChangeSelection(6);
        } else if (Input.GetKeyUp(KeyCode.Alpha7)){
            ChangeSelection(7);
        } 
    }
    void ChangeSelection(int num)
    {
        //Debug.Log(">"+SettingsManager.placeableBlockAvailable.Contains(blocks[selection - 1].GetComponent<GridComponent>().tileType));
        if (num - 1 >= blocks.Count) return;
        if (SettingsManager.placeableBlockAvailable.Contains(blocks[num - 1].GetComponent<GridComponent>().tileType))
        {
            CanvasReference.Instance.selector.localPosition = new Vector3(25, 200 - 50 * num, 0);
            selection = num;
            enableBlock(num);
            placementDisplay.sprite = blocks[num - 1].GetComponent<SpriteRenderer>().sprite;
        }
        
    }

    void ChangeSelection()
    {
        selection++;
        if (selection - 1 >= blocks.Count) { selection = 1;}
        //Debug.Log(">" + SettingsManager.placeableBlockAvailable.Contains(blocks[selection - 1].GetComponent<GridComponent>().tileType));
        if (SettingsManager.placeableBlockAvailable.Contains(blocks[selection - 1].GetComponent<GridComponent>().tileType))
        {
            if (selection > 7)
            {
                selection = 1;
            }
            CanvasReference.Instance.selector.localPosition = new Vector3(25, 200 - 50 * selection, 0);
            enableBlock(selection);
            placementDisplay.sprite = blocks[selection - 1].GetComponent<SpriteRenderer>().sprite;
        }
        
    }

    void PlaceBlock() {
        if(gridComponent.isLerping == false)
        {
            if (Input.GetKeyUp(KeyCode.E) || Input.GetMouseButtonDown(0))
            {
                for (int i = 0; i < blockEquipped.Count; i++)
                {
                    if (blockEquipped[i] == true)
                    {
                        Vector3 position = SetPosition(blocks[i]);
                        GameObject newBlock = Instantiate(blocks[i], position, Quaternion.identity, FindObjectOfType<GameManager>().currentLevel.createdParent.transform); // need to include position
                        newBlock.transform.position = position;
                        
                    }
                }
                //Instantiate(Object.Wall); // need to palce block in current position 
            }


        } 
        
    }

    public void UpdateDisplay()
    {
        Facing direction = player.facing;
        switch (direction)
        {
            case Facing.Left:
                placementDisplay.transform.localPosition = new Vector3(-1,0,0);
                break;
            case Facing.Right:
                placementDisplay.transform.localPosition = new Vector3(1, 0, 0);
                break;
            case Facing.Down:
                placementDisplay.transform.localPosition = new Vector3(0, -1, 0);
                break;
            case Facing.Up:
                placementDisplay.transform.localPosition = new Vector3(0, 1, 0);
                break;
        }
    }

    public Vector3 SetPosition(GameObject block)
    {
        Vector2Int gridPos = checkPosition() + gridComponent.gridPosition;
        //Debug.Log(gridPos);
        block.transform.localPosition = GridManager.convertToVector3(gridPos);
        block.GetComponent<GridComponent>().gridPosition = gridPos;
        return GridManager.convertToVector3(gridPos);
    }

    List<bool> enableBlock(int input )
    { //goes and unequips all blocks and equips inputted block
        for(int i =0; i< blockEquipped.Count; i++){
            if (i == input-1 ){
                blockEquipped[i]= true;
            } else {
                blockEquipped[i]= false;
            }
        }
        return blockEquipped;
    }


    Vector2Int checkPosition() {
        Vector2Int placement = new Vector2Int (0,0);
        if(player.facing == Facing.Up){

            placement = new Vector2Int(0,1);

        } else if (player.facing == Facing.Down){

            placement = new Vector2Int(0,-1);

        } else if (player.facing == Facing.Left) {

            placement =  new Vector2Int(-1,0);

        } else if (player.facing == Facing.Right) {

            placement = new Vector2Int(1,0);
            
        } 
        return placement;
    }

    public void AddPlaceableItem(Collectable collectable)
    {
        blocks.Add(collectable.prefabActive);
        blockEquipped.Add(false);

        GameObject display = Instantiate(CanvasReference.Instance.uiDisplayTemplate, CanvasReference.Instance.selector.transform.parent);
        display.GetComponent<Image>().sprite = collectable.uiDisplay;
        display.GetComponent<RectTransform>().localPosition = new Vector3(25, 200 - 50 * blocks.Count, 0);
        display.transform.SetSiblingIndex(0);

        SettingsManager.placeableBlockAvailable.Add(collectable.prefabActive.GetComponent<GridComponent>().tileType);
        

    }
    
    


}

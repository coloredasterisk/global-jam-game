using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBlockController : MonoBehaviour
{
    private PlayerController player;
    private GridComponent gridComponent;

    public RectTransform selector;
    

    public GameObject block1,block2,block3,block4,block5,block6,block7;
    public List<GameObject> blocks;
    bool[] blockEquip ={false,false,false,false,false,false,false};

    
     
    //private GameObject[] blocks = {block1,block2,block3,block4,block5}; 
    //attempting to combine both 
    //private Tuple<GameObject,bool>[] blockPair = {Tuple.Create(block1, true)};
    // Start is called before the first frame update
    void Start()
    {
        
        blockEquip[0] = true;
        player = GetComponent<PlayerController>();
        gridComponent = GetComponent<GridComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        changeBlock();
        PlaceBlock();
    }

    void changeBlock()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1)) {
            selector.localPosition = new Vector3(25, 150, 0);
            enableBlock(1);
        } else if (Input.GetKeyUp(KeyCode.Alpha2)){
            selector.localPosition = new Vector3(25, 100, 0);
            enableBlock(2);
        } else if (Input.GetKeyUp(KeyCode.Alpha3)){
            selector.localPosition = new Vector3(25, 50, 0);
            enableBlock(3);
        }else if (Input.GetKeyUp(KeyCode.Alpha4)){
            selector.localPosition = new Vector3(25, 0, 0);
            Debug.Log("turned on 4");
            enableBlock(4);
        }else if (Input.GetKeyUp(KeyCode.Alpha5)){
            selector.localPosition = new Vector3(25, -50, 0);
            Debug.Log("turned on 5");
            enableBlock(5);
        } else if (Input.GetKeyUp(KeyCode.Alpha6)){
            selector.localPosition = new Vector3(25, -100, 0);
            Debug.Log("turned on 6");
            enableBlock(6);
        } else if (Input.GetKeyUp(KeyCode.Alpha7)){
            selector.localPosition = new Vector3(25, -150, 0);
            Debug.Log("turned on 7");
            enableBlock(7);
        } 
    }

    void PlaceBlock() {
        if(gridComponent.isLerping == false)
        {
            if (Input.GetKeyUp(KeyCode.B))
            {
                for (int i = 0; i < blockEquip.Length; i++)
                {
                    if (blockEquip[i] == true)
                    {
                        if (i == 0)
                        {
                            Vector3 position = SetPosition(block1);
                            GameObject newBlock = Instantiate(block1, position, Quaternion.identity, FindObjectOfType<GameManager>().currentLevel.createdParent.transform); // need to include position
                            newBlock.transform.position = position;
                        }
                        else if (i == 1)
                        {
                            Vector3 position = SetPosition(block2);
                            GameObject newBlock = Instantiate(block2, position, Quaternion.identity, FindObjectOfType<GameManager>().currentLevel.createdParent.transform); // need to include position
                            newBlock.transform.position = position;
                        }
                        else if (i == 2)
                        {
                            Vector3 position = SetPosition(block3);
                            GameObject newBlock = Instantiate(block3, position, Quaternion.identity, FindObjectOfType<GameManager>().currentLevel.createdParent.transform); // need to include position
                            newBlock.transform.position = position;
                        }
                        else if (i == 3)
                        {
                            Vector3 position = SetPosition(block4);
                            GameObject newBlock = Instantiate(block4, position, Quaternion.identity, FindObjectOfType<GameManager>().currentLevel.createdParent.transform); // need to include position
                            newBlock.transform.position = position;
                        }
                        else if (i == 4)
                        {
                            Vector3 position = SetPosition(block5);
                            GameObject newBlock = Instantiate(block5, position, Quaternion.identity, FindObjectOfType<GameManager>().currentLevel.createdParent.transform); // need to include position
                            newBlock.transform.position = position;

                        }
                        else if (i == 5)
                        {
                            Vector3 position = SetPosition(block6);
                            GameObject newBlock = Instantiate(block6, position, Quaternion.identity, FindObjectOfType<GameManager>().currentLevel.createdParent.transform); // need to include position
                            newBlock.transform.position = position;

                        }
                        else if (i == 6)
                        {
                            Vector3 position = SetPosition(block7);
                            GameObject newBlock = Instantiate(block7, position, Quaternion.identity, FindObjectOfType<GameManager>().currentLevel.createdParent.transform); // need to include position
                            newBlock.transform.position = position;

                        }
                    }
                }
                //Instantiate(Object.Wall); // need to palce block in current position 
            }


        } 
        
    }

    public Vector3 SetPosition(GameObject block)
    {
        Vector2Int gridPos = checkPosition() + gridComponent.gridPosition;
        Debug.Log(gridPos);
        block.transform.localPosition = GridManager.convertToVector3(gridPos);
        block.GetComponent<GridComponent>().gridPosition = gridPos;
        return GridManager.convertToVector3(gridPos);
    }

    bool[] enableBlock(int input )
    { //goes and unequips all blocks and equips inputted block
        for(int i =0; i<blockEquip.Length; i++){
            if (i == input-1 ){
                blockEquip[i]= true;
            } else {
                blockEquip[i]= false;
            }
        }
        return blockEquip ;
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
    
    


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBlockController : MonoBehaviour
{
    private PlayerController player;
    private GridComponent gridComponent;
    

    public GameObject block1,block2,block3,block4,block5;
    public List<GameObject> blocks2 ;
    bool[] blockEquip ={false,false,false,false,false};

    
     
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
        if (Input.GetKeyUp(KeyCode.Keypad1)) {
            enableBlock(1);
        } else if (Input.GetKeyUp(KeyCode.Keypad2)){
            enableBlock(2);
        } else if (Input.GetKeyUp(KeyCode.Keypad3)){
            enableBlock(3);
        }else if (Input.GetKeyUp(KeyCode.Keypad4)){
            enableBlock(4);
        }else if (Input.GetKeyUp(KeyCode.Keypad5)){
            enableBlock(5);
        }
    }

    void PlaceBlock() {
        if (Input.GetKeyUp(KeyCode.B)){
            for(int i =0; i<blockEquip.Length; i++) {
                if (blockEquip[i] == true){
                    if (i == 0){
                        block1.transform.position = checkPosition();
                        GameObject newBlock = Instantiate(block1,FindObjectOfType<GameManager>().currentLevel.createdParent.transform); // need to include position
                        //gridComponent.gridPosition =
                    } else if (i==1) {
                        GameObject newBlock = Instantiate(block2,FindObjectOfType<GameManager>().currentLevel.createdParent.transform); // need to include position
                        newBlock.transform.position = checkPosition();
                    } else if (i==2) {
                        GameObject newBlock = Instantiate(block3,FindObjectOfType<GameManager>().currentLevel.createdParent.transform); // need to include position
                        newBlock.transform.position = checkPosition();
                    }else if (i==3) {
                        GameObject newBlock = Instantiate(block4,FindObjectOfType<GameManager>().currentLevel.createdParent.transform); // need to include position
                        newBlock.transform.position = checkPosition();
                    }else if (i==4) {
                        GameObject newBlock = Instantiate(block5,FindObjectOfType<GameManager>().currentLevel.createdParent.transform); // need to include position
                        newBlock.transform.position = checkPosition();
                    }
                }
            }
           //Instantiate(Object.Wall); // need to palce block in current position 

        } 
        
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

    Vector3 checkPosition() {
        Vector3 current = transform.position;
        Vector3 placement = new Vector3 (0,0,0);
        if(player.facing == Facing.Up){

            placement = current + new Vector3 (0,1,0);

        } else if (player.facing == Facing.Down){

            placement = current + new Vector3 (0,-1,0);

        } else if (player.facing == Facing.Left) {

            placement = current + new Vector3 (-1,0,0);

        } else if (player.facing == Facing.Right) {

            placement = current + new Vector3 (1,0,0);
            
        } 
        return placement;
    }
    
    


}

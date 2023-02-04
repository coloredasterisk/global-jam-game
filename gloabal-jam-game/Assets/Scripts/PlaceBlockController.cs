using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBlockController : MonoBehaviour
{
    private PlayerController player;

    public static GameObject block1,block2,block3,block4,block5;
    public List<System.Tuple<GameObject,bool>> blockEquip = new List<System.Tuple<GameObject,bool>>()
    {
        new System.Tuple<GameObject, bool> (block1,false),
        new System.Tuple<GameObject, bool> (block2,false),
        new System.Tuple<GameObject, bool> (block3,false),
        new System.Tuple<GameObject, bool> (block4,false),
        new System.Tuple<GameObject, bool> (block5,false)
    };
    
    //private GameObject[] blocks = {block1,block2,block3,block4,block5}; 
    //attempting to combine both 
    //private Tuple<GameObject,bool>[] blockPair = {Tuple.Create(block1, true)};
    // Start is called before the first frame update
    void Start()
    {
        blockEquip[0]= new System.Tuple<GameObject, bool> (blockEquip[0].Item1,true);
        player = GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        changeBlock();
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
           //Instantiate(Object.Wall); // need to palce block in current position 

        } 
        
    }

    List<System.Tuple<GameObject,bool>> enableBlock(int input )
    { //goes and unequips all blocks and equips inputted block
        for(int i =0; i<blockEquip.Count; i++){
            if (i == input-1 ){
                blockEquip[i]= new System.Tuple<GameObject, bool> (blockEquip[i].Item1,true); //index is input so equip block
            } else {
                blockEquip[i]=new System.Tuple<GameObject, bool> (blockEquip[i].Item1,false);
            }
        }
        return blockEquip ;
    }


}

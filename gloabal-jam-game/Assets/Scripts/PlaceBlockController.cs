using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBlockController : MonoBehaviour
{
    private PlayerController player;
    

    public GameObject block1,block2,block3,block4,block5;
    
    bool[] blockEquip ={false,false,false,false,false};

    
     
    //private GameObject[] blocks = {block1,block2,block3,block4,block5}; 
    //attempting to combine both 
    //private Tuple<GameObject,bool>[] blockPair = {Tuple.Create(block1, true)};
    // Start is called before the first frame update
    void Start()
    {
        
        blockEquip[0] = true;
        player = GetComponent<PlayerController>();
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
                        Instantiate(block1); // need to include position
                    } else if (i==1) {
                        Instantiate(block2);
                    } else if (i==2) {
                        Instantiate(block3);
                    }else if (i==3) {
                        Instantiate(block4);
                    }else if (i==4) {
                        Instantiate(block5);
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


}

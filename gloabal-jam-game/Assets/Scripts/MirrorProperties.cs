using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorProperties : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void checkHitStatus(TileType mirror) {
        if (mirror == TileType.Mirror45 ) {
            //check leftup and right down 
        } else if (mirror == TileType.Mirror135) {
            //check left down and right up
        }

    }

}

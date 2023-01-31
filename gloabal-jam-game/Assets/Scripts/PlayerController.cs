using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRB;
    private GridComponent gridComponent;
    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        gridComponent = GetComponent<GridComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gridComponent.isLerping)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                gridComponent.MovePosition(0, 1);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                gridComponent.MovePosition(0, -1);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                gridComponent.MovePosition(-1, 0);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                gridComponent.MovePosition(1, 0);
            }
        }
        
    }


}

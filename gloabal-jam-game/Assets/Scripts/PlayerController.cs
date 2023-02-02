using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRB;
    private GridComponent gridComponent;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        gridComponent = GetComponent<GridComponent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gridComponent.isLerping)
        {
            if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
            {
                animator.SetFloat("MoveY", -1);
                gridComponent.MovePosition(0, 1);
            }
            else if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
            {
                animator.SetFloat("MoveY", 1);
                gridComponent.MovePosition(0, -1);
            }
            else
            {
                animator.SetFloat("MoveY", 0);
            }
            
            if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                gridComponent.MovePosition(-1, 0);
            }
            else if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            {
                gridComponent.MovePosition(1, 0);
            }
        }
        
    }


}

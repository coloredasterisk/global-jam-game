using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Facing
{
    Left,
    Right,
    Up,
    Down,
}
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRB;
    private GridComponent gridComponent;
    private Animator animator;

    private float holdThreshold = 0.2f;
    public float holdTimer = 0f;
    public Facing facing = Facing.Down;
    public bool extraControls = true;
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
        if(holdTimer < holdThreshold)
        {
            if (Input.anyKey)
            {
                holdTimer += Time.deltaTime;
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            GameObject gm = Instantiate(this.gameObject);
            gm.transform.parent = null;
        }

        FaceOnPress();
        IdleWhenReleased();

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            holdTimer = 0;
        }
        if (!gridComponent.isLerping && (holdTimer > holdThreshold || extraControls))
        {
            if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
            {
                animator.SetFloat("MoveY", 1);
                gridComponent.MovePosition(0, 1);
            }
            else if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
            {
                animator.SetFloat("MoveY", -1);
                gridComponent.MovePosition(0, -1);
            }
            else if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                animator.SetFloat("MoveX", -1);
                gridComponent.MovePosition(-1, 0);
            }
            else if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            {
                animator.SetFloat("MoveX", 1);
                gridComponent.MovePosition(1, 0);
            } 
            
        }
        
    }

    void FaceOnPress()
    {
        if (Input.GetKeyDown(KeyCode.W) || (extraControls && Input.GetKeyDown(KeyCode.UpArrow)))
        {
            facing = Facing.Up;
            animator.SetInteger("Facing", 2);
        }
        else if (Input.GetKeyDown(KeyCode.S) || (extraControls && Input.GetKeyDown(KeyCode.DownArrow)))
        {
            facing = Facing.Down;
            animator.SetInteger("Facing", 0);
        }
        else if (Input.GetKeyDown(KeyCode.A) || (extraControls && Input.GetKeyDown(KeyCode.LeftArrow)))
        {
            facing = Facing.Left;
            animator.SetInteger("Facing", 1);
        }
        else if (Input.GetKeyDown(KeyCode.D) || (extraControls && Input.GetKeyDown(KeyCode.RightArrow)))
        {
            facing = Facing.Right;
            animator.SetInteger("Facing", 3);
        }
    }
    void IdleWhenReleased()
    {
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            animator.SetFloat("MoveY", 0);
        }
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            animator.SetFloat("MoveX", 0);
        }
    }


}

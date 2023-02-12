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
    public Animator animator;

    public AudioClip[] sounds;
    public AudioSource audioSource;

    public Facing facing = Facing.Down;
    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        gridComponent = GetComponent<GridComponent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!gridComponent.isLerping)
        {
            if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || 
                Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow)) //face only
            {
                //Debug.Log("Test");
                FaceOnPress();
            }
            else //move player
            {
                if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
                {
                    animator.SetFloat("MoveY", 0);
                }
                else if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
                {
                    animator.SetFloat("MoveY", 1);
                    facing = Facing.Up;
                    bool moved = gridComponent.MovePosition(0, 1, MovementStatus.Normal);
                    PlayStepSound(moved);
                }
                else if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
                {
                    animator.SetFloat("MoveY", -1);
                    facing = Facing.Down;
                    bool moved = gridComponent.MovePosition(0, -1, MovementStatus.Normal);
                    PlayStepSound(moved);
                }

                else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
                {
                    animator.SetFloat("MoveX", 0);
                }
                else if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
                {
                    animator.SetFloat("MoveX", -1);
                    facing = Facing.Left;
                    bool moved = gridComponent.MovePosition(-1, 0, MovementStatus.Normal);
                    PlayStepSound(moved);
                }
                else if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
                {
                    animator.SetFloat("MoveX", 1);
                    facing = Facing.Right;
                    bool moved = gridComponent.MovePosition(1, 0, MovementStatus.Normal);
                    PlayStepSound(moved);
                }
            }
            
            
        }

        IdleWhenReleased();
    }
    private void PlayStepSound(bool moved)
    {
        if (moved)
        {
            int index = Random.Range(0, sounds.Length);
            audioSource.clip = sounds[index];
            audioSource.Play();
        }
        
        
    }

    void FaceOnPress()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            facing = Facing.Up;
            InteractionLog.NewFacingLog(this, facing);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            facing = Facing.Down;
            InteractionLog.NewFacingLog(this, facing);

        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            facing = Facing.Left;
            InteractionLog.NewFacingLog(this, facing);

        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            facing = Facing.Right;
            InteractionLog.NewFacingLog(this, facing);
        }

        DirectionToAnimation();
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
    public void DirectionToAnimation()
    {
        if(facing == Facing.Up)
        {
            animator.SetInteger("Facing", 2);
        } else if(facing == Facing.Down)
        {
            animator.SetInteger("Facing", 0);
        } else if(facing == Facing.Left)
        {
            animator.SetInteger("Facing", 1);
        } else if(facing == Facing.Right)
        {
            animator.SetInteger("Facing", 3);
        }
    }


}

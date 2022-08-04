using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Vector2 velocity;
    [SerializeField] private float jumpVelocity;
    [SerializeField] private float gravity;

    [SerializeField] private LayerMask wallMask;
    [SerializeField] private LayerMask floorMask;

    private bool grounded = false;
    private bool walk, walkLeft, walkRight, jump;

    public enum PlayerState
    {
        Jumping,
        Idle,
        Walking
    }

    private PlayerState playerState = PlayerState.Idle;

    private Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        Fall();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerInput();
        UpdatePlayerPosition();
        UpdateAnimationStates();
    }


    private void CheckPlayerInput()
    {
        bool inputLeft = Input.GetKey(KeyCode.LeftArrow);
        bool inputRight = Input.GetKey(KeyCode.RightArrow);
        bool inputSpace = Input.GetKey(KeyCode.Space);

        walk = inputLeft || inputRight;
        walkLeft = inputLeft && !inputRight;
        walkRight = inputRight && !inputLeft;
        jump = inputSpace;
    }

    private void UpdatePlayerPosition()
    {
        Vector3 pos = transform.localPosition;
        Vector3 scale = transform.localScale;


        if (walk)
        {
            if (walkLeft)
            {
                pos.x -= velocity.x * Time.deltaTime;
                scale.x = -1;
            }
            if (walkRight)
            {
                pos.x += velocity.x * Time.deltaTime;
                scale.x = 1;
            }
            pos = CheckWallRays(pos, scale.x);
        }
        if (jump && playerState != PlayerState.Jumping)
        {
            playerState = PlayerState.Jumping;
            velocity = new Vector2(velocity.x, jumpVelocity);
        }
        if(playerState == PlayerState.Jumping)
        {
            pos.y += velocity.y * Time.deltaTime;
            velocity.y -= gravity * Time.deltaTime;
        }
        if (velocity.y <= 0)
            pos = CheckGroundRays(pos);

        if (velocity.y >= 0)
            pos = CheckCeilingRays(pos);

        transform.localPosition = pos;
        transform.localScale = scale;
    }

    private Vector3 CheckWallRays(Vector3 pos, float dir)
    {
        Vector2 originTop = new Vector2(pos.x + dir * 0.4f, pos.y + 1f - 0.2f);
        Vector2 originMiddle = new Vector2(pos.x + dir * 0.4f, pos.y);
        Vector2 originBottom = new Vector2(pos.x + dir * 0.4f, pos.y - 1f + 0.2f);

        RaycastHit2D wallTop = Physics2D.Raycast(originTop, new Vector2(dir, 0), velocity.x * Time.deltaTime, wallMask);
        RaycastHit2D wallMiddle = Physics2D.Raycast(originMiddle, new Vector2(dir, 0), velocity.x * Time.deltaTime, wallMask);
        RaycastHit2D wallBottom = Physics2D.Raycast(originBottom, new Vector2(dir, 0), velocity.x * Time.deltaTime, wallMask);

        if(wallTop.collider != null || wallMiddle.collider != null || wallBottom.collider != null)
        {
            pos.x -= velocity.x * Time.deltaTime * dir;
        }

        return pos;
    }

    private Vector3 CheckGroundRays(Vector3 pos)
    {
        Vector2 originLeft = new Vector2(pos.x - 0.5f + 0.2f, pos.y - 1f);
        Vector2 originRight = new Vector2(pos.x + 0.5f - 0.2f, pos.y - 1f);
        Vector2 originMiddle = new Vector2(pos.x, pos.y - 1f);

        RaycastHit2D floorLeft = Physics2D.Raycast(originLeft, Vector2.down, velocity.y * Time.deltaTime, floorMask);
        RaycastHit2D floorMiddle = Physics2D.Raycast(originMiddle, Vector2.down, velocity.y * Time.deltaTime, floorMask);
        RaycastHit2D floorRight = Physics2D.Raycast(originRight, Vector2.down, velocity.y * Time.deltaTime, floorMask);

        if(floorLeft.collider != null || floorMiddle.collider != null || floorRight.collider != null)
        {
            RaycastHit2D hitRay = floorRight;

            if (floorLeft)
            {
                hitRay = floorLeft;
            }
            else if (floorMiddle)
            {
                hitRay = floorMiddle;
            }
            else if (floorRight)
            {
                hitRay = floorRight;
            }

            playerState = PlayerState.Idle;
            grounded = true;
            velocity.y = 0;

            pos.y = hitRay.collider.bounds.center.y + (hitRay.collider.bounds.size.y / 2) + 1;
        }
        else
        {
            if(playerState != PlayerState.Jumping)
            {
                Fall();
            }
        }
        return pos;
    }

    private Vector3 CheckCeilingRays(Vector3 pos)
    {
        Vector2 originLeft = new Vector2(pos.x - 0.5f + 0.2f, pos.y + 1f);
        Vector2 originRight = new Vector2(pos.x + 0.5f - 0.2f, pos.y + 1f);
        Vector2 originMiddle = new Vector2(pos.x, pos.y + 1f);

        RaycastHit2D ceilingLeft = Physics2D.Raycast(originLeft, Vector2.up, velocity.y * Time.deltaTime, floorMask);
        RaycastHit2D ceilingMiddle = Physics2D.Raycast(originMiddle, Vector2.up, velocity.y * Time.deltaTime, floorMask);
        RaycastHit2D ceilingRight = Physics2D.Raycast(originRight, Vector2.up, velocity.y * Time.deltaTime, floorMask);

        if (ceilingLeft.collider != null || ceilingMiddle.collider != null || ceilingRight.collider != null)
        {
            RaycastHit2D hitRay = ceilingLeft;

            if (ceilingLeft)
            {
                hitRay = ceilingLeft;
            }
            else if (ceilingMiddle)
            {
                hitRay = ceilingMiddle;
            }
            else if (ceilingRight)
            {
                hitRay = ceilingRight;
            }

            pos.y = hitRay.collider.bounds.center.y - hitRay.collider.bounds.size.y / 2 - 1;

            Fall();
        }

        return pos;
    }

    private void Fall()
    {
        velocity.y = 0;
        playerState = PlayerState.Jumping;
        grounded = false;
    }

    private void UpdateAnimationStates()
    {
        if (grounded && !walk)
        {
            anim.SetBool("isJumping", false);
            anim.SetBool("isRunning", false);
            //anim.SetBool("isIdle", true);
        }
        if(grounded && walk)
        {
            anim.SetBool("isJumping", false);
            anim.SetBool("isRunning", true);
            //anim.SetBool("isIdle", false);
        }
        if(playerState == PlayerState.Jumping)
        {
            anim.SetBool("isJumping", true);
            anim.SetBool("isRunning", false);
            //anim.SetBool("isIdle", false);
        }
    }


}

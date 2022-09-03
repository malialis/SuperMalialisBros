using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    private Rigidbody2D _rigidBody;
    private Vector2 velocity;

    public float speed = 1f;
    public Vector2 direction = Vector2.left;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        enabled = false;
    }

    private void OnBecameVisible()
    {
        enabled = true;
    }

    private void OnBecameInvisible()
    {
        enabled = false;
    }

    private void OnEnable()
    {
        _rigidBody.WakeUp();
    }

    private void OnDisable()
    {
        _rigidBody.velocity = Vector2.zero;
        _rigidBody.Sleep();
    }


    private void FixedUpdate()
    {
        velocity.x = direction.x * speed;
        velocity.y += Physics2D.gravity.y * Time.fixedDeltaTime;

        _rigidBody.MovePosition(_rigidBody.position + velocity * Time.fixedDeltaTime);

        if (_rigidBody.Raycast(direction))
        {
            direction = -direction;
        }

        if (_rigidBody.Raycast(Vector2.down))
        {
            velocity.y = Mathf.Max(velocity.y, 0f);
        }
    }


}

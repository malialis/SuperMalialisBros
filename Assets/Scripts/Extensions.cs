using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions 
{
    private static LayerMask layerMask = LayerMask.GetMask("Ground");

    public static bool Raycast(this Rigidbody2D _rigidBody, Vector2 direction)
    {
        if (_rigidBody.isKinematic)
        {
            return false;
        }

        float radious = 0.25f;
        float distance = 0.375f;

        RaycastHit2D hit = Physics2D.CircleCast(_rigidBody.position, radious, direction.normalized, distance, layerMask);

        return hit.collider != null && hit.rigidbody != _rigidBody;
    }

    public static bool DotTest(this Transform transform, Transform otherTransform, Vector2 testDirection)
    {
        Vector2 direction = otherTransform.position - transform.position;
        return Vector2.Dot(direction.normalized, testDirection) > 0.25f;

        //this tests to see if you are going up, sideways or falling and what you are hitting
    }

}

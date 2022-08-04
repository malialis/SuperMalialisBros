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

        RaycastHit2D hit = Physics2D.CircleCast(_rigidBody.position, radious, direction, distance, layerMask);

        return hit.collider != null && hit.rigidbody != _rigidBody;
    }


}

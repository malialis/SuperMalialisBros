using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerSpriteRenderer smallMarioRenderer;
    public PlayerSpriteRenderer bigMarioRenderer;

    public DeathAnimation deathAnimation;

    public bool bigMario => bigMarioRenderer.enabled;
    public bool smallMario => smallMarioRenderer.enabled;

    public bool dead => deathAnimation.enabled;

    private void Awake()
    {
        deathAnimation = GetComponent<DeathAnimation>();
    }

    public void Hit()
    {
        if (bigMario)
        {
            Shrink();
            Debug.Log("I am big, I shrink");
        }
        else
        {
            Death();
            Debug.Log("I am small I die now");
        }
    }

    private void Shrink()
    {
        //add code soon
    }

    private void Death()
    {
        smallMarioRenderer.enabled = false;
        bigMarioRenderer.enabled = false;
        deathAnimation.enabled = true;

        Debug.Log("Time for the Death Animation yo");

        GameManager.Instance.ResetLevel(4f);
    }


}

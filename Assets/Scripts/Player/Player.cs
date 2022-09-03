using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerSpriteRenderer smallMarioRenderer;
    public PlayerSpriteRenderer bigMarioRenderer;
    private PlayerSpriteRenderer activeRenderer;

    public DeathAnimation deathAnimation;

    private CapsuleCollider2D capsul2D;

    public bool bigMario => bigMarioRenderer.enabled;
    public bool smallMario => smallMarioRenderer.enabled;

    public bool dead => deathAnimation.enabled;

    private void Awake()
    {
        deathAnimation = GetComponent<DeathAnimation>();
        capsul2D = GetComponent<CapsuleCollider2D>();
        activeRenderer = smallMarioRenderer;
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

    public void Grow()
    {
        smallMarioRenderer.enabled = false;
        bigMarioRenderer.enabled = true;
        activeRenderer = bigMarioRenderer;

        capsul2D.size = new Vector2(1f, 2f);
        capsul2D.offset = new Vector2(0f, 0.5f);

        StartCoroutine(ScaleAnimation());
    }

    private void Shrink()
    {
        smallMarioRenderer.enabled = true;
        bigMarioRenderer.enabled = false;
        activeRenderer = smallMarioRenderer;

        capsul2D.size = new Vector2(1f, 1f);
        capsul2D.offset = new Vector2(0f, 0f);

        StartCoroutine(ScaleAnimation());
    }

    private void Death()
    {
        smallMarioRenderer.enabled = false;
        bigMarioRenderer.enabled = false;
        deathAnimation.enabled = true;

        Debug.Log("Time for the Death Animation yo");

        GameManager.Instance.ResetLevel(4f);
    }

    private IEnumerator ScaleAnimation()
    {
        float elapsed = 0f;
        float duration = 1.5f;

        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if(Time.frameCount % 4 == 0)
            {
                smallMarioRenderer.enabled = !smallMarioRenderer.enabled;
                bigMarioRenderer.enabled = !smallMarioRenderer;
            }

            yield return null;
        }

        smallMarioRenderer.enabled = false;
        bigMarioRenderer.enabled = false;
        activeRenderer.enabled = true;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHit : MonoBehaviour
{
    public int maxHits = -1;
    public Sprite emptyBlockSprite;
    public GameObject item;
    private bool isAnimating;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isAnimating && maxHits != 0 && collision.gameObject.CompareTag("Player"))
        {
            if(collision.transform.DotTest(transform, Vector2.up))
            {
                Hit();
            }
        }
    }


    private void Hit()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;

        maxHits--;

        if(maxHits == 0)
        {
            spriteRenderer.sprite = emptyBlockSprite;
        }

        if(item != null)
        {
            Instantiate(item, transform.position, Quaternion.identity);
        }

        StartCoroutine(AnimateHit());
    }

    private IEnumerator AnimateHit()
    {
        isAnimating = true;

        Vector3 restingPosition = transform.localPosition;
        Vector3 animatedPosition = restingPosition + Vector3.up * 0.5f;

        yield return BlockMove(restingPosition, animatedPosition);
        yield return BlockMove(animatedPosition, restingPosition);

        isAnimating = false;
    }

    private IEnumerator BlockMove(Vector3 from, Vector3 to)
    {
        float elapsedTime = 0f;
        float duration = 0.125f;

        while(elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            transform.localPosition = Vector3.Lerp(from, to, t);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = to;
    }

}

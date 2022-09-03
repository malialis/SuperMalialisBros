using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCoin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.AddCoin();
        StartCoroutine(AnimatePop());
    }


    private IEnumerator AnimatePop()
    {
        
        Vector3 restingPosition = transform.localPosition;
        Vector3 animatedPosition = restingPosition + Vector3.up * 2f;

        yield return CoinMove(restingPosition, animatedPosition);
        yield return CoinMove(animatedPosition, restingPosition);

        Destroy(gameObject);
    }

    private IEnumerator CoinMove(Vector3 from, Vector3 to)
    {
        float elapsedTime = 0f;
        float duration = 0.35f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            transform.localPosition = Vector3.Lerp(from, to, t);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = to;
    }

}

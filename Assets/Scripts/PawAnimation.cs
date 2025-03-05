using System.Collections;
using UnityEngine;

public class PawAnimation : MonoBehaviour
{
    [SerializeField]
    private float delay;
    [SerializeField]
    private float x;
    [SerializeField]
    private float y;

    void Start()
    {
        StartCoroutine(DelayedAnimation(delay));
    }

    IEnumerator DelayedAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);
        transform.LeanMoveLocal(new Vector2(x, y), 1f).setEaseOutQuart();
    }
}
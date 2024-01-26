using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawAnimation : MonoBehaviour
{
    [SerializeField]
    private float delay;
    [SerializeField]
    private float x;
    [SerializeField]
    private float y;

    private float timeElapsed;

    void Update(){
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= delay){
            FallingDown(x,y);
        }
    } 

    public void FallingDown(float x, float y){
        LeanTween.init(800);
        transform.LeanMoveLocal(new Vector2(x,y),1).setEaseOutQuart();
    }
}

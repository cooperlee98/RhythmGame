using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundBehavior : MonoBehaviour
{
    public GameObject gameBackground;
    private SpriteRenderer gameBackgroundSpriteRenderer;

    void Start()
    {
        gameBackgroundSpriteRenderer = gameBackground.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeOut(gameBackgroundSpriteRenderer, 0.005f));
    }


    IEnumerator FadeOut(SpriteRenderer spriteRenderer, float amount)
    {
        Color color = spriteRenderer.color;
        while (color.a > 0.0f)
        {
            color.a -= amount;//0.005초 마다 색깔이 빼기 연산이 되면서 fade out 됨
            spriteRenderer.color = color;
            yield return new WaitForSeconds(amount);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

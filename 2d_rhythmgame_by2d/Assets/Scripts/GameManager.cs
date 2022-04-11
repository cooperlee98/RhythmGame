﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; set; }//게임매니저를 싱글 톤으로 처리함 

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    public float noteSpeed;

    /*
     * bad:1
     * good:2
     * perfect:3
     * miss:4
     */

    public GameObject scoreUI;
    private float score;
    private Text scoreText;

    public GameObject comboUI;
    private int combo;
    private Text comboText;
    private Animator comboAnimator;


    public enum judges { NONE=0, BAD,GOOD, PERFECT, MISS};
    public GameObject judgeUI;
    private Sprite[] judgeSprites;
    private Image judgementSpriteRenderer;
    private Animator judgementSpriteAnimator;


    public GameObject[] trails;
    private SpriteRenderer[] trailSpriteRenderers;


    void Start()
    {
        judgementSpriteRenderer = judgeUI.GetComponent<Image>();
        judgementSpriteAnimator = judgeUI.GetComponent<Animator>();
        scoreText = scoreUI.GetComponent<Text>();
        comboText = comboUI.GetComponent<Text>();
        comboAnimator = comboUI.GetComponent<Animator>();

        //판정 결과를 보여주는 스프라이트 이미지를 미리 초기화
        judgeSprites = new Sprite[4];
        judgeSprites[0] = Resources.Load<Sprite>("Sprites/Bad");//위에 순서와 인덱스가 다름
        judgeSprites[1] = Resources.Load<Sprite>("Sprites/Good");
        judgeSprites[2] = Resources.Load<Sprite>("Sprites/Miss");
        judgeSprites[3] = Resources.Load<Sprite>("Sprites/Perfect");//이미지 형태와 상관없이 스프라이트로 가져와 사용할 수 있음

        trailSpriteRenderers = new SpriteRenderer[trails.Length];
        for(int i=0; i<trails.Length; i++)
        {
            trailSpriteRenderers[i] = trails[i].GetComponent<SpriteRenderer>();
        }
    }
    void Update()
    {
        //사용자가 입력한 키에 해당하는 라인을 빛나게 처리
        if (Input.GetKey(KeyCode.D)) ShineTrail(0);
        if(Input.GetKey(KeyCode.F)) ShineTrail(1);
        if(Input.GetKey(KeyCode.J)) ShineTrail(2);
        if(Input.GetKey(KeyCode.K)) ShineTrail(3);
        //한번 빛난 라인은 반복적으로 다시 어둡게 처리
        for(int i=0; i<trailSpriteRenderers.Length; i++)
        {
            Color color = trailSpriteRenderers[i].color;
            color.a -= 0.01f;
            trailSpriteRenderers[i].color = color;
        }
    }

    public void ShineTrail(int index)
    {
        Color color = trailSpriteRenderers[index].color;
        color.a = 0.32f;
        trailSpriteRenderers[index].color = color;
    }

    void showJudgement()//노트의 판정 이후 판정결과를 보여줌
    {
        string scoreFormat = "000000";
        scoreText.text = score.ToString(scoreFormat);
        judgementSpriteAnimator.SetTrigger("Show");//판정 이미지를 보여줌

        if (combo >= 2)//콤보가 2 이상일 때만 콤보 이미지를 보여줌
        {
            comboText.text = "COMBO" + combo.ToString();
            comboAnimator.SetTrigger("Show");
        }
    }

    public void processJudge(judges judge, int noteType)//노트 판정 진행
    {
        if (judge==judges.NONE) return;

        if (judge == judges.MISS)
        {
            judgementSpriteRenderer.sprite = judgeSprites[2];
            combo = 0;
            if (score >= 15) score-= 15;
        }
        else if (judge == judges.BAD)
        {
            judgementSpriteRenderer.sprite = judgeSprites[0];
            combo = 0;
            if (score >= 5) score -= 5;
        }


        else
        {
            if (judge == judges.PERFECT)
            {
                judgementSpriteRenderer.sprite = judgeSprites[3];
                score += 20;
            }
            else if (judge == judges.GOOD)
            {
                judgementSpriteRenderer.sprite = judgeSprites[1];
                score += 15;
            }
            combo += 1;
            score += (float)combo * 0.1f;
        }
        showJudgement();//여기다가 위 showJudgement 함수를 넣음!
    }
}

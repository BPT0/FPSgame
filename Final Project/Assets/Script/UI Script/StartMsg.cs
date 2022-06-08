using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class StartMsg : MonoBehaviour
{
    // 타이머 관련 변수
    private float time_current; // 창이 활성화까지 남은 시간
    private float time_Max = 3f; // 창의 활성화 시간
    private bool isEnabled; // 창 활성화 = false

    private int R_count=0;
    private int S_count=0;
    private int A_count=0;

    // 필요한 오브젝트
    [SerializeField] private GameObject start_Msg;
    [SerializeField] private Text R_txt;
    [SerializeField] private Text S_txt;
    [SerializeField] private Text A_txt;
    private PauseMenu thePauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        Reset_Timer();
        Set_Goal_Mine();
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnabled)
            return;
        Check_Timer();
        if (Input.GetKeyDown(KeyCode.G))
            Show_Result();
    }

    public void Show_Result()
    {
        isEnabled = !isEnabled;
        start_Msg.SetActive(!isEnabled);
        R_txt.text = "Rock R X " + R_count.ToString();
        S_txt.text = "Rock S X " + S_count.ToString();
        A_txt.text = "Rock A X " + A_count.ToString();
    }

    private void Set_Goal_Mine()
    {
        System.Random _randomObj = new System.Random();
        R_count = _randomObj.Next(5, 10);
        R_txt.text = "Rock X" + R_count.ToString();

        S_count = _randomObj.Next(5, 10);
        S_txt.text = "Sapaier X" + S_count.ToString();

        A_count = _randomObj.Next(5, 10);
        A_txt.text = "Amethyst X" + A_count.ToString();
    }

    private void Check_Timer()
    {
        if (0 < time_current)
            time_current -= Time.deltaTime;
        else if (!isEnabled)
            Close_Msg();
    }

    private void Close_Msg()
    {
        time_current = 0;
        start_Msg.SetActive(false);
        isEnabled = true;
    }

    private void Reset_Timer()
    {
        time_current = time_Max;
        isEnabled = false;
    }

    // 각 광석을 채굴 할 때마다 count 감소
    public void get_Rock()
    {
        if(R_count>0)
            R_count--;
    }

    public void get_Sapaier()
    {
        if(S_count>0)
            S_count--;
    }

    public void get_Amethyst()
    {
        if(A_count>0)
           A_count--;
    }

    // 모든 광석을 다 켔으면 종료
    public void Check_All_Mining()
    {
        if(R_count < 0 && S_count<0 && A_count < 0)
            thePauseMenu.ClickExit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    // 타이머 관련 변수
    private float time_current; // 창이 활성화까지 남은 시간
    private float time_Max = 30f; // 창의 활성화 시간
    private bool isEnabled= false; // 타이머 텍스트 활성화 = false

    // 필요한 컴포넌트
    [SerializeField]
    private Text Timer_txt;
    private PauseMenu thePauseMenu;
    private Result_Msg theResult_Msg;
    private StartMsg theStartMsg;

    void Start()
    {
        Reset_Timer();
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnabled)
            return;
        Check_Timer();
    }

    private void Check_Timer()
    {
        if(time_current == 5)
        {
            theStartMsg.Show_Result();
            // theResult_Msg.Show_Result();
        }
        if (0 < time_current)
        {
            time_current -= Time.deltaTime;

            Timer_txt.text = ((int)time_current / 60 % 60).ToString()
                +":"+ ((int)time_current % 60).ToString();
        }
        else if (!isEnabled)
        {
            End_Game();
        }
    }

    private void End_Game()
    {
        Debug.Log("End");
        time_current = 0;
        isEnabled = true;
        thePauseMenu.ClickExit();
    }

    private void Reset_Timer()
    {
        time_current = time_Max;
        isEnabled = false;
        Debug.Log("Start");
    }
}

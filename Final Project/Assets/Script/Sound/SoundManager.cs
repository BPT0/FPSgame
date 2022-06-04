using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 시리얼아이즈 필드를 클래스 전체에 적용
[System.Serializable]
public class Sound
{
    public string name; // 곡의 이름
    public AudioClip clip; // 곡
}

public class SoundManager : MonoBehaviour
{

    static public SoundManager instance;

    // 싱글턴화 적용 : 실행중 해당 스크립트는 1개만 유지
    #region singleton
    private void Awake() // 객체 생성시 최초 실행
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this.gameObject);
    }
    #endregion singleton

    public AudioSource[] audioSourceEffects;
    public AudioSource audioSourceBgm;

    public string[] playSoundName;

    public Sound[] effectSounds;
    public Sound[] bgmSound;
    public Sound a;

    void Start()
    {
        playSoundName = new string[audioSourceEffects.Length];    
    }

    public void PlaySE(string _name)
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if (_name == effectSounds[i].name)
            {
                for (int j = 0; j < audioSourceEffects.Length; j++)
                {
                    if (!audioSourceEffects[j].isPlaying)
                    {
                        playSoundName[j] = effectSounds[i].name;
                        audioSourceEffects[j].clip = effectSounds[i].clip;
                        audioSourceEffects[j].Play();
                        return;
                    }
                }
                Debug.Log("모든 가용 AudioSource가 사용중입니다");
                return;
            }
        }
        Debug.Log(_name + "사운드가 SoundManager 에 등록되지 않았습니다.");
    }

    public void StopAllSE()
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            audioSourceEffects[i].Stop();
        }
    }

    public void StopSE(string _name)
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            if(playSoundName[i] == name)
            {
                audioSourceEffects[i].Stop();
                break;
            }
        }
        Debug.Log("재생 중인" + _name + "사운드가 없습니다");
    }
}
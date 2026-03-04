using System;
using UnityEngine;

public class AchiveManager : MonoBehaviour
{
    // Achivement - 업적, 성취
    // 이 클래스는 업적을 관리하는 역할을 수행

    public GameObject[] lockCharacter;  // 해금이 안 된 상태의 버튼을 저장할 배열
    public GameObject[] unlockCharacter;  // 해금된 상태의 버튼을 저장할 배열

    // 업적의 종류를 열거형으로 저장
    enum Achivement
    {
        UnlockPotato,  // 감자 농부 캐릭터 해금
        UnlockBean,  // 콩 농부 캐릭터 해금
    }

    Achivement[] achivements;  // 열거형의 데이터들을 저장할 배열

    // 열거형 데이터들을 저장한 배열 초기화
    private void Awake()
    {
        // (Achivement[])은 Enum.GetValues는 반환형이 자료형 배열이 아닌 그냥 배열이라 명시적 형 변환을 해서 알맞는 자료형 배열로 만든 것이다.(typeof함수는 인자로 자료형 타입 그 자체를 받아 인자로 받은 타입을 그대로 반환하는 함수)
        achivements = (Achivement[])Enum.GetValues(typeof(Achivement));

        // 업적 초기화(게임 시작 할 때마다 초기화가 되면 그 동안 했던 업적이 없어지는 것이기 때문에 이미 초기화된 업적이 있다면 초기화하지 않도록 조건이 있어야 한다.)
        if (!PlayerPrefs.HasKey("MyData"))
        {
            AchiveInit();
        }
    }

    private void Start()
    {
        UnlockCharacter();
    }

    // 업적 데이터를 초기화 하는 함수
    void AchiveInit()
    {
        // PlayerPrefs - 간단한 저장 기능을 제공하는 유니티 클래스
        // 한 번 생성된 데이터는 각 컴퓨터에 맞게 저장되기 때문에 매번 초기화하지 않아도 된다.
        PlayerPrefs.SetInt("MyData", 1);  // 키-값 쌍을 만드는 함수 - 값의 자료형 int, string, float형에 맞는 함수를 사용하여 키-값을 저장할 수 있다.(Get을 통해 키에 맞는 값을 가지고 오는 것도 가능하다.)
        
        foreach(Achivement achivement in achivements)
        {
            PlayerPrefs.SetInt(achivement.ToString(), 0);  // 키는 반드시 문자열 데이터여야 하기 때문에 열거형 데이터를 문자열로 변환
        }
    }

    void UnlockCharacter()
    {
        // 
        for(int i = 0; i < lockCharacter.Length; i++)
        {
            string achivementName = achivements[i].ToString();  // 키 값에 사용할 열거형 데이터 문자열로 변경하여 저장
            bool isUnlock = PlayerPrefs.GetInt(achivementName) == 1;  // 키 값을 이용해 값을 가져와서 해금되었는지를 저장(0이면 해금x 1이면 해금)
            // 해금되었다면 해금 캐릭터 버튼을 활성화하고 잠겨있는 버튼은 비활성화
            lockCharacter[i].SetActive(!isUnlock);
            unlockCharacter[i].SetActive(isUnlock);
        }
    }
}

using System;
using System.Collections;
using UnityEngine;

public class AchiveManager : MonoBehaviour
{
    // Achivement - 업적, 성취
    // 이 클래스는 업적을 관리하는 역할을 수행

    public GameObject[] lockCharacter;  // 해금이 안 된 상태의 버튼을 저장할 배열
    public GameObject[] unlockCharacter;  // 해금된 상태의 버튼을 저장할 배열
    public GameObject noticeUI;  // 알림창 UI를 저장할 변수
    WaitForSecondsRealtime waitTime;  // WaitForSeconds는 Time.timeScale의 영향을 받기 때문에 시간 계산이 멈추거나 빨라진다. 이 게임 내 시간에 영향을 받지 않도록 하기 위해 실시간을 이용하는 WaitForSecondsRealtime을 사용한다.

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

        waitTime = new WaitForSecondsRealtime(3f);  // 코루틴에서 얼마나 기다릴지를 정하는 시간 초기화(3초를 기다리겠다는 의미)

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

    private void LateUpdate()
    {
        // 모든 업적을 순회해서 해당 업적이 달성되었는지를 체크해서 변수에 저장
        foreach(Achivement achivement in achivements)
        {
            CheckAcivement(achivement);
        }
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
        for(int i = 0; i < lockCharacter.Length; i++)
        {
            string achivementName = achivements[i].ToString();  // 키 값에 사용할 열거형 데이터 문자열로 변경하여 저장
            bool isUnlock = PlayerPrefs.GetInt(achivementName) == 1;  // 키 값을 이용해 값을 가져와서 해금되었는지를 저장(0이면 해금x 1이면 해금)
            // 해금되었다면 해금 캐릭터 버튼을 활성화하고 잠겨있는 버튼은 비활성화
            lockCharacter[i].SetActive(!isUnlock);
            unlockCharacter[i].SetActive(isUnlock);
        }
    }

    // 인자 값으로 전달받은 업적에 대한 달성 확인 및 해금 등을 수행할 함수
    void CheckAcivement(Achivement achivement)
    {
        // 항상 유의해야 할 점은 지역 변수는 초기화하지 않으면 컴파일러가 값을 알지 못하여서 에러가 발생할 수 있다는 것이다.
        bool isComplete = false;  // 업적을 달성했는지 여부를 저장할 변수

        switch (achivement)
        {
            case Achivement.UnlockPotato:
                // 해금 조건 - 몬스터 100마리 처치
                isComplete = GameManager.instance.kill == 100;
                break;
            case Achivement.UnlockBean:
                // 해금 조건 - 생존 성공
                isComplete = GameManager.instance.gameTime == GameManager.instance.maxGameTime;
                break;
        }

        // 업적의 달성 여부와 이미 달성되었는지를 확인하여 달성되지 않은 업적의 달성이 이뤄졌다면 업적 달성에 해당하는 값으로 변환
        if (isComplete && PlayerPrefs.GetInt(achivement.ToString()) == 0)
        {
            PlayerPrefs.SetInt(achivement.ToString(), 1);
            
            /* UI에 각 상황에 맞는 설명과 이미지를 알맞게 출력되도록 하는 로직을 추가할 예정*/
            for(int i = 0; i < noticeUI.transform.childCount; i++)
            {
                // 이 코드를 사용하기 위해서는 UI의 하이라키 순서도 중요하다.
                // 하이라키 순서대로 자식 오브젝트의 순서(인덱스)가 결정되기 때문에 열거형으로 정의해놓은 업적 데이터와 알맞는 UI를 출력하기 위해서 하이라키 순서를 맞추는 것은 반드시 필요한 작업이다.
                bool isCompleteAchive = (i == (int)achivement);
                noticeUI.transform.GetChild(i).gameObject.SetActive(isCompleteAchive);
            }

            // 업적을 달성해서 캐릭터가 해금되었음을 알리는 UI를 보여줬다가 일정 시간이 지난 후 안 보이도록 설정
            StartCoroutine(NoticeUIRoutine());
        }
    }

    // 업적 달성을 알리는 UI가 나타났다가 일정 시간이 지나서 다시 사라지도록 하는 코루틴 함수
    IEnumerator NoticeUIRoutine()
    {
        noticeUI.SetActive(true);
        AudioManager.instance.PlaySFX(AudioManager.SFXType.Melee);

        yield return waitTime;  // yield return new WaitForSeconds(3f)는 계속 새로운 WaitForSeconds 객체를 생성해서 사용하기 때문에 메모리적 손해가 발생할 수 있어서 자주 사용될 만한 것들에는 따로 선언을 해주고 난 후 사용하는 것이 좋다.

        noticeUI.SetActive(false);
    }
}

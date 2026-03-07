using NUnit.Framework.Constraints;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;  // 싱글톤 객체

    [Header("BGM")]
    public AudioClip bgmClip;  // 배경음 파일
    public float bgmVolume;  // 배경음 볼륨
    AudioSource bgmPlayer;  // 배경음 플레이어

    // SFX - Sound Effects의 약자로 대사나 음악을 제외한 효과음을 의미한다.(비슷해 보이는 VFX는 Visual Effects로 시각 효과를 의미한다. 같은 단어인데 의미가 다른 경우도 존재하는데 Special Effect라고 촬영 현장에서 폭발, 분장 등 물리적으로 구현하는 장면을 의미하는 단어도 있다.)
    [Header("SFX")]
    public AudioClip[] sfxClips;  // 효과음 파일들
    public float sfxVolume;  // 효과음 볼륨
    public int channels;  // 여러 사운드가 동시에 들릴 수 있도록 채널 개수 설정
    AudioSource[] sfxPlayers;  // 효과음 플레이어들
    int channelIndex;  // 가장 최근 재생한 채널의 인덱스 번호를 저장
    public enum SFXType  // sfxClips 배열에 사운드 클립이 저장된 순서를 맞출 수 있도록 열거형으로 인덱스에 이름 설정
    {
        Dead,
        Hit,
        LevelUp = 3,  // 순서대로 하다가 다른 숫자를 주고 싶다면 이렇게 직접 값을 주는 것도 가능하다.(다음 값은 이 값의 다음 값을 갖는다. 3이면 다음 값은 4이다.)
        Lose,
        Melee,  
        Range = 7,  //이렇게 하는 이유는 앞 순서의 사운드 클립이 동일한 이름을 가진 사운드 클립이기 때문에 인덱스와 맞추기 위해서이다.
        Select,
        Win,
    }

    private void Awake()
    {
        // 오디오도 어디서나 호출할 수 있기 때문에 싱글톤으로 만들어서 사용
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        SoundInit();
    }

    // 소리들을 초기화하는 함수
    void SoundInit()
    {
        // 두 플레이어 공통으로 필요한 초기 설정 - 게임 시작하자마자 소리가 재생되지 않도록 이를 담당하는 요소의 값 변경, 소리의 크기를 변수에 저장한 값으로 적용, 소리 파일 적용, 루프를 할지말지 설정(기본값은 false라 루프하라고 하려면 true로 값 변환을 명시하고 아니면 넘어가도 된다.)
        // 배경음 플레이어 초기화
        GameObject bgmPlayerObject = new GameObject("BGMPlayerObject");  // 빈 게임 오브젝트 하나를 생성(오브젝트의 이름을 문자열 인자 값을 전달하여 정할 수 있다.)
        bgmPlayerObject.transform.parent = transform;  // 생성한 오브젝트의 부모를 AudioManager 오브젝트로 설정
        bgmPlayer = bgmPlayerObject.AddComponent<AudioSource>(); // AudioSource 컴포넌트를 코드로 추가하고 저장
        bgmPlayer.playOnAwake = false;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.loop = true;
        bgmPlayer.clip = bgmClip;

        // 효과음 플레이어 초기화
        GameObject sfxPlayerObject = new GameObject("SFXPlayerObject");  // 빈 게임 오브젝트 하나를 생성
        sfxPlayerObject.transform.parent = transform;  // 생성한 오브젝트의 부모를 AudioManager 오브젝트로 설정
        sfxPlayers = new AudioSource[channels];  // 효과음 플레이어들들을 저장하는 배열의 크기가 channels가 되도록 초기화
        // 각 효과음 플레이어들에 AudioSource 컴포넌트를 코드로 추가하고 저장하고 초기 설정
        for(int i = 0; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i] = sfxPlayerObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].volume = sfxVolume;
            // 플레이어에서 플레이할 사운드 클립은 각 상황에 맞게 출력할 수 있도록 호출할 때 지정되도록 따로 진행한다.
        }
    }

    // 효과음을 재생시키는 함수
    void PlaySFX(SFXType sfxType)
    {
        // 인자 값으로 받은 효과음에 맞는 사운드 클립을 재생하지 않고 있는 효과음 플레이어에 넣고 재생시키도록 한다.
        for(int i = 0; i < sfxPlayers.Length; i++)
        {
            // channelIndex는 가장 최근 재생한 채널의 인덱스 번호를 의미한다. 즉, 가장 최근에 재생한 채널에 그 다음 채널부터 재생 중인지 확인하기 위해 이런 수식을 사용한다.
            // 그런데 i가 커질수록 sfxPlayers.Length보다 큰 값이 나올 수 있기 때문에 % sfxPlayers.Length를 통해 sfxPlayers.Length보다 큰 값이 나올 수 없도록 제한한다.
            int loopIndex = (i + channelIndex) % sfxPlayers.Length;

            // 현재 검사하는 플레이어가 이미 재생 중이라면 다음 인덱스를 확인
            if (sfxPlayers[loopIndex].isPlaying)
                continue;

            // 현재 플레이어가 재생 중이 아니라면 플레이어에 오디오 클립을 넣고 재생시키기
            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfxType];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }
}

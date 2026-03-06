using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;  // 싱글톤 객체

    [Header("BGM")]
    public AudioClip bgmClip;  // 배경음악 파일
    public float bgmVolume;  // 배경음악 볼륨
    AudioSource bgmPlayer;  // 배경음악 플레이어

    // SFX - Sound Effects의 약자로 대사나 음악을 제외한 효과음을 의미한다.(비슷해 보이는 VFX는 Visual Effects로 시각 효과를 의미한다. 같은 단어인데 의미가 다른 경우도 존재하는데 Special Effect라고 촬영 현장에서 폭발, 분장 등 물리적으로 구현하는 장면을 의미하는 단어도 있다.)
    [Header("SFX")]
    public AudioClip[] sfxClips;  // 효과음 파일들
    public float sfxVolume;  // 배경음악 볼륨
    AudioSource[] sfxPlayers;  // 효과음 플레이어들
    // 여러 사운드가 동시에 들릴 수 있도록 채널 설정(내일 할 예정)

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
    }
}

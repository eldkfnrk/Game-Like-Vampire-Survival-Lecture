using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType
    {
        Exp,
        Level,
        Kill,
        Timer,
        HP,
    }

    public InfoType type;

    TextMeshProUGUI myTextMeshPro;
    Text myText;
    Slider mySlider;

    private void Awake()
    {
        myTextMeshPro = GetComponent<TextMeshProUGUI>();
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    private void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Exp:
                float curExp = GameManager.instance.exp;
                float maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length - 1)];
                mySlider.value = curExp / maxExp;
                break;
            case InfoType.Level:
                myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level);
                break;
            case InfoType.Kill:
                myText.text = string.Format("{0:F0}", GameManager.instance.kill);
                break;
            case InfoType.Timer:
                float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
                int min = Mathf.FloorToInt(remainTime / 60f);
                int sec = Mathf.FloorToInt(remainTime % 60f);
                myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;
            case InfoType.HP:
                float curHP = GameManager.instance.HP;
                float maxHP = GameManager.instance.maxHP;
                mySlider.value = curHP / maxHP;
                break;
        }
    }
}

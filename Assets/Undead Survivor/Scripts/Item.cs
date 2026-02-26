using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData itemData;
    public int level;
    public Weapon weapon;
    public Gear gear;

    Image itemIcon;
    Text levelText;
    Text nameText;
    Text descriptionText;

    private void Awake()
    {
        itemIcon = GetComponentsInChildren<Image>()[1];
        itemIcon.sprite = itemData.itemIcon;
        Text[] texts = GetComponentsInChildren<Text>();
        // GetComponentsInChildren로 저장하는 순서는 하이라키 창의 오브젝트 순서로 정해지기 때문에 원하는 결과 값을 적절하게 변수에 담는 것이 가능하다.
        levelText = texts[0];
        nameText = texts[1];
        descriptionText = texts[2];
    }

    private void OnEnable()
    {
        levelText.text = string.Format("Lv.{0}", level);
        nameText.text = itemData.itemName;
        switch (itemData.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                descriptionText.text = string.Format(itemData.itemDescription, itemData.damages[level] * 100f, itemData.counts[level]);
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                descriptionText.text = string.Format(itemData.itemDescription, itemData.damages[level] * 100f);
                break;
            case ItemData.ItemType.Potion:
                descriptionText.text = string.Format(itemData.itemDescription);
                break;
        }

        if (level == itemData.damages.Length)
        {
            //이때 버튼 오작동 방지를 위해서 버튼의 Navigation 속성은 None으로 설정
            GetComponent<Button>().interactable = false;
        }
    }


    public void OnClick()
    {
        switch (itemData.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                if(level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.WeaponInit(itemData);
                }
                else
                {
                    // 레벨 업 시 데미지 및 관통력 증가
                    // 기본 데미지와 관통력에 정의된 레벨 당 증가율을 적용
                    // 무기의 레벨 업을 담당하는 함수 호출로 증가된 값을 게임에 적용
                    float nextDamage = itemData.baseDamage;
                    int nextCount = itemData.baseCount;

                    nextDamage += itemData.baseDamage * itemData.damages[level];
                    nextCount = itemData.counts[level];

                    weapon.LevelUp(nextDamage, nextCount);
                }

                level++;
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                if (level == 0)
                {
                    // 장비도 무기와 마찬가지로 0레벨에서 1레벨로 올라갈 때 생성된다.
                    GameObject newGear = new GameObject();
                    gear = newGear.AddComponent<Gear>();
                    gear.GearInit(itemData);
                }
                else
                {
                    // 레벨 업 시 적용 값 변경
                    // 기본 비율 적용
                    float nextRate = itemData.damages[level];

                    gear.LevelUp(nextRate);
                }

                level++;
                break;
            case ItemData.ItemType.Potion:
                GameManager.instance.HP = GameManager.instance.maxHP;
                break;
        }

        if(level == itemData.damages.Length)
        {
            //이때 버튼 오작동 방지를 위해서 버튼의 Navigation 속성은 None으로 설정
            GetComponent<Button>().interactable = false;
        }
    }
}

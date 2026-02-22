using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData itemData;
    public int level;
    public Weapon weapon;

    Image itemIcon;
    Text levelText;

    private void Awake()
    {
        itemIcon = GetComponentsInChildren<Image>()[1];
        itemIcon.sprite = itemData.itemIcon;
        Text[] texts = GetComponentsInChildren<Text>();
        levelText = texts[0];
    }

    private void LateUpdate()
    {
        levelText.text = string.Format("Lv.{0}", level);
    }

    public void OnClick()
    {
        switch (itemData.itemType)
        {
            //이렇게 두 조건을 함께 쓰는 것도 가능(이 경우는 ||와 같다.)
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                if(level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.WeaponInit(itemData);
                }
                break;
            case ItemData.ItemType.Glove:

                break;
            case ItemData.ItemType.Shoe:

                break;
            case ItemData.ItemType.Potion:

                break;
        }

        level++;

        if(level > itemData.damages.Length)
        {
            //이때 버튼 오작동 방지를 위해서 버튼의 Navigation 속성은 None으로 설정
            GetComponent<Button>().interactable = false;
        }
    }
}

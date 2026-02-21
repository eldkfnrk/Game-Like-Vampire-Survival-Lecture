using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scritable Object/Item Data")]
public class ItemData : ScriptableObject
{
    public enum ItemType
    {
        Melee,
        Range,
        Glove,
        Shoe,
        Potion,
    }

    [Header("Item Info")]
    public ItemType itemType;
    public int itemId;
    public string itemName;
    public string itemDescription;  //Description - 설명
    public Sprite itemIcon;

    [Header("Level Data")]
    public float baseDamage;  //게임 시작 시 데미지
    public int baseCount;  //게임 시작 시 근접 무기 개수(원거리 무기는 관통력)
    public float[] damages;  //레벨 당 데미지 증가률
    public int[] counts;  //레벨 당 개수(관통력) 증가

    [Header("Weapon")]
    public GameObject projectile;  //이 아이템의 프리팹(projectile - 발사체)
}

using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rectTransform;
    Item[] items;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
    }

    public void Show()
    {
        NextOption();
        rectTransform.localScale = Vector3.one;
        GameManager.instance.GameStop();
    }

    public void Hide()
    {
        rectTransform.localScale = Vector3.zero;
        GameManager.instance.GameResume();
    }

    public void Select(int index)
    {
        items[index].OnClick();
    }

    void NextOption()
    {
        // 1. 모든 아이템 비활성화
        foreach(Item item in items)
        {
            if (!item.gameObject.activeSelf)
                continue;

            item.gameObject.SetActive(false);
        }

        // 2. 그 중에서 랜덤 3개 아이템 활성화
        int[] random = new int[3];

        while (true)
        {
            random[0] = Random.Range(0, items.Length);
            random[1] = Random.Range(0, items.Length);
            random[2] = Random.Range(0, items.Length);

            if (random[0] != random[1] && random[0] != random[2] && random[1] != random[2])
                break;
        }

        for (int i = 0; i < random.Length; i++)
        {
            Item activeItem = items[random[i]];

            // 3. 만렙 아이템의 경우는 소비 아이템으로 대체
            if(activeItem.level > activeItem.itemData.damages.Length)
            {
                activeItem = items[4];
                activeItem.gameObject.SetActive(true);
            }
            else
            {
                activeItem.gameObject.SetActive(true);
            }
        }
    }
}

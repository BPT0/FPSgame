using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ScriptableObject : 상속시 게임 오브젝트에 붙이지 않아도 효과가 적용되게 해주는 효과를 줌
[CreateAssetMenu(fileName ="New Item", menuName ="New Item/item")]
public class Item : ScriptableObject {
    public string itemName; // 아이템의 이름
    public ItemType itemType; // 아이템의 유형
    // Sprite : canvas 밖에서도 이미지가 표시가능하게하는 객체
    public Sprite itemImage; // 아이템의 이미지 
    public GameObject itemPrefab; // 아이템의 프리팹

    public string weaponType; // 무기 유형
    public enum ItemType
    {
        Equipment,
        Used,
        Ingredient,
        ETC
    }

}

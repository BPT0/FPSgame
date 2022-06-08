using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // 인벤토리 창이 활성화 되면 다른 기능을 통제하는 변수
    public static bool inventoryActivated = false;

    // 필요한 컴포넌트
    [SerializeField]
    private GameObject go_InventoryBase;
    [SerializeField]
    private GameObject go_SlotsParent;
    private StartMsg theStartMsg;

    // 슬롯들
    private Slot[] slots;

    // Start is called before the first frame update
    void Start()
    {
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();
    }

    // Update is called once per frame
    void Update()
    {
        TryOpenInventory();
    }

    private void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;

            if (inventoryActivated)
                OpenInventory();
            else
                CloseInventory();
        }
    }

    private void OpenInventory()
    {
        go_InventoryBase.SetActive(true);
    } 

    private void CloseInventory()
    {
        go_InventoryBase.SetActive(false);
    }

    public void AcquireItem(Item _item, int _count=1)
    {
        if (Item.ItemType.Equipment != _item.itemType)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if(slots[i].item != null)
                {
                    if(slots[i].item.itemName == _item.itemName)
                    {
                        slots[i].SetSlotCount(_count);
                        return;
                    }
                }
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(_item, _count);
                return;
            }
        }

        // 인벤토리 창에 돌, 사파이어, 자수정을 모을때마다 감소시켜줌
        if (Item.ItemType.Ingredient == _item.itemType)
        {
            if (_item.itemName == "Rock")
                theStartMsg.get_Rock();
            if (_item.itemName == "Sapaier")
                theStartMsg.get_Sapaier();
            if (_item.itemName == "Amethyst")
                theStartMsg.get_Amethyst();
        }
        theStartMsg.Check_All_Mining();
    }

    
}

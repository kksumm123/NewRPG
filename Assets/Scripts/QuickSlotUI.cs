using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotUI : Singleton<QuickSlotUI>
{
    QuickItemUseBox baseBox;

    public string[] keyBinding = new string[]{
    "<keyboard>/1",
    "<keyboard>/2",
    "<keyboard>/3",
    "<keyboard>/4",
    "<keyboard>/5",
    "<keyboard>/6",
    "<keyboard>/7",
    "<keyboard>/8",
    "<keyboard>/9",
    "<keyboard>/0",
    };
    void Awake()
    {
        baseBox = GetComponentInChildren<QuickItemUseBox>();
        baseBox.LinkComponent();
        baseBox.gameObject.SetActive(true);

        for (int i = 0; i < 10; i++)
        {
            var newButton = Instantiate(baseBox, baseBox.transform.parent);
            int itemUID = UserData.Instance.itemData.data.quickItemUIDs[i];
            InventoryItemInfo inventoryItemInfo = UserData.Instance.GetItem(itemUID);
            newButton.Init(i, inventoryItemInfo, keyBinding[i]);
        }
        baseBox.gameObject.SetActive(false);
    }
}
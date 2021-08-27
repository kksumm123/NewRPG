using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipUI : MonoBehaviour
{
    EquipItemBox baseBox;
    
    void Awake()
    {
        baseBox = GetComponentInChildren<EquipItemBox>(true);
        baseBox.LinkComponent();
        Transform parentLeft = transform.Find("EquipItem/Left");
        Transform parentRight = transform.Find("EquipItem/Right");

        baseBox.gameObject.SetActive(true);
        for (int i = 0; i < 8; i++)
        {
            var parent = i < 4 ? parentLeft : parentRight;
            EquipItemBox newBox = Instantiate(baseBox, parent);

            int itemUID = UserData.Instance.itemData.data.equipItemUIDs[i];
            InventoryItemInfo inventoryItemInfo = UserData.Instance.GetItem(itemUID);

            newBox.Init(i, inventoryItemInfo);
            newBox.itemBox.button.onClick.AddListener(() => OnClick(newBox));
        }
        baseBox.gameObject.SetActive(false);
    }

    private void OnClick(EquipItemBox newBox)
    {
        print(newBox.index);
    }
}

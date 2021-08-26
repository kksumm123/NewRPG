using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipUI : MonoBehaviour
{
    EquipItemBox baseBox;
    void Awake()
    {
        baseBox = GetComponentInChildren<EquipItemBox>(true);
        Transform parentLeft = transform.Find("EquipItem/Left");
        Transform parentRight = transform.Find("EquipItem/Right");

        baseBox.gameObject.SetActive(true);
        for (int i = 0; i < 8; i++)
        {
            var parent = i < 4 ? parentLeft : parentRight;
            EquipItemBox newBox = Instantiate(baseBox, parent);
            newBox.Init(i);
        }
        baseBox.gameObject.SetActive(false);
    }
}

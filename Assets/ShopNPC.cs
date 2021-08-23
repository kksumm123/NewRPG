using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNPC : NPC
{
    protected override bool IsUseableMenu()
    {
        return true;
    }
    protected override void ShowUI()
    {
        print("상점 UI 표시하자");
    }
}

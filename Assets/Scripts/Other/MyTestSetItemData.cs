using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyTestSetItemData : MonoBehaviour, ISetLoopItemData
{
    public void SetData(GameObject chileItem, object data)
    {
        LoopDataItem loopDataItem = (LoopDataItem)data;

        chileItem.transform.Find("Text").GetComponent<Text>().text = loopDataItem.id.ToString();
    }
}

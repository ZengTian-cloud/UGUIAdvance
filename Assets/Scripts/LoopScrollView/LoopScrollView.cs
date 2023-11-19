using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum LoopScrollViewType {
    Horizontal,
    Vertical
}

public class LoopScrollView : MonoBehaviour
{

    #region 字段

    // 子物体的预制体
    public GameObject childItemPrefab;

    public LoopScrollViewType scrollViewType = LoopScrollViewType.Vertical;

    private GridLayoutGroup contentLayoutGroup;

    private ContentSizeFitter sizeFitter;

    private RectTransform content;

    private ILoopDataAdpater dataApdater;

    private ISetLoopItemData setLoopItemData;

    #endregion

    #region 事件

    public Action onMoveDataEnd;

    #endregion

    #region Unity回调

    private void Awake()
    {
        Init();
    }

    #endregion

    #region 方法

    // 初始化
    private void Init() {

        content = transform.Find("Viewport/Content").GetComponent<RectTransform>();
        if (content == null) { throw new System.Exception(" content 初始化失败! "); }
        contentLayoutGroup = content.GetComponent<GridLayoutGroup>();
        if ( contentLayoutGroup == null ) { throw new System.Exception(" contentLayoutGroup 初始化失败! "); }
        sizeFitter = content.GetComponent<ContentSizeFitter>();
        if (sizeFitter == null) { throw new System.Exception(" sizeFitter 初始化失败! "); }

        // 优先从自身进行获取
        dataApdater = transform.GetComponent<ILoopDataAdpater>();
        if (dataApdater == null)
        {
            // 如果没有获取到 用默认的
            dataApdater = new DataApdater();
        }

        setLoopItemData = transform.GetComponent<ISetLoopItemData>();
        if (setLoopItemData == null) { throw new System.Exception(" 未实现 设置数据接口! "); }


        // ----------------测试模拟的数据--------------------

        //List<LoopDataItem> loopDataItems = new List<LoopDataItem>();

        //for (int i = 0; i < 100; i++)
        //{
        //    loopDataItems.Add(new LoopDataItem(i));
        //}

        //dataApdater.InitData(loopDataItems.ToArray());
        // ------------------------------------

    }

    public void InitData(object[] datas , GameObject childItem) {

        if ( childItem != null )
        {
            this.childItemPrefab = childItem;
        }

        contentLayoutGroup.enabled = true;
        sizeFitter.enabled = true;

        // 隐藏所有的子节点
        HideAllChild();
        // 初始化数据
        dataApdater.InitData(datas);
        // 添加一个子节点
        OnAddHead();
        // 禁用
        Invoke("EnableFalseGrid", 0.1f);

    }

    public void AddData(object[] datas) {
        dataApdater.AddData(datas);
    }

    // 获取一个子节点 
    private GameObject GetChildItem() {

        // 查找有没有没 被回收 的子节点 

        for (int i = 0; i < content.childCount; i++)
        {
            if ( !content.GetChild(i).gameObject.activeSelf )
            {
                content.GetChild(i).gameObject.SetActive(true);
                return content.GetChild(i).gameObject;
            }
        }

        // 如果没有 创建一个
        GameObject childItem = GameObject.Instantiate(childItemPrefab, content.transform);
        // 设置数据
        childItem.transform.localScale = Vector3.one;
        childItem.transform.localPosition = Vector3.zero;
        // 设置锚点 
        childItem.GetComponent<RectTransform>().anchorMin = new Vector2(0,1);
        childItem.GetComponent<RectTransform>().anchorMax = new Vector2(0,1);
        // 设置宽高
        childItem.GetComponent<RectTransform>().sizeDelta = contentLayoutGroup.cellSize;

        LoopItem loopItem = childItem.AddComponent<LoopItem>();
        loopItem.onAddHead += this.OnAddHead;
        loopItem.onRemoveHead += this.OnRemoveHead;
        loopItem.onAddLast += this.OnAddLast;
        loopItem.onRemoveLast += this.OnRemoveLast;

        loopItem.SetLoopScrollViewType(this.scrollViewType);
        return childItem;
    }

    // 再上面添加一个物体
    private void OnAddHead() {

        object loopDataItem = dataApdater.GetHeaderData();

        if ( loopDataItem != null )
        {
            //Debug.Log(" 添加头 ");
            Transform first = FindFirst();

            GameObject obj = GetChildItem();
            obj.transform.SetAsFirstSibling();

            // 设置数据
            setLoopItemData.SetData(obj, loopDataItem);
            // 动态的设置位置
            if (first != null)
            {
                switch (scrollViewType)
                {
                    case LoopScrollViewType.Horizontal:
                        obj.transform.localPosition = first.localPosition - new Vector3(contentLayoutGroup.cellSize.x + contentLayoutGroup.spacing.x, 0 , 0);
                        break;
                    case LoopScrollViewType.Vertical:
                        obj.transform.localPosition = first.localPosition + new Vector3(0, contentLayoutGroup.cellSize.y + contentLayoutGroup.spacing.y, 0);
                        break;
                }
                

            }
        }

    }

    // 移除当前最上面的物体
    private void OnRemoveHead() {


        if (dataApdater.RemoveHeaderData())
        {
            //Debug.Log(" 移除头 ");
            Transform first = FindFirst();
            if (first != null)
            {
                first.gameObject.SetActive(false);
            }
        }
    }

    // 再最后加一个物体
    private void OnAddLast() {

        object loopDataItem = dataApdater.GetLastData();
        if (loopDataItem != null)
        {
            //Debug.Log(" 添加尾 ");
            Transform last = FindLast();

            GameObject obj = GetChildItem();
            obj.transform.SetAsLastSibling();
            // 设置数据
            setLoopItemData.SetData(obj, loopDataItem);


            switch (scrollViewType)
            {
                case LoopScrollViewType.Horizontal:

                    // 动态的设置位置
                    if (last != null)
                    {
                        obj.transform.localPosition = last.localPosition + new Vector3(contentLayoutGroup.cellSize.x + contentLayoutGroup.spacing.x, 0, 0);
                    }

                    // 要不要增加高度
                    if (IsNeedAddContentHeight(obj.transform))
                    {
                        // 对高度进行增加
                        content.sizeDelta += new Vector2(contentLayoutGroup.cellSize.x + contentLayoutGroup.spacing.x, 0);
                    }

                    break;
                case LoopScrollViewType.Vertical:

                    // 动态的设置位置
                    if (last != null)
                    {
                        obj.transform.localPosition = last.localPosition - new Vector3(0, contentLayoutGroup.cellSize.y + contentLayoutGroup.spacing.y, 0);
                    }

                    // 要不要增加高度
                    if (IsNeedAddContentHeight(obj.transform))
                    {
                        // 对高度进行增加
                        content.sizeDelta += new Vector2(0, contentLayoutGroup.cellSize.y + contentLayoutGroup.spacing.y);
                    }


                    break;
            }
        }
        else {
            // 没有找到数据
            if (onMoveDataEnd!= null) { onMoveDataEnd(); }
        }

        

    }

    // 移除最后一个物体
    private void OnRemoveLast() {

        if ( dataApdater.RemoveLastData() )
        {
            //Debug.Log(" 移除尾 ");
            Transform last = FindLast();
            if (last != null)
            {
                last.gameObject.SetActive(false);
            }
        }


    }

    private Transform FindFirst() {

        for (int i = 0; i < content.childCount; i++)
        {
            if ( content.GetChild(i).gameObject.activeSelf )
            {
                return content.GetChild(i);
            }
        }
        return null;
    }

    private Transform FindLast() {

        for (int i = content.childCount - 1; i >=0 ; i--)
        {
            if (content.GetChild(i).gameObject.activeSelf)
            {
                return content.GetChild(i);
            }
        }
        return null;
    }

    private void EnableFalseGrid() {
        contentLayoutGroup.enabled = false;
        sizeFitter.enabled = false;
    }

    // 判断是不是需要增加 Content 的高度
    private bool IsNeedAddContentHeight( Transform trans ) {
        Vector3[] rectCorners = new Vector3[4];
        Vector3[] contentCorners = new Vector3[4];
        trans.GetComponent<RectTransform>().GetWorldCorners(rectCorners);
        content.GetWorldCorners(contentCorners);

        switch (scrollViewType)
        {
            case LoopScrollViewType.Horizontal:
                if (rectCorners[3].x > contentCorners[3].x)
                {
                    return true;
                }
                break;
            case LoopScrollViewType.Vertical:
                if (rectCorners[0].y < contentCorners[0].y)
                {
                    return true;
                }
                break;
        }



        return false;
    }


    // 隐藏所有的子节点 ( 回收所有的子节点 )
    private void HideAllChild(){
        for (int i = 0; i < content.childCount; i++)
        {
            content.GetChild(i).gameObject.SetActive(false);
        }
    }

    #endregion 


}

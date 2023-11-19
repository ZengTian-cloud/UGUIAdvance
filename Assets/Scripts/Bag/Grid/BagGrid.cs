using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagGrid : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler,IPointerClickHandler
{

    public ArticleItem articleItem;
    public ArticleItem ArticleItem {
        get {
            return articleItem;
        }
    }

    protected Image bagImage;
    protected Color defaultBagImageColor;
    protected virtual void Awake()
    {
        bagImage = transform.GetComponent<Image>();
        defaultBagImageColor = bagImage.color;
    }

    // 设置格子数据
    public void SetArticleItem( ArticleItem articleItem )
    {
        this.articleItem = articleItem;

        this.articleItem.gameObject.SetActive(true);
        // 设置父物体
        this.articleItem.transform.SetParent(transform);
        // 设置位置
        //this.articleItem.transform.localPosition = Vector3.zero;
        this.articleItem.MoveToOrigin(()=> {
            // 设置宽高
            this.articleItem.GetComponent<RectTransform>().offsetMin = new Vector2(5, 5);
            this.articleItem.GetComponent<RectTransform>().offsetMax = new Vector2(-5, -5);
        });
        // 设置Scale
        this.articleItem.transform.localScale = Vector3.one;
        // 修改rotation
        this.articleItem.transform.localRotation = Quaternion.Euler( Vector3.zero);

    }

    // 清空格子
    public void ClearGrid() {
        articleItem.gameObject.SetActive(false);
        //articleItem.transform.SetParent(null);
        articleItem = null;
    }
 
    public virtual void DragToThisGrid( ArticleItem articleItem )
    {
        // 清空以前的格子
        BagGrid lastGrid = articleItem.transform.parent.GetComponent<BagGrid>();
        // 判断这个格子有没有物品
        if (this.articleItem == null)
        {
            // 清空格子
            lastGrid.ClearGrid();
            // 设置
            SetArticleItem(articleItem);
        }
        else {
            // 要进行交换
            lastGrid.SetArticleItem(this.articleItem);
            // 设置
            SetArticleItem(articleItem);
        }


    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (BagPanel._instance.currentDragArticle != null)
        {
            BagPanel._instance.currentHoverGrid = this;
            bagImage.color = Color.yellow;
        }

        if (this.articleItem != null)
        {
            // 显示当前格子的物品信息
            BagPanel._instance.articleInfomation.Show();
            BagPanel._instance.articleInfomation.SetShowInfo(this.articleItem.GetArticleInfo());
        }
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        BagPanel._instance.currentHoverGrid = null;
        bagImage.color = defaultBagImageColor;

        // 隐藏格子的物品信息
        BagPanel._instance.articleInfomation.Hide();

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(" 鼠标点击! " + eventData.pointerId);
        if (eventData.pointerId == -2)
        {
            // 右键点击
            if (this.articleItem != null)
            {
                this.articleItem.Article.UseArticle();
            }
        }
    }
}

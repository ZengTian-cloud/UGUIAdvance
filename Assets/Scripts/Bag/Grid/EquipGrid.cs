using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipGrid : BagGrid
{

    protected Article article;
    private Image articleSprite;

    // 当前这个格子能装备的物品类型
    protected ArticleType currentEquipType;

    protected override void Awake()
    {
        base.Awake();
        articleSprite = transform.Find("articleSprite").GetComponent<Image>();
    }

    public override void DragToThisGrid(ArticleItem articleItem)
    {
        if (articleItem.Article.articleType == this.currentEquipType)
        {
            // 调用使用物品的方法
            articleItem.Article.UseArticle();
        }
        else {
            BagPanel._instance.ShowTip(" 装备错误，类型不匹配! ");
        }
        articleItem.MoveToOrigin(null);


    }

    // 装备
    public virtual void Equip(Article article)
    {
        // 卸载装备
        UnEquip();

        // 设置数据
        this.article = article;
        // 显示图片
        articleSprite.sprite = Resources.Load<Sprite>(this.article.spritePath);
        articleSprite.gameObject.SetActive(true);
    }
    // 卸载装备
    public virtual void UnEquip() {
        if (this.article != null)
        {
            // 把当前装备的物品放回背包
            BagPanel._instance.AddArticleData(article);
            // 卸掉当前的装备
            this.article = null;

        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        if (BagPanel._instance.currentDragArticle != null)
        {
            if (BagPanel._instance.currentDragArticle.Article.articleType == this.currentEquipType)
            {
                bagImage.color = Color.green;
            }
            else {
                bagImage.color = Color.red;
            }
            
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        //Debug.Log(" 鼠标退出 ");
    }

}

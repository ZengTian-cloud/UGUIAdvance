using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipShoesGrid : EquipGrid
{

    protected override void Awake()
    {
        base.Awake();
        this.currentEquipType = ArticleType.Shoes;
    }

    //public override void DragToThisGrid(ArticleItem articleItem)
    //{
    //    base.DragToThisGrid(articleItem);
    //    if (articleItem.Article.articleType == ArticleType.Shoes)
    //    {
    //        // 处理装备鞋子的逻辑
    //        Equip(articleItem.Article);
    //        // 调用使用物品的方法
    //        articleItem.transform.parent.GetComponent<BagGrid>().UseArticle();
    //    }

    //    articleItem.MoveToOrigin(null);
    //}

    public override void Equip(Article article)
    {
        base.Equip(article);
        ShoesArticle shoesArticle = (ShoesArticle)article;
        // 装备鞋子 
        //Debug.Log(" 移速 + " + shoesArticle.moveSpeed);
        BagPanel._instance.ShowTip(" 移速 + " + shoesArticle.moveSpeed);
    }

}

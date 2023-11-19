using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ShoesArticle : Article
{

    public int moveSpeed; // 移速

    public ShoesArticle(string name, string spritePath, ArticleType articleType, int count,int moveSpeed) : base(name, spritePath, articleType, count)
    {
        this.moveSpeed = moveSpeed;
    }

    public override string GetArticleInfo()
    {
        string info = base.GetArticleInfo();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(info).Append("\n");

        stringBuilder.Append("<color=#E2471C>");
        stringBuilder.Append("移速:").Append(moveSpeed);
        stringBuilder.Append("</color>");

        return stringBuilder.ToString();
    }

    public override void UseArticle()
    {
        base.UseArticle();
        BagPanel._instance.shoesGrid.Equip(this);
    }

}

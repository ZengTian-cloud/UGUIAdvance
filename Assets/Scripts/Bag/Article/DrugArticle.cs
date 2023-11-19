using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DrugArticle : Article
{

    public int hp;

    public DrugArticle(string name, string spritePath, ArticleType articleType, int count,int hp) : base(name, spritePath, articleType, count)
    {
        this.hp = hp;
    }

    public override string GetArticleInfo()
    {
        string info = base.GetArticleInfo();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(info).Append("\n");

        stringBuilder.Append("<color=#E2471C>");
        stringBuilder.Append("恢复血量:").Append(hp);
        stringBuilder.Append("</color>");

        return stringBuilder.ToString() ;
    }

    public override void UseArticle()
    {
        base.UseArticle();
        BagPanel._instance.ShowTip(" 生命值 + " + this.hp);
    }


}

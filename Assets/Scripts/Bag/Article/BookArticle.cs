using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookArticle : Article
{

    // 拓展秘籍

    public BookArticle(string name, string spritePath, ArticleType articleType, int count) : base(name, spritePath, articleType, count)
    {

    }

    public override void UseArticle()
    {
        base.UseArticle();
        BagPanel._instance.ShowTip(" 学习技能 : " + this.name);
    }

}

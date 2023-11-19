using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipWeaponGrid : EquipGrid
{

    protected override void Awake()
    {
        base.Awake();
        this.currentEquipType = ArticleType.Weapon;
    }



    public override void Equip(Article article)
    {
        base.Equip(article);

        WeaponArticle weaponArticle = (WeaponArticle)article;

        BagPanel._instance.ShowTip(" 攻击 + " + weaponArticle.attack);
    }

}

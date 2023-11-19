using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagPanel : ViewBase
{

    #region 数据

    private List<Article> articles = new List<Article>();

    private List<GameObject> articleItems = new List<GameObject>();



    #endregion

    public GameObject articleItemPrefab;
    private BagGrid[] bagGrids;

    public MenuPanel menuPanel;

    // 当前所处的格子
    public BagGrid currentHoverGrid;
    // 当前拖拽的物品
    public ArticleItem currentDragArticle;   

    public static BagPanel _instance;

    public ArticleInfomation articleInfomation;

    private Text tipText;

    public EquipGrid weaponGrid;
    public EquipGrid shoesGrid;


    #region Unity回调

    private void Awake()
    {
        _instance = this;
        InitArticleData();
        bagGrids = transform.GetComponentsInChildren<BagGrid>();
        tipText = transform.Find("Tip").GetComponent<Text>();
    }

    private void Start()
    {
        //LoadData();
       StartCoroutine( LoadDataWithAnim());
    }



    #endregion

    #region 方法

    public override void Hide()
    {
        //base.Hide();
        menuPanel.Show();
        transform.GetComponent<Animator>().SetBool("isShow", false);

        Camera.main.GetComponent<Animator>().SetBool("show_bag", false);
    }

    public override void Show()
    {
        //base.Show();
        Invoke("ShowExc", 1);
    }
    public void ShowExc() {
        base.Show();
        transform.GetComponent<Animator>().SetBool("isShow", true);

        Camera.main.GetComponent<Animator>().SetBool("show_bag",true);

    }

    // 初始化物品数据
    public void InitArticleData() {

        // 武器
        articles.Add(new WeaponArticle("枪", "Sprite/weapon1", ArticleType.Weapon, 1,100));
        articles.Add(new WeaponArticle("刀", "Sprite/weapon2", ArticleType.Weapon, 2,200));
        articles.Add(new WeaponArticle("剑", "Sprite/weapon3", ArticleType.Weapon, 3,300));
        articles.Add(new WeaponArticle("仙剑", "Sprite/weapon4", ArticleType.Weapon, 4,400));
        // 药品
        articles.Add(new DrugArticle("饺子", "Sprite/drug1", ArticleType.Drug, 1,100));
        articles.Add(new DrugArticle("鸡肉", "Sprite/drug2", ArticleType.Drug, 2,200));
        articles.Add(new DrugArticle("药丸", "Sprite/drug3", ArticleType.Drug, 3,500));
        articles.Add(new DrugArticle("仙丹", "Sprite/drug4", ArticleType.Drug, 4,1000));
        // 鞋子
        articles.Add(new ShoesArticle("草鞋", "Sprite/shoe1", ArticleType.Shoes, 1,10));
        articles.Add(new ShoesArticle("布鞋", "Sprite/shoe2", ArticleType.Shoes, 2,20));
        articles.Add(new ShoesArticle("鞋", "Sprite/shoe3", ArticleType.Shoes, 3,50));
        articles.Add(new ShoesArticle("皮鞋", "Sprite/shoe4", ArticleType.Shoes, 4,100));
        // 秘籍
        articles.Add(new BookArticle("降龙十八掌", "Sprite/book1", ArticleType.Book, 1));
        articles.Add(new BookArticle("九阳神功", "Sprite/book2", ArticleType.Book, 2));
        articles.Add(new BookArticle("如来神掌", "Sprite/book3", ArticleType.Book, 3));
        articles.Add(new BookArticle("葵花宝典", "Sprite/book4", ArticleType.Book, 4));

    }

    // 加载数据 ( 加载全部的数据 )
    public void LoadData() {

        HideAllArticleItems();

        for (int i = 0; i < articles.Count; i++)
        {
            GetBagGrid().SetArticleItem(LoadArticleItem(articles[i]));
        }
    }

    public void LoadData(ArticleType articleType) {

        HideAllArticleItems();

        for (int i = 0; i < articles.Count; i++)
        {

            if ( articles[i].articleType == articleType )
            {

                GetBagGrid().SetArticleItem(LoadArticleItem(articles[i]));
            }

        }
    }

    public IEnumerator LoadDataWithAnim() {

        

        HideAllArticleItems();
        yield return null;

        for (int i = 0; i < articles.Count; i++)
        {
            ArticleItem articleItem = LoadArticleItem(articles[i]);
            GetBagGrid().SetArticleItem(articleItem);

            // 修改大小
            articleItem.ScaleMoveToOne(0);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator LoadDataWithAnim(ArticleType articleType) {
        HideAllArticleItems();
        yield return null;

        for (int i = 0; i < articles.Count; i++)
        {
            if (articles[i].articleType == articleType) {
                ArticleItem articleItem = LoadArticleItem(articles[i]);
                GetBagGrid().SetArticleItem(articleItem);

                // 修改大小
                articleItem.ScaleMoveToOne(0);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    // 获取一个空闲的格子
    public BagGrid GetBagGrid()
    {

        for (int i = 0; i < bagGrids.Length; i++)
        {
            if (bagGrids[i].ArticleItem == null)
            {
                return bagGrids[i];
            }
        }
        return null;
    }

    // 加载一个物品
    public ArticleItem LoadArticleItem( Article article ) {

        GameObject obj = GetArticleItem();

        ArticleItem articleItem = obj.GetComponent<ArticleItem>();
        articleItem.SetArticle(article);
        return articleItem;
    }

    // 获取一个物品
    public GameObject GetArticleItem() {

        for (int i = 0; i < articleItems.Count; i++)
        {
            if (articleItems[i].activeSelf == false )
            {
                articleItems[i].SetActive(true);
                return articleItems[i];
            }
        }
        return GameObject.Instantiate(articleItemPrefab);
    }

    // 清理 隐藏所有的物品
    public void HideAllArticleItems() {

        for (int i = 0; i < bagGrids.Length; i++)
        {
            if ( bagGrids[i].ArticleItem != null)
            {
                bagGrids[i].ClearGrid();
            }
        }

    }

    // 移除物品数据
    public void RemoveArticleData(Article article)
    {
        this.articles.Remove(article);
    }

    // 添加数据
    public void AddArticleData(Article article) {
        
        // 对物品数量加1
        article.count++;

        if (articles.Contains(article))
        {
            // 包含这个数据
            // 更新显示
            for (int i = 0; i < bagGrids.Length; i++)
            {

                if (bagGrids[i].ArticleItem != null)
                {
                    if (bagGrids[i].ArticleItem.Article == article)
                    {
                        // 更新显示
                        bagGrids[i].ArticleItem.SetArticle(article);
                        bagGrids[i].ArticleItem.ScaleMoveToOne(1.2f);
                        break;
                    }
                }
                
            }
        }
        else {
            // 没有包含
            articles.Add(article);
            // 进行显示
            ArticleItem articleItem = GetArticleItem().GetComponent<ArticleItem>();
            articleItem.SetArticle(article);
            GetBagGrid().SetArticleItem(articleItem);

            articleItem.ScaleMoveToOne(1.2f);
        }

    }


    // 显示提示
    public void ShowTip(string message) {

        HideTip();

        CancelInvoke("HideTip");
        
        tipText.text = message;
        tipText.gameObject.SetActive(true);
        // 1s 之后隐藏
        Invoke("HideTip", 2);
    }

    public void HideTip() {
        tipText.gameObject.SetActive(false);
    }

    #endregion

    #region 点击事件

    public void OnAllToggleValueChange(bool v) {
        if (v) {
            //LoadData();
            StartCoroutine(LoadDataWithAnim());
        }
    }
    public void OnWeaponToggleValueChange(bool v) {
        if (v) { StartCoroutine(LoadDataWithAnim(ArticleType.Weapon)); }
    }
    public void OnShoesToggleValueChange(bool v) {
        if (v) { StartCoroutine(LoadDataWithAnim(ArticleType.Shoes)); }
    }
    public void OnBookToggleValueChange(bool v) {
        if (v) { StartCoroutine(LoadDataWithAnim(ArticleType.Book)); }
    }
    public void OnDrugToggleValueChange(bool v) {
        if (v) { StartCoroutine(LoadDataWithAnim(ArticleType.Drug)); }
    }

    #endregion

}

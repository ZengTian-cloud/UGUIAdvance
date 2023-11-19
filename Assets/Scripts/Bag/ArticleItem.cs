using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArticleItem : MonoBehaviour
{
    #region 字段
    private Image articleSprite;
    private Text number;

    private Article article;

    private UIDrag uIDrag;

    private Canvas canvas;
    private int defaultSort;

    private Vector3 currentLocalPositon;
    private float moveOriginTimer;       // 计时
    private float moveOriginTime = 0.3f;        // 时间
    private bool isMovingOrigin = false;


    private Action onMoveEnd;
    #endregion

    #region Scale动画

    private float scaleTimer;
    private float scaleTime;
    private Vector3 startScale;
    private bool isScaling = false;

    #endregion


    #region 属性

    public Article Article {
        get {
            return article;
        }
    }

    #endregion

    #region Unity回调


    private void Awake()
    {
        articleSprite = transform.GetComponent<Image>();
        number = transform.Find("Text").GetComponent<Text>();

        uIDrag = transform.GetComponent<UIDrag>();
        uIDrag.onStartDrag += this.OnStartDrag;
        uIDrag.onDrag += this.OnDrag;
        uIDrag.onEndDrag += this.OnEndDrag;

        canvas = transform.GetComponent<Canvas>();
        defaultSort = canvas.sortingOrder;
    }

    private void Update()
    {
        // 向原点移动
        MovingOrigin();
        // scale向1变化
        MovingScaleOne();
    }
    #endregion

    #region 方法

    public void SetArticle( Article article )
    {
        this.article = article;

        // this.article.onDataChange -= this.OnDataChange;
        // 绑定事件
        this.article.onDataChange = this.OnDataChange;
        // 显示数据
        articleSprite.sprite = Resources.Load<Sprite>( article.spritePath);
        number.text = article.count.ToString();

    }

    public void OnStartDrag() {
        canvas.sortingOrder = defaultSort + 1;
        BagPanel._instance.currentDragArticle = this;

        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    public void OnDrag() {

    }
    public void OnEndDrag() {

        if (BagPanel._instance.currentHoverGrid != null)
        {
            // 进到这个格子里面
            BagPanel._instance.currentHoverGrid.DragToThisGrid(BagPanel._instance.currentDragArticle);

            canvas.sortingOrder = defaultSort;
        }
        else {
            // 回到原点
            MoveToOrigin(() => {
                // 恢复层级
                canvas.sortingOrder = defaultSort;
            });
        }
        BagPanel._instance.currentDragArticle = null;

        // 恢复
        transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    // 正在向原点移动
    private void MovingOrigin() {
        if (isMovingOrigin)
        {
            moveOriginTimer += Time.deltaTime * (1 / moveOriginTime);
            transform.localPosition = Vector3.Lerp(currentLocalPositon, Vector3.zero, moveOriginTimer);
            if (moveOriginTimer >= 1)
            {
                isMovingOrigin = false;
                if (onMoveEnd != null) { onMoveEnd(); }
            }
        }
    }

    // scale正在向1进行变化
    private void MovingScaleOne() {
        if (isScaling)
        {
            scaleTimer += Time.deltaTime * (1 / scaleTime);
            transform.localScale = Vector3.Lerp(startScale, Vector3.one, scaleTimer);
            if (scaleTimer>=1) {
                isScaling = false;
            }
        }
    }

    public void MoveToOrigin( Action onMoveEnd ) {
        isMovingOrigin = true;
        moveOriginTimer = 0;
        currentLocalPositon = transform.localPosition;
        this.onMoveEnd = onMoveEnd;
    }
    // scale变化到1
    public void ScaleMoveToOne( float scale , float time = 0.5f )
    {

        scaleTimer = 0;
        scaleTime = time;
        startScale = Vector3.one * scale;
        isScaling = true;
    }

    public string GetArticleInfo() {

        return this.article.GetArticleInfo();
    }

    // 当数据改变的事件
    public void OnDataChange(Article article) {

        Debug.Log(" 数据改变： " + this.name);

        if (article.count == 0)
        {
            // 清空格子
            transform.parent.GetComponent<BagGrid>().ClearGrid();
        }
        else {
            // 更新数据
            SetArticle(article);
        }
    }

    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataApdater : ILoopDataAdpater
{
    #region 字段

    // 保存的所有的数据
    public List<object> allData = new List<object>();
    // 当前显示的数据
    public LinkedList<object> currentShowData = new LinkedList<object>();

    #endregion


    #region 方法

    public object GetHeaderData()  
    {
        // 判断总数据的数量
        if (allData.Count == 0) {

            return null;
        }

        // 特殊的情况
        if ( currentShowData.Count == 0 )
        {
            object header = allData[0];
            currentShowData.AddFirst(header);
            return header;
        }

        // 获取到当前的第一个数据
        object t = currentShowData.First.Value;
        int index = allData.IndexOf(t);
        if ( index != 0 )
        {
            object header = allData[index - 1] ;
            // 加到当前显示的数据里面
            currentShowData.AddFirst(header);
            return header;
        }

        return null;
    }

    public bool RemoveHeaderData() {

        // 移除 currentShowData 第一个数据
        if ( currentShowData.Count == 0 || currentShowData.Count == 1)
        {
            return false;
        }
        currentShowData.RemoveFirst();
        return true;
    }

    public object GetLastData()  
    {

        // 判断总数据的数量
        if (allData.Count == 0)
        {

            return null;
        }

        // 特殊的情况
        if (currentShowData.Count == 0)
        {
            object l = allData[0] ;
            currentShowData.AddLast(l);
            return l;
        }

        // 获取 currentShowData 最后一个的下一个

        object last = currentShowData.Last.Value  ;
        int index = allData.IndexOf(last);

        if ( index != allData.Count - 1 )
        {
            object now_last = allData[index + 1]  ;
            currentShowData.AddLast(now_last);
            return now_last;
        }

        return null;
    }

    public bool RemoveLastData() {
        // 移除 currentShowData 最后一个 
        if ( currentShowData.Count == 0 || currentShowData.Count == 1) { return false; }
        currentShowData.RemoveLast();
        return true;
    }

    #endregion

    #region 数据管理

    public void InitData(object[] t) 
    {
        allData.Clear();
        currentShowData.Clear();

        allData.AddRange(t);
    }

    public void InitData(List<object> t) 
    {
        InitData(t.ToArray());
    }

    public void AddData(object[] t) 
    {
        allData.AddRange(t);
    }

    public void AddData(List<object> t)  
    {
        AddData(t.ToArray());
    }

 

    #endregion





}

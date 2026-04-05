using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
public class DataManager : MonoBehaviour
{
    public List<BaseDataSO> m_Listdata = new List<BaseDataSO>();

    //async void Awake()
    //{
    //    List<Task> tasks = new List<Task>();
    //    foreach (var item in m_Listdata)
    //    {
    //        tasks.Add(item.InitAsync());
    //    }
    //    await Task.WhenAll(tasks);
    //}
    [ContextMenu("데이터 로드하기")]
    async void DataLoad()
    {
        List<Task> tasks = new List<Task>();
        foreach (var item in m_Listdata)
        {
            tasks.Add(item.InitAsync());
        }
        await Task.WhenAll(tasks);
    }

    public SheetDataSO<T> GetData<T>() where T : class, IIdentifiable, ISheetParsable, new()
    {
        foreach (var item in m_Listdata)
        {
            // item이 우리가 찾는 T 타입(예: Char2Data)인지 확인하고, 
            // 맞다면 result라는 변수에 T 타입으로 담아서 바로 반환합니다.
            if(item is SheetDataSO<T> result)
            {
                return result;
            }
        }
        return null;
    }
}

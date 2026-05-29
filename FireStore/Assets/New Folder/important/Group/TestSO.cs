using Firebase.Extensions;
using Firebase.Firestore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
[CreateAssetMenu(fileName = "TestSO", menuName = "ScriptableObjects/TestSO")]
public class TestSO : BaseFireStore
{
    public override void InitDataBase(FirebaseFirestore database, string YouserID = null)
    {
        base.InitDataBase(database, YouserID);
    }

    public override Task<DocumentSnapshot> GetSnapshotAsync()=> currentRef.GetSnapshotAsync();
    public override async Task SetDataAsync(object data) => await currentRef.SetAsync(data);
    public override async Task UpdateDataAsync<T>(Dictionary<string, object> data, bool flag = false)
    {
        await base.UpdateDataAsync<T>(data, flag);
    }
    public override async Task<T> GetSnapshotAsync<T>()
    {
       
        var data = await  base.GetSnapshotAsync<T>();
        return data;
        //DocumentSnapshot snapshot = await currentRef.GetSnapshotAsync();
        //if (snapshot.Exists)
        //{
        //    return snapshot.ConvertTo<T>();
        //}
        //Debug.LogWarning("문서를 찾을 수 없습니다.");
        //return default(T);
    }
    public override Task DeleteDataAsync()
    {
        var getdata =base.DeleteDataAsync();
        return getdata;
        //currentRef.DeleteAsync().ContinueWithOnMainThread(task =>
        //{
        //    if (task.IsCompletedSuccessfully)
        //    {
        //        Debug.Log($"✅ {currentRef.Path} 경로의 문서 삭제 완료!");
        //    }
        //    else
        //    {
        //        Debug.LogError($"❌ {currentRef.Path} 경로의 문서 삭제 실패: {task.Exception}");
        //    }
        //});
        //return Task.CompletedTask;
    }
}

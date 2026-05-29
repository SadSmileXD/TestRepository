using Firebase.Extensions;
using Firebase.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks; // Task를 사용하기 위해 추가
using UnityEngine;



public abstract class BaseFireStore : ScriptableObject
{
    
    public List<string> m_collections = new List<string>();
    public List<string> m_documents = new List<string>();
    public DocumentReference currentRef;
    public CollectionReference currentCollection;
    // 매니저에서 딕셔너리 매핑용으로 쓸 식별자
  [SerializeField] private DataType m_EnumType;
    public DataType EnumType => m_EnumType;

    // 보호 수준을 protected로 변경하거나 매니저가 주입해주는 용도로 사용
    protected FirebaseFirestore db;

    public virtual void InitDataBase(FirebaseFirestore database,string YouserID=null)
    {
        db = database;
        //m_documents  = Auth.uid 넣기
        currentRef = db.Collection(m_collections[0]).Document(m_documents[0]);
        currentCollection = db.Collection(m_collections[0]);
        for (int i = 1; i < m_documents.Count; i++)
        {
            currentCollection = currentRef.Collection(m_collections[i]);
            // 기존 주소 뒤에 다음 컬렉션과 다음 문서를 줄줄이 소시지처럼 엮습니다.
            currentRef = currentCollection.Document(m_documents[i]);
        }

       
    }
    public abstract Task SetDataAsync(object data);

    public abstract Task<DocumentSnapshot> GetSnapshotAsync();
    public virtual async Task<T> GetSnapshotAsync<T>()
    {
        DocumentSnapshot snapshot = await currentRef.GetSnapshotAsync();

        if (snapshot.Exists)
        {
            return snapshot.ConvertTo<T>();
        }
        Debug.LogWarning("문서를 찾을 수 없습니다.");
        return default(T);
    }
    public virtual async Task UpdateDataAsync<T>(Dictionary<string, object> data, bool flag=false)
    {
        Type m_TargetDataType = typeof(T);
        PropertyInfo[] properties = m_TargetDataType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (PropertyInfo property in properties)
        {
            if (data.ContainsKey(property.Name))
            {
                if (flag ==false)
                {
                    TryDataCheck(property,data);
                }
                else
                {
                    await currentRef.UpdateAsync(data);
                }
               
            }
            else
            {
                Debug.LogError("같은 필드가 존재 하지 않습니다.");
                
            }
        }

    }
    public virtual Task DeleteDataAsync()
    {
        currentRef.DeleteAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                Debug.Log($"✅ {currentRef.Path} 경로의 문서 삭제 완료!");
            }
            else
            {
                Debug.LogError($"❌ {currentRef.Path} 경로의 문서 삭제 실패: {task.Exception}");
            }
        });
        return Task.CompletedTask;
    }
    private async void TryDataCheck(PropertyInfo property, Dictionary<string, object> data)
    {
        Debug.Log("같음");
        var Datatype = property.PropertyType;
        object dictionaryValue = data[property.Name];
        var getTypvalue = dictionaryValue.GetType();

        Debug.Log($"Datatype: {Datatype}");
        Debug.Log($"getTypvalue: {getTypvalue}");
        bool sameType = Datatype == getTypvalue;
        if (sameType)
        {
            Debug.Log($"데이터 타입 같음");
            await currentRef.UpdateAsync(data);
        }
    }
}
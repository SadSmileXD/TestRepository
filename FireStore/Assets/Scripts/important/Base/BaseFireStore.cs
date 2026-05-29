using Firebase.Extensions;
using Firebase.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks; // Task를 사용하기 위해 추가
using UnityEngine;



public  class BaseFireStore : ScriptableObject
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
    public virtual async Task SetDataAsync(object data) => await currentRef.SetAsync(data);

   
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
    public virtual async Task UpdateDataAsync<T>(Dictionary<string, object> data, bool flag = false)
    {
        Type m_TargetDataType = typeof(T);

        // DTO의 프로퍼티들을 검색하기 쉽게 사전에 딕셔너리 형태로 만듭니다.
        var propertyDict = m_TargetDataType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                           .ToDictionary(p => p.Name);

        // 1️⃣ [검증 단계] 내가 변경 요청한 data 딕셔너리의 키들을 검사합니다.
        foreach (var kvp in data)
        {
            if (propertyDict.TryGetValue(kvp.Key, out PropertyInfo property))
            {
                if (flag == false)
                {
                    // 데이터 타입이나 유효성을 루프를 돌며 검증합니다.
                    await TryDataCheck(property, data);
                }
            }
            else
            {
                // DTO에 없는 엉뚱한 필드가 들어왔을 때 경고를 남기고 중단합니다.
                Debug.LogError($"[Update Error] {m_TargetDataType.Name} 구조체에 '{kvp.Key}' 필드가 존재하지 않습니다.");
                return;
            }
        }

        // 2️⃣ [실행 단계] 검증이 무사히 끝나거나, 처음부터 flag가 true였다면 
        // 루프 바깥에서 딱 "한 번만" 서버에 업데이트를 요청합니다.
        if (flag == true)
        {
            await currentRef.UpdateAsync(data);
            Debug.Log($"✅ {currentRef.Path} 문서 업데이트 완료!");
        }
    }
    public virtual async Task DeleteDataAsync()
    {
        try
        {
            // 콜백 대신 동기 코드처럼 슥 읽히도록 await로 대기합니다.
            await currentRef.DeleteAsync();
            Debug.Log($"  {currentRef.Path} 경로의 문서 삭제 완료!");
        }
        catch (Exception ex)
        {
            Debug.LogError($"  {currentRef.Path} 경로의 문서 삭제 실패: {ex.Message}");
        }

    }
    private async  Task TryDataCheck(PropertyInfo property, Dictionary<string, object> data)
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
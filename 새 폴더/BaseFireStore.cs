using System.Threading.Tasks; // Task를 사용하기 위해 추가
using Firebase.Firestore;
using UnityEngine;

 

public abstract class BaseFireStore : ScriptableObject
{
    // 매니저에서 딕셔너리 매핑용으로 쓸 식별자
    [SerializeField] private DataType m_EnumType;
    public DataType EnumType => m_EnumType;

    // 보호 수준을 protected로 변경하거나 매니저가 주입해주는 용도로 사용
    protected FirebaseFirestore db;

    public abstract void InitDataBase(FirebaseFirestore database);
    

    // ⭐ 핵심 변경: 모든 데이터 조작 함수가 진짜 비동기로 작동하도록 Task 반환형으로 수정
    // 함수 이름 뒤에 Async를 붙여 비동기 함수임을 명시 (C# 컨벤션)

    public abstract Task SetDataAsync(object data);

    public abstract Task<DocumentSnapshot> GetSnapshotAsync();
    public abstract Task<DocumentSnapshot> GetSnapshotAsync<T>();
    public abstract Task UpdateDataAsync(object data);

    public abstract Task DeleteDataAsync();
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using NUnit.Framework;
using UnityEngine;

public enum DataType
{
    None,
    Test,
}
public class FireStoreManager : MonoBehaviour
{
    private FirebaseFirestore db;
    public static FireStoreManager Instance { get; private set; }
    [SerializeField] private List<BaseFireStore> m_Data;
    private Dictionary<DataType, BaseFireStore> m_DataDictionary;
  
    private void Awake()
    {
        InitSingleton();
        InitFirebase();
        InitDictionary();
    }

    private void InitSingleton()
    {
      
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private async void InitFirebase()
    {
        await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(async task =>
        {
            DependencyStatus status = task.Result;
            if (status == DependencyStatus.Available)
            {
                Debug.Log("Firebase 초기화 성공");

                db = FirebaseFirestore.DefaultInstance;
                BindClass();
            }
            else
            {
                Debug.LogError($"Firebase 초기화 실패: {status}");
            }
        });
    }
    private  void BindClass()
    {
        foreach (BaseFireStore item in m_Data)
        {
            item.InitDataBase(db);
        }
    }

    private void InitDictionary()
    {
        m_DataDictionary = new Dictionary<DataType, BaseFireStore>();
        m_DataDictionary = m_Data.ToDictionary(x => x.EnumType, x => x);
    }
  
    public FirestoreRequestContext DocumentType(DataType type)
    {
        // 해당 데이터를 처리할 컨텍스트를 새로 생성해서 반환 (동시성 문제 해결)
        return new FirestoreRequestContext(m_DataDictionary[type]);
    }

    [ContextMenu("Test")]
    public async void sdsd()
    {
        await Test();
    }
    private async Task Test()
    {
        var data= await FireStoreManager.Instance.DocumentType(DataType.Test).GetAsync();
        if(data.Exists)
        {
            Debug.Log(data.ToDictionary());
        }
        
    }
}

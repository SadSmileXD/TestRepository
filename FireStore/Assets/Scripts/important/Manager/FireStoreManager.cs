using System;
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
    public SNSPostDTO m_data=new();

    private FirebaseFirestore db;
    public static FireStoreManager Instance { get; private set; }
    [SerializeField] private List<BaseFireStore> m_Data;
    private static Dictionary<DataType, BaseFireStore> m_DataDictionary;
  
    private async void Awake()
    {
        InitSingleton();
       await InitFirebaseAsync();
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
    private async Task InitFirebaseAsync()
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
            Debug.Log("BindClass");
        }
        
    }

    private void InitDictionary()
    {
        m_DataDictionary = new Dictionary<DataType, BaseFireStore>();
        m_DataDictionary = m_Data.ToDictionary(x => x.EnumType, x => x);
    }
  
    public static FirestoreRequestContext DocumentType(DataType type)
    {
        if(!m_DataDictionary.ContainsKey(type))
        {
            var nullparteen = new FireStoreNullSO();
            return new FirestoreRequestContext(nullparteen);
        }
        else
        {
            // 해당 데이터를 처리할 컨텍스트를 새로 생성해서 반환 (동시성 문제 해결)
            return new FirestoreRequestContext(m_DataDictionary[type]);
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////

    [ContextMenu("Test")]
    public async void sdsd()
    {
        await Test();
    }

    [ContextMenu("Save")]
    public async void Save()
    {
        SNSPostDTO testPost = new SNSPostDTO
        {
            // 1. 기본 필드
            ImageIndex = 2,
            Comment = "오늘 새로 산 스티커로 꾸며본 내 고양이 사진! 너무 귀엽지 않나요? 🐱✨ #반려동물 #일상",

            // 2. 셰이더 프로퍼티 구조체 (약간 따뜻하고 대비가 강한 필터 느낌)
            ShaderProperty = new UIShaderProperty
            {
                Brightness = Math.Round(0.05f, 2),
                Contrast = Math.Round(1.2f, 2),
                Saturation = Math.Round(1.1f, 2),
                Temperature = Math.Round(0.6f, 2)
            },

            // 3. 해시태그 리스트
            Hashtags = new List<string> { "반려동물", "일상", "고양이", "꾸미기" },

            // 4. 스티커 리스트 (스티커 3개 배치 예시)
            Stickers = new List<StickerTransformData>
        {
            new StickerTransformData
            {
                StickerId = 101,          // 고양이 귀 모양 스티커
                RelativeX = Math.Round(0.35f, 2),
                RelativeY = Math.Round(0.68f, 2),
                RelativeScale = Math.Round(1.2f, 2) ,
                Rotation = Math.Round(-15.0f, 2)
            },
            new StickerTransformData
            {
                StickerId = 102,          // 볼터치 스티커
                RelativeX = Math.Round(0.42f,2),
                RelativeY = Math.Round(0.62f,2),
                RelativeScale = Math.Round(0.8f,2),
                Rotation = Math.Round(0.0f, 2)
            },
            new StickerTransformData
            {
                StickerId = 505,          // 반짝이는 별 스티커
                RelativeX = Math.Round(0.75f, 2),
                RelativeY = Math.Round(0.25f, 2),
                RelativeScale = Math.Round(1.5f, 2),
                Rotation = Math.Round(45.0f, 2)
            }
        }
        }; await FireStoreManager.DocumentType(DataType.Test)
            .SetAsync(testPost);
    }
    [ContextMenu("get")]
    public async void get()
    {
        m_data = await FireStoreManager.DocumentType(DataType.Test).
            GetAsync<SNSPostDTO>();
    }
    [ContextMenu("update")]
    public async void dateUpdate()
    {
        Dictionary<string, object> updates = new Dictionary<string, object>
        {
          { "Comment", "zzzz" } //  
        };
         
        await FireStoreManager.DocumentType(DataType.Test).UpdateAsync<SNSPostDTO>(updates);
    }
    [ContextMenu("delete")]
    public async void Delete()
    {
         FireStoreManager.DocumentType(DataType.Test).DeleteAsync();
    }
    [ContextMenu("확장메소드 체크")]
    public   void  Extens()
    {
        var data=FireStoreManager.DocumentType(DataType.Test).GetRandomSixData<SNSPostDTO>();
    }
    private async Task Test()
    {

        Dictionary<string, object> updates = new Dictionary<string, object>
        {
          { "Comment", "zzzz" } //  
        };
        //await FireStoreManager.DocumentType(DataType.Test).UpdateAsync(updates);
       await  FireStoreManager.DocumentType(DataType.None)?.UpdateAsync<SNSPostDTO>(updates);
    }
}
[System.Serializable][FirestoreData] // 👈 파이어스토어 변환기 활성화
public struct StickerTransformData
{
    [field: SerializeField][FirestoreProperty] public int StickerId { get; set; }
    [field: SerializeField][FirestoreProperty] public double RelativeX { get; set; }
    [field: SerializeField][FirestoreProperty] public double RelativeY { get; set; }
    [field: SerializeField][FirestoreProperty] public double RelativeScale { get; set; }
    [field: SerializeField][FirestoreProperty] public double Rotation { get; set; }
}

[System.Serializable]
[FirestoreData] // 👈 파이어스토어 변환기 활성화
public struct UIShaderProperty
{
   [field: SerializeField] [FirestoreProperty] public double Brightness { get; set; }
   [field: SerializeField] [FirestoreProperty] public double Contrast { get; set; }
   [field: SerializeField] [FirestoreProperty] public double Saturation { get; set; }
    [field: SerializeField][FirestoreProperty] public double Temperature { get; set; }
}

[System.Serializable]
[FirestoreData] // 👈 파이어스토어 변환기 활성화
public struct SNSPostDTO
{
    // 만약 uid나 id 필드가 있다면 여기에 추가하고 [FirestoreProperty]를 붙이세요.

   [field: SerializeField] [FirestoreProperty] public int ImageIndex { get; set; }
   [field: SerializeField] [FirestoreProperty] public UIShaderProperty ShaderProperty { get; set; }
   [field: SerializeField] [FirestoreProperty] public string Comment { get; set; }
   [field: SerializeField] [FirestoreProperty] public List<StickerTransformData> Stickers { get; set; }
    [field: SerializeField][FirestoreProperty] public List<string> Hashtags { get; set; }
}
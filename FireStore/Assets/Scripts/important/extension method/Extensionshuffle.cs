using Firebase.Firestore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public static class Extensionshuffle
{

    public static async Task<List<T>> GetRandomSixData<T>(this FirestoreRequestContext context)
    {
        // 1. 임시로 컬렉션의 모든 데이터를 담을 원본 리스트를 생성합니다.
        List<T> allData = new List<T>();

        var documents = context.TargetStore.GetCollection();
        var collections = await documents.GetSnapshotAsync();

        // 2. 먼저 컬렉션 안의 모든 문서를 C# 데이터 모델(T)로 변환해 리스트에 넣습니다.
        foreach (var document in collections)
        {
            if (document.Exists)
            {
                T item = document.ConvertTo<T>();
                allData.Add(item);
            }
        }

        //6보다 작으면 그냥 반환
        if (allData.Count < 6)
        {
            Debug.Log("6개 미만");
            return allData;  
        }

        // 3.   피셔-예이츠(Fisher-Yates) 셔플로 리스트를 완전히 무작위로 섞어버립니다.
        int n = allData.Count;
        for (int i = n - 1; i > 0; i--)
        {
            // 0부터 i 사이의 무작위 인덱스 선택
            int randomIndex = Random.Range(0, i + 1);

            // 요소 위치 바꾸기 (Swap)
            T temp = allData[i];
            allData[i] = allData[randomIndex];
            allData[randomIndex] = temp;
        }

        // 4. ✂️ 앞에서부터 6개만 쏙 잘라서 최종 결과 리스트를 생성합니다.
        // 데이터가 6개보다 적을 경우를 대비해 안정장치(Mathf.Min)를 둡니다.
        int takeCount = Mathf.Min(6, allData.Count);
        List<T> result = allData.GetRange(0, takeCount);
         foreach (T item in result)
        {
            Debug.Log(item);
        }
        // 5. 최종 6개(혹은 그 이하)의 무작위 데이터 반환
        return result;
    }
    public static  CollectionReference GetCollection(this BaseFireStore cxt)
    {
        CollectionReference reference = cxt.currentCollection;
        return reference;
    }
}

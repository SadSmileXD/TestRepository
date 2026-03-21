using System;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
public class SampleScripts : MonoBehaviour
{
    [Header("구글 시트 URL 그냥 다 넣으면 된다.(구글 시트 공유 설정 확인 바람)")]
    public string url;
    [Header("사이트 URL 끝에 gid번호 있는데 시트마다 확인해서 해야함")]//기본 첫 시트는 0 인거 같음 그래도 확인 필수
    public int gid;
    public SheetLoader<Attackinfo> data;
     [SerializeField]public List<Attackinfo> Ldata=new List<Attackinfo>();
   
    async void Start()
    {
        // 1. 로더 생성
        data = new SheetLoader<Attackinfo>(url, gid);
        
        // 2. 데이터가 다 로드될 때까지 기다렸다가(await) 리스트를 받아옵니다.
        // GetDataAsync()의 반환 타입이 Task<List<charData>>이므로 await가 필수입니다.
        Ldata = await data.GetDataAsync();
        
     
    }

}

using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;

public class SkyChanage : MonoBehaviour
{
    //기본으로 설정될 Skybox
    [SerializeField] private Cubemap m_defaultCube;
    //바뀔 데이터들을 저장할 List
    [SerializeField] private List<SkyBoxData> SkyBoxs;
    //빠르게 접근할 Dicionary
    private Dictionary<int, Cubemap> skyBoxRepository=new Dictionary<int, Cubemap>();
    [SerializeField]private condition condition;
    private Material Material;
    [SerializeField] private float delayTime = 0f;
    private Coroutine currentCoroutine;
    [SerializeField] private int m_index=0; 
    private void Awake()
    {
        Init_Material();
        condition?.Init(this);
    }
    void Start()
    {
        skyBoxRepository = SkyBoxs.ToDictionary(x => x.index, x => x.cube);

        OnCoroutine();
    }

    private void Update()
    {
        condition?.OnCheck(OnCoroutine);
    }

    public void OnCoroutine()
    {
        if(currentCoroutine.IsNUll())
        {
            currentCoroutine= StartCoroutine(OnChanageSkyBox());
        }
    }

    private IEnumerator OnChanageSkyBox()
    {
        float blendTime = 0;
        while(blendTime<=1f)
        {
            blendTime += (Time.unscaledDeltaTime / delayTime);
            Material.SetFloat("_Blend",blendTime);
        
            yield return null;
        }
        ChanageCubeMap();
        currentCoroutine = null;
    }


    private void ChanageCubeMap()
    {
        var cubeA = Material.GetTexture("_CubeB");
     
        if(skyBoxRepository.TryGetValue(m_index, out var cubeB))
        {
            if(cubeA == cubeB) return;

            Material.SetTexture("_CubeA", cubeA);
            Material.SetTexture("_CubeB", cubeB);
            Material.SetFloat("_Blend", 0f);
            return;
        }


    }
    private void Init_Material()
    {
        //기본적으로 사용할 스카이박스 초기화
        Material = RenderSettings.skybox;
        Material.SetTexture("_CubeA", m_defaultCube);
        Material.SetFloat("_Blend", 0f);
    }
}

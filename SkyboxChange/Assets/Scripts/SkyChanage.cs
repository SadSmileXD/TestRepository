using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using UnityEngine;

public class SkyChanage : MonoBehaviour
{
    //기본으로 설정될 Skybox
    [SerializeField] private Cubemap m_defaultCube;
    //바뀔 데이터들을 저장할 List
    [SerializeField] private List<SkyBoxData> SkyBoxs;
    //빠르게 접근할 Dicionary
    private Dictionary<int, Cubemap> skyBoxRepository=new Dictionary<int, Cubemap>();

    private Material Material;
    [SerializeField] private float delayTime = 0f;
    private void Awake()
    {
        Init_Material();
    }
    void Start()
    {
        skyBoxRepository = SkyBoxs.ToDictionary(x => x.index, x => x.cube);

        StartCoroutine(OnChanageSkyBox());
    }

    public IEnumerator OnChanageSkyBox()
    {
        float t = 0;
        while(t<=1f)
        {
            t += (Time.unscaledDeltaTime / delayTime);
            Material.SetFloat("_Blend",t);
            yield return null;
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

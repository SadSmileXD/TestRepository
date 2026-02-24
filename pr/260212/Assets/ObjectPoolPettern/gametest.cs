using Unity.Profiling;
using UnityEngine;
using UnityEditor;
using UnityEngine.Pool;
using Unity.VisualScripting;
public class gametest : MonoBehaviour 
{
    public GameObject prefab;
    MeshRenderer mr = null;
    Material mat = null;
    
    private static readonly ProfilerMarker CubeGenerateMaker = new("CubeMaker");
    private static readonly ProfilerMarker Physic2DMaker = new("Physic2DMaker");
    /**/
    private float generateTimer = 0f;       // 0.1초 타이머 누적용
    [Header("큐브 무한 생성 딜레이 시간")] public float generateInterval; // 간격
    [Header("큐브 삭제  시간")] public float deleteTime;
    private float generateTimer2 = 0f;       // 0.1초 타이머 누적용
    [Header("physic2D overlap 딜레이 시간")] public float generateInterval2; // 간격
    public LayerMask layerMask;
    [Header("한번 실행 될 때 몇번 반복할건지( physic2D overlap 전용) ")] public int loopValue;

    public bool generateCube;
    public bool generatephysics;

    private Collider2D []results = new Collider2D[10];//추가한 부분
    ContactFilter2D filter = new ContactFilter2D();//추가한 부분
 
    void Start()
    { 
        //급하게 해서 이부분은 아직 잘 모름
        filter.SetLayerMask(layerMask);
        filter.useTriggers = false;   // 필요에 따라
        filter.useDepth = false;
        /// 추가한 부분

        prefab = createCube();
        prefab.AddComponent<Rigidbody>();
        prefab.AddComponent<BoxCollider>();
        prefab.AddComponent<MeshRenderer>();
        mr = prefab.GetComponent<MeshRenderer>();
        mr.material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        mat = mr.material;
        mr.material.color = Color.red;
        Instantiate(prefab, transform);

    }


    void Update()
    {
        using (Physic2DMaker.Auto())
        {
            TestPhysic2D();
        }
        generateTimer += Time.deltaTime;
        using (CubeGenerateMaker.Auto())
        {
            TestCubeGenerate();

        }


        //using (AutoMaker.Auto())
        //{
        //    GenerateObj();
        //}

    }

    private void TestPhysic2D()
    {
        if (generatephysics == false) return;
        if (generateTimer >= generateInterval2)
        {
            generateTimer = 0f;


            for (int i = 0; i < loopValue; i++)
            {
                // 랜덤 위치에 OverlapCircle 수행
                Vector2 randomPos = (Vector2)transform.position + Random.insideUnitCircle * 50f;

                 Physics2D.OverlapBox(randomPos, new Vector2(10f, 10f), 0f, layerMask); //<--내부에서 New Collider2D해줌

                //var test =Physics2D.OverlapBox( //<--filter에서 레이어마스크 참조해서 검사 후  results 저장해준다.
                //              randomPos,
                //              new Vector2(10f, 10f),
                //              0f,
                //              filter,
                //              results
                //               );
            }

        }
    }

    private void TestCubeGenerate()
    {
        if (generateCube == false) return;
        if (generateTimer >= generateInterval)
        {
            generateTimer = 0f;


            GenerateObj();

        }
    }

    private void Deubug()
    {
        Debug.Log("Test");
    }
    private void GenerateObj()
    {
        GameObject go = Instantiate(prefab, transform);
        go.transform.position = transform.position;
        mr = go.GetComponent<MeshRenderer>();
        mr.material = mat;
        mr.material.color = Random.ColorHSV();
      
        Destroy(go, deleteTime);
    }

    GameObject createCube()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = transform.position;
        return cube;
    }
}
[CustomEditor(typeof(gametest))]
public class gametestEdit : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var Data = target as gametest;
        string name = Data.generateCube ? "큐브 오브젝트 무한 생성 활성화" : "큐브 오브젝트 무한 비활성화";
        string name2 = Data.generatephysics ? "Physic2D 병목 일으키기 활성화" : "Physic2D 병목 일으키기 비활성화";
        if (GUILayout.Button(name))
        {
            Data.generateCube = !Data.generateCube;



        }
        if (GUILayout.Button(name2))
        {


            Data.generatephysics = !Data.generatephysics;
        }
         
    }
}

//아래는 오브젝트 풀링코드

//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using Unity.Profiling;

//public class gametest : MonoBehaviour
//{
//    public GameObject prefab;
//    MeshRenderer mr = null;
//    Material mat = null;
//    public SceneLabel sceneLabel;
//    private static readonly ProfilerMarker CubeGenerateMaker = new("CubeMaker");
//    private static readonly ProfilerMarker Physic2DMaker = new("Physic2DMaker");

//    private float generateTimer = 0f;
//    [Header("큐브 무한 생성 딜레이 시간")] public float generateInterval;
//    [Header("큐브 삭제 시간(풀링에서는 비활성화 딜레이)")] public float deleteTime;

//    private float generateTimer2 = 0f;
//    [Header("physic2D overlap 딜레이 시간")] public float generateInterval2;
//    public LayerMask layerMask;
//    [Header("한번 실행 될 때 몇번 반복할건지( physic2D overlap 전용) ")] public int loopValue;

//    public bool generateCube;
//    public bool generatephysics;

//    private int maxCubeCount = 100; // 최대 생성 큐브 수
//    private List<GameObject> cubePool = new List<GameObject>();
//    private int currentCubeIndex = 0;
//    static readonly Collider2D[] buffer= new Collider2D[50];  
//    void Start()
//    {

//        prefab = createCube();
//        prefab.AddComponent<Rigidbody>();
//        prefab.AddComponent<BoxCollider>();
//        prefab.AddComponent<MeshRenderer>();
//        mr = prefab.GetComponent<MeshRenderer>();
//        mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
//        mat.enableInstancing = true;
//        mr.sharedMaterial = mat;
//        mr.material.color = Color.red;

//        // 풀 초기화: 큐브 100개 미리 생성, 비활성화
//        for (int i = 0; i < maxCubeCount; i++)
//        {
//            GameObject go = Instantiate(prefab, transform);
//            go.SetActive(false);
//            cubePool.Add(go);
//        }
//    }

//    void Update()
//    {
//        using (Physic2DMaker.Auto())
//        {
//            TestPhysic2D();
//        }

//        generateTimer += Time.deltaTime;

//        using (CubeGenerateMaker.Auto())
//        {
//            TestCubeGenerate();
//        }
//    }

//    private void TestPhysic2D()
//    {
//        if (!generatephysics) return;
//        if (generateTimer >= generateInterval2)
//        {
//            generateTimer = 0f;
//            //for (int i = 0; i < loopValue; i++)
//            //{
//            //    Vector2 randomPos = (Vector2)transform.position + Random.insideUnitCircle * 50f;
//            //    Physics2D.OverlapBox(randomPos, new Vector2(10f, 10f), 0f, layerMask);
//            //}

//            int hits = Physics2D.OverlapBox(
//                new Vector2(10f, 10f),
//                new Vector2(10f, 10f),
//                0f,
//                new ContactFilter2D().NoFilter(),  // 전체 검색
//                buffer
//                );
//        }
//    }

//    private void TestCubeGenerate()
//    {
//        if (!generateCube) return;
//        if (generateTimer >= generateInterval)
//        {
//            generateTimer = 0f;
//            GenerateObj();
//        }
//    }

//    private void GenerateObj()
//    {
//        GameObject go;

//        // 풀에 남은 오브젝트 재활용
//        if (currentCubeIndex < maxCubeCount)
//        {
//            go = cubePool[currentCubeIndex];
//            currentCubeIndex++;
//        }
//        else
//        {
//            // 이미 100개 이상이면 풀에서 순환 재사용
//            go = cubePool[currentCubeIndex % maxCubeCount];
//            currentCubeIndex++;
//        }

//        go.transform.position = transform.position;
//        go.SetActive(true);

//        mr = go.GetComponent<MeshRenderer>();
//        mr.sharedMaterial = mat;

//        // 색깔 변경 - MaterialPropertyBlock 사용해 instancing 유지
//        MaterialPropertyBlock block = new MaterialPropertyBlock();
//        block.SetColor("_BaseColor", Random.ColorHSV());
//        mr.SetPropertyBlock(block);

//        // 삭제 타이머 시작 (Destroy 대신 비활성화)
//        StartCoroutine(DisableAfterDelay(go, deleteTime));
//    }

//    private System.Collections.IEnumerator DisableAfterDelay(GameObject obj, float delay)
//    {
//        yield return new WaitForSeconds(delay);
//        obj.SetActive(false);
//    }

//    GameObject createCube()
//    {
//        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
//        cube.transform.position = transform.position;
//        return cube;
//    }
//}

//[CustomEditor(typeof(gametest))]
//public class gametestEdit : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();
//        var Data = target as gametest;
//        string name = Data.generateCube ? "큐브 오브젝트 무한 생성 활성화" : "큐브 오브젝트 무한 비활성화";
//        string name2 = Data.generatephysics ? "Physic2D 병목 일으키기 활성화" : "Physic2D 병목 일으키기 비활성화";

//        if (GUILayout.Button(name))
//        {
//            Data.generateCube = !Data.generateCube;
//        }
//        if (GUILayout.Button(name2))
//        {
//            Data.generatephysics = !Data.generatephysics;
//        }
//        if (GUILayout.Button("타이머 초기화"))
//        {
//            if (Data.sceneLabel != null)
//                Data.sceneLabel.time = 0f;
//        }
//    }
//}


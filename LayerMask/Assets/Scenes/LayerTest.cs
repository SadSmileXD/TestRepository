using UnityEngine;
using UnityEditor;
public class LayerTest : MonoBehaviour
{
    [SerializeField]LayerMask mask;
    [SerializeField] private int layertoname;
    [SerializeField] private string nameToLayer;
    [SerializeField] private string[] masks; 
    public void ddd()
    {
        Debug.Log(mask.value);
    }
    public void LayerToName()
    {
        Debug.Log($"LayerToName(int) : {LayerMask.LayerToName(layertoname)}");
      
    }
    public void NameToLayer()
    {
        Debug.Log($"NameToLayer(string) : {LayerMask.NameToLayer(nameToLayer)}");

    }
    public void GetMask()
    {
        int data = 0;
        foreach( var mask in masks )
        {
            data += LayerMask.GetMask(mask);
            Debug.Log($"GetMask(string) : {LayerMask.GetMask(mask)}");
            
        }
        Debug.Log($"합산{data}");

    }
}
[CustomEditor(typeof(LayerTest))]
public class LayerTestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // 기본 인스펙터 그리기
        DrawDefaultInspector();

        LayerTest layerTest = (LayerTest)target;

        GUILayout.Space(10);
        if(GUILayout.Button("LayerToName"))
        {
            layerTest.LayerToName();
        }
        if (GUILayout.Button("NameToLayer"))
        {
            layerTest.NameToLayer();
        }
        if (GUILayout.Button("GetMask"))
        {
            layerTest.GetMask();
        }
        if (GUILayout.Button("value 호출"))
        {
            layerTest.ddd();
        }
    }
}
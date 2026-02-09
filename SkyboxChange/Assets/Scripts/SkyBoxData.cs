using UnityEngine;

[CreateAssetMenu(menuName = "SkyboxData/Generation", fileName ="SkyBox_")]
public class SkyBoxData :ScriptableObject
{
    [SerializeField] private Cubemap m_Cube;
    [SerializeField] private int m_Index;

    public Cubemap cube => m_Cube;
    public int index => m_Index;
}

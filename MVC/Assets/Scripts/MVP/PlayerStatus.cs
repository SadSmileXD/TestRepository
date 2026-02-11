using UnityEngine;
using UnityEditor;
public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private float Mp;
    [SerializeField] private float Hp;

    private const float MAXHP=100f;
    private const float MAXMP = 100f;

    public delegate void Player_Status_UI(float amount1, float amount2);

    public event Player_Status_UI OnStatus;
    private void Awake()
    {
        Mp = MAXMP;
        Hp = MAXHP;
    }

    public void OnDamage(float damage)
    {
        Hp-=damage;
        var uimp = Mp / MAXMP;
        var uihp = Hp / MAXHP;
        OnStatus?.Invoke(uihp,uimp);
    }

   
}

[CustomEditor(typeof(PlayerStatus))]
public class edit :Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var data = (PlayerStatus)target;
        if(GUILayout.Button("데미지 받기"))
        {
            data.OnDamage(10f);
        }
    }
}
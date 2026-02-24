using UnityEngine;
using UnityEngine.AI; // NavMesh 사용을 위해 필수

public class EnemyFollow : MonoBehaviour
{
    public Transform target; // 추적할 대상 (플레이어)
    private NavMeshAgent agent;

    void Start()
    {
        // 내 오브젝트의 NavMeshAgent를 가져옴
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (target != null)
        {
            // 매 프레임마다 타겟의 위치를 목적지로 갱신
            agent.SetDestination(target.position);
        }
    }
}
using UnityEngine;
using UnityEngine.Pool;
public class PoolTest : MonoBehaviour
{
    [SerializeField]private IObjectPool<GameObject> _pool;
    void Start()
    {
        _pool=new ObjectPool<GameObject>(
            createFunc:()=>new GameObject(),
            actionOnGet:obj=>obj.SetActive(true),
            actionOnRelease:obj=>obj.SetActive(false),
            actionOnDestroy:obj=>Destroy(obj),
            collectionCheck:true,
            defaultCapacity:10,
            maxSize:100
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

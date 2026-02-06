using UnityEngine;

public class ParentScripts : MonoBehaviour
{
    void OnJump()
    {
        Debug.Log("<color=yellow>[부모]</color> Jump 메세지 호출!");
    }
    void OnMove()
    {
        Debug.Log("<color=yellow>[부모]</color> OnMove 메세지 호출!");
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

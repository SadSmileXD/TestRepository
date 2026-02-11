using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    [SerializeField] private Animator animator;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            animator.SetTrigger("Attack");
        }
        if (  Input.GetKey(KeyCode.UpArrow))
        {
           
            animator.SetBool("OnRun", true);
            this.transform.position += this.transform.forward * Time.deltaTime;
        }
        else
        {
            animator.SetBool("OnRun", false);
        }
    }
   
}

using UnityEngine;
using static UnityEditor.Progress;

public class ranPos : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -6f)
        {
            var randomX = Random.Range(-7f, 8f);
            var randomY = Random.Range(6f, 15f);
            this.transform.position = new Vector2(randomX, randomY);
            var data =  GetComponent<Rigidbody2D>();
            data.linearVelocity = Vector2.zero;
            data.gravityScale = Random.value;
        }
       
    }
}

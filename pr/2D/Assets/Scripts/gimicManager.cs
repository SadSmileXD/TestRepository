using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class gimicManager : MonoBehaviour
{
    [SerializeField] private GameObject data;
    [SerializeField] private List<GameObject> GList;
    void Start()
    {
        for(int i=0; i<10; i++)
        {
            var randomX = Random.Range(-6f, 6f);
            var pos = transform.position;
            pos.x = randomX;
            GameObject obj = Instantiate(data, pos, Quaternion.identity);
           
            //obj.SetActive(false);
            GList.Add(obj);

        }
    }

    // Update is called once per frame
    void Update()
    {

        
    }
}

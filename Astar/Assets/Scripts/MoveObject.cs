using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class MoveObject : MonoBehaviour
{
    public GameManager manager;
    public int MoveSpeed;
    public List<Node> wayPoint=new List<Node>();
 

   public void OnMove()
    {
        wayPoint = manager.FinalNodeList;
        
        StartCoroutine(Move());
    } 
    private IEnumerator Move()
    {
        int CurrentNodeIndex=0;
        var node = wayPoint[CurrentNodeIndex];

         
        while (true)
        {
            Vector2 targetPosition = new Vector2(node.x, node.y);
            Vector2 currentPosition = this.transform.position;
            this.transform.position =Vector2.MoveTowards(currentPosition, targetPosition, MoveSpeed * Time.deltaTime);
           
            if (Vector3.Distance(this.transform.position, new Vector3(node.x, node.y, 0)) <= 0.1f)
            {
                CurrentNodeIndex++;
                if (CurrentNodeIndex >= wayPoint.Count)
                {
                    break;
                }
                node = wayPoint[CurrentNodeIndex];
            }
            yield return new WaitForSeconds(0.1f);
        }
         
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class User
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Role { get; set; }
}
public class Test : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        List<User> users = new List<User> 
        {
           new User { Name = "김철수", Age = 15, Role = "User" },
           new User { Name = "이영희", Age = 25, Role = "Admin" },
           new User { Name = "박민수", Age = 32, Role = "User" },
           new User { Name = "최수지", Age = 22, Role = "User" }
        };
        var result = users.Where(x => x.Age > 20);
    }

   
}

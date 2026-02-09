using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public LayerMask mask;
    public LayerMask mask2;
    private void Start()
    {
       
        mask.TContaions(mask2);
        Debug.Log(mask.value);
    }
}

public static  class Extenction
{
    public static bool TContaions<T>(this T value ,T value2)
    {
        return EqualityComparer<T>.Default.Equals(value, value2);
    }
}
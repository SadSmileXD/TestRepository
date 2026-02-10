using UnityEngine;

public static class extensionMethod
{
    public static bool IsNUll<T>(this T Value) 
    {
        if(Value is null)
        {
            return true;
        }
        
        return false;
    }
}

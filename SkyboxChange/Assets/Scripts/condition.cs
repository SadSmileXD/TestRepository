using System;
using UnityEngine;

public abstract class condition :ScriptableObject
{
    public abstract void Init(MonoBehaviour Owner, params object[] Data);

    public abstract void OnCheck(Action OnChange);

}

using UnityEngine;
[System.Serializable]
public abstract class Basedata : ISheetParsable, IIdentifiable
{
    
    [field: SerializeField] public string Name { get; set; }
    public abstract void ApplyRowData(string[] Data);
    
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class SheetDataSO<T> : BaseDataSO where T : class, IIdentifiable, ISheetParsable, new()
{

    private SheetLoader<T> data;
    [SerializeField] public List<T> Ldata = new List<T>();
    [ContextMenu("비동기 로드")]
    public override async Task InitAsync()
    {
        genericType = typeof(T).Name;
        data = new SheetLoader<T>(url, gid);

        
        Ldata = await data.GetDataAsync();
    }
    public T returnValue(string _keycode)
    {
        foreach (var item in Ldata)
        {
            var data = item as IIdentifiable;
            if(data ==null)return null;
            if (data.Name == _keycode)
            {
                return item;
            }
        }
        return null;
    }
    
}

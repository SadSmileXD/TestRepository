using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class CSV : MonoBehaviour
{
    string path;
    public List<ItemData> itemList = new List<ItemData>();
    void Awake()
    {
        path = Application.persistentDataPath + "/ItemSave.csv";
         Path.
    }
    private void Start()
    {
        LoadCSV();
    }
    void LoadCSV()
    {
        if (!File.Exists(path))
        {
            Debug.LogError("파일 없음: " + path);
            return;
        }

        string[] lines = File.ReadAllLines(path);

        for (int i = 1; i < lines.Length; i++) // 헤더 제외
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
                continue;

            string[] values = lines[i].Trim().Split(',');

            if (values.Length < 4)
                continue;

            ItemData data = new ItemData();

            int.TryParse(values[0], out data.ID);
            data.Name = values[1];
            int.TryParse(values[2], out data.Attack);
            int.TryParse(values[3], out data.Price);

            itemList.Add(data);
        }

        Debug.Log("로드 완료: " + itemList.Count);
    }


}

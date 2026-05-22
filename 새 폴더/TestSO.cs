using Firebase.Firestore;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;
[CreateAssetMenu(fileName = "TestSO", menuName = "ScriptableObjects/TestSO")]

public class TestSO : BaseFireStore
{
    public override void InitDataBase(FirebaseFirestore database)
    {
      this.db = database;
    }
    public override Task DeleteDataAsync()
    {
        return null;
    }

    public override Task<DocumentSnapshot> GetSnapshotAsync()
    {
        return db.Collection("Test").Document("TestDoc").GetSnapshotAsync();
    }

  

    public override async Task SetDataAsync(object data)
    { 
        await db.Collection("Userssssssss").Document("TestDoc").SetAsync(data);
          
    }

    public override async Task UpdateDataAsync(object data)
    {
        throw new System.NotImplementedException();
    }
    public override Task<DocumentSnapshot> GetSnapshotAsync<T>()
    {
        return null;
    }
}

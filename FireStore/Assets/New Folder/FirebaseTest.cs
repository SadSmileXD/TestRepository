using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine;

public class FirebaseTest : MonoBehaviour
{
    private FirebaseFirestore db;
    private void Start()
    {
      InitFirebase();
     
        
    }

    private async Task InitFirebase()
    {
        await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(async task =>
        {
            DependencyStatus status = task.Result;

            if (status == DependencyStatus.Available)
            {
                Debug.Log("Firebase 초기화 성공");

                db = FirebaseFirestore.DefaultInstance;
                UserData userData = new UserData
                {
                    gold = 100,
                    name = "Player001",
                    level = 5
                };
               await db.Collection("Users").Document("user_002").SetAsync(userData);
            }
            else
            {
                Debug.LogError($"Firebase 초기화 실패: {status}");
            }
        });
    }

    [ContextMenu("Create User Data")]
    public async void CreateData()
    {
        Dictionary<string, object> data = new Dictionary<string, object>
        {
             { "gold", 100 } // 정수(Int32)를 딕셔너리로 감싸줍니다.
        };
        CollectionReference DocRef = db.Collection("Users");

        QuerySnapshot snapshot = await DocRef.GetSnapshotAsync();
        
        foreach (DocumentSnapshot item in snapshot.Documents)
        {
            Debug.Log("문서 ID: " + item.Id);

            if (item.ContainsField("gold"))
            {
                int gold = item.GetValue<int>("gold");
                Debug.Log("gold: " + gold);
            }
        }


    }
    [ContextMenu("Updatedata User Data")]
    public async void Updatedata()
    {
        Dictionary<string, object> data = new Dictionary<string, object>
        {
             { "gold", 500 } // 정수(Int32)를 딕셔너리로 감싸줍니다.
        };
        var DocRef = db.Collection("Users").Document("user_002");
        await DocRef.UpdateAsync(data);
    }

    /*

       private async Task CreateUserData(string userId)
       {
           DocumentReference docRef = db
               .Collection("Users")
               .Document(userId);

           Dictionary<string, object> userData = new Dictionary<string, object>
           {
               { "name", "GangSuli2" },
               { "level", 12 },
               { "gold", 10000 },
               { "createdAt", Timestamp.GetCurrentTimestamp() }
           };

           await docRef.SetAsync(userData);

           Debug.Log("유저 데이터 등록 성공");
       }

       private async Task AddInventoryItems(string userId)
       {
           Dictionary<string, object> sword = new Dictionary<string, object>
           {
               { "itemName", "Iron Sword" },
               { "type", "Weapon" },
               { "count", 1 },
               { "damage", 15 }
           };

           Dictionary<string, object> potion = new Dictionary<string, object>
           {
               { "itemName", "Health Potion" },
               { "type", "Consumable" },
               { "count", 5 },
               { "heal", 50 }
           };

           await db.Collection("Users")
               .Document(userId)
               .Collection("Inventory")
               .Document("item_001")
               .SetAsync(sword);

           await db.Collection("Users")
               .Document(userId)
               .Collection("Inventory")
               .Document("item_002")
               .SetAsync(potion);

           Debug.Log("아이템 여러 개 저장 완료");
       }

       [ContextMenu("Update User Data")]
       public void UpdateUserData()
       {
           if (db == null)
           {
               Debug.LogError("Firestore가 아직 초기화되지 않았습니다.");
               return;
           }

           string userId = "user_001";

           DocumentReference docRef = db
               .Collection("Users")
               .Document(userId);

           Dictionary<string, object> updateData = new Dictionary<string, object>
           {
               { "level", 2 },
               { "gold", 5000 },
               { "updatedAt", Timestamp.GetCurrentTimestamp() }
           };

           docRef.UpdateAsync(updateData).ContinueWithOnMainThread(task =>
           {
               if (task.IsCompletedSuccessfully)
               {
                   Debug.Log("유저 데이터 업데이트 성공");
               }
               else
               {
                   Debug.LogError($"유저 데이터 업데이트 실패: {task.Exception}");
               }
           });
       }

       [ContextMenu("Load User Data")]
       public void LoadUserData()
       {
           if (db == null)
           {
               Debug.LogError("Firestore가 아직 초기화되지 않았습니다.");
               return;
           }

           string userId = "user_001";

           DocumentReference docRef = db
               .Collection("Users")
               .Document(userId);

           docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
           {
               if (task.IsCompletedSuccessfully == false)
               {
                   Debug.LogError($"유저 데이터 불러오기 실패: {task.Exception}");
                   return;
               }

               DocumentSnapshot snapshot = task.Result;

               if (snapshot.Exists == false)
               {
                   Debug.LogWarning("해당 유저 데이터가 없습니다.");
                   return;
               }

               string name = snapshot.GetValue<string>("name");
               int level = snapshot.GetValue<int>("level");
               int gold = snapshot.GetValue<int>("gold");

               Debug.Log($"이름: {name}");
               Debug.Log($"레벨: {level}");
               Debug.Log($"골드: {gold}");
           });
       }

       [ContextMenu("Load Inventory")]
       public void LoadInventory()
       {
           if (db == null)
           {
               Debug.LogError("Firestore가 아직 초기화되지 않았습니다.");
               return;
           }

           string userId = "user_001";

           db.Collection("Users")
               .Document(userId)
               .Collection("Inventory")
               .GetSnapshotAsync()
               .ContinueWithOnMainThread(task =>
               {
                   if (task.IsCompletedSuccessfully == false)
                   {
                       Debug.LogError($"인벤토리 불러오기 실패: {task.Exception}");
                       return;
                   }

                   QuerySnapshot snapshot = task.Result;

                   foreach (DocumentSnapshot doc in snapshot.Documents)
                   {
                       string itemName = doc.GetValue<string>("itemName");
                       string type = doc.GetValue<string>("type");
                       int count = doc.GetValue<int>("count");

                       Debug.Log($"아이템ID: {doc.Id}, 이름: {itemName}, 타입: {type}, 개수: {count}");
                   }
               });
       }
       */
}

[FirestoreData]
public class UserData
{
    [FirestoreProperty]
    public int gold { get; set; }

    [FirestoreProperty]
    public string name { get; set; }

    [FirestoreProperty]
    public int level { get; set; }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class SaveManager : MonoBehaviour
{
    public ShopManager SM;
    public GameManager GM;

    string filePath;

    public static SaveManager Instance;

    private DatabaseReference db_ref;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        Instance.db_ref = FirebaseDatabase.DefaultInstance.GetReference("users/testo");

        GM = FindObjectOfType<GameManager>();
        filePath = Application.persistentDataPath + "data.gamesave";

        LoadGame();
        SaveGame();
    }

    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(filePath, FileMode.Create);

        Save save = new Save();
        save.coins = GM.Coins;
        save.active_skin_index = (int)SM.ActivateSkin;
        save.SaveBoughtItems(SM.Items);

        bf.Serialize(fs, save);
        fs.Close();
    }

    public async void LoadGame()
    {

        await Instance.db_ref
        .GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted) {
            // Handle the error...
            }
            else if (task.IsCompleted) {
                DataSnapshot snapshot = task.Result;
                Debug.Log(snapshot.GetRawJsonValue());
                Save save = JsonUtility.FromJson<Save>(snapshot.GetRawJsonValue());

                GM.Coins = save.coins;
                SM.ActivateSkin = (ShopItem.ItemType)save.active_skin_index;

                for (int i = 0; i < save.bought_items.Count; i++)
                    SM.Items[i].IsBought = save.bought_items[i];

                GM.RefreshText();
                GM.ActivateSkin((int)SM.ActivateSkin);
            }
        });

        // if (!File.Exists(filePath))
        //     return;

        // BinaryFormatter bf = new BinaryFormatter();
        // FileStream fs = new FileStream(filePath, FileMode.Open);

        // Save save = (Save)bf.Deserialize(fs);


        // for (int i = 0; i < save.bought_items.Count; i++)
        //     SM.Items[i].IsBought = save.bought_items[i];

        // fs.Close();
        // fs.Close();

    }
}

[System.Serializable]
public class Save
{
    public int coins;
    public int active_skin_index;
    public List<int> runs;
    public List<bool> bought_items = new List<bool>();
    public void SaveBoughtItems(List<ShopItem> items)
    {
        foreach (var item in items)
            bought_items.Add(item.IsBought);
    }


}

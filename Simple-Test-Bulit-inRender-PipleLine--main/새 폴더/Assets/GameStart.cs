using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameStart : MonoBehaviour
{
  
    public Button yourButton;
    public string sceneName;
    void Start()
    {
        yourButton.onClick.AddListener(() => LoadScene());
    }
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }

}

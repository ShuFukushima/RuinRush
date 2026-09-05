using UnityEngine;
using UnityEngine.SceneManagement;

public class PrototypeMainMenuManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ゲームスタートボタンが押された際の処理
    /// </summary>
    public void OnClickMainMenuButton()
    {
        SceneManager.LoadScene("Game");
    }
}

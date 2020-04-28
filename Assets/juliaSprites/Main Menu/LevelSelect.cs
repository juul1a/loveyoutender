using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    public Button[] LevelButtons;

    void Start(){
        int levelReached = PlayerPrefs.GetInt("levelReached", 1);
        LevelButtons = gameObject.GetComponentsInChildren<Button>();
        for (int i = 0 ; i < LevelButtons.Length; i++)
        {
            if(i+1>levelReached){
                LevelButtons[i].interactable = false;
            }
        }
    }

    public void Select(string levelName){
        SceneManager.LoadScene(levelName);
    }
}

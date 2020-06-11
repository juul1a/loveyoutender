using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour
{
    public enum State {Start, Playing, Win, Lose};

    State currentState;
    State prevState;

    [SerializeField] GameObject menuCanvas;
    GameObject startScreen, winScreen, loseScreen, HUD;

    // Start is called before the first frame update
    void Awake()
    {
        prevState = State.Start;
        currentState = State.Start;
        startScreen = menuCanvas.transform.Find("Start Screen") ? menuCanvas.transform.Find("Start Screen").gameObject : null;
        if(startScreen == null){
            currentState = State.Playing;
        }
        winScreen = menuCanvas.transform.Find("Win Screen").gameObject;
        loseScreen = menuCanvas.transform.Find("Lose Screen").gameObject;
        HUD = menuCanvas.transform.Find("HUD").gameObject;
        menuCanvas.SetActive(true);
        if(startScreen){
            startScreen.SetActive(true);
        }
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
        HUD.SetActive(false);
    }

    public State GetState(){
        return currentState;
    }

    public bool IsPlaying(){
        if(currentState == State.Playing){
            return true;
        }
        else{
            return false;
        }
    }

    public void SetState(State state){
            Debug.Log("Setting state to "+state);
            currentState = state;
            // DoState(currentState);
    }
    
    // Update is called once per frame
    void Update()
    {
        //Do state function right when it updates
        if(currentState != prevState){
            DoState(currentState);
            prevState = currentState;
        }
    }

    void DoState(State state){
        switch(state){
            case State.Start:
                // menuCanvas.SetActive(true);
                if(startScreen){
                    startScreen.SetActive(true);
                }
                winScreen.SetActive(false);
                loseScreen.SetActive(false);
                HUD.SetActive(false);
            break;
            case State.Lose:
                // menuCanvas.SetActive(true);
                if(startScreen){
                    startScreen.SetActive(false);
                }
                winScreen.SetActive(false);
                loseScreen.SetActive(true);
                HUD.SetActive(false);
            break;
            case State.Playing:
                // menuCanvas.SetActive(false);
                HUD.SetActive(true);
                if(startScreen){
                    startScreen.SetActive(false);    
                }
                winScreen.SetActive(false);
                loseScreen.SetActive(false);
            break;
            case State.Win:
                // menuCanvas.SetActive(false);
                if(startScreen){
                    startScreen.SetActive(false);
                }
                winScreen.SetActive(true);
                loseScreen.SetActive(false);
                HUD.SetActive(true);
            break;
            default:
            break;

        }
    }

    public void Play(){
        currentState = State.Playing;
    }

    public void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        currentState = State.Start;
    }

    public void Quit(){
        Application.Quit();
    }

    public void NextLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public enum Scene {
        Welcome, NewGame, InGame
    }

    public Dictionary<Scene, string> sceneName = new Dictionary<Scene, string>(){
        {Scene.Welcome, "welcome"},
        {Scene.NewGame, "newGame"},
        {Scene.InGame, "inGame"}
    };

    Player[] players = {ThiefController.instance};
    public static Player currentPlayer = null;
    public Scene currentScene;

    [SerializeField]
    string currentAction = "";


    [SerializeField]
    int round = 1;


    private static GameManager instance = null;
    public ButtonManager buttonManager;
    public BagController bagController;

    void Awake() {
        Debug.Log("Awake");
        //get current scene name
        currentScene = SceneManager.GetActiveScene().name == "welcome" ? Scene.Welcome : Scene.NewGame;
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            DoSceneInit();
        }
        else{
            instance.currentScene = currentScene;
            DoSceneInit();
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        string name = SceneManager.GetActiveScene().name;
        currentScene = sceneName.FirstOrDefault(x => x.Value == name).Key;

        if (currentScene == Scene.InGame){
            TMP_Text NowRound = GameObject.Find("Now Round").GetComponent<TMP_Text>();
            NowRound.text = round.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentScene){
            case Scene.Welcome:
                if (Input.GetMouseButtonDown(0)){
                    ChangeToNextScene(Scene.NewGame);
                }
                break;
            case Scene.NewGame:
                if (buttonManager.IsButtonClicked("A_attack")){
                    currentAction = "A";
                }
                if (buttonManager.IsButtonClicked("B_attack")){
                    currentAction = "B";
                }

                if (currentAction != ""){
                    ChangeToNextScene(Scene.InGame);
                }
                break;
            case Scene.InGame:
                // something temp work
                if (currentAction == ""){
                    currentAction = "A";
                }
                // not forget to del

                TMP_Text NowAction = GameObject.Find("Now Action").GetComponent<TMP_Text>();
                NowAction.text = currentAction;
                TMP_Text ThiefHP = GameObject.Find("Thief HP").GetComponent<TMP_Text>();
                ThiefHP.text = ThiefController.instance._hp.ToString();

                if (bagController.currentBagItems.Count == 0)
                {
                    ThiefController.instance.OpenMove();
                }

                if (round > 10 || ThiefController.instance._hp <= 0){
                    ChangeToNextScene(Scene.Welcome);
                }

                if (Input.GetKeyDown("p")){
                    TMP_Text NowRound = GameObject.Find("Now Round").GetComponent<TMP_Text>();
                    NowRound.text = round.ToString();
                    round++;
                }
                else if (Input.GetKeyDown("e")){
                    SwitchBag();
                }
                else if (Input.GetKeyDown("r")){
                    Dice.instance.RollDice();
                }
                else if (Input.GetKeyDown("t")){
                    ThiefController.instance._cost = Dice.instance.StopRollDice();
                }
                else if (Input.GetKeyDown("i")){
                    bagController.AddItem("CatCan");
                }
                break;
            default:
                break;
        }
    }

    public void SwitchBag(){

        if (bagController.bagState == BagState.Close){
            ThiefController.instance.CloseMove();
            bagController.Open();
        }
        else{
            ThiefController.instance.OpenMove();
            bagController.Close();
        }
    }

    private void ChangeToNextScene(Scene nextScene){
        currentScene = nextScene;
        SceneManager.LoadScene(sceneName[nextScene]);
    }
    private void DoSceneInit(){
        switch (instance.currentScene){
            case Scene.Welcome:
                Debug.Log("Welcome init");
                break;
            case Scene.NewGame:
                Debug.Log("NewGame init");
                break;
            case Scene.InGame:
                Debug.Log("InGame init");
                instance.bagController = GameObject.Find("Canvas/Bag").GetComponent<BagController>();
                currentPlayer = players[0];
                break;
            default:
                break;
        }

    }
}

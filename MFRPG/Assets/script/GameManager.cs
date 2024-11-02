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
using UnityEngine.UIElements;

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

    List<Player> players = new List<Player>();
    public Player currentPlayer = null;
    public Scene currentScene;

    [SerializeField]
    string currentAction = "";


    [SerializeField]
    int round = 1;


    public static GameManager instance = null;
    public ButtonManager buttonManager;

    void Awake() {
        Debug.Log("Awake");
        //get current scene name
        string name = SceneManager.GetActiveScene().name;
        currentScene = sceneName.FirstOrDefault(x => x.Value == name).Key;
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

                SetUI();
                ThiefAction();

                if (round > 10 || ThiefController.instance._hp <= 0){
                    ChangeToNextScene(Scene.Welcome);
                }

                if (Input.GetKeyDown("p")){
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
                    BagController.instance.AddItem("CatCan");
                }
                break;
            default:
                break;
        }
    }

    public void SwitchBag(){
        if (BagController.instance.bagState == BagState.Close){
            ThiefController.instance.CloseMove();
            BagController.instance.Open();
        }
        else{
            ThiefController.instance.OpenMove();
            BagController.instance.Close();
        }
    }

    public void ThiefAction(){
        if (BagController.instance.currentBagItems.Count == 0 && ThiefController.instance._cost > 0){
            BagController.instance.Close();
            ThiefController.instance.OpenMove();
        }
        else if (ThiefController.instance._cost <= 0){
            ThiefController.instance.CloseMove();
        }
        else if (BagController.instance.bagState == BagState.Close){
            ThiefController.instance.OpenMove();
        }
    }

    public void SetUI(){
        TMP_Text NowAction = GameObject.Find("Now Action").GetComponent<TMP_Text>();
        NowAction.text = currentAction;

        TMP_Text NowRound = GameObject.Find("Now Round").GetComponent<TMP_Text>();
        NowRound.text = round.ToString();
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
                instance.players.Add(ThiefController.instance);
                ThiefController.instance.SetPlayer("Thief", 10, 0, new List<Item>(), new List<PlayerBuff>());
                currentPlayer = players[0];
                break;
            default:
                break;
        }

    }
}

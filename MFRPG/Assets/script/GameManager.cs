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

    Player[] players = {new Player("A", 10, 20), new Player("B", 20, 30) };
    [SerializeField]
    public Scene currentScene;

    [SerializeField]
    string currentAction = "";

    [SerializeField]
    int round = 1;


    private static GameManager gameManager = null;
    public ButtonManager buttonManager;
    public GameObject bagController;
    
    public Sprite testSprite;
    void Awake() {
        DoSceneInit();
        if (gameManager == null) {
            gameManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        string name = SceneManager.GetActiveScene().name;
        currentScene = sceneName.FirstOrDefault(x => x.Value == name).Key;

        TMP_Text NowRound = GameObject.Find("Now Round").GetComponent<TMP_Text>();
        NowRound.text = round.ToString();
        print("start");
        testSprite = Resources.Load<Sprite>("image/youkai_jinmenken");
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

                if (round > 10){
                    ChangeToNextScene(Scene.Welcome);
                }
                else if (Input.GetKeyDown("p")){
                    TMP_Text NowRound = GameObject.Find("Now Round").GetComponent<TMP_Text>();
                    NowRound.text = round.ToString();
                    round++;
                }

                if (Input.GetKeyDown("e")){
                    BagController bagControllerC = bagController.GetComponent<BagController>();
                    if (bagControllerC.bagState == BagState.Close){
                        bagControllerC.Open();
                    }
                    else{
                        bagControllerC.Close();
                    }

                }
                break;
            default:
                break;
        }
    }

    private void ChangeToNextScene(Scene nextScene){
        currentScene = nextScene;
        SceneManager.LoadScene(sceneName[nextScene]);
    }
    private void DoSceneInit(){

    }
}

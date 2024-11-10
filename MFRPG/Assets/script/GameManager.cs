using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public enum Scene {
        Welcome, NewGame, InGame, WinGame
    }
    
    public enum InGameLoop{
        ThiefEvent , ThiefRollDice, ThiefAction, DefenderEvent,
        DefenderRollDice, DefenderAction, CatAction, RTATime, RoundStart
    }

    public Dictionary<Scene, string> sceneName = new Dictionary<Scene, string>(){
        {Scene.Welcome, "welcome"},
        {Scene.NewGame, "newGame"},
        {Scene.InGame, "inGame"},
        {Scene.WinGame, "WinGame"},
    };

    public Dictionary<InGameLoop, string> inGameLoopName = new Dictionary<InGameLoop, string>(){
        {InGameLoop.ThiefRollDice, "Attacker Roll Dice"},
        {InGameLoop.ThiefAction, "Attacker Action"},
        {InGameLoop.DefenderRollDice, "Defender Roll Dice"},
        {InGameLoop.DefenderAction, "Defender Action"},
        {InGameLoop.CatAction, "Cat Action"},
        {InGameLoop.RTATime, "RTA Time"},
        {InGameLoop.RoundStart, "Round Start"},
        {InGameLoop.ThiefEvent, "Thief Event"},
        {InGameLoop.DefenderEvent, "Defender Event"}
    };
    public InGameLoop currentInGameState;
    IState _inGameState;

    List<Player> players = new List<Player>();
    public Player currentPlayer = null;
    public Scene currentScene;
    public GameObject currentState;

    public string thisMatchThiefName = "";

    public int round = 1;
    public int playerAWinCount = 0;
    public int playerBWinCount = 0;

    public static GameManager instance = null;
    void Awake() {
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
            round = 1;
            NowRound.text = round.ToString();
        }
        triggerEnter = true;
        currentInGameState = InGameLoop.RoundStart;
        _inGameState = new RoundStart();
    }

    // Update is called once per frame
    void Update()
    {
        SetStateDebug();

        switch (currentScene){
            case Scene.Welcome:
                if (Input.anyKey){
                    ChangeToScene(Scene.NewGame);
                }
                break;
            case Scene.NewGame:
                if (NewGameController.instance.isGuideOver){
                    if (Input.GetKeyDown("a")){
                        thisMatchThiefName = "A";
                        ChangeToScene(Scene.InGame);
                    }
                    if (Input.GetKeyDown("b")){
                        thisMatchThiefName = "B";
                        ChangeToScene(Scene.InGame);
                    }
                }
                break;
            case Scene.InGame:
                if (playerAWinCount == 2 || playerBWinCount == 2){
                    ChangeToScene(Scene.WinGame);
                    return;
                }
                
                if (ThiefController.instance.CatWinCG.activeInHierarchy){
                    if (Input.GetKeyDown("n")){
                        thisMatchThiefName = thisMatchThiefName == "A" ? "B" : "A";
                        ChangeToScene(Scene.InGame);
                    }
                }
                else{
                    if (triggerEnter){
                        SetUI();
                        _inGameState?.OnEnter();
                        triggerEnter = false;
                    }
                    if (!BannerController.instance.animator.GetBool("IsShow")){
                        _inGameState.OnUpdate();
                    }

                    if (round > 10){
                        ThiefController.instance.CatWinCG.SetActive(true);
                    }
                }
                break;
            case Scene.WinGame:
                if (Input.GetKeyDown("n")){
                    ChangeToScene(Scene.Welcome);
                }
                break;
            default:
                break;
            }
        }
    
    // get item need after switch player
    public void SwitchPlayer(int index){
        BagController.instance.SaveCurrentPlayerBagItems();
        currentPlayer = players[index];
        currentPlayer._items.Add(BagController.instance.CreateRandomItemDTO());
        BagController.instance.SetCurrentBagItems();
        if (index == 0){
            GameObject.Find("Defender").GetComponent<SpriteRenderer>().color = Color.gray;
            GameObject.Find("ThiefActionImage").GetComponent<SpriteRenderer>().color = Color.white;
        }
        else{
            GameObject.Find("Defender").GetComponent<SpriteRenderer>().color = Color.white;
            GameObject.Find("ThiefActionImage").GetComponent<SpriteRenderer>().color = Color.gray;
        }
    }

    public void SwitchBag(){
        if (BagController.instance.bagState == BagState.Close){
            BagController.instance.Open();
            ThiefController.instance.CloseMove();
        }
        else{
            BagController.instance.Close();
            ThiefController.instance.OpenMove();
        }

        if (currentInGameState != InGameLoop.ThiefAction){
            ThiefController.instance.CloseMove();
        }
    }

    public void SetUI(){

        TMP_Text NowRound = GameObject.Find("Now Round").GetComponent<TMP_Text>();
        NowRound.text = round.ToString();

        TMP_Text NowState = currentState.GetComponent<TMP_Text>();
        NowState.text = inGameLoopName[currentInGameState];

        TMP_Text BannerText = GameObject.Find("BannerText").GetComponent<TMP_Text>();
        if (inGameLoopName[currentInGameState] == inGameLoopName[InGameLoop.RoundStart]){
            BannerText.text = "Round " + round.ToString();
        }
        else{
            BannerText.text = inGameLoopName[currentInGameState];
        }

        TMP_Text Score = GameObject.Find("Score").GetComponent<TMP_Text>();
        Score.text = "A " + playerAWinCount.ToString() + " : " + playerBWinCount.ToString() + " B";
    }

    public void SetStateDebug(){
        if (Input.GetKeyDown(KeyCode.Tab)){
            if (currentState.activeInHierarchy){
                currentState.SetActive(false);
            }
            else{
                currentState.SetActive(true);
            }
        }
    }

    public void ChangeToScene(Scene nextScene){
        currentScene = nextScene;
        SceneManager.LoadScene(sceneName[nextScene]);
    }
    private void DoSceneInit(){
        instance.currentState = GameObject.Find("Now State");
        instance.currentState.SetActive(false);
        switch (instance.currentScene){
            case Scene.Welcome:
                Debug.Log("Welcome init");
                break;
            case Scene.NewGame:
                Debug.Log("NewGame init");
                break;
            case Scene.InGame:
                instance.currentInGameState = InGameLoop.RoundStart;
                instance._inGameState = new RoundStart();
                instance.triggerEnter = true;

                instance.round = 1;
                instance.players.Clear();

                instance.players.Add(ThiefController.instance);
                instance.players.Add(DefenderController.instance);

                ThiefController.instance.SetPlayer("Thief", 10, 0, 2, new List<ItemDTO>());
                ThiefController.instance._items.Add(BagController.instance.CreateRandomItemDTO());
                ThiefController.instance._items.Add(BagController.instance.CreateRandomItemDTO());
                ThiefController.instance._items.Add(BagController.instance.CreateRandomItemDTO());
                ThiefController.instance._items.Add(BagController.instance.CreateRandomItemDTO());

                DefenderController.instance.SetPlayer("Defender", 10, 0, 3, new List<ItemDTO>());
                DefenderController.instance._items.Add(BagController.instance.CreateRandomItemDTO());
                DefenderController.instance._items.Add(BagController.instance.CreateRandomItemDTO());
                DefenderController.instance._items.Add(BagController.instance.CreateRandomItemDTO());
                DefenderController.instance._items.Add(BagController.instance.CreateRandomItemDTO());

                instance.currentPlayer = instance.players[0];
                BagController.instance.SetCurrentBagItems();

                TMP_Text nowThief = GameObject.Find("Now Thief").GetComponent<TMP_Text>();
                nowThief.text = instance.thisMatchThiefName;
                TMP_Text nowDefender = GameObject.Find("Now Defender").GetComponent<TMP_Text>();
                nowDefender.text = instance.thisMatchThiefName == "A" ? "B" : "A";

                break;
            default:
                break;
        }
    }

    private bool triggerEnter;
    public void GotoState(IState status){
        _inGameState?.OnExit();
        _inGameState = status;
        triggerEnter = true;
    }

    public void WinnerAddScore(string name){
        if (name == "Thief"){
            if (thisMatchThiefName == "A"){
                playerAWinCount += 1;
            }
            else{
                playerBWinCount += 1;
            }
        }
        else{
            if (thisMatchThiefName == "A"){
                playerBWinCount += 1;
            }
            else{
                playerAWinCount += 1;
            }
        }
    }

}

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Data.Common;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public enum Scene {
        Welcome, NewGame, InGame
    }
    
    public enum InGameLoop{
        GetRandomItem, ThiefRollDice, ThiefAction, 
        DefenderRollDice, DefenderAction, CatAction, RTATime, RoundStart
    }

    public Dictionary<Scene, string> sceneName = new Dictionary<Scene, string>(){
        {Scene.Welcome, "welcome"},
        {Scene.NewGame, "newGame"},
        {Scene.InGame, "inGame"}
    };

    public Dictionary<InGameLoop, string> inGameLoopName = new Dictionary<InGameLoop, string>(){
        {InGameLoop.GetRandomItem, "GetRandomItem"},
        {InGameLoop.ThiefRollDice, "ThiefRollDice"},
        {InGameLoop.ThiefAction, "ThiefAction"},
        {InGameLoop.DefenderRollDice, "DefenderRollDice"},
        {InGameLoop.DefenderAction, "DefenderAction"},
        {InGameLoop.CatAction, "CatAction"},
        {InGameLoop.RTATime, "RTATime"},
        {InGameLoop.RoundStart, "RoundStart"}
        
    };
    public InGameLoop currentInGameState;
    IState _inGameState;

    List<Player> players = new List<Player>();
    public Player currentPlayer = null;
    public Scene currentScene;
    public GameObject currentState;

    string thisMatchThiefName = "";

    public int round = 1;

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
                if (Input.GetKeyDown("space")){
                    ChangeToNextScene(Scene.NewGame);
                }
                break;
            case Scene.NewGame:
                if (Input.GetKeyDown("a")){
                    thisMatchThiefName = "A";
                    ChangeToNextScene(Scene.InGame);
                }
                if (Input.GetKeyDown("b")){
                    thisMatchThiefName = "B";
                    ChangeToNextScene(Scene.InGame);
                }
                break;
            case Scene.InGame:
                // something temp work
                if (thisMatchThiefName == ""){
                    thisMatchThiefName = "A";
                }
                if (Input.GetKeyDown("q")){
                    instance.round = 9;
                }
                if (Input.GetKeyDown("1")){
                    Debug.Log(currentInGameState);
                }
                // not forget to del
                
                if (triggerEnter){
                    Debug.Log("triggerEnter" + _inGameState);
                    SetUI();
                    _inGameState?.OnEnter();
                    triggerEnter = false;
                }
                if (!BannerController.instance.animator.GetBool("IsShow")){
                    _inGameState.OnUpdate();
                }


                if (round > 10 || ThiefController.instance._hp <= 0){
                    ChangeToNextScene(Scene.Welcome);
                }
                break;
            default:
                break;
            }
        }

    public void NextInGameLoop(){
        switch (currentInGameState){
            case InGameLoop.RoundStart:
                currentInGameState = InGameLoop.ThiefRollDice;
                break;
            case InGameLoop.ThiefRollDice:
                if (ThiefController.instance.thiefState == ThiefState.Skip){
                    currentInGameState = InGameLoop.DefenderRollDice;
                    break;
                }
                currentInGameState = InGameLoop.ThiefAction;
                break;
            case InGameLoop.ThiefAction:
                currentInGameState = InGameLoop.DefenderRollDice;
                break;
            case InGameLoop.DefenderRollDice:
                currentInGameState = InGameLoop.DefenderAction;
                break;
            case InGameLoop.DefenderAction:
                currentInGameState = InGameLoop.CatAction;
                break;
            case InGameLoop.CatAction:
                currentInGameState = InGameLoop.RoundStart;
                break;
            default:
                break;
            }
            // SetUI();
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
        TMP_Text nowThief = GameObject.Find("Now Thief").GetComponent<TMP_Text>();
        nowThief.text = thisMatchThiefName;

        TMP_Text NowRound = GameObject.Find("Now Round").GetComponent<TMP_Text>();
        NowRound.text = round.ToString();

        TMP_Text NowState = currentState.GetComponent<TMP_Text>();
        NowState.text = inGameLoopName[currentInGameState];

        TMP_Text BannerText = GameObject.Find("BannerText").GetComponent<TMP_Text>();
        if (inGameLoopName[currentInGameState] == "RoundStart"){
            BannerText.text = "Round " + round.ToString();
        }
        else{
            BannerText.text = inGameLoopName[currentInGameState];
        }
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

    private void ChangeToNextScene(Scene nextScene){
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
                Debug.Log("InGame init");
                instance.round = 1;
                instance.players.Clear();

                instance.players.Add(ThiefController.instance);
                instance.players.Add(DefenderController.instance);

                ThiefController.instance.SetPlayer("A", 10, 0, 2, new List<ItemDTO>());
                ThiefController.instance._items.Add(BagController.instance.CreateRandomItemDTO());
                ThiefController.instance._items.Add(BagController.instance.CreateRandomItemDTO());
                ThiefController.instance._items.Add(BagController.instance.CreateRandomItemDTO());
                ThiefController.instance._items.Add(BagController.instance.CreateRandomItemDTO());

                DefenderController.instance.SetPlayer("B", 10, 0, 3, new List<ItemDTO>());
                DefenderController.instance._items.Add(BagController.instance.CreateRandomItemDTO());
                DefenderController.instance._items.Add(BagController.instance.CreateRandomItemDTO());
                DefenderController.instance._items.Add(BagController.instance.CreateRandomItemDTO());
                DefenderController.instance._items.Add(BagController.instance.CreateRandomItemDTO());

                instance.currentPlayer = instance.players[0];
                BagController.instance.SetCurrentBagItems();
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

}

using UnityEngine;
public class ThiefAction : IState{
    public override void OnEnter()
    {
        BannerController.instance.ShowBanner();
        BagController.instance.ShowText();
    }

    public override void OnUpdate()
    {
        if (ThiefController.instance._hp <= 0){
            ThiefController.instance.ThiefDeadCG.SetActive(true);
            if (Input.GetKeyDown("n")){
                GameManager.instance.WinnerAddScore("Defender");
                GameManager.instance.thisMatchThiefName = GameManager.instance.thisMatchThiefName == "A" ? "B" : "A";
                GameManager.instance.ChangeToScene(GameManager.Scene.InGame);
            }
        }
        else{
            PerformThiefAction();
            if (ThiefController.instance.RTAGameObject.activeInHierarchy){
                GameManager.instance.GotoState(new RTATime());
                GameManager.instance.currentInGameState = GameManager.InGameLoop.RTATime;
            }

            if (Input.GetKeyDown("e")){
                GameManager.instance.SwitchBag();
            }
            else if (Input.GetKeyDown("n")){
                GameManager.instance.GotoState(new DefenderEvent());
                GameManager.instance.currentInGameState = GameManager.InGameLoop.DefenderEvent;
            }
        }
    }

    public void PerformThiefAction(){
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

    public override void OnExit()
    {
        ThiefController.instance.CloseMove();
        BagController.instance.Close();
        ThiefController.instance.SetNowRound();
        BagController.instance.CloseText();
    }
}
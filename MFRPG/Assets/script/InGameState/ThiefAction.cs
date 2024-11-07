using UnityEngine;
public class ThiefAction : IState{
    public GameObject rtaController;
    public override void OnEnter()
    {
        BannerController.instance.ShowBanner();
        rtaController = RTAController.instance.gameObject;
    }

    public override void OnUpdate()
    {
        PerformThiefAction();
        if (rtaController.activeInHierarchy){
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
    }
}
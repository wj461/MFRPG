using UnityEngine;

public class RTATime : IState
{
    public GameObject rtaController;
    public override void OnEnter()
    {
        BannerController.instance.ShowBanner();
        rtaController = RTAController.instance.gameObject;
    }

    public override void OnUpdate()
    {
        if (!rtaController.activeInHierarchy){
            GameManager.instance.GotoState(new DefenderEvent());
            GameManager.instance.currentInGameState = GameManager.InGameLoop.DefenderEvent;
        }

    }

    public override void OnExit()
    {
    }
}
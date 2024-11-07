
using UnityEngine;

public class RoundStart : IState
{
    public override void OnEnter()
    {
        BannerController.instance.ShowBanner();
    }

    public override void OnUpdate()
    {
        GameManager.instance.GotoState(new ThiefEvent());
        GameManager.instance.currentInGameState = GameManager.InGameLoop.ThiefEvent;
    }

    public override void OnExit()
    {
    }
}


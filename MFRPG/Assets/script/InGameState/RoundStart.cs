
using UnityEngine;

public class RoundStart : IState
{
    public override void OnEnter()
    {
        BannerController.instance.ShowBanner();
    }

    public override void OnUpdate()
    {
        GameManager.instance.GotoState(new ThiefRollDice());
        GameManager.instance.NextInGameLoop();
    }

    public override void OnExit()
    {
    }
}


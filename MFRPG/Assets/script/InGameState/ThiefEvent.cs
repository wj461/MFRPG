
using UnityEngine;

public class ThiefEvent : IState
{
    public override void OnEnter()
    {
        // BannerController.instance.ShowBanner();
        ThiefController.instance.RandomThiefSkipEvent();
    }

    public override void OnUpdate()
    {
        if (Input.GetKeyDown("n"))
        {
            GameManager.instance.GotoState(new DefenderRollDice());
            GameManager.instance.NextInGameLoop();
        }
    }

    public override void OnExit()
    {
        ThiefController.instance.CloseMove();
        BagController.instance.Close();
        ThiefController.instance.SetNowRound();
    }
}
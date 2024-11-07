
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
        if (ThiefController.instance.thiefState == ThiefState.Skip)
        {
            if (Input.GetKeyDown("n"))
            {
                GameManager.instance.GotoState(new DefenderEvent());
                GameManager.instance.currentInGameState = GameManager.InGameLoop.DefenderRollDice;
            }
        }
        else
        {
            GameManager.instance.GotoState(new ThiefRollDice());
            GameManager.instance.currentInGameState = GameManager.InGameLoop.ThiefRollDice;
        }
    }

    public override void OnExit()
    {
        ThiefController.instance.CloseMove();
        BagController.instance.Close();
        ThiefController.instance.SetNowRound();
    }
}
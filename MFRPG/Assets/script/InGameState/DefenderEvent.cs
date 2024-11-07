using UnityEngine;

public class DefenderEvent : IState
{
    public override void OnEnter()
    {
        // BannerController.instance.ShowBanner();
        CatController.instance.RandomCatSkipEvent();
    }

    public override void OnUpdate()
    {
        if (CatController.instance.catState == CatController.CatState.CanNotMove)
        {
            if (Input.GetKeyDown("n"))
            {
                GameManager.instance.GotoState(new DefenderRollDice());
                GameManager.instance.currentInGameState = GameManager.InGameLoop.DefenderRollDice;
            }
        }
        else
        {
            GameManager.instance.GotoState(new DefenderRollDice());
            GameManager.instance.currentInGameState = GameManager.InGameLoop.DefenderRollDice;
        }

    }

    public override void OnExit()
    {
    }
}
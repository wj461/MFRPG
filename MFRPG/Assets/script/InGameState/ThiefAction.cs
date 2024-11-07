using UnityEngine;
public class ThiefAction : IState{
    public override void OnEnter()
    {
        BannerController.instance.ShowBanner();
    }

    public override void OnUpdate()
    {
        GameManager.instance.ThiefAction();

        if (Input.GetKeyDown("e")){
            GameManager.instance.SwitchBag();
        }
        else if (Input.GetKeyDown("n")){
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
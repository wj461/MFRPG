
using UnityEngine;
public class DefenderAction : IState
{
    public override void OnEnter()
    {
        BannerController.instance.ShowBanner();
        BagController.instance.ShowText();
    }

    public override void OnUpdate()
    {
        if (Input.GetKeyDown("e")){
            GameManager.instance.SwitchBag();
        }
        else if (Input.GetKeyDown("n")){
            GridController.instance.SetNowRound();
            GameManager.instance.GotoState(new CatAction());
            GameManager.instance.currentInGameState = GameManager.InGameLoop.CatAction;
        }
    }

    public override void OnExit()
    {
        BagController.instance.Close();
        DefenderController.instance.SetNowRound();
        BagController.instance.CloseText();
    }
}
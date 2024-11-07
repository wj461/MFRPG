
using UnityEngine;
public class DefenderAction : IState
{
    public override void OnEnter()
    {
        BannerController.instance.ShowBanner();
    }

    public override void OnUpdate()
    {
        if (Input.GetKeyDown("e")){
            GameManager.instance.SwitchBag();
        }
        else if (Input.GetKeyDown("n")){
            GridController.instance.SetNowRound();
            GameManager.instance.GotoState(new CatAction());
            GameManager.instance.NextInGameLoop();
        }
    }

    public override void OnExit()
    {
        BagController.instance.Close();
        DefenderController.instance.SetNowRound();
    }
}
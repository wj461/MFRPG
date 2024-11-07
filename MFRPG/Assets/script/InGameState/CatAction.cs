using UnityEngine;

public class CatAction : IState
{
    public override void OnEnter()
    {
        BannerController.instance.ShowBanner();
    }

    public override void OnUpdate()
    {
        if (Input.GetKeyDown("e")) {
            GameManager.instance.SwitchBag();
        }
        if (GridController.instance.catWalkingState == GridController.CatWalkingState.Wait){
            CatController.instance.CatMoveMotion();
        }
        if ((GridController.instance.catWalkingState == GridController.CatWalkingState.MoveEnd ) 
            || (CatController.instance.catState == CatController.CatState.CanNotMove)){
            GameManager.instance.round += 1;
            GameManager.instance.GotoState(new RoundStart());
            GameManager.instance.NextInGameLoop();
        }
    }

    public override void OnExit()
    {
        CatController.instance.SetNowRound();
    }
}
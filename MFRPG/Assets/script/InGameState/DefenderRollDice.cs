
using UnityEngine;
public class DefenderRollDice : IState
{
    public override void OnEnter()
    {
        BannerController.instance.ShowBanner();
        CatController.instance.RandomCatSkipEvent();
        Dice.instance.RollDice();
        GameManager.instance.SwitchPlayer(1);
    }

    public override void OnUpdate()
    {
        if (CatController.instance.catState == CatController.CatState.CanNotMove){
            //CG part
            if (!CatController.instance.eventCG.activeInHierarchy){
                if (Input.GetKeyDown(KeyCode.Space)){
                    GameManager.instance.currentPlayer._cost = Dice.instance.StopRollDice();
                    GameManager.instance.GotoState(new DefenderAction());
                    GameManager.instance.NextInGameLoop();
                }
            }
        }
        else{
            if (Input.GetKeyDown(KeyCode.Space)){
                GameManager.instance.currentPlayer._cost = Dice.instance.StopRollDice();
                GameManager.instance.GotoState(new DefenderAction());
                GameManager.instance.NextInGameLoop();
            }
        }
    }

    public override void OnExit()
    {
    }
}
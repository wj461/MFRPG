
using UnityEngine;
public class DefenderRollDice : IState
{
    public override void OnEnter()
    {
        BannerController.instance.ShowBanner();
        GameManager.instance.SwitchPlayer(1);
        Dice.instance.RollDice();
    }

    public override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space)){
            GameManager.instance.currentPlayer._cost = Dice.instance.StopRollDice();
            GameManager.instance.GotoState(new DefenderAction());
            GameManager.instance.currentInGameState = GameManager.InGameLoop.DefenderAction;
        }
    }

    public override void OnExit()
    {
    }
}
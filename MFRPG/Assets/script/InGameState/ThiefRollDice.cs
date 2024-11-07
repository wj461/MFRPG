using UnityEngine;
public class ThiefRollDice : IState{
    public override void OnEnter()
    {
        BannerController.instance.ShowBanner();
        Dice.instance.RollDice();
        GameManager.instance.SwitchPlayer(0);
        ThiefController.instance.CloseMove();
    }

    public override void OnUpdate()
    {
        if (ThiefController.instance.thiefState == ThiefState.Skip){
            if (Input.GetKeyDown("n")){
                GameManager.instance.GotoState(new DefenderRollDice());
                GameManager.instance.currentInGameState = GameManager.InGameLoop.DefenderRollDice;
            }
        }
        else{
            if (Input.GetKeyDown(KeyCode.Space)){
                GameManager.instance.currentPlayer._cost = Dice.instance.StopRollDice();
                GameManager.instance.GotoState(new ThiefAction());
                GameManager.instance.currentInGameState = GameManager.InGameLoop.ThiefAction;
            }
        }
    }

    public override void OnExit()
    {
    }
}

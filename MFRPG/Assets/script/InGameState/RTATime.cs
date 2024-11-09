using UnityEngine;

public class RTATime : IState
{
    public override void OnEnter()
    {
        BannerController.instance.ShowBanner();
    }

    public override void OnUpdate()
    {
    }

    public override void OnExit()
    {
    }
}
using Unity.VisualScripting;
using UnityEngine;

public abstract class IState
{
    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnExit();

}

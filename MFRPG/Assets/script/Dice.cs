using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour
{
    public static Dice instance;
    public Sprite[] diceSides;
    public Animator animator;

    public int currentSide = 0;
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void RollDice()
    {
        animator.enabled = true;
    }

    public int StopRollDice()
    {
        animator.enabled = false;
        currentSide = Random.Range(1, 7);
        GetComponent<Image>().sprite = diceSides[currentSide - 1];
        return currentSide;
    }

}

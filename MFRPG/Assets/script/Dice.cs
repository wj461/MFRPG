using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour
{
    public static Dice instance;
    public Sprite[] diceSides;
    public Animator animator;

    public Vector3 thiefDicePosition = new Vector3(-230f, 110f, 0);
    public Vector3 defenderDicePosition = new Vector3(250f, 110f, 0);

    public int currentSide = 0;
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // After switch player, roll dice
    public void RollDice()
    {
        if (GameManager.instance.currentPlayer._playerName == "Thief")
        {
            this.gameObject.transform.localPosition = thiefDicePosition;
        }
        else
        {
            this.gameObject.transform.localPosition = defenderDicePosition;
        }
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

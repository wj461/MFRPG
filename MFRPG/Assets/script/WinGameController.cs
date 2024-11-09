using UnityEngine;
using UnityEngine.UI;

public class WinGameController : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite A;
    public Sprite B;
    void Start()
    {
        var winImage = gameObject.GetComponent<Image>();
        if (GameManager.instance.playerAWinCount > GameManager.instance.playerBWinCount)
        {
            winImage.sprite = A;
        }
        else{
            winImage.sprite = B;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            GameManager.instance.playerAWinCount = 0;
            GameManager.instance.playerBWinCount = 0;
            GameManager.instance.ChangeToScene(GameManager.Scene.Welcome);
        }
        
    }
}

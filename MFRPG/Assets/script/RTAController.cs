using UnityEngine;

public class RTAController : MonoBehaviour
{
    public GameObject defender;
    public GameObject thief;
    public GameObject cat;
    public GameObject thiefWin;
    public GameObject defenderWin;

    public Vector3 catStart;
    public Vector3 thiefStart;
    public Camera mainCamera;
    // Start is called before the first frame update
    void Awake()
    {
    }
    void Start()
    {
        ThiefController.instance._cost = 0;
        BagController.instance.bagState = BagState.Close;
        catStart = cat.transform.position;
        thiefStart = thief.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("x")){
            Instantiate(defender, transform);
            cat.transform.position = new Vector3(cat.transform.position.x - 1, cat.transform.position.y, cat.transform.position.z);
        }
        if (Input.GetKeyDown("z")){
            thief.transform.position = new Vector3(thief.transform.position.x - 1, thief.transform.position.y, thief.transform.position.z);
        }
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(cat.transform.position);

        if (viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1 || viewportPosition.z < 0)
        {
            defenderWin.SetActive(true);
            cat.transform.position = catStart;
            thief.transform.position = thiefStart;
            var children = GetComponentsInChildren<DefenderRTAController>();
            foreach (var child in children)
            {
                Destroy(child.gameObject);
            }
        }

        if (Vector3.Distance(thief.transform.position, cat.transform.position) < 1.5){
            thiefWin.SetActive(true);
        }
        if (thiefWin.activeInHierarchy || defenderWin.activeInHierarchy){
            if (Input.GetKeyDown("n")){
                if (thiefWin.activeInHierarchy){
                    GameManager.instance.WinnerAddScore("Thief");
                }
                else{
                    GameManager.instance.WinnerAddScore("Defender");
                }
                GameManager.instance.thisMatchThiefName = GameManager.instance.thisMatchThiefName == "A" ? "B" : "A";
                GameManager.instance.ChangeToScene(GameManager.Scene.InGame);
            }
        }
    }
}

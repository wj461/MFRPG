using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [SerializeField]
    public Button[] buttonList;
    private string clickedButtonName;
    private bool buttonClicked = false;
    private static ButtonManager buttonManager = null;
    
    void Awake() {
        buttonList = GetButtonList();
        if (buttonManager == null) {
            SetButton();
            print("setButton");
            buttonManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            buttonManager.buttonList = buttonList;
            buttonManager.SetButton();
            print("setButton");
            Destroy(gameObject);
        }
    }

    public bool IsButtonClicked(string buttonName)
    {
        if (buttonClicked && clickedButtonName == buttonName)
        {
            buttonClicked = false;
            return true;
        }

        return false;
    }

    public void testUI()
    {
        print("testUI");
    }
    private void SetButton()
    {
        foreach (Button btn in buttonList)
        {
            string buttonName = btn.name;
            btn.onClick.AddListener(() => OnButtonClick(buttonName));
        }
    }

    void OnDestory()
    {
        foreach (Button btn in buttonList)
        {
            btn.onClick.RemoveAllListeners();
        }
    }

    private void OnButtonClick(string buttonName)
    {
        clickedButtonName = buttonName;
        buttonClicked = true;
    }

    private Button[] GetButtonList(){
        return GameObject.FindObjectsOfType<Button>();
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

// This are used in the gamemangaer to keep track on the different aspects of the menu. different elements are turned off and on through the
// functions that are called when buttons are pressed.

public class Gamemanager : MonoBehaviour {
    public Text creditstext;
    public Button returnbutton;
    public Image backgroundSource;
    Image returnimage;
    // Highlight textures
    public Sprite[] highlights = new Sprite[4];
    // Normal textures
    public Sprite[] normal = new Sprite[4];
    

    void Awake()
    {
        // predefines all the menue items in order to being able to find and turn them off in the below functions
        creditstext = GameObject.Find("Credits text").GetComponent<Text>();
        backgroundSource = GameObject.Find("Menu").GetComponent<Image>();
        returnimage = GameObject.Find("Close").GetComponent<Image>();
        

    }
    public void Begingame()
    {
        SceneManager.LoadScene("tmp");
    }
    public void Creditstrue ()
    {
        creditstext.enabled = true;
        returnbutton.enabled = true;
        returnimage.enabled = true;
        // Show a text on the menue screen displaying the credits for the game
    }
    public void Creditsfalse()
    {
        // Hide credits if shown when clicking the back button
        if (creditstext.enabled)
        {
            creditstext.enabled = false;
            returnbutton.enabled = false;
            returnimage.enabled = false;

        }
    }
    public void Quitgame()
    {
        // quits the application
        Application.Quit();
    }
    // following are called through pointer enter events
    public void StartHighlight()
    {
        Image mystart = GameObject.Find("Start").GetComponent<Image>();
        mystart.sprite = highlights[0]; 
    }
    public void CreditHighlight()
    {
        Image mycredit = GameObject.Find("Credits").GetComponent<Image>();
        mycredit.sprite = highlights[1];
    }
    public void QuitHighlight()
    {
        Image myQuit = GameObject.Find("Quit").GetComponent<Image>();
        myQuit.sprite = highlights[2];
    }
    public void CloseHighlight()
    {
        Image myClose = GameObject.Find("Close").GetComponent<Image>();
        myClose.sprite = highlights[3];
    }
    // following are called on pointer exit events
    public void Startreset()
    {
        Image mystart = GameObject.Find("Start").GetComponent<Image>();
        mystart.sprite = normal[0];
    }
    public void Creditreset()
    {
        Image mycredit = GameObject.Find("Credits").GetComponent<Image>();
        mycredit.sprite = normal[1];
    }
    public void Quitreset()
    {
        Image myQuit = GameObject.Find("Quit").GetComponent<Image>();
        myQuit.sprite = normal[2];
    }
    public void Closereset()
    {
        Image myClose = GameObject.Find("Close").GetComponent<Image>();
        myClose.sprite = normal[3];
    }

}

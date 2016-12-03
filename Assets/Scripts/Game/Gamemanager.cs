using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// This are used in the gamemangaer to keep track on the different aspects of the menu. different elements are turned off and on through the
// functions that are called when buttons are pressed.

public class Gamemanager : MonoBehaviour {
    public Text creditstext;
    public Button[] menuButtons = new Button[4];
    public Image backgroundSource;
    public void Begingame()
    {
        menuButtons[0].enabled = false;
        menuButtons[1].enabled = false;
        menuButtons[2].enabled = false;
        backgroundSource.enabled = false;
        Creditsfalse();
        // removes the elements from the canvas and start the game by giving control to the player
        // eventually changing the Ui so that it shows elements needed for the player
    }
    public void Creditstrue ()
    {
        creditstext.enabled = true;
        menuButtons[3].enabled = true;
        // Show a text on the menue screen displaying the credits for the game
    }
    public void Creditsfalse()
    {
        // Hide credits if shown when clicking the back button
        if (creditstext.enabled)
        {
            creditstext.enabled = false;
            menuButtons[3].enabled = false;
        }
    }
    public void Quitgame()
    {
        // quits the application
        Application.Quit();
    }
}

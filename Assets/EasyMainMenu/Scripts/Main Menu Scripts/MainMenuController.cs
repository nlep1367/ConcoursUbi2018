using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    Animator anim;
    MainMenu_KeyboardController controller;

    public string newGameSceneName;
    public int quickSaveSlotID;

    [Header("Options Panel")]
    public GameObject MainPanel;
    public GameObject MainOptionsPanel;
    public GameObject StartGameOptionsPanel;
    public GameObject AudioPanel;
    public GameObject GfxPanel;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();

        //new key
    #if !EMM_ES2
        PlayerPrefs.SetInt("quickSaveSlot", quickSaveSlotID);
#else
        ES2.Save(quickSaveSlotID, "quickSaveSlot");
#endif
    }

#region Open Different panels

    public void openOptions()
    {
        //enable respective panel
        MainOptionsPanel.SetActive(true);
        StartGameOptionsPanel.SetActive(false);
        MainPanel.SetActive(false);

        //play anim for opening main options panel
        anim.Play("buttonTweenAnims_on");

        //play click sfx
        playClickSound();       
    }

    public void openStartGameOptions()
    {
        //enable respective panel
        MainPanel.SetActive(false);
        MainOptionsPanel.SetActive(false);
        StartGameOptionsPanel.SetActive(true);

        //play anim for opening main options panel
        anim.Play("buttonTweenAnims_on");

        //play click sfx
        playClickSound();

        MatchMakingLobbyManager m = GameObject.Find("LobbyManager").GetComponent<MatchMakingLobbyManager>();
        m.InitComponents();
    }

    public void openOptions_Game()
    {
        //enable respective panel
        AudioPanel.SetActive(true);
        GfxPanel.SetActive(false);
        MainOptionsPanel.SetActive(false);
        MainPanel.SetActive(false);

        //play anim for opening game options panel
        anim.Play("OptTweenAnim_on");

        //play click sfx
        playClickSound();

    }
    public void openOptions_Gfx()
    {
        //enable respective panel
        AudioPanel.SetActive(false);
        MainOptionsPanel.SetActive(false);
        GfxPanel.SetActive(true);
        MainPanel.SetActive(false);

        //play anim for opening game options panel
        anim.Play("OptTweenAnim_on");

        //play click sfx
        playClickSound();

    }

    public void newGame()
    {
        

    }
    #endregion

    #region Back Buttons

    public void back_options()
    {
        MainPanel.SetActive(true);
        //simply play anim for CLOSING main options panel
        anim.Play("buttonTweenAnims_off");

        //play click sfx
        playClickSound();
    }

    public void back_options_panels()
    {
        //simply play anim for CLOSING main options panel
        MainOptionsPanel.SetActive(true);
        anim.Play("OptTweenAnim_off");
        
        //play click sfx
        playClickSound();

    }

    public void Quit()
    {
        Application.Quit();
    }
#endregion

#region Sounds
    public void playHoverClip() {}

    void playClickSound() {}
#endregion
}

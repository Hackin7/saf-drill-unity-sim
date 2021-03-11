using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public File file;
    public PlayerMovement player;
    public Transform playerFixedViewReference;

    // Main HUD
    public GameObject HUDMenu;
    public Dropdown commandsUIList;
    public Button runCommand;
    public Button settings;
    public Text subtitle;
    public GameObject SongMenu;

    public GameObject SettingsMenu;
    public InputField settingsManCount;
    public InputField settingsSpeed;
    public InputField settingsDistancing;
    public Button settingsResetPlayerView;
    public Button settingsDone;

    private int prevFileState = -1; // Invalid option to auto refresh
    void updateUIList()
    {
        commandsUIList.ClearOptions();
        List<string> commands = file.availableCommands();
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        foreach (string command in commands) { options.Add(new Dropdown.OptionData(command, null)); }
        commandsUIList.AddOptions(options);
        commandsUIList.RefreshShownValue();
    }

    // Marching Songs


    // Start is called before the first frame update
    void Start()
    {
        // Auto set UI
        SettingsMenu.SetActive(false);
        HUDMenu.SetActive(true);

        // Main HUD //////////////////////////////////////
        runCommand.onClick.AddListener(delegate {
            file.giveCommand(commandsUIList.options[commandsUIList.value].text);
            updateUIList();
        });
        settings.onClick.AddListener(delegate {
            HUDMenu.SetActive(false);
            SettingsMenu.SetActive(true);
        });

        // Settings Menu //////////////////////////////////
        settingsResetPlayerView.onClick.AddListener(delegate {
            player.transform.position = playerFixedViewReference.position;
            player.transform.rotation = playerFixedViewReference.rotation;
            player.transform.localScale = playerFixedViewReference.localScale;
        });
        settingsDone.onClick.AddListener(delegate {
            SettingsMenu.SetActive(false);
            HUDMenu.SetActive(true);

        });
        settingsManCount.onValueChanged.AddListener(delegate
        {
            file.setManCount(Mathf.Clamp(int.Parse(settingsManCount.text), 1, 100));
            updateUIList();
        });
        settingsManCount.text = "1";// file.getManCount().ToString();

        settingsSpeed.onValueChanged.AddListener(delegate
        {
            file.setSpeed(Mathf.Clamp(float.Parse(settingsSpeed.text), 0.1f, 10f));
        });
        settingsSpeed.text = file.getSpeed().ToString();

        settingsDistancing.onValueChanged.AddListener(delegate
        {
            file.socialDistancing = Mathf.Clamp(float.Parse(settingsDistancing.text), 0.8f, 1.5f);
            file.setManCount(file.getManCount());
        });
        
        settingsDistancing.text = file.socialDistancing.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        // Update command List only when needed
        if (file.getState() != prevFileState)
        {
            updateUIList();
        }
        prevFileState = file.getState();

        // Subtitles
        subtitle.text = file.getSubtitles();

        // Song Menu
        SongMenu.SetActive(file.isMarching());
    }
}

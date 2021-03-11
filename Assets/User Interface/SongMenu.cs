using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongMenu : MonoBehaviour
{
    public Dropdown songUIList;
    public Button runSong;
    public File file;

    private List<Song> songs = new List<Song>(new Song[]
    {
        new Song("left right", new List<string>(new string[] {
            "leeeft", "","riiight", "", "left", "right", "left", "right", "left", "",
            "wooh", "", "aah", "", "bra", "vo", "wa", "rriors",
            "leeeft", "","riiight", "", "left", "right", "left", "right", "left", "",
            "wooh", "", "aah", "", "bra", "vo", "wa", "rriors",
            "left left", "left", "right right", "right",
            "left left", "left", "right right", "right",
            "left", "right", "left", "right", "left", "",
            "wooh", "", "aah", "", "bra", "vo", "wa", "rriors",
        } )),
        new Song("your left", new List<string>(new string[] {
            "your left", "", "your left", "","your left", "right",
            "STING", "RAY" ,
            "left", "right",
            "BRA", "VO",
            "left", "right",
            "WAR", "RIORS",
            "5SIR", "","5SIR", "","All", "All", "The way", "",
            "We like", "it here","We like", "it here",
            "we found","ourselves",
            "a home", "(A What?)",
            "a home", "(A What?)",
            "A HOME", "SWEET", "HOME", ""
        })),
        new Song("Cut", new List<string>(new string[] {"" }))
    });
    void updateUIList()
    {
        songUIList.ClearOptions();
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        foreach (Song song in songs) { options.Add(new Dropdown.OptionData(song.getName(), null)); }
        songUIList.AddOptions(options);
        songUIList.RefreshShownValue();
    }
    // Start is called before the first frame update
    void Start()
    {
        updateUIList();
        runSong.onClick.AddListener(delegate {
            // Find Song
            foreach (Song song in songs)
            {
                Debug.Log(song.getName());
                if (song.getName() == songUIList.options[songUIList.value].text) { file.setSong(song); }
            }
            
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public PlayerMovement player;
    public Transform playerFixedViewReference;
    public Button resetPlayerView;
    public Button done;
    public GameObject HUDMenu;
    public InputField manCount;

    // Start is called before the first frame update
    void Start()
    {
        /*
        freeViewToggle.onValueChanged.AddListener(delegate {
            if (freeViewToggle.isOn)
            {

            }
        });*/
        resetPlayerView.onClick.AddListener(delegate {
            player.transform.position = playerFixedViewReference.position;
            player.transform.rotation = playerFixedViewReference.rotation;
            player.transform.localScale = playerFixedViewReference.localScale;
        });
        done.onClick.AddListener(delegate {
            gameObject.SetActive(false);
            HUDMenu.SetActive(true);

        });
        manCount.onValueChanged.AddListener(delegate
        {

        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

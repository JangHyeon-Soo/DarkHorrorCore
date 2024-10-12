using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{

    public GameObject MainPasueMenu;
    public GameObject NotePanelMenu;


    // Start is called before the first frame update
    void Start()
    {

        
        //NotePanelMenu.GetComponent<NoteUIManager>().Initialization();
        NotePanelMenu.SetActive(false);

    }

    public void ShowNotePanelMenu()
    {
        MainPasueMenu.SetActive(false);
        NotePanelMenu.SetActive(true);

        GameManager.Instance.WidgetRecord.Add(NotePanelMenu);
    }
}

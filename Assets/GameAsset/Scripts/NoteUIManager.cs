using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class NoteUIManager : MonoBehaviour
{
    public GameObject ScrollViewContent;
    public GameObject ElementButton;

    public GameObject NoteTitleObject;
    public GameObject NoteContentObject;

    public List<GameObject> ElementButtons = new List<GameObject>();

    bool initialized;

    private void Start()
    {
        Initialization();
    }

    // Start is called before the first frame update
    public void Initialization()
    {
        if(initialized == false)
        {
            if(NoteContentObject != null)
            {
                NoteContentObject.SetActive(false);
            }
            
            if(NoteTitleObject != null)
            {
                NoteTitleObject.SetActive(false);
            }
            
            int count = NotePrefab.instance.notes.Count;

            //Debug.Log(count);
            for (int i = 0; i < count; i++)
            {
                int index = i;
                GameObject go = Instantiate(ElementButton, ScrollViewContent.transform);
                go.GetComponent<UnityEngine.UI.Button>().interactable = false;
                go.GetComponent<UnityEngine.UI.Button>().transform.GetChild(0).GetComponent<TMP_Text>().text = "???";
                

                go.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ClickFunction(index));
                ElementButtons.Add(go);

            }
            initialized = true;
        }
    }

    public void ClickFunction(int num)
    { 
        //PointerEventData pointerData = eventData as PointerEventData;

        // 클릭된 UI 요소의 GameObject 확인
        GameObject clickedObject = ElementButtons[num].gameObject;

        //int num = ElementButtons.IndexOf(clickedObject);

        NoteContentObject.SetActive(true);
        NoteTitleObject.SetActive(true);


        NoteTitleObject.GetComponent<TMP_Text>().text = clickedObject.GetComponent<NoteElemInfo>().noteTitle;
        NoteContentObject.GetComponent<TMP_Text>().text = clickedObject.GetComponent<NoteElemInfo>().noteContent;
    }
}

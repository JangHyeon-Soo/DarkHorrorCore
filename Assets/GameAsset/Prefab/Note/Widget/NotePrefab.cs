using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotePrefab : MonoBehaviour
{
    public static NotePrefab instance;
    public List<GameObject> notes = new List<GameObject>(); 
    //public GameObject note_1;


    // Start is called before the first frame update
    void Awake()
    {
        //Debug.Log(notes.Count);
        instance = this;
    }

  
    public int GetNoteInfoIndex(int NoteID)
    {
        List<GameManager.NoteInfo> tempNotes = GameManager.Instance.playerTF.GetComponent<PlayerInventory>().NoteList;
        
        for(int i = 0; i < tempNotes.Count; i++)
        {
            if(tempNotes[i].NoteID == NoteID)
            {
                return i;
            }
        }

        return -1;

    }

}

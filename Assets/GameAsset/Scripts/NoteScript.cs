using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteScript : MonoBehaviour
{
    public string NoteTitle;
    public string NoteContent;
    public string NoteDescription;
    public int NoteID;
    public GameManager.ItemType Type;


    GameManager.NoteInfo NoteInfo;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "Note";
        NoteInfo = new GameManager.NoteInfo(NoteTitle, NoteContent, NoteDescription,NoteID, Type);
    }

    public GameManager.NoteInfo GetNote()
    {
        return NoteInfo;
    }
}

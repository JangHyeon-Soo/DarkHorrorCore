using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField]
    public List<GameManager.ItemInfo> Inventory = new List<GameManager.ItemInfo>();
    public List<GameManager.NoteInfo> NoteList = new List<GameManager.NoteInfo>();

    int item_lastIndex = 0;

    public InventoryWidget inventoryWidget;


    public void AddItemToInventory(GameManager.ItemInfo item)
    {
        if(Inventory.Count == 0)
        {
            Inventory.Add(item);
            item_lastIndex++;
        }

        else
        {
            int itemIndex = FindItenIndexInInventory(item.id);
            if (itemIndex == -1)
            {
                Inventory.Add(item);
                item_lastIndex++;
                
                
            }

            else
            {
                Inventory[itemIndex].quantity += item.quantity;

            }
         
        }
        //switch (item.id)
        //{
        //    case 0:
        //        GetComponent<InventoryWidget>().ShortCutKeys[1].GetComponent<Image>().sprite = item.sprite;
        //        break;
        //    case 2:
        //        GetComponent<InventoryWidget>().ShortCutKeys[0].GetComponent<Image>().sprite = item.sprite;
        //        break;
        //}
        //Debug.Log("Add Complete" + $"{item.name}");
    }

    public int FindItenIndexInInventory(int itemId)
    {
        if (item_lastIndex <= 0)
        {
            return -1;
        }

        else
        {
            for (int i = 0; i < Inventory.Count; i++)
            {
                if (Inventory[i].id == itemId)
                {
                    return i;
                }
            }

            return -1;
        }

    }

    public List<GameManager.ItemInfo> GetInventory()
    {
        return Inventory;
    }

    public void AddNoteToNoteList(GameManager.NoteInfo noteInfo)
    {
        NoteList.Add(noteInfo);
        UpdateNoteUI_DB(noteInfo);
     
    }

    public void UpdateNoteUI_DB(GameManager.NoteInfo note)
    {
        GameManager.Instance.GetComponent<NoteUIManager>().ElementButtons[note.NoteID - 1].GetComponent<Button>().interactable = true;
        GameManager.Instance.GetComponent<NoteUIManager>().ElementButtons[note.NoteID - 1].transform.GetChild(0).GetComponent<TMP_Text>().text = note.NoteTitle;
        GameManager.Instance.GetComponent<NoteUIManager>().ElementButtons[note.NoteID - 1].GetComponent<NoteElemInfo>().noteTitle = note.NoteTitle;
        GameManager.Instance.GetComponent<NoteUIManager>().ElementButtons[note.NoteID - 1].GetComponent<NoteElemInfo>().noteContent = note.NoteContent;
        GameManager.Instance.GetComponent<NoteUIManager>().ElementButtons[note.NoteID - 1].GetComponent<NoteElemInfo>().noteId = note.NoteID;

        
    }

}

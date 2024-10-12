using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class InventoryWidget : MonoBehaviour
{
    public GameObject inventoryCanvas;
    public GameObject inventoryWidget;
    public GameObject ShortCut_Key_Widget;
    public List<GameObject> ShortCutKeys = new List<GameObject>();

    public List<Tuple<GameObject,GameManager.ItemInfo>> inventoryelem_imgs = new List<Tuple<GameObject,GameManager.ItemInfo>>();
    public GameObject WhitePoint;

    // Start is called before the first frame update
    void Start()
    {
        inventoryWidget.SetActive(false);

        for (int i = 0; i < inventoryWidget.transform.childCount; i++)
        {
            Tuple<GameObject, GameManager.ItemInfo> pair = new Tuple<GameObject, GameManager.ItemInfo>(inventoryWidget.transform.GetChild(i).gameObject, null);
            inventoryelem_imgs.Add(pair);
        }

        //Debug.Log(ShortCut_Key_Widget.transform.childCount);

        //for(int i = 0; i < ShortCut_Key_Widget.transform.childCount; ++i)
        //{
        //    ShortCutKeys.Add(ShortCut_Key_Widget.transform.GetChild(0).gameObject); 
        //}

        //Debug.Log(ShortCutKeys.Count);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) )
        {
            if (inventoryWidget.activeSelf)
            {
                //GameManager.Instance.playerState = GameManager.StateOfPlayer.Default;
                GameManager.Instance.CursorOff();
                inventoryCanvas.SetActive(false);
                inventoryWidget.SetActive(false);
                WhitePoint.SetActive(true);
                GameManager.Instance.WidgetRecord.Remove(inventoryCanvas.gameObject);
                GameManager.Instance.ShowLastWidgetObject();

                
            }

            else
            {
                GameManager.Instance.AddToWidgetRecord(inventoryCanvas.gameObject);

                //GameManager.Instance.WidgetRecord.Add(inventoryCanvas.gameObject);
                //GameManager.Instance.playerState = GameManager.StateOfPlayer.inspection;
                GameManager.Instance.CursorOn();
                inventoryCanvas.SetActive(true);
                inventoryWidget.SetActive(true);
                
                PrintInventoryItems();
                WhitePoint.SetActive(false);
                
            }
        }   
    }

    public void PrintInventoryItems()
    {

        GameManager.Instance.playerState = GameManager.StateOfPlayer.inspection;
        if (GameManager.Instance.playerTF.GetComponent<PlayerInventory>().Inventory.Count == 0)
        {
            for(int i = 0; i < inventoryelem_imgs.Count; ++i)
            {
                inventoryelem_imgs[i].Item1.GetComponent<Image>().sprite = null;
                inventoryelem_imgs[i].Item1.GetComponent<InventoryElemInfo>().itemInfo = null;
            }
        }


        else
        {
            List<GameManager.ItemInfo> inven = GameManager.Instance.playerTF.GetComponent<PlayerInventory>().GetInventory();
            if (inven != null)
            { 
                for (int i = 0; i < inventoryelem_imgs.Count; i++)
                {
                    if(i < inven.Count)
                    {
                        inventoryelem_imgs[i].Item1.GetComponent<Image>().sprite = inven[i].sprite;
                        inventoryelem_imgs[i].Item1.GetComponent<InventoryElemInfo>().itemInfo = inven[i];
                    }

                    else
                    {
                        inventoryelem_imgs[i].Item1.GetComponent<Image>().sprite = null;
                        inventoryelem_imgs[i].Item1.GetComponent<InventoryElemInfo>().itemInfo = null;
                    }
                    
                }

            

            }
        }
        
    }

    


  

    
}

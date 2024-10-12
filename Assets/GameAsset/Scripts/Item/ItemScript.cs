using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public GameManager.ItemType Type;
    public string name;
    public int id;
    public int quantity;
    public Sprite sprite;
    public string description;


    // Start is called before the first frame update
   
    public GameManager.ItemInfo GetItem()
    {
        GameManager.ItemInfo info = new GameManager.ItemInfo(name, id, quantity, sprite, description, Type);

        return info;
    }


}

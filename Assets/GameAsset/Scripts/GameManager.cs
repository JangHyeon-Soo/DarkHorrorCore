using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;



public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    #region class or enum
    public enum ItemType
    {
        Item,
        Note,
    }

    public class ItemInfo
    {
        public string name;
        public int id;
        public int quantity;
        public Sprite sprite;
        public string description;
        public ItemType type;

        public ItemInfo(string name, int id, int quantity, Sprite sprite, string description, ItemType type)
        {
            this.name = name;
            this.id = id;
            this.quantity = quantity;
            this.sprite = sprite;
            this.description = description;
            this.type = type;
        }

    }
    public class NoteInfo
    {
        public string NoteTitle;
        public string NoteContent;
        public string NoteDescription;
        public int NoteID;
        public ItemType type;

        public NoteInfo(string noteTitle, string noteContent, string noteDescription,int noteID, ItemType type)
        {
            NoteTitle = noteTitle;
            NoteContent = noteContent;
            NoteID = noteID;
            this.type = type;
            NoteDescription = noteDescription;
        }
    }

    public enum StateOfPlayer
    {
        Default,
        inspection,
        Die

    }
    public enum GameState
    {
        Pause,
        Play
    }

    #endregion

    [Header("Player Settings")]
    public Transform playerTF;
    public GameObject PlayerCam;
    public LayerMask playerLayerMask;
    [Space(10)]

    [Header("States")]
    public GameState gameState;
    public StateOfPlayer playerState;
    [Space(10)]

    [Header("Key Settings")]
    public KeyCode PauseKey;
    public KeyCode InteractionKey;
    [Space(10)]

    [Header("Widget Settings")]
    public GameObject PauseMenu;
    public GameObject PausePanel;
    public GameObject MainButtons;
    public GameObject ItemInspectionWidget;

    public GameObject BlackOutScreen;
    [Space(10)]

    public List<GameObject> WidgetRecord;
    float time = 0.2f;
    public float fadeDuration = 1.0f;  // 페이드가 완료되는 시간
    // Start is called before the first frame update
    void Start()
    {
        BlackOutScreen.SetActive(false);
        gameState = GameState.Play;
        Instance = this;
        //DontDestroyOnLoad(Instance);

        //DontDestroyOnLoad(playerTF);
        PauseMenu.SetActive(false);
    }

    // Update is called once per frame

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    ItemFunctionHub(2);
        //}

        //else if(Input.GetKeyDown(KeyCode.Alpha0))
        //{
        //    ItemFunctionHub(0);
        //}

        if (Input.GetKeyDown(PauseKey))
        {
            switch (playerState)
            {
                case StateOfPlayer.Default:
                    //playerState = StateOfPlayer.inspection;
                    CursorOn();

                    if (WidgetRecord.Count == 0)
                    {
                        PausePanel.SetActive(true);
                        PauseMenu.SetActive(true);
                        MainButtons.SetActive(true);
                        WidgetRecord.Add(PauseMenu);
                    }

                    else
                    {
                        ShowLastWidgetObject();
                    }
                    
                    break;

                case StateOfPlayer.inspection:
                    CursorOff();

                    ShowLastWidgetObject();
                    playerState = StateOfPlayer.Default;


                    break;
            }


        }

        
    }
    public int CreateIgnoreLayerMask(int[] layerIndices)
    {
        int mask = 0;

        foreach (int layer in layerIndices)
        {
            // 각 레이어를 비트 마스크로 변환하고 OR 연산
            mask |= 1 << layer;
        }

        // 최종 레이어 마스크 반환
        return ~mask;  // 레이캐스트가 무시할 마스크이므로 반전(~)시킴
    }
    public void ItemFunctionHub(BaseEventData eventData)
    {
        PointerEventData pointerData = eventData as PointerEventData;

        // 클릭된 UI 요소의 GameObject 확인
        GameObject clickedObject = pointerData.pointerPress;

        if (clickedObject.GetComponent<UnityEngine.UI.Image>().sprite != null)
        {
            // 클릭된 UI 요소의 이름이나 다른 속성 출력
            Debug.Log("Clicked on: " + clickedObject.name);

            int code = clickedObject.GetComponent<InventoryElemInfo>().itemInfo.id;

            switch(code)
            {
                case 0:
                    Debug.Log("Temp test");
                    break;

                case 1:
                     GameObject go = Instantiate(ItemPrefab.instance.FlashLight_1, GameObject.FindGameObjectWithTag("FlashLight").transform);
                     go.transform.localPosition = Vector3.zero;
                     //go.GetComponent<Rigidbody>().isKinematic = true;
                     go.GetComponent<BoxCollider>().enabled = false;
                     
                     go.GetComponent<FlashLightController>().batteryCanvas.SetActive(true);

                    break;

                case 2:
                    if(GameObject.FindGameObjectWithTag("FlashLight").transform.childCount == 0)
                    {
                       
                        playerTF.GetComponent<InteractionScript>().WarningMessage.SetActive(true);
                        playerTF.GetComponent<InteractionScript>().WarningMessage.GetComponent<TMP_Text>().text = "손전등을 가지고 있지 않습니다";
                        StartCoroutine(FadeInOutText(playerTF.GetComponent<InteractionScript>().WarningMessage.GetComponent<TMP_Text>()));
                        Debug.Log("손전등 없음");
                        return;
                    }

                    else
                    {
                        GameObject.FindGameObjectWithTag("FlashLight").transform.GetChild(0).GetComponent<FlashLightController>().ChargeBattery(50);
                    }
                    
                    break;

                default:
                    
                    break;
            }

            int temp = playerTF.GetComponent<PlayerInventory>().FindItenIndexInInventory(code);
            //clickedObject.GetComponent<InventoryElemInfo>().itemInfo.quantity -= 1;
            playerTF.GetComponent<PlayerInventory>().Inventory[temp].quantity -= 1;

            if (clickedObject.GetComponent<InventoryElemInfo>().itemInfo.quantity <= 0)
            {
                clickedObject.GetComponent<UnityEngine.UI.Image>().sprite = null;
                clickedObject.GetComponent<InventoryElemInfo>().itemInfo = null;
                playerTF.GetComponent<PlayerInventory>().Inventory.RemoveAt(temp);
                playerTF.GetComponent<InventoryWidget>().PrintInventoryItems();
                
                //switch(clickedObject.GetComponent<InventoryElemInfo>().itemInfo.id)
                //{
                //    case 2:

                //        playerTF.GetComponent<InventoryWidget>().ShortCutKeys[0].GetComponent < UnityEngine.UI.Image >().sprite = null;
                //        break;
                //}
            }
            
        }
    }

    public void ItemFunctionHub(int Code)
    {
        switch (Code)
        {
            case 0:
                Debug.Log("Temp test");
                break;

            case 1:
                GameObject go = Instantiate(ItemPrefab.instance.FlashLight_1, GameObject.FindGameObjectWithTag("FlashLight").transform);
                go.transform.localPosition = Vector3.zero;
                go.GetComponent<Rigidbody>().isKinematic = true;
                go.GetComponent<BoxCollider>().enabled = false;

                go.GetComponent<FlashLightController>().batteryCanvas.SetActive(true);

                break;

            case 2:
                if (GameObject.FindGameObjectWithTag("FlashLight").transform.childCount == 0)
                {

                    playerTF.GetComponent<InteractionScript>().WarningMessage.SetActive(true);
                    playerTF.GetComponent<InteractionScript>().WarningMessage.GetComponent<TMP_Text>().text = "손전등을 가지고 있지 않습니다";
                    StartCoroutine(FadeInOutText(playerTF.GetComponent<InteractionScript>().WarningMessage.GetComponent<TMP_Text>()));
                    Debug.Log("손전등 없음");
                    return;
                }

                else
                {
                    GameObject.FindGameObjectWithTag("FlashLight").transform.GetChild(0).GetComponent<FlashLightController>().ChargeBattery(50);
                }

                break;

            default:

                break;
        }

        int temp = playerTF.GetComponent<PlayerInventory>().FindItenIndexInInventory(Code);
        //clickedObject.GetComponent<InventoryElemInfo>().itemInfo.quantity -= 1;
        playerTF.GetComponent<PlayerInventory>().Inventory[temp].quantity -= 1;

        if (GameManager.Instance.playerTF.GetComponent<PlayerInventory>().Inventory[temp].quantity <= 0)
        {
            playerTF.GetComponent<PlayerInventory>().Inventory.RemoveAt(temp);
            

            switch(Code)
            {
                case 0:
                    playerTF.GetComponent<InventoryWidget>().ShortCutKeys[1].GetComponent<UnityEngine.UI.Image>().sprite = null;
                    break;

                case 2:
                    playerTF.GetComponent<InventoryWidget>().ShortCutKeys[0].GetComponent<UnityEngine.UI.Image>().sprite = null;
                    return;
                    break;
            }

            playerTF.GetComponent<InventoryWidget>().PrintInventoryItems();

        }
    }

    public void CursorOn()
    {

        GameManager.Instance.playerState = GameManager.StateOfPlayer.inspection;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CursorOff()
    {

        GameManager.Instance.playerState = GameManager.StateOfPlayer.Default;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ShowLastWidgetObject()
    {
        
        if (WidgetRecord.Count > 0)
        {
            WidgetRecord[WidgetRecord.Count - 1].SetActive(false);
            //WidgetRecord[WidgetRecord.Count - 1].SetActive(true);

            if (WidgetRecord.Count >= 2 && WidgetRecord[WidgetRecord.Count - 2] == PauseMenu)
            {
                
                //WidgetRecord[WidgetRecord.Count - 1].SetActive(false);
                MainButtons.SetActive(true);

            }


            else if (WidgetRecord[WidgetRecord.Count - 1] == ItemInspectionWidget)
            {
                
                playerState = StateOfPlayer.Default;

                InteractionScript interactionScript = playerTF.GetComponent<InteractionScript>();
                interactionScript.InteractionOn = false;
                interactionScript.InteractionObj.transform.position = interactionScript.LastLocation;
                interactionScript.InteractionObj.transform.rotation = interactionScript.LastRotation;
                interactionScript.InteractionObj = null;
            }

            else if(WidgetRecord[WidgetRecord.Count - 1] == playerTF.GetComponent<InventoryWidget>().inventoryCanvas)
            {
                playerState = StateOfPlayer.Default;
            }

            WidgetRecord.Remove(WidgetRecord[WidgetRecord.Count - 1]);

            if(WidgetRecord.Count == 0)
            {
                CursorOff();
                playerState = StateOfPlayer.Default;
            }
            //Debug.Log(WidgetRecord[WidgetRecord.Count - 1].name);

        }

        else
        {
            gameState = GameState.Play;
            playerState = StateOfPlayer.inspection;
            CursorOff();
            PauseMenu.SetActive(false);
            PausePanel.SetActive(false);
        }

        
    }

    public void AddToWidgetRecord(GameObject addWidget)
    {
        WidgetRecord.Add(addWidget);
        if(WidgetRecord.Count > 0)
        {
            playerState = StateOfPlayer.inspection;
        }
    }

    public void GameExit()
    {
        EditorApplication.isPlaying = false;
        UnityEngine.Application.Quit();
    }

    IEnumerator FadeInOutText(TMP_Text text)
    {
        // 텍스트의 알파값을 서서히 증가시키는 페이드 인 과정
        yield return StartCoroutine(FadeTextToFullAlpha(time, text));
        // 텍스트가 완전히 진해진 후 일정 시간 대기
        yield return new WaitForSeconds(1.0f);
        // 텍스트의 알파값을 서서히 감소시키는 페이드 아웃 과정
        yield return StartCoroutine(FadeTextToZeroAlpha(time, text));
    }

    public IEnumerator FadeTextToFullAlpha(float time ,TMP_Text text)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0); // 알파값을 0으로 설정
        while (text.color.a < 1.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime / time));
            yield return null;
        }
    }

    public IEnumerator FadeTextToZeroAlpha(float time, TMP_Text text)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1); // 알파값을 1로 설정
        while (text.color.a > 0.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime / time));
            yield return null;
        }
    }

    public IEnumerator GameOverAndRespawn()
    {
        yield return new WaitForSeconds(2f);
        //PlayerCam.GetComponent<Camera>().clearFlags = CameraClearFlags.Nothing;
        BlackOutScreen.SetActive(true);
        Color temp = Color.black;
        temp.a = 1.0f;
        BlackOutScreen.GetComponent<UnityEngine.UI.Image>().color = temp;
        //PlayerCam.GetComponent<Camera>().cullingMask = 0;

        yield return new WaitForSeconds(2f);
        PlayerCam.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
        BlackOutScreen.SetActive(false);
        playerState = StateOfPlayer.Default;
        //PlayerCam.GetComponent<Camera>().cullingMask = ~0;

        SceneManager.LoadScene(0);
    }

    public IEnumerator FadeOutBlackOutScreen()
    {
        float elapsedTime = 0f;
        Color currentColor = BlackOutScreen.GetComponent<UnityEngine.UI.Image>().color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);  // 알파값을 서서히 1에서 0으로 변화시킴
            BlackOutScreen.GetComponent<UnityEngine.UI.Image>().color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            yield return null;
        }

        // 페이드아웃이 완료되면 알파값을 0으로 고정
        BlackOutScreen.GetComponent<UnityEngine.UI.Image>().color = new Color(currentColor.r, currentColor.g, currentColor.b, 0);
        BlackOutScreen.SetActive(false);
    }
}

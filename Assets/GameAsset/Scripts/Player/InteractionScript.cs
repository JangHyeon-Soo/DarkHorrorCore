using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.UI.Image;

public class InteractionScript : MonoBehaviour
{
    [Header("InteractionSettings")]
    public Camera playerCam;
    public bool isInteractable;
    public bool InteractionOn;
    public LayerMask interactableLM;
    public Transform ItemOffsetPoint;

    public LineRenderer lineRenderer;
    
    public GameObject InteractionObj;
    public GameObject InteractionMessage;
    public GameObject WarningMessage;
    [Space(10)]

    float xRot = 0;
    float yRot = 0;

    [Header("Dynamic White Aim Point")]
    public GameObject WhitePoint;
    public Vector3 DefaultScale;
    public Vector3 TargetScale;
    [Space(10)]

    [Header("WidgetSettings")]
    public GameObject ItemInspectionWidget;
    public GameObject ItemTitle;
    public GameObject ItemDescription;

    public Vector3 LastLocation = Vector3.zero;
    public Quaternion LastRotation = Quaternion.identity;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        WarningMessage.SetActive(false);
        if(InteractionMessage != null )
        {
            InteractionMessage.SetActive( false );
            
        }

        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2; // 선의 점의 수
            lineRenderer.startWidth = 0.1f; // 선의 시작 너비
            lineRenderer.endWidth = 0.1f; // 선의 끝 너비
            lineRenderer.startColor = Color.red; // 선의 시작 색상
            lineRenderer.endColor = Color.red; // 선의 끝 색상
        }
    }

    // Update is called once per frame
    void Update()
    {
        #region InteractionCheck
        Ray ray = playerCam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 100f))
        {
            // 레이캐스트가 충돌한 위치를 끝 점으로 설정
            lineRenderer.SetPosition(0, ray.origin); // 시작 지점
            lineRenderer.SetPosition(1, hit.point); // 끝 지점

            if (hit.transform.gameObject.layer == 6)
            {

                InteractionMessageOn(6);
                isInteractable = true;
                InteractionObj = hit.transform.gameObject;
                
            }

            else
            {
                InteractionMessageOff();
                isInteractable = false;
                //GameManager.Instance.playerState = GameManager.StateOfPlayer.Default;
            }

         
            
        }

        else
        {
            InteractionMessageOff();
            isInteractable = false;
            //InteractionObj.GetComponent<Rigidbody>().useGravity = true;
            // 레이캐스트가 충돌하지 않은 경우, 최대 사거리 위치를 끝 점으로 설정
            lineRenderer.SetPosition(0, ray.origin); // 시작 지점
            lineRenderer.SetPosition(1, ray.origin + ray.direction * 100); // 끝 지점
            InteractionObj = null;
        }

        if (isInteractable)
        {
            Vector3 currentScale = WhitePoint.transform.localScale;
            WhitePoint.transform.localScale = Vector3.Lerp(currentScale, TargetScale, Time.deltaTime * 10f);
            if (Input.GetKeyDown(GameManager.Instance.InteractionKey) && InteractionOn == false)
            {
                InteractionOn = true;
                LastLocation = hit.transform.position;
                LastRotation = hit.transform.rotation;
                return;
            }

            else
            {
                //if (Input.GetKeyDown(GameManager.Instance.PauseKey) && InteractionOn == true)
                //{
                //    GameManager.Instance.CursorOff();
                //    InteractionOn = false;
                //    InteractionObj.transform.position = LastLocation;
                //    LastLocation = Vector3.zero;
                //}
            }
        }

        else
        {
            Vector3 currentScale = WhitePoint.transform.localScale;
            WhitePoint.transform.localScale = Vector3.Lerp(currentScale, DefaultScale, Time.deltaTime * 10f);
            InteractionOn = false;
        }

        #endregion

        if (InteractionOn && InteractionObj != null)
        {
            GameManager.Instance.playerState = GameManager.StateOfPlayer.inspection;
            GameManager.Instance.ItemInspectionWidget.SetActive(true);
            ItemTitle.SetActive(true);
            ItemDescription.SetActive(true);

            if (InteractionObj.GetComponent<MeshFilter>().mesh.bounds.size.z >= 1)
            {
                ItemOffsetPoint.localPosition = new Vector3(0, 0, InteractionObj.GetComponent<MeshFilter>().mesh.bounds.size.z * 3);
            }

            else
            {
                ItemOffsetPoint.localPosition = new Vector3(0, 0, 1);
            }
            
            
            InteractionMessageOff();
            WhitePoint.gameObject.SetActive(false);

            if(GameManager.Instance.WidgetRecord.Count == 0)
            {
                
                GameManager.Instance.WidgetRecord.Add(ItemInspectionWidget);
            }
            


            switch (InteractionObj.tag)
            {
                case "Item":
                    ItemTitle.transform.GetChild(0).GetComponent<TMP_Text>().text = InteractionObj.GetComponent<ItemScript>().name;
                    ItemDescription.transform.GetChild(0).GetComponent<TMP_Text>().text = InteractionObj.GetComponent<ItemScript>().description;
                    
                    
                    break;

                case "Note":
                    ItemTitle.transform.GetChild(0).GetComponent<TMP_Text>().text = InteractionObj.GetComponent<NoteScript>().NoteTitle;
                    ItemDescription.transform.GetChild(0).GetComponent<TMP_Text>().text = InteractionObj.GetComponent<NoteScript>().NoteDescription;
 
                    break;

            }


            // 아이템 둘러 보기
            InteractionObj.transform.position = ItemOffsetPoint.position;
            //InteractionObj.transform.rotation = inspectionPoint.rotation;

            //InteractionObj.GetComponent<Rigidbody>().isKinematic = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            GameManager.Instance.playerState = GameManager.StateOfPlayer.inspection;

            if(Input.GetMouseButton(0))
            {
                float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * 300f;
                float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * 300f;

                xRot -= mouseX;
                yRot -= mouseY;

                InteractionObj.transform.rotation = Quaternion.Euler(yRot, xRot, 0);
            }

            if(Input.GetKeyDown(GameManager.Instance.InteractionKey))
            {
                switch(hit.transform.tag)
                {
                    case "Item":
                        GameManager.Instance.playerTF.GetComponent<PlayerInventory>().AddItemToInventory(hit.transform.GetComponent<ItemScript>().GetItem());
                        //GameManager.Instance.WidgetRecord.Remove(ItemInspectionWidget);
                        break;

                    case "Note":
                        Debug.Log("Note");
                        GameManager.Instance.playerTF.GetComponent<PlayerInventory>().AddNoteToNoteList(hit.transform.GetComponent<NoteScript>().GetNote());
                        
                        
                        
                        break;

                }
                

                GameManager.Instance.playerState = GameManager.StateOfPlayer.Default;
                Destroy(InteractionObj);

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                InteractionOn = false;
                WhitePoint.gameObject.SetActive(true);
                GameManager.Instance.WidgetRecord.Remove(ItemInspectionWidget);

            }
        }

        else
        {
            // 아이템 둘러보기가 끝났을 때
            WhitePoint.gameObject.SetActive(true);
            //GameManager.Instance.playerState = GameManager.StateOfPlayer.Default;
            if (InteractionObj != null)
            {
                
                //InteractionObj.GetComponent<Rigidbody>().isKinematic = false;
                InteractionObj = null;
            }
            

            ItemTitle.SetActive(false);
            ItemDescription.SetActive(false);
            GameManager.Instance.ItemInspectionWidget.SetActive(false);

        }

    }

    void InteractionMessageOn(int hitObject_LayerMask)
    {
        InteractionMessage.SetActive(true);

        if(hitObject_LayerMask == 6)
        {
            InteractionMessage.GetComponent<TMP_Text>().text = "Press [E] to Interact";
        }

    }

    void InteractionMessageOff() 
    {
        InteractionMessage.GetComponent<TMP_Text>().text = "";
        InteractionMessage.SetActive(false);
    }
}

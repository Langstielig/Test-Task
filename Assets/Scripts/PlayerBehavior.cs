using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehavior : MonoBehaviour
{
    
    private Animator _anim;
    private CharacterController controller;

    public float speed = 100f;
    public float rotationSpeed = 720;

    public Camera firstPersonCamera;
    public Camera thirdPersonCamera;

    public Transform bathroomDoor;
    public Transform cabinetDoor;
    public Transform kitchenDoor;
    public Transform bedroomDoor;
    public Transform fridge;
    public Transform oven;

    private OpenCloseBathroomDoor openCloseBathroomDoorScript;
    private OpenCloseBedroomDoor openCloseBedroomDoorScript;
    private OpenCloseCabinetDoor openCloseCabinetDoorScript;
    private OpenCloseKitchenDoor openCloseKitchenDoorScript;
    private OpenCloseFridgeDoors openCloseFridgeDoorsScript;
    private OpenCloseOvenDoor openCloseOvenDoorScript;
    private OpenCloseDresserDoors openCloseDresserDoors;
    private ScenesScript scenesScript;

    public Toggle haveCamera;
    public Toggle haveChess;
    public Toggle haveBag;
    public Toggle haveDrink;
    public Toggle havePlate;
    public Toggle haveSheet;
    public Toggle haveDonats;
    public Toggle haveController;

    public GameObject dialogPanel;
    public Text dialogText;
    public GameObject checklist;
    private bool checklistVisibility = false;
    public GameObject finishPanel;
    public Text timeText;
    public Text countText;

    public float secundomer;
    private int countOfFoundedObjects = 0;

    void Awake() 
    {
        //rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();  
        controller = GetComponent<CharacterController>();

        openCloseBathroomDoorScript = FindObjectOfType<OpenCloseBathroomDoor>();
        openCloseBedroomDoorScript = FindObjectOfType<OpenCloseBedroomDoor>();
        openCloseCabinetDoorScript = FindObjectOfType<OpenCloseCabinetDoor>();
        openCloseKitchenDoorScript = FindObjectOfType<OpenCloseKitchenDoor>();
        openCloseFridgeDoorsScript = FindObjectOfType<OpenCloseFridgeDoors>();
        openCloseOvenDoorScript = FindObjectOfType<OpenCloseOvenDoor>();
        openCloseDresserDoors = FindObjectOfType<OpenCloseDresserDoors>();
        scenesScript = FindObjectOfType<ScenesScript>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;  
    }
    
    void Start()
    {
        thirdPersonCamera.enabled = false;
        dialogPanel.SetActive(false);
        checklist.SetActive(checklistVisibility);
        finishPanel.SetActive(false);
    }

    void Update()
    {
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        _anim.SetBool("isWalk", moveInput.magnitude > 0.1f);
        _anim.SetBool("isOpeningDoor", Input.GetKeyDown(KeyCode.E));
        _anim.SetBool("isInteracting", Input.GetMouseButtonDown(0));

        if(Input.GetKeyDown(KeyCode.F))
        {
            firstPersonCamera.enabled = !firstPersonCamera.enabled;
            thirdPersonCamera.enabled = !thirdPersonCamera.enabled;
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            checklistVisibility = !checklistVisibility;
            checklist.SetActive(checklistVisibility);
        }

        // if(Input.GetKeyDown(KeyCode.Escape))
        // {
        //     scenesScript.Scenes(0);
        // }

        InteractWithBathroomDoor();
        InteractWithBedroomDoor();
        InteractWithCabinetDoor();
        InteractWithKitchenDoor();
        InteractWithFridge();
        InteractWithOven();
    }

    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;

        moveDirection.y -= 9.81f * Time.fixedDeltaTime;

        controller.Move(moveDirection * speed * Time.fixedDeltaTime * 5);

        secundomer += Time.fixedDeltaTime; 

        //Debug.Log(secundomer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Door" || other.gameObject.tag == "DresserDoor")
        {
            dialogPanel.SetActive(true);
            dialogText.text = "E";
        }
        else if(other.gameObject.tag == "Finish") 
        {
            dialogPanel.SetActive(true);
            dialogText.text = "Для завершения игры нажмите E";    
        }
        else if(other.gameObject.tag == "Drink" && openCloseFridgeDoorsScript._isOpened)
        {
            dialogPanel.SetActive(true);
            dialogText.text = "ЛКМ";
        }
        else if(other.gameObject.tag == "Donats" && openCloseOvenDoorScript._isOpened)
        {
            dialogPanel.SetActive(true);
            dialogText.text = "ЛКМ";
        }
        else if(other.gameObject.tag == "Sheet" && openCloseDresserDoors._isOpened)
        {
            dialogPanel.SetActive(true);
            dialogText.text = "ЛКМ";
        }
        else if(other.gameObject.tag != "Untagged") 
        {
            dialogPanel.SetActive(true);
            dialogText.text = "ЛКМ";
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.gameObject.tag != "Untagged")
        {
            dialogPanel.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other) 
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(other.gameObject.tag == "DresserDoor")
            {
                Debug.Log("OpenOrCloseDresser");
                openCloseDresserDoors.OpenOrClose();
                dialogText.text = "ЛКМ";
            }

            if(other.gameObject.tag == "Finish")
            {
                dialogPanel.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Finish();
            }
        }
        else if(Input.GetMouseButtonDown(0))
        {
            if(other.gameObject.tag == "Camera") 
            {
                Debug.Log("InteractionWithCamera");
                haveCamera.isOn = true;
                Destroy(other.gameObject);
                dialogPanel.SetActive(false);
                countOfFoundedObjects++;
            }

            if(other.gameObject.tag == "Chess") 
            {
                Debug.Log("InteractionWithChess");
                haveChess.isOn = true;
                Destroy(other.gameObject);
                dialogPanel.SetActive(false);
                countOfFoundedObjects++;
            }

            if(other.gameObject.tag == "Bag") 
            {
                Debug.Log("InteractionWithBag");
                haveBag.isOn = true;
                Destroy(other.gameObject);
                dialogPanel.SetActive(false);
                countOfFoundedObjects++;
            }

            if(other.gameObject.tag == "Drink" && openCloseFridgeDoorsScript._isOpened) 
            {
                Debug.Log("InteractionWithDrink");
                haveDrink.isOn = true;
                Destroy(other.gameObject);
                dialogPanel.SetActive(false);
                countOfFoundedObjects++;
            }

            if(other.gameObject.tag == "Plate") 
            {
                Debug.Log("InteractionWithPlate");
                havePlate.isOn = true;
                Destroy(other.gameObject);
                dialogPanel.SetActive(false);
                countOfFoundedObjects++;
            }

            if(other.gameObject.tag == "Sheet" && openCloseDresserDoors._isOpened) 
            {
                Debug.Log("InteractionWithSheet");
                haveSheet.isOn = true;
                Destroy(other.gameObject);
                dialogPanel.SetActive(false);
                countOfFoundedObjects++;
            }

            if(other.gameObject.tag == "Donats" && openCloseOvenDoorScript._isOpened) 
            {
                Debug.Log("InteractionWithDonats");
                haveDonats.isOn = true;
                Destroy(other.gameObject);
                dialogPanel.SetActive(false);
                countOfFoundedObjects++;
            }

            if(other.gameObject.tag == "Controller") 
            {
                Debug.Log("InteractionWithController");
                haveController.isOn = true;
                Destroy(other.gameObject);
                dialogPanel.SetActive(false);
                countOfFoundedObjects++;
            }
        }
    }

    private void InteractWithBathroomDoor()
    {
        float dist = Vector3.Distance(bathroomDoor.position, transform.position);
        //Debug.Log(dist);
        if(dist <= 2f && Input.GetKeyDown(KeyCode.E))
        {
            //Debug.Log("OpenDoor");
            openCloseBathroomDoorScript.OpenOrClose();
        }
    }

    private void InteractWithCabinetDoor()
    {
        float dist = Vector3.Distance(cabinetDoor.position, transform.position);
        //Debug.Log(dist);
        if(dist <= 2f && Input.GetKeyDown(KeyCode.E))
        {
            //Debug.Log("OpenDoor");
            openCloseCabinetDoorScript.OpenOrClose();
        }
    }

    private void InteractWithKitchenDoor()
    {
        float dist = Vector3.Distance(kitchenDoor.position, transform.position);
        //Debug.Log(dist);
        if(dist <= 2f && Input.GetKeyDown(KeyCode.E))
        {
            //Debug.Log("OpenDoor");
            openCloseKitchenDoorScript.OpenOrClose();
        }
    }

    private void InteractWithBedroomDoor()
    {
        float dist = Vector3.Distance(bedroomDoor.position, transform.position);
        //Debug.Log(dist);
        if(dist <= 2f && Input.GetKeyDown(KeyCode.E))
        {
            //Debug.Log("OpenDoor");
            openCloseBedroomDoorScript.OpenOrClose();
        }
    }

    private void InteractWithFridge()
    {
        float dist = Vector3.Distance(fridge.position, transform.position);
        //Debug.Log(dist);
        if(dist <= 1.5f && Input.GetKeyDown(KeyCode.E))
        {
            //Debug.Log("OpenDoor");
            openCloseFridgeDoorsScript.OpenOrClose();
            dialogText.text = "ЛКМ";
        }
    }

    private void InteractWithOven()
    {
        float dist = Vector3.Distance(oven.position, transform.position);
        //Debug.Log(dist);
        if(dist <= 1.5f && Input.GetKeyDown(KeyCode.E))
        {
            //Debug.Log("OpenDoor");
            openCloseOvenDoorScript.OpenOrClose();
            dialogText.text = "ЛКМ";
        }
    }

    private void Finish()
    {
        finishPanel.SetActive(true);
        timeText.text = "Время прохождения: " + Mathf.Floor(secundomer/60) + " минут " + Mathf.Floor(secundomer - Mathf.Floor(secundomer/60)*60) + " секунд";
        countText.text = "Количество найденных предметов: " + countOfFoundedObjects + " из 8";
    }
}

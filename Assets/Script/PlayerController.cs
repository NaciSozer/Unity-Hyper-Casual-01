using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Rigidbody myBody;

    //Animation
    public float playerJump;
    Animator playerAnim;

    //Controll
    public bool onGround;
    public bool timberCol;


    public float mouseSensitivity;
    public Transform playerPos;
    public float playerSpeed;

    //Stack Mekanikleri
    [SerializeField] private float distanceBetweenObjects = 0.08f;
    [SerializeField] private Transform timberObject;
    [SerializeField] private Transform timberParentObject;
    [SerializeField] private List<GameObject> timbers;
    public bool createdtimber;
    public float timberNumber;

    //Ladders Mekanikleri
    [SerializeField] private GameObject laddersPrefab;
    [SerializeField] private GameObject laddersReferance;
    public bool createdLadders;

    

    void Start()
    {
        
        playerAnim = GetComponent<Animator>();
        myBody = this.GetComponent<Rigidbody>();
        InvokeRepeating(nameof(BridgeBuilding), 0.0f, 0.2f);

    }

    
    void Update()
    {

        MouseControl();

        Movement();


        if(timberObject == null)
        {
            timberObject = GameObject.FindGameObjectWithTag("Timber").GetComponent<Transform>();
        }

        
        if(createdtimber == false && onGround == false)//Zýplama animasyonunu baþlatma
        {
            
                transform.Translate(Vector3.up * Time.deltaTime * playerJump);
                playerAnim.SetTrigger("Jump");
                myBody.useGravity = true;
              
        }


        
        else if (timberCol)//Taþýma animasyonunu baþlatma
        {
            playerAnim.SetBool("Transport" , true);
        }


        if(this.transform.position.y <= 0)//Y posizyonu 0 ve altýna düþerse bölümü tekrarla
        {
            SceneManager.LoadScene(0);

        }

    }



    private void OnCollisionEnter(Collision collision)
    {
        

        if (collision.gameObject.CompareTag("Platform"))
        {
            onGround = true;
            createdLadders = false;
        }


    }

    private void OnCollisionExit(Collision collision)
    {
        
        onGround = false;
        createdLadders = true;


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Timber"))//Tahta ile temas olunca
        {
            TimberUp(other.gameObject, true, "Untagged", true);
            timberNumber++;
            timberCol = true;
            createdtimber = true;
            myBody.useGravity = false;
        }

    }


    public void MouseControl()//Fare mekaniði
    {
        if (Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            playerPos.Rotate(Vector3.up * mouseX);
        }
    }

    public void Movement()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * playerSpeed);
    }


    public void TimberUp(GameObject timberObjects, bool neddTag = false, string tag=null,bool down = true)//Stack Mekanikleri
    {
        if (neddTag)
        {
            timberObjects.tag = tag;
        }

        timbers.Add(timberObjects);

        timberObjects.transform.parent = timberParentObject;
        Vector3 desPos = timberObject.localPosition;
        desPos.y += down ? distanceBetweenObjects : -distanceBetweenObjects;

        timberObjects.transform.localPosition = desPos;

        timberObject = timberObjects.transform;


    }

    public void BridgeBuilding()//Merdivenlerin ayaðýn altýnda oluþturulmasý
    {
        if (onGround == false)
        {

            if (0 < timberNumber)//Tahtanýn uzunluðu kadar çalýþýr
            {

                for (int i = 0; i <= timberNumber; i++)
                {
                    Quaternion desRot = laddersReferance.transform.localRotation;

                    desRot = Quaternion.Euler(0, 90, 90);

                    Instantiate(laddersPrefab, laddersReferance.transform.position, desRot);
                    timberNumber--;

                    timbers.RemoveAt(timbers.Count - 1);

                    Destroy(timberParentObject.GetComponent<Transform>().GetChild(timberParentObject.childCount - 1).gameObject);



                    break;

                }

            }
            else
            {
                CancelInvoke(); 
                createdtimber = false;
                myBody.useGravity = true;
                transform.Translate(Vector3.up * Time.deltaTime * playerSpeed);
                
                timberNumber = 0;
                playerAnim.SetBool("Transport", false);

            }

        }


    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCode : MonoBehaviour
{
    Rigidbody myBody;

    //Karakter hareketi
    public float mouseSensitivity;
    public Transform playerPos;
    public float playerSpeed;

    public bool onGround;


    //Yerden alýnan tahtalarý ele dizme
    public Transform tahta;
    public Transform parent;
    public List<GameObject> tahtalar;
    [SerializeField] private float distanceBetweenObjects = 0.08f;
    public bool tahtaVar;
    public float tahtaSayisi;

    //Boþlukta ayaðýn altýna köprü dizme
    public GameObject merdivenPrefab;
    public GameObject merdivenReferans;
    public bool merdivenOldu;

    void Start()
    {
        myBody = this.GetComponent<Rigidbody>();

        InvokeRepeating(nameof(KopruOlustur), 0.0f, 0.2f);

         
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            playerPos.Rotate(Vector3.up * mouseX);
        }


        transform.Translate(Vector3.forward * Time.deltaTime * playerSpeed);


        if (tahta == null)
        {
            tahta = GameObject.FindGameObjectWithTag("Timber").GetComponent<Transform>();



        }




        if (tahtaVar == false)
        {
            if (onGround == false)
            {
                transform.Translate(Vector3.up * Time.deltaTime * playerSpeed);
                
            }
        }


        

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))//Tahta ile temas olunca
        {
            onGround = true;
            merdivenOldu = false;

        }



    }
    private void OnCollisionExit(Collision collision)
    {
        onGround = false;

        merdivenOldu = true;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Timber"))//Tahta ile temas olunca
        {
            TahtaEkle(other.gameObject, true, "Untagged", true);
            tahtaVar = true;
            myBody.useGravity = false;
            tahtaSayisi++;
        }

    }

    public void TahtaEkle(GameObject tahtam, bool neddTag = false, string tag = null, bool down = true)
    {
        if (neddTag)
        {
            tahtam.tag = tag;
        }


        

        tahtalar.Add(tahtam);

        tahtam.transform.parent = parent.transform;
        
        Vector3 desPos = tahta.localPosition;
        desPos.y += down ? distanceBetweenObjects : -distanceBetweenObjects;

        tahtam.transform.localPosition = desPos;

        tahta = tahtam.transform;



    }


    


    public void KopruOlustur()
    {
        if (onGround == false)
        {

            if (0 < tahtaSayisi)
            {

                for (int i = 0; i <= tahtaSayisi; i++)
                {
                    Quaternion desRot = merdivenReferans.transform.localRotation;

                    desRot = Quaternion.Euler(0, 90, 90);

                    Instantiate(merdivenPrefab, merdivenReferans.transform.position, desRot);
                    tahtaSayisi--;

                    tahtalar.RemoveAt(tahtalar.Count - 1);

                    Destroy(parent.GetComponent<Transform>().GetChild(parent.childCount - 1).gameObject);



                    break;

                }

            }
            else
            {
                CancelInvoke();
                myBody.useGravity = true;
                transform.Translate(Vector3.up * Time.deltaTime * playerSpeed);
                tahtaVar = false;
                tahtaSayisi = 0;
            }



        }


       



    }

    
   
}

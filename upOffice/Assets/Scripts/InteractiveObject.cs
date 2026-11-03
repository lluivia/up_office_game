using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractiveObject : MonoBehaviour
{
    bool isHolding; // anlık olarak elimizde tutuyor muyuz anlamak için
    [SerializeField] GameObject target; // taşıyabileceğimiz objeler 'target' olacak

    [SerializeField] float swayAmount = 0.05f; // objenin tutulurken ne kadar esneyeceği
    [SerializeField] float smoothAmount = 6f; // tutulurken ne kadar yumuşak döneceği
    [SerializeField] float tiltAmount = 0.2f; // obje tutulurken y ekseninde ufak hareketi
    [SerializeField] float horizontalInteractRange = 0.6f;
    [SerializeField] float verticalInteractRange = 2f;


    void Update()
    {
        Vector2 targetHorizontal = new Vector2(target.transform.position.x, target.transform.position.z);
        Vector2 objectHorizontal = new Vector2(this.transform.position.x, this.transform.position.z);
        float currentHorizontalRange = Vector2.Distance(targetHorizontal, objectHorizontal);

        float currentVerticalRange = Mathf.Abs(target.transform.position.y - this.transform.position.y);
        
        bool isNear = (currentHorizontalRange <= horizontalInteractRange) && (currentVerticalRange <= verticalInteractRange);

        if (Input.GetKeyDown(KeyCode.Mouse0) && isNear == true && isHolding == false)
        {
            isHolding = true;
            this.transform.parent = target.transform;
            this.transform.localEulerAngles = new Vector3(0, 0, 0);
            this.GetComponent<Rigidbody>().isKinematic = true;
            Debug.Log("Obje alındı.");
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && isHolding == true)
        {
            isHolding = false;
            this.transform.parent = null;
            this.GetComponent<Rigidbody>().isKinematic = false;
            Debug.Log("Obje bırakıldı.");
        }

        if (isHolding == true)
        {
            float mouseX = -Input.GetAxis("Mouse X") * swayAmount;
            float mouseY = -Input.GetAxis("Mouse Y") * swayAmount;

            float tiltY = Camera.main.transform.forward.y * tiltAmount;
            Vector3 finalPosition = new Vector3(mouseX, mouseY + tiltY, 0);

            this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, finalPosition, Time.deltaTime * smoothAmount);
            
        }
    }

    void OnTriggerEnter(Collider other)
    {
       
    }

    void OnTriggerExit(Collider other)
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    bool isActive;
    [SerializeField] float speed = 1f;
    [SerializeField] float rotate = 100f;


    public void StartMoving()
    {
        isActive = true;
        StartCoroutine(Moving());
    }

    public void EndMoving()
    {
        isActive = false;
    }

    IEnumerator Moving()
    {
        while (isActive)
        {
            float x = Input.GetAxis("Horizontal") * Time.deltaTime;
            float z = Input.GetAxis("Vertical") * Time.deltaTime;
            Vector3 move = /*transform.right * x + */transform.forward * z;

            gameObject.transform.Rotate(Vector3.up * x * rotate);
            gameObject.transform.position += move * speed;
            yield return new WaitForEndOfFrame();
        }
    }
}

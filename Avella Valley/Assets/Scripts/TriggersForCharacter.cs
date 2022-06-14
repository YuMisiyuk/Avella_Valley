using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggersForCharacter : MonoBehaviour
{
    [SerializeField] private GameObject doorButton;

    private void Start()
    {
        doorButton.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnterToHome"))
        {
            doorButton.SetActive(true);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        doorButton.SetActive(true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("EnterToHome"))
        {
            doorButton.SetActive(false);
        }
    }
}

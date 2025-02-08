using System;
using UnityEngine;
using TMPro;
public class ButtonElevator : MonoBehaviour
{
    public GameObject elevator;
    public GameObject player;
    public float pressDepth = 0.001f;
    public float returnSpeed = 5f;
    public float holdTime = 0.5f;
    public float elevatorMoveDistance = 10f;
    public float elevatorMoveSpeed = 2f;

    private Vector3 initialPosition;
    private bool isPressed = false;
    private float pressStartTime = 0f;
    private bool actionTriggered = false;
    public GameObject btnDown;
    public GameObject btnUp;
    public TextMeshProUGUI heightText;
    
    private void Start()
    {
        initialPosition = transform.localPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finger") && !isPressed)
        {
            isPressed = true;
            transform.localPosition = initialPosition + new Vector3(pressDepth, 0, 0);
            pressStartTime = Time.time;
            actionTriggered = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isPressed && !actionTriggered && Time.time - pressStartTime >= holdTime)
        {
            OnButtonHeld();
            actionTriggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Finger"))
        {
            isPressed = false;
            actionTriggered = false;
        }
    }

    private void Update()
    {
        if (!isPressed)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition, Time.deltaTime * returnSpeed);
        }
    }

    private void OnButtonHeld()
    {
        Debug.Log("Кнопка удержана 0.5 сек! Выполняем действие...");

        Elevator elevatorScript = elevator.GetComponent<Elevator>();
        if (elevatorScript.isDoorsOpen)
        {
            if(btnUp == gameObject && elevatorScript.height <= 50){
                elevatorScript.ToggleDoors();
                StartCoroutine(MoveElevatorUpAfterDoorsClose(elevatorScript));
            }
            else if (btnDown == gameObject && elevatorScript.height != 0){
                elevatorScript.ToggleDoors();
                StartCoroutine(MoveElevatorDownAfterDoorsClose(elevatorScript));
            }
        }
    }

    private System.Collections.IEnumerator MoveElevatorUpAfterDoorsClose(Elevator elevatorScript)
    {
        // Ждём, пока двери закроются
        while ((elevatorScript.leftDoor.transform.position != elevatorScript.closedLeftPosition.position) 
            || (elevatorScript.rightDoor.transform.position != elevatorScript.closedRightPosition.position))
        {
            yield return null;
        }

        yield return new WaitForSeconds(1f); // Короткая пауза перед движением

        Debug.Log("Двери закрылись. Лифт поднимается!");
        
        // Делаем игрока дочерним объектом лифта
        if (player != null)
        {
            player.transform.SetParent(elevator.transform);
        }

        Vector3 targetPosition = elevator.transform.position + new Vector3(0, elevatorMoveDistance, 0);
        while (Vector3.Distance(elevator.transform.position, targetPosition) > 0.01f)
        {
            elevator.transform.position = Vector3.MoveTowards(elevator.transform.position, targetPosition, Time.deltaTime * elevatorMoveSpeed);
            yield return null;
        }

        Debug.Log("Лифт поднялся на 10 метров.");
        elevatorScript.height += 10;
        heightText.text = elevatorScript.height + " метров";
        // Открепляем игрока от лифта
        if (player != null)
        {
            player.transform.SetParent(null);
        }

        elevatorScript.ToggleDoors();
    }

    private System.Collections.IEnumerator MoveElevatorDownAfterDoorsClose(Elevator elevatorScript)
    {
        // Ждём, пока двери закроются
        while ((elevatorScript.leftDoor.transform.position != elevatorScript.closedLeftPosition.position) 
            || (elevatorScript.rightDoor.transform.position != elevatorScript.closedRightPosition.position))
        {
            yield return null;
        }

        yield return new WaitForSeconds(1f); // Короткая пауза перед движением

        Debug.Log("Двери закрылись. Лифт опускается!");

        // Делаем игрока дочерним объектом лифта
        if (player != null)
        {
            player.transform.SetParent(elevator.transform);
        }

        Vector3 targetPosition = elevator.transform.position - new Vector3(0, elevatorMoveDistance, 0);
        while (Vector3.Distance(elevator.transform.position, targetPosition) > 0.01f)
        {
            elevator.transform.position = Vector3.MoveTowards(elevator.transform.position, targetPosition, Time.deltaTime * elevatorMoveSpeed);
            yield return null;
        }

        Debug.Log("Лифт опустился на 10 метров.");
        elevatorScript.height -= 10;
        heightText.text = elevatorScript.height + " метров";
        // Открепляем игрока от лифта
        if (player != null)
        {
            player.transform.SetParent(null);
        }

        elevatorScript.ToggleDoors();
    }

}

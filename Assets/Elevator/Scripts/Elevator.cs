using System.Collections;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public bool isDoorsOpen = true;
    public GameObject leftDoor;
    public GameObject rightDoor;
    public Transform closedLeftPosition;
    public Transform closedRightPosition;
    public Transform openedLeftPosition;
    public Transform openedRightPosition;
    public float doorSpeed = 0.5f; // скорость движения дверей
    public int height = 0;

    private void Start()
    {
        if (isDoorsOpen)
        {
            SetDoorsPosition(openedLeftPosition.localPosition, openedRightPosition.localPosition);
        }
        else
        {
            SetDoorsPosition(closedLeftPosition.localPosition, closedRightPosition.localPosition);
        }
    }

    private void SetDoorsPosition(Vector3 leftPos, Vector3 rightPos)
    {
        leftDoor.transform.localPosition = leftPos;
        rightDoor.transform.localPosition = rightPos;
    }

    private IEnumerator MoveDoors(Vector3 leftTargetPosition, Vector3 rightTargetPosition)
    {
        Vector3 leftStartPosition = leftDoor.transform.localPosition;
        Vector3 rightStartPosition = rightDoor.transform.localPosition;
        float timeElapsed = 0f;

        while (timeElapsed < 1f)
        {
            leftDoor.transform.localPosition = Vector3.Lerp(leftStartPosition, leftTargetPosition, timeElapsed);
            rightDoor.transform.localPosition = Vector3.Lerp(rightStartPosition, rightTargetPosition, timeElapsed);
            timeElapsed += Time.deltaTime * doorSpeed;
            yield return null;
        }

        SetDoorsPosition(leftTargetPosition, rightTargetPosition);
    }

    public void ToggleDoors()
    {
        isDoorsOpen = !isDoorsOpen;
        if (isDoorsOpen)
        {
            StartCoroutine(MoveDoors(openedLeftPosition.localPosition, openedRightPosition.localPosition));
        }
        else
        {
            StartCoroutine(MoveDoors(closedLeftPosition.localPosition, closedRightPosition.localPosition));
        }
    }
}

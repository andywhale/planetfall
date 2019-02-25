using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControlButtons : MonoBehaviour {

    private bool dragging = false;
    private float dragStartX;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void MoveUp()
    {
        GameObject.FindWithTag("Player").GetComponent<MazePlayer>().SetMoveUp();
    }

    public void MoveDown()
    {
        GameObject.FindWithTag("Player").GetComponent<MazePlayer>().SetMoveDown();
    }

    public void MoveLeft()
    {
        GameObject.FindWithTag("Player").GetComponent<MazePlayer>().SetMoveLeft();
    }

    public void MoveRight()
    {
        GameObject.FindWithTag("Player").GetComponent<MazePlayer>().SetMoveRight();
    }

    public void MoveReset()
    {
        GameObject.FindWithTag("Player").GetComponent<MazePlayer>().ResetMove();
    }

    public void SwitchCamera()
    {
        GameObject.FindWithTag("Player").GetComponent<MazePlayer>().SwitchCamera();
    }

    public void StartClickAndDrag(BaseEventData data)
    {
        dragging = true;
        PointerEventData pointerData = data as PointerEventData;
        dragStartX = pointerData.position.x;
        //GameObject.FindWithTag("Player").GetComponent<MazePlayer>().StartMoveForward();
    }

    public void EndClickAndDrag(BaseEventData data)
    {
        dragging = false;
        GameObject.FindWithTag("Player").GetComponent<MazePlayer>().EndRotation();
    }

    public void ClickAndDrag(BaseEventData data)
    {
        PointerEventData pointerData = data as PointerEventData;
        if (pointerData.IsPointerMoving() && dragging)
        {
            GameObject.FindWithTag("Player").GetComponent<MazePlayer>().StartRotation(pointerData.position.x - dragStartX);
        }

    }

    public void MoveForward()
    {
        GameObject.FindWithTag("Player").GetComponent<MazePlayer>().StartMoveForward();
    }

    public void StopMoveForward()
    {
        GameObject.FindWithTag("Player").GetComponent<MazePlayer>().EndMoveForward();
    }

}

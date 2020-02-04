using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JumpScript : MonoBehaviour
{

	public void Highlight()
    {
        GetComponent<Material>().color = new Color32(255, 0, 0, 255);
    }
}

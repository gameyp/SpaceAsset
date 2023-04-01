using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUIController : MonoBehaviour
{
    public Canvas healthCanvas;

    public void SetHealthCanvasActive(bool active)
    {
        healthCanvas.enabled = active;
    }
}

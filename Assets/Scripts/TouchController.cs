using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class TouchController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var touch in Touch.activeTouches)
        {
            Debug.Log($"{touch.touchId}: {touch.screenPosition}, {touch.phase}");
        }
    }
}

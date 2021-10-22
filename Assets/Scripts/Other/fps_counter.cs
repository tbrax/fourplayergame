using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fps_counter : MonoBehaviour
{
    public int avgFrameRate;
    public TMPro.TextMeshProUGUI display_Text;

    public void Update()
    {
        float current = 0;
        current = (int)(1f / Time.unscaledDeltaTime);
        avgFrameRate = (int)current;
        display_Text.text = avgFrameRate.ToString() + " FPS";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScale : MonoBehaviour {
    public SpriteRenderer background;

    float screenRatio, savedRatio = 0;

	void Update(){
        screenRatio = (float)Screen.width / (float)Screen.height;
        if (savedRatio != screenRatio){
            savedRatio = screenRatio; 
            AdjustCamera();
        }
	}

    void AdjustCamera(){
        float targetRatio = background.bounds.size.x / background.bounds.size.y;

        if (screenRatio >= targetRatio){
            Camera.main.orthographicSize = background.bounds.size.y / 2;
        } else {
            float differenceInSize = targetRatio / screenRatio;
            Camera.main.orthographicSize = background.bounds.size.y / 2 * differenceInSize;
        }
    }
}

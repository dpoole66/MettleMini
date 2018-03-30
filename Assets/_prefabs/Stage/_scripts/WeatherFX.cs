using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherFX : MonoBehaviour {

    public Light lightning;
    private float[] smoothing = new float[20];        

	void Start () {

        for(int i = 0; i < smoothing.Length; i++){

                smoothing[i] = .0f;                         // add array smoothing for intensity values. init all in it to .0f

        }
		
	}

    void Update() {

        float sum = .0f;

        for (int i = 1; i < smoothing.Length; i++) {

            smoothing[i - 1] = smoothing[i];        // move up the array from pos 1 (not 0) subtracting 1 each step. 
            sum += smoothing[i - 1];                     // switching 1 to 0, 2 to 1 and so on until 20 becomes 19 leaving 20 blank.
        }

        smoothing[smoothing.Length - 1] = Random.value;
        sum += smoothing[smoothing.Length - 1];          // add a random value at slot 20 in the array.

        lightning.intensity = (sum / smoothing.Length) * 4;       // get the array's average and give that value to the Light intensity.

        if (lightning.intensity <= 2.5) {

            lightning.enabled = false;

        } else lightning.enabled = true;
    }

}

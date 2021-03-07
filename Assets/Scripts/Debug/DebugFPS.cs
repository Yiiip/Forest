// #define DEBUG_FPS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugFPS : MonoBehaviour
{

	public Text TextFPS;

	private float mLastUpdateTime;

	//FPS
	private float frames;
	private float updateInterval = 1.0f;
	private float timeLeft;
	private string fpsStr;
	private float accum;

	void Start()
	{
#if DEBUG_FPS
        TextFPS.gameObject.SetActive(true);
#else
        TextFPS.gameObject.SetActive(false);
#endif
    }

#if DEBUG_FPS
    void Update()
	{
		if (TextFPS != null && TextFPS.gameObject.activeSelf)
		{
			timeLeft -= Time.deltaTime;

			accum += Time.timeScale / Time.deltaTime;
			frames++;

			if (timeLeft <= 0)
			{
				var fps = accum / frames;
				fpsStr = $"FPS: {fps:F0}";
				frames = 0;
				accum = 0;
				timeLeft = updateInterval;
			}
		}

		if (Time.time - mLastUpdateTime > 0.2f)
		{
			mLastUpdateTime = Time.time;

			if (TextFPS != null && TextFPS.gameObject.activeSelf)
			{
				TextFPS.text = fpsStr;
			}
		}
	}
#endif
}
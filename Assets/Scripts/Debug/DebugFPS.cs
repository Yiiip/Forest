// #define DEBUG_FPS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugFPS : SingletonMono<DebugFPS>
{

	public Text TextFPS;

	private float mLastUpdateTime;

	//FPS
	private float frames;
	private float updateInterval = 1.0f;
	private float timeLeft;
	private string fpsStr;
	private float accum;

	public bool IsAllow { get; set; } = false;

	void OnEnable()
	{
#if DEBUG_FPS
		IsAllow = true;
#endif
	}

	void Update()
	{
		if (TextFPS == null)
		{
			return;
		}

		if (!IsAllow)
		{
			TextFPS.text = string.Empty;
			fpsStr = null;
			return;
		}

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

		if (Time.time - mLastUpdateTime > 0.2f)
		{
			mLastUpdateTime = Time.time;

			TextFPS.text = fpsStr;
		}
	}
}
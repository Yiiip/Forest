using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : SingletonMonoEntire<TimerManager>
    {
        public static List<TimerEvent> m_timers = new List<TimerEvent>();

        public void Close()
        {
            m_timers.Clear();
        }

        public void Init()
        {
            m_timers.Clear();
        }

        public void Update()
        {
            for (int i = 0; i < m_timers.Count; i++)
            {
                m_timers[i].Update();

                if (m_timers[i].m_isDone)
                {
                    TimerEvent e = m_timers[i];

                    e.CompleteTimer();

                    if (e.m_repeatCount == 0)
                    {
                        m_timers.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        public static bool GetIsExistTimer(string timerName)
        {
            for (int i = 0; i < m_timers.Count; i++)
            {
                if (m_timers[i].m_timerName == timerName)
                {
                    return true;
                }
            }
            return false;
        }

        public static TimerEvent GetTimer(string timerName)
        {
            for (int i = 0; i < m_timers.Count; i++)
            {
                if (m_timers[i].m_timerName == timerName)
                {
                    return m_timers[i];
                }
            }

            throw new System.Exception("Get Timer  Exception not find ->" + timerName + "<-");
        }

        /// <summary>
        /// 延迟调用
        /// </summary>
        /// <param name="delayTime">间隔时间</param>
        /// <param name="callBack">回调函数</param>
        /// <param name="objs">回调函数的参数</param>
        /// <returns></returns>
        public static TimerEvent DelayCallBack(float delayTime, TimerCallBack callBack, params object[] objs)
        {
            return TimerManager.Instance.AddTimer(delayTime, false, 0, null, callBack, objs);
        }

        /// <summary>
        /// 延迟调用
        /// </summary>
        /// <param name="delayTime">间隔时间</param>
        /// <param name="isIgnoreTimeScale">是否忽略时间缩放</param>
        /// <param name="callBack">回调函数</param>
        /// <param name="objs">回调函数的参数</param>
        /// <returns></returns>
        public static TimerEvent DelayCallBack(float delayTime, bool isIgnoreTimeScale, TimerCallBack callBack, params object[] objs)
        {
            return TimerManager.Instance.AddTimer(delayTime, isIgnoreTimeScale, 0, null, callBack, objs);
        }

        /// <summary>
        /// 间隔一定时间重复调用
        /// </summary>
        /// <param name="intervalTime">间隔时间</param>
        /// <param name="callBack">回调函数</param>
        /// <param name="objs">回调函数的参数</param>
        /// <returns></returns>
        public static TimerEvent CallBackOfIntervalTimer(float intervalTime, TimerCallBack callBack, params object[] objs)
        {
            return TimerManager.Instance.AddTimer(intervalTime, false, -1, null, callBack, objs);
        }

        /// <summary>
        /// 间隔一定时间重复调用
        /// </summary>
        /// <param name="intervalTime">间隔时间</param>
        /// <param name="isIgnoreTimeScale">是否忽略时间缩放</param>
        /// <param name="callBack">回调函数</param>
        /// <param name="objs">回调函数的参数</param>
        /// <returns></returns>
        public static TimerEvent CallBackOfIntervalTimer(float intervalTime, bool isIgnoreTimeScale, TimerCallBack callBack, params object[] objs)
        {
            return TimerManager.Instance.AddTimer(intervalTime, isIgnoreTimeScale, -1, null, callBack, objs);
        }

        /// <summary>
        /// 间隔一定时间重复调用
        /// </summary>
        /// <param name="intervalTime">间隔时间</param>
        /// <param name="isIgnoreTimeScale">是否忽略时间缩放</param>
        /// <param name="timerName">Timer的名字</param>
        /// <param name="callBack">回调函数</param>
        /// <param name="objs">回调函数的参数</param>
        /// <returns></returns>
        public static TimerEvent CallBackOfIntervalTimer(float intervalTime, bool isIgnoreTimeScale, string timerName, TimerCallBack callBack, params object[] objs)
        {
            return TimerManager.Instance.AddTimer(intervalTime, isIgnoreTimeScale, -1, timerName, callBack, objs);
        }

        /// <summary>
        /// 有限次数的重复调用
        /// </summary>
        /// <param name="intervalTime">间隔时间</param>
        /// <param name="callBackCount">重复调用的次数</param>
        /// <param name="callBack">回调函数</param>
        /// <param name="objs">回调函数的参数</param>
        /// <returns></returns>
        public static TimerEvent CallBackOfIntervalTimer(float intervalTime, int callBackCount, TimerCallBack callBack, params object[] objs)
        {
            return TimerManager.Instance.AddTimer(intervalTime, false, -1, null, callBack, objs);
        }

        /// <summary>
        /// 有限次数的重复调用
        /// </summary>
        /// <param name="intervalTime">间隔时间</param>
        /// <param name="isIgnoreTimeScale">是否忽略时间缩放</param>
        /// <param name="callBackCount">重复调用的次数</param>
        /// <param name="callBack">回调函数</param>
        /// <param name="objs">回调函数的参数</param>
        /// <returns></returns>
        public static TimerEvent CallBackOfIntervalTimer(float intervalTime, bool isIgnoreTimeScale, int callBackCount, TimerCallBack callBack, params object[] objs)
        {
            return TimerManager.Instance.AddTimer(intervalTime, isIgnoreTimeScale, -1, null, callBack, objs);;
        }

        /// <summary>
        /// 有限次数的重复调用
        /// </summary>
        /// <param name="intervalTime">间隔时间</param>
        /// <param name="isIgnoreTimeScale">是否忽略时间缩放</param>
        /// <param name="callBackCount">重复调用的次数</param>
        /// <param name="timerName">Timer的名字</param>
        /// <param name="callBack">回调函数</param>
        /// <param name="objs">回调函数的参数</param>
        /// <returns></returns>
        public static TimerEvent CallBackOfIntervalTimer(float intervalTime, bool isIgnoreTimeScale, int callBackCount, string timerName, TimerCallBack callBack, params object[] objs)
        {
            return TimerManager.Instance.AddTimer(intervalTime, isIgnoreTimeScale, -1, timerName, callBack, objs);
        }

        /// <summary>
        /// 添加一个Timer
        /// </summary>
        /// <param name="intervalTime">间隔时间</param>
        /// <param name="isIgnoreTimeScale">是否忽略时间缩放</param>
        /// <param name="callBackCount">重复调用的次数</param>
        /// <param name="timerName">Timer的名字</param>
        /// <param name="callBack">回调函数</param>
        /// <param name="objs">回调函数的参数</param>
        /// <returns></returns>
        private TimerEvent AddTimer(float intervalTime, bool isIgnoreTimeScale, int callBackCount, string timerName, TimerCallBack callBack, params object[] objs)
        {
            TimerEvent te = new TimerEvent();

            te.m_timerName = timerName ?? te.GetHashCode().ToString();
            te.m_currentTimer = 0;
            te.m_intervalTime = intervalTime;

            te.m_callBack = callBack;
            te.m_objs = objs;

            te.m_isIgnoreTimeScale = isIgnoreTimeScale;
            te.m_repeatCount = callBackCount;

            m_timers.Add(te);

            return te;
        }

        public static void DestroyTimer(TimerEvent timer, bool isCallBack = false)
        {
            if (m_timers.Contains(timer))
            {
                if (isCallBack)
                {
                    timer.CallBackTimer();
                }

                m_timers.Remove(timer);
            }
            else
            {
                Debug.LogError("Timer DestroyTimer error: don't exist timer " + timer);
            }
        }

        public static void DestroyTimer(string timerName, bool isCallBack = false)
        {
            //Debug.Log("DestroyTimer2  ----TIMER " + timerName);
            for (int i = 0; i < m_timers.Count; i++)
            {
                if (m_timers[i].m_timerName.Equals(timerName))
                {
                    DestroyTimer(m_timers[i], isCallBack);
                }
            }
        }

        public static void DestroyAllTimer(bool isCallBack = false)
        {
            for (int i = 0; i < m_timers.Count; i++)
            {
                if (isCallBack)
                {
                    m_timers[i].CallBackTimer();
                }
            }

            m_timers.Clear();
        }

        public static void ResetTimer(TimerEvent timer)
        {
            if (m_timers.Contains(timer))
            {
                timer.ResetTimer();
            }
            else
            {
                Debug.LogError("Timer ResetTimer error: don't exist timer " + timer);
            }
        }

        public static void ResetTimer(string timerName)
        {
            for (int i = 0; i < m_timers.Count; i++)
            {
                if (m_timers[i].m_timerName.Equals(timerName))
                {
                    ResetTimer(m_timers[i]);
                }
            }
        }
    }
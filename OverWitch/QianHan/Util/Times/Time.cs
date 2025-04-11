
using System;
using System.Runtime.CompilerServices;

namespace Assets.OverWitch.QianHan.Util.Times
{
    /// <summary>
    /// 仿照Untiy的Time编写，模拟Untiy的Time类，源码灵感来源：借鉴Unity的Time类进行模拟实现
    /// </summary>
    [StaticAccessor("getTimeManager()",StaticAccessorType.Dot)]
    public static class Time
    {
        public static float Times => TimeManager.time;
        public static float DeltaTime => TimeManager.deltaTime;
        public static float timeScale
        {
            get => TimeManager.timeScale;
            set => TimeManager.timeScale = value;
        }
        /// <summary>
        /// 下面方法中，除了带有注释的都是为了区分外部库
        /// </summary>
        /// <param name="delta"></param>
        public static void Update(float delta)
        {
            TimeManager.Update(delta);
        }
        //本类已经有了实现，这是另一个方便不同使用
        public static float time
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public static double TimeAsDouble
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public static float TimeSinceLevelLoad
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public static double TimeSinceLevelLoadAsDouble
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }
        //本类已经有了实现，这是另一个方便不同使用
        public static float deltaTime
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public static float FixedTime
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public static double FixedTimeAsDouble
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public static float UnscaledTime
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public static double UnscaledTimeAsDouble
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public static float FixedUnscaledTime
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public static double FixedUnscaledTimeAsDouble
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public static float UnscaledDeltaTime
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public static float FixedUnscaledDeltaTime
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public static float FixedDeltaTime
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public static float MaximumDeltaTime
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public static float SmoothDeltaTime
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public static float MaximumParticleDeltaTime
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public static float TimeScale
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public static int FrameCount
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public static int RenderedFrameCount
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public static float RealtimeSinceStartup
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public static double RealtimeSinceStartupAsDouble
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public static float CaptureDeltaTime
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public static int CaptureFramerate
        {
            get
            {
                return (CaptureDeltaTime != 0f) ? ((int)MathF.Round(1f / CaptureDeltaTime)) : 0;
            }
            set
            {
                CaptureDeltaTime = ((value == 0) ? 0f : (1f / (float)value));
            }
        }

        public static bool InFixedTimeStep
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }
    }
    public static class TimeManager 
    {
        public static float time { get; private set; }
        public static float deltaTime { get; private set; }
        public static float timeScale { get; set; } = 1f;

        // 更新方法，每帧调用
        public static void Update(float delta)
        {
            deltaTime = delta * timeScale;
            time += deltaTime;
        }
    }
}

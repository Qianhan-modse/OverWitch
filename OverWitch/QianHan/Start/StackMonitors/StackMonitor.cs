using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Assets.OverWitch.QianHan.Events.fml.common.eventhandler;

namespace Assets.OverWitch.QianHan.Start.StackMonitors
{
    /// <summary>
    /// 栈监视器，主要用于对栈空间的监控
    /// 特别提醒，若使用这个类，也就意味着您已经同意并许可对您计算机的部分权限开放，这是一个危险的操作，若不了解，请不要使用这个类
    /// </summary>
    public class StackMonitor : IDisposable
    {
        //虽然这里有一些不安全的代码，但是这里的代码是安全的，因为这里的代码是用监控栈空间的，所以这里的代码是安全的
#pragma warning disable CS0626 // 方法、运算符或访问器标记为外部对象并且上面没有任何特性
        private static extern void getCurrentThreadStackLimits(out uint lowLimit, out uint highLimit);
        private bool disposedValue;
        private static extern IntPtr intPtr(int level);
#pragma warning restore CS0626 // 方法、运算符或访问器标记为外部对象并且上面没有任何特性
        private Thread thread;
        //是否正在运行
        private volatile bool isRunning;
        //栈空间的阈值
        private const float STACK_THRESHOLD = 0.95f;
        //栈空间监控的事件
        public event Action<StackOverflowException> onStackCritical = delegate { };

        /// <summary>
        /// 初始化栈监视器
        /// </summary>
        public StackMonitor()
        {
            //启动监控
            isRunning = true;
            //创建一个线程
            thread = new Thread(MonitoringLoop)
            {
                //设置线程的优先级
                Priority = ThreadPriority.Highest,
                //设置线程为后台线程
                IsBackground = true
            };
            //启动线程
            thread.Start();
        }
        /// <summary>
        /// 监控循环
        /// </summary>
        public void MonitoringLoop()
        {
            //循环监控
            while (isRunning)
            {
                //检查栈空间
                try
                {
                    CheckStackByManaged();
                    CheckStackByUnmanaged();
                    CheckStackByInstrumentation();
                }
                //如果出现异常
                catch (Exception ex)
                {
                    //触发栈空间监控异常
                    onStackCritical(new StackOverflowException("监控异常", ex));
                }
                //休眠10毫秒
                Thread.Sleep(10);
            }
        }
        private void CheckStackByManaged()
        {
            int depth = CalculateCallStackDepth();
            if(depth>500)
            {
                TriggerEmergencyProtocol();
            }
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        private int CalculateCallStackDepth()
        {
            return new StackTrace().FrameCount;
        }
        /// <summary>
        /// 通过托管代码检查栈空间,请注意，这个方法是不安全的，因为这个方法是用于监控栈空间，因为内部使用了指针，所以出于安全考虑，在非必要的情况下，请不要使用这个方法
        /// </summary>
        private unsafe void CheckStackByUnmanaged()
        {
            //获取栈的低地址和高地址
            uint low, high;
            //获取当前线程的栈的低地址和高地址
            getCurrentThreadStackLimits(out low, out high);
            //确保栈范围有效
            if (high-low<4096)
            {
                //触发栈空间监控异常
                onStackCritical(new StackOverflowException("栈空间检测异常"));
                //返回,什么都不做
                return;
            }
            //被注释的代码逻辑是存在可能的问题，目前暂时停用
            //uint* stackData = stackalloc uint[1];
            //stackData[0] = low;
            uint[] data = new uint[] { low };
            //使用指针,风险等级高，非特殊情况下不要使用
            fixed (uint* pLow = data)//&stackData[0])这一个是错误的被额外标出，但具体是否不用尚未确定
            {
                //获取当前栈的地址
                uint current = (uint)pLow;
                float usage = (current - low) / (float)(high - low);
                //如果栈的使用率超过阈值
                if (usage>=STACK_THRESHOLD)
                {
                    TriggerEmergencyProtocol();
                }
            }
        }
        //该逻辑目前暂时停用
        /*private unsafe void CheckStackByManaged()
        {
            uint low, high;
            getCurrentThreadStackLimits(out low, out high);

            if (high <= low) // 确保栈范围有效
            {
                onStackCritical(new StackOverflowException("栈空间检测异常"));
                return;
            }

            if (high - low < 4096) // 最小安全栈空间
            {
                onStackCritical(new StackOverflowException("栈空间检测异常"));
                return;
            }

            uint* pLow = &low; // 不使用 fixed
            UIntPtr current = (UIntPtr)pLow;

            ulong usedStack = (ulong)current.ToUInt64() - low; // 防止溢出
            float usage = usedStack / (float)(high - low);

            if (usage >= STACK_THRESHOLD)
            {
                TriggerEmergencyProtocol();
            }
        }*/
        /// <summary>
        /// 触发紧急协议
        /// </summary>
        private void TriggerEmergencyProtocol()
        {
            //如果栈空间突破阈值，触发紧急协议
            if (LegalGuardian.ValidateOperation("STACK_OVERFLOW_TERMINATE"))
            {
                Debug.WriteLine("栈空间突破阈值，触发紧急协议");
                ExectueControlledCrash();
            }
        }
        /// <summary>
        /// 通过检测栈空间的方法
        /// </summary>
        private void CheckStackByInstrumentation()
        {
            try
            {
                var stackTrace = new StackTrace(true);
                if (stackTrace.FrameCount > 500)
                    TriggerEmergencyProtocol();
            }
            catch
            {
                return;
            }
        }
        /// <summary>
        /// 执行受控崩溃
        /// </summary>
        /// <exception cref="ApplicationException"></exception>
        private void ExectueControlledCrash()
        {
            //停止应用程序
            ApplicationStop.Stop();
            throw new ApplicationException("栈空间监控触发强制崩溃");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                }

                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~StackMonitor()
        // {
        //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    /*internal static class LegalGuardian
    {
        private static readonly ConcurrentDictionary<string, DateTime> operations = new();
        private static readonly byte[] stackTrace = GetOrgkey();
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static byte[]GetOrgkey()
        {
            return 
                Encoding.UTF8.GetBytes(Guid.NewGuid().ToString() 
                + 
                Environment.MachineName.GetHashCode().ToString("x8"));
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool ValidateTopology()
        {
            var stack = new StackTrace(skipFrames: 2, fNeedFileInfo: false);
            return stack.GetFrames()?.All(f => !f.GetMethod()?.DeclaringType?.Namespace?.Contains("Unsafe") ?? true) ?? true;
        }
        public static bool ValidateOperation(string operationCode)
        {
            return ValidateTopology()&&VerifyOperationSignature(operationCode)&&CheckRuntimeIntegrity();
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool VerifyOperationSignature(string code)
        {
            try
            {
                using var hmac = new HMACSHA256(stackTrace);
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(code));
                var timestamp = BitConverter.ToInt64(hash, 0) % 30000;
                return Math.Abs(Environment.TickCount - timestamp) < 5000;
            }
            catch
            {
                return false;
            }
        }
        /*[MethodImpl(MethodImplOptions.NoInlining)]
        private static bool CheckRuntimeIntegrity()
        {
            if(Debugger.IsAttached||Environment.GetEnvironmentVariable("COR_ENABLE_PROFILING")=="1")
            {
                return false;
            }
            var legalPtr = Marshal.AllocHGlobal(4);
            Marshal.WriteInt32(legalPtr,0);
#pragma warning disable CS0652 // 与整数常量比较无意义；该常量不在类型的范围之内
            var check = Marshal.ReadInt32(legalPtr) == 0xDEADBEEF;
#pragma warning restore CS0652 // 与整数常量比较无意义；该常量不在类型的范围之内
            Marshal.FreeHGlobal(legalPtr);
            return check;
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool CheckRuntimeIntegrity()
        {
            if (Debugger.IsAttached || Environment.GetEnvironmentVariable("COR_ENABLE_PROFILING") == "1")
            {
                return false;
            }

            return !IsDebuggerPresent(); // 进一步检测调试器
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool IsDebuggerPresent();

        public static string GenerateCode(string operation)
        {
            var code = $"{operation}_{Environment.TickCount}";
            var token=Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(code)));
            operations.TryAdd(token, DateTime.UtcNow.AddMinutes(1));
            return token;
        }
        public static bool verifyCode(string token)
        {
            return operations.TryRemove(token,out var expity)&&expity>DateTime.UtcNow;
        }
    }*/
    internal static class LegalGuardian
{
    private static readonly ConcurrentDictionary<string, DateTime> operations = new();
    private static readonly byte[] stackTrace = GetOrgkey();

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static byte[] GetOrgkey()
    {
        return Encoding.UTF8.GetBytes(Guid.NewGuid().ToString() + Environment.MachineName.GetHashCode().ToString("x8"));
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    /*private static bool ValidateTopology()
    {
        var stack = new StackTrace(skipFrames: 2, fNeedFileInfo: false);
        return stack.GetFrames()?.All(f =>
            !f.GetMethod()?.DeclaringType?.Namespace?.Contains("Unsafe") &&
            f.GetMethod()?.MetadataToken < 0x06000000) ?? true;
    }*/
        private static bool ValidateTopology()
        {
            var stack = new StackTrace(skipFrames: 2, fNeedFileInfo: false);
            return stack.GetFrames()?.All(f => !f.GetMethod()?.DeclaringType?.Namespace?.Contains("Unsafe") ?? true) ?? true;
        }

        public static bool ValidateOperation(string operationCode)
    {
        return ValidateTopology() && VerifyOperationSignature(operationCode) && CheckRuntimeIntegrity();
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool VerifyOperationSignature(string code)
    {
        try
        {
            using var hmac = new HMACSHA256(stackTrace);
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(code));
            var timestamp = BitConverter.ToInt64(hash, 0) % 30000;
            return Math.Abs(Environment.TickCount - timestamp) < 5000;
        }
        catch
        {
            return false;
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool CheckRuntimeIntegrity()
    {
        if (Debugger.IsAttached || Environment.GetEnvironmentVariable("COR_ENABLE_PROFILING") == "1")
        {
            return false;
        }

        return !IsDebuggerPresent();
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool IsDebuggerPresent();

    public static string GenerateCode(string operation)
    {
        var code = $"{operation}_{Environment.TickCount}";
        var token = Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(code)));
        operations.TryAdd(token, DateTime.UtcNow.AddMinutes(1));
        return token;
    }

    public static bool VerifyCode(string token)
    {
        return operations.TryRemove(token, out var expiry) && expiry > DateTime.UtcNow;
    }
}
    /// <summary>
    /// 这一个方案是应对无限递归时的操作，这个方案是直接终止应用程序
    /// </summary>
    public static class ApplicationStop 
    {
        /// <summary>
        /// 停止应用程序
        /// </summary>
        public static void Stop()
        {
            //获取当前进程的句柄,直接消除进程内存，高危操作
            IntPtr processHandle = Process.GetCurrentProcess().Handle;
            //终止进程
            Win32Api.terminateProcess(processHandle, 0xDEADBEEF);
            //释放进程句柄，从根基上消除进程，触发进程的崩溃
            Marshal.WriteByte(IntPtr.Zero, 0x00);//对空指针进行写入操作，直接导致进程崩溃
        }
    }
    /// <summary>
    /// windows的原生API,通过这个API可以直接终止进程
    /// </summary>
    internal static class Win32Api
    {
#pragma warning disable CS0626 // 方法、运算符或访问器标记为外部对象并且上面没有任何特性
        /// <summary>
        /// 终止进程，使其直接崩溃
        /// </summary>
        /// <param name="hProcess"></param>
        /// <param name="uExitCode"></param>
        /// <returns></returns>
        public static extern bool terminateProcess(IntPtr hProcess, uint uExitCode);
#pragma warning restore CS0626 // 方法、运算符或访问器标记为外部对象并且上面没有任何特性
    }
}

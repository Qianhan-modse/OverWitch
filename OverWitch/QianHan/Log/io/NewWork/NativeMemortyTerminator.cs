using System;
using System.Runtime.InteropServices;
using OverWitch.QianHan.Entities;
using UnityEngine;

namespace Assets.OverWitch.QianHan.Log.io.NewWork
{
    public static class NativeMemortyTerminator
    {
        private static int GCCount;
        private static int tick;
        private static bool GCCload()
        {
            tick++;
            if(tick>5)
            {
                GCCount++;
                if(GCCount==2)
                {
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    GCCount = 0;
                }
                tick = 0;
            }
            return true;
        }
        private static class NativeMethods
        {
            [DllImport("kernel32.dll", EntryPoint = "RtlzeroMemory")]
            public static extern bool ZeroMemory(IntPtr dest, IntPtr size);
            [DllImport("msvcrt.dll", EntryPoint = "memset")]
            public static extern IntPtr MemSet(IntPtr dest, int c, IntPtr count);
        }
        //这里因为烦人的警告，所以采取大写驼峰命名
        public static void FinallyRemove(this UnityEngine.Object obj)
        {
            if(obj==null)
            {
                return;
            }
            //清除内存
            var ptr = obj.GetInstanceID();//获取语句/句柄-底层
            NativeMethods.ZeroMemory((IntPtr)ptr, (IntPtr)Marshal.SizeOf(obj));
            //标记以弃用，使其可被回收
            NativeMethods.MemSet((IntPtr)ptr,0xDEAD,(IntPtr)Marshal.SizeOf(obj));
            //反射/反解引用的Unity系统
            obj.hideFlags |= HideFlags.DontSaveInEditor | HideFlags.DontSaveInBuild;
            Resources.UnloadAsset(obj);
            GCHandle.Alloc(obj, GCHandleType.WeakTrackResurrection);
            //GC在最后调用，以确保已经没有可释放的无引用对象
            GCCload();
        }
        public static void removeEntityType(this Entity entity)
        {
            foreach(var comp in entity.GetComponents<Component>())
            {
                comp.FinallyRemove();
            }
            var go = entity.gameObject;
            go.FinallyRemove();
            //手动解除引用/托管
            entity.transform.DetachChildren();
            UnityEngine.Object.DestroyImmediate(go);
        }
    }
}

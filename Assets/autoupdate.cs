using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Linq;

public class autoupdate : MonoBehaviour
{
    public clickonoff rib;
    public clickonoff heart;
    public clickonoff eyeball;
    public clickonoff nail;
    public clickonoff ring;
    public clickonoff cross;
    public clickonoff bag;

    public clickonoff wcrystal;
    public clickonoff bcrystal;
    public clickonoff rcrystal;

    public clickonoff dagger;
    public clickonoff sdagger;
    public clickonoff goldknife;
    public clickonoff holywater;
    public clickonoff flame;
    public clickonoff diamond;

    public clickonoff laurels;
    public clickonoff garlic;
    public clickonoff stake;

    public Process ReadProcess = null;
    private IntPtr handle = IntPtr.Zero;
    int lastbyte = 0;



    public void OpenProcess()
    {

        handle = ProcessMemoryReaderApi.OpenProcess((uint)ProcessMemoryReaderApi.ProcessAccessType.PROCESS_VM_READ, 0, (uint)ReadProcess.Id);

    }

    public byte[] ReadMemory (IntPtr memoryaddress, uint bytesToRead, out int bytesRead) {
        byte[] buffer = new byte[bytesToRead];
        IntPtr pBytesRead;
        int returned = ProcessMemoryReaderApi.ReadProcessMemory(handle, memoryaddress, buffer, bytesToRead, out pBytesRead);

        bytesRead = pBytesRead.ToInt32();
        return buffer;

    }
    uint offset = 0;
    IEnumerator getdata()
    {

        UnityEngine.Debug.Log("running");

        if (ReadProcess == null && handle == IntPtr.Zero)
        {


            Process[] processes = Process.GetProcessesByName("fceux");
            if (processes.Length == 0 )
            {
                yield break;
            }

            ReadProcess = processes[0];

            if (ReadProcess == null)
            {
                UnityEngine.Debug.Log("TEST null");

                yield break;
            }

            OpenProcess();


        }

        uint baseaddress = 0x003B1388;

        uint paddress = baseaddress;



        offset = BitConverter.ToUInt32( ReadMemory((IntPtr)(paddress + (uint)ReadProcess.Modules[0].BaseAddress), 4, out int bytesread), 0 );

        byte[] results = ReadMemory((IntPtr)(offset + 0x91), 2, out bytesread);



        if ((results[0] & 32) == 32)
        {
            if ((results[0] & 64) == 64)
            {
                UnityEngine.Debug.Log("Have red crystal");
                rcrystal.toggleon();
            }
            else
            {
                UnityEngine.Debug.Log("Have white crystal");
                wcrystal.toggleon();
            }
        }
        if ((results[0] & 64) == 64)
        {
            UnityEngine.Debug.Log("Have blue crystal");
            bcrystal.toggleon();
            wcrystal.toggleon();
        }

        if ((results[0] & 1) == 1)
        {
            UnityEngine.Debug.Log("Have rib");
            rib.toggleon();
        }
        if ((results[0] & 2) == 2)
        {
            UnityEngine.Debug.Log("Have heart");
            heart.toggleon();
        }
        if ((results[0] & 4) == 4)
        {
            UnityEngine.Debug.Log("Have eye");
            eyeball.toggleon();
        }
        if ((results[0] & 8) == 8)
        {
            UnityEngine.Debug.Log("Have nail");
            nail.toggleon();
        }
        if ((results[0] & 16) == 16)
        {
            UnityEngine.Debug.Log("Have ring");
            ring.toggleon();
        }
        if ((results[1] & 2) == 2)
        {
            UnityEngine.Debug.Log("Have magic cross");
            cross.toggleon();
        }
        if ((results[1] & 1) == 1)
        {
            UnityEngine.Debug.Log("Have bag");
            bag.toggleon();
        }
        results = ReadMemory((IntPtr)(offset + 0x004A), 1, out bytesread);
        for (int i = 0; i < results.Length; i++)
        {
            UnityEngine.Debug.Log(results[i].ToString());
        }
        if ((results[0] & 1) == 1)
        {
            dagger.toggleon();
            UnityEngine.Debug.Log("Have dagger");
        }
        if ((results[0] & 2) == 2)
        {
            sdagger.toggleon();
            UnityEngine.Debug.Log("Have silver dagger");
        }
        if ((results[0] & 4) == 4)
        {
            goldknife.toggleon();
            UnityEngine.Debug.Log("Have Gold Knife");
        }
        if ((results[0] & 8) == 8)
        {
            holywater.toggleon();
            UnityEngine.Debug.Log("Have holy water");
        }
        if ((results[0] & 16) == 16)
        {
            diamond.toggleon();
            UnityEngine.Debug.Log("Have diamond");
        }
        if ((results[0] & 32) == 32)
        {
            flame.toggleon();
            UnityEngine.Debug.Log("Have Sacred Flame");
        }
        if ((results[0] & 64) == 64)
        {
            stake.toggleon();
            UnityEngine.Debug.Log("Have Oak Stake");
        }else
        {
            stake.toggleoff();
        }


        results = ReadMemory((IntPtr)(offset + 0x004D), 1, out bytesread);
        UnityEngine.Debug.Log("Garlic: "+results[0].ToString());
        if (results[0]> 0)
        {
            garlic.toggleon();
        }else
        {
            garlic.toggleoff();
        }
        results = ReadMemory((IntPtr)(offset + 0x004C), 1, out bytesread);
        UnityEngine.Debug.Log("Laurels: " + results[0].ToString());
        if (results[0] > 0)
        {
            laurels.toggleon();
        }else
        {
            laurels.toggleoff();
        }







    }
    void test()
    {

        StartCoroutine(getdata());
    }

    // Start is called before the first frame update
    void Start()
    {


        InvokeRepeating("test", 2.0f, 1.0f);

    }


    // Update is called once per frame
    void Update()
    {


    }

}



/// <summary>
/// ProcessMemoryReader is a class that enables direct reading a process memory
/// </summary>
class ProcessMemoryReaderApi
{
    // constants information can be found in <winnt.h>
    [Flags]
    public enum ProcessAccessType
    {
        PROCESS_TERMINATE = (0x0001),
        PROCESS_CREATE_THREAD = (0x0002),
        PROCESS_SET_SESSIONID = (0x0004),
        PROCESS_VM_OPERATION = (0x0008),
        PROCESS_VM_READ = (0x0010),
        PROCESS_VM_WRITE = (0x0020),
        PROCESS_DUP_HANDLE = (0x0040),
        PROCESS_CREATE_PROCESS = (0x0080),
        PROCESS_SET_QUOTA = (0x0100),
        PROCESS_SET_INFORMATION = (0x0200),
        PROCESS_QUERY_INFORMATION = (0x0400),
        PROCESS_QUERY_LIMITED_INFORMATION = (0x1000)
    }

    // function declarations are found in the MSDN and in <winbase.h>

    //		HANDLE OpenProcess(
    //			DWORD dwDesiredAccess,  // access flag
    //			BOOL bInheritHandle,    // handle inheritance option
    //			DWORD dwProcessId       // process identifier
    //			);
    [DllImport("kernel32.dll")]
    public static extern IntPtr OpenProcess(UInt32 dwDesiredAccess, Int32 bInheritHandle, UInt32 dwProcessId);

    //		BOOL CloseHandle(
    //			HANDLE hObject   // handle to object
    //			);
    [DllImport("kernel32.dll")]
    public static extern Int32 CloseHandle(IntPtr hObject);

    //		BOOL ReadProcessMemory(
    //			HANDLE hProcess,              // handle to the process
    //			LPCVOID lpBaseAddress,        // base of memory area
    //			LPVOID lpBuffer,              // data buffer
    //			SIZE_T nSize,                 // number of bytes to read
    //			SIZE_T * lpNumberOfBytesRead  // number of bytes read
    //			);
    [DllImport("kernel32.dll")]
    public static extern Int32 ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesRead);

    //		BOOL WriteProcessMemory(
    //			HANDLE hProcess,                // handle to process
    //			LPVOID lpBaseAddress,           // base of memory area
    //			LPCVOID lpBuffer,               // data buffer
    //			SIZE_T nSize,                   // count of bytes to write
    //			SIZE_T * lpNumberOfBytesWritten // count of bytes written
    //			);
    [DllImport("kernel32.dll")]
    public static extern Int32 WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesWritten);
}

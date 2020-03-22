using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Linq;



public class autoupdate : MonoBehaviour
{
   

    public Process ReadProcess = null;
    private IntPtr handle = IntPtr.Zero;
    

    public controller controller;


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

        //UnityEngine.Debug.Log("running");

        if (ReadProcess != null)
        {
            Process check = Process.GetProcessById(ReadProcess.Id);
            if (check.Id != ReadProcess.Id)
            {
                UnityEngine.Debug.Log("lost process? "+check.Id+" "+ReadProcess.Id);
                ReadProcess = null;
                handle = IntPtr.Zero;

            }
        }


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
            UnityEngine.Debug.Log("found process " + ReadProcess.Id.ToString());
            

            OpenProcess();
            UnityEngine.Debug.Log("opened process " + ReadProcess.Id.ToString());
            

            

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
                controller.curitems["rcrystal"] = true;
            }
            else
            {
                UnityEngine.Debug.Log("Have white crystal");
                
                controller.curitems["wcrystal"] = true;
            }
        }
        if ((results[0] & 64) == 64)
        {
            UnityEngine.Debug.Log("Have blue crystal");
            controller.curitems["bcrystal"] = true;
        }

        if ((results[0] & 1) == 1)
        {
            UnityEngine.Debug.Log("Have rib");
            controller.curitems["rib"] = true;
        }
        if ((results[0] & 2) == 2)
        {
            UnityEngine.Debug.Log("Have heart");
            controller.curitems["heart"] = true;
        }
        if ((results[0] & 4) == 4)
        {
            UnityEngine.Debug.Log("Have eye");
            controller.curitems["eyeball"] = true;
        }
        if ((results[0] & 8) == 8)
        {
            UnityEngine.Debug.Log("Have nail");
            controller.curitems["nail"] = true;
        }
        if ((results[0] & 16) == 16)
        {
            UnityEngine.Debug.Log("Have ring");
            controller.curitems["ring"] = true;
        }
        if ((results[1] & 2) == 2)
        {
            UnityEngine.Debug.Log("Have magic cross");
            controller.curitems["cross"] = true;
        }
        if ((results[1] & 1) == 1)
        {
            UnityEngine.Debug.Log("Have bag");
            controller.curitems["bag"] = true;
        }
        results = ReadMemory((IntPtr)(offset + 0x004A), 1, out bytesread);
        
        if ((results[0] & 1) == 1)
        {
            controller.curitems["dagger"] = true;
            UnityEngine.Debug.Log("Have dagger");
        }
        if ((results[0] & 2) == 2)
        {
            controller.curitems["silver knife"] = true;
            UnityEngine.Debug.Log("Have silver dagger");
        }
        if ((results[0] & 4) == 4)
        {
            controller.curitems["gold knife"] = true;
            UnityEngine.Debug.Log("Have Gold Knife");
        }
        if ((results[0] & 8) == 8)
        {
            controller.curitems["holy water"] = true;
            UnityEngine.Debug.Log("Have holy water");
        }
        if ((results[0] & 16) == 16)
        {
            controller.curitems["diamond"] = true;
            UnityEngine.Debug.Log("Have diamond");
        }
        if ((results[0] & 32) == 32)
        {
            controller.curitems["flame"] = true;
            UnityEngine.Debug.Log("Have Sacred Flame");
        }
        if ((results[0] & 64) == 64)
        {
            controller.curitems["stake"] = true;
            UnityEngine.Debug.Log("Have Oak Stake");
        }else
        {
            controller.olditems["stake"] = false;
        }


        results = ReadMemory((IntPtr)(offset + 0x004D), 1, out bytesread);
        //UnityEngine.Debug.Log("Garlic: "+results[0].ToString());
        if (results[0]> 0)
        {
            controller.curitems["garlic"] = true;
        }
        else
        {
            controller.olditems["garlic"] = false;
        }
        results = ReadMemory((IntPtr)(offset + 0x004C), 1, out bytesread);
        //UnityEngine.Debug.Log("Laurels: " + results[0].ToString());
        if (results[0] > 0)
        {
            controller.curitems["laurels"] = true;
        }
        else
        {
            controller.olditems["laurels"] = false;
        }







    }
    void test()
    {

        StartCoroutine(getdata());
    }

    // Start is called before the first frame update
    void Start()
    {

        controller = FindObjectOfType<controller>();

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

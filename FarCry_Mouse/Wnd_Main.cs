using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;

namespace FarCry_Mouse
{
    public partial class Wnd_Main : Form
    {
        #region Variables declararion

        private static Wnd_Main _This = null;

        private static int _ScreenWidth = 0;
        private static int _ScreenHeight = 0;
        private static int _ClientWidth = 0;
        private static int _ClientHeight = 0;

        private static Point _MouseScreenPosition;
        private static Point _MouseClientPosition;
        private static Point _MouseJoystickPosition;
        private static IntPtr _MouseHookID = IntPtr.Zero;

        private static string Debug_MouseScreen;
        private static string Debug_MouseClient;
        private static string Debug_MouseJoystick;
        private static string Debug_ClientSize;

        private static string _BgwErrorMsg = string.Empty;
        private static string _BgwErrorSrc = string.Empty;
        private static string _BgwErrorStk = string.Empty;

        private static Process _TargetProcess = null;
        private const string _TargetProcess_Name = "FarCry_r";
        private static bool _TargetProcess_Hooked = false;
        private static Timer _tProcess;

        private static XOutput _XOutputManager;
        private static int _Player1_X360_Gamepad_Port = 0;

        #endregion
        
        #region WIN32

        /// <summary>
        /// Struct representing a point.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator Point(POINT point)
            {
                return new Point(point.X, point.Y);
            }
        }

        public struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public int MouseData;
            public int flags;
            public int time;
            public UIntPtr dwExtraInfo;
        }

        /// <summary>
        /// Retrieves the cursor's position, in screen coordinates.
        /// </summary>
        /// <see>See MSDN documentation for further information.</see>
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool ScreenToClient(IntPtr hWnd, ref Point lpPoint);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetClientRect(IntPtr hWnd, ref Rect rectangle);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);
        
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);
        
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
        
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
        
        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out IntPtr ProcessId);
        private const int WH_MOUSE_LL = 14;
        private const int WM_MOUSEMOVE = 0x0200;
        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_LBUTTONUP = 0x0202;
        private const int WM_MBUTTONDOWN = 0x0207;
        private const int WM_MBUTTONUP = 0x0208;
        private const int WM_RBUTTONDOWN = 0x0204;
        private const int WM_RBUTTONUP = 0x0205;
        
        #endregion
        
        public Wnd_Main()
        {
            InitializeComponent();

            _This = this;

            /*** getting optionnal args ***/
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                for (int i = 1; i < args.Length; i++)
                {
                    if (args[i].ToLower().Equals("-invertx"))
                    {
                        Chk_x.Checked = true;
                    }
                    if (args[i].ToLower().Equals("-inverty"))
                    {
                        Chk_y.Checked = true;
                    }
                }
            }

            // Windows can handle up to 4 Xinput devices
            // So before plugging avirtual Gamepad, we force unplug all devices to be sure we have room for ours
            _XOutputManager = new XOutput();
            UninstallAllX360Gamepad();
            System.Threading.Thread.Sleep(500);
            InstallX360Gamepad();

            //Install system-wide mouse hook to intercep movements and buttons
            ApplyMouseHook();

            //Starting ProcessHooking Timer
            _tProcess = new Timer();
            _tProcess.Interval = 500;
            _tProcess.Tick += new EventHandler(tProcess_Tick);
            _tProcess.Enabled = true;
            _tProcess.Start();
        }

        private void WndMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            UninstallAllX360Gamepad();
        }

        #region TimerProcessHook

        /// <summary>
        /// Timer event when looking for Game's Process (auto-Hook)
        /// As opposed to Aliens: Extermination program, it will not close itself when the game's process has finished as
        /// the "patch" to bypass dongle request makes the game to quit and reload itself.....
        /// </summary>        
        private void tProcess_Tick(Object Sender, EventArgs e)
        {
            try
            {
                Process[] processes = Process.GetProcessesByName(_TargetProcess_Name);
                if (processes.Length > 0)
                {
                    _TargetProcess = processes[0];
                    WriteLog("Attached to Process " + _TargetProcess_Name + ".exe");
                    Bgw_Mouse.RunWorkerAsync();
                }
            }
            catch
            { }
        }

        #endregion

        #region BackGroundWorker

        private void BgwMouse_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Debug_MouseScreen = string.Empty;
                Debug_ClientSize = string.Empty;
                Debug_MouseClient = string.Empty;
                Debug_MouseJoystick = string.Empty;

                //Getting On-screen Position
                Debug_MouseScreen = "X=" + _MouseScreenPosition.X.ToString() + ", Y=" + _MouseScreenPosition.Y.ToString();
                Bgw_Mouse.ReportProgress(0);

                
                if (_TargetProcess_Hooked)
                {
                    //Getting game's client size 
                    try
                    {
                        Rect TotalRes = new Rect();
                        GetClientRect(_TargetProcess.MainWindowHandle, ref TotalRes);
                        _ClientWidth = TotalRes.Right - TotalRes.Left;
                        _ClientHeight = TotalRes.Bottom - TotalRes.Top;
                    }
                    catch (Exception Ex)
                    {
                        _BgwErrorMsg = Ex.Message.ToString();
                        _BgwErrorSrc = Ex.Source.ToString();
                        _BgwErrorStk = Ex.StackTrace.ToString();
                    }
                    Debug_ClientSize = _ClientWidth + "x" + _ClientHeight;
                    Bgw_Mouse.ReportProgress(1);                 
                    
                    
                    //Getting On-client cursor position
                    _BgwErrorMsg = string.Empty;
                    try
                    {
                        _MouseClientPosition.X = _MouseScreenPosition.X;
                        _MouseClientPosition.Y = _MouseScreenPosition.Y;
                        ScreenToClient(_TargetProcess.MainWindowHandle, ref _MouseClientPosition);
                        Debug_MouseClient = "X=" + _MouseClientPosition.X.ToString() + ", Y=" + _MouseClientPosition.Y.ToString();
                    }
                    catch (Exception Ex)
                    {
                        _BgwErrorMsg = Ex.Message.ToString();
                        _BgwErrorSrc = Ex.Source.ToString();
                        _BgwErrorStk = Ex.StackTrace.ToString();
                    }
                    Bgw_Mouse.ReportProgress(2); 


                    //Convert to game compatible Axis value
                    _BgwErrorMsg = string.Empty;
                    try
                    {
                        //X,Y => [-32768 ; 32767] => 0xFFFF range
                        double dMaxX = 65535.0;
                        double dMaxY = 65535.0;

                        _MouseJoystickPosition.X = Convert.ToInt32(Math.Round(dMaxX * _MouseClientPosition.X / _ClientWidth) - 32768);
                        _MouseJoystickPosition.Y = Convert.ToInt32(Math.Round(dMaxY * _MouseClientPosition.Y / _ClientHeight) - 32768) * -1;
                        if (_MouseJoystickPosition.X < -32768)
                            _MouseJoystickPosition.X = -32768;
                        if (_MouseJoystickPosition.Y < -32768)
                            _MouseJoystickPosition.Y = -32768;
                        if (_MouseJoystickPosition.X > 32767)
                            _MouseJoystickPosition.X = 32767;
                        if (_MouseJoystickPosition.Y > 32767)
                            _MouseJoystickPosition.Y = 32767;

                        if (Chk_x.Checked)
                            _MouseJoystickPosition.X = -_MouseJoystickPosition.X - 1;
                        if (Chk_y.Checked)
                            _MouseJoystickPosition.Y = -_MouseJoystickPosition.Y - 1;

                        Debug_MouseJoystick = "X=" + _MouseJoystickPosition.X.ToString() + ", Y=" + _MouseJoystickPosition.X.ToString();

                        _XOutputManager.SetLAxis_X(1, (short)_MouseJoystickPosition.X);
                        _XOutputManager.SetLAxis_Y(1, (short)_MouseJoystickPosition.Y);                       
                    }
                    catch (Exception Ex)
                    {
                        _BgwErrorMsg = Ex.Message.ToString();
                        _BgwErrorSrc = Ex.Source.ToString();
                        _BgwErrorStk = Ex.StackTrace.ToString();
                    }
                    Bgw_Mouse.ReportProgress(3);                    
                }

                System.Threading.Thread.Sleep(10);
           }
        }

        private void BgwMouse_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
            {
                Lbl_Screen.Text = Debug_MouseScreen;
            }
            else if (e.ProgressPercentage == 1)
            {
                Lbl_ClientSize.Text = Debug_ClientSize;
                /*if (_BgwErrorMsg != string.Empty)
                {
                    WriteLog("-----GetClientRect API error--------");
                    WriteLog(_BgwErrorMsg);
                    WriteLog(_BgwErrorStk);
                    WriteLog("");
                }*/
            }

            else if (e.ProgressPercentage == 2)
            {
                Lbl_Client.Text = Debug_MouseClient;
                /*if (_BgwErrorMsg != string.Empty)
                {
                    WriteLog("-----ScreenToClient API error-----");
                    WriteLog(_BgwErrorMsg);
                    WriteLog(_BgwErrorStk);
                    WriteLog("");
                }*/
            }
            else if (e.ProgressPercentage == 3)
            {

                Lbl_Joystick.Text = Debug_MouseJoystick;
                /*if (_BgwErrorMsg != string.Empty)
                {
                    WriteLog("-----Game coordinate calculation error--------");
                    WriteLog(_BgwErrorMsg);
                    WriteLog(_BgwErrorStk);
                    WriteLog("");
                }*/
            }   
        }

        private void BgwMouse_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Never happening
        }

        #endregion

        #region GamePad

        private void InstallX360Gamepad()
        {
            if (_XOutputManager != null)
            {
                if (_XOutputManager.isVBusExists())
                {
                    for (int i = 1; i < 5; i++)
                    {
                        if (_XOutputManager.PlugIn(i))
                        {
                            WriteLog("Plugged P1 virtual Gamepad to port " + i.ToString());
                            _Player1_X360_Gamepad_Port = i;
                            break;
                        }
                        else
                            WriteLog("Failed to plug virtual GamePad to port " + i.ToString() + ". (Port already used ?)");
                    }
                }
                else
                {
                    WriteLog("ScpBus driver not found or not installed");
                }
            }
            else
            {
                WriteLog("XOutputManager Creation Failed !");
            }
        }

        private bool UninstallX360Gamepad()
        {
            if (_XOutputManager != null)
            {
                if (_Player1_X360_Gamepad_Port != 0)
                {
                    if (_XOutputManager.Unplug(_Player1_X360_Gamepad_Port, true))
                    {
                        WriteLog("Succesfully unplug P1 virtual Gamepad on port " + _Player1_X360_Gamepad_Port.ToString());
                        return true;
                    }
                    else
                    {
                        WriteLog("Failed to unplug P1 virtual Gamepad on port " + _Player1_X360_Gamepad_Port.ToString());
                        return false;
                    }
                }
            }
            return true;
        }

        private bool UninstallAllX360Gamepad()
        {
            if (_XOutputManager != null)
            {
                for (int i = 1; i < 5; i++)
                {
                    if (_XOutputManager.Unplug(i, true))
                    {
                        WriteLog("Succesfully unplug virtual Gamepad on port " + i.ToString());
                    }
                    else
                    {
                        WriteLog("Failed to unplug virtual Gamepad on port " + i.ToString());
                    }
                }
            }
            return true;
        }

        #endregion

        #region Screen

        public void GetScreenResolution()
        {
            _ScreenWidth = Screen.PrimaryScreen.Bounds.Width;
            _ScreenHeight = Screen.PrimaryScreen.Bounds.Height;
        }

        #endregion

        #region MouseHook

        private static void ApplyMouseHook()
        {
            _MouseHookID = SetHook(ll_mouse_proc, _TargetProcess);
        }
        private static IntPtr SetHook(LowLevelMouseProc proc, Process TargetProcess)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                //System Wide Hook
                return SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);
        private static LowLevelMouseProc ll_mouse_proc = new LowLevelMouseProc(MouseHook_HookCallback);
        private static IntPtr MouseHook_HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0) 
            {
                if ((UInt32)wParam == WM_MOUSEMOVE)
                {
                    MSLLHOOKSTRUCT s = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                    _MouseScreenPosition.X = s.pt.X;
                    _MouseScreenPosition.Y = s.pt.Y;
                }
                else if ((UInt32)wParam == WM_LBUTTONDOWN)
                {
                    _XOutputManager.SetButton_A(1, true);
                }
                else if ((UInt32)wParam == WM_LBUTTONUP)
                {
                    _XOutputManager.SetButton_A(1, false);
                }
                else if ((UInt32)wParam == WM_RBUTTONDOWN || (UInt32)wParam == WM_MBUTTONDOWN)
                {
                    _XOutputManager.SetButton_B(1, true);
                }
                else if ((UInt32)wParam == WM_RBUTTONUP || (UInt32)wParam == WM_MBUTTONUP)
                {
                    _XOutputManager.SetButton_B(1, false);
                }
            }
            return CallNextHookEx(_MouseHookID, nCode, wParam, lParam);
        }

        #endregion

        #region Logger

        private static void WriteLog(String Data)
        {
            _This.Txt_Log.Text += DateTime.Now.ToString("[HH:mm:ss] ") + Data + "\n";
        }

        #endregion        

    }

}

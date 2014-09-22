using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using CodeProject.Win32;
using System.Collections.Generic;


namespace MySystemHotkey
{
	/// <summary>
	/// Handles a System Hotkey
	/// </summary>
    /// 
    public enum RegisterHotKeyModifiers
    {
        MOD_NONE=0,
        MOD_ALT = 1,
        MOD_CONTROL = 2,
        MOD_CTRL_ALT=3,
        MOD_SHIFT = 4,
        MOD_SHIFT_ALT=5,
        MOD_SHIFT_CTRL=6,
        MOD_SHIFT_ALT_CTRL=7
    }
    public enum VK
    {
        VK_LBUTTON =0x1,//Left mouse button
        VK_RBUTTON =0x2,//Right mouse button
        VK_CANCEL =0x3,//Control-break processing
        VK_MBUTTON =0x4,//Middle mouse button (three-button mouse)
        VK_BACK =0x8,//BACKSPACE key
        VK_TAB =0x9,//TAB key
        VK_CLEAR =0x0C ,//CLEAR key
        VK_RETURN =0x0D ,//ENTER key
        VK_SHIFT =0x10,//SHIFT key
        VK_CONTROL =0x11,//CTRL key
        VK_MENU =0x12,//ALT key
        VK_PAUSE =0x13,//PAUSE key
        VK_CAPITAL =0x14,//CAPS LOCK key
        VK_ESCAPE =0x1B ,//ESC key
        VK_SPACE =0x20,//SPACEBAR
        VK_PRIOR =0x21,//PAGE UP key
        VK_NEXT =0x22,//PAGE DOWN key
        VK_END =0x23,//END key
        VK_HOME =0x24,//HOME key
        VK_LEFT =0x25,//LEFT ARROW key
        VK_UP =0x26,//UP ARROW key
        VK_RIGHT =0x27,//RIGHT ARROW key
        VK_DOWN =0x28,//DOWN ARROW key
        VK_SELECT =0x29,//SELECT key
        VK_PRINT =0x2A ,//PRINT key
        VK_EXECUTE =0x2B ,//EXECUTE key
        VK_SNAPSHOT =0x2C ,//PRINT SCREEN key
        VK_INSERT =0x2D ,//INS key
        VK_DELETE =0x2E ,//DEL key
        VK_HELP =0x2F ,//HELP key
        VK_0=0x30,//0 key
        VK_1=0x31,//1 key
        VK_2=0x32,//2 key
        VK_3=0x33,//3 key
        VK_4=0x34,//4 key
        VK_5=0x35,//5 key
        VK_6=0x36,//6 key
        VK_7=0x37,//7 key
        VK_8=0x38,//8 key
        VK_9=0x39,//9 key
        VK_A=0x41,//A
        VK_B=0x42,//B
        VK_C=0x43,//C
        VK_D=0x44,//D
        VK_E=0x45,//E
        VK_F=0x46,//F
        VK_G=0x47,//G
        VK_H=0x48,//H
        VK_I=0x49,//I
        VK_J=0x4A ,//J
        VK_K=0x4B ,//K
        VK_L=0x4C ,//L
        VK_M=0x4D ,//M
        VK_N=0x4E ,//N
        VK_O=0x4F ,//O
        VK_P=0x50,//P
        VK_Q=0x51,//Q
        VK_R=0x52,//R
        VK_S=0x53,//S
        VK_T=0x54,//T
        VK_U=0x55,//U
        VK_V=0x56,//V
        VK_W=0x57,//W
        VK_X=0x58,//X
        VK_Y=0x59,//Y
        VK_Z=0x5A ,//Z
        VK_NUMPAD0 =0x60,//Numeric keypad 0 key
        VK_NUMPAD1 =0x61,//Numeric keypad 1 key
        VK_NUMPAD2 =0x62,//Numeric keypad 2 key
        VK_NUMPAD3 =0x63,//Numeric keypad 3 key
        VK_NUMPAD4 =0x64,//Numeric keypad 4 key
        VK_NUMPAD5 =0x65,//Numeric keypad 5 key
        VK_NUMPAD6 =0x66,//Numeric keypad 6 key
        VK_NUMPAD7 =0x67,//Numeric keypad 7 key
        VK_NUMPAD8 =0x68,//Numeric keypad 8 key
        VK_NUMPAD9 =0x69,//Numeric keypad 9 key
        VK_SEPARATOR =0x6C ,//Separator key
        VK_SUBTRACT =0x6D ,//Subtract key
        VK_DECIMAL =0x6E ,//Decimal key
        VK_DIVIDE =0x6F ,//Divide key
        VK_F1 =0x70,//F1 key
        VK_F2 =0x71,//F2 key
        VK_F3 =0x72,//F3 key
        VK_F4 =0x73,//F4 key
        VK_F5 =0x74,//F5 key
        VK_F6 =0x75,//F6 key
        VK_F7 =0x76,//F7 key
        VK_F8 =0x77,//F8 key
        VK_F9 =0x78,//F9 key
        VK_F10 =0x79,//F10 key
        VK_F11 =0x7A ,//F11 key
        VK_F12 =0x7B ,//F12 key
        VK_F13 =0x7C ,//F13 key
        VK_F14 =0x7D ,//F14 key
        VK_F15 =0x7E ,//F15 key
        VK_F16 =0x7F ,//F16 key
        /*
        VK_F17 =0x80H ,//F17 key
        VK_F18 =0x81H ,//F18 key
        VK_F19 =0x82H ,//F19 key
        VK_F20 =0x83H ,//F20 key
        VK_F21 =0x84H ,//F21 key
        VK_F22 =0x85H ,//F22 key
        VK_F23 =0x86H ,//F23 key
        VK_F24 =0x87H ,//F24 key
        */
        VK_NUMLOCK  =0x90,//NUM LOCK key
        VK_SCROLL   =0x91,//SCROLL LOCK key
        VK_LSHIFT   =0xA0 ,//Left SHIFT key
        VK_RSHIFT   =0xA1 ,//Right SHIFT key
        VK_LCONTROL =0xA2 ,//Left CONTROL key
        VK_RCONTROL =0xA3 ,//Right CONTROL key
        VK_LMENU    =0xA4 ,//Left MENU key
        VK_RMENU    =0xA5 ,//Right MENU key
        VK_SEMICOLON=0xBA,//;
        VK_EQUALS   =0xBB,//187/*=*/, 
        VK_COMMA    =0xBC,//188/*,*/, 
        VK_HYPHEN   =0xBD,//189/*-*/, 
        VK_PERIOD   =0xBE,//190/*.*/, 
        VK_SLASH    =0xBF,//191 /*/*/,
        VK_BACK_QUOTE=0xC0,//192/*`*/ ,
        VK_OPEN_BRACKET=0xDB,// 219/*[*/,
        VK_BACK_SLASH=0xDC,//220/*\*/,
        VK_CLOSE_BRACKET=0xDD,//221/*]*/,
        VK_QUOTE    =0xDE,//222/*'*/,   
        VK_PLAY =0xFA ,//Play key
        VK_ZOOM =0xFB ,//Zoom key
    }


    public enum t_key_type
    {
        ALPHA=1,
        NONALPHA=2,
        CTRL=4
    }

    public delegate void RegisterHotKeyDelegate(bool register,t_key_type t);

    public delegate void PressedDelegate(SystemHotkey p);
    #region old_class
    /*
    public class SystemHotkey : System.ComponentModel.Component, IDisposable
    {
        //public delegate void test();


        private System.ComponentModel.Container components = null;
        protected DummyWindowWithEvent m_Window = new DummyWindowWithEvent();	//window for WM_Hotkey Messages
        protected int Virtual_key;
        protected bool isRegistered = false;

        static protected t_key_type RegisteredTypes = 0;
        public static t_key_type _RegisteredTypes
        {
            get { return RegisteredTypes; }
        }

        private event System.EventHandler Pressed;
        t_key_type key_type;
        //t_MyDele MyDele;

        public static void dummy(bool register, t_key_type t)
        {
            RegisteredTypes = register ? RegisteredTypes | t : RegisteredTypes & ~t;
        }

        public SystemHotkey(System.ComponentModel.IContainer container, int vk, t_key_type t, System.EventHandler p, ref t_MyDele dele)
        {
            container.Add(this);
            InitializeComponent();
            m_Window.ProcessMessage += new MessageEventHandler(MessageEvent);
            Virtual_key = vk;
            key_type = t;
            Pressed += p;
            dele += new t_MyDele(this.RegiterType);
        }

        public new void Dispose()
        {
            if (isRegistered)
                UnregisterHotkey();
        }
        #region Component Designer generated code
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion

        protected void MessageEvent(object sender, ref Message m, ref bool Handled)
        {	//Handle WM_Hotkey event
            if ((m.Msg == (int)Msgs.WM_HOTKEY) && (m.WParam == (IntPtr)this.GetType().GetHashCode()))
            {
                Handled = true;
                if (Pressed != null) Pressed(this, EventArgs.Empty);
            }
        }

        private bool UnregisterHotkey()
        {
            isRegistered = false;
            return User32.UnregisterHotKey(m_Window.Handle, this.GetType().GetHashCode());
        }

        private bool RegisterHotkey()
        {
            if (User32.RegisterHotKey(m_Window.Handle, this.GetType().GetHashCode(), 0, Virtual_key))
            {
                isRegistered = true;
                return true;
            }
            return false;
        }

        private void RegiterType(bool register, t_key_type t)
        {
            if ((key_type & t) != 0)
                if (register)
                    RegisterHotkey();
                else
                    UnregisterHotkey();
        }

        public int VirtualKey
        {
            get { return Virtual_key; }
        }
    }
    */
    #endregion

    public class SystemHotkey //System.ComponentModel.Component, IDisposable
    {
        protected DummyWindowWithEvent m_Window = new DummyWindowWithEvent();	//window for WM_Hotkey Messages
        protected int Virtual_key;
        protected int vk_mod;
        protected bool isRegistered = false;
        public static Dictionary<t_key_type,PressedDelegate> Pressed=new Dictionary<t_key_type,PressedDelegate>();
        public static RegisterHotKeyDelegate RegisterHotKey = new RegisterHotKeyDelegate(SystemHotkey.dummy);

        static protected t_key_type RegisteredTypes = 0;
        public static t_key_type _RegisteredTypes
        {
            get { return RegisteredTypes; }
        }

        t_key_type key_type;
        public t_key_type Key_Type
        {
            set
            {
                UnregisterHotkey();
                key_type = value;
            }
            get
            {
                return key_type;
            }
        }

        public static void dummy(bool register, t_key_type t)
        {
            RegisteredTypes = register ? RegisteredTypes | t : RegisteredTypes & ~t;
        }

        public SystemHotkey(VK vk,RegisterHotKeyModifiers vk_md)
        {
            m_Window.ProcessMessage += new MessageEventHandler(MessageEvent);
            Virtual_key = (int)vk;
            vk_mod = (int)vk_md;
            RegisterHotKey += new RegisterHotKeyDelegate(this.RegiterType);
        }

        protected void MessageEvent(object sender, ref Message m, ref bool Handled)
        {	//Handle WM_Hotkey event
            if ((m.Msg == (int)Msgs.WM_HOTKEY) && (m.WParam == (IntPtr)this.GetType().GetHashCode()))
            {
                Handled = true;
                Pressed[key_type](this);
            }
        }

        private bool UnregisterHotkey()
        {
            isRegistered = false;
            return User32.UnregisterHotKey(m_Window.Handle, this.GetType().GetHashCode());
        }

        private bool RegisterHotkey()
        {
            if (User32.RegisterHotKey(m_Window.Handle, this.GetType().GetHashCode(), vk_mod, Virtual_key))
            {
                isRegistered = true;
                return true;
            }
            return false;
        }

        private void RegiterType(bool register, t_key_type t)
        {
            if ((key_type & t) != 0)
                if (register)
                    RegisterHotkey();
                else
                    UnregisterHotkey();
        }

        public int VirtualKey
        {
            get { return Virtual_key; }
        }
        public char Char;
        public int VK_MOD
        {
            get { return vk_mod; }
        }
    }
}

/*
function GetCharFromVirtualKey(Key: Word): string;
var
   keyboardState: TKeyboardState;
   asciiResult: Integer;
begin
   GetKeyboardState(keyboardState) ;

   SetLength(Result, 2) ;
   asciiResult := ToAscii(key, MapVirtualKey(key, 0), keyboardState, @Result[1], 0) ;
   case asciiResult of
     0: Result := '';
     1: SetLength(Result, 1) ;
     2:;
     else
       Result := '';
   end;
end;
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using MySystemHotkey;
using System.IO;
using sritext;
using System.Text.RegularExpressions;
using CodeProject.Win32;
using System.Drawing;

namespace SriShell_Guess
{
    public class Engine:Form
    {
        //string m_SelectForm.Input_TextBox.Text;
        public SettingsForm m_SettingsForm;
        SelectForm m_SelectForm;
        public List<string> selections_strings = new List<string>();
        List<word> selections_words = new List<word>();
        Dictionary<int, SystemHotkey> HotKeys = new Dictionary<int, SystemHotkey>();

        VK[] nonalphas = new VK[]          
            {
                VK.VK_SEMICOLON,//186/*;*/
                VK.VK_EQUALS,//187/*=*/, 
                VK.VK_COMMA,//188/*,*/, 
                VK.VK_HYPHEN,//189/*-*/, 
                VK.VK_PERIOD,// 190/*.*/, 
                VK.VK_BACK_QUOTE,////192/*`*/ ,
                VK.VK_OPEN_BRACKET,//219/*[*/,
                VK.VK_BACK_SLASH,//220/*\*/
                VK.VK_CLOSE_BRACKET,//221/*]*/
                VK.VK_QUOTE,//222/*'*/,                         
                VK.VK_DOWN,
                VK.VK_UP,
                VK.VK_BACK,
                VK.VK_SPACE,
                VK.VK_RETURN,
                //VK.VK_SHIFT, 
                VK.VK_CONTROL,
                VK.VK_MENU,
                VK.VK_TAB,

                VK.VK_SLASH,
                VK.VK_BACK_QUOTE,VK.VK_QUOTE,
                VK.VK_0,VK.VK_1,VK.VK_2,VK.VK_3,VK.VK_4,VK.VK_5,VK.VK_6,VK.VK_7,VK.VK_8,VK.VK_9,
                VK.VK_OPEN_BRACKET,VK.VK_CLOSE_BRACKET,VK.VK_SEMICOLON
            };

        public dele_converter reader;
        public dele_converter getter;
        public Method input_method
        {
            set
            {
                foreach (KeyValuePair<int, SystemHotkey> pair in HotKeys)
                {
                    pair.Value.Key_Type = t_key_type.NONALPHA;
                    //pair.Value.Char = "";
                }
                reader = Converter.readers[value];
                VK vk=VK.VK_0;
                RegisterHotKeyModifiers md=RegisterHotKeyModifiers.MOD_NONE;
                
                foreach (char c in Converter.alpha[value])
                {
                    GetVkMod(c, ref vk, ref md);
                    HotKeys[(int)md * 0x100 + (int)vk].Key_Type = t_key_type.ALPHA;
                    HotKeys[(int)md * 0x100 + (int)vk].Char = c;
                    HotKeys[(int)md * 0x100 + (int)vk].Char = c;
                }
            }
        }
        public Method output_method
        {
            set
            {
                getter = Converter.getters[value];
            }
        }

#if DEBUG
        public string errors="";

        ~Engine()
        {
            TextWriter tw = new StreamWriter("errors.txt");
            tw.WriteLine(errors);
            tw.Close();
        }
#endif

        private void ALPHA_Pressed(SystemHotkey p)
        {
            //Show();
            
            if(p.Char!=0)m_SelectForm.Input_TextBox.Text += p.Char;
            if (Regex.IsMatch(m_SelectForm.Input_TextBox.Text, @"(^|[^/])[+\-]$"))
            {
                NONALPHA_Pressed(p);
                return;
            }
            selections_strings.Clear();
            selections_words=word.Selections(m_SelectForm.Input_TextBox.Text);
            foreach (word w in selections_words)
                selections_strings.Add(Converter.getKaputa(w.ToString().Replace("|", "")));
            //if (selections_words.Count == 0) 
            //selections_strings.Add(Converter.getKaputa(m_SelectForm.Input_TextBox.Text.ToString().Replace("|", "")));
            Reread();
            m_SelectForm.SelectListBox.SelectedIndex = 0;
            SystemHotkey.RegisterHotKey(true, t_key_type.ALPHA | t_key_type.NONALPHA);
            m_SelectForm.Show();
            User32.SetForegroundWindow(LastActiveWindow);
        }

        private void NONALPHA_Pressed(SystemHotkey p)
        {
            switch ((VK)(p.VirtualKey))
            {
                case VK.VK_DOWN:
                    m_SelectForm.SelectListBox.SelectedIndex++;
                    m_SelectForm.Input_TextBox.Text = selections_words[m_SelectForm.SelectListBox.SelectedIndex].ToString().Replace("|", "");
                    Reread();
                    return;
                case VK.VK_UP:
                    m_SelectForm.SelectListBox.SelectedIndex--;
                    m_SelectForm.Input_TextBox.Text = selections_words[m_SelectForm.SelectListBox.SelectedIndex].ToString().Replace("|", "");
                    Reread();
                    return;
                case VK.VK_BACK:
                    if (m_SelectForm.Input_TextBox.Text.Length > 0)
                        m_SelectForm.Input_TextBox.Text = m_SelectForm.Input_TextBox.Text.Substring(0, m_SelectForm.Input_TextBox.Text.Length - 1);
                    ALPHA_Pressed(HotKeys[(int)VK.VK_BACK]);
                    return;
            }
            SystemHotkey.RegisterHotKey(false, t_key_type.NONALPHA | t_key_type.ALPHA);//ALPHA added
            //MessageBox.Show("OK");
            //UpdateWord(m_SelectForm.SelectListBox.SelectedItem.ToString());
            if(m_SelectForm.SelectListBox.SelectedIndex>=0)// && selections_words.Count>m_SelectForm.SelectListBox.SelectedIndex)
                UpdateWord(getter(selections_words[m_SelectForm.SelectListBox.SelectedIndex].ToString().Replace("|","")));
            m_SelectForm.Input_TextBox.Text = "";
            m_SelectForm.Hide();
            User32.keybd_event((byte)p.VirtualKey, 0, 0, 0);
            selections_strings.Clear();
            Reread();
            SystemHotkey.RegisterHotKey(false, t_key_type.NONALPHA);
            SystemHotkey.RegisterHotKey(true, t_key_type.ALPHA);//Added
        }
       
        bool Active
        {
            get
            {
                return ((int)SystemHotkey._RegisteredTypes & (int)t_key_type.ALPHA) != 0;
            }
        }
       
        private void CTRL_Pressed(SystemHotkey p)
        {
            m_SelectForm.Input_TextBox.Text = "";
            SystemHotkey.RegisterHotKey(false, t_key_type.NONALPHA);
            if(p!=null)
                if (!Active)
                    ActiveWNDsList.Add(User32.GetForegroundWindow());
                else
                    ActiveWNDsList.Remove(User32.GetForegroundWindow());
            SystemHotkey.RegisterHotKey(!Active, t_key_type.ALPHA);
        }
        private char ReadVk(int vk)
        {
            if (vk == (int)VK.VK_SLASH)
                return '/';
            return (char)(vk + 'a' - 'A');
        }
        private void GetVkMod(char c,ref VK vk,ref RegisterHotKeyModifiers  md)
        {
            short s = User32.VkKeyScan((byte)c);
            if ((s & 0x100) != 0)
                md = RegisterHotKeyModifiers.MOD_SHIFT;
            else
                md = RegisterHotKeyModifiers.MOD_NONE;
            vk = (VK)(byte)s;
        }

        List<IntPtr> ActiveWNDsList = new List<IntPtr>();
        
        private void UpdateWord(string output_text)
        {
            User32.keybd_event((int)VK.VK_SPACE/*0x20*/, 0, 0, 0);
            User32.keybd_event((int)VK.VK_SPACE/*0x20*/, 0, 2, 0);
            User32.keybd_event((int)VK.VK_BACK/*0x08*/, 0, 0, 0);
            User32.keybd_event((int)VK.VK_BACK/*0x08*/, 0, 2, 0);

            System.Windows.Forms.Clipboard.SetText(output_text);

            User32.keybd_event((int)VK.VK_CONTROL/*0x11*/, 0, 0, 0);
            User32.keybd_event((int)VK.VK_V/*(byte)'V'*/, 0, 0, 0);
            User32.keybd_event((int)VK.VK_V/*(byte)'V'*/, 0, 2, 0);
            User32.keybd_event((int)VK.VK_CONTROL/*0x11*/, 0, 2, 0);
        }

        public Engine()
        {
            word.Initialize();
            SystemHotkey.Pressed[t_key_type.CTRL    ] = new PressedDelegate(CTRL_Pressed);
            SystemHotkey.Pressed[t_key_type.ALPHA   ] = new PressedDelegate(ALPHA_Pressed);
            SystemHotkey.Pressed[t_key_type.NONALPHA] = new PressedDelegate(NONALPHA_Pressed);
            

            new SystemHotkey(VK.VK_F8, 0).Key_Type=t_key_type.CTRL;
            for (int i = 'A'; i <= 'Z'; i++)
            {
                HotKeys[i] = 
                    new SystemHotkey((VK)i, RegisterHotKeyModifiers.MOD_NONE);
                HotKeys[(int)RegisterHotKeyModifiers.MOD_SHIFT * 0x100 + i] = 
                    new SystemHotkey((VK)i, RegisterHotKeyModifiers.MOD_SHIFT);
            }
            foreach (VK i in nonalphas)
                HotKeys[(int)i] = 
                    new SystemHotkey(i, 0);
            foreach (VK i in nonalphas)
                HotKeys[(int)RegisterHotKeyModifiers.MOD_SHIFT * 0x100 + (int)i] = 
                    new SystemHotkey(i, RegisterHotKeyModifiers.MOD_SHIFT);

            input_method = Method.SriShell_Guess;
            output_method = Method.kaputadotcom;
            m_SettingsForm = new SettingsForm(this);
            m_SelectForm = new SelectForm(this);
           

            SystemHotkey.RegisterHotKey(true, t_key_type.CTRL);

            //InitializeComponent();
        }
        
        public void Reread()
        {
            Point p = new Point();
            User32.GetCursorPos(ref p);
            p.Y += 10;
            m_SelectForm.Location = p;
            m_SelectForm.SelectListBox.DataSource = null;
            m_SelectForm.SelectListBox.DataSource = selections_strings;
        }

        IntPtr LastActiveWindow;

        protected void Timer_Tick()
        {
            IntPtr i,x;
            i=User32.GetForegroundWindow();
            if (i == m_SelectForm.Handle)
            {
                User32.SetForegroundWindow(LastActiveWindow);
                return;
            }
            x = ActiveWNDsList.Find(delegate(IntPtr p) { return p == i; });
            if ((x.ToInt32() == 0) == (Active))
                CTRL_Pressed(null);
            if (Active)
                LastActiveWindow = i;
            else
                m_SelectForm.Hide();
        }
        public void SelectListBox_MouseDown()
        {
            m_SelectForm.Input_TextBox.Text = selections_words[m_SelectForm.SelectListBox.SelectedIndex].ToString().Replace("|", "");
        }
 
    }
}

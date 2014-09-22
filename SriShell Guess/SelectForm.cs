using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MySystemHotkey;
using CodeProject.Win32;
using System.IO;
using System.Text.RegularExpressions;
using sritext;

namespace SriShell_Guess
{
    public partial class SelectForm : Form
    {
        /*SettingsForm m_SettingsForm;
        public List<string> selections_strings = new List<string>();
        List<word> selections_words = new List<word>();
        Dictionary<int, SystemHotkey> HotKeys = new Dictionary<int, SystemHotkey>();
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

        ~SelectForm()
        {
            TextWriter tw = new StreamWriter("errors.txt");
            tw.WriteLine(errors);
            tw.Close();
        }
#endif

        private void ALPHA_Pressed(SystemHotkey p)
        {
            //Show();
            if(p.Char!=0)Input_TextBox.Text += p.Char;
            if (Regex.IsMatch(Input_TextBox.Text, @"(^|[^/])[+\-]$"))
            {
                NONALPHA_Pressed(p);
                return;
            }
            selections_strings.Clear();
            selections_words=word.Selections(Input_TextBox.Text);
            foreach (word w in selections_words)
                selections_strings.Add(Converter.getKaputa(w.ToString().Replace("|", "")));
            //if (selections_words.Count == 0) 
            //selections_strings.Add(Converter.getKaputa(Input_TextBox.Text.ToString().Replace("|", "")));
            Reread();
            SelectListBox.SelectedIndex = 0;
            SystemHotkey.RegisterHotKey(true, t_key_type.ALPHA | t_key_type.NONALPHA);
            Show();
            User32.SetForegroundWindow(LastActiveWindow);
        }

        private void NONALPHA_Pressed(SystemHotkey p)
        {
            switch ((VK)(p.VirtualKey))
            {
                case VK.VK_DOWN:
                    SelectListBox.SelectedIndex++;
                    Input_TextBox.Text = selections_words[SelectListBox.SelectedIndex].ToString().Replace("|", "");
                    Reread();
                    return;
                case VK.VK_UP:
                    SelectListBox.SelectedIndex--;
                    Input_TextBox.Text = selections_words[SelectListBox.SelectedIndex].ToString().Replace("|", "");
                    Reread();
                    return;
                case VK.VK_BACK:
                    if (Input_TextBox.Text.Length > 0)
                        Input_TextBox.Text = Input_TextBox.Text.Substring(0, Input_TextBox.Text.Length - 1);
                    ALPHA_Pressed(HotKeys[(int)VK.VK_BACK]);
                    return;
            }
            SystemHotkey.RegisterHotKey(false, t_key_type.NONALPHA | t_key_type.ALPHA);//ALPHA added
            //MessageBox.Show("OK");
            //UpdateWord(SelectListBox.SelectedItem.ToString());
            if(SelectListBox.SelectedIndex>=0)// && selections_words.Count>SelectListBox.SelectedIndex)
                UpdateWord(getter(selections_words[SelectListBox.SelectedIndex].ToString().Replace("|","")));
            Input_TextBox.Text = "";
            Hide();
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
            Input_TextBox.Text = "";
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
        
*/
        MainForm engine;
        public SelectForm(MainForm e)
        {
            engine = e;
            InitializeComponent();
        }
        
        private void SelectListBox_MouseDown(object sender, MouseEventArgs e)
        {
            engine.SelectListBox_MouseDown();
            //Input_TextBox.Text = selections_words[SelectListBox.SelectedIndex].ToString().Replace("|", "");
        }
        
      
    }
}
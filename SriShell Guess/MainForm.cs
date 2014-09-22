//#define BI
//#define DEBUGING

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MySystemHotkey;
using System.IO;
using sritext;
using System.Text.RegularExpressions;
using CodeProject.Win32;
using CaptScrn;
using System.Drawing.Imaging;

namespace SriShell_Guess
{
    public partial class MainForm : Form
    {        //string m_SelectForm.Input_TextBox.Text;
        //Thread alphaThread;
        public SettingsForm m_SettingsForm;
        public SriWordForm m_SriWordForm;
        SelectForm m_SelectForm;
        public List<string> selections_strings = new List<string>();
        List<trie.word> selections_words = new List<trie.word>();
        Dictionary<int, SystemHotkey> HotKeys = new Dictionary<int, SystemHotkey>();
        string previous_seleted_word=".|";

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
                //VK.VK_BACK_QUOTE,VK.VK_QUOTE,
                VK.VK_0,VK.VK_1,VK.VK_2,VK.VK_3,VK.VK_4,VK.VK_5,VK.VK_6,VK.VK_7,VK.VK_8,VK.VK_9,
                VK.VK_OPEN_BRACKET,VK.VK_CLOSE_BRACKET,VK.VK_SEMICOLON
            };

        public dele_converter reader;
        public dele_converter getter;
        Method m_input_method;
        Method m_output_method;
        public Method menu_method;
        public Method input_method
        {
            set
            {
                m_input_method = value;
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
            get
            {
                return m_input_method;
            }
        }
        public Method output_method
        {
            set
            {
                m_output_method = value;
                getter = Converter.getters[value];
            }
            get
            {
                return m_output_method;
            }
        }

#if DEBUG
        public string errors="";

        ~MainForm()
        {
            TextWriter tw = new StreamWriter("errors.txt");
            tw.WriteLine(errors);
            tw.Close();
            //pos_thread.Abort();
        }
#endif
        private void ALPHA_Pressed(SystemHotkey p)
        {
            //LOG("Alpha_Pressed(" + p.Char.ToString() + ")");
            //MessageBox.Show("OK");
           
            if (p.Char != 0) m_SelectForm.Input_TextBox.Text += p.Char;
            if (Regex.IsMatch(m_SelectForm.Input_TextBox.Text, @"(^|[^/])[+\-]$"))
            {
                NONALPHA_Pressed(p);
                return;
            }
            if (m_SelectForm.SelectListBox.SelectedIndex > 0)
                LOG(">" + m_SelectForm.Input_TextBox.Text + "=>" + selections_words[m_SelectForm.SelectListBox.SelectedIndex].ToString().Replace("|", "") + "(" + m_SelectForm.SelectListBox.SelectedIndex.ToString() + ")");
            LOG(":" + p.Char.ToString());
            
            SystemHotkey.RegisterHotKey(true, t_key_type.ALPHA | t_key_type.NONALPHA);
            //ALPHA_Thread();
            /*try
            {
                alphaThread.Abort();
            }
            catch { } alphaThread = new Thread(new ThreadStart(ALPHA_Thread));
            alphaThread.Start();*/
            ALPHA_Thread();
            //LOG("~Alpha_Pressed(" + p.Char.ToString()+ ")");
        }

        
        private void ALPHA_Thread(/*SystemHotkey p*/)
        {
            selections_words = trie.TRIE.findAll(/*previous_seleted_word,*/m_SelectForm.Input_TextBox.Text,false);
            selections_strings.Clear();
            int i = 0;
            foreach (trie.word w in selections_words)
                selections_strings.Add(
                    (++i == selections_words.Count && -1==w.ToString().IndexOf('|') ? "0" : i < 10 ? i.ToString() : " ") 
                    + " "
                    + Converter.getters[menu_method](w.ToString().Replace("|", ""))
                    //+ " "+w.m_index+ " "
                    //+ " (" + w.OriginalProbability.ToString("F2") + ")"
                    //+ " (" +(w.Probability-w.OriginalProbability).ToString("F2") + ")"//+w.m_index.ToString()
                    );
            Reread();
            if(menu_method==Method.kaputadotcom)
                m_SelectForm.SelectListBox.Font = new Font("kaputadotcom", 22);
            else
                m_SelectForm.SelectListBox.Font = new Font("Potha", 22);

            m_SelectForm.SelectListBox.SelectedIndex = 0;
            m_SelectForm.Show();
            User32.SetForegroundWindow(LastActiveWindow);
        }
        
        private void NONALPHA_Pressed(SystemHotkey p)
        {
            switch ((VK)(p.VirtualKey))
            {
                case VK.VK_DOWN:
                    m_SelectForm.SelectListBox.SelectedIndex++;
                    //m_SelectForm.Input_TextBox.Text = selections_words[m_SelectForm.SelectListBox.SelectedIndex].ToString().Replace("|", "");
                    Reread();
                    return;
                case VK.VK_UP:
                    m_SelectForm.SelectListBox.SelectedIndex--;
                    //m_SelectForm.Input_TextBox.Text = selections_words[m_SelectForm.SelectListBox.SelectedIndex].ToString().Replace("|", "");
                    Reread();
                    return;
                case VK.VK_BACK:
                    LOG("BACK");
                    if (m_SelectForm.Input_TextBox.Text == "") goto N_ALPHA; 
                    if (m_SelectForm.Input_TextBox.Text.Length > 0)
                        m_SelectForm.Input_TextBox.Text = m_SelectForm.Input_TextBox.Text.Substring(0, m_SelectForm.Input_TextBox.Text.Length - 1);
                    ALPHA_Pressed(HotKeys[(int)VK.VK_BACK]);
                    return;
                case VK.VK_0:
                case VK.VK_1:
                case VK.VK_2:
                case VK.VK_3:
                case VK.VK_4:
                case VK.VK_5:
                case VK.VK_6:
                case VK.VK_7:
                case VK.VK_8:
                    m_SelectForm.SelectListBox.SelectedIndex = (VK)p.VirtualKey == VK.VK_0 ? selections_strings.Count - 1 : p.VirtualKey - (int)VK.VK_1;
                    return;
            }
            //alphaThread.Join();

            //MessageBox.Show("OK");
            trie.word w=selections_words[m_SelectForm.SelectListBox.SelectedIndex];
            string parse=variations.Parse2String(
                m_SelectForm.Input_TextBox.Text,
                variations.SplitIntoChars(w.ToString().Replace(" ","")));
            List<trie.word> tmp = trie.TRIE.findAll(m_SelectForm.Input_TextBox.Text, true);
            /*foreach (trie.word iw in tmp)
                iw.TravelProbability = double.NegativeInfinity;
            tmp.Sort();*/
            int old_selection=tmp.IndexOf(w);
            if (old_selection == -1)
                for (int i = 0; i < tmp.Count; i++)
                    if (tmp[i].ToString() == w.ToString())
                    {
                        old_selection = i;
                        break;
                    }
            LOG(parse + "=>" + 
                selections_words[m_SelectForm.SelectListBox.SelectedIndex].ToString() +
                "(" + m_SelectForm.SelectListBox.SelectedIndex.ToString() + ")"
                +"[" + old_selection.ToString() + "]"
                );

            trie.TRIE.FeedBack(m_SelectForm.Input_TextBox.Text, selections_words[m_SelectForm.SelectListBox.SelectedIndex]);
            //MessageBox.Show("NONALPHA");

            SystemHotkey.RegisterHotKey(false, t_key_type.NONALPHA | t_key_type.ALPHA);//ALPHA added
            //UpdateWord(m_SelectForm.SelectListBox.SelectedItem.ToString());
            
            if (m_SelectForm.SelectListBox.SelectedIndex >= 0)
            {
                UpdateWord(getter(selections_words[m_SelectForm.SelectListBox.SelectedIndex].ToString().Replace("|", "")));
                previous_seleted_word = selections_words[m_SelectForm.SelectListBox.SelectedIndex].ToString();
            }
N_ALPHA:
            m_SelectForm.Input_TextBox.Text = "";
#if !BI
            m_SelectForm.Hide();
#endif
#if !DEBUGING
            User32.keybd_event((byte)p.VirtualKey, 0, 0, 0);
#endif
            selections_strings.Clear();
            Reread();

            SystemHotkey.RegisterHotKey(true, t_key_type.ALPHA);//Added
#if !BI
            SystemHotkey.RegisterHotKey(false , t_key_type.NONALPHA);
#else
            SystemHotkey.RegisterHotKey(true, t_key_type.NONALPHA);
            ALPHA_Thread();
#endif
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
            //if (Active) ALPHA_Pressed(null);
            //try { pos_thread.Abort(); }
            //catch { }
            //pos_thread = new Thread(GetCursorPosition);
            //pos_thread.Start();
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
#if !DEBUGING
            User32.keybd_event((int)VK.VK_SPACE/*0x20*/, 0, 0, 0);
            User32.keybd_event((int)VK.VK_SPACE/*0x20*/, 0, 2, 0);
            User32.keybd_event((int)VK.VK_BACK/*0x08*/, 0, 0, 0);
            User32.keybd_event((int)VK.VK_BACK/*0x08*/, 0, 2, 0);

            System.Windows.Forms.Clipboard.SetText(output_text);

            User32.keybd_event((int)VK.VK_CONTROL/*0x11*/, 0, 0, 0);
            User32.keybd_event((int)VK.VK_V/*(byte)'V'*/, 0, 0, 0);
            User32.keybd_event((int)VK.VK_V/*(byte)'V'*/, 0, 2, 0);
            User32.keybd_event((int)VK.VK_CONTROL/*0x11*/, 0, 2, 0);
#endif
            //try { pos_thread.Abort(); }
            //catch { }
            //pos_thread.Start();
        }
        //Point cur_pos = new Point(0, 0);
        /*
        void GetCursorPosition()
        {
            Image img1,img2;
            Bitmap b1, b2;
            User32.RECT rect = new User32.RECT();
            User32.GetWindowRect(User32.GetForegroundWindow(), ref rect);
            cur_pos.X = Math.Max(rect.left, cur_pos.X);
            cur_pos.Y = Math.Max(rect.top, cur_pos.Y);
            while (true)
            {
                img1 = ScreenCapture.CaptureScreen();
                Thread.Sleep(100);
                img2 = ScreenCapture.CaptureScreen();
                b1 = new Bitmap(img1);
                b2 = new Bitmap(img2);
                for (int x = Math.Max(rect.left, 0); x < Math.Min(rect.right,b1.Width); x++)
                    for (int y = Math.Max(rect.top,0) ; y <Math.Min(rect.bottom,b1.Height) ; y+=5)
                        if (b1.GetPixel(x, y) == b2.GetPixel(x, y))
                            b2.SetPixel(x, y, Color.Black);
                        else
                        {
                            b2.SetPixel(x, y, Color.White);
                            cur_pos.X = x;
                            cur_pos.Y = y;
                            goto brk;
                        }
            }
            brk:
                b1.Save("img1.jpg", ImageFormat.Jpeg);
                b2.Save("img2.jpg", ImageFormat.Jpeg);
        }*/
        public void Reread()
        {
            User32.RECT rect = new User32.RECT();
            User32.GetWindowRect(LastActiveWindow, ref rect);

            Point p = new Point();
            User32.GetCursorPos(ref p);
            p.Y += 10;
            if (p.X < rect.left || p.X > rect.right || p.Y < rect.top || p.Y > rect.bottom)
                p =new Point( (rect.left + rect.right )/ 2,(rect.top+rect.bottom)/2);
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
        //Thread pos_thread;
        public MainForm()
        {
            //word.Initialize();
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
            menu_method = Method.kaputadotcom;
            m_SettingsForm = new SettingsForm(this);
            m_SelectForm = new SelectForm(this);
            m_SriWordForm = new SriWordForm(this);
           

            SystemHotkey.RegisterHotKey(true, t_key_type.CTRL);

            InitializeComponent();
            //File.WriteAllText("sri shell guess.log", "");
            //pos_thread = new Thread(GetCursorPosition);
            //pos_thread.Start();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_SettingsForm.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void sriWordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_SriWordForm.Show();
        }

        private void ForgroundWNDtimer_Tick_1(object sender, EventArgs e)
        {          
            //LoadingProgress.Value =(LoadingProgress.Value<95)?LoadingProgress.Value+5:word.LoadingCompleted;
            // (int)trie.TRIE.LoadingCompleted;
            if (LoadingProgress.Value == 100)
                Hide();
            else
                LoadingProgress.Value += 25;
            Timer_Tick();
        }

        DateTime last = DateTime.Now; 
        void LOG(string s)
        {
            DateTime now=DateTime.Now;
            TimeSpan span = now - last;
            try
            {
                File.AppendAllText("sri shell guess.log", s + Convert.ToInt32(span.TotalMilliseconds).ToString() + "(" + now.ToLongTimeString() + "." + now.Millisecond.ToString("000") + ")\r\n");
            }
            catch { }
            last = now;            
        }
    }
}
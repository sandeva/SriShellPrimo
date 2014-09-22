using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using sritext;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Text.RegularExpressions;


namespace SriShell_Guess
{
    public partial class SriWordForm : Form
    {

        MainForm engine;
        public SriWordForm(MainForm e)
        {
            engine=e;
            InitializeComponent();
        }

        void Update_Output_Text()
        {

            OutputUpadatingThreadsOutputText = "";
            foreach(string s in Regex.Split(OutputUpadatingThreadsInputText," "))
                OutputUpadatingThreadsOutputText +=engine.getter(engine.reader(s))+" ";
        }
        Thread OutputUpadatingThread;
        string OutputUpadatingThreadsInputText = "", OutputUpadatingThreadsOutputText = "";
        private void input_TextChanged(object sender, EventArgs e)
        {
            if (input.Text == OutputUpadatingThreadsInputText) return;
            //MessageBox.Show("TEXT CHNG");
            OutputTextTimer.Enabled = true;
            try
            {
                OutputUpadatingThread.Abort();
            }
            catch { }
            OutputUpadatingThread = new Thread(new ThreadStart(Update_Output_Text));
            OutputUpadatingThreadsInputText = input.Text;
            OutputUpadatingThread.Start();
        }

        private void notifyIcn_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.SetTopLevel(true);
        }

        private void ok_Click(object sender, EventArgs e)
        {
            Hide();
        }

       
        private void terminate_Click(object sender, EventArgs e)
        {
            Hide();
            Environment.Exit(0);
        }


        private void output_focused(object sender, EventArgs e)
        {
            //input.Focus();
        }


 


        private void openToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlgOpen = new OpenFileDialog();

            dlgOpen.Title = "Open file";
            dlgOpen.ShowReadOnly = true;
            if (dlgOpen.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(dlgOpen.FileName);
                input.Text = sr.ReadToEnd();
                sr.Close();
            }
        }

      
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IDataObject iData = Clipboard.GetDataObject();
            //Determine whether the data is in a format you can use.
            if (iData.GetDataPresent(DataFormats.Text))
            {
                //Yes it is, so display it in a text box.			
                input.SelectedText = (String)iData.GetData(DataFormats.Text);

            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlgOpen = new OpenFileDialog();

            dlgOpen.Title = "Save file";
            dlgOpen.ShowReadOnly = true;
            if (dlgOpen.ShowDialog() == DialogResult.OK)
                output.SaveFile(dlgOpen.FileName);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (OutputUpadatingThread.ThreadState == System.Threading.ThreadState.Stopped)
            {
                OutputTextTimer.Enabled = false;
                input_SelectionChanged(null, null);
            }
            output.Text = OutputUpadatingThreadsOutputText;
        }

        List<trie.word> selections_words=new List<trie.word>();
        List<string> selections_strings = new List<string>();
        bool WorkingWithMenu = false;
        private void input_SelectionChanged(object sender, EventArgs e)
        {
            if (engine.input_method != Method.SriShell_Guess) return;
            if (input.Text != OutputUpadatingThreadsInputText) return;
            if (!input.Focused) return;
            if (OutputTextTimer.Enabled) return;
            if (WorkingWithMenu) return;
            WorkingWithMenu = true;
            //MessageBox.Show("INPUT");
            int i = input.SelectionStart;
            int wd = input.SelectionLength;

            string a = input.Text.Substring(0, i);
            string b = input.Text.Substring(i);
            string aa = Regex.Match(a, @"[a-z/+\-]*[0-9]?$", RegexOptions.Singleline).Groups[0].ToString();
            string bb = Regex.Match(b, @"^[a-z/+\-]*[0-9]?", RegexOptions.Singleline).Groups[0].ToString();
            if (Regex.IsMatch(aa, @"[0-9]")) bb = "";
            string s = aa + bb;
            a=engine.getter(engine.reader(a.Substring(0, a.Length - aa.Length)));
            
            string ss = engine.getter(engine.reader(s));
            output.Select(0,output.Text.Length);
            output.SelectionBackColor = Color.White;
            output.Select(a.Length, ss.Length);
            output.SelectionBackColor = Color.Red;
            input.Select(0, input.Text.Length);
            input.SelectionBackColor = Color.White;
            input.Select(i, wd);
            int x=1;
            try
            {
                x = int.Parse(s.Substring(s.Length - 1));
                s = s.Substring(0, s.Length - 1);
            }
            catch { }
            if (Convert_TextBox.Text == s)
            {
                WorkingWithMenu = false;
                return;
            }
            Convert_TextBox.Text = s;
            selections_strings.Clear();
            if (s == "")
                selections_words.Clear();
            else
                selections_words = trie.TRIE.findAll(/*".|",*/s,false);//word.SelectionsSafe(s);
            foreach (trie.word w in selections_words)
                selections_strings.Add(Converter.getKaputa(w.ToString().Replace("|", "")));
            SelectListBox.DataSource = null;
            SelectListBox.DataSource = selections_strings;
            if (selections_strings.Count >= x)
                SelectListBox.SelectedIndex = x==0?selections_strings.Count-1 :x - 1;
            //MessageBox.Show(a + "\n" + b + "\n" + aa + "\n" + bb);
            WorkingWithMenu = false;

        }

        private void SelectListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (WorkingWithMenu) return;
            WorkingWithMenu = true;
            int i = input.SelectionStart;

            string a = input.Text.Substring(0, i);
            string b = input.Text.Substring(i);
            string aa = Regex.Match(a, @"[a-z+\-]*[0-9]?$", RegexOptions.Singleline).Groups[0].ToString();
            string bb = Regex.Match(b, @"^[a-z+\-]*[0-9]?", RegexOptions.Singleline).Groups[0].ToString();
            if (Regex.IsMatch(aa, @"[0-9]")) bb = "";
            string s = aa + bb;
            input.Select(i - aa.Length,s.Length);
            input.SelectedText = selections_words[SelectListBox.SelectedIndex].ToString().Replace("|","")+"0";
            WorkingWithMenu = false;
        }

        private void output_SelectionChanged(object sender, EventArgs e)
        {
            if (input.Text != OutputUpadatingThreadsInputText) return;
            if (!output.Focused) return;
            if (OutputTextTimer.Enabled) return;
            if (WorkingWithMenu) return;
            WorkingWithMenu = true;
            
            int i,x;
            string ai="", bi, aai, bbi, si="",ao,bo,aao,bbo,so;
            i = output.SelectionStart;
            x = i;
            int wd = output.SelectionLength;

            ao = output.Text.Substring(0, i);
            bo = output.Text.Substring(i);
            aao = Regex.Match(ao, @"[^., ]*$", RegexOptions.Singleline).Groups[0].ToString();
            bbo = Regex.Match(bo, @"^[^., ]*", RegexOptions.Singleline).Groups[0].ToString();
            ao = ao.Substring(0, ao.Length - aao.Length);
            so = aao + bbo;
            
            
            for (i = 0; i < input.Text.Length; i++)
            {
                ai = input.Text.Substring(0, i);
                bi = input.Text.Substring(i);
                aai = Regex.Match(ai, @"[a-z/+\-]*[0-9]?$", RegexOptions.Singleline).Groups[0].ToString();
                bbi = Regex.Match(bi, @"^[a-z/+\-]*[0-9]?", RegexOptions.Singleline).Groups[0].ToString();
                if (Regex.IsMatch(aai, @"[0-9]")) bbi = "";
                ai = ai.Substring(0, ai.Length - aai.Length);
                si = aai + bbi;
                if(ao==engine.getter(engine.reader(ai)))
                    break;
            }
            
            //a = engine.getter(engine.reader(a.Substring(0, a.Length - aa.Length)));

            input.Select(0, input.Text.Length);
            input.SelectionBackColor = Color.White;
            
            input.Select(ai.Length, si.Length);//s.Length);
            input.SelectionBackColor = Color.Red;
            output.Select(0, output.Text.Length);
            output.SelectionBackColor = Color.White;
            output.Select(x, wd);

            try
            {
                x = int.Parse(si.Substring(si.Length - 1));
                si = si.Substring(0, si.Length - 1);
            }
            catch { }
            if (Convert_TextBox.Text == si)
            {
                WorkingWithMenu = false;
                return;
            }
            Convert_TextBox.Text = si;
            selections_strings.Clear();
            if (si == "")
                selections_words.Clear();
            else
                selections_words =trie.TRIE.findAll(/*".|"*/si,false);//word.SelectionsSafe(si);
            foreach (trie.word w in selections_words)
                selections_strings.Add(Converter.getKaputa(w.ToString().Replace("|", "")));
            SelectListBox.DataSource = null;
            SelectListBox.DataSource = selections_strings;
            if (selections_strings.Count >= x)
                SelectListBox.SelectedIndex = x == 0 ? selections_strings.Count - 1 : x - 1;
            WorkingWithMenu = false;
        }

        private void SriWordForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }
    }
}
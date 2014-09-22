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


namespace SriShell_Guess
{
    public partial class SettingsForm : Form
    {
        MainForm engine;
        private void LoadSettings()
        {
            Stream stream = null;
            try {
                IFormatter formatter = new BinaryFormatter();
                stream = new FileStream("sri shell settings.dat", FileMode.Open, FileAccess.Read, FileShare.None);

                radio_output_kaputa    .Checked = (bool)formatter.Deserialize(stream);
                radio_output_singlish  .Checked = (bool)formatter.Deserialize(stream);
                radio_output_unicode   .Checked = (bool)formatter.Deserialize(stream);
                radio_output_sritext   .Checked = (bool)formatter.Deserialize(stream);
                radio_input_guess      .Checked = (bool)formatter.Deserialize(stream);
                radio_input_kaputa     .Checked = (bool)formatter.Deserialize(stream);
                radio_input_unicode    .Checked = (bool)formatter.Deserialize(stream);
                radio_input_singlish   .Checked = (bool)formatter.Deserialize(stream);
                radio_input_sritext    .Checked = (bool)formatter.Deserialize(stream);
                radioButton1           .Checked = (bool)formatter.Deserialize(stream);
                radioButton3           .Checked = (bool)formatter.Deserialize(stream);
                radioButton4           .Checked = (bool)formatter.Deserialize(stream);
            } catch {
                // do nothing, just ignore any possible errors
            } finally {
                if (null != stream)
                    stream.Close();
            }
            //OutputChanged(null,null);
        }

        private void SaveSettings()
        {
            Stream stream = null;
            try
            {
                IFormatter formatter = new BinaryFormatter();
                stream = new FileStream("sri shell settings.dat", FileMode.Create, FileAccess.Write, FileShare.None);

                formatter.Serialize(stream, radio_output_kaputa    .Checked);
                formatter.Serialize(stream, radio_output_singlish  .Checked);
                formatter.Serialize(stream, radio_output_unicode   .Checked);
                formatter.Serialize(stream, radio_output_sritext   .Checked);
                formatter.Serialize(stream, radio_input_guess      .Checked);
                formatter.Serialize(stream, radio_input_kaputa     .Checked);
                formatter.Serialize(stream, radio_input_unicode    .Checked);
                formatter.Serialize(stream, radio_input_singlish   .Checked);
                formatter.Serialize(stream, radio_input_sritext    .Checked);
                formatter.Serialize(stream, radioButton1           .Checked);
                formatter.Serialize(stream, radioButton3           .Checked);
                formatter.Serialize(stream, radioButton4           .Checked);
            }
            catch
            {
                // do nothing, just ignore any possible errors
            }
            finally
            {
                if (null != stream)
                    stream.Close();
            }
        }

        public SettingsForm(MainForm e)
        {
            engine = e;
            InitializeComponent();
            LoadSettings();
            radio_output_singlish.Enabled = false;
        }

       

        private void OutputChanged(object sender, EventArgs e)
        {
            if (!((RadioButton)sender).Checked) return;
            engine.output_method = (Method)Enum.Parse(typeof(Method), ((RadioButton)sender).Text.Replace(" ", "_"));
            SaveSettings();
        }

        private void InputChanged(object sender, EventArgs e)
        {
            if (!((RadioButton)sender).Checked) return;
            engine.input_method = (Method)Enum.Parse(typeof(Method), ((RadioButton)sender).Text.Replace(" ", "_"));
            SaveSettings();
            //input_TextChanged(null, null);
        }

        private void MenuChanged(object sender, EventArgs e)
        {
            if (!((RadioButton)sender).Checked) return;
            engine.menu_method = (Method)Enum.Parse(typeof(Method), ((RadioButton)sender).Text.Replace(" ", "_"));
            SaveSettings();
        }
    }
}
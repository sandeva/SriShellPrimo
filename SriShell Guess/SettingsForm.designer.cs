namespace SriShell_Guess
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radio_output_kaputa = new System.Windows.Forms.RadioButton();
            this.radio_output_singlish = new System.Windows.Forms.RadioButton();
            this.radio_output_unicode = new System.Windows.Forms.RadioButton();
            this.radio_output_sritext = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radio_input_guess = new System.Windows.Forms.RadioButton();
            this.radio_input_kaputa = new System.Windows.Forms.RadioButton();
            this.radio_input_unicode = new System.Windows.Forms.RadioButton();
            this.radio_input_singlish = new System.Windows.Forms.RadioButton();
            this.radio_input_sritext = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radio_output_kaputa);
            this.groupBox2.Controls.Add(this.radio_output_singlish);
            this.groupBox2.Controls.Add(this.radio_output_unicode);
            this.groupBox2.Controls.Add(this.radio_output_sritext);
            this.groupBox2.Location = new System.Drawing.Point(133, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(115, 115);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Output Methods";
            // 
            // radio_output_kaputa
            // 
            this.radio_output_kaputa.AutoSize = true;
            this.radio_output_kaputa.Checked = true;
            this.radio_output_kaputa.Location = new System.Drawing.Point(14, 66);
            this.radio_output_kaputa.Name = "radio_output_kaputa";
            this.radio_output_kaputa.Size = new System.Drawing.Size(94, 16);
            this.radio_output_kaputa.TabIndex = 20;
            this.radio_output_kaputa.TabStop = true;
            this.radio_output_kaputa.Text = "kaputadotcom";
            this.radio_output_kaputa.UseVisualStyleBackColor = true;
            this.radio_output_kaputa.CheckedChanged += new System.EventHandler(this.OutputChanged);
            // 
            // radio_output_singlish
            // 
            this.radio_output_singlish.AutoSize = true;
            this.radio_output_singlish.Enabled = false;
            this.radio_output_singlish.Location = new System.Drawing.Point(14, 44);
            this.radio_output_singlish.Name = "radio_output_singlish";
            this.radio_output_singlish.Size = new System.Drawing.Size(65, 16);
            this.radio_output_singlish.TabIndex = 18;
            this.radio_output_singlish.Text = "SinGlish";
            this.radio_output_singlish.UseVisualStyleBackColor = true;
            this.radio_output_singlish.CheckedChanged += new System.EventHandler(this.OutputChanged);
            // 
            // radio_output_unicode
            // 
            this.radio_output_unicode.AutoSize = true;
            this.radio_output_unicode.Location = new System.Drawing.Point(14, 87);
            this.radio_output_unicode.Name = "radio_output_unicode";
            this.radio_output_unicode.Size = new System.Drawing.Size(62, 16);
            this.radio_output_unicode.TabIndex = 19;
            this.radio_output_unicode.Text = "unicode";
            this.radio_output_unicode.UseVisualStyleBackColor = true;
            this.radio_output_unicode.CheckedChanged += new System.EventHandler(this.OutputChanged);
            // 
            // radio_output_sritext
            // 
            this.radio_output_sritext.AutoSize = true;
            this.radio_output_sritext.Location = new System.Drawing.Point(14, 23);
            this.radio_output_sritext.Name = "radio_output_sritext";
            this.radio_output_sritext.Size = new System.Drawing.Size(64, 16);
            this.radio_output_sritext.TabIndex = 17;
            this.radio_output_sritext.Text = "Sri Text";
            this.radio_output_sritext.UseVisualStyleBackColor = true;
            this.radio_output_sritext.CheckedChanged += new System.EventHandler(this.OutputChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radio_input_guess);
            this.groupBox1.Controls.Add(this.radio_input_kaputa);
            this.groupBox1.Controls.Add(this.radio_input_unicode);
            this.groupBox1.Controls.Add(this.radio_input_singlish);
            this.groupBox1.Controls.Add(this.radio_input_sritext);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(115, 148);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input Methods";
            // 
            // radio_input_guess
            // 
            this.radio_input_guess.AutoSize = true;
            this.radio_input_guess.Checked = true;
            this.radio_input_guess.Location = new System.Drawing.Point(14, 46);
            this.radio_input_guess.Name = "radio_input_guess";
            this.radio_input_guess.Size = new System.Drawing.Size(98, 16);
            this.radio_input_guess.TabIndex = 17;
            this.radio_input_guess.TabStop = true;
            this.radio_input_guess.Text = "SriShell Guess";
            this.radio_input_guess.UseVisualStyleBackColor = true;
            this.radio_input_guess.CheckedChanged += new System.EventHandler(this.InputChanged);
            // 
            // radio_input_kaputa
            // 
            this.radio_input_kaputa.AutoSize = true;
            this.radio_input_kaputa.Location = new System.Drawing.Point(14, 89);
            this.radio_input_kaputa.Name = "radio_input_kaputa";
            this.radio_input_kaputa.Size = new System.Drawing.Size(94, 16);
            this.radio_input_kaputa.TabIndex = 16;
            this.radio_input_kaputa.Text = "kaputadotcom";
            this.radio_input_kaputa.UseVisualStyleBackColor = true;
            this.radio_input_kaputa.CheckedChanged += new System.EventHandler(this.InputChanged);
            // 
            // radio_input_unicode
            // 
            this.radio_input_unicode.AutoSize = true;
            this.radio_input_unicode.Location = new System.Drawing.Point(14, 111);
            this.radio_input_unicode.Name = "radio_input_unicode";
            this.radio_input_unicode.Size = new System.Drawing.Size(62, 16);
            this.radio_input_unicode.TabIndex = 15;
            this.radio_input_unicode.Text = "unicode";
            this.radio_input_unicode.UseVisualStyleBackColor = true;
            this.radio_input_unicode.CheckedChanged += new System.EventHandler(this.InputChanged);
            // 
            // radio_input_singlish
            // 
            this.radio_input_singlish.AutoSize = true;
            this.radio_input_singlish.Enabled = false;
            this.radio_input_singlish.Location = new System.Drawing.Point(14, 68);
            this.radio_input_singlish.Name = "radio_input_singlish";
            this.radio_input_singlish.Size = new System.Drawing.Size(65, 16);
            this.radio_input_singlish.TabIndex = 14;
            this.radio_input_singlish.Text = "SinGlish";
            this.radio_input_singlish.UseVisualStyleBackColor = true;
            this.radio_input_singlish.CheckedChanged += new System.EventHandler(this.InputChanged);
            // 
            // radio_input_sritext
            // 
            this.radio_input_sritext.AutoSize = true;
            this.radio_input_sritext.Location = new System.Drawing.Point(14, 24);
            this.radio_input_sritext.Name = "radio_input_sritext";
            this.radio_input_sritext.Size = new System.Drawing.Size(64, 16);
            this.radio_input_sritext.TabIndex = 0;
            this.radio_input_sritext.Text = "Sri Text";
            this.radio_input_sritext.UseVisualStyleBackColor = true;
            this.radio_input_sritext.CheckedChanged += new System.EventHandler(this.InputChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radioButton1);
            this.groupBox3.Controls.Add(this.radioButton3);
            this.groupBox3.Controls.Add(this.radioButton4);
            this.groupBox3.Location = new System.Drawing.Point(254, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(115, 115);
            this.groupBox3.TabIndex = 21;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Menu Display";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(14, 66);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(94, 16);
            this.radioButton1.TabIndex = 20;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "kaputadotcom";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.MenuChanged);
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(14, 87);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(62, 16);
            this.radioButton3.TabIndex = 19;
            this.radioButton3.Text = "unicode";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.MenuChanged);
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(14, 23);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(64, 16);
            this.radioButton4.TabIndex = 17;
            this.radioButton4.Text = "Sri Text";
            this.radioButton4.UseVisualStyleBackColor = true;
            this.radioButton4.CheckedChanged += new System.EventHandler(this.MenuChanged);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 175);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.Text = "SriShell Settings";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radio_output_kaputa;
        private System.Windows.Forms.RadioButton radio_output_singlish;
        private System.Windows.Forms.RadioButton radio_output_unicode;
        private System.Windows.Forms.RadioButton radio_output_sritext;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radio_input_kaputa;
        private System.Windows.Forms.RadioButton radio_input_unicode;
        private System.Windows.Forms.RadioButton radio_input_singlish;
        private System.Windows.Forms.RadioButton radio_input_sritext;
        private System.Windows.Forms.RadioButton radio_input_guess;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton4;

    }
}


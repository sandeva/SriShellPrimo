namespace SriShell_Guess
{
    partial class SelectForm
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
            this.SelectListBox = new System.Windows.Forms.ListBox();
            this.Input_TextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // SelectListBox
            // 
            this.SelectListBox.Font = new System.Drawing.Font("kaputadotcom", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SelectListBox.FormattingEnabled = true;
            this.SelectListBox.ItemHeight = 22;
            this.SelectListBox.Location = new System.Drawing.Point(0, 29);
            this.SelectListBox.Name = "SelectListBox";
            this.SelectListBox.Size = new System.Drawing.Size(310, 356);
            this.SelectListBox.TabIndex = 0;
            this.SelectListBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SelectListBox_MouseDown);
            // 
            // Input_TextBox
            // 
            this.Input_TextBox.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Input_TextBox.Location = new System.Drawing.Point(1, 1);
            this.Input_TextBox.Name = "Input_TextBox";
            this.Input_TextBox.Size = new System.Drawing.Size(309, 26);
            this.Input_TextBox.TabIndex = 1;
            // 
            // SelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(311, 363);
            this.Controls.Add(this.Input_TextBox);
            this.Controls.Add(this.SelectListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectForm";
            this.Text = "SelectForm";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ListBox SelectListBox;
        public System.Windows.Forms.TextBox Input_TextBox;
    }
}


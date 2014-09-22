namespace SriShell_Guess
{
    partial class Activation
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
            this.HDDID = new System.Windows.Forms.RichTextBox();
            this.NAME = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.PROD = new System.Windows.Forms.TextBox();
            this.GO = new System.Windows.Forms.Button();
            this.ACT = new System.Windows.Forms.Button();
            this.ACTCODE = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // HDDID
            // 
            this.HDDID.Location = new System.Drawing.Point(12, 151);
            this.HDDID.Name = "HDDID";
            this.HDDID.Size = new System.Drawing.Size(240, 101);
            this.HDDID.TabIndex = 3;
            this.HDDID.Text = "";
            // 
            // NAME
            // 
            this.NAME.Location = new System.Drawing.Point(12, 25);
            this.NAME.MaxLength = 75;
            this.NAME.Name = "NAME";
            this.NAME.Size = new System.Drawing.Size(240, 19);
            this.NAME.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(4, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Product Key:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(4, 135);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Installation ID:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(4, 255);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(197, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Paste Your Activation Code here:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(4, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Your Name:";
            // 
            // PROD
            // 
            this.PROD.Location = new System.Drawing.Point(12, 63);
            this.PROD.MaxLength = 25;
            this.PROD.Name = "PROD";
            this.PROD.Size = new System.Drawing.Size(240, 19);
            this.PROD.TabIndex = 2;
            // 
            // GO
            // 
            this.GO.Location = new System.Drawing.Point(12, 96);
            this.GO.Name = "GO";
            this.GO.Size = new System.Drawing.Size(240, 30);
            this.GO.TabIndex = 3;
            this.GO.Text = "Generate Installation ID";
            this.GO.UseVisualStyleBackColor = true;
            this.GO.Click += new System.EventHandler(this.GO_Click);
            // 
            // ACT
            // 
            this.ACT.Location = new System.Drawing.Point(12, 378);
            this.ACT.Name = "ACT";
            this.ACT.Size = new System.Drawing.Size(240, 30);
            this.ACT.TabIndex = 9;
            this.ACT.Text = "Activate";
            this.ACT.UseVisualStyleBackColor = true;
            this.ACT.Click += new System.EventHandler(this.ACT_Click);
            // 
            // ACTCODE
            // 
            this.ACTCODE.Location = new System.Drawing.Point(12, 271);
            this.ACTCODE.Name = "ACTCODE";
            this.ACTCODE.Size = new System.Drawing.Size(240, 101);
            this.ACTCODE.TabIndex = 10;
            this.ACTCODE.Text = "";
            // 
            // Activation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(265, 418);
            this.Controls.Add(this.ACTCODE);
            this.Controls.Add(this.ACT);
            this.Controls.Add(this.GO);
            this.Controls.Add(this.PROD);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NAME);
            this.Controls.Add(this.HDDID);
            this.Name = "Activation";
            this.Text = "Activation";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox HDDID;
        private System.Windows.Forms.TextBox NAME;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox PROD;
        private System.Windows.Forms.Button GO;
        private System.Windows.Forms.Button ACT;
        private System.Windows.Forms.RichTextBox ACTCODE;

    }
}
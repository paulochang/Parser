namespace Parser
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.filenameTB = new System.Windows.Forms.TextBox();
            this.BrowseBtn = new System.Windows.Forms.Button();
            this.GenerateBtn = new System.Windows.Forms.Button();
            this.instructionsLbl = new System.Windows.Forms.Label();
            this.stringViewer = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // filenameTB
            // 
            this.filenameTB.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.filenameTB.Location = new System.Drawing.Point(12, 28);
            this.filenameTB.Name = "filenameTB";
            this.filenameTB.Size = new System.Drawing.Size(268, 20);
            this.filenameTB.TabIndex = 0;
            // 
            // BrowseBtn
            // 
            this.BrowseBtn.Location = new System.Drawing.Point(286, 25);
            this.BrowseBtn.Name = "BrowseBtn";
            this.BrowseBtn.Size = new System.Drawing.Size(75, 23);
            this.BrowseBtn.TabIndex = 1;
            this.BrowseBtn.Text = "Examinar";
            this.BrowseBtn.UseVisualStyleBackColor = true;
            this.BrowseBtn.Click += new System.EventHandler(this.BrowseBtn_Click);
            // 
            // GenerateBtn
            // 
            this.GenerateBtn.Enabled = false;
            this.GenerateBtn.Location = new System.Drawing.Point(12, 55);
            this.GenerateBtn.Name = "GenerateBtn";
            this.GenerateBtn.Size = new System.Drawing.Size(75, 23);
            this.GenerateBtn.TabIndex = 2;
            this.GenerateBtn.Text = "Generar";
            this.GenerateBtn.UseVisualStyleBackColor = true;
            this.GenerateBtn.Click += new System.EventHandler(this.GenerateBtn_Click);
            // 
            // instructionsLbl
            // 
            this.instructionsLbl.AutoSize = true;
            this.instructionsLbl.Location = new System.Drawing.Point(9, 9);
            this.instructionsLbl.Name = "instructionsLbl";
            this.instructionsLbl.Size = new System.Drawing.Size(193, 13);
            this.instructionsLbl.TabIndex = 3;
            this.instructionsLbl.Text = "Seleccione el archivo con su gramática";
            // 
            // stringViewer
            // 
            this.stringViewer.Location = new System.Drawing.Point(13, 85);
            this.stringViewer.Multiline = true;
            this.stringViewer.Name = "stringViewer";
            this.stringViewer.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.stringViewer.Size = new System.Drawing.Size(348, 243);
            this.stringViewer.TabIndex = 4;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 340);
            this.Controls.Add(this.stringViewer);
            this.Controls.Add(this.instructionsLbl);
            this.Controls.Add(this.GenerateBtn);
            this.Controls.Add(this.BrowseBtn);
            this.Controls.Add(this.filenameTB);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Generador de Parsers";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox filenameTB;
        private System.Windows.Forms.Button BrowseBtn;
        private System.Windows.Forms.Button GenerateBtn;
        private System.Windows.Forms.Label instructionsLbl;
        private System.Windows.Forms.TextBox stringViewer;
    }
}


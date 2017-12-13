namespace WindowsFormsApp1
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.cbReaders = new System.Windows.Forms.ComboBox();
            this.txBxReponse = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txBxATR = new System.Windows.Forms.TextBox();
            this.btnDF = new System.Windows.Forms.Button();
            this.lblSelectReader = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.btnGetMemory = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(46, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Liste lecteurs";
            // 
            // cbReaders
            // 
            this.cbReaders.FormattingEnabled = true;
            this.cbReaders.Location = new System.Drawing.Point(115, 36);
            this.cbReaders.Name = "cbReaders";
            this.cbReaders.Size = new System.Drawing.Size(191, 21);
            this.cbReaders.TabIndex = 3;
            this.cbReaders.SelectedIndexChanged += new System.EventHandler(this.cbReaders_SelectedIndexChanged);
            // 
            // txBxReponse
            // 
            this.txBxReponse.Location = new System.Drawing.Point(115, 108);
            this.txBxReponse.Multiline = true;
            this.txBxReponse.Name = "txBxReponse";
            this.txBxReponse.Size = new System.Drawing.Size(191, 24);
            this.txBxReponse.TabIndex = 10;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(312, 34);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "Connecte";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(46, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Card ATR";
            // 
            // txBxATR
            // 
            this.txBxATR.Location = new System.Drawing.Point(115, 75);
            this.txBxATR.Name = "txBxATR";
            this.txBxATR.ReadOnly = true;
            this.txBxATR.Size = new System.Drawing.Size(191, 20);
            this.txBxATR.TabIndex = 13;
            // 
            // btnDF
            // 
            this.btnDF.Location = new System.Drawing.Point(49, 109);
            this.btnDF.Name = "btnDF";
            this.btnDF.Size = new System.Drawing.Size(51, 23);
            this.btnDF.TabIndex = 14;
            this.btnDF.Text = "Get MF";
            this.btnDF.UseVisualStyleBackColor = true;
            this.btnDF.Click += new System.EventHandler(this.btn_Get_MF_Click);
            // 
            // lblSelectReader
            // 
            this.lblSelectReader.AutoSize = true;
            this.lblSelectReader.Location = new System.Drawing.Point(131, 9);
            this.lblSelectReader.Name = "lblSelectReader";
            this.lblSelectReader.Size = new System.Drawing.Size(153, 13);
            this.lblSelectReader.TabIndex = 15;
            this.lblSelectReader.Text = "Veuillez sélectionner un lecteur";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(346, 157);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 16;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnGetMemory
            // 
            this.btnGetMemory.Location = new System.Drawing.Point(49, 157);
            this.btnGetMemory.Name = "btnGetMemory";
            this.btnGetMemory.Size = new System.Drawing.Size(75, 23);
            this.btnGetMemory.TabIndex = 17;
            this.btnGetMemory.Text = "Get Memory";
            this.btnGetMemory.UseVisualStyleBackColor = true;
            this.btnGetMemory.Click += new System.EventHandler(this.btn_Get_Memory_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(49, 186);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 18;
            this.button3.Text = "Structure 2";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(492, 239);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.btnGetMemory);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.lblSelectReader);
            this.Controls.Add(this.btnDF);
            this.Controls.Add(this.txBxATR);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txBxReponse);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbReaders);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ComboBox cbReaders;
        public System.Windows.Forms.TextBox txBxReponse;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txBxATR;
        private System.Windows.Forms.Button btnDF;
        private System.Windows.Forms.Label lblSelectReader;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnGetMemory;
        private System.Windows.Forms.Button button3;
    }
}


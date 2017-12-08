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
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboProtocols = new System.Windows.Forms.ComboBox();
            this.cbReaders = new System.Windows.Forms.ComboBox();
            this.lblShareMode = new System.Windows.Forms.Label();
            this.comboShareMode = new System.Windows.Forms.ComboBox();
            this.txBxReponse = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 242);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Card Type";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 207);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Card UID";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 169);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Card ATR";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 139);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Reponse";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Protocol";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Liste lecteurs";
            // 
            // comboProtocols
            // 
            this.comboProtocols.FormattingEnabled = true;
            this.comboProtocols.Location = new System.Drawing.Point(81, 102);
            this.comboProtocols.Name = "comboProtocols";
            this.comboProtocols.Size = new System.Drawing.Size(191, 21);
            this.comboProtocols.TabIndex = 2;
            this.comboProtocols.SelectedIndexChanged += new System.EventHandler(this.comboProtocols_SelectedIndexChanged);
            // 
            // cbReaders
            // 
            this.cbReaders.FormattingEnabled = true;
            this.cbReaders.Location = new System.Drawing.Point(81, 36);
            this.cbReaders.Name = "cbReaders";
            this.cbReaders.Size = new System.Drawing.Size(191, 21);
            this.cbReaders.TabIndex = 3;
            // 
            // lblShareMode
            // 
            this.lblShareMode.AutoSize = true;
            this.lblShareMode.Location = new System.Drawing.Point(12, 74);
            this.lblShareMode.Name = "lblShareMode";
            this.lblShareMode.Size = new System.Drawing.Size(62, 13);
            this.lblShareMode.TabIndex = 8;
            this.lblShareMode.Text = "ShareMode";
            // 
            // comboShareMode
            // 
            this.comboShareMode.FormattingEnabled = true;
            this.comboShareMode.Location = new System.Drawing.Point(81, 71);
            this.comboShareMode.Name = "comboShareMode";
            this.comboShareMode.Size = new System.Drawing.Size(191, 21);
            this.comboShareMode.TabIndex = 2;
            this.comboShareMode.SelectedValueChanged += new System.EventHandler(this.comboShareMode_SelectedValueChanged);
            // 
            // txBxReponse
            // 
            this.txBxReponse.Location = new System.Drawing.Point(81, 139);
            this.txBxReponse.Multiline = true;
            this.txBxReponse.Name = "txBxReponse";
            this.txBxReponse.Size = new System.Drawing.Size(191, 24);
            this.txBxReponse.TabIndex = 10;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(295, 36);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(396, 351);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txBxReponse);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblShareMode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboShareMode);
            this.Controls.Add(this.comboProtocols);
            this.Controls.Add(this.cbReaders);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblShareMode;
        public System.Windows.Forms.ComboBox comboProtocols;
        public System.Windows.Forms.ComboBox cbReaders;
        public System.Windows.Forms.ComboBox comboShareMode;
        public System.Windows.Forms.TextBox txBxReponse;
        private System.Windows.Forms.Button button1;
    }
}


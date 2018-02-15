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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.cbReaders = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txBxATR = new System.Windows.Forms.TextBox();
            this.lblSelectReader = new System.Windows.Forms.Label();
            this.lblProtocol = new System.Windows.Forms.Label();
            this.cBxProtocol = new System.Windows.Forms.ComboBox();
            this.lblShareMode = new System.Windows.Forms.Label();
            this.cBxShareMode = new System.Windows.Forms.ComboBox();
            this.btnDeconnecte = new System.Windows.Forms.Button();
            this.lblVendor = new System.Windows.Forms.Label();
            this.txBxAID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txBxappSN = new System.Windows.Forms.TextBox();
            this.btnQuitter = new System.Windows.Forms.Button();
            this.btnIso1443 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Liste lecteurs";
            // 
            // cbReaders
            // 
            this.cbReaders.FormattingEnabled = true;
            this.cbReaders.Location = new System.Drawing.Point(95, 36);
            this.cbReaders.Name = "cbReaders";
            this.cbReaders.Size = new System.Drawing.Size(225, 21);
            this.cbReaders.TabIndex = 1;
            this.cbReaders.SelectedIndexChanged += new System.EventHandler(this.cbReaders_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(114, 101);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Connecte";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 141);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Card ATR";
            // 
            // txBxATR
            // 
            this.txBxATR.Location = new System.Drawing.Point(95, 138);
            this.txBxATR.Name = "txBxATR";
            this.txBxATR.ReadOnly = true;
            this.txBxATR.Size = new System.Drawing.Size(225, 20);
            this.txBxATR.TabIndex = 13;
            // 
            // lblSelectReader
            // 
            this.lblSelectReader.AutoSize = true;
            this.lblSelectReader.Location = new System.Drawing.Point(111, 9);
            this.lblSelectReader.Name = "lblSelectReader";
            this.lblSelectReader.Size = new System.Drawing.Size(153, 13);
            this.lblSelectReader.TabIndex = 15;
            this.lblSelectReader.Text = "Veuillez sélectionner un lecteur";
            // 
            // lblProtocol
            // 
            this.lblProtocol.AutoSize = true;
            this.lblProtocol.Location = new System.Drawing.Point(16, 68);
            this.lblProtocol.Name = "lblProtocol";
            this.lblProtocol.Size = new System.Drawing.Size(46, 13);
            this.lblProtocol.TabIndex = 19;
            this.lblProtocol.Text = "Protocol";
            // 
            // cBxProtocol
            // 
            this.cBxProtocol.FormattingEnabled = true;
            this.cBxProtocol.Location = new System.Drawing.Point(95, 65);
            this.cBxProtocol.Name = "cBxProtocol";
            this.cBxProtocol.Size = new System.Drawing.Size(62, 21);
            this.cBxProtocol.TabIndex = 2;
            // 
            // lblShareMode
            // 
            this.lblShareMode.AutoSize = true;
            this.lblShareMode.Location = new System.Drawing.Point(186, 68);
            this.lblShareMode.Name = "lblShareMode";
            this.lblShareMode.Size = new System.Drawing.Size(62, 13);
            this.lblShareMode.TabIndex = 19;
            this.lblShareMode.Text = "ShareMode";
            // 
            // cBxShareMode
            // 
            this.cBxShareMode.FormattingEnabled = true;
            this.cBxShareMode.Location = new System.Drawing.Point(255, 65);
            this.cBxShareMode.Name = "cBxShareMode";
            this.cBxShareMode.Size = new System.Drawing.Size(65, 21);
            this.cBxShareMode.TabIndex = 3;
            // 
            // btnDeconnecte
            // 
            this.btnDeconnecte.Location = new System.Drawing.Point(215, 101);
            this.btnDeconnecte.Name = "btnDeconnecte";
            this.btnDeconnecte.Size = new System.Drawing.Size(75, 23);
            this.btnDeconnecte.TabIndex = 23;
            this.btnDeconnecte.Text = "Deconnecte";
            this.btnDeconnecte.UseVisualStyleBackColor = true;
            this.btnDeconnecte.Click += new System.EventHandler(this.btnDeconnecte_Click);
            // 
            // lblVendor
            // 
            this.lblVendor.AutoSize = true;
            this.lblVendor.Location = new System.Drawing.Point(16, 163);
            this.lblVendor.Name = "lblVendor";
            this.lblVendor.Size = new System.Drawing.Size(25, 13);
            this.lblVendor.TabIndex = 26;
            this.lblVendor.Text = "AID";
            // 
            // txBxAID
            // 
            this.txBxAID.Location = new System.Drawing.Point(95, 164);
            this.txBxAID.Name = "txBxAID";
            this.txBxAID.ReadOnly = true;
            this.txBxAID.Size = new System.Drawing.Size(225, 20);
            this.txBxAID.TabIndex = 27;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 193);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 29;
            this.label3.Text = "appSN";
            // 
            // txBxappSN
            // 
            this.txBxappSN.Location = new System.Drawing.Point(95, 190);
            this.txBxappSN.Name = "txBxappSN";
            this.txBxappSN.ReadOnly = true;
            this.txBxappSN.Size = new System.Drawing.Size(225, 20);
            this.txBxappSN.TabIndex = 30;
            // 
            // btnQuitter
            // 
            this.btnQuitter.Location = new System.Drawing.Point(245, 216);
            this.btnQuitter.Name = "btnQuitter";
            this.btnQuitter.Size = new System.Drawing.Size(75, 23);
            this.btnQuitter.TabIndex = 32;
            this.btnQuitter.Text = "&Quitter";
            this.btnQuitter.UseVisualStyleBackColor = true;
            this.btnQuitter.Click += new System.EventHandler(this.btnQuitter_Click);
            // 
            // btnIso1443
            // 
            this.btnIso1443.Location = new System.Drawing.Point(19, 101);
            this.btnIso1443.Name = "btnIso1443";
            this.btnIso1443.Size = new System.Drawing.Size(75, 23);
            this.btnIso1443.TabIndex = 4;
            this.btnIso1443.Text = "iso1443";
            this.btnIso1443.UseVisualStyleBackColor = true;
            this.btnIso1443.Click += new System.EventHandler(this.btnIso1443_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 265);
            this.Controls.Add(this.btnIso1443);
            this.Controls.Add(this.btnQuitter);
            this.Controls.Add(this.txBxappSN);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txBxAID);
            this.Controls.Add(this.lblVendor);
            this.Controls.Add(this.btnDeconnecte);
            this.Controls.Add(this.cBxShareMode);
            this.Controls.Add(this.lblShareMode);
            this.Controls.Add(this.cBxProtocol);
            this.Controls.Add(this.lblProtocol);
            this.Controls.Add(this.lblSelectReader);
            this.Controls.Add(this.txBxATR);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbReaders);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Changement des clés 1 et 3";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ComboBox cbReaders;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblProtocol;
        private System.Windows.Forms.Label lblShareMode;
        private System.Windows.Forms.Button btnDeconnecte;
        private System.Windows.Forms.Label lblVendor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnQuitter;
        public System.Windows.Forms.TextBox txBxATR;
        public System.Windows.Forms.ComboBox cBxProtocol;
        public System.Windows.Forms.ComboBox cBxShareMode;
        public System.Windows.Forms.TextBox txBxAID;
        public System.Windows.Forms.TextBox txBxappSN;
        public System.Windows.Forms.Label lblSelectReader;
        private System.Windows.Forms.Button btnIso1443;
    }
}


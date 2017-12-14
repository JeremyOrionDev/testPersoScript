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
            this.components = new System.ComponentModel.Container();
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
            this.btnCreationStructure2 = new System.Windows.Forms.Button();
            this.lblProtocol = new System.Windows.Forms.Label();
            this.cBxProtocol = new System.Windows.Forms.ComboBox();
            this.lblShareMode = new System.Windows.Forms.Label();
            this.cBxShareMode = new System.Windows.Forms.ComboBox();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnDeconnecte = new System.Windows.Forms.Button();
            this.lblCardID = new System.Windows.Forms.Label();
            this.txBxCardID = new System.Windows.Forms.TextBox();
            this.lblVendor = new System.Windows.Forms.Label();
            this.txBxVendor = new System.Windows.Forms.TextBox();
            this.panelConnexion = new System.Windows.Forms.Panel();
            this.rondVert = new System.Windows.Forms.PictureBox();
            this.lblEtatConnexion = new System.Windows.Forms.Label();
            this.rond_rouge = new System.Windows.Forms.PictureBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.panelConnexion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rondVert)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rond_rouge)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
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
            this.cbReaders.Size = new System.Drawing.Size(195, 21);
            this.cbReaders.TabIndex = 3;
            this.cbReaders.SelectedIndexChanged += new System.EventHandler(this.cbReaders_SelectedIndexChanged);
            // 
            // txBxReponse
            // 
            this.txBxReponse.Location = new System.Drawing.Point(115, 220);
            this.txBxReponse.Multiline = true;
            this.txBxReponse.Name = "txBxReponse";
            this.txBxReponse.Size = new System.Drawing.Size(191, 24);
            this.txBxReponse.TabIndex = 10;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(115, 101);
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
            this.label2.Location = new System.Drawing.Point(46, 141);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Card ATR";
            // 
            // txBxATR
            // 
            this.txBxATR.Location = new System.Drawing.Point(115, 138);
            this.txBxATR.Name = "txBxATR";
            this.txBxATR.ReadOnly = true;
            this.txBxATR.Size = new System.Drawing.Size(191, 20);
            this.txBxATR.TabIndex = 13;
            // 
            // btnDF
            // 
            this.btnDF.Location = new System.Drawing.Point(49, 221);
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
            this.button2.Location = new System.Drawing.Point(346, 179);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 16;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnGetMemory
            // 
            this.btnGetMemory.Location = new System.Drawing.Point(49, 269);
            this.btnGetMemory.Name = "btnGetMemory";
            this.btnGetMemory.Size = new System.Drawing.Size(75, 23);
            this.btnGetMemory.TabIndex = 17;
            this.btnGetMemory.Text = "Get Memory";
            this.btnGetMemory.UseVisualStyleBackColor = true;
            this.btnGetMemory.Click += new System.EventHandler(this.btn_Get_Memory_Click);
            // 
            // btnCreationStructure2
            // 
            this.btnCreationStructure2.Location = new System.Drawing.Point(49, 298);
            this.btnCreationStructure2.Name = "btnCreationStructure2";
            this.btnCreationStructure2.Size = new System.Drawing.Size(75, 23);
            this.btnCreationStructure2.TabIndex = 18;
            this.btnCreationStructure2.Text = "Structure 2";
            this.btnCreationStructure2.UseVisualStyleBackColor = true;
            this.btnCreationStructure2.Click += new System.EventHandler(this.btnCreationStructure2_Click);
            // 
            // lblProtocol
            // 
            this.lblProtocol.AutoSize = true;
            this.lblProtocol.Location = new System.Drawing.Point(46, 68);
            this.lblProtocol.Name = "lblProtocol";
            this.lblProtocol.Size = new System.Drawing.Size(46, 13);
            this.lblProtocol.TabIndex = 19;
            this.lblProtocol.Text = "Protocol";
            // 
            // cBxProtocol
            // 
            this.cBxProtocol.FormattingEnabled = true;
            this.cBxProtocol.Location = new System.Drawing.Point(115, 65);
            this.cBxProtocol.Name = "cBxProtocol";
            this.cBxProtocol.Size = new System.Drawing.Size(62, 21);
            this.cBxProtocol.TabIndex = 20;
            this.cBxProtocol.SelectedIndexChanged += new System.EventHandler(this.cBxProtocol_SelectedIndexChanged);
            // 
            // lblShareMode
            // 
            this.lblShareMode.AutoSize = true;
            this.lblShareMode.Location = new System.Drawing.Point(183, 68);
            this.lblShareMode.Name = "lblShareMode";
            this.lblShareMode.Size = new System.Drawing.Size(62, 13);
            this.lblShareMode.TabIndex = 19;
            this.lblShareMode.Text = "ShareMode";
            // 
            // cBxShareMode
            // 
            this.cBxShareMode.FormattingEnabled = true;
            this.cBxShareMode.Location = new System.Drawing.Point(251, 65);
            this.cBxShareMode.Name = "cBxShareMode";
            this.cBxShareMode.Size = new System.Drawing.Size(59, 21);
            this.cBxShareMode.TabIndex = 21;
            this.cBxShareMode.SelectedIndexChanged += new System.EventHandler(this.cBxShareMode_SelectedIndexChanged);
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(37, 4);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(78, 23);
            this.btnCopy.TabIndex = 22;
            this.btnCopy.Text = "Copier";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnDeconnecte
            // 
            this.btnDeconnecte.Location = new System.Drawing.Point(235, 101);
            this.btnDeconnecte.Name = "btnDeconnecte";
            this.btnDeconnecte.Size = new System.Drawing.Size(75, 23);
            this.btnDeconnecte.TabIndex = 23;
            this.btnDeconnecte.Text = "Deconnecte";
            this.btnDeconnecte.UseVisualStyleBackColor = true;
            this.btnDeconnecte.Click += new System.EventHandler(this.btnDeconnecte_Click);
            // 
            // lblCardID
            // 
            this.lblCardID.AutoSize = true;
            this.lblCardID.Location = new System.Drawing.Point(46, 164);
            this.lblCardID.Name = "lblCardID";
            this.lblCardID.Size = new System.Drawing.Size(43, 13);
            this.lblCardID.TabIndex = 24;
            this.lblCardID.Text = "Card ID";
            // 
            // txBxCardID
            // 
            this.txBxCardID.Location = new System.Drawing.Point(115, 164);
            this.txBxCardID.Name = "txBxCardID";
            this.txBxCardID.ReadOnly = true;
            this.txBxCardID.Size = new System.Drawing.Size(191, 20);
            this.txBxCardID.TabIndex = 25;
            // 
            // lblVendor
            // 
            this.lblVendor.AutoSize = true;
            this.lblVendor.Location = new System.Drawing.Point(46, 189);
            this.lblVendor.Name = "lblVendor";
            this.lblVendor.Size = new System.Drawing.Size(41, 13);
            this.lblVendor.TabIndex = 26;
            this.lblVendor.Text = "Vendor";
            // 
            // txBxVendor
            // 
            this.txBxVendor.Location = new System.Drawing.Point(115, 190);
            this.txBxVendor.Name = "txBxVendor";
            this.txBxVendor.ReadOnly = true;
            this.txBxVendor.Size = new System.Drawing.Size(191, 20);
            this.txBxVendor.TabIndex = 27;
            // 
            // panelConnexion
            // 
            this.panelConnexion.Controls.Add(this.rond_rouge);
            this.panelConnexion.Controls.Add(this.lblEtatConnexion);
            this.panelConnexion.Controls.Add(this.rondVert);
            this.panelConnexion.Location = new System.Drawing.Point(316, 36);
            this.panelConnexion.Name = "panelConnexion";
            this.panelConnexion.Size = new System.Drawing.Size(172, 100);
            this.panelConnexion.TabIndex = 28;
            // 
            // rondVert
            // 
            this.rondVert.Location = new System.Drawing.Point(19, 28);
            this.rondVert.Name = "rondVert";
            this.rondVert.Size = new System.Drawing.Size(60, 60);
            this.rondVert.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.rondVert.TabIndex = 0;
            this.rondVert.TabStop = false;
            this.rondVert.Visible = false;
            // 
            // lblEtatConnexion
            // 
            this.lblEtatConnexion.AutoSize = true;
            this.lblEtatConnexion.Location = new System.Drawing.Point(16, 8);
            this.lblEtatConnexion.Name = "lblEtatConnexion";
            this.lblEtatConnexion.Size = new System.Drawing.Size(78, 13);
            this.lblEtatConnexion.TabIndex = 1;
            this.lblEtatConnexion.Text = "etat Connexion";
            // 
            // rond_rouge
            // 
            this.rond_rouge.Location = new System.Drawing.Point(85, 28);
            this.rond_rouge.Name = "rond_rouge";
            this.rond_rouge.Size = new System.Drawing.Size(60, 60);
            this.rond_rouge.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.rond_rouge.TabIndex = 2;
            this.rond_rouge.TabStop = false;
            this.rond_rouge.Visible = false;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(492, 397);
            this.Controls.Add(this.panelConnexion);
            this.Controls.Add(this.txBxVendor);
            this.Controls.Add(this.lblVendor);
            this.Controls.Add(this.txBxCardID);
            this.Controls.Add(this.lblCardID);
            this.Controls.Add(this.btnDeconnecte);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.cBxShareMode);
            this.Controls.Add(this.lblShareMode);
            this.Controls.Add(this.cBxProtocol);
            this.Controls.Add(this.lblProtocol);
            this.Controls.Add(this.btnCreationStructure2);
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
            this.panelConnexion.ResumeLayout(false);
            this.panelConnexion.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rondVert)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rond_rouge)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
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
        private System.Windows.Forms.Button btnCreationStructure2;
        private System.Windows.Forms.Label lblProtocol;
        private System.Windows.Forms.ComboBox cBxProtocol;
        private System.Windows.Forms.Label lblShareMode;
        private System.Windows.Forms.ComboBox cBxShareMode;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnDeconnecte;
        private System.Windows.Forms.Label lblCardID;
        private System.Windows.Forms.TextBox txBxCardID;
        private System.Windows.Forms.Label lblVendor;
        private System.Windows.Forms.TextBox txBxVendor;
        private System.Windows.Forms.Panel panelConnexion;
        private System.Windows.Forms.PictureBox rondVert;
        private System.Windows.Forms.PictureBox rond_rouge;
        private System.Windows.Forms.Label lblEtatConnexion;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}


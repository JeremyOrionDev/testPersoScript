namespace Desfire
{
    partial class formDesfire
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
            this.cbReadersDesfire = new System.Windows.Forms.ComboBox();
            this.btnDesfireConnect = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tBxATRDesfire = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tBxDesfireVersion = new System.Windows.Forms.TextBox();
            this.btnDesfireAuthAES = new System.Windows.Forms.Button();
            this.tBxDesfireCle = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tBxDesfireResultat = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.cBxDesfireKeyNb = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tBxVendor = new System.Windows.Forms.TextBox();
            this.tBxAnnee = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Liste lecteurs";
            // 
            // cbReadersDesfire
            // 
            this.cbReadersDesfire.FormattingEnabled = true;
            this.cbReadersDesfire.Location = new System.Drawing.Point(91, 25);
            this.cbReadersDesfire.Name = "cbReadersDesfire";
            this.cbReadersDesfire.Size = new System.Drawing.Size(205, 21);
            this.cbReadersDesfire.TabIndex = 10;
            // 
            // btnDesfireConnect
            // 
            this.btnDesfireConnect.Location = new System.Drawing.Point(15, 52);
            this.btnDesfireConnect.Name = "btnDesfireConnect";
            this.btnDesfireConnect.Size = new System.Drawing.Size(79, 30);
            this.btnDesfireConnect.TabIndex = 12;
            this.btnDesfireConnect.Text = "Connecte";
            this.btnDesfireConnect.UseVisualStyleBackColor = true;
            this.btnDesfireConnect.Click += new System.EventHandler(this.btnDesfireConnect_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "ATR";
            // 
            // tBxATRDesfire
            // 
            this.tBxATRDesfire.Enabled = false;
            this.tBxATRDesfire.Location = new System.Drawing.Point(91, 103);
            this.tBxATRDesfire.Name = "tBxATRDesfire";
            this.tBxATRDesfire.ReadOnly = true;
            this.tBxATRDesfire.Size = new System.Drawing.Size(205, 20);
            this.tBxATRDesfire.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 140);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Version";
            // 
            // tBxDesfireVersion
            // 
            this.tBxDesfireVersion.Location = new System.Drawing.Point(91, 140);
            this.tBxDesfireVersion.Name = "tBxDesfireVersion";
            this.tBxDesfireVersion.ReadOnly = true;
            this.tBxDesfireVersion.Size = new System.Drawing.Size(27, 20);
            this.tBxDesfireVersion.TabIndex = 17;
            // 
            // btnDesfireAuthAES
            // 
            this.btnDesfireAuthAES.Location = new System.Drawing.Point(96, 298);
            this.btnDesfireAuthAES.Name = "btnDesfireAuthAES";
            this.btnDesfireAuthAES.Size = new System.Drawing.Size(75, 23);
            this.btnDesfireAuthAES.TabIndex = 18;
            this.btnDesfireAuthAES.Text = "Auth AES";
            this.btnDesfireAuthAES.UseVisualStyleBackColor = true;
            this.btnDesfireAuthAES.Click += new System.EventHandler(this.btnDesfireAuthAES_Click);
            // 
            // tBxDesfireCle
            // 
            this.tBxDesfireCle.Location = new System.Drawing.Point(91, 272);
            this.tBxDesfireCle.Name = "tBxDesfireCle";
            this.tBxDesfireCle.Size = new System.Drawing.Size(200, 20);
            this.tBxDesfireCle.TabIndex = 19;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 279);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(22, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "Clé";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 328);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "Resultat";
            // 
            // tBxDesfireResultat
            // 
            this.tBxDesfireResultat.Location = new System.Drawing.Point(96, 328);
            this.tBxDesfireResultat.Multiline = true;
            this.tBxDesfireResultat.Name = "tBxDesfireResultat";
            this.tBxDesfireResultat.Size = new System.Drawing.Size(200, 20);
            this.tBxDesfireResultat.TabIndex = 22;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(223, 299);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(73, 23);
            this.button2.TabIndex = 23;
            this.button2.Text = "Auth Natif";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // cBxDesfireKeyNb
            // 
            this.cBxDesfireKeyNb.FormattingEnabled = true;
            this.cBxDesfireKeyNb.Location = new System.Drawing.Point(40, 272);
            this.cBxDesfireKeyNb.Name = "cBxDesfireKeyNb";
            this.cBxDesfireKeyNb.Size = new System.Drawing.Size(42, 21);
            this.cBxDesfireKeyNb.TabIndex = 24;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 166);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 27;
            this.label7.Text = "Année";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(136, 143);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 13);
            this.label8.TabIndex = 29;
            this.label8.Text = "Vendor";
            // 
            // tBxVendor
            // 
            this.tBxVendor.Location = new System.Drawing.Point(191, 140);
            this.tBxVendor.Name = "tBxVendor";
            this.tBxVendor.ReadOnly = true;
            this.tBxVendor.Size = new System.Drawing.Size(105, 20);
            this.tBxVendor.TabIndex = 30;
            // 
            // tBxAnnee
            // 
            this.tBxAnnee.Location = new System.Drawing.Point(91, 166);
            this.tBxAnnee.Name = "tBxAnnee";
            this.tBxAnnee.ReadOnly = true;
            this.tBxAnnee.Size = new System.Drawing.Size(75, 20);
            this.tBxAnnee.TabIndex = 31;
            // 
            // formDesfire
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 366);
            this.Controls.Add(this.tBxAnnee);
            this.Controls.Add(this.tBxVendor);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cBxDesfireKeyNb);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.tBxDesfireResultat);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tBxDesfireCle);
            this.Controls.Add(this.btnDesfireAuthAES);
            this.Controls.Add(this.tBxDesfireVersion);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tBxATRDesfire);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnDesfireConnect);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbReadersDesfire);
            this.Name = "formDesfire";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ComboBox cbReadersDesfire;
        private System.Windows.Forms.Button btnDesfireConnect;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tBxATRDesfire;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tBxDesfireVersion;
        private System.Windows.Forms.Button btnDesfireAuthAES;
        private System.Windows.Forms.TextBox tBxDesfireCle;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tBxDesfireResultat;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox cBxDesfireKeyNb;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tBxVendor;
        private System.Windows.Forms.TextBox tBxAnnee;
    }
}


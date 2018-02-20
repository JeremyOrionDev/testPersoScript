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
            this.lblUID = new System.Windows.Forms.Label();
            this.tBxATRDesfire = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tBxVendor = new System.Windows.Forms.TextBox();
            this.btnDesfireAuthAES = new System.Windows.Forms.Button();
            this.tBxDesfireCle = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.cBxDesfireKeyNb = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tBxDesfireVersion = new System.Windows.Forms.TextBox();
            this.tBxAnnee = new System.Windows.Forms.TextBox();
            this.greenBtnConnect = new System.Windows.Forms.PictureBox();
            this.redBtnConnect = new System.Windows.Forms.PictureBox();
            this.greenBtnAuth = new System.Windows.Forms.PictureBox();
            this.redBtnAuth = new System.Windows.Forms.PictureBox();
            this.chek = new System.Windows.Forms.Label();
            this.gBxInfo = new System.Windows.Forms.GroupBox();
            this.tBxDFName = new System.Windows.Forms.TextBox();
            this.tBxISOID = new System.Windows.Forms.TextBox();
            this.cBxISO = new System.Windows.Forms.CheckBox();
            this.tBxKS2 = new System.Windows.Forms.TextBox();
            this.lblKS2 = new System.Windows.Forms.Label();
            this.tBxKS1 = new System.Windows.Forms.TextBox();
            this.lblKS1 = new System.Windows.Forms.Label();
            this.tBxAID = new System.Windows.Forms.TextBox();
            this.lblAID = new System.Windows.Forms.Label();
            this.btnCreateApllication = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.gBxConnect = new System.Windows.Forms.GroupBox();
            this.flowCreate = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panelISO = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tBxRetourCreate = new System.Windows.Forms.TextBox();
            this.lblRetourCreate = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.greenBtnConnect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.redBtnConnect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.greenBtnAuth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.redBtnAuth)).BeginInit();
            this.gBxInfo.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.gBxConnect.SuspendLayout();
            this.flowCreate.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panelISO.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Liste lecteurs";
            // 
            // cbReadersDesfire
            // 
            this.cbReadersDesfire.FormattingEnabled = true;
            this.cbReadersDesfire.Location = new System.Drawing.Point(86, 17);
            this.cbReadersDesfire.Name = "cbReadersDesfire";
            this.cbReadersDesfire.Size = new System.Drawing.Size(205, 21);
            this.cbReadersDesfire.TabIndex = 1;
            // 
            // btnDesfireConnect
            // 
            this.btnDesfireConnect.Location = new System.Drawing.Point(5, 44);
            this.btnDesfireConnect.Name = "btnDesfireConnect";
            this.btnDesfireConnect.Size = new System.Drawing.Size(79, 30);
            this.btnDesfireConnect.TabIndex = 2;
            this.btnDesfireConnect.Text = "Connecte";
            this.btnDesfireConnect.UseVisualStyleBackColor = true;
            this.btnDesfireConnect.Click += new System.EventHandler(this.btnDesfireConnect_Click);
            // 
            // lblUID
            // 
            this.lblUID.AutoSize = true;
            this.lblUID.Location = new System.Drawing.Point(14, 19);
            this.lblUID.Name = "lblUID";
            this.lblUID.Size = new System.Drawing.Size(26, 13);
            this.lblUID.TabIndex = 14;
            this.lblUID.Text = "UID";
            // 
            // tBxATRDesfire
            // 
            this.tBxATRDesfire.Enabled = false;
            this.tBxATRDesfire.Location = new System.Drawing.Point(93, 19);
            this.tBxATRDesfire.Name = "tBxATRDesfire";
            this.tBxATRDesfire.ReadOnly = true;
            this.tBxATRDesfire.Size = new System.Drawing.Size(205, 20);
            this.tBxATRDesfire.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(133, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Version";
            // 
            // tBxVendor
            // 
            this.tBxVendor.Location = new System.Drawing.Point(93, 56);
            this.tBxVendor.Name = "tBxVendor";
            this.tBxVendor.ReadOnly = true;
            this.tBxVendor.Size = new System.Drawing.Size(27, 20);
            this.tBxVendor.TabIndex = 17;
            // 
            // btnDesfireAuthAES
            // 
            this.btnDesfireAuthAES.Location = new System.Drawing.Point(86, 112);
            this.btnDesfireAuthAES.Name = "btnDesfireAuthAES";
            this.btnDesfireAuthAES.Size = new System.Drawing.Size(63, 23);
            this.btnDesfireAuthAES.TabIndex = 5;
            this.btnDesfireAuthAES.Text = "Auth AES";
            this.btnDesfireAuthAES.UseVisualStyleBackColor = true;
            this.btnDesfireAuthAES.Click += new System.EventHandler(this.btnDesfireAuthAES_Click);
            // 
            // tBxDesfireCle
            // 
            this.tBxDesfireCle.Location = new System.Drawing.Point(86, 86);
            this.tBxDesfireCle.Name = "tBxDesfireCle";
            this.tBxDesfireCle.Size = new System.Drawing.Size(200, 20);
            this.tBxDesfireCle.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(2, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(22, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "Clé";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(213, 113);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(73, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "Auth Natif";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // cBxDesfireKeyNb
            // 
            this.cBxDesfireKeyNb.FormattingEnabled = true;
            this.cBxDesfireKeyNb.Location = new System.Drawing.Point(30, 86);
            this.cBxDesfireKeyNb.Name = "cBxDesfireKeyNb";
            this.cBxDesfireKeyNb.Size = new System.Drawing.Size(42, 21);
            this.cBxDesfireKeyNb.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 82);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 27;
            this.label7.Text = "Année";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 56);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 13);
            this.label8.TabIndex = 29;
            this.label8.Text = "Vendor";
            // 
            // tBxDesfireVersion
            // 
            this.tBxDesfireVersion.Location = new System.Drawing.Point(193, 56);
            this.tBxDesfireVersion.Name = "tBxDesfireVersion";
            this.tBxDesfireVersion.ReadOnly = true;
            this.tBxDesfireVersion.Size = new System.Drawing.Size(105, 20);
            this.tBxDesfireVersion.TabIndex = 30;
            // 
            // tBxAnnee
            // 
            this.tBxAnnee.Location = new System.Drawing.Point(93, 82);
            this.tBxAnnee.Name = "tBxAnnee";
            this.tBxAnnee.ReadOnly = true;
            this.tBxAnnee.Size = new System.Drawing.Size(75, 20);
            this.tBxAnnee.TabIndex = 31;
            // 
            // greenBtnConnect
            // 
            this.greenBtnConnect.ImageLocation = "";
            this.greenBtnConnect.Location = new System.Drawing.Point(90, 44);
            this.greenBtnConnect.Name = "greenBtnConnect";
            this.greenBtnConnect.Size = new System.Drawing.Size(27, 25);
            this.greenBtnConnect.TabIndex = 32;
            this.greenBtnConnect.TabStop = false;
            this.greenBtnConnect.Visible = false;
            // 
            // redBtnConnect
            // 
            this.redBtnConnect.Location = new System.Drawing.Point(123, 44);
            this.redBtnConnect.Name = "redBtnConnect";
            this.redBtnConnect.Size = new System.Drawing.Size(26, 25);
            this.redBtnConnect.TabIndex = 33;
            this.redBtnConnect.TabStop = false;
            this.redBtnConnect.Visible = false;
            // 
            // greenBtnAuth
            // 
            this.greenBtnAuth.ImageLocation = "";
            this.greenBtnAuth.Location = new System.Drawing.Point(158, 112);
            this.greenBtnAuth.Name = "greenBtnAuth";
            this.greenBtnAuth.Size = new System.Drawing.Size(22, 24);
            this.greenBtnAuth.TabIndex = 32;
            this.greenBtnAuth.TabStop = false;
            this.greenBtnAuth.Visible = false;
            // 
            // redBtnAuth
            // 
            this.redBtnAuth.Location = new System.Drawing.Point(186, 112);
            this.redBtnAuth.Name = "redBtnAuth";
            this.redBtnAuth.Size = new System.Drawing.Size(22, 24);
            this.redBtnAuth.TabIndex = 33;
            this.redBtnAuth.TabStop = false;
            this.redBtnAuth.Visible = false;
            // 
            // chek
            // 
            this.chek.AutoSize = true;
            this.chek.Location = new System.Drawing.Point(167, 118);
            this.chek.Name = "chek";
            this.chek.Size = new System.Drawing.Size(0, 13);
            this.chek.TabIndex = 34;
            // 
            // gBxInfo
            // 
            this.gBxInfo.Controls.Add(this.tBxATRDesfire);
            this.gBxInfo.Controls.Add(this.lblUID);
            this.gBxInfo.Controls.Add(this.label3);
            this.gBxInfo.Controls.Add(this.tBxVendor);
            this.gBxInfo.Controls.Add(this.label7);
            this.gBxInfo.Controls.Add(this.label8);
            this.gBxInfo.Controls.Add(this.tBxAnnee);
            this.gBxInfo.Controls.Add(this.tBxDesfireVersion);
            this.gBxInfo.Location = new System.Drawing.Point(3, 158);
            this.gBxInfo.Name = "gBxInfo";
            this.gBxInfo.Size = new System.Drawing.Size(309, 107);
            this.gBxInfo.TabIndex = 35;
            this.gBxInfo.TabStop = false;
            this.gBxInfo.Text = "Info carte";
            // 
            // tBxDFName
            // 
            this.tBxDFName.Location = new System.Drawing.Point(55, 32);
            this.tBxDFName.Name = "tBxDFName";
            this.tBxDFName.Size = new System.Drawing.Size(170, 20);
            this.tBxDFName.TabIndex = 12;
            // 
            // tBxISOID
            // 
            this.tBxISOID.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tBxISOID.Location = new System.Drawing.Point(55, 5);
            this.tBxISOID.Name = "tBxISOID";
            this.tBxISOID.Size = new System.Drawing.Size(64, 20);
            this.tBxISOID.TabIndex = 11;
            // 
            // cBxISO
            // 
            this.cBxISO.AutoSize = true;
            this.cBxISO.Location = new System.Drawing.Point(119, 6);
            this.cBxISO.Name = "cBxISO";
            this.cBxISO.Size = new System.Drawing.Size(44, 17);
            this.cBxISO.TabIndex = 8;
            this.cBxISO.Text = "ISO";
            this.cBxISO.UseVisualStyleBackColor = true;
            this.cBxISO.CheckedChanged += new System.EventHandler(this.cBxISO_CheckedChanged);
            // 
            // tBxKS2
            // 
            this.tBxKS2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tBxKS2.Location = new System.Drawing.Point(190, 3);
            this.tBxKS2.Name = "tBxKS2";
            this.tBxKS2.Size = new System.Drawing.Size(64, 20);
            this.tBxKS2.TabIndex = 10;
            // 
            // lblKS2
            // 
            this.lblKS2.AutoSize = true;
            this.lblKS2.Location = new System.Drawing.Point(130, 6);
            this.lblKS2.Name = "lblKS2";
            this.lblKS2.Size = new System.Drawing.Size(46, 13);
            this.lblKS2.TabIndex = 5;
            this.lblKS2.Text = "Param 2";
            // 
            // tBxKS1
            // 
            this.tBxKS1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tBxKS1.Location = new System.Drawing.Point(55, 3);
            this.tBxKS1.Name = "tBxKS1";
            this.tBxKS1.Size = new System.Drawing.Size(57, 20);
            this.tBxKS1.TabIndex = 9;
            // 
            // lblKS1
            // 
            this.lblKS1.AutoSize = true;
            this.lblKS1.Location = new System.Drawing.Point(3, 6);
            this.lblKS1.Name = "lblKS1";
            this.lblKS1.Size = new System.Drawing.Size(46, 13);
            this.lblKS1.TabIndex = 3;
            this.lblKS1.Text = "Param 1";
            // 
            // tBxAID
            // 
            this.tBxAID.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tBxAID.Location = new System.Drawing.Point(52, 3);
            this.tBxAID.Name = "tBxAID";
            this.tBxAID.Size = new System.Drawing.Size(60, 20);
            this.tBxAID.TabIndex = 7;
            this.tBxAID.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblAID
            // 
            this.lblAID.AutoSize = true;
            this.lblAID.Location = new System.Drawing.Point(3, 6);
            this.lblAID.Name = "lblAID";
            this.lblAID.Size = new System.Drawing.Size(25, 13);
            this.lblAID.TabIndex = 1;
            this.lblAID.Text = "AID";
            // 
            // btnCreateApllication
            // 
            this.btnCreateApllication.Location = new System.Drawing.Point(225, 4);
            this.btnCreateApllication.Name = "btnCreateApllication";
            this.btnCreateApllication.Size = new System.Drawing.Size(75, 23);
            this.btnCreateApllication.TabIndex = 13;
            this.btnCreateApllication.Text = "Créer";
            this.btnCreateApllication.UseVisualStyleBackColor = true;
            this.btnCreateApllication.Click += new System.EventHandler(this.btnCreateApllication_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.gBxConnect);
            this.flowLayoutPanel1.Controls.Add(this.gBxInfo);
            this.flowLayoutPanel1.Controls.Add(this.flowCreate);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(318, 443);
            this.flowLayoutPanel1.TabIndex = 37;
            // 
            // gBxConnect
            // 
            this.gBxConnect.Controls.Add(this.tBxDesfireCle);
            this.gBxConnect.Controls.Add(this.cbReadersDesfire);
            this.gBxConnect.Controls.Add(this.label1);
            this.gBxConnect.Controls.Add(this.chek);
            this.gBxConnect.Controls.Add(this.btnDesfireConnect);
            this.gBxConnect.Controls.Add(this.redBtnAuth);
            this.gBxConnect.Controls.Add(this.btnDesfireAuthAES);
            this.gBxConnect.Controls.Add(this.redBtnConnect);
            this.gBxConnect.Controls.Add(this.label4);
            this.gBxConnect.Controls.Add(this.greenBtnAuth);
            this.gBxConnect.Controls.Add(this.button2);
            this.gBxConnect.Controls.Add(this.greenBtnConnect);
            this.gBxConnect.Controls.Add(this.cBxDesfireKeyNb);
            this.gBxConnect.Location = new System.Drawing.Point(3, 3);
            this.gBxConnect.Name = "gBxConnect";
            this.gBxConnect.Size = new System.Drawing.Size(309, 149);
            this.gBxConnect.TabIndex = 0;
            this.gBxConnect.TabStop = false;
            // 
            // flowCreate
            // 
            this.flowCreate.AutoSize = true;
            this.flowCreate.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowCreate.Controls.Add(this.panel1);
            this.flowCreate.Controls.Add(this.panel2);
            this.flowCreate.Controls.Add(this.panelISO);
            this.flowCreate.Controls.Add(this.panel3);
            this.flowCreate.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowCreate.Location = new System.Drawing.Point(3, 271);
            this.flowCreate.Name = "flowCreate";
            this.flowCreate.Size = new System.Drawing.Size(312, 169);
            this.flowCreate.TabIndex = 37;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tBxAID);
            this.panel1.Controls.Add(this.lblAID);
            this.panel1.Controls.Add(this.cBxISO);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(306, 27);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tBxKS1);
            this.panel2.Controls.Add(this.lblKS1);
            this.panel2.Controls.Add(this.lblKS2);
            this.panel2.Controls.Add(this.tBxKS2);
            this.panel2.Location = new System.Drawing.Point(3, 36);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(306, 28);
            this.panel2.TabIndex = 1;
            // 
            // panelISO
            // 
            this.panelISO.Controls.Add(this.label6);
            this.panelISO.Controls.Add(this.label5);
            this.panelISO.Controls.Add(this.tBxDFName);
            this.panelISO.Controls.Add(this.tBxISOID);
            this.panelISO.Location = new System.Drawing.Point(3, 70);
            this.panelISO.Name = "panelISO";
            this.panelISO.Size = new System.Drawing.Size(306, 58);
            this.panelISO.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 35);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Iso DF";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Iso ID";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.tBxRetourCreate);
            this.panel3.Controls.Add(this.lblRetourCreate);
            this.panel3.Controls.Add(this.btnCreateApllication);
            this.panel3.Location = new System.Drawing.Point(3, 134);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(306, 32);
            this.panel3.TabIndex = 38;
            // 
            // tBxRetourCreate
            // 
            this.tBxRetourCreate.Enabled = false;
            this.tBxRetourCreate.Location = new System.Drawing.Point(55, 4);
            this.tBxRetourCreate.Name = "tBxRetourCreate";
            this.tBxRetourCreate.Size = new System.Drawing.Size(164, 20);
            this.tBxRetourCreate.TabIndex = 12;
            this.tBxRetourCreate.TabStop = false;
            // 
            // lblRetourCreate
            // 
            this.lblRetourCreate.AutoSize = true;
            this.lblRetourCreate.Location = new System.Drawing.Point(3, 4);
            this.lblRetourCreate.Name = "lblRetourCreate";
            this.lblRetourCreate.Size = new System.Drawing.Size(39, 13);
            this.lblRetourCreate.TabIndex = 11;
            this.lblRetourCreate.Text = "Retour";
            // 
            // formDesfire
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(340, 477);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "formDesfire";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.greenBtnConnect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.redBtnConnect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.greenBtnAuth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.redBtnAuth)).EndInit();
            this.gBxInfo.ResumeLayout(false);
            this.gBxInfo.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.gBxConnect.ResumeLayout(false);
            this.gBxConnect.PerformLayout();
            this.flowCreate.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panelISO.ResumeLayout(false);
            this.panelISO.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ComboBox cbReadersDesfire;
        private System.Windows.Forms.Button btnDesfireConnect;
        private System.Windows.Forms.Label lblUID;
        private System.Windows.Forms.TextBox tBxATRDesfire;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tBxVendor;
        private System.Windows.Forms.Button btnDesfireAuthAES;
        private System.Windows.Forms.TextBox tBxDesfireCle;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox cBxDesfireKeyNb;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tBxDesfireVersion;
        private System.Windows.Forms.TextBox tBxAnnee;
        private System.Windows.Forms.PictureBox greenBtnConnect;
        private System.Windows.Forms.PictureBox redBtnConnect;
        private System.Windows.Forms.PictureBox greenBtnAuth;
        private System.Windows.Forms.PictureBox redBtnAuth;
        private System.Windows.Forms.Label chek;
        private System.Windows.Forms.GroupBox gBxInfo;
        private System.Windows.Forms.TextBox tBxDFName;
        private System.Windows.Forms.TextBox tBxISOID;
        private System.Windows.Forms.CheckBox cBxISO;
        private System.Windows.Forms.TextBox tBxKS2;
        private System.Windows.Forms.Label lblKS2;
        private System.Windows.Forms.TextBox tBxKS1;
        private System.Windows.Forms.Label lblKS1;
        private System.Windows.Forms.TextBox tBxAID;
        private System.Windows.Forms.Label lblAID;
        private System.Windows.Forms.Button btnCreateApllication;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.GroupBox gBxConnect;
        private System.Windows.Forms.TextBox tBxRetourCreate;
        private System.Windows.Forms.Label lblRetourCreate;
        private System.Windows.Forms.FlowLayoutPanel flowCreate;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panelISO;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel3;
    }
}


namespace MifareReadWrite
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
            this.btnTrouverLecteur = new System.Windows.Forms.Button();
            this.cBxLecteur = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnTrouverLecteur
            // 
            this.btnTrouverLecteur.AutoSize = true;
            this.btnTrouverLecteur.Location = new System.Drawing.Point(12, 12);
            this.btnTrouverLecteur.Name = "btnTrouverLecteur";
            this.btnTrouverLecteur.Size = new System.Drawing.Size(93, 23);
            this.btnTrouverLecteur.TabIndex = 0;
            this.btnTrouverLecteur.Text = "Trouver Lecteur";
            this.btnTrouverLecteur.UseVisualStyleBackColor = true;
            this.btnTrouverLecteur.Click += new System.EventHandler(this.btnTrouverLecteur_Click);
            // 
            // cBxLecteur
            // 
            this.cBxLecteur.FormattingEnabled = true;
            this.cBxLecteur.Location = new System.Drawing.Point(121, 14);
            this.cBxLecteur.Name = "cBxLecteur";
            this.cBxLecteur.Size = new System.Drawing.Size(121, 21);
            this.cBxLecteur.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.cBxLecteur);
            this.Controls.Add(this.btnTrouverLecteur);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnTrouverLecteur;
        private System.Windows.Forms.ComboBox cBxLecteur;
    }
}


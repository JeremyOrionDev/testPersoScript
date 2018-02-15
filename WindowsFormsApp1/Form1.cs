using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PCSC;
using PCSC.Iso7816;
using System.Runtime.InteropServices;




namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
            //Etablissement du context et creation de la liste des lecteur
            controleurPcsc.establishContext();
            controleurPcsc.readerList(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*appel de la méthode readerConnect qui se connecte au lecteur choisi en comboBox
             * et avec les paramètres sélectionnés pour le mode de connexion et récupère l'ATR*/
            controleurPcsc.readerConnect(this);
        }


        /// <summary>
        /// Méthode appelée lorsqu'un lecteur à été selectionné dans la comboBox et effaçant le message demandant 
        /// de sélectionner un lecteur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbReaders_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblSelectReader.Visible = false;
        }

        /// <summary>
        /// Méthode de déconnexion du lecteur appelée au clic sur le bouton Déconnecte
        /// Vide la texte box affichant l'ATR
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeconnecte_Click(object sender, EventArgs e)
        {
            controleurPcsc.readerDisconnect();
            txBxATR.Clear();
        }

        /// <summary>
        /// Méthode appelée lors du clic sur le bouton de chargement des clés
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoadKey_Click(object sender, EventArgs e)
        {
            //Etablissement du context pour les connexions et transmissions de commandes
            var contextFactory = ContextFactory.Instance;

            //Récupération du nom du lecteur sélectionné
            var reader = cbReaders.SelectedText;

            //utilisation du context et création de la connexion en utilisant le scope système
            using (var context = contextFactory.Establish(SCardScope.System))
            {

                //Création du lecteur pour les envoi de commandes
                SCardReader readerContact;

                //Utilisation du lecteur avec le context établit plus haut
                using (readerContact = new SCardReader(context))
                {
                    /*Connexion sur le lecteur sélectionné, utilisation des paramètres fixe car utilisation d'un modèle unique de carte*/
                    var sc = readerContact.Connect(cbReaders.SelectedItem.ToString(),SCardShareMode.Exclusive, SCardProtocol.T1);

                    /*création de la commande de récupération d'un fichier dont l'iD est connue
                     * utilisation du cas 4 car envoi et réception de données*/
                    var getFile = new CommandApdu(IsoCase.Case4Extended, readerContact.ActiveProtocol)
                    {                        
                        CLA = 0x94,
                        Instruction=InstructionCode.SelectFile,
                        P1 = 0x09,
                        P2 = 0x00,
                        Le = 0x19,
                        Data=new byte[] {0x00, 0x00}
                    };

                    //Lancement de la transaction entre le lecteur et la carte
                    sc = readerContact.BeginTransaction();

                    //En cas d'erreur
                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("erreur lors de la transaction" + "\n" + SCardHelper.StringifyError(sc)+"\n"+"L106");
                    }

                    //Déclaration des variables nécessaire lors de la transmission de la commande
                    var receivePci = new SCardPCI();
                    var sendPci = SCardPCI.GetPci(readerContact.ActiveProtocol);
                    var receiveBuffer = new byte[256];
                    var command = getFile.ToArray();

                    //Envoi de la commande à la carte
                    sc = readerContact.Transmit(sendPci, command, receivePci, ref receiveBuffer);

                    //En cas d'erreur envoi des informations concernant l'erreur ainsi que le numéro de ligne
                    if (sc != SCardError.Success)
                    {
                       MessageBox.Show("erreur lors de l'envoi de la commande" + "\n" + SCardHelper.StringifyError(sc)+"\n"+"L121");
                    }

                    //Création de la commande GetReponse nécessaire pour récupérer les données
                    var getRep = new CommandApdu(IsoCase.Case2Extended, SCardProtocol.T0)
                    {
                        CLA = 0x94,
                        Instruction = InstructionCode.GetResponse
                    };

                    //début de la transaction
                    sc = readerContact.BeginTransaction();

                    //Contrôle des erreur lors de la transaction 
                    if (sc!=SCardError.Success)
                    {
                        MessageBox.Show("Erreur lors de la transaction"+"\n"+SCardHelper.StringifyError(sc)+"\n"+"L137");
                    }

                    //Réutilisation des variables déclarées précedemment en les réassignant
                    receivePci = new SCardPCI();
                    sendPci = SCardPCI.GetPci(readerContact.ActiveProtocol);
                    receiveBuffer = new byte[256];
                    command = getRep.ToArray();

                    //transmission de la commande getRep et vérification des erreurs
                    sc = readerContact.Transmit(sendPci, command, receivePci, ref receiveBuffer);
                    if (sc!=SCardError.Success)
                    {
                        MessageBox.Show("Erreur dans l'envoi de la commande Get Response"+"\n"+SCardHelper.StringifyError(sc)+"\n"+"L150");
                    }

                    //Préparation de la commande de réponse et contrôle des erreurs
                    var apduRep = new ResponseApdu(receiveBuffer, IsoCase.Case2Extended, readerContact.ActiveProtocol);
                    //Vérification de l'existence de données dans la réponse 
                    if (apduRep.HasData)
                    {
                        //Création d'un tableau de byte des données
                        byte[] incomingData = apduRep.GetData();

                        //déclaration de la chaine qui contiendra les données reçues
                        string file = "";
                        //Boucle de remplissage de la chaine par les données reçues affichées en hexa
                        for (int i = 0; i < incomingData.Length; i++)
                        {
                            file += incomingData[i].ToString("X2");
                        }
                        //Message box affichant le résultat
                        MessageBox.Show("résultat select file..."+"\n"+file);

                    }
                    
                    

                }
            }
        }

        /// <summary>
        /// Méthode appelée lors du clic sur le bouton quitter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuitter_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void btnIso1443_Click(object sender, EventArgs e)
        {
            Iso1443B.establishContext();
            Iso1443B.iso1443_ID(this);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1;
using System.Runtime.InteropServices;
using SpringCard.PCSC;
using AtlanticZeiser.CpiPc.Tools.Misc;
using Desfire;


namespace Desfire
{
    public partial class formDesfire : Form
    {
        static SCardReader reader;
        static IntPtr hContext = IntPtr.Zero;
        public string SessionKey;
        string error = "";
        string[] readersList;
        byte[] key;
        int rc;
        int HCard;
        bool Auth;
        byte nKey;
        int iKey;
        uint lResult;
        string sKey = "";
        SCardChannel channel;
        uint ActivProtocol;
        IntPtr phCard, handle;
        SCARD card;
        SCardChannel sCard;
        CardData keyMaster,knull;
       
        List<string> lLecteurs = new List<string>();
        public formDesfire()
        {
            InitializeComponent();
            init();
        }


        private void init()
        {
            gBxInfo.Visible = false;
            panelISO.Visible= false;
            greenBtnAuth.Image= greenBtnConnect.Image =Properties.Resources.SmallRoundGreen;
           redBtnAuth.Image= redBtnConnect.Image = Properties.Resources.SmallRoundRed;
            lResult = SCARD.EstablishContext(SCARD.SCOPE_SYSTEM, IntPtr.Zero, IntPtr.Zero, ref hContext);
            if (lResult != SCARD.S_SUCCESS)
            {
                MessageBox.Show("Erreur context");
            }
            readersList = SCARD.GetReaderList();
            for (int i = 0; i < readersList.Length; i++)
            {
                cbReadersDesfire.Items.Add(readersList[i]);
            }
            byte[] keyNb = new byte[] { 00, 01, 02, 03, 04, 05, 06, 07, 08, 09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };
            cBxDesfireKeyNb.DataSource = keyNb;
            lResult= SCARD.ReleaseContext(hContext);
            
            if (lResult!=SCARD.S_SUCCESS)
            {
                MessageBox.Show("Erreur context not released");
            }
        }

        private void cBxISO_CheckedChanged(object sender, EventArgs e)
        {
            if (cBxISO.Checked)
            {
                panelISO.Visible = true;
            } else panelISO.Visible = false;
        }

        

        private void btnCreateApllication_Click(object sender, EventArgs e)
        {
            int rc;
            rc = SCARD_DESFIRE.AuthenticateAes(channel.hCard, nKey, key);
            uint AID = Convert.ToUInt32(tBxAID.Text,16);
            byte KS1 = (byte)Convert.ToUInt16(tBxKS1.Text);
            byte KS2= (byte)Convert.ToUInt16(tBxKS2.Text);
            if (rc != SCARD.S_SUCCESS)
            {
                MessageBox.Show("Erreur authentification" + "\n" + rc.ToString("X2"));
                return;
            }
            if (!cBxISO.Checked)
            {
         
                rc = SCARD_DESFIRE.CreateApplication(channel.hCard , AID, KS1,KS2);
            
                tBxRetourCreate.Text = rc.ToString("X2");

            }
            else
            {
                ushort isoID = Convert.ToUInt16(tBxISOID.Text);
                byte keyCount = (byte)Convert.ToUInt16(tBxKS2.Text[0]);
                byte[] DFname = Encoding.ASCII.GetBytes(tBxDFName.Text);
                byte DFlength = (byte)DFname.Length;
                rc = SCARD_DESFIRE.CreateIsoApplication(channel.hCard, AID, KS1, keyCount, isoID, DFname, DFlength);
                tBxRetourCreate.Text = rc.ToString("X2");
            }
        }

        private void btnDesfireConnect_Click(object sender, EventArgs e)
        {
            lResult = SCARD.EstablishContext(SCARD.SCOPE_SYSTEM, IntPtr.Zero, IntPtr.Zero, ref hContext);
            if (lResult != SCARD.S_SUCCESS)
            {
                MessageBox.Show("Erreur context");
            }
            if (cbReadersDesfire.SelectedItem==null)
            {
                redBtnConnect.Visible = true;
                greenBtnConnect.Visible = false;
                MessageBox.Show("Merci de sélectionner un lecteur");
                return;
            }
            reader = new SCardReader(SCARD.SCOPE_SYSTEM,cbReadersDesfire.SelectedItem.ToString());
            if (!reader.CardPresent)
            {
                redBtnConnect.Visible = true;
                greenBtnConnect.Visible = false;
                MessageBox.Show("Pas de cartes sur le lecteur!!", "Erreur de carte ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            channel = new SCardChannel(reader);
            if (channel==null)
            {
                greenBtnConnect.Visible = false;
                redBtnConnect.Visible = true;
                MessageBox.Show("Erreur de connexion a la carte");
                return;
            }
            
            channel.ShareMode = SCARD.SHARE_SHARED;
            channel.Protocol = SCARD.PROTOCOL_T1;

            if (!channel.Connect())
            {
                greenBtnConnect.Visible = false;
                redBtnConnect.Visible = true;
                MessageBox.Show("Erreur de connexion");
                return;
            }
            else
            {
                greenBtnConnect.Visible = true;              redBtnConnect.Visible = false;
            }

        }

        private void getVersion()
        {
            string info = "";
            SCARD_DESFIRE.DF_VERSION_INFO version_info = new SCARD_DESFIRE.DF_VERSION_INFO();
            byte[] uid = new byte[14];
            SCARD_DESFIRE.GetCardUID(channel.hCard, uid);
            for (int i = 0; i < 14; i++)
            {
                tBxATRDesfire.Text += uid[i];
            }
            rc = SCARD_DESFIRE.GetVersion(channel.hCard, ref version_info);
            if (rc != SCARD.S_SUCCESS)
            {
                Console.WriteLine("Desfire 'get version' command failed.");
            }
            if (version_info.bSwVendorID == 0x04)
            {
                tBxDesfireVersion.Text = " NXP";
            }
            if (version_info.bSwSubType == 0x01)
            {
                tBxVendor.Text = "Desfire";
            }

            tBxAnnee.Text = "20" + version_info.bProductionYear.ToString("X2");

            if ((version_info.bHwVendorID != 0x04) || (version_info.bSwVendorID != 0x04))
            {
                MessageBox.Show("Manufacturer is not NXP\n");
            }

            if ((version_info.bHwType != 0x01) || (version_info.bSwType != 0x01))
            {
                MessageBox.Show("Type is not Desfire\n");
            }

            if (version_info.bSwMajorVersion < 1)
            {
                MessageBox.Show("Software version is below EV1\n");
            }
            gBxInfo.Visible = true;
        }

        private void btnDesfireAuthAES_Click(object sender, EventArgs e)
        {
            if (cBxDesfireKeyNb.SelectedItem==null)
            {
                error+="Veuillez sélectionné l'index de la clé utilisée"+"\n";
                return;
            }
            if (tBxDesfireCle.Text == string.Empty)
            {
                error+="Veuillez entrer une clé pour l'authentification AES";
            }
            if (error!="")
            {
                MessageBox.Show(error);
                return;
            }
            switch (cBxDesfireKeyNb.SelectedItem.ToString())
            {
                //todo: attribution cle
                case "0":
                   nKey=0x00;
                    break;
                case "1":
                   nKey=0x01;
                    break;
                case "2":
                    nKey = 0x02;
                    break;
                case "3":
                    nKey = 0x03;
                    break;
                case "4":
                    nKey = 0x04;
                    break;
                case "5":
                    nKey = 0x05;
                    break;
                case "6":
                    nKey = 0x06;
                    break;
                case "7":
                    nKey = 0x07;
                    break;
                case "8":
                    nKey = 0x08;
                    break;
                case "9":
                    nKey = 0x09;
                    break;
                case "10":
                    nKey = 0x10;
                    break;
                case "11":
                    nKey = 0x11;
                    break;
                case "12":
                    nKey = 0x12;
                    break;
                case "13":
                    nKey = 0x13;
                    break;
                case "14":
                    nKey = 0x14;
                    break;
                    
                
            }
            rc = SCARD_DESFIRE.AttachLibrary(channel.hCard);
            if (rc != SCARD.S_SUCCESS)
            {
                redBtnAuth.Visible = true;
                MessageBox.Show("erreur chargement librairie");
            }
            
            rc = SCARD_DESFIRE.IsoWrapping(channel.hCard, 1);
            if (rc != SCARD.S_SUCCESS)
            {
                redBtnAuth.Visible = true;
                MessageBox.Show("Failed to select the ISO 7816 wrapping mode.");
                return;
            }
            
            
            if (tBxDesfireCle.TextLength == 32)
            {
                iKey = tBxDesfireCle.TextLength / 2;

            }
            else
            {
                MessageBox.Show("La clé doit être de 32bits");
                return;
            }

            
            for (int i = 0; i < tBxDesfireCle.TextLength; i=i+2)
            {
                if (i==0)
                {
                    sKey = tBxDesfireCle.Text.Substring(i,2);
                } else sKey +=" "+ tBxDesfireCle.Text.Substring(i, 2);
            }
            string[] arKey = sKey.Split(' ');
            
            key = new byte[iKey];
            for (int i = 0; i < arKey.Length; i++)
            {
                key[i] =(byte)Convert.ToInt32( arKey[i],16);
            }
            rc = SCARD_DESFIRE.AuthenticateAes(channel.hCard,nKey, key); //00-key0
            if (rc != SCARD.S_SUCCESS)
            {
                MessageBox.Show("Desfire 'AuthenticateAes' command failed - rc=" + rc.ToString("X2"));
            }
            else
            {
                chek.ForeColor = Color.Green;
                chek.Text = "\u2714";
                Auth = true;
            }

            if (Auth)
            {
                getVersion();
            }
        }
    }
}

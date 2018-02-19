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
        string[] readersList;
        int HCard;
        bool Auth;
        uint lResult;
        SCardChannel channel;
        uint ActivProtocol;
        IntPtr phCard, handle;
        SCARD card;
        SCardChannel sCard;
        CardData keyMaster,knull;
        //static string lecteur = "SpringCard Prox'N'Roll Contactless 0";
        List<string> lLecteurs = new List<string>();
        public formDesfire()
        {
            InitializeComponent();
            init();
        }


        private void init()
        {
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
        }


        private void btnDesfireConnect_Click(object sender, EventArgs e)
        {
            reader = new SCardReader(cbReadersDesfire.SelectedItem.ToString());
            if (!reader.CardPresent)
            {
                MessageBox.Show("Pas de cartes sur le lecteur!!", "Erreur de carte ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            channel = new SCardChannel(reader);
            if (channel==null)
            {
                MessageBox.Show("Erreur de connexion a la carte");
                return;
            }
            
            channel.ShareMode = SCARD.SHARE_SHARED;
            channel.Protocol = SCARD.PROTOCOL_T1;

            if (!channel.Connect())
            {
                MessageBox.Show("Erreur de connexion");
                return;
            }
            
            




        }
        //if (sCard.CardAvailable)
        //{
        //    sCard.ReconnectLeave();
        //}
        //sCard.Command = new CAPDU("6F");
        //if (!sCard.Transmit())
        //{
        //    MessageBox.Show("Erreur transmission getAID");
        //}

        //var data = sCard.Response.AsString();


        private void btnDesfireAuthAES_Click(object sender, EventArgs e)
        {
            switch (cBxDesfireKeyNb.SelectedItem.ToString())
            {
                //todo: attribution cle
                case "0":
                    MessageBox.Show(cBxDesfireKeyNb.SelectedItem.ToString());
                    break;
                case "1":
                    MessageBox.Show(cBxDesfireKeyNb.SelectedItem.ToString());
                    break;
                case "2":
                    MessageBox.Show(cBxDesfireKeyNb.SelectedItem.ToString());
                    break;
                case "3":
                    MessageBox.Show(cBxDesfireKeyNb.SelectedItem.ToString());
                    break;
                case "4":
                    MessageBox.Show(cBxDesfireKeyNb.SelectedItem.ToString());
                    break;
                case "5":
                    MessageBox.Show(cBxDesfireKeyNb.SelectedItem.ToString());
                    break;
                case "6":
                    MessageBox.Show(cBxDesfireKeyNb.SelectedItem.ToString());
                    break;
                case "7":
                    MessageBox.Show(cBxDesfireKeyNb.SelectedItem.ToString());
                    break;
                case "8":
                    MessageBox.Show(cBxDesfireKeyNb.SelectedItem.ToString());
                    break;
                case "9":
                    MessageBox.Show(cBxDesfireKeyNb.SelectedItem.ToString());
                    break;
                case "10":
                    MessageBox.Show(cBxDesfireKeyNb.SelectedItem.ToString());
                    break;
                case "11":
                    MessageBox.Show(cBxDesfireKeyNb.SelectedItem.ToString());
                    break;
                case "12":
                    MessageBox.Show(cBxDesfireKeyNb.SelectedItem.ToString());
                    break;
                case "13":
                    MessageBox.Show(cBxDesfireKeyNb.SelectedItem.ToString());
                    break;
                case "14":
                    MessageBox.Show(cBxDesfireKeyNb.SelectedItem.ToString());
                    break;
                    
                
            }
            int rc = SCARD_DESFIRE.AttachLibrary(channel.hCard);
            if (rc != SCARD.S_SUCCESS)
            {
                MessageBox.Show("erreur chargement librairie");
            }
            else MessageBox.Show("lib ok");
            rc = SCARD_DESFIRE.IsoWrapping(channel.hCard, 1);
            if (rc != SCARD.S_SUCCESS)
            {
                MessageBox.Show("Failed to select the ISO 7816 wrapping mode.");
                return;
            }
            byte[] key = { 0x24,0xDE,0x17,0x44,0x0A,0xB7,0x5C,0x99,0x12,0xD7,0x71,0x11,0x2E,0x66,0x44,0xA6 }; // A modifier sans doute
            rc = SCARD_DESFIRE.AuthenticateAes(channel.hCard, 0x00, key); //00-key0
            if (rc != SCARD.S_SUCCESS)
            {
                MessageBox.Show("Desfire 'AuthenticateAes' command failed - rc=" + rc);
            }
            else
            {
                MessageBox.Show("AuthenticateAes OK");
                Auth = true;
            }

            if (Auth)
            {
                string info = "";
                SCARD_DESFIRE.DF_VERSION_INFO version_info = new SCARD_DESFIRE.DF_VERSION_INFO();
                rc = SCARD_DESFIRE.GetVersion(channel.hCard, ref version_info);
                if (rc != SCARD.S_SUCCESS)
                {
                    Console.WriteLine("Desfire 'get version' command failed.");
                }
                if (version_info.bSwVendorID == 0x04)
                {
                    tBxVendor.Text = " NXP";
                }
                if (version_info.bSwSubType == 0x01)
                {
                    tBxDesfireVersion.Text = "Desfire";
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
                byte[] fidList = new byte[12];
                byte fidCount=new byte();
                rc=SCARD_DESFIRE.SelectApplication(channel.hCard, 990144);
                if (rc!=SCARD.S_SUCCESS)
                {
                    MessageBox.Show("application non sélectionnée");
                }
                rc = SCARD_DESFIRE.AuthenticateAes(channel.hCard, 00, key);
                if (rc!=SCARD.S_SUCCESS)
                {
                    MessageBox.Show("Erreur auth"+"\n"+SCARD_DESFIRE.GetErrorMessage(rc));
                }
                rc = SCARD_DESFIRE.GetFileIDs(channel.hCard, 12, fidList , ref fidCount);
                if (rc!=SCARD.S_SUCCESS)
                {
                    MessageBox.Show("Erreur recup file ID list");
                };
            
                rc = SCARD_DESFIRE.CreateApplication(channel.hCard, 495352, 0x11, 0x62);
                if (rc!=SCARD.S_SUCCESS)
                {
                    MessageBox.Show("application non créée"+"\n"+"erreur: "+rc.ToString("X2"));
                }
                else MessageBox.Show("Application n°495352 créée"+"\n"+rc.ToString("X2"));
            }

                
            
        }
    }
}

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
        byte[] AMK = new byte[16] { 0x9B, 0x64, 0x29, 0xEF, 0x91, 0x19, 0x19, 0xA4, 0x04, 0xD5, 0x3D, 0x38, 0xE3, 0xBA, 0xE6, 0xB0 };

        byte[] read1 = new byte[16] { 0xA2, 0xAB, 0xFD, 0xB3, 0x43, 0xE6, 0x19, 0xB0, 0x0B, 0xF2, 0x67, 0x9B, 0xDF, 0xD6, 0xE5, 0x57 };

        byte[] write1 = new byte[16] { 0x8C, 0x89, 0x8C, 0x29, 0x19, 0x7C, 0x6D, 0x9E, 0xF2, 0x4C, 0xE5, 0x49, 0x0D, 0xD5, 0xD5, 0x97 };
        byte[] read2 = new byte[16] { 0xF8, 0x4F, 0xA4, 0xB9, 0xEF, 0x62, 0xA8, 0xFE, 0xBF, 0x9D, 0x7F, 0xF2, 0x54, 0x6E, 0x83, 0x85 };
        byte[] write2 = new byte[16] { 0xE9, 0x2C, 0x0D, 0x02, 0xCE, 0x64, 0xA7, 0x0D, 0xB9, 0x57, 0xEA, 0xD0, 0x80, 0xD9, 0x8C, 0xFD };
        byte[] Master = new byte[16] { 0x24, 0xDE, 0x17, 0x44, 0x0A, 0xB7, 0x5C, 0x99, 0x12, 0xD7, 0x71, 0x11, 0x2E, 0x66, 0x44, 0xA6 };
        byte[] keyNul = new byte[16] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 00, 00, 00, 00, 00, 00, 00, 00, 00 };
        int rc;
        byte KS1;
        byte KS2;
        int HCard;
        uint AID;
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
            checkCleChange.BorderStyle = BorderStyle.None;
            checkConnect.BorderStyle = BorderStyle.None;
            checkCreate.BorderStyle = BorderStyle.None;
            checkSelect.BorderStyle = BorderStyle.None;
            
            gBxInfo.Visible = false;
            panelISO.Visible= false;
            
            lResult = SCARD.EstablishContext(SCARD.SCOPE_SYSTEM, IntPtr.Zero, IntPtr.Zero, ref hContext);
            if (lResult != SCARD.S_SUCCESS)
            {
                MessageBox.Show("Erreur context");
            }
            readersList = SCARD.GetReaderList(hContext);
            if (readersList==null)
            {
                MessageBox.Show("Aucun lecteur branché");
                return;
            }
            for (int i = 0; i < readersList.Length; i++)
            {
                cbReadersDesfire.Items.Add(readersList[i]);
            }
            byte[] keyNb = new byte[] { 00, 01, 02, 03, 04, 05, 06, 07, 08, 09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };
            cBxDesfireKeyNb.DataSource = keyNb;

        }

        private void cBxISO_CheckedChanged(object sender, EventArgs e)
        {
            if (cBxISO.Checked)
            {
                panelISO.Visible = true;
            } else panelISO.Visible = false;
        }

        private void desfireSetup()
        {
            checkConnect.Text = "";
            lResult = SCARD.EstablishContext(SCARD.SCOPE_SYSTEM, IntPtr.Zero, IntPtr.Zero, ref hContext);
            if (lResult != SCARD.S_SUCCESS)
            {
                MessageBox.Show("Erreur context");
            }
           
            if (cbReadersDesfire.SelectedItem == null)
            {
                
                MessageBox.Show("Merci de sélectionner un lecteur");
                return;
            }
            reader = new SCardReader(SCARD.SCOPE_SYSTEM, cbReadersDesfire.SelectedItem.ToString());
            if (!reader.CardPresent)
            {
                
                MessageBox.Show("Pas de cartes sur le lecteur!!", "Erreur de carte ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            lResult = SCARD.Connect(hContext, reader.Name, SCARD.SHARE_SHARED, SCARD.PROTOCOL_T1, ref phCard, ref ActivProtocol);
            if (lResult!=SCARD.S_SUCCESS)
            {
                MessageBox.Show("Erreur connect");
                return;
            }
            else MessageBox.Show("Connecté");
            channel = new SCardChannel(reader);
            if (channel == null)
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
            else
            {
                checkConnect.Text = "\u2714";
                checkConnect.ForeColor = Color.Green;
            }

            rc = SCARD_DESFIRE.AttachLibrary(channel.hCard);
            if (rc != SCARD.S_SUCCESS)
            {
             
                MessageBox.Show("erreur chargement librairie");
            }


            rc = SCARD_DESFIRE.IsoWrapping(channel.hCard, 1);
            if (rc != SCARD.S_SUCCESS)
            {
                MessageBox.Show("Failed to select the ISO 7816 wrapping mode.");
                return;
            }
        }

        private void btnCreateApllication_Click(object sender, EventArgs e)
        {
           
            checkCreate.Text = "";
            if (!cBxISO.Checked)
            {

                
                byte[] option=new byte[17] { 0x24, 0xDE, 0x17, 0x44, 0x0A, 0xB7, 0x5C, 0x99, 0x12, 0xD7, 0x71, 0x11, 0x2E, 0x66, 0x44, 0xA6 ,0x02};
                rc = SCARD_DESFIRE.SetConfiguration(channel.hCard,0x01,option,0x11);
                if (rc != SCARD.S_SUCCESS)
                {
                    MessageBox.Show("Erreur set config Master");
                    return;
                }
                else MessageBox.Show("Config Master OK");

                rc = SCARD_DESFIRE.AuthenticateAes(channel.hCard, 00, keyNul);

                if (rc != SCARD.S_SUCCESS)
                {
                    MessageBox.Show("Erreur authentification l. 186");
                    return;
                }

                ////récupération des id et KeySet
                // UInt32.TryParse(tBxAIDCreate.Text,out AID);
                ////AID = Convert.ToUInt32(tBxAIDCreate.Text, 32);
                //KS1 = Convert.ToByte(tBxKS1.Text, 16);
                //KS2 = Convert.ToByte(tBxKS2.Text, 16);
                uint aID = 0x55_55_55;
                byte ks11 = 0x0B;
                byte ks22 = 0x85;
                byte b0 = 00,b1=01;
                // envoi de la commande de creation application

                

                rc = SCARD_DESFIRE.CreateApplication(channel.hCard, aID,ks11,ks22);


                if (rc != SCARD.S_SUCCESS)
                {
                    checkCreate.ForeColor = Color.Red;
                    checkCreate.Text = "X";
                    MessageBox.Show("Erreur création application " + aID + "\n" + "l. 207");
                    return;
                }
                else
                {
                    checkCreate.Text = "\u2714";
                    checkCreate.ForeColor = Color.Green;
                }

                rc = SCARD_DESFIRE.SelectApplication(channel.hCard, aID);

                if (rc != SCARD.S_SUCCESS)
                {                    
                    MessageBox.Show("Erreur select application " + aID +"l. 220");
                    return;
                }

                rc = SCARD_DESFIRE.AuthenticateAes(channel.hCard, b0, Master);

                if (rc != SCARD.S_SUCCESS)
                {
                    MessageBox.Show("Erreur auth l. 228");
                    return;
                }
                rc = SCARD_DESFIRE.ChangeKeySettings(channel.hCard, 0xE9);
                if (rc != SCARD.S_SUCCESS)
                {
                    MessageBox.Show("Erreur change key settings l. 234");
                    return;
                }
                rc = SCARD_DESFIRE.SelectApplication(channel.hCard, aID);

                if (rc != SCARD.S_SUCCESS)
                {
                    MessageBox.Show("Erreur select application l. 241" + aID);
                    return;
                }

                rc = SCARD_DESFIRE.AuthenticateAes(channel.hCard, b0, Master);

                if (rc != SCARD.S_SUCCESS)
                {
                    MessageBox.Show("Erreur auth l.249");
                    return;
                }
                uint size = 000050;
                ushort AR1 = 4608;//0x1200
                rc = SCARD_DESFIRE.CreateStdDataFile(channel.hCard, b0, b0, AR1, size);
                if (rc != SCARD.S_SUCCESS)
                {
                    MessageBox.Show("Erreur create file 00 l.257");
                    return;
                }
                

                ushort AR2 = 13312; //0x3400
                rc = SCARD_DESFIRE.CreateStdDataFile(channel.hCard, b1, b0, AR2, size);
                if (rc != SCARD.S_SUCCESS)
                {
                    MessageBox.Show("Erreur create file 00 l.266");
                    return;
                }
                

                rc = SCARD_DESFIRE.SelectApplication(channel.hCard, aID);

                if (rc != SCARD.S_SUCCESS)
                {
                    MessageBox.Show("Erreur select application " + aID+" l.275");
                    return;
                }


                rc = SCARD_DESFIRE.AuthenticateAes(channel.hCard, b0, Master);

                if (rc != SCARD.S_SUCCESS)
                {
                    MessageBox.Show("Erreur authentification" + "\n" + "l. 284");
                    return;
                }
                rc = SCARD_DESFIRE.ChangeKeyAes(channel.hCard, b0,b1,  read1, null);
                if (rc != SCARD.S_SUCCESS)
                {
                    MessageBox.Show("Erreur change key 00 l.290");
                    return;
                }
                else MessageBox.Show("Key 00 updated");


                rc = SCARD_DESFIRE.AuthenticateAes(channel.hCard, 0x01, Master);

                if (rc != SCARD.S_SUCCESS)
                {
                    MessageBox.Show("Erreur authentification" + "\n" + "l. 284");
                    return;
                }

                rc = SCARD_DESFIRE.ChangeKeyAes(channel.hCard, 0x01, 0x03, read1, null);
                if (rc != SCARD.S_SUCCESS)
                {
                    MessageBox.Show("Erreur change key 01 l306");
                    return;
                }
                else MessageBox.Show("Key 01 updated");


                rc = SCARD_DESFIRE.AuthenticateAes(channel.hCard, 0x02, Master);

                if (rc != SCARD.S_SUCCESS)
                {
                    MessageBox.Show("Erreur authentification" + "\n" + "l. 284");
                    return;
                }

                rc = SCARD_DESFIRE.ChangeKeyAes(channel.hCard, 0x02, 0x03, write1, null);
                if (rc != SCARD.S_SUCCESS)
                {
                    MessageBox.Show("Erreur change key 02 l.322");
                    return;
                }
                else MessageBox.Show("Key 02 updated");

                rc = SCARD_DESFIRE.AuthenticateAes(channel.hCard, 0x03, Master);

                if (rc != SCARD.S_SUCCESS)
                {
                    MessageBox.Show("Erreur authentification l.331" );
                    return;
                }

                rc = SCARD_DESFIRE.ChangeKeyAes(channel.hCard, 0x03, 0x03, read2, null);
                if (rc != SCARD.S_SUCCESS)
                {
                    MessageBox.Show("Erreur change key 03 l.338");
                    return;
                }
                else MessageBox.Show("Key 03 updated");

                rc = SCARD_DESFIRE.AuthenticateAes(channel.hCard, 0x04, Master);

                if (rc != SCARD.S_SUCCESS)
                {
                    MessageBox.Show("Erreur authentification l.347");
                    return;
                }

                rc = SCARD_DESFIRE.ChangeKeyAes(channel.hCard, 0x04, 0x03, write1, null);
                if (rc != SCARD.S_SUCCESS)
                {
                    MessageBox.Show("Erreur change key 04 l.354");
                    return;
                }
                else MessageBox.Show("Key 04 updated");
            }
            else
            {
                ushort isoID = Convert.ToUInt16(tBxISOID.Text);
                byte keyCount = controleurPcsc.createByte(tBxKS2.Text)[0];
                byte[] DFname =controleurPcsc.createByte(tBxDFName.Text);
                byte DFlength = (byte)DFname.Length;
                rc = SCARD_DESFIRE.CreateIsoApplication(channel.hCard, AID, KS1, keyCount, isoID, DFname, DFlength);
                tBxRetourCreate.Text = rc.ToString("X2");
            }
        }



        private void btnDesfireConnect_Click(object sender, EventArgs e)
        {
            desfireSetup();
        }

        private void btnSelectAppAID_Click(object sender, EventArgs e)
        {
            if (tBxAIDSelect.Enabled)
            {
                
                tBxAIDSelect.Enabled = false;
                panelFileCreation.Visible = true;
                btnSelectAppAID.Text = "Ann&uler";

                ////Authentification
                //rc = SCARD_DESFIRE.AuthenticateAes(channel.hCard, nKey, key); 
                //if (rc != SCARD.S_SUCCESS)
                //{
                //    MessageBox.Show("Desfire 'AuthenticateAes' command failed - rc=" + rc.ToString("X2"));
                //}

                uint App;
                
                App = Convert.ToUInt32(tBxAIDSelect.Text);
                
               rc= SCARD_DESFIRE.SelectApplication(channel.hCard, App);
                if (rc!=SCARD.S_SUCCESS)
                {
                    MessageBox.Show("Erreur app select"+App+"\n"+rc.ToString("X2"));
                    return;
                }
                else
                {
                    checkSelect.ForeColor = Color.Green;
                    checkSelect.Text = "\u2714";
                }
            }
            else
            {
                btnSelectAppAID.Text = "&Select";
                panelFileCreation.Visible = false;
                tBxAIDSelect.Enabled = true;
            }
        }

        private void btnFileCreate_Click(object sender, EventArgs e)
        {
            //Authentification
            rc = SCARD_DESFIRE.AuthenticateAes(channel.hCard, nKey, key); //00-key0
            if (rc != SCARD.S_SUCCESS)
            {
                MessageBox.Show("Desfire 'AuthenticateAes' command failed - rc=" + rc.ToString("X2"));
            }

            byte FID = controleurPcsc.createByte(tBxFileCreateNum.Text)[0];

            byte comSet = controleurPcsc.createByte(tBxFileCreateComSet.Text)[0];

            ushort accessRight = (ushort)Convert.ToUInt32(tBxFileCreateAR.Text);

            uint fileSize = (uint)Convert.ToUInt32(tBxFileCreateSize.Text);
            rc = SCARD_DESFIRE.CreateStdDataFile(channel.hCard, FID, comSet, accessRight, fileSize);
            if (rc != SCARD.S_SUCCESS)
            {
                MessageBox.Show("Fichier n°" + FID.ToString("X2") + " non crée" + "\n" + rc.ToString("X2"));
            }
            else MessageBox.Show("Fichier n°" + FID.ToString() + " crée");

            rc = SCARD_DESFIRE.SelectApplication(channel.hCard, (uint)FID);
            if (rc != SCARD.S_SUCCESS)
            {
                MessageBox.Show("Erreur select app" + FID.ToString("X2") + "\n" + rc.ToString("X2"));
                return;
            }
            byte[] keyNul = new byte[16];
            rc = SCARD_DESFIRE.AuthenticateAes(channel.hCard, 0x00, keyNul);
            if (rc != SCARD.S_SUCCESS)
            {
                MessageBox.Show("erreur auth 0x00 keyNul" + "\n" + rc.ToString("X2"));
                return;
            }
            rc = SCARD_DESFIRE.ChangeKeySettings(channel.hCard, 0x09);
            if (rc != SCARD.S_SUCCESS)
            {
                MessageBox.Show("Erreur change key setting" + "\n" + rc.ToString("X2"));
                return;
            }
            else MessageBox.Show("Change key setting reussi");

            rc = SCARD_DESFIRE.SelectApplication(channel.hCard, (uint)FID);
            if (rc != SCARD.S_SUCCESS)
            {
                MessageBox.Show("Erreur select app" + FID.ToString("X2") + "\n" + rc.ToString("X2"));
                return;
            }
            rc = SCARD_DESFIRE.AuthenticateAes(channel.hCard, 0x00, keyNul);
            if (rc != SCARD.S_SUCCESS)
            {
                MessageBox.Show("erreur auth 0x00 keyNul" + "\n" + rc.ToString("X2"));
                return;
            }
            rc = SCARD_DESFIRE.CreateStdDataFile(channel.hCard, 0x01, 0x00, 61507, 50);
            if (rc != SCARD.S_SUCCESS)
            {
                MessageBox.Show("Erreur creation fichier 01" + "\n" + rc.ToString("X2"));
                return;
            }
            else MessageBox.Show("Fichier 01 crée");

            rc = SCARD_DESFIRE.SelectApplication(channel.hCard, (uint)FID);
            if (rc != SCARD.S_SUCCESS)
            {
                MessageBox.Show("Erreur select app" + FID.ToString("X2") + "\n" + rc.ToString("X2"));
                return;
            }
            byte[] AMK = controleurPcsc.createByte("9B6429EF911919A404D53D38E3BAE6B0");
            byte[] read1 = controleurPcsc.createByte("A2ABFDB343E619B00BF2679BDFD6E557");
            byte[] write1 = controleurPcsc.createByte("8C898C29197C6D9EF24CE5490DD5D597");
            byte[] read2 = controleurPcsc.createByte("F84FA4B9EF62A8FEBF9D7FF2546E8385");
            byte[] write2 = controleurPcsc.createByte("E92C0D02CE64A70DB957EAD080D98CFD");
            rc = SCARD_DESFIRE.ChangeKeyAes(channel.hCard, 0x00, 0x03, AMK,keyNul);
            if (rc!=SCARD.S_SUCCESS)
            {
                MessageBox.Show("Erreur changement de clé");
                return;
            }
            else MessageBox.Show("Clé 00 changée");

            rc = SCARD_DESFIRE.ChangeKeyAes(channel.hCard, 0x01, 0x03, read1, keyNul);
            if (rc!=SCARD.S_SUCCESS)
            {
                MessageBox.Show("Erreur changement clé 01");
                return;
            }
            else MessageBox.Show("Clé 01 changée");

            rc = SCARD_DESFIRE.ChangeKeyAes(channel.hCard, 0x02, 0x03, write1, keyNul);
            if (rc!=SCARD.S_SUCCESS)
            {
                MessageBox.Show("Erreur changement clé 02");
                return;
            }
            else MessageBox.Show("Clé 02 changée");

            rc = SCARD_DESFIRE.ChangeKeyAes(channel.hCard, 0x03, 0x03, read2, keyNul);
            if (rc != SCARD.S_SUCCESS)
            {
                MessageBox.Show("Erreur changement clé 03");
                return;
            }
            else MessageBox.Show("Clé 03 changée");

            rc = SCARD_DESFIRE.ChangeKeyAes(channel.hCard, 0x03, 0x04, write2, keyNul);
            if (rc != SCARD.S_SUCCESS)
            {
                MessageBox.Show("Erreur changement clé 04");
                return;
            }
            else MessageBox.Show("Clé 04 changée");

        }

        private void cBxAfficheCreateApp_CheckedChanged(object sender, EventArgs e)
        {
            if (panelAppCreate.Visible)
            {
                panelAppCreate.Visible = false;
            }
            else panelAppCreate.Visible = true;
            //SCARD_DESFIRE.SetConfiguration();
        }

        private void btnFormatPICC_Click(object sender, EventArgs e)
        {
            rc=SCARD_DESFIRE.FormatPICC(channel.hCard);
            if (rc != SCARD.S_SUCCESS)
            {
                MessageBox.Show("Format failed" + "\n" + rc.ToString("X2"));
            }
            else
            {
                MessageBox.Show("Format completed");
                btnFormatPICC.Visible = false;
            }
        }

        private void chkChangeKey_CheckedChanged(object sender, EventArgs e)
        {
            if (panelChangeKey.Visible)
            {
                panelChangeKey.Visible = false;

            }
            else panelChangeKey.Visible = true;
        }

        private void tBxKS1_MouseClick(object sender, MouseEventArgs e)
        {
            tBxKS1.Clear();
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
            btnSelectApp.Visible = true;
        }

        private void btnSelectApp_Click(object sender, EventArgs e)
        {
            
            if (!panelFileCreation.Visible)
            {
                panelFileCreation.Visible = true;
                btnSelectApp.Text = "Ann&uler";
                uint ID = Convert.ToUInt32( listBoxAppID.SelectedItem.ToString());
                rc = SCARD_DESFIRE.SelectApplication(channel.hCard, ID);
                if (rc!=SCARD.S_SUCCESS)
                {
                    MessageBox.Show("Erreur selection application "+ID+"\n"+rc.ToString("X2"));
                }
                else
                {
                    byte[] keyy = new byte[16];
                    rc = SCARD_DESFIRE.AuthenticateAes(channel.hCard, 0x00, keyy);
                    if (rc!=SCARD.S_SUCCESS)
                    {
                        MessageBox.Show("Erreur authentification");
                        return;
                    }
                    uint aid = Convert.ToUInt32(listBoxAppID.SelectedItem.ToString());
                    rc = SCARD_DESFIRE.SelectApplication(channel.hCard, aid);
                    if (rc!=SCARD.S_SUCCESS)
                    {
                        MessageBox.Show("Erreur select app n°"+aid+"\n"+rc.ToString("X2"));
                    }

                    rc = SCARD_DESFIRE.AuthenticateAes(channel.hCard, 0x00, keyy);
                    if (rc != SCARD.S_SUCCESS)
                    {
                        MessageBox.Show("Erreur authentification");
                        return;
                    }
                    byte[] read1 = controleurPcsc.createByte("A2ABFDB343E619B00BF2679BDFD6E557");
                    byte[] write1 = controleurPcsc.createByte("8C898C29197C6D9EF24CE5490DD5D597");
                    rc = SCARD_DESFIRE.ChangeKeyAes(channel.hCard, 0x01,0x01, read1,keyy);
                    if (rc!=SCARD.S_SUCCESS)
                    {
                        MessageBox.Show("Erreur changement clé 01"+"\n"+rc.ToString("X2"));
                    }
                    else MessageBox.Show("Clé 01 changée");
                    rc = SCARD_DESFIRE.ChangeKeyAes(channel.hCard, 0x02, 0x01, write1, keyy);
                    if (rc!=SCARD.S_SUCCESS)
                    {
                        MessageBox.Show("Erreur changement clé 02"+"\n"+rc.ToString("X2"));
                    }
                    else MessageBox.Show("Clé 02 changée");

                    rc = SCARD_DESFIRE.CreateStdDataFile(channel.hCard, 0x00, 0x00, 61473, 50);
                    if (rc!=SCARD.S_SUCCESS)
                    {
                        MessageBox.Show("Erreur creation 00");
                        return;
                    }
                    else MessageBox.Show("creation ficheir OK");
                }
            }
            else
            {
                panelFileCreation.Visible = false;
                btnSelectApp.Text = "S&elect";
            }
        }

        private void listBoxAppID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxAppID.SelectedItem.ToString()!="00")
            {
                int val = Convert.ToInt32( listBoxAppID.SelectedItem.ToString());
                tBxAIDhex.Text = val.ToString("X2");
            }
        }

        private void btnSetKeys_Click(object sender, EventArgs e)
        {
            byte[] AMK = controleurPcsc.createByte("9B6429EF911919A404D53D38E3BAE6B0");
            byte[] read1 = controleurPcsc.createByte("A2ABFDB343E619B00BF2679BDFD6E557");
            byte[] write1 = controleurPcsc.createByte("8C898C29197C6D9EF24CE5490DD5D597");
            byte[] read2 = controleurPcsc.createByte("F84FA4B9EF62A8FEBF9D7FF2546E8385");
            byte[] write2 = controleurPcsc.createByte("E92C0D02CE64A70DB957EAD080D98CFD");
            byte[] Master = controleurPcsc.createByte("24DE17440AB75C9912D771112E6644A6");
            byte[] keyNul = controleurPcsc.createByte("00000000000000000000000000000000");

            uint appID = 0x495352;
            rc = SCARD_DESFIRE.AuthenticateAes(channel.hCard, 0x00, keyNul);
            if (rc != SCARD.S_SUCCESS)
            {
                MessageBox.Show("Erreur auth master");
                return;
            }
            
            //Create app 495352
            rc = SCARD_DESFIRE.CreateApplication(channel.hCard, appID, 0x0B, 0x85);
            if (rc!=SCARD.S_SUCCESS)
            {
                MessageBox.Show("Erreur application "+appID+" non créée");
                return;
            }
            

            //Select app 495352
            rc = SCARD_DESFIRE.SelectApplication(channel.hCard, appID);
            if (rc != SCARD.S_SUCCESS)
            {
                MessageBox.Show("Erreur select application 495352 ");
                return;
            }
            

            //Auth Master key
            rc = SCARD_DESFIRE.AuthenticateAes(channel.hCard, 0x00,Master);
            if (rc != SCARD.S_SUCCESS)
            {
                MessageBox.Show("Erreur auth master");
                return;
            }
            

            //Set keys
            rc = SCARD_DESFIRE.ChangeKeySettings(channel.hCard, 0x09);
            if (rc != SCARD.S_SUCCESS)
            {
                MessageBox.Show("Erreur change key setting" );
                return;
            }
            else MessageBox.Show("Change key setting reussi");


            rc = SCARD_DESFIRE.CreateStdDataFile(channel.hCard, 0x00,0x00,61473,50);
            if (rc != SCARD.S_SUCCESS)
            {
                MessageBox.Show("erreur creation fichier 00");
                return;
            }
            else MessageBox.Show("App 00 créée");

            rc = SCARD_DESFIRE.CreateStdDataFile(channel.hCard, 0x01, 0x00, 61507, 50);
            if (rc != SCARD.S_SUCCESS)
            {
                MessageBox.Show("Erreur creation fichier 01" );
                return;
            }
            else MessageBox.Show("Fichier 01 crée");

           
            rc = SCARD_DESFIRE.ChangeKeyAes(channel.hCard, 0x00, 0x03, AMK, keyNul);
            if (rc != SCARD.S_SUCCESS)
            {
                MessageBox.Show("Erreur changement de clé");
                return;
            }
            else MessageBox.Show("Clé 00 changée");

            rc = SCARD_DESFIRE.ChangeKeyAes(channel.hCard, 0x01, 0x03, read1, keyNul);
            if (rc != SCARD.S_SUCCESS)
            {
                MessageBox.Show("Erreur changement clé 01");
                return;
            }
            else MessageBox.Show("Clé 01 changée");

            rc = SCARD_DESFIRE.ChangeKeyAes(channel.hCard, 0x02, 0x03, write1, keyNul);
            if (rc != SCARD.S_SUCCESS)
            {
                MessageBox.Show("Erreur changement clé 02");
                return;
            }
            else MessageBox.Show("Clé 02 changée");

            rc = SCARD_DESFIRE.ChangeKeyAes(channel.hCard, 0x03, 0x03, read2, keyNul);
            if (rc != SCARD.S_SUCCESS)
            {
                MessageBox.Show("Erreur changement clé 03");
                return;
            }
            else MessageBox.Show("Clé 03 changée");

            rc = SCARD_DESFIRE.ChangeKeyAes(channel.hCard, 0x03, 0x04, write2, keyNul);
            if (rc != SCARD.S_SUCCESS)
            {
                MessageBox.Show("Erreur changement clé 04");
                return;
            }
            else MessageBox.Show("Clé 04 changée");

        }

        private void Valider_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            rc = SCARD_DESFIRE.SetConfiguration(channel.hCard, 0x01, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },0x11);
            if (rc!=SCARD.S_SUCCESS)
            {
                MessageBox.Show("error set config");
            }
            else MessageBox.Show("set config ok");
        }

        private void getAppID()
        {
            uint[] aidList=new uint[10];
            byte aidCount=new byte();
            rc = SCARD_DESFIRE.GetApplicationIDs(channel.hCard, 0x10, aidList, ref aidCount);
            for (int i = 0; i < aidList.Count(); i++)
            {
                if (aidList[i].ToString("X2")!="00")
                {

                    listBoxAppID.Items.Add(aidList[i]);
                }
            }
            panelFileCreation.Visible = true;
        }

        private void btnDesfireAuthAES_Click(object sender, EventArgs e)
        {
            error = string.Empty;
            listBoxAppID.Items.Clear();
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

            
            
            
            
            if (tBxDesfireCle.TextLength == 32)
            {
                iKey = tBxDesfireCle.TextLength / 2;

            }
            else
            {
                MessageBox.Show("La clé doit être de 32bits");
                return;
            }
    
            key = new byte[iKey];
            int I = 0;
            for (int i = 0; i < tBxDesfireCle.Text.Length; i=i+2)
            {
                key[I] = Convert.ToByte(tBxDesfireCle.Text.Substring(i, 2));
                I++;
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
                btnFormatPICC.Visible = true;
                getVersion();
                getAppID();
            }
        }
    }
}

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
using SpringCardPCSC;
using SpringCardUtils;


namespace Desfire
{
    public partial class formDesfire : Form
    {
        static SCardReader reader;
        static IntPtr hContext = IntPtr.Zero;
        string[] readersList;
        uint lResult;
        uint ActivProtocol;
        IntPtr phCard;
        SCARD card;
        SCardChannel sCard;
        //static string lecteur = "SpringCard Prox'N'Roll Contactless 0";
        List<string> lLecteurs = new List<string>();
        public formDesfire()
        {
            InitializeComponent();
            init();
        }


        private void init()
        {
            lResult = SCARD.EstablishContext(SCARD.SCOPE_SYSTEM, IntPtr.Zero,IntPtr.Zero, ref hContext);
            if (lResult!=SCARD.S_SUCCESS)
            {
                MessageBox.Show("Erreur context");
            }
            readersList= SCARD.GetReaderList();
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
                MessageBox.Show("Pas de cartes sur le lecteur!!","Erreur de carte ",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                return;
            }
            sCard = new SCardChannel(reader);
            
            if (!sCard.Connect())
            {
                MessageBox.Show("Erreur de connexion");
                return;
            }

            byte[] ATR = reader.CardAtr.GetBytes();
            for (int i = 0; i < ATR.Length; i++)
            {
                tBxATRDesfire.Text += ATR[i].ToString("X2");
            }
            
            sCard.Command = new CAPDU("60");
            if (!sCard.Transmit())
            {
                MessageBox.Show("Erreur transmission");
                return;
            }
            else
            {
                string result = sCard.Response.AsString();
                if (result.Substring(0,2)=="AF")
                {
                    result = result.Substring(2, result.Length - 2);
                    sCard.Command = new CAPDU("AF");
                    sCard.Transmit();
                    string X = sCard.Response.AsString();
                    if (X.Substring(0,2)=="AF")
                    {
                        result += X.Substring(2, X.Length - 2);
                    }
                }
                tBxDesfireVersion.Text = result;
            }
        }

        private void btnDesfireAuthAES_Click(object sender, EventArgs e)
        {

            string cle = tBxDesfireCle.Text;
            byte keyNb = Convert.ToByte(cBxDesfireKeyNb.SelectedItem.ToString());
            byte[] keyAuth = Encoding.ASCII.GetBytes(tBxDesfireCle.Text);
            if (SpringCardPCSC.SCARD_DESFIRE.AuthenticateAes(phCard, keyNb, keyAuth)!=SCARD.S_SUCCESS)
            {
                MessageBox.Show("erreur authentification");
            }
            else MessageBox.Show("Test");
            
        }
    }
}

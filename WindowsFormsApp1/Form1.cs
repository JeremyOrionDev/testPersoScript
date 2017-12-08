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
using System.Runtime.InteropServices;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        [DllImport("ctacs.dll")]
        static extern int ACSdll();
        
        SCardReader theReader = null;
        public SCardShareMode selShareMode;
        private SCardProtocol selProtocol;
        SCardContext hContext = new SCardContext();
        public Form1()
        {
            
            InitializeComponent();
            hContext.Establish(SCardScope.System);
            readerList();
        }
        void readerList()
        {
            string[] szReaders = hContext.GetReaders();
            if (szReaders.Length <= 0)
            {
                throw new PCSCException(SCardError.NoReadersAvailable, "Aucun lecteur trouvé");
            }
            foreach (var item in szReaders)
            {
                cbReaders.Items.Add(item);
            }

        }
        public void test()
        {

            void CheckError(SCardError Error)
            {
                if (Error != SCardError.Success)
                {
                    throw new PCSCException(Error, SCardHelper.StringifyError(Error));
                }
            }
            try
            {
                

                SCardReader reader = new SCardReader(hContext);

                SCardError err = reader.Connect(cbReaders.SelectedItem.ToString(), SCardShareMode.Shared, SCardProtocol.T1);
                CheckError(err);
                MessageBox.Show("etat connexion lecteur: "+"\n"+err);
                IntPtr pioSendPci;
                switch (reader.ActiveProtocol)
                {
                    case SCardProtocol.T0:
                        pioSendPci = SCardPCI.T0;
                        break;
                    case SCardProtocol.T1:
                        pioSendPci = SCardPCI.T1;
                        break;
                    default:
                        throw new PCSCException(SCardError.ProtocolMismatch, "procotol non supporté" + reader.ActiveProtocol.ToString());
                }

                byte[] pbRecvBuffer = new byte[8];

                //Send select command
                byte[] cmd1 = new byte[] { 0xFF, 0x82, 0x41, 0x00, 0x06, 0x12, 0x12, 0x12, 0x12, 0x12, 0x12};

                err = reader.Transmit(SCardPCI.T1, cmd1, ref pbRecvBuffer);
                CheckError(err);

                string X = "";
                for (int i = 0; i < pbRecvBuffer.Length; i++)
                {
                    X += pbRecvBuffer[i].ToString("X2") + "-";
                }
                MessageBox.Show("pbRecvBuffer = " + X);
            }
            catch (Exception ex)
            {

                MessageBox.Show("erreur: " + "\n" + ex.Message+"\n"+ex); ;
            }
        }

        public void prepare()
        {
            comboShareMode.DataSource = Enum.GetNames(typeof(SCardShareMode));
            comboProtocols.DataSource = Enum.GetNames(typeof(SCardProtocol));
            foreach (var item in Enum.GetNames(typeof(SCardProtocol)))
            {
                string txt = "";
                if (item.Length>txt.Length)
                {
                    txt = item;
                }
                Font X = comboProtocols.Font;
                comboProtocols.Width = TextRenderer.MeasureText(txt, X).Width+30;
            }
            foreach (var item in Enum.GetNames(typeof(SCardShareMode)))
            {
                string txt = "";
                if (item.Length > txt.Length)
                {
                    txt = item;
                }
                Font X = comboProtocols.Font;
                comboShareMode.Width = TextRenderer.MeasureText(txt, X).Width + 30;
            }
            
        }

       

        private void comboShareMode_SelectedValueChanged(object sender, EventArgs e)
        {
           
            switch (comboShareMode.SelectedIndex)
            {
                case 0:
                    selShareMode = SCardShareMode.Direct;
                    break;
                case 1:
                    selShareMode = SCardShareMode.Exclusive;
                    break;
                case 2:
                    selShareMode = SCardShareMode.Shared;
                    break;

                default:
                    break;
            }
        }

        private void comboProtocols_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboProtocols.SelectedIndex)
            {

                case 0:
                    selProtocol = SCardProtocol.Unset;
                    break;
                case 1:
                    selProtocol = SCardProtocol.T0;
                    break;
                case 2:
                    selProtocol = SCardProtocol.T1;
                    break;
                case 3:
                    selProtocol = SCardProtocol.Any;
                    break;
                case 4:
                    selProtocol = SCardProtocol.Raw;
                    break;
                case 5:
                    selProtocol = SCardProtocol.T15;
                    break;

                default:
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            test();
        }
    }
}

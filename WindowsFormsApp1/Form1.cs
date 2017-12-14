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
using AtlanticZeiser.CpiPc.Scripting.CSharp;
using AtlanticZeiser.CpiPc.Reader.Smartware;
using AtlanticZeiser.CpiPc.Reader.Smartware.Cards;
using AtlanticZeiser.CpiPc.Scripting.Compiler;
using AtlanticZeiser.CpiPc.Tools.Misc;
using AtlanticZeiser.CpiPc.Scripting.Converter;
using AtlanticZeiser.CpiPc.Scripting.Xml.Mifare;
using System.Security.Cryptography;
using AtlanticZeiser.CpiPc.Scripting.Xml.Iso7816;
using System.Runtime.InteropServices;



namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        
        SCardShareMode cardShareMode = new SCardShareMode();
        SCardProtocol cardProtocol = new SCardProtocol();
        [DllImport("winscard.dll")]
        static extern int SCardEstablishContext(uint dwScope, IntPtr pvReserved1, IntPtr pvReserved2, out IntPtr phContext);
        byte[] pbRecvBuffer = new byte[256];
        SCardError err;
        SCardReader theReader = null;
        SCardReader SAM = null;
        CSharpScriptProvider provider;
        CardData card;
        SCardContext hContext = new SCardContext();
        int CardID;

        public Form1()
        {
            InitializeComponent();
            rondVert.ImageLocation = @"C:\Users\jeremy\Downloads\button-green.png";
            panelConnexion.Visible = false;
            rond_rouge.ImageLocation = @"C:\Users\jeremy\Downloads\circle-red.png";
            hContext.Establish(SCardScope.System);
            readerList();
        }

        void readerList()
        {
            
            cBxProtocol.Items.AddRange(new string[] { "T0", "Any", "T1", "T15", "Unset", "Raw" });
            cBxShareMode.Items.AddRange(new string[] { "Direct", "Exclusive", "Shared" });
            if (cbReaders.SelectedIndex == -1)
            {
                lblSelectReader.Visible = true;
            }
            else lblSelectReader.Visible = false;
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
        ErrorProvider err2=new ErrorProvider();
        public void readerConnect()
        {
            byte[] getATR = new byte[] { };
            string value = cbReaders.SelectedItem.ToString();
            switch (value)
            {
                case "ACS CCID USB Reader 0":
                    getATR = new byte[] { 0x94, 0x84, 0x00, 0x00, 0x08 };
                    break;
                case "Duali DE-620 Contact Reader 0":
                    getATR = new byte[] { 0x94, 0xBE, 0x00, 0x00, 0x13 };
                    break;
            }
            
            switch (cBxProtocol.SelectedIndex)
            {
                case 0:
                    cardProtocol = SCardProtocol.T0;
                    break;
                case 1:
                    cardProtocol = SCardProtocol.Any;
                    break;
                case 2:
                    cardProtocol = SCardProtocol.T1;
                    break;
                case 3:
                    cardProtocol = SCardProtocol.T15;
                    break;
                case 4:
                    cardProtocol = SCardProtocol.Unset;
                    break;
                case 5:
                    cardProtocol = SCardProtocol.Raw;
                    break;          
            }
            switch (cBxShareMode.SelectedIndex)
            {
                case 0:
                    //MessageBox.Show("ShareMode Direct");
                    cardShareMode = SCardShareMode.Direct;
                    break;
                case 1:
                    //MessageBox.Show("ShareMode Exclusive");
                    cardShareMode = SCardShareMode.Exclusive;
                    break;
                case 2:
                    //MessageBox.Show("ShareMode Shared");
                    cardShareMode = SCardShareMode.Shared;
                    break;
            }
            panelConnexion.Visible = true;
            theReader = new SCardReader(hContext);
            SAM = new SCardReader(hContext);
            err= theReader.Connect(cbReaders.SelectedItem.ToString(), cardShareMode, cardProtocol);
            if (err == SCardError.Success)
            {
                rondVert.Visible = true;
                rond_rouge.Visible = false;
                ATR(getATR);
            }
            else
            {
                rond_rouge.Visible = true;
                rondVert.Visible = false;
            }
        }

        
        public void ATR(byte[] atrByte)
        {


            err = theReader.Transmit(SCardPCI.T0, atrByte, ref pbRecvBuffer);
            CheckError(err);
            if (err==SCardError.Success)
            {
                
                string X = "";
                for (int i = 0; i < pbRecvBuffer.Length; i++)
                {
                    X += pbRecvBuffer[i].ToString("X2") ;
                }
                if (X.Length>10)
                {
                    X = X.Substring(0, X.Length - 4);
                }
                string atrAffich = "";
                for (int i = 0; i < X.Length; i=i+2)
                {
                    atrAffich +=  X.Substring(i, 2);
                }
                CardID = Convert.ToInt32(X.Substring(24, 8));
       
                txBxATR.Text = atrAffich;
                Font F = DefaultFont;
                txBxATR.Width = atrAffich.Length * 6;
                txBxCardID.Text = CardID.ToString("D8");

                getApp();
            }
        }
        public void getApp()
        {
            // AID 33 4D 54 52 2E 49 43 41 00 00 00 00 00 00 00 00h
            err = theReader.Transmit(SCardPCI.T0, new byte[] { 0x94, 0xA4, 0x04, 0x00, 0x08, 0x33,0x4D,0x54,0x52,0x2E,0x49, 0x43, 0x41, 0x00}, ref pbRecvBuffer);
            if (err==SCardError.Success)
            {
                string app = "";
                for (int i = 0; i < pbRecvBuffer.Length; i++)
                {
                    app += pbRecvBuffer[i].ToString("X2");
                }
                MessageBox.Show("resultat app"+"\n"+app);
            }
        }
        public void getChallenge(int serial)
        {
            err=SAM.Connect("ACS CCID USB Reader 0", SCardShareMode.Exclusive, SCardProtocol.T0);
            CheckError(err);
            //byte[] cmd=new byte[] {}
            //err=SAM.Transmit()
        }
        void CheckError(SCardError Error)
        {
            if (Error != SCardError.Success)
            {
                throw new PCSCException(Error, SCardHelper.StringifyError(Error));
            }
        }
        public void test()
        {

            try
            {
                IntPtr pioSendPci;

                //Send select command
                #region groupe1
                // byte[] cmd1 = new byte[] { 0x94, 0x1C, 0x00, 0x00, 0x50, 0x74, 0x84, 0x1F, 0x19, 0xBE, 0xF7, 0x3A, 0x88, 0x02, 0x8B, 0x7B, 0x1C, 0xDE, 0xCE, 0xAB, 0x57, 0x57, 0xE3, 0x30, 0xEA, 0x18, 0xCD, 0xC8, 0x9E, 0x6A, 0xBB, 0x96, 0xD0, 0xCC, 0x88, 0x74, 0x3C, 0xD1, 0xF3, 0x5C, 0x66, 0x0C, 0xD3, 0x82, 0xE5, 0x09, 0x9D, 0x65, 0x6A, 0x7F, 0xD8, 0xF2, 0x4D, 0xDD, 0xED, 0x2F, 0x14, 0xA2, 0x5F, 0xED, 0x62, 0x00, 0xF4, 0x99, 0x3D, 0x06, 0x35, 0x20, 0x58, 0xB4, 0x8E, 0x71, 0xE6, 0x4C, 0xE3, 0xD6, 0xD0, 0x69, 0x1B, 0xB0, 0x46, 0x73, 0xAD, 0x02, 0x34 };
                // byte[] cmd2 = new byte[] { 0x94, 0x1C, 0x00, 0x00, 0x10, 0xD9, 0x81, 0xFE, 0xEF, 0x90, 0x62, 0x67, 0x20, 0xA8, 0x94, 0x32, 0x52, 0xD5, 0xD6, 0x88, 0x33 };
                //NN NN NN NN NN NN NN NN
                //94 1E 00 00 27 00 00 00 71
                // byte[] cmd3 = new byte[] { 0x94, 0x1E, 0x00 , 0x00, 0x27 , 0x00, 0x00, 0x00,  0X71, 0x01 , 0x01 , 0x01 ,0x01,0x01,0x10, 0x10,0x20, 0x02 , 0x50, 0x18, 0x00 , 0x11 , 0x02 , 0x50, 0x01, 0x8E, 0x07, 0x01, 0x69, 0xF5, 0xFD, 0xEA, 0x7E, 0x44, 0x7E, 0xA4, 0x6E, 0x4D, 0x3A, 0x28, 0x69, 0x1E, 0x4D, 0x0D };

                #endregion
                #region groupe2
                byte[] cmd1 = new byte[] { 0x94, 0x1C, 0x00, 0x00, 0x50, 0xE6, 0xF5, 0x95, 0x66, 0x52, 0xF3, 0xE0, 0xFA, 0xC8, 0x29, 0x50, 0xA2, 0xBD, 0x59, 0x68, 0xFE, 0xF6, 0xBD, 0xB2, 0x31, 0x02, 0xAC, 0x0E, 0x39, 0xF5, 0x70, 0x6C, 0xF2, 0xBF, 0x58, 0x88, 0x8E, 0x2F, 0x0D, 0x21, 0x15, 0xBA, 0x67, 0x05, 0xAA, 0xC5, 0x99, 0xC4, 0x42, 0x59, 0xE4, 0x54, 0x73, 0x17, 0xBA, 0xE9, 0x12, 0xCD, 0x96, 0x79, 0xAD, 0x35, 0x0D, 0x73, 0x48, 0x09, 0x4D, 0x11, 0x97, 0x3C, 0x9A, 0x06, 0x1A, 0x67, 0xD7, 0x00, 0xCF, 0xA3, 0xF5, 0x0D, 0xA0, 0x05, 0x86, 0x15, 0x43 };
                //byte[] cmd2 = new byte[] { 0x94, 0x1C, 0x00, 0x00, 0x10, 0xF7, 0xDB, 0x48, 0x1D, 0x9D, 0x17, 0x11, 0x5D, 0xDE, 0xFC, 0xE7, 0xFE, 0xFF, 0xEC, 0x85, 0x9C };
                //byte[] cmd3 = new byte[] { 0x94, 0x1E, 0x00, 0x00, 0x27, 0x00, 0x00, 0x00, 0X71, 0x99, 0x99, 0x99, 0x99, 0x99, 0x99, 0x99, 0x99, 0x02, 0x50, 0x28, 0x00, 0x11, 0x02, 0x50, 0x08, 0x8E, 0x12, 0x08, 0xB5, 0x67, 0x93, 0x07, 0xE8, 0x6A, 0x44, 0x40, 0xE5, 0xA3, 0xDA, 0xBA, 0xDA, 0x2E, 0x69, 0xF6 };
                #endregion


                err = theReader.Transmit(SCardPCI.T0, cmd1, ref pbRecvBuffer);

                CheckError(err);
                string X = "";
                for (int i = 0; i < pbRecvBuffer.Length; i++)
                {
                    X += pbRecvBuffer[i].ToString("X2") + "-";
                }
                MessageBox.Show("pbRecvBuffer = " + X);
                
                
            }
            catch (PCSCException ex)
            {

                MessageBox.Show("erreur: " + "\n" + ex.Message); ;
            }
        }



       




        private void button1_Click(object sender, EventArgs e)
        {
            SCardProtocol SP = new SCardProtocol();

            readerConnect();
        }

        private void btn_Get_MF_Click(object sender, EventArgs e)
        {
            byte[] cmd1 = new byte[] { 0x94, 0xA4, 0x04, 0x00, 0x0B, 0xA0, 0x00, 0x00, 0x02, 0x91, 0xD0, 0x12, 0x00, 0x08, 0x90, 0x01 };
            err = theReader.Transmit(SCardPCI.T0, cmd1, ref pbRecvBuffer);

            CheckError(err);
            string X = "";
            for (int i = 0; i < pbRecvBuffer.Length; i++)
            {
                X += pbRecvBuffer[i].ToString("X2") + "-";
            }
            MessageBox.Show("result: "+"\n"+X);
        }

        private void cbReaders_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblSelectReader.Visible = false;
        }
        private void structure2()
        {
            byte[] cmd1 = new byte[] { 0x94, 0x1C, 0x00, 0x00, 0x50, 0x80, 0x28, 0xB8, 0x79, 0xDC, 0x53, 0xC0, 0x1A, 0x48, 0xFC, 0x8F, 0x24, 0x3B, 0x35, 0x31, 0x85, 0x23, 0xB2, 0xF7, 0x53, 0x83, 0xB7, 0xD4, 0x12, 0x85, 0x93, 0x98, 0x5A, 0xA6, 0x40, 0xBC, 0x47, 0xF2, 0xD7, 0x1C, 0x05, 0x0D, 0xB9, 0xFA, 0xCE, 0x6F, 0x32, 0xF1, 0xC7, 0x9D, 0x47, 0xAB, 0x8C, 0xDF, 0x65, 0x20, 0x5B, 0x1F, 0x31, 0x48, 0xF9, 0x51, 0x72, 0x90, 0x95, 0xED, 0xA7, 0x74, 0x98, 0xA0, 0x1B, 0x7F, 0x62, 0x35, 0x42, 0xE2, 0x4F, 0x47, 0xDD, 0x8B, 0x6E, 0x34, 0xB2, 0x59, 0x74 },
            cmd2 = new byte[] { 0x94, 0x1C, 0x00, 0x00, 0x10, 0x2D, 0xDA, 0x5A, 0x47, 0xAD, 0xA1, 0x7F, 0xAA, 0x32, 0xE8, 0x56, 0xDF, 0xB6, 0xD8, 0x86, 0x8B },
            cmd3 = new byte[] { 0x94, 0x1E, 0x00, 0x00, 0x27, 0x00, 0x00, 0x00, 0x71, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x02, 0x50, 0x28, 0x00, 0x11, 0x02, 0x50, 0x02, 0x8E, 0x07, 0x02, 0xBF, 0x45, 0xE3, 0xF3, 0x20, 0x0F, 0x5B, 0xB2, 0xE8, 0xDD, 0xFE, 0x45, 0xCA, 0x09, 0x05, 0xB5 };
            err = theReader.Transmit(SCardPCI.T0, cmd1, ref pbRecvBuffer);
            string X = "";
            for (int i = 0; i < pbRecvBuffer.Length; i++)
            {
                X += pbRecvBuffer[i].ToString("X2") + "-";
                

    }

    MessageBox.Show("result: " + "\n" + X);
            if (err==SCardError.Success)
            {
                err = theReader.Transmit(SCardPCI.T0, cmd2, ref pbRecvBuffer);
                string Y = "";
                for (int i = 0; i < pbRecvBuffer.Length; i++)
                {
                    Y += pbRecvBuffer[i].ToString("X2") + "-";
                }
                MessageBox.Show("result: " + "\n" + Y);
                if (err==SCardError.Success)
                {
                    err = theReader.Transmit(SCardPCI.T0, cmd3, ref pbRecvBuffer);
                    string z = "";
                    for (int i = 0; i < pbRecvBuffer.Length; i++)
                    {
                        z += pbRecvBuffer[i].ToString("X2") + "-";
                    }
                    MessageBox.Show("result: " + "\n" + z);
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            byte[] cmd = new byte[] { 0x94, 0xA4, 0x04, 0x00, 0x0B, 0xA0, 0x00, 0x00, 0x02, 0x91, 0xD0, 0x12, 0x00, 0x08, 0x90, 0x01 };
    
            err = theReader.Transmit(SCardPCI.T0, cmd, ref pbRecvBuffer);

            CheckError(err);
            if (err==SCardError.Success)
            {
                err = theReader.Transmit(SCardPCI.T0, cmd, ref pbRecvBuffer);
                string X = "";
                for (int i = 0; i < pbRecvBuffer.Length; i++)
                {
                    X += pbRecvBuffer[i].ToString("X2") + "-";
                }
                MessageBox.Show("result: " + "\n" + X);

            }
        }

        private void btn_Get_Memory_Click(object sender, EventArgs e)
        {
            byte[] cmd = new byte[] { 0x94, 0xE1,0x00,0x02,0x02};
            pbRecvBuffer = new byte[256];
            err = theReader.Transmit(SCardPCI.T0, cmd, ref pbRecvBuffer);
            CheckError(err);
            string Y = "";
            for (int i = 0; i < pbRecvBuffer.Length; i++)
            {
                Y += pbRecvBuffer[i].ToString("X2") + "/";
            }
            MessageBox.Show("résultat de la lecture EEPROM: "+"\n"+Y);
        }

        private void btnCreationStructure2_Click(object sender, EventArgs e)
        {
            structure2();
        }

        private void cBxProtocol_SelectedIndexChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("protocole selected index:"+cBxProtocol.SelectedIndex);
        }

        private void cBxShareMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("Share Mode:"+cBxShareMode.SelectedIndex);
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(cbReaders.SelectedItem.ToString());
        }

        private void btnDeconnecte_Click(object sender, EventArgs e)
        {
            err=theReader.Disconnect(SCardReaderDisposition.Eject);
            txBxATR.Clear();
        }
    }
}

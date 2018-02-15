using PCSC;
using PCSC.Iso7816;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    class Iso1443B
    {
        private Form1 formAffich;
        [DllImport("winscard.dll")]
        static extern int SCardEstablishContext(uint dwScope, IntPtr pvReserved1, IntPtr pvReserved2, out IntPtr phContext);

        static SCardShareMode cardShareMode = new SCardShareMode();
        static SCardProtocol cardProtocol = new SCardProtocol();

        byte[] pbRecvBuffer = new byte[256];

        static SCardReader isoReader;

        static SCardContext hContext = new SCardContext();

        public static void establishContext()
        {
            hContext.Establish(SCardScope.System);
        }

        public static void iso1443_ID(Form1 form)
        {
            var contextFactory = ContextFactory.Instance;

            var reader = form.cbReaders.SelectedItem.ToString();

            using(var context= contextFactory.Establish(SCardScope.System))
            {

                using (isoReader = new SCardReader(context))
                {
                    var sc = isoReader.Connect(reader, SCardShareMode.Exclusive, SCardProtocol.T1);

                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("Reader connection error" + "\n" + SCardHelper.StringifyError(sc));

                    }
                    
                    var apdu = new CommandApdu(IsoCase.Case2Extended, isoReader.ActiveProtocol)
                    {
                        CLA = 0xFF,
                        INS = 0xCA,
                        P1 = 0x00,
                        P2 = 0x00,
                        Le=0x10
                    };
                    sc = isoReader.BeginTransaction();
                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("Erreur lors de l'initialisation de la transaction" + "\n" + SCardHelper.StringifyError(sc));
                    }

                    var command = apdu.ToArray();

                    var receivePCI = new SCardPCI();
                    var sendPCI = SCardPCI.GetPci(isoReader.ActiveProtocol);
                    var receiveBuffer = new byte[16];
                    sc = isoReader.Transmit(sendPCI, command, receivePCI, ref receiveBuffer);
                    string X="";

                    var getRep = new CommandApdu(IsoCase.Case1, isoReader.ActiveProtocol)
                    {
                        CLA = 0xFF,
                        Instruction = InstructionCode.GetResponse,
                        P1P2 = 00
                    };
                    sc = isoReader.BeginTransaction();
                    command = getRep.ToArray();
                    receivePCI = new SCardPCI();
                    sendPCI = SCardPCI.GetPci(isoReader.ActiveProtocol);
                    receiveBuffer = new byte[256];
                    sc = isoReader.Transmit(sendPCI, command, receivePCI, ref receiveBuffer);

                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("Erreur de get rep" + "\n" + SCardHelper.StringifyError(sc));
                    }
                    receiveBuffer = new byte[256];


                    var apduReponse = new ResponseApdu(receiveBuffer, IsoCase.Case2Extended, SCardProtocol.T1);
                    var e = 2;
                    receiveBuffer = new byte[256];
                    string UX = "";
                    if (apduReponse.HasData)
                    {
                        for (int i = 0; i < apduReponse.GetData().Length; i++)
                        {
                            X += apduReponse.GetData()[i].ToString("X2");
                        }
                    }
                    MessageBox.Show(X);
                    
                }
            }
        }
    }
}

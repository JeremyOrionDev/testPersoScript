using System;
using System.Collections.Generic;
using PCSC;
using PCSC.Iso7816;

namespace Transmit
{
    public class Program
    {
        public static void Main() {
            var contextFactory = ContextFactory.Instance;
            using (var context = contextFactory.Establish(SCardScope.System)) {

                var readerNames = context.GetReaders();
                if (NoReaderFound(readerNames)) {
                    Console.WriteLine("You need at least one reader in order to run this example.");
                    Console.ReadKey();
                    return;
                }

                var readerName = ChooseRfidReader(readerNames);
                if (readerName == null) {
                    return;
                }

                // 'using' statement to make sure the reader will be disposed (disconnected) on exit
                using (var rfidReader = new SCardReader(context)) {
                    var sc = rfidReader.Connect(readerName, SCardShareMode.Shared, SCardProtocol.Any);
                    if (sc != SCardError.Success) {
                        Console.WriteLine("Could not connect to reader {0}:\n{1}",
                            readerName,
                            SCardHelper.StringifyError(sc));
                        Console.ReadKey();
                        return;
                    }

                    var apdu = new CommandApdu(IsoCase.Case2Short, rfidReader.ActiveProtocol) {
                        CLA = 0xFF,
                        Instruction = InstructionCode.GetData,
                        P1 = 0x00,
                        P2 = 0x00,
                        Le = 0 // We don't know the ID tag size
                    };

                    sc = rfidReader.BeginTransaction();
                    if (sc != SCardError.Success) {
                        Console.WriteLine("Could not begin transaction.");
                        Console.ReadKey();
                        return;
                    }

                    Console.WriteLine("Retrieving the UID .... ");

                    var receivePci = new SCardPCI(); // IO returned protocol control information.
                    var sendPci = SCardPCI.GetPci(rfidReader.ActiveProtocol);

                    var receiveBuffer = new byte[256];
                    var command = apdu.ToArray();

                    sc = rfidReader.Transmit(
                        sendPci, // Protocol Control Information (T0, T1 or Raw)
                        command, // command APDU
                        receivePci, // returning Protocol Control Information
                        ref receiveBuffer); // data buffer

                    if (sc != SCardError.Success) {
                        Console.WriteLine("Error: " + SCardHelper.StringifyError(sc));
                    }

                    var responseApdu = new ResponseApdu(receiveBuffer, IsoCase.Case2Short, rfidReader.ActiveProtocol);
                    Console.Write("SW1: {0:X2}, SW2: {1:X2}\nUid: {2}",
                        responseApdu.SW1,
                        responseApdu.SW2,
                        responseApdu.HasData ? BitConverter.ToString(responseApdu.GetData()) : "No uid received");

                    rfidReader.EndTransaction(SCardReaderDisposition.Leave);
                    rfidReader.Disconnect(SCardReaderDisposition.Reset);

                    Console.ReadKey();
                }
            }
        }

        private static string ChooseRfidReader(IList<string> readerNames) {
            // Show available readers.
            Console.WriteLine("Available readers: ");
            for (var i = 0; i < readerNames.Count; i++) {
                Console.WriteLine("[" + i + "] " + readerNames[i]);
            }

            // Ask the user which one to choose.
            Console.Write("Which reader is an RFID reader? ");
            var line = Console.ReadLine();
            int choice;

            if (int.TryParse(line, out choice) && (choice >= 0) && (choice <= readerNames.Count)) {
                return readerNames[choice];
            }

            Console.WriteLine("An invalid number has been entered.");
            Console.ReadKey();
            return null;
        }

        private static bool NoReaderFound(ICollection<string> readerNames) {
            return readerNames == null || readerNames.Count < 1;
        }
    }
}

/*
 * var contextFactory = ContextFactory.Instance;
            using (var ctx = contextFactory.Establish(SCardScope.System))
            {
                byte[] Byte = createByte(input);
                using (var Sam = new SCardReader(ctx))
                {

                    var sc = Sam.Connect("ACS CCID USB Reader 0", SCardShareMode.Exclusive, SCardProtocol.T0);
                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("erreur de connexion au lecteur SAM" + "\n" + SCardHelper.StringifyError(sc));
                    }

                    var apdu = new CommandApdu(IsoCase.Case3Extended, Sam.ActiveProtocol)
                    {
                        CLA = 0x80,
                        Instruction = InstructionCode.InternalAuthenticate,
                        P1 = 0x00,
                        P2 = 0x00,
                        Data = Byte
                    };

                    sc = Sam.BeginTransaction();

                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("erreur lors de la transaction "+"\n"+"L424" + "\n" + SCardHelper.StringifyError(sc));
                        
                    };

                    var receivePci = new SCardPCI();
                    var sendPci = SCardPCI.GetPci(Sam.ActiveProtocol);
                    var receiveBuffer = new byte[256];
                    var command = apdu.ToArray();
                    
                    sc = Sam.Transmit( command, ref receiveBuffer);

                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("erreur dans la transmission de la commande au lecteur SAM" + "\n" + "L 437");
                       
                    }

                    var getRep = new CommandApdu(IsoCase.Case2Extended, SCardProtocol.T0)
                    {
                        CLA = 0x94,
                        Instruction = InstructionCode.GetResponse,
                        P1 = 0x00,
                        P2 = 0x00

                    };
                    sc = Sam.BeginTransaction();
                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("erreur lors de la transaction : " + "\n" + SCardHelper.StringifyError(sc));
                       
                    }

                    receivePci = new SCardPCI();
                    sendPci = SCardPCI.GetPci(Sam.ActiveProtocol);
                    receiveBuffer = new byte[256];
                    command = getRep.ToArray();
                    sc = Sam.Transmit(sendPci, command, receivePci, ref receiveBuffer);

                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("erreur");
                        return false;
                    }
                    else return true;
                }
 * 
*/

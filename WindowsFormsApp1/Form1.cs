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
        [DllImport("winscard.dll")]
        static extern int SCardEstablishContext(uint dwScope, IntPtr pvReserved1, IntPtr pvReserved2, out IntPtr phContext);
        // déclaration des variables pour les mode de partage et protocole
        SCardShareMode cardShareMode = new SCardShareMode();
        SCardProtocol cardProtocol = new SCardProtocol();
        // déclaration du buffer de réception sur 256 bytes
        byte[] pbRecvBuffer = new byte[256];
        //déclaration de la constante d'erreur 
        SCardError err;
        string chalenge = "";
        // déclaration des lecteur carte contact et SAM
        SCardReader theReader = null;
        SCardReader SAM = null;
        //création du context à partir de la librairie
        SCardContext hContext = new SCardContext();
        int CardID;
        string send;
        public Form1()
        {
            InitializeComponent();           
            hContext.Establish(SCardScope.System);
            readerList();
        }

        void readerList()
        {
            var contextFactory = ContextFactory.Instance;
            using (var context = contextFactory.Establish(SCardScope.System))
            {
                var readerNames = context.GetReaders();
                foreach (var item in readerNames)
                {
                    cbReaders.Items.Add(item);
                }
            }
            //Ajout des items dans les comboBox
            cBxProtocol.Items.AddRange(new string[] { "T0", "Any", "T1", "T15", "Unset", "Raw" });
            cBxShareMode.Items.AddRange(new string[] { "Direct", "Exclusive", "Shared" });
            //si aucun lecteur n'est sélectionné le label demandant la sélection d'un lecteur apparaît
            if (cbReaders.SelectedIndex == -1)
            {
                lblSelectReader.Visible = true;
            }
            else lblSelectReader.Visible = false;
            //récupération des lecteurs connectés
            string[] szReaders = hContext.GetReaders();
            if (szReaders.Length <= 0)
            {
                throw new PCSCException(SCardError.NoReadersAvailable, "Aucun lecteur trouvé");
            }


        }
        /// <summary>
        /// fonction de connection au lecteur et récupération de l'ATR
        /// préparation des protocoles d'échanges et mode de transfert
        /// </summary>
        public void readerConnect()
        {
            byte Cla, Ins, p1, p2, le = new byte();
            Cla=Ins=p1=p2=0x00;
            string value = cbReaders.SelectedItem.ToString();
            switch (value)
            {
                case "ACS CCID USB Reader 0":

                    Cla = 0x94;
                    Ins = 0x84;
                    p1= 0x00;
                    p2 = 0x00;
                    le=0x08 ;
                    break;
                case "Duali DE-620 Contact Reader 0":

                    Cla = 0x94;
                    Ins = 0xBE;
                    p1= 0x00;
                    p2 = 0x00;
                    le= 0x13 ;
                    break;
            }
            //switch du protocol en fonction de l'item choisi en comboBox
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
            //switch du mode de partage en fonction de l'item sélectionné en comboBox
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

            //déclaration des lecteur en liaison avec le context
            theReader = new SCardReader(hContext);
            SAM = new SCardReader(hContext);
            var contextFactory = ContextFactory.Instance;
            var reader = cbReaders.SelectedText;
            using (var context = contextFactory.Establish(SCardScope.System)){

                using (var readerContact = new SCardReader(context))
                {
                    var sc = readerContact.Connect(cbReaders.SelectedItem.ToString(), cardShareMode, cardProtocol);
                    if (sc!=SCardError.Success)
                    {
                        MessageBox.Show("Reader connection error");
                    }
#region getATR
                    var aTR = new CommandApdu(IsoCase.Case2Extended, readerContact.ActiveProtocol)
                    {
                        //0x94, 0xBE, 0x00, 0x00, 0x13
                        CLA = Cla,
                        INS=Ins,
                        P1=p1,
                        P2=p2,                       
                        Le=le 

                    };

                    sc = readerContact.BeginTransaction();
                    if (sc!=SCardError.Success)
                    {
                        MessageBox.Show("erreur lors de la transaction"+"\n"+SCardHelper.StringifyError(sc));
                    }

                    var receivePci = new SCardPCI();
                    var sendPci = SCardPCI.GetPci(readerContact.ActiveProtocol);

                    var receiveBuffer = new byte[256];
                    var command = aTR.ToArray();
                    sc = readerContact.Transmit(sendPci, command, receivePci, ref receiveBuffer);

                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("erreur lors de l'envoi de la commande" + "\n" + SCardHelper.StringifyError(sc));
                    }

                    var apduResponse = new ResponseApdu(receiveBuffer, IsoCase.Case2Extended, SCardProtocol.T1);
       
                    if (!string.IsNullOrEmpty(apduResponse.GetData().ToString()))
                    {
                        byte[] incomingData = apduResponse.GetData();
                        string X = "";
                        for (int i = 0; i < incomingData.Length; i++)
                        {
                            X += incomingData[i].ToString("X2");
                        }
                        X = X.Substring(0, X.Length - 4);
                        txBxATR.Text = X;
                    }
                    #endregion
#region applicationSelect
                    var apdu = new CommandApdu(IsoCase.Case4Extended, SCardProtocol.T1)
                    {
                        CLA = 0x00,
                        INS = 0xA4,
                        P1 = 0x04,
                        P2 = 0x00,
                        Data = new byte[] { 0x31, 0x54, 0x49, 0x43, 0x2E, 0x49, 0x43, 0x41},
                        Le = 0x24

                    };
                    
                    sc = readerContact.BeginTransaction();
                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("Erreur lors de la transaction" + "\n" + SCardHelper.StringifyError(sc));
                        return;
                    }

                    var ReceivePci = new SCardPCI();
                    var SendPci = SCardPCI.GetPci(readerContact.ActiveProtocol);

                    receiveBuffer = new byte[256];
                    command = apdu.ToArray();

                    sc = readerContact.Transmit(SendPci, command, ReceivePci, ref receiveBuffer);

                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("Erreur lors de la transmission de la commande" + "\n" + SCardHelper.StringifyError(sc));

                    }
                    
                    apduResponse = new ResponseApdu(receiveBuffer, IsoCase.Case2Extended,SCardProtocol.T1);
                    

                    var getRep = new CommandApdu(IsoCase.Case2Extended, SCardProtocol.T0)
                    {
                        CLA = 0x94,
                        Instruction=InstructionCode.GetResponse,
                        P1 = 0x00,
                        P2 = 0x00,                        
                        Le = 0x24

                    };
                    sc = readerContact.BeginTransaction();
                    if (sc!=SCardError.Success)
                    {
                        MessageBox.Show("erreur lors de la transaction : "+"\n"+SCardHelper.StringifyError(sc));
                    }

                    ReceivePci = new SCardPCI();
                    sendPci = SCardPCI.GetPci(readerContact.ActiveProtocol);
                    receiveBuffer = new byte[256];
                    command=getRep.ToArray();
                    sc = readerContact.Transmit(sendPci, command, ReceivePci, ref receiveBuffer);

                    if (sc!=SCardError.Success)
                    {
                        MessageBox.Show("erreur");
                    }

                    apduResponse = new ResponseApdu(receiveBuffer, IsoCase.Case2Short, readerContact.ActiveProtocol);
                    string app="";
                    if (apduResponse.HasData)
                    {
                        var data = apduResponse.GetData();
                        for (int i = 0; i < data.Length; i++)
                        {
                            app += data[i].ToString("X2");
                        }
                        
                    }
                    app = app.Substring(0, app.Length - 4);
                  
                    string AID = app.Substring(8, 16);
                    txBxAID.Text = AID;
                    string num = app.Substring(38, 16);
                    txBxappSN.Text = num;
                    string SN = "";
                    for (int i = 0; i < num.Length; i=i+2)
                    {
                        SN += "-" + num[i] + num[i + 1];
                    }
                    string cmd = "80-14-00-00-08" + num;
                    byte[] Byte = createByte(cmd);
                    #endregion
                    bool diversifierRep= SamDiversifier(num);
                    if (!diversifierRep)
                    {
                        MessageBox.Show("Erreur lors de la création du diversifier");
                    }
                    else MessageBox.Show("diversifier OK");

#region getChallenge
                    apdu = new CommandApdu(IsoCase.Case2Extended, readerContact.ActiveProtocol)
                    {
                        CLA = 0x00,
                        Instruction = InstructionCode.GetChallenge,
                        P1 = 0x00,
                        P2 = 0x00,
                        Le = 0x08
                    };
                    sc = readerContact.BeginTransaction();
                    if (sc!=SCardError.Success)
                    {
                        MessageBox.Show("erreur dans la commande apdu"+SCardHelper.StringifyError(sc));
                    }

                    ReceivePci = new SCardPCI();
                    sendPci = SCardPCI.GetPci(readerContact.ActiveProtocol);
                    receiveBuffer = new byte[256];
                    command = apdu.ToArray();
                    sc = readerContact.Transmit(sendPci, command, ReceivePci, ref receiveBuffer);
                    if (sc!=SCardError.Success)
                    {
                        MessageBox.Show("erreur lors de l'envoi de la commande");
                    }

                    apduResponse = new ResponseApdu(receiveBuffer, IsoCase.Case2Extended, readerContact.ActiveProtocol);
                    
                    if (apduResponse.HasData)
                    {
                        byte[] data = apduResponse.GetData();
                        for (int i = 0; i < data.Length; i++)
                        {
                            if (i==0)
                            {
                                chalenge = data[i].ToString("X2");
                            } else
                            {
                                chalenge +="-"+ data[i].ToString("X2");

                            }
                        }
                        bool SamRandom= giveSAMrandom(chalenge);
                        if (!SamRandom)
                        {
                            MessageBox.Show("Erreur lors de la génération du nombre aléatoire par le SAM");
                        }
                        else
                        {
                            MessageBox.Show("Random OK");
                        }
                    }
                    #endregion
#region ChangeKey
                    string Key =SamGenKey();
                    byte[] byteKey = createByte(Key);
                    apdu = new CommandApdu(IsoCase.Case3Extended, readerContact.ActiveProtocol)
                    {
                        CLA = 0x94,
                        INS = 0xD8,
                        P1 = 0x00,
                        P2=0x01,                        
                        Data=byteKey

                    };

                    sc = readerContact.BeginTransaction();

                    if (sc!=SCardError.Success)
                    {
                        MessageBox.Show("Erreur lors du changement de la clé "+"\n"+SCardHelper.StringifyError(sc)+"\n"+"L373");
                    }
                    else MessageBox.Show("Clé changée n°1");

                    string Key2 = SamGenKey();
                    byte[] byteKey2 = createByte(Key2);
                    apdu = new CommandApdu(IsoCase.Case3Extended, readerContact.ActiveProtocol)
                    {
                        CLA = 0x94,
                        INS = 0xD8,
                        P1 = 0x00,
                        P2 = 0x03,
                        Data = byteKey2

                    };

                    sc = readerContact.BeginTransaction();

                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("Erreur lors du changement de la clé " + "\n" + SCardHelper.StringifyError(sc) + "\n" + "L373");
                    }
                    else MessageBox.Show("Clé changée n°3");

                    #endregion
                    #region getFile
                    var getFile = new CommandApdu(IsoCase.Case4Extended, SCardProtocol.T1)
                    {

                        CLA = 0x94,
                        Instruction = InstructionCode.SelectFile,
                        P1 = 0x02,
                        P2 = 0x00,
                        Le = 0x19,
                        Data = new byte[] { 0x30 ,0x45 ,0x54 ,0x50 ,0x2E ,0x49 ,0x43 ,0x41 }

                    };

                    sc = readerContact.BeginTransaction();
                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("erreur lors de la transaction" + "\n" + SCardHelper.StringifyError(sc));
                    }

                    receivePci = new SCardPCI();
                    sendPci = SCardPCI.GetPci(readerContact.ActiveProtocol);
                    receiveBuffer = new byte[256];
                    command = getFile.ToArray();

                    sc = readerContact.Transmit( command, ref receiveBuffer);

                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("erreur lors de l'envoi de la commande" + "\n" + SCardHelper.StringifyError(sc) + "\n" + "L1190");
                    }

                    getRep = new CommandApdu(IsoCase.Case2Extended, SCardProtocol.T0)
                    {
                        CLA = 0x94,
                        Instruction = InstructionCode.GetResponse,
                        Le=0x19
                    };
                    sc = readerContact.BeginTransaction();
                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("Erreur lors de la transaction" + "\n" + SCardHelper.StringifyError(sc) + "\n" + "L1201");
                    }

                    receivePci = new SCardPCI();
                    sendPci = SCardPCI.GetPci(readerContact.ActiveProtocol);
                    receiveBuffer = new byte[256];
                    command = getRep.ToArray();
                        sc = readerContact.Transmit( command, ref receiveBuffer);
                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("Erreur dans l'envoi de la commande Get Response" + "\n" + SCardHelper.StringifyError(sc) + "\n" + "L1211");
                    }
                    string Sel = "";
                    for (int i = 0; i < receiveBuffer.Length; i++)
                    {
                        Sel += receiveBuffer[i].ToString("X2");
                    }
                                      
                    #endregion
                }



            }
        }

        public  string SamGenKey()
        {
            var contextFactory = ContextFactory.Instance;
            using (var ctx = contextFactory.Establish(SCardScope.System))
            {
                using (var Sam = new SCardReader(ctx))
                {

                    var sc = Sam.Connect("ACS CCID USB Reader 0", SCardShareMode.Exclusive, SCardProtocol.T0);
                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("erreur de connexion au lecteur SAM" + "\n" + SCardHelper.StringifyError(sc));
                    }
                    //x94, 0x12, 0xFF, 0xFF, 0X04, 0x61, 0x41, 0x67, 0x41,0x18

                    var apdu = new CommandApdu(IsoCase.Case3Extended, Sam.ActiveProtocol)
                    {
                        CLA = 0x94,
                        INS = 0x12,
                        P1 = 0xFF,
                        P2 = 0xFF,
                        Data = new byte[] {0x61, 0x41, 0x67, 0x41}
                    };

                    sc = Sam.BeginTransaction();

                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("erreur lors de la transmission de la commande au lecteur SAM" + "\n" + SCardHelper.StringifyError(sc));
                    }

                    var receivePci = new SCardPCI();
                    var sendPci = SCardPCI.GetPci(Sam.ActiveProtocol);
                    var receiveBuffer = new byte[256];
                    var command = apdu.ToArray();

                    sc = Sam.Transmit(command,ref receiveBuffer);

                    if (sc!=SCardError.Success)
                    {
                        MessageBox.Show("Erreur lors de l'envoi de la commande de génération de la clé"+"\n"+SCardHelper.StringifyError(sc)+"\n"+"L405");
                    }

                    var getRep = new CommandApdu(IsoCase.Case2Extended, Sam.ActiveProtocol)
                    {
                        CLA = 0x94,
                        Instruction = InstructionCode.GetResponse,
                        Le = 0x20,
                        P1P2=0
                    };

                    sc = Sam.BeginTransaction();
                    if (sc!=SCardError.Success)
                    {
                        MessageBox.Show("erreur lors de la réception de la réponse du SAM"+"\n"+SCardHelper.StringifyError(sc)+"\n"+"L407");
                    }

                    receivePci = new SCardPCI();
                    sendPci = SCardPCI.GetPci(Sam.ActiveProtocol);
                    receiveBuffer = new byte[256];
                    command = getRep.ToArray();
                    sc = Sam.Transmit(sendPci, command, receivePci, ref receiveBuffer);
                    string Key = "";
                    for (int l = 0; l < receiveBuffer.Length; l++)
                    {
                        if (l==0)
                        {
                            Key = receiveBuffer[l].ToString("X2") ;
                        } else Key += "-"+receiveBuffer[l].ToString("X2");
                    }
                    //MessageBox.Show("X= "+Key);
                    return Key;
                }
            }

            

        }

        public bool SamDiversifier(string input)
        {
            var contextFactory = ContextFactory.Instance;
            using (var ctx = contextFactory.Establish(SCardScope.System))
            {
                byte[] Byte = createByte(input);
                using (var Sam = new SCardReader(ctx))
                {
                    
                    var sc = Sam.Connect("ACS CCID USB Reader 0", SCardShareMode.Exclusive, SCardProtocol.T0);
                    if (sc!=SCardError.Success)
                    {
                        MessageBox.Show("erreur de connexion au lecteur SAM"+"\n"+SCardHelper.StringifyError(sc));
                    }

                    var apdu = new CommandApdu(IsoCase.Case3Extended, SCardProtocol.T0)
                    {
                        CLA = 0x80,
                        INS = 0x14,
                        P1 = 0x00,
                        P2 = 0x00,
                        Data=Byte
                    };

                    sc = Sam.BeginTransaction();

                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("erreur lors de la transmission de la commande au lecteur SAM" + "\n" + SCardHelper.StringifyError(sc));
                        return false;
                    } else return true;
                
                }
            }
        }
        public bool giveSAMrandom(string input)
        {
            var contextFactory = ContextFactory.Instance;
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
            }
           
        }

    
        public byte[] createByte(string input)
        {
           
            
            string[] hexTab = input.Split('-');
            string binString = "";
            for (int i = 0; i < hexTab.Count(); i++)
            {
                binString += HexVersBin(hexTab[i]);
            }
            int numOfBytes = binString.Length / 8;
            byte[] bytes = new byte[numOfBytes];
            for (int i = 0; i < numOfBytes; ++i)
            {
                bytes[i] = Convert.ToByte(binString.Substring(8 * i, 8), 2);
            }
            return bytes;
        }
        public string HexVersBin(string ch_bin)
        {
            string aa;
            ch_bin = ch_bin.ToUpper();
            aa = "";
            switch (ch_bin)
            {
                case "00": aa = "00000000"; break;
                case "01": aa = "00000001"; break;
                case "02": aa = "00000010"; break;
                case "03": aa = "00000011"; break;
                case "04": aa = "00000100"; break;
                case "05": aa = "00000101"; break;
                case "06": aa = "00000110"; break;
                case "07": aa = "00000111"; break;
                case "08": aa = "00001000"; break;
                case "09": aa = "00001001"; break;
                case "0A": aa = "00001010"; break;
                case "0B": aa = "00001011"; break;
                case "0C": aa = "00001100"; break;
                case "0D": aa = "00001101"; break;
                case "0E": aa = "00001110"; break;
                case "0F": aa = "00001111"; break;
                case "10": aa = "00010000"; break;
                case "11": aa = "00010001"; break;
                case "12": aa = "00010010"; break;
                case "13": aa = "00010011"; break;
                case "14": aa = "00010100"; break;
                case "15": aa = "00010101"; break;
                case "16": aa = "00010110"; break;
                case "17": aa = "00010111"; break;
                case "18": aa = "00011000"; break;
                case "19": aa = "00011001"; break;
                case "1A": aa = "00011010"; break;
                case "1B": aa = "00011011"; break;
                case "1C": aa = "00011100"; break;
                case "1D": aa = "00011101"; break;
                case "1E": aa = "00011110"; break;
                case "1F": aa = "00011111"; break;
                case "20": aa = "00100000"; break;
                case "21": aa = "00100001"; break;
                case "22": aa = "00100010"; break;
                case "23": aa = "00100011"; break;
                case "24": aa = "00100100"; break;
                case "25": aa = "00100101"; break;
                case "26": aa = "00100110"; break;
                case "27": aa = "00100111"; break;
                case "28": aa = "00101000"; break;
                case "29": aa = "00101001"; break;
                case "2A": aa = "00101010"; break;
                case "2B": aa = "00101011"; break;
                case "2C": aa = "00101100"; break;
                case "2D": aa = "00101101"; break;
                case "2E": aa = "00101110"; break;
                case "2F": aa = "00101111"; break;
                case "30": aa = "00110000"; break;
                case "31": aa = "00110001"; break;
                case "32": aa = "00110010"; break;
                case "33": aa = "00110011"; break;
                case "34": aa = "00110100"; break;
                case "35": aa = "00110101"; break;
                case "36": aa = "00110110"; break;
                case "37": aa = "00110111"; break;
                case "38": aa = "00111000"; break;
                case "39": aa = "00111001"; break;
                case "3A": aa = "00111010"; break;
                case "3B": aa = "00111011"; break;
                case "3C": aa = "00111100"; break;
                case "3D": aa = "00111101"; break;
                case "3E": aa = "00111110"; break;
                case "3F": aa = "00111111"; break;
                case "40": aa = "01000000"; break;
                case "41": aa = "01000001"; break;
                case "42": aa = "01000010"; break;
                case "43": aa = "01000011"; break;
                case "44": aa = "01000100"; break;
                case "45": aa = "01000101"; break;
                case "46": aa = "01000110"; break;
                case "47": aa = "01000111"; break;
                case "48": aa = "01001000"; break;
                case "49": aa = "01001001"; break;
                case "4A": aa = "01001010"; break;
                case "4B": aa = "01001011"; break;
                case "4C": aa = "01001100"; break;
                case "4D": aa = "01001101"; break;
                case "4E": aa = "01001110"; break;
                case "4F": aa = "01001111"; break;
                case "50": aa = "01010000"; break;
                case "51": aa = "01010001"; break;
                case "52": aa = "01010010"; break;
                case "53": aa = "01010011"; break;
                case "54": aa = "01010100"; break;
                case "55": aa = "01010101"; break;
                case "56": aa = "01010110"; break;
                case "57": aa = "01010111"; break;
                case "58": aa = "01011000"; break;
                case "59": aa = "01011001"; break;
                case "5A": aa = "01011010"; break;
                case "5B": aa = "01011011"; break;
                case "5C": aa = "01011100"; break;
                case "5D": aa = "01011101"; break;
                case "5E": aa = "01011110"; break;
                case "5F": aa = "01011111"; break;
                case "60": aa = "01100000"; break;
                case "61": aa = "01100001"; break;
                case "62": aa = "01100010"; break;
                case "63": aa = "01100011"; break;
                case "64": aa = "01100100"; break;
                case "65": aa = "01100101"; break;
                case "66": aa = "01100110"; break;
                case "67": aa = "01100111"; break;
                case "68": aa = "01101000"; break;
                case "69": aa = "01101001"; break;
                case "6A": aa = "01101010"; break;
                case "6B": aa = "01101011"; break;
                case "6C": aa = "01101100"; break;
                case "6D": aa = "01101101"; break;
                case "6E": aa = "01101110"; break;
                case "6F": aa = "01101111"; break;
                case "70": aa = "01110000"; break;
                case "71": aa = "01110001"; break;
                case "72": aa = "01110010"; break;
                case "73": aa = "01110011"; break;
                case "74": aa = "01110100"; break;
                case "75": aa = "01110101"; break;
                case "76": aa = "01110110"; break;
                case "77": aa = "01110111"; break;
                case "78": aa = "01111000"; break;
                case "79": aa = "01111001"; break;
                case "7A": aa = "01111010"; break;
                case "7B": aa = "01111011"; break;
                case "7C": aa = "01111100"; break;
                case "7D": aa = "01111101"; break;
                case "7E": aa = "01111110"; break;
                case "7F": aa = "01111111"; break;
                case "80": aa = "10000000"; break;
                case "81": aa = "10000001"; break;
                case "82": aa = "10000010"; break;
                case "83": aa = "10000011"; break;
                case "84": aa = "10000100"; break;
                case "85": aa = "10000101"; break;
                case "86": aa = "10000110"; break;
                case "87": aa = "10000111"; break;
                case "88": aa = "10001000"; break;
                case "89": aa = "10001001"; break;
                case "8A": aa = "10001010"; break;
                case "8B": aa = "10001011"; break;
                case "8C": aa = "10001100"; break;
                case "8D": aa = "10001101"; break;
                case "8E": aa = "10001110"; break;
                case "8F": aa = "10001111"; break;
                case "90": aa = "10010000"; break;
                case "91": aa = "10010001"; break;
                case "92": aa = "10010010"; break;
                case "93": aa = "10010011"; break;
                case "94": aa = "10010100"; break;
                case "95": aa = "10010101"; break;
                case "96": aa = "10010110"; break;
                case "97": aa = "10010111"; break;
                case "98": aa = "10011000"; break;
                case "99": aa = "10011001"; break;
                case "9A": aa = "10011010"; break;
                case "9B": aa = "10011011"; break;
                case "9C": aa = "10011100"; break;
                case "9D": aa = "10011101"; break;
                case "9E": aa = "10011110"; break;
                case "9F": aa = "10011111"; break;
                case "A0": aa = "10100000"; break;
                case "A1": aa = "10100001"; break;
                case "A2": aa = "10100010"; break;
                case "A3": aa = "10100011"; break;
                case "A4": aa = "10100100"; break;
                case "A5": aa = "10100101"; break;
                case "A6": aa = "10100110"; break;
                case "A7": aa = "10100111"; break;
                case "A8": aa = "10101000"; break;
                case "A9": aa = "10101001"; break;
                case "AA": aa = "10101010"; break;
                case "AB": aa = "10101011"; break;
                case "AC": aa = "10101100"; break;
                case "AD": aa = "10101101"; break;
                case "AE": aa = "10101110"; break;
                case "AF": aa = "10101111"; break;
                case "B0": aa = "10110000"; break;
                case "B1": aa = "10110001"; break;
                case "B2": aa = "10110010"; break;
                case "B3": aa = "10110011"; break;
                case "B4": aa = "10110100"; break;
                case "B5": aa = "10110101"; break;
                case "B6": aa = "10110110"; break;
                case "B7": aa = "10110111"; break;
                case "B8": aa = "10111000"; break;
                case "B9": aa = "10111001"; break;
                case "BA": aa = "10111010"; break;
                case "BB": aa = "10111011"; break;
                case "BC": aa = "10111100"; break;
                case "BD": aa = "10111101"; break;
                case "BE": aa = "10111110"; break;
                case "BF": aa = "10111111"; break;
                case "C0": aa = "11000000"; break;
                case "C1": aa = "11000001"; break;
                case "C2": aa = "11000010"; break;
                case "C3": aa = "11000011"; break;
                case "C4": aa = "11000100"; break;
                case "C5": aa = "11000101"; break;
                case "C6": aa = "11000110"; break;
                case "C7": aa = "11000111"; break;
                case "C8": aa = "11001000"; break;
                case "C9": aa = "11001001"; break;
                case "CA": aa = "11001010"; break;
                case "CB": aa = "11001011"; break;
                case "CC": aa = "11001100"; break;
                case "CD": aa = "11001101"; break;
                case "CE": aa = "11001110"; break;
                case "CF": aa = "11001111"; break;
                case "D0": aa = "11010000"; break;
                case "D1": aa = "11010001"; break;
                case "D2": aa = "11010010"; break;
                case "D3": aa = "11010011"; break;
                case "D4": aa = "11010100"; break;
                case "D5": aa = "11010101"; break;
                case "D6": aa = "11010110"; break;
                case "D7": aa = "11010111"; break;
                case "D8": aa = "11011000"; break;
                case "D9": aa = "11011001"; break;
                case "DA": aa = "11011010"; break;
                case "DB": aa = "11011011"; break;
                case "DC": aa = "11011100"; break;
                case "DD": aa = "11011101"; break;
                case "DE": aa = "11011110"; break;
                case "DF": aa = "11011111"; break;
                case "E0": aa = "11100000"; break;
                case "E1": aa = "11100001"; break;
                case "E2": aa = "11100010"; break;
                case "E3": aa = "11100011"; break;
                case "E4": aa = "11100100"; break;
                case "E5": aa = "11100101"; break;
                case "E6": aa = "11100110"; break;
                case "E7": aa = "11100111"; break;
                case "E8": aa = "11101000"; break;
                case "E9": aa = "11101001"; break;
                case "EA": aa = "11101010"; break;
                case "EB": aa = "11101011"; break;
                case "EC": aa = "11101100"; break;
                case "ED": aa = "11101101"; break;
                case "EE": aa = "11101110"; break;
                case "EF": aa = "11101111"; break;
                case "F0": aa = "11110000"; break;
                case "F1": aa = "11110001"; break;
                case "F2": aa = "11110010"; break;
                case "F3": aa = "11110011"; break;
                case "F4": aa = "11110100"; break;
                case "F5": aa = "11110101"; break;
                case "F6": aa = "11110110"; break;
                case "F7": aa = "11110111"; break;
                case "F8": aa = "11111000"; break;
                case "F9": aa = "11111001"; break;
                case "FA": aa = "11111010"; break;
                case "FB": aa = "11111011"; break;
                case "FC": aa = "11111100"; break;
                case "FD": aa = "11111101"; break;
                case "FE": aa = "11111110"; break;
                case "FF": aa = "11111111"; break;
            }
            return aa;
        }
     
        private void button1_Click(object sender, EventArgs e)
        {
            
            readerConnect();
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


        private void btnCreationStructure2_Click(object sender, EventArgs e)
        {
            structure2();
        }





        private void btnDeconnecte_Click(object sender, EventArgs e)
        {
            err=theReader.Disconnect(SCardReaderDisposition.Eject);
            txBxATR.Clear();
        }

        private void btnLoadKey_Click(object sender, EventArgs e)
        {
            var contextFactory = ContextFactory.Instance;
            var reader = cbReaders.SelectedText;
            using (var context = contextFactory.Establish(SCardScope.System))
            {

                using (var readerContact = new SCardReader(context))
                {
                    var sc = readerContact.Connect(cbReaders.SelectedItem.ToString(),SCardShareMode.Exclusive, SCardProtocol.T1);

                    var getFile = new CommandApdu(IsoCase.Case4Extended, readerContact.ActiveProtocol)
                    {
                        
                        CLA = 0x94,
                        Instruction=InstructionCode.SelectFile,
                        P1 = 0x09,
                        P2 = 0x00,
                        Le = 0x19,
                        Data=new byte[] {0x00, 0x00}

                    };

                    sc = readerContact.BeginTransaction();
                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("erreur lors de la transaction" + "\n" + SCardHelper.StringifyError(sc));
                    }

                    var receivePci = new SCardPCI();
                    var sendPci = SCardPCI.GetPci(readerContact.ActiveProtocol);
                    var receiveBuffer = new byte[256];
                    var command = getFile.ToArray();

                    sc = readerContact.Transmit(sendPci, command, receivePci, ref receiveBuffer);

                    if (sc != SCardError.Success)
                    {
                       MessageBox.Show("erreur lors de l'envoi de la commande" + "\n" + SCardHelper.StringifyError(sc)+"\n"+"L1190");
                    }

                    var getRep = new CommandApdu(IsoCase.Case2Extended, SCardProtocol.T0)
                    {
                        CLA = 0x94,
                        Instruction = InstructionCode.GetResponse
                    };
                    sc = readerContact.BeginTransaction();
                    if (sc!=SCardError.Success)
                    {
                        MessageBox.Show("Erreur lors de la transaction"+"\n"+SCardHelper.StringifyError(sc)+"\n"+"L1201");
                    }

                    receivePci = new SCardPCI();
                    sendPci = SCardPCI.GetPci(readerContact.ActiveProtocol);
                    receiveBuffer = new byte[256];
                    command = getRep.ToArray();
                    sc = readerContact.Transmit(sendPci, command, receivePci, ref receiveBuffer);
                    if (sc!=SCardError.Success)
                    {
                        MessageBox.Show("Erreur dans l'envoi de la commande Get Response"+"\n"+SCardHelper.StringifyError(sc)+"\n"+"L1211");
                    }

                    var apduRep = new ResponseApdu(receiveBuffer, IsoCase.Case2Extended, readerContact.ActiveProtocol);
                    if (apduRep.HasData)
                    {
                        byte[] incomingData = apduRep.GetData();
                        string X = "";
                        for (int i = 0; i < incomingData.Length; i++)
                        {
                            X += incomingData[i].ToString("X2");
                        }
                        
                        MessageBox.Show("résultat select file..."+"\n"+X);

                    }
                    
                    

                }
            }
        }

        private void btnQuitter_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

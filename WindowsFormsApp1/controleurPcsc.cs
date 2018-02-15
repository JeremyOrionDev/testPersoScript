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

    public class controleurPcsc
    {
        //Récupération du form pour entrée des données dans les différents élements
         private Form1 formAffichage;
 
        // déclaration des variables pour les mode de partage et protocole
        static SCardShareMode cardShareMode = new SCardShareMode();
        static SCardProtocol cardProtocol = new SCardProtocol();

        // déclaration du buffer de réception sur 256 bytes
        byte[] pbRecvBuffer = new byte[256];
        static SCardReader readerContact;
        //création du context à partir de la librairie
        static SCardContext hContext = new SCardContext();
        //déclaration des variables nécessaires
        int CardID;
        string chalenge = "";
        string send;
        /// <summary>
        /// Méthode statique établissant le context avec un scope système
        /// </summary>
        public static void establishContext()
        {
            hContext.Establish(SCardScope.System);
        }

        public static List<string> ListerLecteurs()
        {
            List<String> lesLecteurs = new List<string>();

            var contextFactory = ContextFactory.Instance;

            using (var context = contextFactory.Establish(SCardScope.System))
            {
                var readerNames = context.GetReaders();
                foreach (var reader in readerNames)
                {
                    lesLecteurs.Add(reader);
                }
            }

            return lesLecteurs;
        }

        /// <summary>
        /// Méthode statique de récupération des lecteur connectés
        /// </summary>
        /// <param name="form"></param>
        public static void readerList(Form1 form)
        {
            //utilisation du context 
            var contextFactory = ContextFactory.Instance;
            using (var context = contextFactory.Establish(SCardScope.System))
            {
               //récupération des noms des lecteurs utilisables par la librairie et ajout en comboBox
                var readerNames = context.GetReaders();
                foreach (var item in readerNames)
                {
                    form.cbReaders.Items.Add(item);
                }

                //récupération des lecteurs connectés et levée d'exception si aucun lecteur trouvé
                string[] szReaders = context.GetReaders();
                if (szReaders.Length <= 0)
                {
                    throw new PCSCException(SCardError.NoReadersAvailable, "Aucun lecteur trouvé");
                }
            }

            //Ajout des items dans les comboBox
            form.cBxProtocol.Items.AddRange(new string[] { "T0", "Any", "T1", "T15", "Unset", "Raw" });
            form.cBxShareMode.Items.AddRange(new string[] { "Direct", "Exclusive", "Shared" });

            //si aucun lecteur n'est sélectionné le label demandant la sélection d'un lecteur apparaît
            if (form.cbReaders.SelectedIndex == -1)
            {
                form.lblSelectReader.Visible = true;
            }
            else form.lblSelectReader.Visible = false;
        }

        
        /// <summary>
        /// Méthode statique permettant la création d'un tableau de byte à partir d'une chaine de caractères hex
        /// </summary>
        /// <param name="input">La chaine à traiter</param>
        /// <returns>renvoi un tableau de byte avec chaque élément convertit</returns>
        public static byte[] createByte(string input)
        {
            //création du tableau en découpant la chaine par les caractères séparateurs
            string[] hexTab = input.Split('-');
            //Création de la chaine de sortie en binaire
            string binString = "";
            //Boucle de remplissage de la chaine de sortie
            for (int i = 0; i < hexTab.Count(); i++)
            {
                binString += HexVersBin(hexTab[i]);
            }
            //récupération du nombre de byte de la chaine
            int numOfBytes = binString.Length / 8;
            //création du tableau à la taille voulue
            byte[] bytes = new byte[numOfBytes];
            //remplissage du tableau en convertissant la chaine binaire
            for (int i = 0; i < numOfBytes; ++i)
            {
                bytes[i] = Convert.ToByte(binString.Substring(8 * i, 8), 2);
            }
            //renvoi du tableau crée
            return bytes;
        }

        /// <summary>
        /// Méthode de conversion de chaine hexadécimale vers binaire
        /// </summary>
        /// <param name="chaineHex">Chaine d'entrée</param>
        /// <returns>code binaire de la chaine</returns>
        public static string HexVersBin(string chaineHex)
        {
            //Création de la chaine de sortie
            string bin;
            chaineHex = chaineHex.ToUpper();
            
            bin = "";
            //attribution des code binaire en fonction du code hexadecimal entré
            switch (chaineHex)
            {
                case "00": bin = "00000000"; break;
                case "01": bin = "00000001"; break;
                case "02": bin = "00000010"; break;
                case "03": bin = "00000011"; break;
                case "04": bin = "00000100"; break;
                case "05": bin = "00000101"; break;
                case "06": bin = "00000110"; break;
                case "07": bin = "00000111"; break;
                case "08": bin = "00001000"; break;
                case "09": bin = "00001001"; break;
                case "0A": bin = "00001010"; break;
                case "0B": bin = "00001011"; break;
                case "0C": bin = "00001100"; break;
                case "0D": bin = "00001101"; break;
                case "0E": bin = "00001110"; break;
                case "0F": bin = "00001111"; break;
                case "10": bin = "00010000"; break;
                case "11": bin = "00010001"; break;
                case "12": bin = "00010010"; break;
                case "13": bin = "00010011"; break;
                case "14": bin = "00010100"; break;
                case "15": bin = "00010101"; break;
                case "16": bin = "00010110"; break;
                case "17": bin = "00010111"; break;
                case "18": bin = "00011000"; break;
                case "19": bin = "00011001"; break;
                case "1A": bin = "00011010"; break;
                case "1B": bin = "00011011"; break;
                case "1C": bin = "00011100"; break;
                case "1D": bin = "00011101"; break;
                case "1E": bin = "00011110"; break;
                case "1F": bin = "00011111"; break;
                case "20": bin = "00100000"; break;
                case "21": bin = "00100001"; break;
                case "22": bin = "00100010"; break;
                case "23": bin = "00100011"; break;
                case "24": bin = "00100100"; break;
                case "25": bin = "00100101"; break;
                case "26": bin = "00100110"; break;
                case "27": bin = "00100111"; break;
                case "28": bin = "00101000"; break;
                case "29": bin = "00101001"; break;
                case "2A": bin = "00101010"; break;
                case "2B": bin = "00101011"; break;
                case "2C": bin = "00101100"; break;
                case "2D": bin = "00101101"; break;
                case "2E": bin = "00101110"; break;
                case "2F": bin = "00101111"; break;
                case "30": bin = "00110000"; break;
                case "31": bin = "00110001"; break;
                case "32": bin = "00110010"; break;
                case "33": bin = "00110011"; break;
                case "34": bin = "00110100"; break;
                case "35": bin = "00110101"; break;
                case "36": bin = "00110110"; break;
                case "37": bin = "00110111"; break;
                case "38": bin = "00111000"; break;
                case "39": bin = "00111001"; break;
                case "3A": bin = "00111010"; break;
                case "3B": bin = "00111011"; break;
                case "3C": bin = "00111100"; break;
                case "3D": bin = "00111101"; break;
                case "3E": bin = "00111110"; break;
                case "3F": bin = "00111111"; break;
                case "40": bin = "01000000"; break;
                case "41": bin = "01000001"; break;
                case "42": bin = "01000010"; break;
                case "43": bin = "01000011"; break;
                case "44": bin = "01000100"; break;
                case "45": bin = "01000101"; break;
                case "46": bin = "01000110"; break;
                case "47": bin = "01000111"; break;
                case "48": bin = "01001000"; break;
                case "49": bin = "01001001"; break;
                case "4A": bin = "01001010"; break;
                case "4B": bin = "01001011"; break;
                case "4C": bin = "01001100"; break;
                case "4D": bin = "01001101"; break;
                case "4E": bin = "01001110"; break;
                case "4F": bin = "01001111"; break;
                case "50": bin = "01010000"; break;
                case "51": bin = "01010001"; break;
                case "52": bin = "01010010"; break;
                case "53": bin = "01010011"; break;
                case "54": bin = "01010100"; break;
                case "55": bin = "01010101"; break;
                case "56": bin = "01010110"; break;
                case "57": bin = "01010111"; break;
                case "58": bin = "01011000"; break;
                case "59": bin = "01011001"; break;
                case "5A": bin = "01011010"; break;
                case "5B": bin = "01011011"; break;
                case "5C": bin = "01011100"; break;
                case "5D": bin = "01011101"; break;
                case "5E": bin = "01011110"; break;
                case "5F": bin = "01011111"; break;
                case "60": bin = "01100000"; break;
                case "61": bin = "01100001"; break;
                case "62": bin = "01100010"; break;
                case "63": bin = "01100011"; break;
                case "64": bin = "01100100"; break;
                case "65": bin = "01100101"; break;
                case "66": bin = "01100110"; break;
                case "67": bin = "01100111"; break;
                case "68": bin = "01101000"; break;
                case "69": bin = "01101001"; break;
                case "6A": bin = "01101010"; break;
                case "6B": bin = "01101011"; break;
                case "6C": bin = "01101100"; break;
                case "6D": bin = "01101101"; break;
                case "6E": bin = "01101110"; break;
                case "6F": bin = "01101111"; break;
                case "70": bin = "01110000"; break;
                case "71": bin = "01110001"; break;
                case "72": bin = "01110010"; break;
                case "73": bin = "01110011"; break;
                case "74": bin = "01110100"; break;
                case "75": bin = "01110101"; break;
                case "76": bin = "01110110"; break;
                case "77": bin = "01110111"; break;
                case "78": bin = "01111000"; break;
                case "79": bin = "01111001"; break;
                case "7A": bin = "01111010"; break;
                case "7B": bin = "01111011"; break;
                case "7C": bin = "01111100"; break;
                case "7D": bin = "01111101"; break;
                case "7E": bin = "01111110"; break;
                case "7F": bin = "01111111"; break;
                case "80": bin = "10000000"; break;
                case "81": bin = "10000001"; break;
                case "82": bin = "10000010"; break;
                case "83": bin = "10000011"; break;
                case "84": bin = "10000100"; break;
                case "85": bin = "10000101"; break;
                case "86": bin = "10000110"; break;
                case "87": bin = "10000111"; break;
                case "88": bin = "10001000"; break;
                case "89": bin = "10001001"; break;
                case "8A": bin = "10001010"; break;
                case "8B": bin = "10001011"; break;
                case "8C": bin = "10001100"; break;
                case "8D": bin = "10001101"; break;
                case "8E": bin = "10001110"; break;
                case "8F": bin = "10001111"; break;
                case "90": bin = "10010000"; break;
                case "91": bin = "10010001"; break;
                case "92": bin = "10010010"; break;
                case "93": bin = "10010011"; break;
                case "94": bin = "10010100"; break;
                case "95": bin = "10010101"; break;
                case "96": bin = "10010110"; break;
                case "97": bin = "10010111"; break;
                case "98": bin = "10011000"; break;
                case "99": bin = "10011001"; break;
                case "9A": bin = "10011010"; break;
                case "9B": bin = "10011011"; break;
                case "9C": bin = "10011100"; break;
                case "9D": bin = "10011101"; break;
                case "9E": bin = "10011110"; break;
                case "9F": bin = "10011111"; break;
                case "A0": bin = "10100000"; break;
                case "A1": bin = "10100001"; break;
                case "A2": bin = "10100010"; break;
                case "A3": bin = "10100011"; break;
                case "A4": bin = "10100100"; break;
                case "A5": bin = "10100101"; break;
                case "A6": bin = "10100110"; break;
                case "A7": bin = "10100111"; break;
                case "A8": bin = "10101000"; break;
                case "A9": bin = "10101001"; break;
                case "AA": bin = "10101010"; break;
                case "AB": bin = "10101011"; break;
                case "AC": bin = "10101100"; break;
                case "AD": bin = "10101101"; break;
                case "AE": bin = "10101110"; break;
                case "AF": bin = "10101111"; break;
                case "B0": bin = "10110000"; break;
                case "B1": bin = "10110001"; break;
                case "B2": bin = "10110010"; break;
                case "B3": bin = "10110011"; break;
                case "B4": bin = "10110100"; break;
                case "B5": bin = "10110101"; break;
                case "B6": bin = "10110110"; break;
                case "B7": bin = "10110111"; break;
                case "B8": bin = "10111000"; break;
                case "B9": bin = "10111001"; break;
                case "BA": bin = "10111010"; break;
                case "BB": bin = "10111011"; break;
                case "BC": bin = "10111100"; break;
                case "BD": bin = "10111101"; break;
                case "BE": bin = "10111110"; break;
                case "BF": bin = "10111111"; break;
                case "C0": bin = "11000000"; break;
                case "C1": bin = "11000001"; break;
                case "C2": bin = "11000010"; break;
                case "C3": bin = "11000011"; break;
                case "C4": bin = "11000100"; break;
                case "C5": bin = "11000101"; break;
                case "C6": bin = "11000110"; break;
                case "C7": bin = "11000111"; break;
                case "C8": bin = "11001000"; break;
                case "C9": bin = "11001001"; break;
                case "CA": bin = "11001010"; break;
                case "CB": bin = "11001011"; break;
                case "CC": bin = "11001100"; break;
                case "CD": bin = "11001101"; break;
                case "CE": bin = "11001110"; break;
                case "CF": bin = "11001111"; break;
                case "D0": bin = "11010000"; break;
                case "D1": bin = "11010001"; break;
                case "D2": bin = "11010010"; break;
                case "D3": bin = "11010011"; break;
                case "D4": bin = "11010100"; break;
                case "D5": bin = "11010101"; break;
                case "D6": bin = "11010110"; break;
                case "D7": bin = "11010111"; break;
                case "D8": bin = "11011000"; break;
                case "D9": bin = "11011001"; break;
                case "DA": bin = "11011010"; break;
                case "DB": bin = "11011011"; break;
                case "DC": bin = "11011100"; break;
                case "DD": bin = "11011101"; break;
                case "DE": bin = "11011110"; break;
                case "DF": bin = "11011111"; break;
                case "E0": bin = "11100000"; break;
                case "E1": bin = "11100001"; break;
                case "E2": bin = "11100010"; break;
                case "E3": bin = "11100011"; break;
                case "E4": bin = "11100100"; break;
                case "E5": bin = "11100101"; break;
                case "E6": bin = "11100110"; break;
                case "E7": bin = "11100111"; break;
                case "E8": bin = "11101000"; break;
                case "E9": bin = "11101001"; break;
                case "EA": bin = "11101010"; break;
                case "EB": bin = "11101011"; break;
                case "EC": bin = "11101100"; break;
                case "ED": bin = "11101101"; break;
                case "EE": bin = "11101110"; break;
                case "EF": bin = "11101111"; break;
                case "F0": bin = "11110000"; break;
                case "F1": bin = "11110001"; break;
                case "F2": bin = "11110010"; break;
                case "F3": bin = "11110011"; break;
                case "F4": bin = "11110100"; break;
                case "F5": bin = "11110101"; break;
                case "F6": bin = "11110110"; break;
                case "F7": bin = "11110111"; break;
                case "F8": bin = "11111000"; break;
                case "F9": bin = "11111001"; break;
                case "FA": bin = "11111010"; break;
                case "FB": bin = "11111011"; break;
                case "FC": bin = "11111100"; break;
                case "FD": bin = "11111101"; break;
                case "FE": bin = "11111110"; break;
                case "FF": bin = "11111111"; break;
            }
            //renvoi le code correspondant
            return bin;
        }

        /// <summary>
        /// Méthode de création du diversifier par le module SAM à partir du numero de série de la carte
        /// </summary>
        /// <param name="input">  numero de série de la carte   </param>
        /// <returns>booléen en fonction de la réussite</returns>
        public static bool SamDiversifier(string input)
        {
            //Etablissement d'une instance du context
            var contextFactory = ContextFactory.Instance;
            using (var ctx = contextFactory.Establish(SCardScope.System))
            {
                //Récupération du tableau de byte à partir de la chaine hexa
                byte[] Byte = createByte(input);
                // atribution de la variable Sam correspondant au lecteur SAM
                using (var Sam = new SCardReader(ctx))
                {
                    //Connexion au lecteur directement, nom connu donc entré en manuel pour éviter les erreurs
                    var sc = Sam.Connect("ACS CCID USB Reader 0", SCardShareMode.Exclusive, SCardProtocol.T0);
                    //en cas d'erreur remontée de l'erreur et n° de ligne
                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("erreur de connexion au lecteur SAM" + "\n" + SCardHelper.StringifyError(sc)+"\n"+"L400");
                    }
                    //Création de la commande apdu correspondante
                    var apdu = new CommandApdu(IsoCase.Case3Extended, SCardProtocol.T0)
                    {
                        CLA = 0x80,
                        INS = 0x14,
                        P1 = 0x00,
                        P2 = 0x00,
                        Data = Byte
                    };

                    // démarrage de la transaction
                    sc = Sam.BeginTransaction();

                    //en cas d'erreur remontée et renvoi faux sinon OK
                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("erreur lors de la transmission de la commande au lecteur SAM" + "\n" + SCardHelper.StringifyError(sc)+"\n"+"L417");
                        return false;
                    }
                    else return true;

                }
            }
        }

        /// <summary>
        /// Méthode envoyant le nombre aléatoire créé par la carte précédemment
        /// </summary>
        /// <param name="input">nombre aléatoire généré par la carte</param>
        /// <returns>Vrai ou faux</returns>
        public static bool giveSAMrandom(string input)
        {
            //Etablissement du context
            var contextFactory = ContextFactory.Instance;
            using (var ctx = contextFactory.Establish(SCardScope.System))
            {
                //Création du tableau de byte a partir de la chaine hexa
                byte[] Byte = createByte(input);
                using (var Sam = new SCardReader(ctx))
                {
                    //utilisation des paramètres de connexion fixes
                    var sc = Sam.Connect("ACS CCID USB Reader 0", SCardShareMode.Exclusive, SCardProtocol.T0);
                    //remontée de l'erreur eventuelle
                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("erreur de connexion au lecteur SAM" + "\n" + SCardHelper.StringifyError(sc)+"\n"+"L447");
                    }

                    //création de la commande APDU avec les données contenues dans le tableau de byte
                    var apdu = new CommandApdu(IsoCase.Case3Extended, Sam.ActiveProtocol)
                    {
                        CLA = 0x80,
                        Instruction = InstructionCode.InternalAuthenticate,
                        P1 = 0x00,
                        P2 = 0x00,
                        Data = Byte
                    };

                    //démarrage de la transaction
                    sc = Sam.BeginTransaction();

                    //remontée de l'erreur éventuelle 
                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("erreur lors de la transaction " + "\n" + "L465" + "\n" + SCardHelper.StringifyError(sc));

                    };

                    //Déclaration des variables nécessaires a la transmission
                    var receivePci = new SCardPCI();
                    var sendPci = SCardPCI.GetPci(Sam.ActiveProtocol);
                    var receiveBuffer = new byte[256];
                    var command = apdu.ToArray();

                    //transmission de la commande au SAM
                    sc = Sam.Transmit(command, ref receiveBuffer);

                    //remontée de l'erreur éventuelle
                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("erreur dans la transmission de la commande au lecteur SAM" + "\n" + "L 481");

                    }

                    //Création de la commande APDU demandant la réponse
                    var getRep = new CommandApdu(IsoCase.Case2Extended, SCardProtocol.T0)
                    {
                        CLA = 0x94,
                        Instruction = InstructionCode.GetResponse,
                        P1 = 0x00,
                        P2 = 0x00

                    };

                    //démarrage de la nouvelle transaction
                    sc = Sam.BeginTransaction();

                    //remontée de l'éventuelle erreur 
                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("erreur lors de la transaction : " + "\n" + SCardHelper.StringifyError(sc)+"\n"+"L501");

                    }

                    //assignation des nouvelles valeurs
                    receivePci = new SCardPCI();
                    sendPci = SCardPCI.GetPci(Sam.ActiveProtocol);
                    receiveBuffer = new byte[256];
                    command = getRep.ToArray();

                    //transmission de la commande
                    sc = Sam.Transmit(sendPci, command, receivePci, ref receiveBuffer);

                    //remontée de l'éventuelle erreur et retour en fonction
                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("erreur lors de la commande getRep"+"\n"+SCardHelper.StringifyError(sc)+"\n"+"L517");
                        return false;
                    }
                    else return true;
                }
            }

        }

        /// <summary>
        /// Méthode de génération de la clé aléatoire par le lecteur SAM 
        /// </summary>
        /// <returns>clé</returns>
        public static string SamGenKey()
        {
            //etablissement du context
            var contextFactory = ContextFactory.Instance;
            using (var ctx = contextFactory.Establish(SCardScope.System))
            {
                using (var Sam = new SCardReader(ctx))
                {
                    //connexion au lecteur
                    var sc = Sam.Connect("ACS CCID USB Reader 0", SCardShareMode.Exclusive, SCardProtocol.T0);

                    //détection et remontée des erreurs
                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("erreur de connexion au lecteur SAM" + "\n" + SCardHelper.StringifyError(sc)+"\n"+"L544");
                    }
                   
                    //préparation de la commande avec les emplacement de clés sélectionnés
                    var apdu = new CommandApdu(IsoCase.Case3Extended, Sam.ActiveProtocol)
                    {
                        CLA = 0x94,
                        INS = 0x12,
                        P1 = 0xFF,
                        P2 = 0xFF,
                        Data = new byte[] { 0x61, 0x41, 0x67, 0x41 }
                    };

                    //démarrage de la transaction
                    sc = Sam.BeginTransaction();

                    //détection et remontée des erreurs
                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("erreur lors de la transmission de la commande au lecteur SAM" + "\n" + SCardHelper.StringifyError(sc)+"\n"+"L563");
                    }

                    //Déclaration des variables nécessaires
                    var receivePci = new SCardPCI();
                    var sendPci = SCardPCI.GetPci(Sam.ActiveProtocol);
                    var receiveBuffer = new byte[256];
                    var command = apdu.ToArray();

                    //transmission de la commande
                    sc = Sam.Transmit(command, ref receiveBuffer);

                    //récupération des erreurs eventuelles
                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("Erreur lors de l'envoi de la commande de génération de la clé" + "\n" + SCardHelper.StringifyError(sc) + "\n" + "L578");
                    }

                    //préparation de la commande
                    var getRep = new CommandApdu(IsoCase.Case2Extended, Sam.ActiveProtocol)
                    {
                        CLA = 0x94,
                        Instruction = InstructionCode.GetResponse,
                        Le = 0x20,
                        P1P2 = 0
                    };

                    //lancement de la transaction et remontée des erreurs
                    sc = Sam.BeginTransaction();
                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("erreur lors de la réception de la réponse du SAM" + "\n" + SCardHelper.StringifyError(sc) + "\n" + "L596");
                    }

                    receivePci = new SCardPCI();
                    sendPci = SCardPCI.GetPci(Sam.ActiveProtocol);
                    receiveBuffer = new byte[256];
                    command = getRep.ToArray();

                    //transmission de la commande
                    sc = Sam.Transmit(sendPci, command, receivePci, ref receiveBuffer);
                    //déclaration de la chaine qui contiendra la clé
                    string Key = "";

                    //boucle de remplissage à partir du fichier tampon reçu
                    for (int l = 0; l < receiveBuffer.Length; l++)
                    {
                        if (l == 0)
                        {
                            Key = receiveBuffer[l].ToString("X2");
                        }
                        else Key += "-" + receiveBuffer[l].ToString("X2");
                    }
                    //renvoi de la clé
                    return Key;
                }
            }
        }

        /// <summary>
        /// fonction de connection au lecteur et récupération de l'ATR
        /// préparation des protocoles d'échanges et mode de transfert
        /// </summary>
        public static void readerConnect(Form1 form)
        {
            //Déclaration des variables et assignation des valeurs
            byte Cla, Ins, p1, p2, le = new byte();
            Cla=Ins=p1 = p2 = 0x00;

            //récupération du lecteur 
            string value = form.cbReaders.SelectedItem.ToString();

            //attribution des valeurs en fonction du lecteur
            switch (value)
            {
                case "ACS CCID USB Reader 0":

                    Cla = 0x94;
                    Ins = 0x84;
                    le = 0x08;
                    break;
                case "Duali DE-620 Contact Reader 0":

                    Cla = 0x94;
                    Ins = 0xBE;
                    le = 0x13;
                    break;
                case "SpringCard Prox'N'Roll Contactless 0":
                    Cla = 0xff;
                    Ins = 0xca;
                    le = 16;
                    break;
            }

            //switch du protocol en fonction de l'item choisi en comboBox
            switch (form.cBxProtocol.SelectedIndex)
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
            switch (form.cBxShareMode.SelectedIndex)
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
            var contextFactory = ContextFactory.Instance;
            var reader = form.cbReaders.SelectedText;
            using (var context = contextFactory.Establish(SCardScope.System))
            {

                using ( readerContact = new SCardReader(context))
                {
                    //connexion au lecteur sélectionné 
                    var sc = readerContact.Connect(form.cbReaders.SelectedItem.ToString(), cardShareMode, cardProtocol);

                    //remonté des erreurs
                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("Reader connection error" +"\n"+SCardHelper.StringifyError(sc)+"\n"+"L711");
                    }
                    #region getATR
                    var aTR = new CommandApdu(IsoCase.Case2Extended, readerContact.ActiveProtocol)
                    {
                        
                        CLA = Cla,
                        INS = Ins,
                        P1 = p1,
                        P2 = p2,
                        Le = le

                    };

                    //départ transaction
                    sc = readerContact.BeginTransaction();

                    //remonté en cas d'erreur
                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("erreur lors de la transaction" + "\n" + SCardHelper.StringifyError(sc)+"L731");
                    }
                    //déclaration des variables
                    var receivePci = new SCardPCI();
                    var sendPci = SCardPCI.GetPci(readerContact.ActiveProtocol);
                    var receiveBuffer = new byte[256];
                    var command = aTR.ToArray();

                    //transmission de la commande à la carte
                    sc = readerContact.Transmit(sendPci, command, receivePci, ref receiveBuffer);

                    //remontée en cas d'erreur
                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("erreur lors de l'envoi de la commande" + "\n" + SCardHelper.StringifyError(sc)+"L745");
                    }

                    //récupération de la réponse de la carte
                    var apduResponse = new ResponseApdu(receiveBuffer, IsoCase.Case2Extended, SCardProtocol.T1);

                    //Si la réponse contient des données
                    if (!string.IsNullOrEmpty(apduResponse.GetData().ToString()))
                    {
                        //création du tableau de byte et assignation des données
                        byte[] incomingData = apduResponse.GetData();

                        //déclaration de la chaine contenant l'ATR obtenue et boucle de remplissage
                        string ATR = "";
                        for (int i = 0; i < incomingData.Length; i++)
                        {
                            ATR += incomingData[i].ToString("X2");
                        }

                        //découpe des 4 derniers caractères contenant le code de retour valide et envoi en textBox
                        ATR = ATR.Substring(0, ATR.Length - 4);
                        form.txBxATR.Text = ATR;
                    }
                    #endregion


                    #region applicationSelect

                    //déclaration de la commande apdu pour sélectionner l'application sur la carte
                    var apdu = new CommandApdu(IsoCase.Case4Extended, SCardProtocol.T1)
                    {
                        CLA = 0x00,
                        INS = 0xA4,
                        P1 = 0x04,
                        P2 = 0x00,
                        Data = new byte[] { 0x31, 0x54, 0x49, 0x43, 0x2E, 0x49, 0x43, 0x41 },
                        Le = 0x24

                    };

                    //démarrage de la transaction
                    sc = readerContact.BeginTransaction();

                    //remontée des erreurs
                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("Erreur lors de la transaction" + "\n" + SCardHelper.StringifyError(sc)+"\n"+"L791");
                        return;
                    }

                    //déclaration des variables
                    var ReceivePci = new SCardPCI();
                    var SendPci = SCardPCI.GetPci(readerContact.ActiveProtocol);
                    receiveBuffer = new byte[256];
                    command = apdu.ToArray();

                    //transmission de la commande au lecteur de carte
                    sc = readerContact.Transmit(SendPci, command, ReceivePci, ref receiveBuffer);

                    //remontée en cas d'erreur
                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("Erreur lors de la transmission de la commande" + "\n" + SCardHelper.StringifyError(sc)+"\n"+"L807");
                    }

                    

                    //déclaration de la commande de réponse
                    var getRep = new CommandApdu(IsoCase.Case2Extended, SCardProtocol.T0)
                    {
                        CLA = 0x94,
                        Instruction = InstructionCode.GetResponse,
                        P1 = 0x00,
                        P2 = 0x00,
                        Le = 0x24

                    };

                    //début de la transaction
                    sc = readerContact.BeginTransaction();

                    //remontée des erreurs
                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("erreur lors de la transaction : " + "\n" + SCardHelper.StringifyError(sc)+"\n"+"L829");
                    }

                    //assignation des nouvelles valeurs
                    ReceivePci = new SCardPCI();
                    sendPci = SCardPCI.GetPci(readerContact.ActiveProtocol);
                    receiveBuffer = new byte[256];
                    command = getRep.ToArray();

                    //transmission de la commande a la carte
                    sc = readerContact.Transmit(sendPci, command, ReceivePci, ref receiveBuffer);

                    //remontée des erreurs
                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("erreur");
                    }

                    //récupération de la réponse
                    apduResponse = new ResponseApdu(receiveBuffer, IsoCase.Case2Short, readerContact.ActiveProtocol);

                    //déclaration de la chaine contenant la valeur de l'application récupérée
                    // et boucle de remplissage en cas de données reçues
                    string app = "";
                    if (apduResponse.HasData)
                    {
                        var data = apduResponse.GetData();
                        for (int i = 0; i < data.Length; i++)
                        {
                            app += data[i].ToString("X2");
                        }

                    } else
                    {
                        MessageBox.Show("Erreur aucune données reçues"+"\n"+"L853");
                    }

                    //découpe des caractères de validation
                    app = app.Substring(0, app.Length - 4);

                    //récupération de la valeur de l'AID et envoi en textBox
                    string AID = app.Substring(8, 16);
                    form.txBxAID.Text = AID;

                    //récupération du numéro de la carte et envoi en textBox
                    string num = app.Substring(38, 16);
                    form.txBxappSN.Text = num;

                    //déclaration et assignation du numéro de série et formatage
                    string SN = "";
                    for (int i = 0; i < num.Length; i = i + 2)
                    {
                        SN += "-" + num[i] + num[i + 1];
                    }

                    //déclaration et assignation de la chaine contenant la commande avec le numero
                    string cmd = "80-14-00-00-08" + num;

                    //création du tableau de byte correspondant
                    byte[] Byte = createByte(cmd);
                    #endregion

                    //booléen de controle de réussite
                    bool diversifierRep = SamDiversifier(num);
                    if (!diversifierRep)
                    {
                        MessageBox.Show("Erreur lors de la création du diversifier"+"\n"+"L897");
                    }
                    

                    #region getChallenge

                    //préparation de la commande de récupération du challenge pour la génération de la clé
                    apdu = new CommandApdu(IsoCase.Case2Extended, readerContact.ActiveProtocol)
                    {
                        CLA = 0x00,
                        Instruction = InstructionCode.GetChallenge,
                        P1 = 0x00,
                        P2 = 0x00,
                        Le = 0x08
                    };

                    //début de la transaction
                    sc = readerContact.BeginTransaction();

                    //remontée des erreurs
                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("erreur dans la commande apdu" + SCardHelper.StringifyError(sc)+"\n"+"L917");
                    }

                    //assignation des variables
                    ReceivePci = new SCardPCI();
                    sendPci = SCardPCI.GetPci(readerContact.ActiveProtocol);
                    receiveBuffer = new byte[256];
                    command = apdu.ToArray();

                    //transmission de la commande a la carte
                    sc = readerContact.Transmit(sendPci, command, ReceivePci, ref receiveBuffer);
                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("erreur lors de l'envoi de la commande GetChallenge"+SCardHelper.StringifyError(sc)+"\n"+"L932");
                    }

                    //préparation de la réponse
                    apduResponse = new ResponseApdu(receiveBuffer, IsoCase.Case2Extended, readerContact.ActiveProtocol);
                    //déclaration de la variable contenant la valeur 
                    string chalenge = "";

                    if (apduResponse.HasData)
                    {
                        //déclaration de la variable contenant les données et récupération
                        byte[] data = apduResponse.GetData();
                        //boucle de remplissage et formatage en hexadécimal
                        for (int i = 0; i < data.Length; i++)
                        {
                            if (i == 0)
                            {
                                chalenge = data[i].ToString("X2");
                            }
                            else
                            {
                                chalenge += "-" + data[i].ToString("X2");

                            }
                        }
                        //envoi au SAM
                        bool SamRandom = giveSAMrandom(chalenge);

                        if (!SamRandom)
                        {
                            MessageBox.Show("Erreur lors de la génération du nombre aléatoire par le SAM");
                        }
                        else
                        {
                            MessageBox.Show("Random OK");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Erreur challenge non reçu"+"\n"+"L939");
                    }
                    #endregion


                    #region ChangeKey

                    //déclaration et assignation de la nouvelle clé
                    string Key = SamGenKey();

                    //conversion de la clé en tableau de byte
                    byte[] byteKey = createByte(Key);

                    //préparation de la commande apdu de transfert de la clé vers la carte
                    apdu = new CommandApdu(IsoCase.Case3Extended, readerContact.ActiveProtocol)
                    {
                        CLA = 0x94,
                        INS = 0xD8,
                        P1 = 0x00,
                        P2 = 0x01,
                        Data = byteKey

                    };

                    //début de la transaction
                    sc = readerContact.BeginTransaction();

                    //remontée des erreurs
                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("Erreur lors du changement de la clé " + "\n" + SCardHelper.StringifyError(sc) + "\n" + "L999");
                    }
                    else MessageBox.Show("Clé n°1 changée ");

                    //génération de la clé 2 et transformation en tableau de byte
                    string Key2 = SamGenKey();
                    byte[] byteKey2 = createByte(Key2);

                    //préparation de la commande avec la clé
                    apdu = new CommandApdu(IsoCase.Case3Extended, readerContact.ActiveProtocol)
                    {
                        CLA = 0x94,
                        INS = 0xD8,
                        P1 = 0x00,
                        P2 = 0x03,
                        Data = byteKey2

                    };

                    //début de la transaction
                    sc = readerContact.BeginTransaction();

                    //remontée en cas d'erreur
                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("Erreur lors du changement de la clé " + "\n" + SCardHelper.StringifyError(sc) + "\n" + "L1024");
                    }
                    else MessageBox.Show("Clé changée n°3");

                    #endregion

                    #region getFile
                    //préparation de la commande de récupération du dossier
                    var getFile = new CommandApdu(IsoCase.Case4Extended, SCardProtocol.T1)
                    {
                        CLA = 0x94,
                        Instruction = InstructionCode.SelectFile,
                        P1 = 0x02,
                        P2 = 0x00,
                        Le = 0x19,
                        Data = new byte[] { 0x30, 0x45, 0x54, 0x50, 0x2E, 0x49, 0x43, 0x41 }

                    };

                    //début de la transaction et remontée des erreurs
                    sc = readerContact.BeginTransaction();
                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("erreur lors de la transaction" + "\n" + SCardHelper.StringifyError(sc)+"\n"+"L1049");
                    }

                    //assignation des variables
                    receivePci = new SCardPCI();
                    sendPci = SCardPCI.GetPci(readerContact.ActiveProtocol);
                    receiveBuffer = new byte[256];
                    command = getFile.ToArray();

                    //transmission de la commande
                    sc = readerContact.Transmit(command, ref receiveBuffer);

                    //remontée des erreurs
                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("erreur lors de l'envoi de la commande" + "\n" + SCardHelper.StringifyError(sc) + "\n" + "L1062");
                    }

                    //préparation de la commande de réponse
                    getRep = new CommandApdu(IsoCase.Case2Extended, SCardProtocol.T0)
                    {
                        CLA = 0x94,
                        Instruction = InstructionCode.GetResponse,
                        Le = 0x19
                    };

                    //début de la transaction
                    sc = readerContact.BeginTransaction();

                    //remontée des erreurs
                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("Erreur lors de la transaction" + "\n" + SCardHelper.StringifyError(sc) + "\n" + "L1079");
                    }

                    //assignation des nouvelles variables
                    receivePci = new SCardPCI();
                    sendPci = SCardPCI.GetPci(readerContact.ActiveProtocol);
                    receiveBuffer = new byte[256];
                    command = getRep.ToArray();

                    //envoi de la commande
                    sc = readerContact.Transmit(command, ref receiveBuffer);

                    //remontée des erreurs
                    if (sc != SCardError.Success)
                    {
                        MessageBox.Show("Erreur dans l'envoi de la commande Get Response" + "\n" + SCardHelper.StringifyError(sc) + "\n" + "L1094");
                    }

                    //déclaration et assignation de la chaine contenant la sélection
                    string Sel = "";
                    for (int i = 0; i < receiveBuffer.Length; i++)
                    {
                        Sel += receiveBuffer[i].ToString("X2");
                    }

                    #endregion
                } // fin using readerContact



            } //fin using context
        } // balise ferme readerConnect

        /// <summary>
        /// méthode de déconnexion du lecteur
        /// </summary>
        public static void readerDisconnect()
        {
            var contextFactory = ContextFactory.Instance;
            using (var context = contextFactory.Establish(SCardScope.System))
            {
                readerContact.Disconnect(SCardReaderDisposition.Leave);                
            }
        }
    }
}

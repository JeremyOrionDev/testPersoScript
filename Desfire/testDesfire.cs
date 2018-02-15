//  Auteur : YOANN
//  Date Début : 07/06/2016
//  Client : LABID
//  Carte : Carte Desfire ICAR
//  Dossier : 
//  Puce : Desfire
//  Machine : Persoline        
//       
namespace Desfire
{
    using System;
    using System.IO;
    using System.Text;
    using System.Collections;
    using System.Threading;
    using AtlanticZeiser.CpiPc.Reader.Smartware;
    using AtlanticZeiser.CpiPc.Reader.Smartware.Cards;
    using AtlanticZeiser.CpiPc.Scripting.Compiler;
    using AtlanticZeiser.CpiPc.Tools.Misc;
    using AtlanticZeiser.CpiPc.Scripting.Converter;
    using AtlanticZeiser.CpiPc.Scripting.Xml.Mifare;
    using System.Security.Cryptography;
    using System.Runtime.InteropServices;



    /// <summary>
    /// Desfire dummy implementation.
    /// </summary>
    public class testDesfire : CardIso14443A
    {

        public static readonly byte StatusResponseOk = 0x00;
        public const int ALGO_DES = 0;
        public const int ALGO_TDES = 1;
        public const int ALGO_XOR = 3;
        public const int DATA_ENCRYPT = 1;
        public const int DATA_DECRYPT = 2;
        public string SessionKey;
        public string CurrentIV;

        #region Library entry points
        [DllImport("chaindes.dll", EntryPoint = "Chain_DES")]
        public static extern int EncrypteDES(byte[] Text, byte[] Key, int Algo, int Block, int Methode);
        #endregion
        /// <summary>
        /// Initializes a new instance of the <see cref="testDesfire"/> class.
        /// </summary>
        /// <param name="readerID">The reader ID.</param>
        public testDesfire(uint readerID)
            : base(readerID)
        {
        }
        /// <summary>
        /// Exchanges an APDU and expects exactly one response byte with the specified value.
        /// </summary>
        /// <param name="cmdApdu">The command APDU.</param>
        /// <param name="expectedStatusResponse">The expected status response.</param>
        public void ExchangeApdu(CardData cmdApdu, byte expectedStatusResponse)
        {
            ExchangeApdu(cmdApdu, new CardData(new byte[] { expectedStatusResponse }));
        }
        /// <summary>
        /// Exchanges an APDU and expects exactly one response byte with the specified value.
        /// </summary>
        /// <param name="cmdApdu">The command APDU.</param>
        /// <param name="expected">The expected response APDU.</param>
        /// <returns>The card response</returns>
        public new CardData ExchangeApdu(CardData cmdApdu, CardData expected)
        {
            CardData response;
            response = ExchangeApdu(cmdApdu);

            response.Compare(expected, true);

            return response;
        }
        /// <summary>
        /// Selects the application.
        /// </summary>
        /// <param name="AID">The AID as three byte value like 000000.</param>
        public void SelectApplication(CardData AID)
        {
            // select AID 0x000000
            ExchangeApdu("5A" + AID, testDesfire.StatusResponseOk);
        }
        public void SelectApplication_AES(CardData AID)
        {
            // select AID 0x000000
            ExchangeApdu("5A" + AID, "00*");
        }
        //AID = 3 bytes
        //KeySet1 = 1 byte
        //KeySet2 = 1 byte
        public void CreateApplication(CardData AID, CardData KeySet1, CardData KeySet2)
        {
            ExchangeApdu("CA" + AID + KeySet1 + KeySet2, testDesfire.StatusResponseOk);
        }
        public void CreateApplication_ISO(CardData AID, CardData KeySet1, CardData KeySet2, CardData IsoID, CardData IsoDFName)
        {
            ExchangeApdu("CA" + AID + KeySet1 + KeySet2 + IsoID + IsoDFName, "00*");
        }
        public void CreateApplication_AES(CardData AID, CardData KeySet1, CardData KeySet2)
        {
            ExchangeApdu("CA" + AID + KeySet1 + KeySet2, "00*");
        }
        //FileNo = 1 byte
        //ComSet = 1 byte
        //AccessRights = 2 bytes
        //FileSize = 3 bytes
        public void CreateBackupFile(CardData FileNo, CardData ComSet, CardData AccessRights, CardData FileSize)
        {
            ExchangeApdu("CB" + FileNo + ComSet + AccessRights + FileSize, testDesfire.StatusResponseOk);
        }
        public void CreateBackupFile_AES(CardData FileNo, CardData ComSet, CardData AccessRights, CardData FileSize)
        {
            ExchangeApdu("CB" + FileNo + ComSet + AccessRights + FileSize, "00*");
        }
        public void CreateStdDataFile(CardData FileNo, CardData ComSet, CardData AccessRights, CardData FileSize)
        {
            ExchangeApdu("CD" + FileNo + ComSet + AccessRights + FileSize, "00*");
        }
        public void CreateStdDataFile_ISO(CardData FileNo, CardData FileIDIso, CardData ComSet, CardData AccessRights, CardData FileSize)
        {
            ExchangeApdu("CD" + FileNo + FileIDIso + ComSet + AccessRights + FileSize, "00*");
        }
        public void Format_PICC()
        {
            ExchangeApdu("FC", testDesfire.StatusResponseOk);
        }
        public void Format_PICC_AES()
        {
            ExchangeApdu("FC", "00*");
        }
        public void WriteData(CardData FileNo, CardData offset, CardData length, CardData data)
        {
            ExchangeApdu("3D" + FileNo + offset + length + data, "00*");
        }
        public void changeFileSettings(CardData FileNo, CardData comSet, CardData AccessRights)
        {
            ExchangeApdu("5F" + FileNo + comSet + AccessRights, "00*");
        }


        /// <summary>
        /// Gets the tag versions.
        /// </summary>
        /// <param name="hardware">The hardware version.</param>
        /// <param name="software">The software version.</param>
        /// <param name="serials">The serials numbers</param>
        public void GetVersion(out CardData hardware, out CardData software, out CardData serials)
        {
            // get hardware version
            hardware = ExchangeApdu("60", "AF*");
            hardware = hardware.SubArray(1, hardware.Length - 1, CardData.Datatype.Hex);

            // send "additional frame" command to retrieve software version
            software = ExchangeApdu("AF", "AF*");
            software = software.SubArray(1, software.Length - 1, CardData.Datatype.Hex);

            // send "additional frame" command to retrieve serial numbers
            serials = ExchangeApdu("AF", "00*");
            serials = serials.SubArray(1, serials.Length - 1, CardData.Datatype.Hex);
        }
        public byte[] OuExclusif(byte[] text1, byte[] text2)
        {
            int[] textXor = new int[8];
            byte[] textXor1 = new byte[8];
            int i;
            for (i = 0; i < text1.Length; i++)
            {
                textXor[i] = text1[i] ^ text2[i];
            }
            for (i = 0; i < textXor.Length; i++)
            {
                textXor1[i] = (byte)textXor[i];
            }
            return textXor1;
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
        public string BinVersHex(string ch_hex)
        {
            string aa;
            aa = "";
            switch (ch_hex)
            {
                case "00000000": aa = "00"; break;
                case "00000001": aa = "01"; break;
                case "00000010": aa = "02"; break;
                case "00000011": aa = "03"; break;
                case "00000100": aa = "04"; break;
                case "00000101": aa = "05"; break;
                case "00000110": aa = "06"; break;
                case "00000111": aa = "07"; break;
                case "00001000": aa = "08"; break;
                case "00001001": aa = "09"; break;
                case "00001010": aa = "0A"; break;
                case "00001011": aa = "0B"; break;
                case "00001100": aa = "0C"; break;
                case "00001101": aa = "0D"; break;
                case "00001110": aa = "0E"; break;
                case "00001111": aa = "0F"; break;
                case "00010000": aa = "10"; break;
                case "00010001": aa = "11"; break;
                case "00010010": aa = "12"; break;
                case "00010011": aa = "13"; break;
                case "00010100": aa = "14"; break;
                case "00010101": aa = "15"; break;
                case "00010110": aa = "16"; break;
                case "00010111": aa = "17"; break;
                case "00011000": aa = "18"; break;
                case "00011001": aa = "19"; break;
                case "00011010": aa = "1A"; break;
                case "00011011": aa = "1B"; break;
                case "00011100": aa = "1C"; break;
                case "00011101": aa = "1D"; break;
                case "00011110": aa = "1E"; break;
                case "00011111": aa = "1F"; break;
                case "00100000": aa = "20"; break;
                case "00100001": aa = "21"; break;
                case "00100010": aa = "22"; break;
                case "00100011": aa = "23"; break;
                case "00100100": aa = "24"; break;
                case "00100101": aa = "25"; break;
                case "00100110": aa = "26"; break;
                case "00100111": aa = "27"; break;
                case "00101000": aa = "28"; break;
                case "00101001": aa = "29"; break;
                case "00101010": aa = "2A"; break;
                case "00101011": aa = "2B"; break;
                case "00101100": aa = "2C"; break;
                case "00101101": aa = "2D"; break;
                case "00101110": aa = "2E"; break;
                case "00101111": aa = "2F"; break;
                case "00110000": aa = "30"; break;
                case "00110001": aa = "31"; break;
                case "00110010": aa = "32"; break;
                case "00110011": aa = "33"; break;
                case "00110100": aa = "34"; break;
                case "00110101": aa = "35"; break;
                case "00110110": aa = "36"; break;
                case "00110111": aa = "37"; break;
                case "00111000": aa = "38"; break;
                case "00111001": aa = "39"; break;
                case "00111010": aa = "3A"; break;
                case "00111011": aa = "3B"; break;
                case "00111100": aa = "3C"; break;
                case "00111101": aa = "3D"; break;
                case "00111110": aa = "3E"; break;
                case "00111111": aa = "3F"; break;
                case "01000000": aa = "40"; break;
                case "01000001": aa = "41"; break;
                case "01000010": aa = "42"; break;
                case "01000011": aa = "43"; break;
                case "01000100": aa = "44"; break;
                case "01000101": aa = "45"; break;
                case "01000110": aa = "46"; break;
                case "01000111": aa = "47"; break;
                case "01001000": aa = "48"; break;
                case "01001001": aa = "49"; break;
                case "01001010": aa = "4A"; break;
                case "01001011": aa = "4B"; break;
                case "01001100": aa = "4C"; break;
                case "01001101": aa = "4D"; break;
                case "01001110": aa = "4E"; break;
                case "01001111": aa = "4F"; break;
                case "01010000": aa = "50"; break;
                case "01010001": aa = "51"; break;
                case "01010010": aa = "52"; break;
                case "01010011": aa = "53"; break;
                case "01010100": aa = "54"; break;
                case "01010101": aa = "55"; break;
                case "01010110": aa = "56"; break;
                case "01010111": aa = "57"; break;
                case "01011000": aa = "58"; break;
                case "01011001": aa = "59"; break;
                case "01011010": aa = "5A"; break;
                case "01011011": aa = "5B"; break;
                case "01011100": aa = "5C"; break;
                case "01011101": aa = "5D"; break;
                case "01011110": aa = "5E"; break;
                case "01011111": aa = "5F"; break;
                case "01100000": aa = "60"; break;
                case "01100001": aa = "61"; break;
                case "01100010": aa = "62"; break;
                case "01100011": aa = "63"; break;
                case "01100100": aa = "64"; break;
                case "01100101": aa = "65"; break;
                case "01100110": aa = "66"; break;
                case "01100111": aa = "67"; break;
                case "01101000": aa = "68"; break;
                case "01101001": aa = "69"; break;
                case "01101010": aa = "6A"; break;
                case "01101011": aa = "6B"; break;
                case "01101100": aa = "6C"; break;
                case "01101101": aa = "6D"; break;
                case "01101110": aa = "6E"; break;
                case "01101111": aa = "6F"; break;
                case "01110000": aa = "70"; break;
                case "01110001": aa = "71"; break;
                case "01110010": aa = "72"; break;
                case "01110011": aa = "73"; break;
                case "01110100": aa = "74"; break;
                case "01110101": aa = "75"; break;
                case "01110110": aa = "76"; break;
                case "01110111": aa = "77"; break;
                case "01111000": aa = "78"; break;
                case "01111001": aa = "79"; break;
                case "01111010": aa = "7A"; break;
                case "01111011": aa = "7B"; break;
                case "01111100": aa = "7C"; break;
                case "01111101": aa = "7D"; break;
                case "01111110": aa = "7E"; break;
                case "01111111": aa = "7F"; break;
                case "10000000": aa = "80"; break;
                case "10000001": aa = "81"; break;
                case "10000010": aa = "82"; break;
                case "10000011": aa = "83"; break;
                case "10000100": aa = "84"; break;
                case "10000101": aa = "85"; break;
                case "10000110": aa = "86"; break;
                case "10000111": aa = "87"; break;
                case "10001000": aa = "88"; break;
                case "10001001": aa = "89"; break;
                case "10001010": aa = "8A"; break;
                case "10001011": aa = "8B"; break;
                case "10001100": aa = "8C"; break;
                case "10001101": aa = "8D"; break;
                case "10001110": aa = "8E"; break;
                case "10001111": aa = "8F"; break;
                case "10010000": aa = "90"; break;
                case "10010001": aa = "91"; break;
                case "10010010": aa = "92"; break;
                case "10010011": aa = "93"; break;
                case "10010100": aa = "94"; break;
                case "10010101": aa = "95"; break;
                case "10010110": aa = "96"; break;
                case "10010111": aa = "97"; break;
                case "10011000": aa = "98"; break;
                case "10011001": aa = "99"; break;
                case "10011010": aa = "9A"; break;
                case "10011011": aa = "9B"; break;
                case "10011100": aa = "9C"; break;
                case "10011101": aa = "9D"; break;
                case "10011110": aa = "9E"; break;
                case "10011111": aa = "9F"; break;
                case "10100000": aa = "A0"; break;
                case "10100001": aa = "A1"; break;
                case "10100010": aa = "A2"; break;
                case "10100011": aa = "A3"; break;
                case "10100100": aa = "A4"; break;
                case "10100101": aa = "A5"; break;
                case "10100110": aa = "A6"; break;
                case "10100111": aa = "A7"; break;
                case "10101000": aa = "A8"; break;
                case "10101001": aa = "A9"; break;
                case "10101010": aa = "AA"; break;
                case "10101011": aa = "AB"; break;
                case "10101100": aa = "AC"; break;
                case "10101101": aa = "AD"; break;
                case "10101110": aa = "AE"; break;
                case "10101111": aa = "AF"; break;
                case "10110000": aa = "B0"; break;
                case "10110001": aa = "B1"; break;
                case "10110010": aa = "B2"; break;
                case "10110011": aa = "B3"; break;
                case "10110100": aa = "B4"; break;
                case "10110101": aa = "B5"; break;
                case "10110110": aa = "B6"; break;
                case "10110111": aa = "B7"; break;
                case "10111000": aa = "B8"; break;
                case "10111001": aa = "B9"; break;
                case "10111010": aa = "BA"; break;
                case "10111011": aa = "BB"; break;
                case "10111100": aa = "BC"; break;
                case "10111101": aa = "BD"; break;
                case "10111110": aa = "BE"; break;
                case "10111111": aa = "BF"; break;
                case "11000000": aa = "C0"; break;
                case "11000001": aa = "C1"; break;
                case "11000010": aa = "C2"; break;
                case "11000011": aa = "C3"; break;
                case "11000100": aa = "C4"; break;
                case "11000101": aa = "C5"; break;
                case "11000110": aa = "C6"; break;
                case "11000111": aa = "C7"; break;
                case "11001000": aa = "C8"; break;
                case "11001001": aa = "C9"; break;
                case "11001010": aa = "CA"; break;
                case "11001011": aa = "CB"; break;
                case "11001100": aa = "CC"; break;
                case "11001101": aa = "CD"; break;
                case "11001110": aa = "CE"; break;
                case "11001111": aa = "CF"; break;
                case "11010000": aa = "D0"; break;
                case "11010001": aa = "D1"; break;
                case "11010010": aa = "D2"; break;
                case "11010011": aa = "D3"; break;
                case "11010100": aa = "D4"; break;
                case "11010101": aa = "D5"; break;
                case "11010110": aa = "D6"; break;
                case "11010111": aa = "D7"; break;
                case "11011000": aa = "D8"; break;
                case "11011001": aa = "D9"; break;
                case "11011010": aa = "DA"; break;
                case "11011011": aa = "DB"; break;
                case "11011100": aa = "DC"; break;
                case "11011101": aa = "DD"; break;
                case "11011110": aa = "DE"; break;
                case "11011111": aa = "DF"; break;
                case "11100000": aa = "E0"; break;
                case "11100001": aa = "E1"; break;
                case "11100010": aa = "E2"; break;
                case "11100011": aa = "E3"; break;
                case "11100100": aa = "E4"; break;
                case "11100101": aa = "E5"; break;
                case "11100110": aa = "E6"; break;
                case "11100111": aa = "E7"; break;
                case "11101000": aa = "E8"; break;
                case "11101001": aa = "E9"; break;
                case "11101010": aa = "EA"; break;
                case "11101011": aa = "EB"; break;
                case "11101100": aa = "EC"; break;
                case "11101101": aa = "ED"; break;
                case "11101110": aa = "EE"; break;
                case "11101111": aa = "EF"; break;
                case "11110000": aa = "F0"; break;
                case "11110001": aa = "F1"; break;
                case "11110010": aa = "F2"; break;
                case "11110011": aa = "F3"; break;
                case "11110100": aa = "F4"; break;
                case "11110101": aa = "F5"; break;
                case "11110110": aa = "F6"; break;
                case "11110111": aa = "F7"; break;
                case "11111000": aa = "F8"; break;
                case "11111001": aa = "F9"; break;
                case "11111010": aa = "FA"; break;
                case "11111011": aa = "FB"; break;
                case "11111100": aa = "FC"; break;
                case "11111101": aa = "FD"; break;
                case "11111110": aa = "FE"; break;
                case "11111111": aa = "FF"; break;
            }
            return aa;
        }
        public string Chaine_OuExclusif(string text1, string text2)
        {
            int[] Tab_textXor = new int[text1.Length / 2];
            int[] Tab_text1 = new int[text1.Length / 2];
            int[] Tab_text2 = new int[text1.Length / 2];
            byte[] textXor1 = new byte[text1.Length / 2];
            int i, j;
            string chaine_renvoie;
            chaine_renvoie = "";
            j = 0;
            for (i = 0; i < text1.Length; i = i + 2)
            {
                Tab_text1[j] = int.Parse(text1.Substring(i, 2), System.Globalization.NumberStyles.HexNumber);
                j++;
            }
            j = 0;
            for (i = 0; i < text2.Length; i = i + 2)
            {
                Tab_text2[j] = int.Parse(text2.Substring(i, 2), System.Globalization.NumberStyles.HexNumber);
                j++;
            }

            for (i = 0; i < Tab_text1.Length; i++)
            {
                Tab_textXor[i] = Tab_text1[i] ^ Tab_text2[i];
            }

            for (i = 0; i < Tab_textXor.Length; i++)
            {
                textXor1[i] = (byte)Tab_textXor[i];
            }

            for (i = 0; i < textXor1.Length; i++)
            {
                chaine_renvoie = chaine_renvoie + textXor1[i].ToString("X2");
            }

            return chaine_renvoie;
        }
        public string ComputeCrc16(string buffer)
        {
            int k, Char, wCrc;
            string Ch_CRC;
            string Chaine_Renvoie = "";

            wCrc = 0x6363;
            k = new int();
            k = 1;
            for (k = 0; k < buffer.Length - 1; k = k + 2)
            {
                Char = int.Parse(buffer.Substring(k, 2), System.Globalization.NumberStyles.HexNumber);
                wCrc = UpdateDesfireCrc16(Char, wCrc);
            }
            Ch_CRC = wCrc.ToString("X2");

            for (k = Ch_CRC.Length - 1; k > 0; k = k - 2)
            {
                Chaine_Renvoie = Chaine_Renvoie + Ch_CRC.Substring(k - 1, 2);
            }
            return Chaine_Renvoie;
        }
        public int UpdateDesfireCrc16(int Ch, int lpwCrc)
        {
            int Ch1, Ch2;
            byte Ch3;

            Ch1 = Ch ^ (lpwCrc & 0x00FF);
            Ch2 = Ch1 << 4;
            Ch3 = (byte)(Ch1 ^ Ch2);
            lpwCrc = (lpwCrc >> 8) ^ (Ch3 << 8) ^ (Ch3 << 3) ^ (Ch3 >> 4);
            return lpwCrc;
        }

        public string ComputeCrc32(string buffer)
        {
            int k, Char;
            long wCrc;
            string Ch_CRC;
            string Chaine_Renvoie = "";

            wCrc = 0xFFFFFFFF;

            k = 1;
            for (k = 0; k < buffer.Length - 1; k = k + 2)
            {
                Char = int.Parse(buffer.Substring(k, 2), System.Globalization.NumberStyles.HexNumber);
                wCrc = UpdateDesfireCrc32(Char, wCrc);
            }
            Ch_CRC = wCrc.ToString("X2");

            StringBuilder builder = new StringBuilder(Ch_CRC);
            string nouvelleChaine = builder.Insert(0, "0", 8 - Ch_CRC.Length).ToString();
            Ch_CRC = nouvelleChaine;


            for (k = Ch_CRC.Length - 1; k > 0; k = k - 2)
            {
                Chaine_Renvoie = Chaine_Renvoie + Ch_CRC.Substring(k - 1, 2);
            }
            return Chaine_Renvoie;
        }

        public long UpdateDesfireCrc32(int Ch, long lpwCrc)
        {
            int i;

            lpwCrc = lpwCrc ^ Ch;
            for (i = 0; i < 8; i++)
            {
                if ((lpwCrc & 0x00000001) == 1)
                {
                    lpwCrc = lpwCrc >> 1;
                    lpwCrc = lpwCrc ^ 0xEDB88320;
                }
                else
                    lpwCrc = lpwCrc >> 1;
            }
            return lpwCrc;
        }
        public string Generate_K1_K2(string Cle, int Type_Cle)
        {
            //Type_Cle 
            //0 = DES
            //1 = 2KTDES
            //2 = 3KTDES
            //3 = AES128
            //4 = AES192
            long Bit_Cle;
            ulong A1, A2, Rep;
            string ChaineResult, k0, k1, k2, Chaine_Bits, Chaine_Hex;
            uint Rb64, Rb128;
            int i;
            BitArray ba;
            ChaineResult = ""; k0 = ""; k1 = ""; k2 = ""; Chaine_Bits = "";
            k0 = ""; k1 = ""; k2 = ""; Chaine_Bits = "";
            ////////////////////////// CIPHK(0b) //////////////////////////////////////
            switch (Type_Cle)
            {
                case 0: // DES
                case 1: // 2KTDES
                    if (Cle.Length != 32) ChaineResult = "La Cle 2KTDES doit est egal a 16 Bytes.";//goto fini;
                    k0 = TDES_Standard_Enciphering("0000000000000000", Cle, "0000000000000000", 2);
                    break;
                case 2: // 3KTDES
                    if (Cle.Length != 48) ChaineResult = "La Cle 3KTDES doit est egal a 24 Bytes.";//goto fini;
                    k0 = TDES_Standard_Enciphering("0000000000000000", Cle, "0000000000000000", 2);
                    break;
                case 3:
                    if (Cle.Length != 32) ChaineResult = "La Cle AES128 doit est egal a 16 Bytes.";//goto fini;
                    k0 = AES_Enciphering("00000000000000000000000000000000", Cle, "00000000000000000000000000000000", 2);
                    break;
                case 4:
                    if (Cle.Length != 48) ChaineResult = "La Cle AES128 doit est egal a 24 Bytes.";//goto fini;
                    k0 = AES_Enciphering("00000000000000000000000000000000", Cle, "00000000000000000000000000000000", 2);
                    break;
            }


            /////////////////////////////// Rb128 et Rb64 ////////////////////
            Rb64 = 27;
            Rb128 = 135;

            switch (Type_Cle)
            {
                case 1:
                case 2: // 2KTDES, 3KTDES
                        /////////////////////////////// K1 //////////////////////////
                    Bit_Cle = int.Parse(k0.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
                    A1 = (ulong)int.Parse(k0, System.Globalization.NumberStyles.HexNumber);
                    A2 = 1;
                    ba = new BitArray(new[] { (int)Bit_Cle });
                    switch (ba[8])
                    {
                        case true:
                            Rep = A1 << (int)A2;
                            k1 = Rep.ToString("X2");
                            Rep = (ulong)int.Parse(k1.Substring(k1.Length - 2, 2), System.Globalization.NumberStyles.HexNumber);
                            Rep = Rep ^ Rb64;
                            k1 = k1.Substring(0, k1.Length - 2) + Rep.ToString("X2");
                            break;
                        case false:
                            Rep = A1 << (int)A2;
                            k1 = Rep.ToString("X2");
                            break;
                    }
                    /////////////////////////////// K2 /////////////////////////
                    Bit_Cle = int.Parse(k1.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
                    A1 = (ulong)int.Parse(k1, System.Globalization.NumberStyles.HexNumber);
                    A2 = 1;
                    ba = new BitArray(new[] { (int)Bit_Cle });

                    switch (ba[8])
                    {
                        case true:
                            Rep = A1 << (int)A2;
                            k2 = Rep.ToString("X2");
                            Rep = (ulong)int.Parse(k2.Substring(k2.Length - 2, 2), System.Globalization.NumberStyles.HexNumber);
                            Rep = Rep ^ Rb64;
                            k2 = k2.Substring(0, k2.Length - 2) + Rep.ToString("X2");
                            break;
                        case false:
                            Rep = A1 << (int)A2;
                            k2 = Rep.ToString("X2");
                            break;
                    }
                    break;
                case 3:
                case 4: // AES128, AES192
                        ////////////////////////////// K1 //////////////////////////////////
                    for (i = 0; i < k0.Length; i = i + 2)
                    {
                        Chaine_Bits = Chaine_Bits + HexVersBin(k0.Substring(i, 2));
                    }
                    switch (Chaine_Bits.Substring(0, 1))
                    {
                        case "1":
                            Chaine_Bits = Chaine_Bits.Substring(1, Chaine_Bits.Length - 1) + "0";
                            Chaine_Hex = BinVersHex(Chaine_Bits.Substring(Chaine_Bits.Length - 8, 8));
                            Rep = (ulong)int.Parse(Chaine_Hex, System.Globalization.NumberStyles.HexNumber);
                            Rep = Rep ^ Rb128;
                            Chaine_Hex = Rep.ToString("X");
                            for (i = 0; i < Chaine_Bits.Length - 8; i = i + 8)
                            {
                                k1 = k1 + BinVersHex(Chaine_Bits.Substring(i, 8));
                            }
                            k1 = k1 + Chaine_Hex;
                            break;
                        case "0":
                            Chaine_Bits = Chaine_Bits.Substring(1, Chaine_Bits.Length - 1) + "0";
                            for (i = 0; i < Chaine_Bits.Length; i = i + 8)
                            {
                                k1 = k1 + BinVersHex(Chaine_Bits.Substring(i, 8));
                            }
                            break;
                    }
                    ///////////////////////////// K2 /////////////////////////////////////
                    Chaine_Bits = ""; Chaine_Hex = "";
                    for (i = 0; i < k1.Length; i = i + 2)
                    {
                        Chaine_Bits = Chaine_Bits + HexVersBin(k1.Substring(i, 2));
                    }
                    switch (Chaine_Bits.Substring(0, 1))
                    {
                        case "1":
                            Chaine_Bits = Chaine_Bits.Substring(1, Chaine_Bits.Length - 1) + "0";
                            Chaine_Hex = BinVersHex(Chaine_Bits.Substring(Chaine_Bits.Length - 8, 8));
                            Rep = (ulong)int.Parse(Chaine_Hex, System.Globalization.NumberStyles.HexNumber);
                            Rep = Rep ^ Rb128;
                            Chaine_Hex = Rep.ToString("X2");
                            for (i = 0; i < Chaine_Bits.Length - 8; i = i + 8)
                            {
                                k2 = k2 + BinVersHex(Chaine_Bits.Substring(i, 8));
                            }
                            k2 = k2 + Chaine_Hex;
                            break;
                        case "0":
                            Chaine_Bits = Chaine_Bits.Substring(1, Chaine_Bits.Length - 1) + "0";
                            for (i = 0; i < Chaine_Bits.Length; i = i + 8)
                            {
                                k2 = k2 + BinVersHex(Chaine_Bits.Substring(i, 8));
                            }
                            break;
                    }
                    break;
            }
            ChaineResult = k0 + k1 + k2;
            return ChaineResult;

        }
        public string Generate_Mac(string Text, string Cle, bool Padding, int Type_Cle, string InitVecteur)
        {
            //Type_Cle
            //0 = DES
            //1 = 2KTDES
            //2 = 3KTDES
            //3 = AES128
            //4 = AES192
            string K0, K1, K2;
            string Chaine_Renvoie, Chaine_Result, Dernier_8_Bytes, Dernier_16_Bytes, ChaineXOR, Chaine_Traiter;
            int Nb_Block;
            Chaine_Renvoie = "";
            ChaineXOR = ""; K0 = ""; K1 = ""; K2 = "";
            Chaine_Traiter = Text;
            if (Padding == true)
            {
                switch (Type_Cle)
                {
                    case 0:
                    case 1:
                    case 2:
                        if (Chaine_Traiter == "")
                        {
                            Chaine_Traiter = "8000000000000000";
                        }
                        else
                        {
                            Nb_Block = Chaine_Traiter.Length % 16;
                            if (Nb_Block != 0) Chaine_Traiter = Chaine_Traiter + "80";
                            Nb_Block = Chaine_Traiter.Length % 16;
                            while ((Nb_Block < 16) && (Nb_Block != 0))
                            {
                                Chaine_Traiter = Chaine_Traiter + "0";
                                Nb_Block++;
                            }
                        }
                        break;
                    case 3:
                    case 4:
                        if (Chaine_Traiter == "")
                        {
                            Chaine_Traiter = "80000000000000000000000000000000";
                        }
                        else
                        {
                            Nb_Block = Chaine_Traiter.Length % 32;
                            if (Nb_Block != 0) Chaine_Traiter = Chaine_Traiter + "80";
                            Nb_Block = Chaine_Traiter.Length % 32;
                            while ((Nb_Block < 32) && (Nb_Block != 0))
                            {
                                Chaine_Traiter = Chaine_Traiter + "0";
                                Nb_Block++;
                            }
                        }
                        break;
                }
            }
            switch (Type_Cle)
            {
                case 0: // DES
                    break;
                case 1: // 2KTDES
                    if ((Chaine_Traiter.Length % 16) != 0) Chaine_Renvoie = "La chaine MAC doit etre un multiple de 8 bytes";//goto fini;
                    if (Cle.Length != 32) Chaine_Renvoie = "La cle doit etre egal a 16 Bytes";//goto fini;
                    Chaine_Result = Generate_K1_K2(Cle, 1);
                    K0 = Chaine_Result.Substring(0, 32);
                    K1 = Chaine_Result.Substring(32, 32);
                    K2 = Chaine_Result.Substring(64, 32);
                    Dernier_8_Bytes = Chaine_Traiter.Substring(Chaine_Traiter.Length - 16, 16);
                    switch (Padding)
                    {
                        case false: ChaineXOR = Chaine_OuExclusif(Dernier_8_Bytes, K1); break;
                        case true: ChaineXOR = Chaine_OuExclusif(Dernier_8_Bytes, K2); break;
                    }
                    Chaine_Result = ""; Chaine_Result = Text.Substring(0, Chaine_Traiter.Length - 16) + ChaineXOR;
                    Chaine_Traiter = ""; Chaine_Traiter = Chaine_Result;
                    Chaine_Result = ""; Chaine_Result = TDES_Standard_Enciphering(Chaine_Traiter, Cle, InitVecteur, 2);
                    Chaine_Renvoie = Chaine_Result.Substring(Chaine_Result.Length - 16, 16);
                    break;
                case 2: // 3KTDES
                    if ((Chaine_Traiter.Length % 16) != 0) Chaine_Renvoie = "La chaine MAC doit �tre un multiple de 8 bytes";//goto fini;
                    if (Cle.Length != 48) Chaine_Renvoie = "La cle doit etre egal a 16 Bytes";//goto fini;
                    Chaine_Result = Generate_K1_K2(Cle, 2);
                    K0 = Chaine_Result.Substring(0, 32);
                    K1 = Chaine_Result.Substring(32, 32);
                    K2 = Chaine_Result.Substring(64, 32);
                    Dernier_8_Bytes = Chaine_Traiter.Substring(Chaine_Traiter.Length - 16, 16);
                    switch (Padding)
                    {
                        case false: ChaineXOR = Chaine_OuExclusif(Dernier_8_Bytes, K1); break;
                        case true: ChaineXOR = Chaine_OuExclusif(Dernier_8_Bytes, K2); break;
                    }
                    Chaine_Result = ""; Chaine_Result = Text.Substring(0, Chaine_Traiter.Length - 16) + ChaineXOR;
                    Chaine_Traiter = ""; Chaine_Traiter = Chaine_Result;
                    Chaine_Result = ""; Chaine_Result = TDES_Standard_Enciphering(Chaine_Traiter, Cle, InitVecteur, 2);
                    Chaine_Renvoie = Chaine_Result.Substring(Chaine_Result.Length - 16, 16);
                    break;
                case 3: // AES128                    
                    if ((Chaine_Traiter.Length % 32) != 0) Chaine_Renvoie = "La chaine MAC doit etre un multiple de 16 bytes";//goto fini;
                    if (Cle.Length != 32) Chaine_Renvoie = "La cle doit etre egal a 16 Bytes";//goto fini;                    
                    Chaine_Result = Generate_K1_K2(Cle, 3);
                    K0 = Chaine_Result.Substring(0, 32);
                    K1 = Chaine_Result.Substring(32, 32);
                    K2 = Chaine_Result.Substring(64, 32);
                    Dernier_16_Bytes = Chaine_Traiter.Substring(Chaine_Traiter.Length - 32, 32);
                    switch (Padding)
                    {
                        case false: ChaineXOR = Chaine_OuExclusif(Dernier_16_Bytes, K1); break;
                        case true: ChaineXOR = Chaine_OuExclusif(Dernier_16_Bytes, K2); break;
                    }
                    Chaine_Result = ""; Chaine_Result = Text.Substring(0, Chaine_Traiter.Length - 32) + ChaineXOR;
                    Chaine_Traiter = ""; Chaine_Traiter = Chaine_Result;
                    Chaine_Result = ""; Chaine_Result = AES_Enciphered_CBCMode(Chaine_Traiter, Cle, InitVecteur, 2);
                    Chaine_Renvoie = Chaine_Result.Substring(Chaine_Result.Length - 32, 32);
                    break;
                case 4: // AES192	
                    break;
            }
            //fini:
            return Chaine_Renvoie;

        }
        public string DESFire_Authentification(string NumKey, CardData Key, int Mode)
        {
            //Mode = 1=TDES,2=TDES Standard mode,3=AES
            int StatusReponse;
            string RNDA, RNDA_Crypter, RNDA_Decrypter, RNDB, RNDB_Bis, RNDA_RNDB_Bis, RNDB_Decrypter, RNDA_RNDB_Bis_Crypter, IniVector;
            string rep;
            RNDB = "";
            RNDB_Decrypter = "";
            RNDA_Crypter = "";
            rep = "";
            StatusReponse = 0;
            if (NumKey.Length < 2) NumKey = "0" + NumKey;
            switch (Mode)
            {
                case 1: //DESFire Native mode
                    RNDA = "1122334455667788";
                    IniVector = "0000000000000000";
                    SessionKey = RNDA.Substring(0, 8);
                    RNDB = ExchangeApdu("0A" + NumKey, "AF*");
                    RNDB = RNDB.Substring(2, RNDB.Length - 2);
                    RNDB_Decrypter = TDES_Standard_Enciphering(RNDB, Key, IniVector, 1);
                    if (RNDB_Decrypter != "")
                    {
                        SessionKey = SessionKey + RNDB_Decrypter.Substring(0, 8);
                        RNDB_Bis = RNDB_Decrypter.Substring(2, RNDB.Length - 2) + RNDB_Decrypter.Substring(0, 2);
                        RNDA_RNDB_Bis = RNDA + RNDB_Bis;
                        IniVector = "0000000000000000";
                        RNDA_RNDB_Bis_Crypter = DESFire_Native_Enciphering(RNDA_RNDB_Bis, Key, IniVector, 2);
                        RNDA_Crypter = "";
                        RNDA_Crypter = ExchangeApdu("AF" + RNDA_RNDB_Bis_Crypter, "00*");
                        RNDA_Crypter = RNDA_Crypter.Substring(2, RNDA_Crypter.Length - 2);
                        IniVector = "0000000000000000";
                        RNDA_Decrypter = DESFire_Native_Enciphering(RNDA_Crypter, Key, IniVector, 1);
                        if (RNDA_Decrypter != "")
                        {
                            SessionKey = SessionKey + SessionKey;
                            RNDA_Decrypter = RNDA_Decrypter.Substring(RNDA_Decrypter.Length - 2, 2) + RNDA_Decrypter.Substring(0, RNDA_Decrypter.Length - 2);
                            rep = RNDA_Decrypter;
                            if (RNDA != RNDA_Crypter) StatusReponse = 2;
                        }
                        else
                            StatusReponse = 1;
                    }
                    else
                        StatusReponse = 1;
                    break;
                case 2: //TDES Standard mode
                    break;
                case 3: //AES  
                    string[] Tab_PlainText;
                    string textXor, chaine_crypter;
                    RNDA = "00112233445566778899AABBCCDDEEFF";
                    IniVector = "00000000000000000000000000000000";
                    SessionKey = RNDA.Substring(0, 8);
                    RNDB = ExchangeApdu("AA" + NumKey, "AF*");
                    RNDB = RNDB.Substring(2, RNDB.Length - 2);
                    RNDB_Decrypter = AES_Enciphering(RNDB, Key, IniVector, 1);
                    IniVector = RNDB;
                    if (RNDB_Decrypter != "")
                    {
                        SessionKey = SessionKey + RNDB_Decrypter.Substring(0, 8) + RNDA.Substring(24, 8) + RNDB_Decrypter.Substring(24, 8);
                        RNDB_Bis = RNDB_Decrypter.Substring(2, RNDB.Length - 2) + RNDB_Decrypter.Substring(0, 2);
                        RNDA_RNDB_Bis = RNDA + RNDB_Bis;
                        Tab_PlainText = new string[2];
                        Tab_PlainText[0] = RNDA_RNDB_Bis.Substring(0, 32);
                        Tab_PlainText[1] = RNDA_RNDB_Bis.Substring(32, 32);
                        textXor = Chaine_OuExclusif(Tab_PlainText[0], IniVector);
                        IniVector = "00000000000000000000000000000000";
                        chaine_crypter = AES_Enciphering(textXor, Key, IniVector, 2);
                        Tab_PlainText[0] = chaine_crypter;
                        IniVector = Tab_PlainText[0];
                        textXor = Chaine_OuExclusif(Tab_PlainText[1], IniVector);
                        IniVector = "00000000000000000000000000000000";
                        chaine_crypter = AES_Enciphering(textXor, Key, IniVector, 2);
                        Tab_PlainText[1] = chaine_crypter;
                        IniVector = Tab_PlainText[1];
                        //CurrentIV = IniVector;
                        RNDA_RNDB_Bis_Crypter = Tab_PlainText[0] + Tab_PlainText[1];
                        RNDA_Crypter = "";
                        RNDA_Crypter = ExchangeApdu("AF" + RNDA_RNDB_Bis_Crypter, "00*");
                        RNDA_Crypter = RNDA_Crypter.Substring(2, RNDA_Crypter.Length - 2);
                        IniVector = "00000000000000000000000000000000";
                        RNDA_Decrypter = AES_Enciphering(RNDA_Crypter, Key, IniVector, 1);
                        textXor = Chaine_OuExclusif(RNDA_Decrypter, Tab_PlainText[1]);
                        RNDA_Decrypter = textXor;
                        if (RNDA_Decrypter != "")
                        {
                            RNDA_Decrypter = RNDA_Decrypter.Substring(RNDA_Decrypter.Length - 2, 2) + RNDA_Decrypter.Substring(0, RNDA_Decrypter.Length - 2);
                            rep = RNDA_Decrypter;
                            if (RNDA != RNDA_Crypter) StatusReponse = 2;
                        }
                        else
                            StatusReponse = 1;
                    }
                    else
                        StatusReponse = 1;
                    break;

            }

            //return StatusReponse;
            return rep;
        }
        public string DESFire_ChangeKey(string keyNumber, string KeyVersion, string KeyOld, string KeyNew, int Methode, string IV)
        {
            string Crc16, Crc32, Padding, Chaine_A_Crypter, Chaine_Crypter, InitVector, TextXor, CrcNewKey, Key;

            Chaine_Crypter = ""; InitVector = "";
            switch (Methode)
            {
                case 1: //DESFire Native TDES mode n� KeyNum != n� Key Authentifier
                case 2: //DESFire Native TDES mode n� KeyNum == n� Key Authentifier
                    Crc16 = ComputeCrc16(KeyNew);
                    Padding = "000000000000";
                    Chaine_A_Crypter = KeyNew + Crc16 + Padding;
                    InitVector = "0000000000000000";
                    Chaine_Crypter = DESFire_Native_Enciphering(Chaine_A_Crypter, SessionKey, InitVector, 2);
                    ExchangeApdu("C400" + Chaine_Crypter, testDesfire.StatusResponseOk);
                    break;

                case 3: //Change PICC Master Key to AES cas 2 Authentifier avec TDES Standars mode

                case 4: //Change PICC Master Key to AES cas 2 Authentifier avec DESFire TDES native mode
                    Crc16 = ComputeCrc16(KeyNew + KeyVersion);
                    Padding = "0000000000";
                    Chaine_A_Crypter = KeyNew + KeyVersion + Crc16 + Padding;
                    InitVector = "0000000000000000";
                    Chaine_Crypter = DESFire_Native_Enciphering(Chaine_A_Crypter, SessionKey, InitVector, 2);
                    ExchangeApdu("C480" + Chaine_Crypter, testDesfire.StatusResponseOk);
                    break;
                case 5: //Change PICC Master Key AES cas 2 Authentifier avec AES mode
                    Crc32 = ComputeCrc32("C480" + KeyNew + KeyVersion);
                    Padding = "0000000000000000000000";
                    Chaine_A_Crypter = KeyNew + KeyVersion + Crc32 + Padding;
                    InitVector = "00000000000000000000000000000000";
                    Chaine_Crypter = AES_Enciphered_CBCMode(Chaine_A_Crypter, SessionKey, InitVector, 2);
                    ExchangeApdu("C480" + Chaine_Crypter, testDesfire.StatusResponseOk);
                    break;
                case 6: //AES mode num KeyChange != num Key Authentifier
                    TextXor = Chaine_OuExclusif(KeyOld, KeyNew);
                    CrcNewKey = ComputeCrc32(KeyNew);
                    Crc32 = ComputeCrc32("C4" + keyNumber + TextXor + KeyVersion);
                    Padding = "00000000000000";
                    Chaine_A_Crypter = TextXor + KeyVersion + Crc32 + CrcNewKey + Padding;
                    if (IV == "") InitVector = "00000000000000000000000000000000";
                    else InitVector = IV;
                    Chaine_Crypter = AES_Enciphered_CBCMode(Chaine_A_Crypter, SessionKey, InitVector, 2);
                    ExchangeApdu("C4" + keyNumber + Chaine_Crypter, "00*");
                    InitVector = CurrentIV;
                    CurrentIV = Generate_Mac("00", SessionKey, true, 3, InitVector);
                    break;
                case 7: //AES mode num KeyChange == num Key Authentifier
                    Crc32 = ComputeCrc32("C4" + keyNumber + KeyNew + KeyVersion);
                    Padding = "0000000000000000000000";
                    Chaine_A_Crypter = KeyNew + KeyVersion + Crc32 + Padding;
                    InitVector = "00000000000000000000000000000000";
                    Chaine_Crypter = AES_Enciphered_CBCMode(Chaine_A_Crypter, SessionKey, InitVector, 2);
                    ExchangeApdu("C4" + keyNumber + Chaine_Crypter, testDesfire.StatusResponseOk);

                    break;
            }
            return Chaine_Crypter;
        }
        public string DESFire_ChangeKeySetting(string keySetting, int Methode, string IV)
        {
            string Crc16, Crc32, Padding, Chaine_A_Crypter, Chaine_Crypter, InitVector, TextXor, CrcNewKey, Key;

            Chaine_Crypter = ""; InitVector = "";
            switch (Methode)
            {
                case 1: //DESFire Native TDES mode n� KeyNum != n� Key Authentifier
                case 2: //DESFire Native TDES mode n� KeyNum == n� Key Authentifier
                    Crc16 = ComputeCrc16(keySetting);
                    Padding = "0000000000";
                    Chaine_A_Crypter = keySetting + Crc16 + Padding;
                    InitVector = "0000000000000000";
                    Chaine_Crypter = DESFire_Native_Enciphering(Chaine_A_Crypter, SessionKey, InitVector, 2);
                    ExchangeApdu("54" + Chaine_Crypter, "00*");
                    break;
                case 3: //Change PICC Master Key to AES cas 2 Authentifier avec TDES Standars mode

                case 4: //Change PICC Master Key to AES cas 2 Authentifier avec DESFire TDES native mode

                case 5: //Change PICC Master Key AES cas 2 Authentifier avec AES mode

                case 6: //AES mode num KeyChange != num Key Authentifier

                case 7: //AES mode num KeyChange == num Key Authentifier
                    Crc32 = ComputeCrc32("54" + keySetting);
                    Padding = "0000000000000000000000";
                    Chaine_A_Crypter = keySetting + Crc32 + Padding;
                    InitVector = "00000000000000000000000000000000";
                    Chaine_Crypter = AES_Enciphered_CBCMode(Chaine_A_Crypter, SessionKey, InitVector, 2);
                    ExchangeApdu("54" + Chaine_Crypter, "00*");
                    break;
            }
            return Chaine_Crypter;
        }

        public string DESFire_Native_Enciphering(CardData PlainText, CardData Key, CardData IniVector, int Mode)
        {
            //PlainText = Chaine a crypter
            //Key = Cl� de cryptage
            //IniVector = Vecteur d'initialisation
            //Mode = 1=Decrypte --> Receive mode, 2=Encrypte --> Send Mode
            string Chaine_renvoie;
            byte[] Text_A_Crypter;
            byte[] Tab_Key;
            byte[] Tab_Xor;
            byte[] Tab_InitVector;
            byte[] Tab_PlainText = new byte[10];
            int TaillePlainText, NbCrypte, index, j, StatusReponse, i;

            TaillePlainText = PlainText.Length;
            Chaine_renvoie = "";
            switch (Mode)
            {
                case 1: //Deciphering in receive mode
                    NbCrypte = TaillePlainText % 8;
                    if (NbCrypte != 0) Chaine_renvoie = "Erreur Taille PlainText";
                    NbCrypte = TaillePlainText / 8;
                    Tab_Key = new byte[16];
                    Tab_Key = Key.SubArray(0, 16, CardData.Datatype.Hex);
                    Tab_InitVector = new byte[8];
                    Tab_InitVector = IniVector.SubArray(0, 8, CardData.Datatype.Hex);
                    j = 0;
                    for (index = 0; index < NbCrypte; index++)
                    {
                        Text_A_Crypter = new byte[8];
                        Text_A_Crypter = PlainText.SubArray(j, 8, CardData.Datatype.Hex);
                        if (index != 0)
                        {
                            Tab_InitVector = new byte[8];
                            Tab_InitVector = PlainText.SubArray(j - 8, 8, CardData.Datatype.Hex);
                            for (i = 0; i < Tab_InitVector.Length; i++)
                            {
                                CurrentIV = CurrentIV + Tab_InitVector[i].ToString("X2");
                            }
                        }
                        StatusReponse = testDesfire.EncrypteDES(Text_A_Crypter, Tab_Key, 1, 1, 2);
                        if (StatusReponse == 0)
                        {
                            Tab_Xor = new byte[8];
                            Tab_Xor = OuExclusif(Text_A_Crypter, Tab_InitVector);
                            Text_A_Crypter = new byte[8];
                            Text_A_Crypter = Tab_Xor;
                            for (i = 0; i < Text_A_Crypter.Length; i++)
                            {
                                Chaine_renvoie = Chaine_renvoie + Text_A_Crypter[i].ToString("X2");
                            }
                            j = j + 8;
                        }
                        else
                            Chaine_renvoie = "";
                    }
                    break;
                case 2: //Enciphering in Send mode
                    NbCrypte = TaillePlainText % 8;
                    if (NbCrypte != 0) Chaine_renvoie = "Erreur Taille PlainText";
                    NbCrypte = TaillePlainText / 8;
                    Tab_Key = new byte[16];
                    Tab_Key = Key.SubArray(0, 16, CardData.Datatype.Hex);
                    Tab_InitVector = new byte[8];
                    Tab_InitVector = IniVector.SubArray(0, 8, CardData.Datatype.Hex);
                    j = 0;
                    for (index = 0; index < NbCrypte; index++)
                    {
                        Text_A_Crypter = new byte[8];
                        Text_A_Crypter = PlainText.SubArray(j, 8, CardData.Datatype.Hex);
                        Tab_Xor = new byte[8];
                        Tab_Xor = OuExclusif(Text_A_Crypter, Tab_InitVector);
                        Text_A_Crypter = new byte[8];
                        Text_A_Crypter = Tab_Xor;
                        StatusReponse = testDesfire.EncrypteDES(Text_A_Crypter, Tab_Key, 1, 1, 2);
                        if (StatusReponse == 0)
                        {
                            for (i = 0; i < Text_A_Crypter.Length; i++)
                            {
                                Chaine_renvoie = Chaine_renvoie + Text_A_Crypter[i].ToString("X2");
                            }
                            Tab_InitVector = new byte[8];
                            Tab_InitVector = Text_A_Crypter;

                            for (i = 0; i < Tab_InitVector.Length; i++)
                            {
                                CurrentIV = CurrentIV + Tab_InitVector[i].ToString("X2");
                            }

                            j = j + 8;
                        }
                        else
                            Chaine_renvoie = "";
                    }
                    break;
            }
            return Chaine_renvoie;
        }
        public string TDES_Standard_Enciphering(CardData PlainText, CardData Key, CardData IniVector, int Mode)
        {
            //PlainText = Chaine a crypter
            //Key = Cl� de cryptage
            //IniVector = Vecteur d'initialisation
            //Mode = 1=Decrypte, 2=Encrypte
            string Chaine_renvoie;
            byte[] Text_A_Crypter;
            byte[] Tab_Key;
            byte[] Tab_Xor;
            byte[] Tab_InitVector;
            byte[] Tab_PlainText = new byte[10];
            int TaillePlainText, NbCrypte, index, j, StatusReponse, i;

            TaillePlainText = PlainText.Length;
            Chaine_renvoie = "";
            switch (Mode)
            {
                case 1: //Deciphering in receive mode
                    NbCrypte = TaillePlainText % 8;
                    if (NbCrypte != 0) Chaine_renvoie = "Erreur Taille PlainText";
                    NbCrypte = TaillePlainText / 8;
                    Tab_Key = new byte[16];
                    Tab_Key = Key.SubArray(0, 16, CardData.Datatype.Hex);
                    Tab_InitVector = new byte[8];
                    Tab_InitVector = IniVector.SubArray(0, 8, CardData.Datatype.Hex);
                    j = 0;
                    for (index = 0; index < NbCrypte; index++)
                    {
                        Text_A_Crypter = new byte[8];
                        Text_A_Crypter = PlainText.SubArray(j, 8, CardData.Datatype.Hex);
                        if (index != 0)
                        {
                            Tab_InitVector = new byte[8];
                            Tab_InitVector = PlainText.SubArray(j - 8, 8, CardData.Datatype.Hex);
                            for (i = 0; i < Tab_InitVector.Length; i++)
                            {
                                CurrentIV = CurrentIV + Tab_InitVector[i].ToString("X2");
                            }
                        }
                        StatusReponse = testDesfire.EncrypteDES(Text_A_Crypter, Tab_Key, 1, 1, 2);
                        if (StatusReponse == 0)
                        {
                            Tab_Xor = new byte[8];
                            Tab_Xor = OuExclusif(Text_A_Crypter, Tab_InitVector);
                            Text_A_Crypter = new byte[8];
                            Text_A_Crypter = Tab_Xor;
                            for (i = 0; i < Text_A_Crypter.Length; i++)
                            {
                                Chaine_renvoie = Chaine_renvoie + Text_A_Crypter[i].ToString("X2");
                            }
                            j = j + 8;
                        }
                        else
                            Chaine_renvoie = "";
                    }
                    break;
                case 2: //Enciphering in Send mode
                    NbCrypte = TaillePlainText % 8;
                    if (NbCrypte != 0) Chaine_renvoie = "Erreur Taille PlainText";
                    NbCrypte = TaillePlainText / 8;
                    Tab_Key = new byte[16];
                    Tab_Key = Key.SubArray(0, 16, CardData.Datatype.Hex);
                    Tab_InitVector = new byte[8];
                    Tab_InitVector = IniVector.SubArray(0, 8, CardData.Datatype.Hex);
                    j = 0;
                    for (index = 0; index < NbCrypte; index++)
                    {
                        Text_A_Crypter = new byte[8];
                        Text_A_Crypter = PlainText.SubArray(j, 8, CardData.Datatype.Hex);
                        Tab_Xor = new byte[8];
                        Tab_Xor = OuExclusif(Text_A_Crypter, Tab_InitVector);
                        Text_A_Crypter = new byte[8];
                        Text_A_Crypter = Tab_Xor;
                        StatusReponse = testDesfire.EncrypteDES(Text_A_Crypter, Tab_Key, 1, 1, 1);
                        if (StatusReponse == 0)
                        {
                            for (i = 0; i < Text_A_Crypter.Length; i++)
                            {
                                Chaine_renvoie = Chaine_renvoie + Text_A_Crypter[i].ToString("X2");
                            }
                            Tab_InitVector = new byte[8];
                            Tab_InitVector = Text_A_Crypter;
                            for (i = 0; i < Tab_InitVector.Length; i++)
                            {
                                CurrentIV = CurrentIV + Tab_InitVector[i].ToString("X2");
                            }
                            j = j + 8;
                        }
                        else
                            Chaine_renvoie = "";
                    }
                    break;
            }
            return Chaine_renvoie;
        }
        public string AES_Enciphering(CardData PlainText, CardData Key, CardData IniVector, int Mode)
        {
            //PlainText = Chaine a crypter
            //Key = Cl� de cryptage
            //IniVector = Vecteur d'initialisation
            //Mode = 1=Decrypte , 2=Encrypte

            byte[] Text_A_Crypter;
            byte[] Tab_Key;
            byte[] Tab_InitVector;
            byte[] Text_Crypter;
            int TaillePlainText, NbCrypte, i;
            string chaine_renvoie;

            TaillePlainText = PlainText.Length;
            chaine_renvoie = "";

            NbCrypte = TaillePlainText % 16;
            if (NbCrypte != 0) chaine_renvoie = "Erreur Taille PlainText";
            Tab_Key = new byte[16];
            Tab_Key = Key.SubArray(0, 16, CardData.Datatype.Hex);
            Tab_InitVector = new byte[16];
            Tab_InitVector = IniVector.SubArray(0, 16, CardData.Datatype.Hex);
            Text_A_Crypter = new byte[16];
            Text_A_Crypter = PlainText.SubArray(0, 16, CardData.Datatype.Hex);
            Aes myAes = Aes.Create();
            ICryptoTransform CryptageAes;
            myAes.KeySize = 128;
            myAes.Padding = PaddingMode.None;
            myAes.Mode = CipherMode.CBC;
            myAes.Key = Tab_Key;
            myAes.IV = Tab_InitVector;

            switch (Mode)
            {
                case 1: //Decrypte    
                    CryptageAes = myAes.CreateDecryptor();
                    Text_Crypter = CryptageAes.TransformFinalBlock(Text_A_Crypter, 0, Text_A_Crypter.Length);
                    for (i = 0; i < Text_Crypter.Length; i++)
                    {
                        chaine_renvoie = chaine_renvoie + Text_Crypter[i].ToString("X2");
                    }
                    break;
                case 2: //Encrypte   
                    CryptageAes = myAes.CreateEncryptor();
                    Text_Crypter = CryptageAes.TransformFinalBlock(Text_A_Crypter, 0, Text_A_Crypter.Length);
                    for (i = 0; i < Text_Crypter.Length; i++)
                    {
                        chaine_renvoie = chaine_renvoie + Text_Crypter[i].ToString("X2");
                    }
                    break;
            }
            return chaine_renvoie;
        }
        public string AES_Enciphered_CBCMode(string PlainText, string Key, string IniVector, int Mode)
        {
            string[] Tab_PlainText;
            int NbCycle, i, j, TaillePlainText;
            string textXor, chaine_crypter, chaine_renvoie;

            chaine_renvoie = "";
            TaillePlainText = PlainText.Length;
            NbCycle = TaillePlainText % 32;
            if (NbCycle != 0) chaine_renvoie = "Erreur Taille PlainText";
            NbCycle = TaillePlainText / 32;
            Tab_PlainText = new string[NbCycle];
            j = 0;
            for (i = 0; i < PlainText.Length; i = i + 32)
            {
                Tab_PlainText[j] = PlainText.Substring(i, 32);
                j++;
            }
            switch (Mode)
            {
                case 1: //Decrypte mode CBC

                    for (i = 0; i < NbCycle; i++)
                    {
                        IniVector = "00000000000000000000000000000000";
                        chaine_crypter = AES_Enciphering(Tab_PlainText[i], Key, IniVector, 1);
                        if (i != 0)
                        {
                            IniVector = Tab_PlainText[i - 1];
                            CurrentIV = IniVector;
                        }
                        textXor = Chaine_OuExclusif(chaine_crypter, IniVector);
                        chaine_renvoie = chaine_renvoie + textXor;
                    }
                    break;
                case 2: //Encrypte mode CBC
                    for (i = 0; i < NbCycle; i++)
                    {
                        textXor = Chaine_OuExclusif(Tab_PlainText[i], IniVector);
                        IniVector = "00000000000000000000000000000000";
                        chaine_crypter = AES_Enciphering(textXor, Key, IniVector, 2);
                        Tab_PlainText[i] = chaine_crypter;
                        IniVector = Tab_PlainText[i];
                        CurrentIV = IniVector;
                        chaine_renvoie = chaine_renvoie + Tab_PlainText[i];
                    }
                    break;
            }
            return chaine_renvoie;
        }

        public byte[] AESEncrypt(byte[] key, byte[] iv, byte[] data)
        {

            using (MemoryStream ms = new MemoryStream())
            {
                Aes myAes = Aes.Create();
                myAes.Padding = PaddingMode.None;
                myAes.Mode = CipherMode.CBC;


                using (CryptoStream cs = new CryptoStream(ms, myAes.CreateEncryptor(key, iv), CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();

                    return ms.ToArray();
                }
            }
        }

        public byte[] Rol(byte[] b)
        {
            byte[] r = new byte[b.Length];
            byte carry = 0;

            for (int i = b.Length - 1; i >= 0; i--)
            {
                ushort u = (ushort)(b[i] << 1);
                r[i] = (byte)((u & 0xff) + carry);
                carry = (byte)((u & 0xff00) >> 8);
            }

            return r;
        }

        public byte[] AESCMAC(byte[] key, byte[] data)
        {
            // SubKey generation
            // step 1, AES-128 with key K is applied to an all-zero input block.
            byte[] L = AESEncrypt(key, new byte[16], new byte[16]);

            // step 2, K1 is derived through the following operation:
            byte[] FirstSubkey = Rol(L); //If the most significant bit of L is equal to 0, K1 is the left-shift of L by 1 bit.
            if ((L[0] & 0x80) == 0x80)
                FirstSubkey[15] ^= 0x87; // Otherwise, K1 is the exclusive-OR of const_Rb and the left-shift of L by 1 bit.

            // step 3, K2 is derived through the following operation:
            byte[] SecondSubkey = Rol(FirstSubkey); // If the most significant bit of K1 is equal to 0, K2 is the left-shift of K1 by 1 bit.
            if ((FirstSubkey[0] & 0x80) == 0x80)
                SecondSubkey[15] ^= 0x87; // Otherwise, K2 is the exclusive-OR of const_Rb and the left-shift of K1 by 1 bit.

            // MAC computing
            if (((data.Length != 0) && (data.Length % 16 == 0)) == true)
            {
                // If the size of the input message block is equal to a positive multiple of the block size (namely, 128 bits),
                // the last block shall be exclusive-OR'ed with K1 before processing
                for (int j = 0; j < FirstSubkey.Length; j++)
                    data[data.Length - 16 + j] ^= FirstSubkey[j];
            }
            else
            {
                // Otherwise, the last block shall be padded with 10^i
                byte[] padding = new byte[16 - data.Length % 16];
                padding[0] = 0x80;

                byte[] Tab_data2;
                Tab_data2 = new byte[data.Length + padding.Length];

                Buffer.BlockCopy(data, 0, Tab_data2, 0, data.Length);
                Buffer.BlockCopy(padding, 0, Tab_data2, data.Length, padding.Length);
                data = new byte[data.Length + padding.Length];
                Buffer.BlockCopy(Tab_data2, 0, data, 0, Tab_data2.Length);
                //return data;
                //Buffer.BlockCopy(padding,0,data,data.Length,16 - data.Length % 16);
                //data = data.Concat<byte>(padding.AsEnumerable()).ToArray();

                // and exclusive-OR'ed with K2
                for (int j = 0; j < SecondSubkey.Length; j++)
                    data[data.Length - 16 + j] ^= SecondSubkey[j];

            }
            // The result of the previous process will be the input of the last encryption.
            byte[] encResult = AESEncrypt(key, new byte[16], data);

            byte[] HashValue = new byte[16];
            Array.Copy(encResult, encResult.Length - HashValue.Length, HashValue, 0, HashValue.Length);

            return HashValue;
        }

    }


    /// <summary>
    /// Script class.
    /// </summary>
    public class PersoScriptGenerated : PersoScriptBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PersoScriptGenerated"/> class.
        /// </summary>
        /// <param name="readerId">The reader id.</param>
        /// <param name="keys">The keys.</param>
        public PersoScriptGenerated(UInt32 readerId, String[] keys)
            : base(readerId, keys)
        {
        }
        /// <summary>
        /// Card personalization method. Called for each single card personalization.
        /// </summary>
        /// <param name="Data">The variable personalization data for this specific product.</param>
        /// <returns>true when programmed successfully, else false or an exception</returns>
        protected override bool OnPersoCard(CardData Data)
        {
            CardData AMK, read1, read2, write1, write2, Master,knull;
            AMK = "9B6429EF911919A404D53D38E3BAE6B0";
            read1 = "A2ABFDB343E619B00BF2679BDFD6E557";
            read2 = "F84FA4B9EF62A8FEBF9D7FF2546E8385";
            write1 = "8C898C29197C6D9EF24CE5490DD5D597";
            write2 = "E92C0D02CE64A70DB957EAD080D98CFD";
            Master = "24DE17440AB75C9912D771112E6644A6";
            knull = "00000000000000000000000000000000";
            // create card object (out own one)
            using (testDesfire theCardObject = new testDesfire(ReaderId))
            {
                ADF1 = theCardObject.Reset();
                ADF2 = theCardObject.Uid;
                theCardObject.Format_PICC();
                theCardObject.CreateApplication("495352", "0F", "82");
                
                theCardObject.SelectApplication_AES("495352");
                ADF3=theCardObject.ExchangeApdu("DA495352");
                theCardObject.DESFire_Authentification("00", knull, 3);

                //Ajout des cles
                theCardObject.DESFire_ChangeKey("0F", "01", knull, AMK, 3, theCardObject.CurrentIV);
                theCardObject.DESFire_ChangeKey("01", "01", knull, read1, 3, theCardObject.CurrentIV);
                theCardObject.DESFire_ChangeKey("02", "01", knull, write1, 3, theCardObject.CurrentIV);
                theCardObject.DESFire_ChangeKey("03", "01", knull, read2, 3, theCardObject.CurrentIV);
                theCardObject.DESFire_ChangeKey("04", "01", knull, write2, 3, theCardObject.CurrentIV);

                theCardObject.CreateStdDataFile("00", "00", "2212", "50");
                theCardObject.CreateStdDataFile("01", "00", "4434", "50");

                
            }

            return true;
        }
    }
}
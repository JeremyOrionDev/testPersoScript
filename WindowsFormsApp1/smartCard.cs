using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using AtlanticZeiser.CpiPc.Reader.Smartware;
using AtlanticZeiser.CpiPc.Reader.Smartware.Cards;
using AtlanticZeiser.CpiPc.Scripting.Compiler;
using AtlanticZeiser.CpiPc.Tools.Misc;
using AtlanticZeiser.CpiPc.Scripting.Converter;
using AtlanticZeiser.CpiPc.Scripting.Xml.Mifare;
using System.Security.Cryptography;
using AtlanticZeiser.CpiPc.Scripting.Xml.Iso7816;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class smartCard : CardIso7816
    {
        public smartCard(uint readerID):base(readerID)
        {
        }
        /// <summary>
        /// Exchanges an APDU and expects exactly one response byte with the specified value.
        /// </summary>
        /// <param name="cmdApdu">The command APDU.</param>
        /// <param name="expected">The expected response APDU.</param>
        /// <returns>The card response</returns>
        public  new CardData ExchangeApdu(CardData cmdApdu, CardData expected)
        {
            CardData response;
            response = ExchangeApdu(cmdApdu);

            response.Compare(expected, true);

            return response;
        }
        // WinSCard APIs to be imported.
        [DllImport("WinScard.dll")]
        public static extern int SCardEstablishContext(uint dwScope,
        IntPtr notUsed1,
        IntPtr notUsed2,
        out IntPtr phContext);

        [DllImport("WinScard.dll")]
        public static extern int SCardReleaseContext(IntPtr phContext);

        [DllImport("WinScard.dll")]
        public static extern int SCardConnect(IntPtr hContext,
        string cReaderName,
        uint dwShareMode,
        uint dwPrefProtocol,
        ref IntPtr phCard,
        ref IntPtr ActiveProtocol);

        [DllImport("WinScard.dll")]
        public static extern int SCardDisconnect(IntPtr hCard, int Disposition);

        // [DllImport("WinScard.dll")]
        // static extern int SCardListReaderGroups(IntPtr hContext,
        // ref string cGroups,
        // ref int nStringSize);

        [DllImport("WinScard.dll", EntryPoint = "SCardListReadersA", CharSet = CharSet.Ansi)]
        public static extern int SCardListReaders(
          IntPtr hContext,
          byte[] mszGroups,
          byte[] mszReaders,
          ref UInt32 pcchReaders
          );

        // [DllImport("WinScard.dll")]
        // static extern int SCardFreeMemory(IntPtr hContext,
        // string cResourceToFree);
    }
}

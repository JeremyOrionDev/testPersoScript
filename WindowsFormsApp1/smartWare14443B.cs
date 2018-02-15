//                                                                        
//  Script purpose:                                                       
//  This script demonstrates how to communicate with an ISO 14443 B chip. 
//  It activates the tag and exchanges and APDU.
//                                                                        
//  Card article number:      none
//  Chip type:                ISO 14443 type B, level 4
//  Programming:              about 0.1 second                            
//  Production Data:          none                                        
//  ADF1:                     UID or pseudo UID
//       
namespace AlanticZeiser.CpiPc.ScriptCompiler
{
    using System;
    using System.Threading;
    using AtlanticZeiser.CpiPc.Reader.Smartware;
    using AtlanticZeiser.CpiPc.Reader.Smartware.Cards;
    using AtlanticZeiser.CpiPc.Scripting.Compiler;
    using AtlanticZeiser.CpiPc.Tools.Misc;
    using AtlanticZeiser.CpiPc.Scripting.Converter;
    using AtlanticZeiser.CpiPc.Scripting.Xml.Mifare;
    using System.Security.Cryptography;


    public class CrcB
    {
        const ushort __crcBDefault = 0xffff;

        private static ushort UpdateCrc(byte b, ushort crc)
        {
            unchecked
            {
                byte ch = (byte)(b ^ (byte)(crc & 0x00ff));
                ch = (byte)(ch ^ (ch << 4));
                return (ushort)((crc >> 8) ^ (ch << 8) ^ (ch << 3) ^ (ch >> 4));
            }
        }

        public static ushort ComputeCrc(byte[] bytes)
        {
            var res = __crcBDefault;
            foreach (var b in bytes)
                res = UpdateCrc(b, res);
            return (ushort)~res;
        }
    }

    /// <summary>
    /// Iso14443B dummy implementation.
    /// </summary>
    public class MyCardIso14443B : CardIso14443B
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="MyCardIso14443B"/> class.
        /// </summary>
        /// <param name="readerID">The reader ID.</param>
        public MyCardIso14443B(uint readerID)
            : base(readerID)
        {
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
            bool ok = false;

            // create card object (out own one)
            using (MyCardIso14443B theCardObject = new MyCardIso14443B(ReaderId))
            {
                // modify power level
                //theCardObject.Power = 12;
                ADF1 = theCardObject.Power.ToString();
                // modify communication speed (BEFORE Reset, see Smartware documentation !!!)
                theCardObject.SetBaudRate(CardInterfacePicc.Baudrates.Baud212, CardInterfacePicc.Baudrates.Baud212);
                ADF3 = theCardObject.Frequency.ToString();

                CardData iso = new CardData(new byte[] { 0xFF, 0xFE, 0x02, 0x00, 0x01, 0x0b });
                var bytes = new byte[] { 0x0B };
                var crc = CrcB.ComputeCrc(bytes);
                var cbytes = BitConverter.GetBytes(crc);
                //ADF1 = cbytes[0].ToString("X2") + cbytes[1].ToString("X2");
                ADF4=theCardObject.Model.ToString();
                ADF2 = theCardObject.Uid;
                // acticate card (ISO 14443B level 4)
                
                Console.WriteLine("Card activated (UID is 0x" + theCardObject.Uid);

                // send an APDU and expect a response ending with 0x9000
                ADF5 = theCardObject.ExchangeApdu("00A40000", "*9000");

                ok = true;
            }

            return ok;
        }
    }
}

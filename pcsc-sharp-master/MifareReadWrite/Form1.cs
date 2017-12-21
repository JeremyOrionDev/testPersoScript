using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PCSC.Interop.Windows;
using System.Runtime.InteropServices;

namespace MifareReadWrite
{
    public partial class Form1 : Form
    {
        private IntPtr hContext;
        private byte[] mszGroups;
        private byte[] pmszReaders;
        private int pcchReaders;

        public Form1()
        {
            InitializeComponent();
        }
        [DllImport("winscard.dll", CharSet = CharSet.Auto)]
        private static extern int SCardListReaders(
        [In] IntPtr hContext,
        [In] byte[] mszGroups,
        [Out] byte[] pmszReaders,
        [In, Out] ref int pcchReaders);
        private void btnTrouverLecteur_Click(object sender, EventArgs e) => SCardListReaders(hContext, mszGroups, pmszReaders,ref pcchReaders);
    }
}

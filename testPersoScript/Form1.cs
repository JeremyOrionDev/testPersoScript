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

namespace testPersoScript
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public void start()
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
        }
    }
}

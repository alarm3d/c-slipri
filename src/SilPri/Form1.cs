using System;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
namespace SilPri
{
    public partial class Form1 : Form
    {
        Monitoring watch;
        string default_path = Properties.Settings.Default.path;
        string default_printer = Properties.Settings.Default.printer;
        bool inTray = Properties.Settings.Default.tray;
        public Form1()
        {
            InitializeComponent();
            this.Load += new EventHandler(this.Form1_Load);
            this.Load += new EventHandler(this.Form1_Deactivate);
            this.Shown  += new EventHandler(this.Form1_Show);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            PrintDocument prtdoc = new PrintDocument();
            watch = new Monitoring();
            string strDefaultPrinter = prtdoc.PrinterSettings.PrinterName;
            if (default_printer != "")
            {
                strDefaultPrinter = default_printer;
            }
            foreach (String strPrinter in PrinterSettings.InstalledPrinters)
            {
                comboPrinters.Items.Add(strPrinter);
                if (strPrinter == strDefaultPrinter)
                {
                    comboPrinters.SelectedIndex = comboPrinters.Items.IndexOf(strPrinter);
                }
            }
            this.textBox1.Text = default_path;
            this.checkBox1.Checked = inTray;
        }
        private void Form1_Deactivate(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                notifyIcon1.Visible = true;
            }
            if (inTray)
            {
                var path = textBox1.Text;
                watch.Start(path);
                Hide();
            }
        }
        private void Form1_Show(object sender, EventArgs e)
        {
            if (inTray)
            {
                var path = textBox1.Text;
                watch.Start(path);
                Hide();
            }
        }

        private void comboPrinters_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.printer = (string)comboPrinters.SelectedItem;
            Properties.Settings.Default.Save();

            // string selectedEmployee = (string)comboPrinters.SelectedItem;
            // myPrinters.SetDefaultPrinter(selectedEmployee);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;
            // Show the FolderBrowserDialog.  
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                string ch_path = folderDlg.SelectedPath;
                textBox1.Text = ch_path;
                Environment.SpecialFolder root = folderDlg.RootFolder;
                Properties.Settings.Default.path = ch_path;
                Properties.Settings.Default.Save();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var path = textBox1.Text;
            watch.Start(path);
            this.Hide();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            watch.Stop();
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.tray = false;
            if (checkBox1.Checked)
            {
                Properties.Settings.Default.tray = true;
            }
            Properties.Settings.Default.Save();
        }
    }
    public static class myPrinters
    {
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetDefaultPrinter(string Name);

    }
}

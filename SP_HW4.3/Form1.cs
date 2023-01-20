using System.Diagnostics;

namespace SP_HW4._3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var currentProcess = Process.GetCurrentProcess();

 
            var processes = Process.GetProcessesByName(currentProcess.ProcessName);

            
            if (processes.Length > 3)
            {
                MessageBox.Show("Only 3 instances of this application can run at the same time.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
        }
    }
}
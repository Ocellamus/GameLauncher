using System;
using System.Windows.Forms;

namespace GameLauncher
{
    public partial class OprimizationForm : Form
    {
        public OprimizationForm()
        {
            InitializeComponent();
        }

        private void OprimizationForm_Load(object sender, EventArgs e)
        {
            // You can leave this empty or use it for any future tweaks.
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Optional: no logic needed here unless you want the image clickable.
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // No, launch the game without changing any settings
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Yes, change the settings and launch the game
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            // Optional: can stay empty if you don't need dynamic text behaviour.
        }
    }
}

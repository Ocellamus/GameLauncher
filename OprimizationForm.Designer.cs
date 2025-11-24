namespace GameLauncher
{
    partial class OprimizationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OprimizationForm));
            button1 = new System.Windows.Forms.Button();
            richTextBox1 = new System.Windows.Forms.RichTextBox();
            button2 = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.BackColor = System.Drawing.Color.White;
            button1.FlatAppearance.BorderColor = System.Drawing.Color.Fuchsia;
            button1.FlatAppearance.BorderSize = 0;
            button1.Font = new System.Drawing.Font("Segoe UI", 11F);
            button1.Location = new System.Drawing.Point(333, 279);
            button1.Margin = new System.Windows.Forms.Padding(5);
            button1.Name = "button1";
            button1.Padding = new System.Windows.Forms.Padding(5);
            button1.Size = new System.Drawing.Size(385, 64);
            button1.TabIndex = 1;
            button1.Text = "No, launch the game without changing any settings";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = System.Drawing.Color.FromArgb(23, 29, 37);
            richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            richTextBox1.Font = new System.Drawing.Font("Segoe UI", 11F);
            richTextBox1.ForeColor = System.Drawing.SystemColors.Window;
            richTextBox1.Location = new System.Drawing.Point(66, 33);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new System.Drawing.Size(652, 216);
            richTextBox1.TabIndex = 2;
            richTextBox1.Text = resources.GetString("richTextBox1.Text");
            richTextBox1.TextChanged += richTextBox1_TextChanged;
            // 
            // button2
            // 
            button2.BackColor = System.Drawing.Color.White;
            button2.FlatAppearance.BorderColor = System.Drawing.Color.Fuchsia;
            button2.FlatAppearance.BorderSize = 0;
            button2.Font = new System.Drawing.Font("Segoe UI", 11F);
            button2.Location = new System.Drawing.Point(333, 353);
            button2.Margin = new System.Windows.Forms.Padding(5);
            button2.Name = "button2";
            button2.Padding = new System.Windows.Forms.Padding(5);
            button2.Size = new System.Drawing.Size(385, 64);
            button2.TabIndex = 3;
            button2.Text = "Yes, change the settings and launch the game";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // OprimizationForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(23, 29, 37);
            ClientSize = new System.Drawing.Size(765, 451);
            Controls.Add(button2);
            Controls.Add(richTextBox1);
            Controls.Add(button1);
            Name = "OprimizationForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "PGMMV Player - Launcher";
            Load += OprimizationForm_Load;
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button2;
    }
}
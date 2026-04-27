namespace WinFormsApp2
{
    partial class DangKi
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
            button1 = new Button();
            textBox_MatKhau = new TextBox();
            textBox_TenTaiKhoan = new TextBox();
            pictureBox1 = new PictureBox();
            textBox_XNMatKhau = new TextBox();
            textBox_Email = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(119, 316);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 14;
            button1.Text = "Registor";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button_DangKi_Click;
            // 
            // textBox_MatKhau
            // 
            textBox_MatKhau.Location = new Point(119, 219);
            textBox_MatKhau.Name = "textBox_MatKhau";
            textBox_MatKhau.Size = new Size(210, 23);
            textBox_MatKhau.TabIndex = 11;
            // 
            // textBox_TenTaiKhoan
            // 
            textBox_TenTaiKhoan.Location = new Point(119, 190);
            textBox_TenTaiKhoan.Name = "textBox_TenTaiKhoan";
            textBox_TenTaiKhoan.Size = new Size(210, 23);
            textBox_TenTaiKhoan.TabIndex = 10;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.Screenshot_2026_04_25_220446;
            pictureBox1.Location = new Point(46, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(244, 172);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 9;
            pictureBox1.TabStop = false;
            // 
            // textBox_XNMatKhau
            // 
            textBox_XNMatKhau.Location = new Point(119, 248);
            textBox_XNMatKhau.Name = "textBox_XNMatKhau";
            textBox_XNMatKhau.Size = new Size(210, 23);
            textBox_XNMatKhau.TabIndex = 15;
            // 
            // textBox_Email
            // 
            textBox_Email.Location = new Point(119, 277);
            textBox_Email.Name = "textBox_Email";
            textBox_Email.Size = new Size(210, 23);
            textBox_Email.TabIndex = 16;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(9, 190);
            label1.Name = "label1";
            label1.Size = new Size(39, 15);
            label1.TabIndex = 17;
            label1.Text = "Name";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(9, 222);
            label2.Name = "label2";
            label2.Size = new Size(57, 15);
            label2.TabIndex = 18;
            label2.Text = "Password";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(9, 251);
            label3.Name = "label3";
            label3.Size = new Size(104, 15);
            label3.TabIndex = 19;
            label3.Text = "Confirm Password";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(9, 280);
            label4.Name = "label4";
            label4.Size = new Size(36, 15);
            label4.TabIndex = 20;
            label4.Text = "Email";
            // 
            // DangKi
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(337, 450);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textBox_Email);
            Controls.Add(textBox_XNMatKhau);
            Controls.Add(button1);
            Controls.Add(textBox_MatKhau);
            Controls.Add(textBox_TenTaiKhoan);
            Controls.Add(pictureBox1);
            Name = "DangKi";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private TextBox textBox_MatKhau;
        private TextBox textBox_TenTaiKhoan;
        private PictureBox pictureBox1;
        private TextBox textBox_XNMatKhau;
        private TextBox textBox_Email;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
    }
}
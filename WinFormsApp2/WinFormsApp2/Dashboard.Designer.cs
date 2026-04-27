namespace WinFormsApp2
{
    partial class Dashboard
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
            tabPage2 = new TabPage();
            label_performance = new Label();
            button_connect = new Button();
            button_Renew = new Button();
            label_APIServer = new Label();
            label_BaudRate = new Label();
            label_COMPort = new Label();
            textBox_APIServer = new TextBox();
            textBox_BaudRate = new TextBox();
            textBox_COMPort = new TextBox();
            tabPage1 = new TabPage();
            button_stop = new Button();
            button_start = new Button();
            pictureBox_Camera = new PictureBox();
            label_LEDPerformance = new Label();
            label_gestureRate = new Label();
            tabControl1 = new TabControl();
            tabPage3 = new TabPage();
            button_logout = new Button();
            button_save = new Button();
            button_delete = new Button();
            button_print = new Button();
            button_reset = new Button();
            dataGridView1 = new DataGridView();
            pictureBox1 = new PictureBox();
            colGesture = new DataGridViewTextBoxColumn();
            colConfidence = new DataGridViewTextBoxColumn();
            colTime = new DataGridViewTextBoxColumn();
            tabPage2.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox_Camera).BeginInit();
            tabControl1.SuspendLayout();
            tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(label_performance);
            tabPage2.Controls.Add(button_connect);
            tabPage2.Controls.Add(button_Renew);
            tabPage2.Controls.Add(label_APIServer);
            tabPage2.Controls.Add(label_BaudRate);
            tabPage2.Controls.Add(label_COMPort);
            tabPage2.Controls.Add(textBox_APIServer);
            tabPage2.Controls.Add(textBox_BaudRate);
            tabPage2.Controls.Add(textBox_COMPort);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(767, 606);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "COM Port";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // label_performance
            // 
            label_performance.AutoSize = true;
            label_performance.Location = new Point(223, 333);
            label_performance.Name = "label_performance";
            label_performance.Size = new Size(81, 15);
            label_performance.TabIndex = 29;
            label_performance.Text = "Performance: ";
            // 
            // button_connect
            // 
            button_connect.Location = new Point(456, 293);
            button_connect.Name = "button_connect";
            button_connect.Size = new Size(75, 23);
            button_connect.TabIndex = 28;
            button_connect.Text = "Connect";
            button_connect.UseVisualStyleBackColor = true;
            button_connect.Click += button_connect_Click;
            // 
            // button_Renew
            // 
            button_Renew.Location = new Point(238, 293);
            button_Renew.Name = "button_Renew";
            button_Renew.Size = new Size(75, 23);
            button_Renew.TabIndex = 27;
            button_Renew.Text = "Renew";
            button_Renew.UseVisualStyleBackColor = true;
            button_Renew.Click += button_Renew_Click;
            // 
            // label_APIServer
            // 
            label_APIServer.AutoSize = true;
            label_APIServer.Location = new Point(222, 245);
            label_APIServer.Name = "label_APIServer";
            label_APIServer.Size = new Size(60, 15);
            label_APIServer.TabIndex = 26;
            label_APIServer.Text = "API Server";
            // 
            // label_BaudRate
            // 
            label_BaudRate.AutoSize = true;
            label_BaudRate.Location = new Point(222, 216);
            label_BaudRate.Name = "label_BaudRate";
            label_BaudRate.Size = new Size(60, 15);
            label_BaudRate.TabIndex = 25;
            label_BaudRate.Text = "Baud Rate";
            // 
            // label_COMPort
            // 
            label_COMPort.AutoSize = true;
            label_COMPort.Location = new Point(222, 187);
            label_COMPort.Name = "label_COMPort";
            label_COMPort.Size = new Size(60, 15);
            label_COMPort.TabIndex = 24;
            label_COMPort.Text = "COM Port";
            // 
            // textBox_APIServer
            // 
            textBox_APIServer.Location = new Point(332, 242);
            textBox_APIServer.Name = "textBox_APIServer";
            textBox_APIServer.Size = new Size(210, 23);
            textBox_APIServer.TabIndex = 23;
            // 
            // textBox_BaudRate
            // 
            textBox_BaudRate.Location = new Point(332, 213);
            textBox_BaudRate.Name = "textBox_BaudRate";
            textBox_BaudRate.Size = new Size(210, 23);
            textBox_BaudRate.TabIndex = 22;
            // 
            // textBox_COMPort
            // 
            textBox_COMPort.Location = new Point(332, 184);
            textBox_COMPort.Name = "textBox_COMPort";
            textBox_COMPort.Size = new Size(210, 23);
            textBox_COMPort.TabIndex = 21;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(button_stop);
            tabPage1.Controls.Add(button_start);
            tabPage1.Controls.Add(pictureBox_Camera);
            tabPage1.Controls.Add(label_LEDPerformance);
            tabPage1.Controls.Add(label_gestureRate);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(767, 606);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Camera";
            tabPage1.UseVisualStyleBackColor = true;
            tabPage1.Click += tabPage1_Click;
            // 
            // button_stop
            // 
            button_stop.Location = new Point(6, 556);
            button_stop.Name = "button_stop";
            button_stop.Size = new Size(158, 44);
            button_stop.TabIndex = 25;
            button_stop.Text = "Stop";
            button_stop.UseVisualStyleBackColor = true;
            button_stop.Click += button_stop_Click;
            // 
            // button_start
            // 
            button_start.Location = new Point(602, 556);
            button_start.Name = "button_start";
            button_start.Size = new Size(158, 44);
            button_start.TabIndex = 24;
            button_start.Text = "Start";
            button_start.UseVisualStyleBackColor = true;
            button_start.Click += button_start_Click;
            // 
            // pictureBox_Camera
            // 
            pictureBox_Camera.Location = new Point(6, 6);
            pictureBox_Camera.Name = "pictureBox_Camera";
            pictureBox_Camera.Size = new Size(754, 495);
            pictureBox_Camera.TabIndex = 23;
            pictureBox_Camera.TabStop = false;
            // 
            // label_LEDPerformance
            // 
            label_LEDPerformance.AutoSize = true;
            label_LEDPerformance.Location = new Point(6, 528);
            label_LEDPerformance.Name = "label_LEDPerformance";
            label_LEDPerformance.Size = new Size(101, 15);
            label_LEDPerformance.TabIndex = 22;
            label_LEDPerformance.Text = "LED Performance:";
            // 
            // label_gestureRate
            // 
            label_gestureRate.AutoSize = true;
            label_gestureRate.Location = new Point(6, 504);
            label_gestureRate.Name = "label_gestureRate";
            label_gestureRate.Size = new Size(76, 15);
            label_gestureRate.TabIndex = 21;
            label_gestureRate.Text = "Gesture Rate:";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Location = new Point(12, 12);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(775, 634);
            tabControl1.TabIndex = 0;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(button_logout);
            tabPage3.Controls.Add(button_save);
            tabPage3.Controls.Add(button_delete);
            tabPage3.Controls.Add(button_print);
            tabPage3.Controls.Add(button_reset);
            tabPage3.Controls.Add(dataGridView1);
            tabPage3.ForeColor = SystemColors.ControlText;
            tabPage3.Location = new Point(4, 24);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(767, 606);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "History";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // button_logout
            // 
            button_logout.Location = new Point(629, 518);
            button_logout.Name = "button_logout";
            button_logout.Size = new Size(104, 49);
            button_logout.TabIndex = 7;
            button_logout.Text = "logout";
            button_logout.UseVisualStyleBackColor = true;
            button_logout.Click += button_logout_Click;
            // 
            // button_save
            // 
            button_save.Location = new Point(629, 427);
            button_save.Name = "button_save";
            button_save.Size = new Size(104, 49);
            button_save.TabIndex = 5;
            button_save.Text = "Save";
            button_save.UseVisualStyleBackColor = true;
            button_save.Click += button_save_Click;
            // 
            // button_delete
            // 
            button_delete.Location = new Point(629, 339);
            button_delete.Name = "button_delete";
            button_delete.Size = new Size(104, 49);
            button_delete.TabIndex = 5;
            button_delete.Text = "Delete";
            button_delete.UseVisualStyleBackColor = true;
            button_delete.Click += button_delete_Click;
            // 
            // button_print
            // 
            button_print.Location = new Point(629, 257);
            button_print.Name = "button_print";
            button_print.Size = new Size(104, 49);
            button_print.TabIndex = 6;
            button_print.Text = "Print";
            button_print.UseVisualStyleBackColor = true;
            button_print.Click += button_print_Click;
            // 
            // button_reset
            // 
            button_reset.Location = new Point(629, 174);
            button_reset.Name = "button_reset";
            button_reset.Size = new Size(104, 49);
            button_reset.TabIndex = 5;
            button_reset.Text = "Reset";
            button_reset.UseVisualStyleBackColor = true;
            button_reset.Click += button_reset_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { colGesture, colConfidence, colTime });
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(584, 606);
            dataGridView1.TabIndex = 3;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.Screenshot_2026_04_25_220446;
            pictureBox1.Location = new Point(0, -1);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(802, 659);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // colGesture
            // 
            colGesture.HeaderText = "colGesture";
            colGesture.Name = "colGesture";
            colGesture.Width = 180;
            // 
            // colConfidence
            // 
            colConfidence.HeaderText = "colConfidence";
            colConfidence.Name = "colConfidence";
            colConfidence.Width = 180;
            // 
            // colTime
            // 
            colTime.HeaderText = "colTime";
            colTime.Name = "colTime";
            colTime.Width = 180;
            // 
            // Dashboard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(802, 658);
            Controls.Add(tabControl1);
            Controls.Add(pictureBox1);
            Name = "Dashboard";
            Text = "Dashboard";
            Load += Dashboard_Load;
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox_Camera).EndInit();
            tabControl1.ResumeLayout(false);
            tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TabPage tabPage2;
        private Label label_performance;
        private Button button_connect;
        private Button button_Renew;
        private Label label_APIServer;
        private Label label_BaudRate;
        private Label label_COMPort;
        private TextBox textBox_APIServer;
        private TextBox textBox_BaudRate;
        private TextBox textBox_COMPort;
        private TabPage tabPage1;
        private Button button_stop;
        private Button button_start;
        private PictureBox pictureBox_Camera;
        private Label label_LEDPerformance;
        private Label label_gestureRate;
        private TabControl tabControl1;
        private TabPage tabPage3;
        private DataGridView dataGridView1;
        private Button button_logout;
        private Button button_save;
        private Button button_delete;
        private Button button_print;
        private Button button_reset;
        private PictureBox pictureBox1;
        private DataGridViewTextBoxColumn colGesture;
        private DataGridViewTextBoxColumn colConfidence;
        private DataGridViewTextBoxColumn colTime;
    }
}
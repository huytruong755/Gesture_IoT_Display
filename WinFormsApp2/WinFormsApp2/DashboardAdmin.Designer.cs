namespace WinFormsApp2
{
    partial class DashboardAdmin
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
            components = new System.ComponentModel.Container();
            sidebar = new Panel();
            button_logout = new PictureBox();
            pictureBox4 = new PictureBox();
            pictureBox3 = new PictureBox();
            pictureBox2 = new PictureBox();
            button_Account = new Button();
            button_Device = new Button();
            button_Session = new Button();
            pictureBox1 = new PictureBox();
            button_Overview = new Button();
            panel_logo = new Panel();
            label1 = new Label();
            menuButton = new PictureBox();
            sidebarTimer = new System.Windows.Forms.Timer(components);
            panelContent = new Panel();
            panelOverviewRoot = new Panel();
            panelDeviceStatus = new Panel();
            flowDevices = new FlowLayoutPanel();
            labelSectionDevices = new Label();
            panelSessions = new Panel();
            dgvSessions = new DataGridView();
            labelSectionSessions = new Label();
            panelPopularGesture = new Panel();
            lblQualityRange = new Label();
            lblConfidenceRange = new Label();
            lblPopularPercent = new Label();
            lblPopularGestureName = new Label();
            labelSectionPopular = new Label();
            panelKpi4 = new Panel();
            lblKpiDeviceDetail = new Label();
            lblKpiDeviceValue = new Label();
            lblKpiDeviceTitle = new Label();
            panelKpi3 = new Panel();
            lblKpiAvgDetail = new Label();
            lblKpiAvgValue = new Label();
            lblKpiAvgTitle = new Label();
            panelKpi2 = new Panel();
            lblKpiGesturesDetail = new Label();
            lblKpiGesturesValue = new Label();
            lblKpiGesturesTitle = new Label();
            panelKpi1 = new Panel();
            lblKpiSessionDetail = new Label();
            lblKpiSessionValue = new Label();
            lblKpiSessionTitle = new Label();
            lblOverviewTitle = new Label();
            sidebar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)button_logout).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel_logo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)menuButton).BeginInit();
            panelContent.SuspendLayout();
            panelOverviewRoot.SuspendLayout();
            panelDeviceStatus.SuspendLayout();
            panelSessions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSessions).BeginInit();
            panelPopularGesture.SuspendLayout();
            panelKpi4.SuspendLayout();
            panelKpi3.SuspendLayout();
            panelKpi2.SuspendLayout();
            panelKpi1.SuspendLayout();
            SuspendLayout();
            // 
            // sidebar
            // 
            sidebar.BackColor = Color.FromArgb(51, 51, 76);
            sidebar.Controls.Add(button_logout);
            sidebar.Controls.Add(pictureBox4);
            sidebar.Controls.Add(pictureBox3);
            sidebar.Controls.Add(pictureBox2);
            sidebar.Controls.Add(button_Account);
            sidebar.Controls.Add(button_Device);
            sidebar.Controls.Add(button_Session);
            sidebar.Controls.Add(pictureBox1);
            sidebar.Controls.Add(button_Overview);
            sidebar.Controls.Add(panel_logo);
            sidebar.Dock = DockStyle.Left;
            sidebar.Location = new Point(0, 0);
            sidebar.MaximumSize = new Size(224, 0);
            sidebar.MinimumSize = new Size(106, 0);
            sidebar.Name = "sidebar";
            sidebar.Size = new Size(224, 700);
            sidebar.TabIndex = 0;
            // 
            // button_logout
            // 
            button_logout.Dock = DockStyle.Bottom;
            button_logout.Image = Properties.Resources.logout;
            button_logout.Location = new Point(0, 655);
            button_logout.Name = "button_logout";
            button_logout.Size = new Size(224, 45);
            button_logout.SizeMode = PictureBoxSizeMode.Zoom;
            button_logout.TabIndex = 8;
            button_logout.TabStop = false;
            button_logout.Click += button_logout_Click;
            // 
            // pictureBox4
            // 
            pictureBox4.Image = Properties.Resources.account;
            pictureBox4.Location = new Point(25, 263);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(54, 45);
            pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox4.TabIndex = 7;
            pictureBox4.TabStop = false;
            // 
            // pictureBox3
            // 
            pictureBox3.Image = Properties.Resources.device;
            pictureBox3.Location = new Point(24, 202);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(55, 45);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 6;
            pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.session;
            pictureBox2.Location = new Point(25, 141);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(54, 48);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 5;
            pictureBox2.TabStop = false;
            // 
            // button_Account
            // 
            button_Account.Dock = DockStyle.Top;
            button_Account.FlatAppearance.BorderSize = 0;
            button_Account.FlatStyle = FlatStyle.Flat;
            button_Account.ForeColor = Color.Gainsboro;
            button_Account.Location = new Point(0, 255);
            button_Account.Name = "button_Account";
            button_Account.Size = new Size(224, 60);
            button_Account.TabIndex = 4;
            button_Account.Text = "Account";
            button_Account.UseVisualStyleBackColor = true;
            button_Account.Click += button_Account_Click;
            // 
            // button_Device
            // 
            button_Device.Dock = DockStyle.Top;
            button_Device.FlatAppearance.BorderSize = 0;
            button_Device.FlatStyle = FlatStyle.Flat;
            button_Device.ForeColor = Color.Gainsboro;
            button_Device.Location = new Point(0, 195);
            button_Device.Name = "button_Device";
            button_Device.Size = new Size(224, 60);
            button_Device.TabIndex = 3;
            button_Device.Text = "Device";
            button_Device.UseVisualStyleBackColor = true;
            button_Device.Click += button_Device_Click;
            // 
            // button_Session
            // 
            button_Session.Dock = DockStyle.Top;
            button_Session.FlatAppearance.BorderSize = 0;
            button_Session.FlatStyle = FlatStyle.Flat;
            button_Session.ForeColor = Color.Gainsboro;
            button_Session.Location = new Point(0, 135);
            button_Session.Name = "button_Session";
            button_Session.Size = new Size(224, 60);
            button_Session.TabIndex = 2;
            button_Session.Text = "Session";
            button_Session.UseVisualStyleBackColor = true;
            button_Session.Click += button_Session_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.overall;
            pictureBox1.Location = new Point(24, 84);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(55, 44);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // button_Overview
            // 
            button_Overview.Dock = DockStyle.Top;
            button_Overview.FlatAppearance.BorderSize = 0;
            button_Overview.FlatStyle = FlatStyle.Flat;
            button_Overview.ForeColor = Color.Gainsboro;
            button_Overview.Location = new Point(0, 75);
            button_Overview.Name = "button_Overview";
            button_Overview.Size = new Size(224, 60);
            button_Overview.TabIndex = 0;
            button_Overview.Text = "Overview";
            button_Overview.UseVisualStyleBackColor = true;
            button_Overview.Click += button_Overview_Click;
            // 
            // panel_logo
            // 
            panel_logo.BackColor = Color.FromArgb(39, 39, 58);
            panel_logo.Controls.Add(label1);
            panel_logo.Controls.Add(menuButton);
            panel_logo.Dock = DockStyle.Top;
            panel_logo.Location = new Point(0, 0);
            panel_logo.Name = "panel_logo";
            panel_logo.Size = new Size(224, 75);
            panel_logo.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.Transparent;
            label1.Location = new Point(85, 36);
            label1.Name = "label1";
            label1.Size = new Size(50, 21);
            label1.TabIndex = 1;
            label1.Text = "Menu";
            label1.Click += label1_Click_sidebarTimer;
            // 
            // menuButton
            // 
            menuButton.Image = Properties.Resources.icons8_menu_50;
            menuButton.Location = new Point(25, 26);
            menuButton.Name = "menuButton";
            menuButton.Size = new Size(54, 39);
            menuButton.SizeMode = PictureBoxSizeMode.Zoom;
            menuButton.TabIndex = 0;
            menuButton.TabStop = false;
            menuButton.Click += menuButton_Click;
            // 
            // sidebarTimer
            // 
            sidebarTimer.Tick += label1_Click_sidebarTimer;
            // 
            // panelContent
            // 
            panelContent.BackColor = Color.FromArgb(30, 30, 30);
            panelContent.Controls.Add(panelOverviewRoot);
            panelContent.Dock = DockStyle.Fill;
            panelContent.Location = new Point(224, 0);
            panelContent.Name = "panelContent";
            panelContent.Padding = new Padding(12);
            panelContent.Size = new Size(976, 700);
            panelContent.TabIndex = 1;
            // 
            // panelOverviewRoot
            // 
            panelOverviewRoot.BackColor = Color.FromArgb(35, 35, 35);
            panelOverviewRoot.Controls.Add(panelDeviceStatus);
            panelOverviewRoot.Controls.Add(panelSessions);
            panelOverviewRoot.Controls.Add(panelPopularGesture);
            panelOverviewRoot.Controls.Add(panelKpi4);
            panelOverviewRoot.Controls.Add(panelKpi3);
            panelOverviewRoot.Controls.Add(panelKpi2);
            panelOverviewRoot.Controls.Add(panelKpi1);
            panelOverviewRoot.Controls.Add(lblOverviewTitle);
            panelOverviewRoot.Dock = DockStyle.Fill;
            panelOverviewRoot.Location = new Point(12, 12);
            panelOverviewRoot.Name = "panelOverviewRoot";
            panelOverviewRoot.Size = new Size(952, 676);
            panelOverviewRoot.TabIndex = 0;
            // 
            // panelDeviceStatus
            // 
            panelDeviceStatus.BackColor = Color.FromArgb(46, 46, 46);
            panelDeviceStatus.Controls.Add(flowDevices);
            panelDeviceStatus.Controls.Add(labelSectionDevices);
            panelDeviceStatus.Location = new Point(22, 488);
            panelDeviceStatus.Name = "panelDeviceStatus";
            panelDeviceStatus.Size = new Size(908, 170);
            panelDeviceStatus.TabIndex = 7;
            // 
            // flowDevices
            // 
            flowDevices.AutoScroll = true;
            flowDevices.Location = new Point(16, 42);
            flowDevices.Name = "flowDevices";
            flowDevices.Size = new Size(876, 114);
            flowDevices.TabIndex = 1;
            // 
            // labelSectionDevices
            // 
            labelSectionDevices.AutoSize = true;
            labelSectionDevices.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            labelSectionDevices.ForeColor = Color.WhiteSmoke;
            labelSectionDevices.Location = new Point(16, 12);
            labelSectionDevices.Name = "labelSectionDevices";
            labelSectionDevices.Size = new Size(164, 21);
            labelSectionDevices.TabIndex = 0;
            labelSectionDevices.Text = "Trạng thái thiết bị";
            // 
            // panelSessions
            // 
            panelSessions.BackColor = Color.FromArgb(46, 46, 46);
            panelSessions.Controls.Add(dgvSessions);
            panelSessions.Controls.Add(labelSectionSessions);
            panelSessions.Location = new Point(451, 227);
            panelSessions.Name = "panelSessions";
            panelSessions.Size = new Size(479, 245);
            panelSessions.TabIndex = 6;
            // 
            // dgvSessions
            // 
            dgvSessions.AllowUserToAddRows = false;
            dgvSessions.AllowUserToDeleteRows = false;
            dgvSessions.AllowUserToResizeRows = false;
            dgvSessions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSessions.BackgroundColor = Color.FromArgb(46, 46, 46);
            dgvSessions.BorderStyle = BorderStyle.None;
            dgvSessions.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSessions.EnableHeadersVisualStyles = false;
            dgvSessions.Location = new Point(16, 44);
            dgvSessions.MultiSelect = false;
            dgvSessions.Name = "dgvSessions";
            dgvSessions.ReadOnly = true;
            dgvSessions.RowHeadersVisible = false;
            dgvSessions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSessions.Size = new Size(447, 184);
            dgvSessions.TabIndex = 1;
            // 
            // labelSectionSessions
            // 
            labelSectionSessions.AutoSize = true;
            labelSectionSessions.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            labelSectionSessions.ForeColor = Color.WhiteSmoke;
            labelSectionSessions.Location = new Point(16, 12);
            labelSectionSessions.Name = "labelSectionSessions";
            labelSectionSessions.Size = new Size(156, 21);
            labelSectionSessions.TabIndex = 0;
            labelSectionSessions.Text = "Danh sách sessions";
            // 
            // panelPopularGesture
            // 
            panelPopularGesture.BackColor = Color.FromArgb(46, 46, 46);
            panelPopularGesture.Controls.Add(lblQualityRange);
            panelPopularGesture.Controls.Add(lblConfidenceRange);
            panelPopularGesture.Controls.Add(lblPopularPercent);
            panelPopularGesture.Controls.Add(lblPopularGestureName);
            panelPopularGesture.Controls.Add(labelSectionPopular);
            panelPopularGesture.Location = new Point(22, 227);
            panelPopularGesture.Name = "panelPopularGesture";
            panelPopularGesture.Size = new Size(413, 245);
            panelPopularGesture.TabIndex = 5;
            // 
            // lblQualityRange
            // 
            lblQualityRange.AutoSize = true;
            lblQualityRange.Font = new Font("Segoe UI", 11F);
            lblQualityRange.ForeColor = Color.FromArgb(128, 255, 128);
            lblQualityRange.Location = new Point(16, 186);
            lblQualityRange.Name = "lblQualityRange";
            lblQualityRange.Size = new Size(127, 20);
            lblQualityRange.TabIndex = 4;
            lblQualityRange.Text = "Quality: chưa có";
            // 
            // lblConfidenceRange
            // 
            lblConfidenceRange.AutoSize = true;
            lblConfidenceRange.Font = new Font("Segoe UI", 11F);
            lblConfidenceRange.ForeColor = Color.Gainsboro;
            lblConfidenceRange.Location = new Point(16, 150);
            lblConfidenceRange.Name = "lblConfidenceRange";
            lblConfidenceRange.Size = new Size(162, 20);
            lblConfidenceRange.TabIndex = 3;
            lblConfidenceRange.Text = "Confidence: chưa có dữ liệu";
            // 
            // lblPopularPercent
            // 
            lblPopularPercent.AutoSize = true;
            lblPopularPercent.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold);
            lblPopularPercent.ForeColor = Color.FromArgb(168, 142, 255);
            lblPopularPercent.Location = new Point(16, 103);
            lblPopularPercent.Name = "lblPopularPercent";
            lblPopularPercent.Size = new Size(68, 37);
            lblPopularPercent.TabIndex = 2;
            lblPopularPercent.Text = "0%";
            // 
            // lblPopularGestureName
            // 
            lblPopularGestureName.AutoSize = true;
            lblPopularGestureName.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold);
            lblPopularGestureName.ForeColor = Color.WhiteSmoke;
            lblPopularGestureName.Location = new Point(16, 58);
            lblPopularGestureName.Name = "lblPopularGestureName";
            lblPopularGestureName.Size = new Size(114, 30);
            lblPopularGestureName.TabIndex = 1;
            lblPopularGestureName.Text = "Chưa có dữ liệu";
            // 
            // labelSectionPopular
            // 
            labelSectionPopular.AutoSize = true;
            labelSectionPopular.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            labelSectionPopular.ForeColor = Color.WhiteSmoke;
            labelSectionPopular.Location = new Point(16, 12);
            labelSectionPopular.Name = "labelSectionPopular";
            labelSectionPopular.Size = new Size(189, 21);
            labelSectionPopular.TabIndex = 0;
            labelSectionPopular.Text = "Gesture phổ biến nhất";
            // 
            // panelKpi4
            // 
            panelKpi4.BackColor = Color.FromArgb(46, 46, 46);
            panelKpi4.Controls.Add(lblKpiDeviceDetail);
            panelKpi4.Controls.Add(lblKpiDeviceValue);
            panelKpi4.Controls.Add(lblKpiDeviceTitle);
            panelKpi4.Location = new Point(730, 73);
            panelKpi4.Name = "panelKpi4";
            panelKpi4.Size = new Size(200, 136);
            panelKpi4.TabIndex = 4;
            // 
            // lblKpiDeviceDetail
            // 
            lblKpiDeviceDetail.AutoSize = true;
            lblKpiDeviceDetail.Font = new Font("Segoe UI", 10F);
            lblKpiDeviceDetail.ForeColor = Color.Gainsboro;
            lblKpiDeviceDetail.Location = new Point(12, 99);
            lblKpiDeviceDetail.Name = "lblKpiDeviceDetail";
            lblKpiDeviceDetail.Size = new Size(87, 19);
            lblKpiDeviceDetail.TabIndex = 2;
            lblKpiDeviceDetail.Text = "0 hoạt động";
            // 
            // lblKpiDeviceValue
            // 
            lblKpiDeviceValue.AutoSize = true;
            lblKpiDeviceValue.Font = new Font("Segoe UI Semibold", 26F, FontStyle.Bold);
            lblKpiDeviceValue.ForeColor = Color.WhiteSmoke;
            lblKpiDeviceValue.Location = new Point(12, 43);
            lblKpiDeviceValue.Name = "lblKpiDeviceValue";
            lblKpiDeviceValue.Size = new Size(75, 47);
            lblKpiDeviceValue.TabIndex = 1;
            lblKpiDeviceValue.Text = "0/0";
            // 
            // lblKpiDeviceTitle
            // 
            lblKpiDeviceTitle.AutoSize = true;
            lblKpiDeviceTitle.Font = new Font("Segoe UI", 10F);
            lblKpiDeviceTitle.ForeColor = Color.Gainsboro;
            lblKpiDeviceTitle.Location = new Point(12, 12);
            lblKpiDeviceTitle.Name = "lblKpiDeviceTitle";
            lblKpiDeviceTitle.Size = new Size(111, 19);
            lblKpiDeviceTitle.TabIndex = 0;
            lblKpiDeviceTitle.Text = "Devices hoạt động";
            // 
            // panelKpi3
            // 
            panelKpi3.BackColor = Color.FromArgb(46, 46, 46);
            panelKpi3.Controls.Add(lblKpiAvgDetail);
            panelKpi3.Controls.Add(lblKpiAvgValue);
            panelKpi3.Controls.Add(lblKpiAvgTitle);
            panelKpi3.Location = new Point(494, 73);
            panelKpi3.Name = "panelKpi3";
            panelKpi3.Size = new Size(220, 136);
            panelKpi3.TabIndex = 3;
            // 
            // lblKpiAvgDetail
            // 
            lblKpiAvgDetail.AutoSize = true;
            lblKpiAvgDetail.Font = new Font("Segoe UI", 10F);
            lblKpiAvgDetail.ForeColor = Color.FromArgb(97, 202, 142);
            lblKpiAvgDetail.Location = new Point(12, 99);
            lblKpiAvgDetail.Name = "lblKpiAvgDetail";
            lblKpiAvgDetail.Size = new Size(104, 19);
            lblKpiAvgDetail.TabIndex = 2;
            lblKpiAvgDetail.Text = "session #-- | --";
            // 
            // lblKpiAvgValue
            // 
            lblKpiAvgValue.AutoSize = true;
            lblKpiAvgValue.Font = new Font("Segoe UI Semibold", 26F, FontStyle.Bold);
            lblKpiAvgValue.ForeColor = Color.WhiteSmoke;
            lblKpiAvgValue.Location = new Point(12, 43);
            lblKpiAvgValue.Name = "lblKpiAvgValue";
            lblKpiAvgValue.Size = new Size(68, 47);
            lblKpiAvgValue.TabIndex = 1;
            lblKpiAvgValue.Text = "0%";
            // 
            // lblKpiAvgTitle
            // 
            lblKpiAvgTitle.AutoSize = true;
            lblKpiAvgTitle.Font = new Font("Segoe UI", 10F);
            lblKpiAvgTitle.ForeColor = Color.Gainsboro;
            lblKpiAvgTitle.Location = new Point(12, 12);
            lblKpiAvgTitle.Name = "lblKpiAvgTitle";
            lblKpiAvgTitle.Size = new Size(151, 19);
            lblKpiAvgTitle.TabIndex = 0;
            lblKpiAvgTitle.Text = "Avg confidence cao nhất";
            // 
            // panelKpi2
            // 
            panelKpi2.BackColor = Color.FromArgb(46, 46, 46);
            panelKpi2.Controls.Add(lblKpiGesturesDetail);
            panelKpi2.Controls.Add(lblKpiGesturesValue);
            panelKpi2.Controls.Add(lblKpiGesturesTitle);
            panelKpi2.Location = new Point(258, 73);
            panelKpi2.Name = "panelKpi2";
            panelKpi2.Size = new Size(220, 136);
            panelKpi2.TabIndex = 2;
            // 
            // lblKpiGesturesDetail
            // 
            lblKpiGesturesDetail.AutoSize = true;
            lblKpiGesturesDetail.Font = new Font("Segoe UI", 10F);
            lblKpiGesturesDetail.ForeColor = Color.Gainsboro;
            lblKpiGesturesDetail.Location = new Point(12, 99);
            lblKpiGesturesDetail.Name = "lblKpiGesturesDetail";
            lblKpiGesturesDetail.Size = new Size(112, 19);
            lblKpiGesturesDetail.TabIndex = 2;
            lblKpiGesturesDetail.Text = "TotalGestures tổng";
            // 
            // lblKpiGesturesValue
            // 
            lblKpiGesturesValue.AutoSize = true;
            lblKpiGesturesValue.Font = new Font("Segoe UI Semibold", 26F, FontStyle.Bold);
            lblKpiGesturesValue.ForeColor = Color.WhiteSmoke;
            lblKpiGesturesValue.Location = new Point(12, 43);
            lblKpiGesturesValue.Name = "lblKpiGesturesValue";
            lblKpiGesturesValue.Size = new Size(56, 47);
            lblKpiGesturesValue.TabIndex = 1;
            lblKpiGesturesValue.Text = "0";
            // 
            // lblKpiGesturesTitle
            // 
            lblKpiGesturesTitle.AutoSize = true;
            lblKpiGesturesTitle.Font = new Font("Segoe UI", 10F);
            lblKpiGesturesTitle.ForeColor = Color.Gainsboro;
            lblKpiGesturesTitle.Location = new Point(12, 12);
            lblKpiGesturesTitle.Name = "lblKpiGesturesTitle";
            lblKpiGesturesTitle.Size = new Size(157, 19);
            lblKpiGesturesTitle.TabIndex = 0;
            lblKpiGesturesTitle.Text = "Tổng gestures ghi nhận";
            // 
            // panelKpi1
            // 
            panelKpi1.BackColor = Color.FromArgb(46, 46, 46);
            panelKpi1.Controls.Add(lblKpiSessionDetail);
            panelKpi1.Controls.Add(lblKpiSessionValue);
            panelKpi1.Controls.Add(lblKpiSessionTitle);
            panelKpi1.Location = new Point(22, 73);
            panelKpi1.Name = "panelKpi1";
            panelKpi1.Size = new Size(220, 136);
            panelKpi1.TabIndex = 1;
            // 
            // lblKpiSessionDetail
            // 
            lblKpiSessionDetail.AutoSize = true;
            lblKpiSessionDetail.Font = new Font("Segoe UI", 10F);
            lblKpiSessionDetail.ForeColor = Color.Gainsboro;
            lblKpiSessionDetail.Location = new Point(12, 99);
            lblKpiSessionDetail.Name = "lblKpiSessionDetail";
            lblKpiSessionDetail.Size = new Size(130, 19);
            lblKpiSessionDetail.TabIndex = 2;
            lblKpiSessionDetail.Text = "0 chưa kết thúc";
            // 
            // lblKpiSessionValue
            // 
            lblKpiSessionValue.AutoSize = true;
            lblKpiSessionValue.Font = new Font("Segoe UI Semibold", 26F, FontStyle.Bold);
            lblKpiSessionValue.ForeColor = Color.WhiteSmoke;
            lblKpiSessionValue.Location = new Point(12, 43);
            lblKpiSessionValue.Name = "lblKpiSessionValue";
            lblKpiSessionValue.Size = new Size(56, 47);
            lblKpiSessionValue.TabIndex = 1;
            lblKpiSessionValue.Text = "0";
            // 
            // lblKpiSessionTitle
            // 
            lblKpiSessionTitle.AutoSize = true;
            lblKpiSessionTitle.Font = new Font("Segoe UI", 10F);
            lblKpiSessionTitle.ForeColor = Color.Gainsboro;
            lblKpiSessionTitle.Location = new Point(12, 12);
            lblKpiSessionTitle.Name = "lblKpiSessionTitle";
            lblKpiSessionTitle.Size = new Size(95, 19);
            lblKpiSessionTitle.TabIndex = 0;
            lblKpiSessionTitle.Text = "Tổng Sessions";
            // 
            // lblOverviewTitle
            // 
            lblOverviewTitle.AutoSize = true;
            lblOverviewTitle.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblOverviewTitle.ForeColor = Color.WhiteSmoke;
            lblOverviewTitle.Location = new Point(22, 22);
            lblOverviewTitle.Name = "lblOverviewTitle";
            lblOverviewTitle.Size = new Size(100, 30);
            lblOverviewTitle.TabIndex = 0;
            lblOverviewTitle.Text = "Overview";
            // 
            // DashboardAdmin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1200, 700);
            Controls.Add(panelContent);
            Controls.Add(sidebar);
            Name = "DashboardAdmin";
            Text = "DashboardAdmin";
            Load += DashboardAdmin_Load;
            sidebar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)button_logout).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel_logo.ResumeLayout(false);
            panel_logo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)menuButton).EndInit();
            panelContent.ResumeLayout(false);
            panelOverviewRoot.ResumeLayout(false);
            panelOverviewRoot.PerformLayout();
            panelDeviceStatus.ResumeLayout(false);
            panelDeviceStatus.PerformLayout();
            panelSessions.ResumeLayout(false);
            panelSessions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSessions).EndInit();
            panelPopularGesture.ResumeLayout(false);
            panelPopularGesture.PerformLayout();
            panelKpi4.ResumeLayout(false);
            panelKpi4.PerformLayout();
            panelKpi3.ResumeLayout(false);
            panelKpi3.PerformLayout();
            panelKpi2.ResumeLayout(false);
            panelKpi2.PerformLayout();
            panelKpi1.ResumeLayout(false);
            panelKpi1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel sidebar;
        private Panel panel_logo;
        private Button button_Overview;
        private PictureBox pictureBox1;
        private Button button_Account;
        private Button button_Device;
        private Button button_Session;
        private PictureBox pictureBox4;
        private PictureBox pictureBox3;
        private PictureBox pictureBox2;
        private Label label1;
        private PictureBox menuButton;
        private System.Windows.Forms.Timer sidebarTimer;
        private PictureBox button_logout;
        private Panel panelContent;
        private Panel panelOverviewRoot;
        private Panel panelDeviceStatus;
        private FlowLayoutPanel flowDevices;
        private Label labelSectionDevices;
        private Panel panelSessions;
        private DataGridView dgvSessions;
        private Label labelSectionSessions;
        private Panel panelPopularGesture;
        private Label lblQualityRange;
        private Label lblConfidenceRange;
        private Label lblPopularPercent;
        private Label lblPopularGestureName;
        private Label labelSectionPopular;
        private Panel panelKpi4;
        private Label lblKpiDeviceDetail;
        private Label lblKpiDeviceValue;
        private Label lblKpiDeviceTitle;
        private Panel panelKpi3;
        private Label lblKpiAvgDetail;
        private Label lblKpiAvgValue;
        private Label lblKpiAvgTitle;
        private Panel panelKpi2;
        private Label lblKpiGesturesDetail;
        private Label lblKpiGesturesValue;
        private Label lblKpiGesturesTitle;
        private Panel panelKpi1;
        private Label lblKpiSessionDetail;
        private Label lblKpiSessionValue;
        private Label lblKpiSessionTitle;
        private Label lblOverviewTitle;
    }
}
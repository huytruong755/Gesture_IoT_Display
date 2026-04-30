using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp2
{
    public partial class DashboardAdmin : Form
    {
        private bool sidebarExpand;
        private readonly Color _cardBackColor = Color.FromArgb(55, 55, 55);

        private readonly List<SessionInfo> _sessions = new List<SessionInfo>();
        private string _sessionFilter = "All";
        private Panel? _sessionRoot;
        private FlowLayoutPanel? _sessionCardsFlow;
        private TextBox? _sessionSearchBox;
        private FlowLayoutPanel? _locationSummaryFlow;
        private FlowLayoutPanel? _confidenceSummaryFlow;
        private FlowLayoutPanel? _speedSummaryFlow;
        private Label? _sessionCountLabel;
        private Label? _sessionOpenLabel;
        private Label? _sessionGesturesLabel;
        private Label? _sessionAvgLabel;

        private readonly List<DeviceInfo> _devices = new List<DeviceInfo>();
        private Panel? _deviceRoot;
        private FlowLayoutPanel? _deviceCardsFlow;
        private Label? _deviceTotalLabel;
        private Label? _deviceOnlineLabel;
        private Label? _deviceBaudLabel;
        private Label? _deviceLastSeenLabel;
        private Label? _deviceAlertLabel;
        private Label? _deviceConflictLabel;

        private readonly List<AccountInfo> _accounts = new List<AccountInfo>();
        private Panel? _accountRoot;
        private FlowLayoutPanel? _accountListFlow;
        private TextBox? _accountSearchBox;
        private string _accountFilter = "All";
        private Label? _accountTotalKpi;
        private Label? _accountAdminKpi;
        private Label? _accountUserKpi;
        private Label? _accountSecurityKpi;
        private Label? _accDetailName;
        private Label? _accDetailRoleBadge;
        private TextBox? _accDetailUsername;
        private TextBox? _accDetailEmail;
        private TextBox? _accDetailPassword;
        private Label? _accPassWarn;
        private Label? _accStrengthLabel;
        private Panel? _accStrengthFill;
        private AccountInfo? _selectedAccount;

        public DashboardAdmin()
        {
            InitializeComponent();
        }

        private void label1_Click_sidebarTimer(object sender, EventArgs e)
        {
            if (sidebarExpand)
            {
                sidebar.Width -= 10;
                if (sidebar.Width == sidebar.MinimumSize.Width)
                {
                    sidebarExpand = false;
                    sidebarTimer.Stop();
                }
            }
            else
            {
                sidebar.Width += 10;
                if (sidebar.Width == sidebar.MaximumSize.Width)
                {
                    sidebarExpand = true;
                    sidebarTimer.Stop();
                }
            }
        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            sidebarTimer.Start();
        }

        private void button_Overview_Click(object sender, EventArgs e)
        {
            label1.Text = "Overview";
            panelOverviewRoot.Visible = true;
            if (_sessionRoot != null) _sessionRoot.Visible = false;
            if (_deviceRoot != null) _deviceRoot.Visible = false;
            if (_accountRoot != null) _accountRoot.Visible = false;
            LoadOverviewData();
        }

        private void button_Account_Click(object sender, EventArgs e)
        {
            label1.Text = "Accounts";
            EnsureAccountUi();
            panelOverviewRoot.Visible = false;
            if (_sessionRoot != null) _sessionRoot.Visible = false;
            if (_deviceRoot != null) _deviceRoot.Visible = false;
            if (_accountRoot != null) _accountRoot.Visible = true;
            LoadAccountData();
        }

        private void button_Device_Click(object sender, EventArgs e)
        {
            label1.Text = "Devices";
            EnsureDeviceUi();
            panelOverviewRoot.Visible = false;
            if (_sessionRoot != null) _sessionRoot.Visible = false;
            if (_accountRoot != null) _accountRoot.Visible = false;
            if (_deviceRoot != null) _deviceRoot.Visible = true;
            LoadDeviceScreenData();
        }

        private void button_Session_Click(object sender, EventArgs e)
        {
            label1.Text = "Sessions";
            EnsureSessionUi();
            panelOverviewRoot.Visible = false;
            if (_deviceRoot != null) _deviceRoot.Visible = false;
            if (_accountRoot != null) _accountRoot.Visible = false;
            if (_sessionRoot != null) _sessionRoot.Visible = true;
            LoadSessionData();
        }

        private void button_logout_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DashboardAdmin_Load(object sender, EventArgs e)
        {
            ConfigureSessionGrid();
            EnsureSessionUi();
            EnsureDeviceUi();
            EnsureAccountUi();
            LoadOverviewData();
        }
    }
}

namespace WISEPaaS.SCADA.DotNet.SDK.Test
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numTTagCount = new System.Windows.Forms.NumericUpDown();
            this.lblTTagCount = new System.Windows.Forms.Label();
            this.numDTagCount = new System.Windows.Forms.NumericUpDown();
            this.lblDTagCount = new System.Windows.Forms.Label();
            this.numATagCount = new System.Windows.Forms.NumericUpDown();
            this.lblATagCount = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lblDCCSKey = new System.Windows.Forms.Label();
            this.txtDCCSKey = new System.Windows.Forms.TextBox();
            this.lblDCCSAPIUrl = new System.Windows.Forms.Label();
            this.txtDCCSAPIUrl = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lblHostName = new System.Windows.Forms.Label();
            this.txtHostName = new System.Windows.Forms.TextBox();
            this.lblUserName = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.btnDeleteTags = new System.Windows.Forms.Button();
            this.btnDeleteDevices = new System.Windows.Forms.Button();
            this.btnDeleteAllConfig = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.ckbSecure = new System.Windows.Forms.CheckBox();
            this.lblScadaId = new System.Windows.Forms.Label();
            this.btnUpdateConfig = new System.Windows.Forms.Button();
            this.btnDeviceStatus = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.txtScadaId = new System.Windows.Forms.TextBox();
            this.btnSendData = new System.Windows.Forms.Button();
            this.btnUploadConfig = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.lblDeviceCount = new System.Windows.Forms.Label();
            this.numDeviceCount = new System.Windows.Forms.NumericUpDown();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblDataFreq = new System.Windows.Forms.Label();
            this.numDataFreq = new System.Windows.Forms.NumericUpDown();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTTagCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDTagCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numATagCount)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDeviceCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDataFreq)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblDataFreq);
            this.groupBox2.Controls.Add(this.numDataFreq);
            this.groupBox2.Controls.Add(this.numTTagCount);
            this.groupBox2.Controls.Add(this.lblTTagCount);
            this.groupBox2.Controls.Add(this.numDTagCount);
            this.groupBox2.Controls.Add(this.lblDTagCount);
            this.groupBox2.Controls.Add(this.numATagCount);
            this.groupBox2.Controls.Add(this.lblATagCount);
            this.groupBox2.Controls.Add(this.tabControl1);
            this.groupBox2.Controls.Add(this.btnDeleteTags);
            this.groupBox2.Controls.Add(this.btnDeleteDevices);
            this.groupBox2.Controls.Add(this.btnDeleteAllConfig);
            this.groupBox2.Controls.Add(this.btnDisconnect);
            this.groupBox2.Controls.Add(this.ckbSecure);
            this.groupBox2.Controls.Add(this.lblScadaId);
            this.groupBox2.Controls.Add(this.btnUpdateConfig);
            this.groupBox2.Controls.Add(this.btnDeviceStatus);
            this.groupBox2.Controls.Add(this.lblStatus);
            this.groupBox2.Controls.Add(this.txtScadaId);
            this.groupBox2.Controls.Add(this.btnSendData);
            this.groupBox2.Controls.Add(this.btnUploadConfig);
            this.groupBox2.Controls.Add(this.btnConnect);
            this.groupBox2.Controls.Add(this.lblDeviceCount);
            this.groupBox2.Controls.Add(this.numDeviceCount);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(812, 481);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            // 
            // numTTagCount
            // 
            this.numTTagCount.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.numTTagCount.Location = new System.Drawing.Point(364, 438);
            this.numTTagCount.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.numTTagCount.Name = "numTTagCount";
            this.numTTagCount.Size = new System.Drawing.Size(120, 29);
            this.numTTagCount.TabIndex = 50;
            this.numTTagCount.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // lblTTagCount
            // 
            this.lblTTagCount.AutoSize = true;
            this.lblTTagCount.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblTTagCount.Location = new System.Drawing.Point(360, 415);
            this.lblTTagCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTTagCount.Name = "lblTTagCount";
            this.lblTTagCount.Size = new System.Drawing.Size(124, 20);
            this.lblTTagCount.TabIndex = 49;
            this.lblTTagCount.Text = "Text Tag Count";
            // 
            // numDTagCount
            // 
            this.numDTagCount.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.numDTagCount.Location = new System.Drawing.Point(185, 438);
            this.numDTagCount.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.numDTagCount.Name = "numDTagCount";
            this.numDTagCount.Size = new System.Drawing.Size(120, 29);
            this.numDTagCount.TabIndex = 48;
            this.numDTagCount.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // lblDTagCount
            // 
            this.lblDTagCount.AutoSize = true;
            this.lblDTagCount.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblDTagCount.Location = new System.Drawing.Point(181, 415);
            this.lblDTagCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDTagCount.Name = "lblDTagCount";
            this.lblDTagCount.Size = new System.Drawing.Size(153, 20);
            this.lblDTagCount.TabIndex = 47;
            this.lblDTagCount.Text = "Discrete Tag Count";
            // 
            // numATagCount
            // 
            this.numATagCount.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.numATagCount.Location = new System.Drawing.Point(25, 438);
            this.numATagCount.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.numATagCount.Name = "numATagCount";
            this.numATagCount.Size = new System.Drawing.Size(120, 29);
            this.numATagCount.TabIndex = 46;
            this.numATagCount.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // lblATagCount
            // 
            this.lblATagCount.AutoSize = true;
            this.lblATagCount.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblATagCount.Location = new System.Drawing.Point(22, 415);
            this.lblATagCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblATagCount.Name = "lblATagCount";
            this.lblATagCount.Size = new System.Drawing.Size(146, 20);
            this.lblATagCount.TabIndex = 45;
            this.lblATagCount.Text = "Analog Tag Count";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(0, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(356, 247);
            this.tabControl1.TabIndex = 27;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.lblDCCSKey);
            this.tabPage1.Controls.Add(this.txtDCCSKey);
            this.tabPage1.Controls.Add(this.lblDCCSAPIUrl);
            this.tabPage1.Controls.Add(this.txtDCCSAPIUrl);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(348, 214);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "DCCS";
            // 
            // lblDCCSKey
            // 
            this.lblDCCSKey.AutoSize = true;
            this.lblDCCSKey.Location = new System.Drawing.Point(12, 18);
            this.lblDCCSKey.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDCCSKey.Name = "lblDCCSKey";
            this.lblDCCSKey.Size = new System.Drawing.Size(118, 20);
            this.lblDCCSKey.TabIndex = 33;
            this.lblDCCSKey.Text = "Credential Key";
            // 
            // txtDCCSKey
            // 
            this.txtDCCSKey.Location = new System.Drawing.Point(18, 43);
            this.txtDCCSKey.Margin = new System.Windows.Forms.Padding(4);
            this.txtDCCSKey.Name = "txtDCCSKey";
            this.txtDCCSKey.Size = new System.Drawing.Size(309, 29);
            this.txtDCCSKey.TabIndex = 34;
            // 
            // lblDCCSAPIUrl
            // 
            this.lblDCCSAPIUrl.AutoSize = true;
            this.lblDCCSAPIUrl.Location = new System.Drawing.Point(14, 81);
            this.lblDCCSAPIUrl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDCCSAPIUrl.Name = "lblDCCSAPIUrl";
            this.lblDCCSAPIUrl.Size = new System.Drawing.Size(61, 20);
            this.lblDCCSAPIUrl.TabIndex = 35;
            this.lblDCCSAPIUrl.Text = "API Url";
            // 
            // txtDCCSAPIUrl
            // 
            this.txtDCCSAPIUrl.Location = new System.Drawing.Point(17, 105);
            this.txtDCCSAPIUrl.Margin = new System.Windows.Forms.Padding(4);
            this.txtDCCSAPIUrl.Name = "txtDCCSAPIUrl";
            this.txtDCCSAPIUrl.Size = new System.Drawing.Size(309, 29);
            this.txtDCCSAPIUrl.TabIndex = 36;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.lblHostName);
            this.tabPage2.Controls.Add(this.txtHostName);
            this.tabPage2.Controls.Add(this.lblUserName);
            this.tabPage2.Controls.Add(this.txtUserName);
            this.tabPage2.Controls.Add(this.lblPassword);
            this.tabPage2.Controls.Add(this.txtPassword);
            this.tabPage2.Controls.Add(this.lblPort);
            this.tabPage2.Controls.Add(this.txtPort);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(348, 214);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "MQTT";
            // 
            // lblHostName
            // 
            this.lblHostName.AutoSize = true;
            this.lblHostName.Location = new System.Drawing.Point(16, 15);
            this.lblHostName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHostName.Name = "lblHostName";
            this.lblHostName.Size = new System.Drawing.Size(90, 20);
            this.lblHostName.TabIndex = 23;
            this.lblHostName.Text = "HostName";
            // 
            // txtHostName
            // 
            this.txtHostName.Location = new System.Drawing.Point(19, 39);
            this.txtHostName.Margin = new System.Windows.Forms.Padding(4);
            this.txtHostName.Name = "txtHostName";
            this.txtHostName.Size = new System.Drawing.Size(190, 29);
            this.txtHostName.TabIndex = 24;
            this.txtHostName.Text = "127.0.0.1";
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(14, 72);
            this.lblUserName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(86, 20);
            this.lblUserName.TabIndex = 26;
            this.lblUserName.Text = "Username";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(20, 97);
            this.txtUserName.Margin = new System.Windows.Forms.Padding(4);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(309, 29);
            this.txtUserName.TabIndex = 28;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(16, 135);
            this.lblPassword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(80, 20);
            this.lblPassword.TabIndex = 30;
            this.lblPassword.Text = "Password";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(19, 159);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(309, 29);
            this.txtPassword.TabIndex = 32;
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(223, 15);
            this.lblPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(41, 20);
            this.lblPort.TabIndex = 34;
            this.lblPort.Text = "Port";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(227, 39);
            this.txtPort.Margin = new System.Windows.Forms.Padding(4);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(101, 29);
            this.txtPort.TabIndex = 36;
            this.txtPort.Text = "1883";
            // 
            // btnDeleteTags
            // 
            this.btnDeleteTags.Location = new System.Drawing.Point(661, 348);
            this.btnDeleteTags.Margin = new System.Windows.Forms.Padding(5);
            this.btnDeleteTags.Name = "btnDeleteTags";
            this.btnDeleteTags.Size = new System.Drawing.Size(134, 69);
            this.btnDeleteTags.TabIndex = 26;
            this.btnDeleteTags.Text = "Delete Tags";
            this.btnDeleteTags.UseVisualStyleBackColor = true;
            this.btnDeleteTags.Click += new System.EventHandler(this.btnDeleteTags_Click);
            // 
            // btnDeleteDevices
            // 
            this.btnDeleteDevices.Location = new System.Drawing.Point(661, 269);
            this.btnDeleteDevices.Margin = new System.Windows.Forms.Padding(5);
            this.btnDeleteDevices.Name = "btnDeleteDevices";
            this.btnDeleteDevices.Size = new System.Drawing.Size(134, 69);
            this.btnDeleteDevices.TabIndex = 25;
            this.btnDeleteDevices.Text = "Delete Devices";
            this.btnDeleteDevices.UseVisualStyleBackColor = true;
            this.btnDeleteDevices.Click += new System.EventHandler(this.btnDeleteDevices_Click);
            // 
            // btnDeleteAllConfig
            // 
            this.btnDeleteAllConfig.Location = new System.Drawing.Point(661, 190);
            this.btnDeleteAllConfig.Margin = new System.Windows.Forms.Padding(5);
            this.btnDeleteAllConfig.Name = "btnDeleteAllConfig";
            this.btnDeleteAllConfig.Size = new System.Drawing.Size(134, 69);
            this.btnDeleteAllConfig.TabIndex = 24;
            this.btnDeleteAllConfig.Text = "Delete All Config";
            this.btnDeleteAllConfig.UseVisualStyleBackColor = true;
            this.btnDeleteAllConfig.Click += new System.EventHandler(this.btnDeleteAllConfig_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(508, 147);
            this.btnDisconnect.Margin = new System.Windows.Forms.Padding(5);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(134, 69);
            this.btnDisconnect.TabIndex = 23;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // ckbSecure
            // 
            this.ckbSecure.AutoSize = true;
            this.ckbSecure.Location = new System.Drawing.Point(276, 261);
            this.ckbSecure.Margin = new System.Windows.Forms.Padding(4);
            this.ckbSecure.Name = "ckbSecure";
            this.ckbSecure.Size = new System.Drawing.Size(79, 24);
            this.ckbSecure.TabIndex = 37;
            this.ckbSecure.Text = "Secure";
            this.ckbSecure.UseVisualStyleBackColor = true;
            // 
            // lblScadaId
            // 
            this.lblScadaId.AutoSize = true;
            this.lblScadaId.Location = new System.Drawing.Point(21, 285);
            this.lblScadaId.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblScadaId.Name = "lblScadaId";
            this.lblScadaId.Size = new System.Drawing.Size(84, 20);
            this.lblScadaId.TabIndex = 25;
            this.lblScadaId.Text = "SCADA ID";
            // 
            // btnUpdateConfig
            // 
            this.btnUpdateConfig.Location = new System.Drawing.Point(661, 109);
            this.btnUpdateConfig.Margin = new System.Windows.Forms.Padding(5);
            this.btnUpdateConfig.Name = "btnUpdateConfig";
            this.btnUpdateConfig.Size = new System.Drawing.Size(134, 69);
            this.btnUpdateConfig.TabIndex = 23;
            this.btnUpdateConfig.Text = "Update Config";
            this.btnUpdateConfig.UseVisualStyleBackColor = true;
            this.btnUpdateConfig.Click += new System.EventHandler(this.btnUpdateConfig_Click);
            // 
            // btnDeviceStatus
            // 
            this.btnDeviceStatus.Location = new System.Drawing.Point(364, 238);
            this.btnDeviceStatus.Margin = new System.Windows.Forms.Padding(5);
            this.btnDeviceStatus.Name = "btnDeviceStatus";
            this.btnDeviceStatus.Size = new System.Drawing.Size(134, 69);
            this.btnDeviceStatus.TabIndex = 22;
            this.btnDeviceStatus.Text = "Update Device Status";
            this.btnDeviceStatus.UseVisualStyleBackColor = true;
            this.btnDeviceStatus.Click += new System.EventHandler(this.btnDeviceStatus_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.Color.Gray;
            this.lblStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblStatus.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblStatus.Location = new System.Drawing.Point(443, 27);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(199, 100);
            this.lblStatus.TabIndex = 21;
            this.lblStatus.Text = "DISCONNECTED";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtScadaId
            // 
            this.txtScadaId.Location = new System.Drawing.Point(22, 309);
            this.txtScadaId.Margin = new System.Windows.Forms.Padding(4);
            this.txtScadaId.Name = "txtScadaId";
            this.txtScadaId.Size = new System.Drawing.Size(309, 29);
            this.txtScadaId.TabIndex = 27;
            // 
            // btnSendData
            // 
            this.btnSendData.Location = new System.Drawing.Point(508, 238);
            this.btnSendData.Margin = new System.Windows.Forms.Padding(5);
            this.btnSendData.Name = "btnSendData";
            this.btnSendData.Size = new System.Drawing.Size(134, 69);
            this.btnSendData.TabIndex = 21;
            this.btnSendData.Text = "Send Data in Loop";
            this.btnSendData.UseVisualStyleBackColor = true;
            this.btnSendData.Click += new System.EventHandler(this.btnSendData_Click);
            // 
            // btnUploadConfig
            // 
            this.btnUploadConfig.Location = new System.Drawing.Point(661, 27);
            this.btnUploadConfig.Margin = new System.Windows.Forms.Padding(5);
            this.btnUploadConfig.Name = "btnUploadConfig";
            this.btnUploadConfig.Size = new System.Drawing.Size(134, 69);
            this.btnUploadConfig.TabIndex = 20;
            this.btnUploadConfig.Text = "Upload Config";
            this.btnUploadConfig.UseVisualStyleBackColor = true;
            this.btnUploadConfig.Click += new System.EventHandler(this.btnUploadConfig_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(364, 147);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(5);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(134, 69);
            this.btnConnect.TabIndex = 12;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // lblDeviceCount
            // 
            this.lblDeviceCount.AutoSize = true;
            this.lblDeviceCount.Location = new System.Drawing.Point(21, 351);
            this.lblDeviceCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDeviceCount.Name = "lblDeviceCount";
            this.lblDeviceCount.Size = new System.Drawing.Size(110, 20);
            this.lblDeviceCount.TabIndex = 29;
            this.lblDeviceCount.Text = "Device Count";
            // 
            // numDeviceCount
            // 
            this.numDeviceCount.Location = new System.Drawing.Point(22, 374);
            this.numDeviceCount.Name = "numDeviceCount";
            this.numDeviceCount.Size = new System.Drawing.Size(120, 29);
            this.numDeviceCount.TabIndex = 31;
            this.numDeviceCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lblDataFreq
            // 
            this.lblDataFreq.AutoSize = true;
            this.lblDataFreq.Location = new System.Drawing.Point(521, 417);
            this.lblDataFreq.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDataFreq.Name = "lblDataFreq";
            this.lblDataFreq.Size = new System.Drawing.Size(127, 20);
            this.lblDataFreq.TabIndex = 51;
            this.lblDataFreq.Text = "Data Frequency";
            // 
            // numDataFreq
            // 
            this.numDataFreq.Location = new System.Drawing.Point(522, 440);
            this.numDataFreq.Name = "numDataFreq";
            this.numDataFreq.Size = new System.Drawing.Size(98, 29);
            this.numDataFreq.TabIndex = 52;
            this.numDataFreq.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(812, 481);
            this.Controls.Add(this.groupBox2);
            this.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTTagCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDTagCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numATagCount)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDeviceCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDataFreq)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnUploadConfig;
        private System.Windows.Forms.Button btnSendData;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnDeviceStatus;
        private System.Windows.Forms.Button btnUpdateConfig;
        private System.Windows.Forms.Button btnDeleteAllConfig;
        private System.Windows.Forms.Button btnDeleteTags;
        private System.Windows.Forms.Button btnDeleteDevices;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label lblDCCSKey;
        private System.Windows.Forms.TextBox txtDCCSKey;
        private System.Windows.Forms.Label lblDCCSAPIUrl;
        private System.Windows.Forms.TextBox txtDCCSAPIUrl;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label lblHostName;
        private System.Windows.Forms.TextBox txtHostName;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.CheckBox ckbSecure;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label lblScadaId;
        private System.Windows.Forms.TextBox txtScadaId;
        private System.Windows.Forms.Label lblDeviceCount;
        private System.Windows.Forms.NumericUpDown numDeviceCount;
        private System.Windows.Forms.NumericUpDown numTTagCount;
        private System.Windows.Forms.Label lblTTagCount;
        private System.Windows.Forms.NumericUpDown numDTagCount;
        private System.Windows.Forms.Label lblDTagCount;
        private System.Windows.Forms.NumericUpDown numATagCount;
        private System.Windows.Forms.Label lblATagCount;
        private System.Windows.Forms.Label lblDataFreq;
        private System.Windows.Forms.NumericUpDown numDataFreq;

    }
}


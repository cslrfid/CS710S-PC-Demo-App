namespace CS710_PC_Client
{
    partial class RFIDForm
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cb_compact_mode = new System.Windows.Forms.CheckBox();
            this.cb_reconnect = new System.Windows.Forms.CheckBox();
            this.btn_read = new System.Windows.Forms.Button();
            this.lb_total = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lb_rate = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_version = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.tb_version = new System.Windows.Forms.TextBox();
            this.lv_tag = new CS710_PC_Client.TListView();
            this.col_index = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_pc = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_xpc1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_xpc2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_epc = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_rssi = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_count = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_phasebegin = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_phaseend = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_timestamp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_mb = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btn_stop_inventory = new System.Windows.Forms.Button();
            this.btn_inventory = new System.Windows.Forms.Button();
            this.btn_rfid_clear = new System.Windows.Forms.Button();
            this.btn_rfid_poweroff = new System.Windows.Forms.Button();
            this.btn_rfid_poweron = new System.Windows.Forms.Button();
            this.tb_info = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label42 = new System.Windows.Forms.Label();
            this.btn_reset_factory = new System.Windows.Forms.Button();
            this.cb_set_default = new System.Windows.Forms.CheckBox();
            this.tb_max_query = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.comboBox_rfmode = new System.Windows.Forms.ComboBox();
            this.comboBox_sel = new System.Windows.Forms.ComboBox();
            this.label22 = new System.Windows.Forms.Label();
            this.cb_tagcaching = new System.Windows.Forms.CheckBox();
            this.tb_deinterval = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.cb_tagfocus = new System.Windows.Forms.CheckBox();
            this.cb_fastid = new System.Windows.Forms.CheckBox();
            this.cb_q_decrease_use_query = new System.Windows.Forms.CheckBox();
            this.cb_q_increase_use_query = new System.Windows.Forms.CheckBox();
            this.comboBox_region = new System.Windows.Forms.ComboBox();
            this.tb_min_q_cycle = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tb_maxq = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tb_minq = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tb_startq = new System.Windows.Forms.TextBox();
            this.tb_channel = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.tb_power = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.btn_invset = new System.Windows.Forms.Button();
            this.comboBox_algorithm = new System.Windows.Forms.ComboBox();
            this.comboBox_session = new System.Windows.Forms.ComboBox();
            this.comboBox_target = new System.Windows.Forms.ComboBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.btn_clear_info = new System.Windows.Forms.Button();
            this.timer_rate = new System.Windows.Forms.Timer(this.components);
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btn_write_oem = new System.Windows.Forms.Button();
            this.btn_read_oem = new System.Windows.Forms.Button();
            this.btn_write_register = new System.Windows.Forms.Button();
            this.tb_register_value = new System.Windows.Forms.TextBox();
            this.label37 = new System.Windows.Forms.Label();
            this.btn_read_register = new System.Windows.Forms.Button();
            this.tb_length = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tb_address = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lb_elapsed = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.tb_status = new System.Windows.Forms.TextBox();
            this.timer_reset = new System.Windows.Forms.Timer(this.components);
            this.btn_reset = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cb_selenable = new System.Windows.Forms.CheckBox();
            this.btn_selset = new System.Windows.Forms.Button();
            this.comboBox_action = new System.Windows.Forms.ComboBox();
            this.label30 = new System.Windows.Forms.Label();
            this.comboBox_selTarget = new System.Windows.Forms.ComboBox();
            this.label29 = new System.Windows.Forms.Label();
            this.tb_selLength = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.tb_offset = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.comboBox_bank = new System.Windows.Forms.ComboBox();
            this.label25 = new System.Windows.Forms.Label();
            this.tb_mask = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cb_bank2_enable = new System.Windows.Forms.CheckBox();
            this.tb_bank2_length = new System.Windows.Forms.TextBox();
            this.label31 = new System.Windows.Forms.Label();
            this.tb_bank2_offset = new System.Windows.Forms.TextBox();
            this.label32 = new System.Windows.Forms.Label();
            this.comboBox_bank2 = new System.Windows.Forms.ComboBox();
            this.label36 = new System.Windows.Forms.Label();
            this.cb_bank1_enable = new System.Windows.Forms.CheckBox();
            this.btn_mbset = new System.Windows.Forms.Button();
            this.tb_bank1_length = new System.Windows.Forms.TextBox();
            this.label33 = new System.Windows.Forms.Label();
            this.tb_bank1_offset = new System.Windows.Forms.TextBox();
            this.label34 = new System.Windows.Forms.Label();
            this.comboBox_bank1 = new System.Windows.Forms.ComboBox();
            this.label35 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.comboBox_rx_mixer = new System.Windows.Forms.ComboBox();
            this.label41 = new System.Windows.Forms.Label();
            this.comboBox_rx_pga3 = new System.Windows.Forms.ComboBox();
            this.label40 = new System.Windows.Forms.Label();
            this.comboBox_rx_pga2 = new System.Windows.Forms.ComboBox();
            this.label39 = new System.Windows.Forms.Label();
            this.comboBox_rx_pga1 = new System.Windows.Forms.ComboBox();
            this.label38 = new System.Windows.Forms.Label();
            this.btn_rxset = new System.Windows.Forms.Button();
            this.comboBox_rx_atten = new System.Windows.Forms.ComboBox();
            this.label43 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cb_compact_mode);
            this.groupBox1.Controls.Add(this.cb_reconnect);
            this.groupBox1.Controls.Add(this.btn_read);
            this.groupBox1.Controls.Add(this.lb_total);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.lb_rate);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btn_version);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.tb_version);
            this.groupBox1.Controls.Add(this.lv_tag);
            this.groupBox1.Controls.Add(this.btn_stop_inventory);
            this.groupBox1.Controls.Add(this.btn_inventory);
            this.groupBox1.Controls.Add(this.btn_rfid_clear);
            this.groupBox1.Controls.Add(this.btn_rfid_poweroff);
            this.groupBox1.Controls.Add(this.btn_rfid_poweron);
            this.groupBox1.Location = new System.Drawing.Point(12, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(944, 280);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "RFID";
            // 
            // cb_compact_mode
            // 
            this.cb_compact_mode.AutoSize = true;
            this.cb_compact_mode.Location = new System.Drawing.Point(621, 257);
            this.cb_compact_mode.Name = "cb_compact_mode";
            this.cb_compact_mode.Size = new System.Drawing.Size(98, 17);
            this.cb_compact_mode.TabIndex = 44;
            this.cb_compact_mode.Text = "Compact Mode";
            this.cb_compact_mode.UseVisualStyleBackColor = true;
            // 
            // cb_reconnect
            // 
            this.cb_reconnect.AutoSize = true;
            this.cb_reconnect.Location = new System.Drawing.Point(724, 257);
            this.cb_reconnect.Name = "cb_reconnect";
            this.cb_reconnect.Size = new System.Drawing.Size(212, 17);
            this.cb_reconnect.TabIndex = 43;
            this.cb_reconnect.Text = "Auto-reconnect when USB is disrupted.";
            this.cb_reconnect.UseVisualStyleBackColor = true;
            this.cb_reconnect.Visible = false;
            this.cb_reconnect.CheckedChanged += new System.EventHandler(this.cb_reconnect_CheckedChanged);
            // 
            // btn_read
            // 
            this.btn_read.Location = new System.Drawing.Point(452, 208);
            this.btn_read.Name = "btn_read";
            this.btn_read.Size = new System.Drawing.Size(105, 29);
            this.btn_read.TabIndex = 24;
            this.btn_read.Text = "Read/Write";
            this.btn_read.UseVisualStyleBackColor = true;
            this.btn_read.Click += new System.EventHandler(this.btn_read_Click);
            // 
            // lb_total
            // 
            this.lb_total.AutoSize = true;
            this.lb_total.Location = new System.Drawing.Point(708, 216);
            this.lb_total.Name = "lb_total";
            this.lb_total.Size = new System.Drawing.Size(13, 13);
            this.lb_total.TabIndex = 23;
            this.lb_total.Text = "0";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(674, 216);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(37, 13);
            this.label13.TabIndex = 22;
            this.label13.Text = "Total: ";
            // 
            // lb_rate
            // 
            this.lb_rate.AutoSize = true;
            this.lb_rate.Location = new System.Drawing.Point(778, 217);
            this.lb_rate.Name = "lb_rate";
            this.lb_rate.Size = new System.Drawing.Size(13, 13);
            this.lb_rate.TabIndex = 21;
            this.lb_rate.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(802, 216);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "tags/s";
            // 
            // btn_version
            // 
            this.btn_version.Location = new System.Drawing.Point(8, 243);
            this.btn_version.Name = "btn_version";
            this.btn_version.Size = new System.Drawing.Size(105, 29);
            this.btn_version.TabIndex = 25;
            this.btn_version.Text = "Get Version";
            this.btn_version.UseVisualStyleBackColor = true;
            this.btn_version.Click += new System.EventHandler(this.btn_version_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(748, 216);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(36, 13);
            this.label11.TabIndex = 19;
            this.label11.Text = "Rate: ";
            // 
            // tb_version
            // 
            this.tb_version.Location = new System.Drawing.Point(119, 247);
            this.tb_version.MaxLength = 100;
            this.tb_version.Name = "tb_version";
            this.tb_version.ReadOnly = true;
            this.tb_version.Size = new System.Drawing.Size(88, 20);
            this.tb_version.TabIndex = 26;
            // 
            // lv_tag
            // 
            this.lv_tag.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.col_index,
            this.col_pc,
            this.col_xpc1,
            this.col_xpc2,
            this.col_epc,
            this.col_rssi,
            this.col_count,
            this.col_phasebegin,
            this.col_phaseend,
            this.col_timestamp,
            this.col_mb});
            this.lv_tag.FullRowSelect = true;
            this.lv_tag.GridLines = true;
            this.lv_tag.HideSelection = false;
            this.lv_tag.Location = new System.Drawing.Point(8, 23);
            this.lv_tag.MultiSelect = false;
            this.lv_tag.Name = "lv_tag";
            this.lv_tag.Size = new System.Drawing.Size(930, 178);
            this.lv_tag.TabIndex = 17;
            this.lv_tag.UseCompatibleStateImageBehavior = false;
            this.lv_tag.View = System.Windows.Forms.View.Details;
            // 
            // col_index
            // 
            this.col_index.Tag = "";
            this.col_index.Text = "Index";
            // 
            // col_pc
            // 
            this.col_pc.Text = "PC";
            this.col_pc.Width = 50;
            // 
            // col_xpc1
            // 
            this.col_xpc1.Text = "XPC1";
            this.col_xpc1.Width = 50;
            // 
            // col_xpc2
            // 
            this.col_xpc2.Text = "XPC2";
            this.col_xpc2.Width = 50;
            // 
            // col_epc
            // 
            this.col_epc.Text = "EPC";
            this.col_epc.Width = 210;
            // 
            // col_rssi
            // 
            this.col_rssi.Text = "RSSI";
            // 
            // col_count
            // 
            this.col_count.Text = "Count";
            this.col_count.Width = 40;
            // 
            // col_phasebegin
            // 
            this.col_phasebegin.Text = "Phase Begin";
            this.col_phasebegin.Width = 73;
            // 
            // col_phaseend
            // 
            this.col_phaseend.Text = "Phase End";
            this.col_phaseend.Width = 68;
            // 
            // col_timestamp
            // 
            this.col_timestamp.Text = "Timestamp";
            this.col_timestamp.Width = 110;
            // 
            // col_mb
            // 
            this.col_mb.Text = "Multibank";
            this.col_mb.Width = 120;
            // 
            // btn_stop_inventory
            // 
            this.btn_stop_inventory.Location = new System.Drawing.Point(341, 208);
            this.btn_stop_inventory.Name = "btn_stop_inventory";
            this.btn_stop_inventory.Size = new System.Drawing.Size(105, 29);
            this.btn_stop_inventory.TabIndex = 16;
            this.btn_stop_inventory.Text = "Stop Inventory";
            this.btn_stop_inventory.UseVisualStyleBackColor = true;
            this.btn_stop_inventory.Click += new System.EventHandler(this.btn_stop_inventory_Click);
            // 
            // btn_inventory
            // 
            this.btn_inventory.Location = new System.Drawing.Point(230, 208);
            this.btn_inventory.Name = "btn_inventory";
            this.btn_inventory.Size = new System.Drawing.Size(105, 29);
            this.btn_inventory.TabIndex = 15;
            this.btn_inventory.Text = "Start Inventory";
            this.btn_inventory.UseVisualStyleBackColor = true;
            this.btn_inventory.Click += new System.EventHandler(this.btn_inventory_Click);
            // 
            // btn_rfid_clear
            // 
            this.btn_rfid_clear.Location = new System.Drawing.Point(563, 208);
            this.btn_rfid_clear.Name = "btn_rfid_clear";
            this.btn_rfid_clear.Size = new System.Drawing.Size(105, 29);
            this.btn_rfid_clear.TabIndex = 11;
            this.btn_rfid_clear.Text = "Clear";
            this.btn_rfid_clear.UseVisualStyleBackColor = true;
            this.btn_rfid_clear.Click += new System.EventHandler(this.btn_rfid_clear_Click);
            // 
            // btn_rfid_poweroff
            // 
            this.btn_rfid_poweroff.Location = new System.Drawing.Point(119, 208);
            this.btn_rfid_poweroff.Name = "btn_rfid_poweroff";
            this.btn_rfid_poweroff.Size = new System.Drawing.Size(105, 29);
            this.btn_rfid_poweroff.TabIndex = 10;
            this.btn_rfid_poweroff.Text = "Power Off";
            this.btn_rfid_poweroff.UseVisualStyleBackColor = true;
            this.btn_rfid_poweroff.Click += new System.EventHandler(this.btn_rfid_poweroff_Click);
            // 
            // btn_rfid_poweron
            // 
            this.btn_rfid_poweron.Location = new System.Drawing.Point(8, 208);
            this.btn_rfid_poweron.Name = "btn_rfid_poweron";
            this.btn_rfid_poweron.Size = new System.Drawing.Size(105, 29);
            this.btn_rfid_poweron.TabIndex = 7;
            this.btn_rfid_poweron.Text = "Power On";
            this.btn_rfid_poweron.UseVisualStyleBackColor = true;
            this.btn_rfid_poweron.Click += new System.EventHandler(this.btn_rfid_poweron_Click);
            // 
            // tb_info
            // 
            this.tb_info.Location = new System.Drawing.Point(12, 681);
            this.tb_info.Multiline = true;
            this.tb_info.Name = "tb_info";
            this.tb_info.ReadOnly = true;
            this.tb_info.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_info.Size = new System.Drawing.Size(342, 77);
            this.tb_info.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 664);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Info:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label42);
            this.groupBox2.Controls.Add(this.btn_reset_factory);
            this.groupBox2.Controls.Add(this.cb_set_default);
            this.groupBox2.Controls.Add(this.tb_max_query);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.comboBox_rfmode);
            this.groupBox2.Controls.Add(this.comboBox_sel);
            this.groupBox2.Controls.Add(this.label22);
            this.groupBox2.Controls.Add(this.cb_tagcaching);
            this.groupBox2.Controls.Add(this.tb_deinterval);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.cb_tagfocus);
            this.groupBox2.Controls.Add(this.cb_fastid);
            this.groupBox2.Controls.Add(this.cb_q_decrease_use_query);
            this.groupBox2.Controls.Add(this.cb_q_increase_use_query);
            this.groupBox2.Controls.Add(this.comboBox_region);
            this.groupBox2.Controls.Add(this.tb_min_q_cycle);
            this.groupBox2.Controls.Add(this.label27);
            this.groupBox2.Controls.Add(this.label23);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.tb_maxq);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.tb_minq);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.tb_startq);
            this.groupBox2.Controls.Add(this.tb_channel);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.tb_power);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label19);
            this.groupBox2.Controls.Add(this.btn_invset);
            this.groupBox2.Controls.Add(this.comboBox_algorithm);
            this.groupBox2.Controls.Add(this.comboBox_session);
            this.groupBox2.Controls.Add(this.comboBox_target);
            this.groupBox2.Controls.Add(this.label21);
            this.groupBox2.Controls.Add(this.label20);
            this.groupBox2.Controls.Add(this.label18);
            this.groupBox2.Location = new System.Drawing.Point(12, 294);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(944, 149);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Inventory Settings";
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(536, 131);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(178, 13);
            this.label42.TabIndex = 59;
            this.label42.Text = "* 0 for HID mode means 20 seconds";
            // 
            // btn_reset_factory
            // 
            this.btn_reset_factory.Location = new System.Drawing.Point(767, 103);
            this.btn_reset_factory.Name = "btn_reset_factory";
            this.btn_reset_factory.Size = new System.Drawing.Size(93, 29);
            this.btn_reset_factory.TabIndex = 58;
            this.btn_reset_factory.Text = "Reset Factory";
            this.btn_reset_factory.UseVisualStyleBackColor = true;
            this.btn_reset_factory.Click += new System.EventHandler(this.btn_reset_factory_Click);
            // 
            // cb_set_default
            // 
            this.cb_set_default.AutoSize = true;
            this.cb_set_default.Location = new System.Drawing.Point(758, 41);
            this.cb_set_default.Name = "cb_set_default";
            this.cb_set_default.Size = new System.Drawing.Size(145, 17);
            this.cb_set_default.TabIndex = 57;
            this.cb_set_default.Text = "Save to power up default";
            this.cb_set_default.UseVisualStyleBackColor = true;
            // 
            // tb_max_query
            // 
            this.tb_max_query.Location = new System.Drawing.Point(604, 77);
            this.tb_max_query.MaxLength = 4;
            this.tb_max_query.Name = "tb_max_query";
            this.tb_max_query.Size = new System.Drawing.Size(39, 20);
            this.tb_max_query.TabIndex = 56;
            this.tb_max_query.Text = "8";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(536, 80);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(61, 13);
            this.label10.TabIndex = 55;
            this.label10.Text = "Max Query:";
            // 
            // comboBox_rfmode
            // 
            this.comboBox_rfmode.FormattingEnabled = true;
            this.comboBox_rfmode.Items.AddRange(new object[] {
            "103",
            "302",
            "120",
            "104",
            "323",
            "203",
            "202",
            "226",
            "344",
            "345",
            "225",
            "326",
            "325",
            "324",
            "342",
            "343",
            "205",
            "382",
            "385",
            "386",
            "1120",
            "1220",
            "1320",
            "1420",
            "4323",
            "4345",
            "4324",
            "4342",
            "4343",
            "4382",
            "4385",
            "4386",
            "5323",
            "5345",
            "5324",
            "5342",
            "5343",
            "5382"});
            this.comboBox_rfmode.Location = new System.Drawing.Point(59, 77);
            this.comboBox_rfmode.Name = "comboBox_rfmode";
            this.comboBox_rfmode.Size = new System.Drawing.Size(52, 21);
            this.comboBox_rfmode.TabIndex = 54;
            this.comboBox_rfmode.Text = "302";
            // 
            // comboBox_sel
            // 
            this.comboBox_sel.FormattingEnabled = true;
            this.comboBox_sel.Items.AddRange(new object[] {
            "ALL",
            "ALL2",
            "~SL",
            "SL"});
            this.comboBox_sel.Location = new System.Drawing.Point(329, 108);
            this.comboBox_sel.Name = "comboBox_sel";
            this.comboBox_sel.Size = new System.Drawing.Size(78, 21);
            this.comboBox_sel.TabIndex = 53;
            this.comboBox_sel.Text = "ALL";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(271, 111);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(25, 13);
            this.label22.TabIndex = 52;
            this.label22.Text = "Sel:";
            // 
            // cb_tagcaching
            // 
            this.cb_tagcaching.AutoSize = true;
            this.cb_tagcaching.Location = new System.Drawing.Point(758, 18);
            this.cb_tagcaching.Name = "cb_tagcaching";
            this.cb_tagcaching.Size = new System.Drawing.Size(87, 17);
            this.cb_tagcaching.TabIndex = 51;
            this.cb_tagcaching.Text = "Tag Caching";
            this.cb_tagcaching.UseVisualStyleBackColor = true;
            // 
            // tb_deinterval
            // 
            this.tb_deinterval.Location = new System.Drawing.Point(702, 108);
            this.tb_deinterval.MaxLength = 3;
            this.tb_deinterval.Name = "tb_deinterval";
            this.tb_deinterval.Size = new System.Drawing.Size(36, 20);
            this.tb_deinterval.TabIndex = 50;
            this.tb_deinterval.Text = "0";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(536, 111);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(164, 13);
            this.label12.TabIndex = 49;
            this.label12.Text = "Duplicate Elimination Interval* (s):";
            // 
            // cb_tagfocus
            // 
            this.cb_tagfocus.AutoSize = true;
            this.cb_tagfocus.Location = new System.Drawing.Point(677, 41);
            this.cb_tagfocus.Name = "cb_tagfocus";
            this.cb_tagfocus.Size = new System.Drawing.Size(74, 17);
            this.cb_tagfocus.TabIndex = 47;
            this.cb_tagfocus.Text = "TagFocus";
            this.cb_tagfocus.UseVisualStyleBackColor = true;
            // 
            // cb_fastid
            // 
            this.cb_fastid.AutoSize = true;
            this.cb_fastid.Location = new System.Drawing.Point(677, 18);
            this.cb_fastid.Name = "cb_fastid";
            this.cb_fastid.Size = new System.Drawing.Size(55, 17);
            this.cb_fastid.TabIndex = 46;
            this.cb_fastid.Text = "FastId";
            this.cb_fastid.UseVisualStyleBackColor = true;
            // 
            // cb_q_decrease_use_query
            // 
            this.cb_q_decrease_use_query.AutoSize = true;
            this.cb_q_decrease_use_query.Checked = true;
            this.cb_q_decrease_use_query.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_q_decrease_use_query.Location = new System.Drawing.Point(538, 41);
            this.cb_q_decrease_use_query.Name = "cb_q_decrease_use_query";
            this.cb_q_decrease_use_query.Size = new System.Drawing.Size(127, 17);
            this.cb_q_decrease_use_query.TabIndex = 45;
            this.cb_q_decrease_use_query.Text = "QDecreaseUseQuery";
            this.cb_q_decrease_use_query.UseVisualStyleBackColor = true;
            // 
            // cb_q_increase_use_query
            // 
            this.cb_q_increase_use_query.AutoSize = true;
            this.cb_q_increase_use_query.Checked = true;
            this.cb_q_increase_use_query.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_q_increase_use_query.Location = new System.Drawing.Point(538, 18);
            this.cb_q_increase_use_query.Name = "cb_q_increase_use_query";
            this.cb_q_increase_use_query.Size = new System.Drawing.Size(122, 17);
            this.cb_q_increase_use_query.TabIndex = 44;
            this.cb_q_increase_use_query.Text = "QIncreaseUseQuery";
            this.cb_q_increase_use_query.UseVisualStyleBackColor = true;
            // 
            // comboBox_region
            // 
            this.comboBox_region.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_region.FormattingEnabled = true;
            this.comboBox_region.Items.AddRange(new object[] {
            "0 - Factory Default",
            "1 - Albania1",
            "2 - Albania2",
            "3 - Algeria1",
            "4 - Algeria2",
            "5 - Algeria3",
            "6 - Algeria4",
            "7 - Argentina",
            "8 - Armenia",
            "9 - Australia1",
            "10 - Australia2",
            "11 - Austria1",
            "12 - Austria1",
            "13 - Azerbaijan",
            "14 - Bahrain",
            "15 - Bangladesh",
            "16 - Belarus",
            "17 - Belgium1",
            "18 - Belgium2",
            "19 - Bolivia",
            "20 - Bosnia",
            "21 - Botswana",
            "22 - Brazil1",
            "23 - Brazil2",
            "24 - Brunei1",
            "25 - Brunei2",
            "26 - Bulgaria1",
            "27 - Bulgaria2",
            "28 - Cambodia",
            "29 - Cameroon",
            "30 - Canada",
            "31 - Chile1",
            "32 - Chile2",
            "33 - Chile3",
            "34 - China",
            "35 - Colombia",
            "36 - Congo",
            "37 - CostaRica",
            "38 - Cotedlvoire",
            "39 - Croatia",
            "40 - Cuba",
            "41 - Cyprus1",
            "42 - Cyprus2",
            "43 - Czech1",
            "44 - Czech2",
            "45 - Denmark1",
            "46 - Denmark2",
            "47 - Dominican",
            "48 - Ecuador",
            "49 - Egypt",
            "50 - ElSalvador",
            "51 - Estonia",
            "52 - Finland1",
            "53 - Finland2",
            "54 - France",
            "55 - Georgia",
            "56 - Germany",
            "57 - Ghana",
            "58 - Greece",
            "59 - Guatemala",
            "60 - HongKong1",
            "61 - HongKong2",
            "62 - Hungary1",
            "63 - Hungary2",
            "64 - Iceland",
            "65 - India",
            "66 - Indonesia",
            "67 - Iran",
            "68 - Ireland1",
            "69 - Ireland2",
            "70 - Israel",
            "71 - Italy",
            "72 - Jamaica",
            "73 - Japan4",
            "74 - Japan6",
            "75 - Jordan",
            "76 - Kazakhstan",
            "77 - Kenya",
            "78 - Korea",
            "79 - KoreaDPR",
            "80 - Kuwait",
            "81 - Kyrgyz",
            "82 - Latia",
            "83 - Lebanon",
            "84 - Libya",
            "85 - Liechtenstein1",
            "86 - Liechtenstein2",
            "87 - Lithuania1",
            "88 - Lithuania2",
            "89 - Luxembourg1",
            "90 - Luxembourg2",
            "91 - Macao",
            "92 - Macedonia",
            "93 - Malaysia",
            "94 - Malta1",
            "95 - Malta2",
            "96 - Mauritius",
            "97 - Mexico",
            "98 - Moldova1",
            "99 - Moldova2",
            "100 - Mongolia",
            "101 - Montenegro",
            "102 - Morocco",
            "103 - Netherlands",
            "104 - NewZealand1",
            "105 - NewZealand2",
            "106 - Nicaragua",
            "107 - Nigeria",
            "108 - Norway1",
            "109 - Norway2",
            "110 - Oman",
            "111 - Pakistan",
            "112 - Panama",
            "113 - Paraguay",
            "114 - Peru",
            "115 - Philippines",
            "116 - Poland",
            "117 - Portugal",
            "118 - Romania",
            "119 - Russia1",
            "120 - Russia3",
            "121 - Senegal",
            "122 - Serbia",
            "123 - Singapore1",
            "124 - Singapore2",
            "125 - Slovak1",
            "126 - Slovak2",
            "127 - Slovenia1",
            "128 - Solvenia2",
            "129 - SAfrica1",
            "130 - SAfrica2",
            "131 - Spain",
            "132 - SriLanka",
            "133 - Sudan",
            "134 - Sweden1",
            "135 - Sweden2",
            "136 - Switzerland1",
            "137 - Switzerland2",
            "138 - Syria",
            "139 - Taiwan1",
            "140 - Taiwan2",
            "141 - Tajikistan",
            "142 - Tanzania",
            "143 - Thailand",
            "144 - Trinidad",
            "145 - Tunisia",
            "146 - Turkey",
            "147 - Turkmenistan",
            "148 - Uganda",
            "149 - Ukraine",
            "150 - UAE",
            "151 - UK1",
            "152 - UK2",
            "153 - USA",
            "154 - Uruguay",
            "155 - Venezuela",
            "156 - Vietnam1",
            "157 - Vietnam2",
            "158 - Yemen",
            "159 - Zimbabwe"});
            this.comboBox_region.Location = new System.Drawing.Point(59, 107);
            this.comboBox_region.Name = "comboBox_region";
            this.comboBox_region.Size = new System.Drawing.Size(152, 21);
            this.comboBox_region.TabIndex = 26;
            // 
            // tb_min_q_cycle
            // 
            this.tb_min_q_cycle.Location = new System.Drawing.Point(488, 107);
            this.tb_min_q_cycle.MaxLength = 2;
            this.tb_min_q_cycle.Name = "tb_min_q_cycle";
            this.tb_min_q_cycle.Size = new System.Drawing.Size(39, 20);
            this.tb_min_q_cycle.TabIndex = 42;
            this.tb_min_q_cycle.Text = "3";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(6, 111);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(44, 13);
            this.label27.TabIndex = 25;
            this.label27.Text = "Region:";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(420, 110);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(66, 13);
            this.label23.TabIndex = 41;
            this.label23.Text = "MinQCycles:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(104, 20);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(46, 13);
            this.label9.TabIndex = 24;
            this.label9.Text = "0 - 3200";
            // 
            // tb_maxq
            // 
            this.tb_maxq.Location = new System.Drawing.Point(488, 77);
            this.tb_maxq.MaxLength = 2;
            this.tb_maxq.Name = "tb_maxq";
            this.tb_maxq.Size = new System.Drawing.Size(39, 20);
            this.tb_maxq.TabIndex = 39;
            this.tb_maxq.Text = "15";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 80);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "RF Mode:";
            // 
            // tb_minq
            // 
            this.tb_minq.Location = new System.Drawing.Point(488, 47);
            this.tb_minq.MaxLength = 2;
            this.tb_minq.Name = "tb_minq";
            this.tb_minq.Size = new System.Drawing.Size(39, 20);
            this.tb_minq.TabIndex = 38;
            this.tb_minq.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(104, 50);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(165, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "0 = hopping; 1-50 = fixed channel";
            // 
            // tb_startq
            // 
            this.tb_startq.Location = new System.Drawing.Point(488, 16);
            this.tb_startq.MaxLength = 2;
            this.tb_startq.Name = "tb_startq";
            this.tb_startq.Size = new System.Drawing.Size(39, 20);
            this.tb_startq.TabIndex = 25;
            this.tb_startq.Text = "7";
            // 
            // tb_channel
            // 
            this.tb_channel.Location = new System.Drawing.Point(59, 47);
            this.tb_channel.MaxLength = 2;
            this.tb_channel.Name = "tb_channel";
            this.tb_channel.Size = new System.Drawing.Size(39, 20);
            this.tb_channel.TabIndex = 19;
            this.tb_channel.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Channel:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(420, 80);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(41, 13);
            this.label15.TabIndex = 33;
            this.label15.Text = "Max Q:";
            // 
            // tb_power
            // 
            this.tb_power.Location = new System.Drawing.Point(59, 16);
            this.tb_power.MaxLength = 4;
            this.tb_power.Name = "tb_power";
            this.tb_power.Size = new System.Drawing.Size(39, 20);
            this.tb_power.TabIndex = 1;
            this.tb_power.Text = "3000";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(420, 50);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(38, 13);
            this.label17.TabIndex = 32;
            this.label17.Text = "Min Q:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Power:";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(420, 20);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(43, 13);
            this.label19.TabIndex = 31;
            this.label19.Text = "Start Q:";
            // 
            // btn_invset
            // 
            this.btn_invset.Location = new System.Drawing.Point(866, 103);
            this.btn_invset.Name = "btn_invset";
            this.btn_invset.Size = new System.Drawing.Size(65, 29);
            this.btn_invset.TabIndex = 25;
            this.btn_invset.Text = "Set";
            this.btn_invset.UseVisualStyleBackColor = true;
            this.btn_invset.Click += new System.EventHandler(this.btn_invset_Click);
            // 
            // comboBox_algorithm
            // 
            this.comboBox_algorithm.FormattingEnabled = true;
            this.comboBox_algorithm.Items.AddRange(new object[] {
            "Fixed Q",
            "Dynamic Q"});
            this.comboBox_algorithm.Location = new System.Drawing.Point(329, 78);
            this.comboBox_algorithm.Name = "comboBox_algorithm";
            this.comboBox_algorithm.Size = new System.Drawing.Size(78, 21);
            this.comboBox_algorithm.TabIndex = 30;
            this.comboBox_algorithm.Text = "Fixed Q";
            // 
            // comboBox_session
            // 
            this.comboBox_session.FormattingEnabled = true;
            this.comboBox_session.Items.AddRange(new object[] {
            "S0",
            "S1",
            "S2",
            "S3"});
            this.comboBox_session.Location = new System.Drawing.Point(329, 17);
            this.comboBox_session.Name = "comboBox_session";
            this.comboBox_session.Size = new System.Drawing.Size(78, 21);
            this.comboBox_session.TabIndex = 28;
            this.comboBox_session.Text = "S0";
            // 
            // comboBox_target
            // 
            this.comboBox_target.FormattingEnabled = true;
            this.comboBox_target.Items.AddRange(new object[] {
            "Toggle",
            "A",
            "B"});
            this.comboBox_target.Location = new System.Drawing.Point(329, 48);
            this.comboBox_target.Name = "comboBox_target";
            this.comboBox_target.Size = new System.Drawing.Size(78, 21);
            this.comboBox_target.TabIndex = 29;
            this.comboBox_target.Text = "Toggle";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(271, 21);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(47, 13);
            this.label21.TabIndex = 0;
            this.label21.Text = "Session:";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(271, 51);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(41, 13);
            this.label20.TabIndex = 18;
            this.label20.Text = "Target:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(271, 81);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(53, 13);
            this.label18.TabIndex = 21;
            this.label18.Text = "Algorithm:";
            // 
            // btn_clear_info
            // 
            this.btn_clear_info.Location = new System.Drawing.Point(763, 729);
            this.btn_clear_info.Name = "btn_clear_info";
            this.btn_clear_info.Size = new System.Drawing.Size(105, 29);
            this.btn_clear_info.TabIndex = 18;
            this.btn_clear_info.Text = "Clear";
            this.btn_clear_info.UseVisualStyleBackColor = true;
            this.btn_clear_info.Click += new System.EventHandler(this.btn_clear_info_Click);
            // 
            // timer_rate
            // 
            this.timer_rate.Enabled = true;
            this.timer_rate.Interval = 1000;
            this.timer_rate.Tick += new System.EventHandler(this.readrate_Tick);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btn_write_oem);
            this.groupBox3.Controls.Add(this.btn_read_oem);
            this.groupBox3.Controls.Add(this.btn_write_register);
            this.groupBox3.Controls.Add(this.tb_register_value);
            this.groupBox3.Controls.Add(this.label37);
            this.groupBox3.Controls.Add(this.btn_read_register);
            this.groupBox3.Controls.Add(this.tb_length);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.tb_address);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Location = new System.Drawing.Point(12, 609);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(944, 55);
            this.groupBox3.TabIndex = 19;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Test Settings";
            // 
            // btn_write_oem
            // 
            this.btn_write_oem.Location = new System.Drawing.Point(839, 19);
            this.btn_write_oem.Name = "btn_write_oem";
            this.btn_write_oem.Size = new System.Drawing.Size(92, 29);
            this.btn_write_oem.TabIndex = 68;
            this.btn_write_oem.Text = "Restore OEM";
            this.btn_write_oem.UseVisualStyleBackColor = true;
            this.btn_write_oem.Click += new System.EventHandler(this.btn_write_oem_Click);
            // 
            // btn_read_oem
            // 
            this.btn_read_oem.Location = new System.Drawing.Point(741, 19);
            this.btn_read_oem.Name = "btn_read_oem";
            this.btn_read_oem.Size = new System.Drawing.Size(92, 29);
            this.btn_read_oem.TabIndex = 67;
            this.btn_read_oem.Text = "Save OEM";
            this.btn_read_oem.UseVisualStyleBackColor = true;
            this.btn_read_oem.Click += new System.EventHandler(this.btn_read_oem_Click);
            // 
            // btn_write_register
            // 
            this.btn_write_register.Location = new System.Drawing.Point(526, 20);
            this.btn_write_register.Name = "btn_write_register";
            this.btn_write_register.Size = new System.Drawing.Size(92, 29);
            this.btn_write_register.TabIndex = 66;
            this.btn_write_register.Text = "Write Register";
            this.btn_write_register.UseVisualStyleBackColor = true;
            this.btn_write_register.Click += new System.EventHandler(this.btn_write_register_Click);
            // 
            // tb_register_value
            // 
            this.tb_register_value.Location = new System.Drawing.Point(263, 25);
            this.tb_register_value.MaxLength = 32;
            this.tb_register_value.Name = "tb_register_value";
            this.tb_register_value.Size = new System.Drawing.Size(152, 20);
            this.tb_register_value.TabIndex = 65;
            this.tb_register_value.Text = "00";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(209, 28);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(51, 13);
            this.label37.TabIndex = 64;
            this.label37.Text = "Value: 0x";
            // 
            // btn_read_register
            // 
            this.btn_read_register.Location = new System.Drawing.Point(428, 20);
            this.btn_read_register.Name = "btn_read_register";
            this.btn_read_register.Size = new System.Drawing.Size(92, 29);
            this.btn_read_register.TabIndex = 52;
            this.btn_read_register.Text = "Read Register";
            this.btn_read_register.UseVisualStyleBackColor = true;
            this.btn_read_register.Click += new System.EventHandler(this.btn_read_register_Click);
            // 
            // tb_length
            // 
            this.tb_length.Location = new System.Drawing.Point(163, 25);
            this.tb_length.MaxLength = 2;
            this.tb_length.Name = "tb_length";
            this.tb_length.Size = new System.Drawing.Size(40, 20);
            this.tb_length.TabIndex = 51;
            this.tb_length.Text = "1";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(117, 28);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(43, 13);
            this.label8.TabIndex = 50;
            this.label8.Text = "Length:";
            // 
            // tb_address
            // 
            this.tb_address.Location = new System.Drawing.Point(72, 25);
            this.tb_address.MaxLength = 4;
            this.tb_address.Name = "tb_address";
            this.tb_address.Size = new System.Drawing.Size(39, 20);
            this.tb_address.TabIndex = 49;
            this.tb_address.Text = "0000";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 48;
            this.label2.Text = "Address: 0x";
            // 
            // lb_elapsed
            // 
            this.lb_elapsed.AutoSize = true;
            this.lb_elapsed.Location = new System.Drawing.Point(813, 678);
            this.lb_elapsed.Name = "lb_elapsed";
            this.lb_elapsed.Size = new System.Drawing.Size(13, 13);
            this.lb_elapsed.TabIndex = 25;
            this.lb_elapsed.Text = "0";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(760, 678);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(51, 13);
            this.label16.TabIndex = 24;
            this.label16.Text = "Elapsed: ";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(353, 664);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(40, 13);
            this.label24.TabIndex = 30;
            this.label24.Text = "Status:";
            // 
            // tb_status
            // 
            this.tb_status.Location = new System.Drawing.Point(353, 681);
            this.tb_status.Multiline = true;
            this.tb_status.Name = "tb_status";
            this.tb_status.ReadOnly = true;
            this.tb_status.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_status.Size = new System.Drawing.Size(401, 77);
            this.tb_status.TabIndex = 29;
            // 
            // timer_reset
            // 
            this.timer_reset.Enabled = true;
            this.timer_reset.Interval = 4000;
            this.timer_reset.Tick += new System.EventHandler(this.timer_reset_Tick);
            // 
            // btn_reset
            // 
            this.btn_reset.Location = new System.Drawing.Point(762, 694);
            this.btn_reset.Name = "btn_reset";
            this.btn_reset.Size = new System.Drawing.Size(105, 29);
            this.btn_reset.TabIndex = 31;
            this.btn_reset.Text = "Reset Reader";
            this.btn_reset.UseVisualStyleBackColor = true;
            this.btn_reset.Click += new System.EventHandler(this.btn_reset_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cb_selenable);
            this.groupBox4.Controls.Add(this.btn_selset);
            this.groupBox4.Controls.Add(this.comboBox_action);
            this.groupBox4.Controls.Add(this.label30);
            this.groupBox4.Controls.Add(this.comboBox_selTarget);
            this.groupBox4.Controls.Add(this.label29);
            this.groupBox4.Controls.Add(this.tb_selLength);
            this.groupBox4.Controls.Add(this.label28);
            this.groupBox4.Controls.Add(this.tb_offset);
            this.groupBox4.Controls.Add(this.label26);
            this.groupBox4.Controls.Add(this.comboBox_bank);
            this.groupBox4.Controls.Add(this.label25);
            this.groupBox4.Controls.Add(this.tb_mask);
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Location = new System.Drawing.Point(12, 449);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(944, 52);
            this.groupBox4.TabIndex = 32;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Select Settings";
            // 
            // cb_selenable
            // 
            this.cb_selenable.AutoSize = true;
            this.cb_selenable.Location = new System.Drawing.Point(666, 18);
            this.cb_selenable.Name = "cb_selenable";
            this.cb_selenable.Size = new System.Drawing.Size(59, 17);
            this.cb_selenable.TabIndex = 54;
            this.cb_selenable.Text = "Enable";
            this.cb_selenable.UseVisualStyleBackColor = true;
            // 
            // btn_selset
            // 
            this.btn_selset.Location = new System.Drawing.Point(866, 12);
            this.btn_selset.Name = "btn_selset";
            this.btn_selset.Size = new System.Drawing.Size(65, 29);
            this.btn_selset.TabIndex = 54;
            this.btn_selset.Text = "Set";
            this.btn_selset.UseVisualStyleBackColor = true;
            this.btn_selset.Click += new System.EventHandler(this.btn_selset_Click);
            // 
            // comboBox_action
            // 
            this.comboBox_action.FormattingEnabled = true;
            this.comboBox_action.Items.AddRange(new object[] {
            "000",
            "001",
            "010",
            "011",
            "100",
            "101",
            "110",
            "111"});
            this.comboBox_action.Location = new System.Drawing.Point(609, 16);
            this.comboBox_action.Name = "comboBox_action";
            this.comboBox_action.Size = new System.Drawing.Size(46, 21);
            this.comboBox_action.TabIndex = 61;
            this.comboBox_action.Text = "000";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(571, 19);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(40, 13);
            this.label30.TabIndex = 62;
            this.label30.Text = "Action:";
            // 
            // comboBox_selTarget
            // 
            this.comboBox_selTarget.FormattingEnabled = true;
            this.comboBox_selTarget.Items.AddRange(new object[] {
            "S0",
            "S1",
            "S2",
            "S3",
            "SL"});
            this.comboBox_selTarget.Location = new System.Drawing.Point(530, 16);
            this.comboBox_selTarget.Name = "comboBox_selTarget";
            this.comboBox_selTarget.Size = new System.Drawing.Size(38, 21);
            this.comboBox_selTarget.TabIndex = 59;
            this.comboBox_selTarget.Text = "SL";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(492, 19);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(41, 13);
            this.label29.TabIndex = 60;
            this.label29.Text = "Target:";
            // 
            // tb_selLength
            // 
            this.tb_selLength.Location = new System.Drawing.Point(453, 17);
            this.tb_selLength.MaxLength = 3;
            this.tb_selLength.Name = "tb_selLength";
            this.tb_selLength.Size = new System.Drawing.Size(34, 20);
            this.tb_selLength.TabIndex = 58;
            this.tb_selLength.Text = "16";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(413, 20);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(43, 13);
            this.label28.TabIndex = 57;
            this.label28.Text = "Length:";
            // 
            // tb_offset
            // 
            this.tb_offset.Location = new System.Drawing.Point(360, 17);
            this.tb_offset.MaxLength = 6;
            this.tb_offset.Name = "tb_offset";
            this.tb_offset.Size = new System.Drawing.Size(51, 20);
            this.tb_offset.TabIndex = 56;
            this.tb_offset.Text = "32";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(321, 20);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(38, 13);
            this.label26.TabIndex = 55;
            this.label26.Text = "Offset:";
            // 
            // comboBox_bank
            // 
            this.comboBox_bank.FormattingEnabled = true;
            this.comboBox_bank.Items.AddRange(new object[] {
            "Reserved",
            "EPC",
            "TID",
            "User"});
            this.comboBox_bank.Location = new System.Drawing.Point(257, 17);
            this.comboBox_bank.Name = "comboBox_bank";
            this.comboBox_bank.Size = new System.Drawing.Size(55, 21);
            this.comboBox_bank.TabIndex = 54;
            this.comboBox_bank.Text = "EPC";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(219, 20);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(35, 13);
            this.label25.TabIndex = 54;
            this.label25.Text = "Bank:";
            // 
            // tb_mask
            // 
            this.tb_mask.Location = new System.Drawing.Point(59, 17);
            this.tb_mask.MaxLength = 64;
            this.tb_mask.Name = "tb_mask";
            this.tb_mask.Size = new System.Drawing.Size(152, 20);
            this.tb_mask.TabIndex = 53;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(5, 20);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(50, 13);
            this.label14.TabIndex = 52;
            this.label14.Text = "Mask: 0x";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.cb_bank2_enable);
            this.groupBox5.Controls.Add(this.tb_bank2_length);
            this.groupBox5.Controls.Add(this.label31);
            this.groupBox5.Controls.Add(this.tb_bank2_offset);
            this.groupBox5.Controls.Add(this.label32);
            this.groupBox5.Controls.Add(this.comboBox_bank2);
            this.groupBox5.Controls.Add(this.label36);
            this.groupBox5.Controls.Add(this.cb_bank1_enable);
            this.groupBox5.Controls.Add(this.btn_mbset);
            this.groupBox5.Controls.Add(this.tb_bank1_length);
            this.groupBox5.Controls.Add(this.label33);
            this.groupBox5.Controls.Add(this.tb_bank1_offset);
            this.groupBox5.Controls.Add(this.label34);
            this.groupBox5.Controls.Add(this.comboBox_bank1);
            this.groupBox5.Controls.Add(this.label35);
            this.groupBox5.Location = new System.Drawing.Point(12, 503);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(944, 52);
            this.groupBox5.TabIndex = 63;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Multiple Bank Settings";
            // 
            // cb_bank2_enable
            // 
            this.cb_bank2_enable.AutoSize = true;
            this.cb_bank2_enable.Location = new System.Drawing.Point(664, 19);
            this.cb_bank2_enable.Name = "cb_bank2_enable";
            this.cb_bank2_enable.Size = new System.Drawing.Size(65, 17);
            this.cb_bank2_enable.TabIndex = 59;
            this.cb_bank2_enable.Text = "Enable2";
            this.cb_bank2_enable.UseVisualStyleBackColor = true;
            // 
            // tb_bank2_length
            // 
            this.tb_bank2_length.Location = new System.Drawing.Point(621, 17);
            this.tb_bank2_length.MaxLength = 3;
            this.tb_bank2_length.Name = "tb_bank2_length";
            this.tb_bank2_length.Size = new System.Drawing.Size(34, 20);
            this.tb_bank2_length.TabIndex = 65;
            this.tb_bank2_length.Text = "2";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(573, 20);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(49, 13);
            this.label31.TabIndex = 64;
            this.label31.Text = "Length2:";
            // 
            // tb_bank2_offset
            // 
            this.tb_bank2_offset.Location = new System.Drawing.Point(520, 17);
            this.tb_bank2_offset.MaxLength = 6;
            this.tb_bank2_offset.Name = "tb_bank2_offset";
            this.tb_bank2_offset.Size = new System.Drawing.Size(51, 20);
            this.tb_bank2_offset.TabIndex = 63;
            this.tb_bank2_offset.Text = "0";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(476, 20);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(44, 13);
            this.label32.TabIndex = 62;
            this.label32.Text = "Offset2:";
            // 
            // comboBox_bank2
            // 
            this.comboBox_bank2.FormattingEnabled = true;
            this.comboBox_bank2.Items.AddRange(new object[] {
            "Reserved",
            "EPC",
            "TID",
            "User"});
            this.comboBox_bank2.Location = new System.Drawing.Point(413, 17);
            this.comboBox_bank2.Name = "comboBox_bank2";
            this.comboBox_bank2.Size = new System.Drawing.Size(55, 21);
            this.comboBox_bank2.TabIndex = 60;
            this.comboBox_bank2.Text = "User";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(374, 20);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(41, 13);
            this.label36.TabIndex = 61;
            this.label36.Text = "Bank2:";
            // 
            // cb_bank1_enable
            // 
            this.cb_bank1_enable.AutoSize = true;
            this.cb_bank1_enable.Location = new System.Drawing.Point(300, 19);
            this.cb_bank1_enable.Name = "cb_bank1_enable";
            this.cb_bank1_enable.Size = new System.Drawing.Size(65, 17);
            this.cb_bank1_enable.TabIndex = 54;
            this.cb_bank1_enable.Text = "Enable1";
            this.cb_bank1_enable.UseVisualStyleBackColor = true;
            // 
            // btn_mbset
            // 
            this.btn_mbset.Location = new System.Drawing.Point(866, 12);
            this.btn_mbset.Name = "btn_mbset";
            this.btn_mbset.Size = new System.Drawing.Size(65, 29);
            this.btn_mbset.TabIndex = 54;
            this.btn_mbset.Text = "Set";
            this.btn_mbset.UseVisualStyleBackColor = true;
            this.btn_mbset.Click += new System.EventHandler(this.btn_mbset_Click);
            // 
            // tb_bank1_length
            // 
            this.tb_bank1_length.Location = new System.Drawing.Point(257, 17);
            this.tb_bank1_length.MaxLength = 3;
            this.tb_bank1_length.Name = "tb_bank1_length";
            this.tb_bank1_length.Size = new System.Drawing.Size(34, 20);
            this.tb_bank1_length.TabIndex = 58;
            this.tb_bank1_length.Text = "2";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(209, 20);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(49, 13);
            this.label33.TabIndex = 57;
            this.label33.Text = "Length1:";
            // 
            // tb_bank1_offset
            // 
            this.tb_bank1_offset.Location = new System.Drawing.Point(156, 17);
            this.tb_bank1_offset.MaxLength = 6;
            this.tb_bank1_offset.Name = "tb_bank1_offset";
            this.tb_bank1_offset.Size = new System.Drawing.Size(51, 20);
            this.tb_bank1_offset.TabIndex = 56;
            this.tb_bank1_offset.Text = "0";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(112, 20);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(44, 13);
            this.label34.TabIndex = 55;
            this.label34.Text = "Offset1:";
            // 
            // comboBox_bank1
            // 
            this.comboBox_bank1.FormattingEnabled = true;
            this.comboBox_bank1.Items.AddRange(new object[] {
            "Reserved",
            "EPC",
            "TID",
            "User"});
            this.comboBox_bank1.Location = new System.Drawing.Point(49, 17);
            this.comboBox_bank1.Name = "comboBox_bank1";
            this.comboBox_bank1.Size = new System.Drawing.Size(55, 21);
            this.comboBox_bank1.TabIndex = 54;
            this.comboBox_bank1.Text = "TID";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(10, 20);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(41, 13);
            this.label35.TabIndex = 54;
            this.label35.Text = "Bank1:";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.comboBox_rx_mixer);
            this.groupBox6.Controls.Add(this.label41);
            this.groupBox6.Controls.Add(this.comboBox_rx_pga3);
            this.groupBox6.Controls.Add(this.label40);
            this.groupBox6.Controls.Add(this.comboBox_rx_pga2);
            this.groupBox6.Controls.Add(this.label39);
            this.groupBox6.Controls.Add(this.comboBox_rx_pga1);
            this.groupBox6.Controls.Add(this.label38);
            this.groupBox6.Controls.Add(this.btn_rxset);
            this.groupBox6.Controls.Add(this.comboBox_rx_atten);
            this.groupBox6.Controls.Add(this.label43);
            this.groupBox6.Location = new System.Drawing.Point(11, 556);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(945, 52);
            this.groupBox6.TabIndex = 66;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Rx Settings";
            // 
            // comboBox_rx_mixer
            // 
            this.comboBox_rx_mixer.FormattingEnabled = true;
            this.comboBox_rx_mixer.Items.AddRange(new object[] {
            "1.6 dB",
            "11.2 dB",
            "17.2 dB",
            "20.7 dB"});
            this.comboBox_rx_mixer.Location = new System.Drawing.Point(483, 17);
            this.comboBox_rx_mixer.Name = "comboBox_rx_mixer";
            this.comboBox_rx_mixer.Size = new System.Drawing.Size(70, 21);
            this.comboBox_rx_mixer.TabIndex = 61;
            this.comboBox_rx_mixer.Text = "11.2 dB";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(441, 20);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(35, 13);
            this.label41.TabIndex = 62;
            this.label41.Text = "Mixer:";
            // 
            // comboBox_rx_pga3
            // 
            this.comboBox_rx_pga3.FormattingEnabled = true;
            this.comboBox_rx_pga3.Items.AddRange(new object[] {
            "0 dB",
            "6 dB",
            "12 dB",
            "18 dB"});
            this.comboBox_rx_pga3.Location = new System.Drawing.Point(378, 17);
            this.comboBox_rx_pga3.Name = "comboBox_rx_pga3";
            this.comboBox_rx_pga3.Size = new System.Drawing.Size(55, 21);
            this.comboBox_rx_pga3.TabIndex = 59;
            this.comboBox_rx_pga3.Text = "18 dB";
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(338, 20);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(38, 13);
            this.label40.TabIndex = 60;
            this.label40.Text = "PGA3:";
            // 
            // comboBox_rx_pga2
            // 
            this.comboBox_rx_pga2.FormattingEnabled = true;
            this.comboBox_rx_pga2.Items.AddRange(new object[] {
            "0 dB",
            "6 dB",
            "12 dB",
            "18 dB"});
            this.comboBox_rx_pga2.Location = new System.Drawing.Point(274, 17);
            this.comboBox_rx_pga2.Name = "comboBox_rx_pga2";
            this.comboBox_rx_pga2.Size = new System.Drawing.Size(55, 21);
            this.comboBox_rx_pga2.TabIndex = 57;
            this.comboBox_rx_pga2.Text = "0 dB";
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(234, 20);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(38, 13);
            this.label39.TabIndex = 58;
            this.label39.Text = "PGA2:";
            // 
            // comboBox_rx_pga1
            // 
            this.comboBox_rx_pga1.FormattingEnabled = true;
            this.comboBox_rx_pga1.Items.AddRange(new object[] {
            "-6 dB",
            "0 dB",
            "6 dB",
            "12 dB"});
            this.comboBox_rx_pga1.Location = new System.Drawing.Point(171, 17);
            this.comboBox_rx_pga1.Name = "comboBox_rx_pga1";
            this.comboBox_rx_pga1.Size = new System.Drawing.Size(55, 21);
            this.comboBox_rx_pga1.TabIndex = 55;
            this.comboBox_rx_pga1.Text = "12 dB";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(132, 20);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(38, 13);
            this.label38.TabIndex = 56;
            this.label38.Text = "PGA1:";
            // 
            // btn_rxset
            // 
            this.btn_rxset.Location = new System.Drawing.Point(866, 12);
            this.btn_rxset.Name = "btn_rxset";
            this.btn_rxset.Size = new System.Drawing.Size(65, 29);
            this.btn_rxset.TabIndex = 54;
            this.btn_rxset.Text = "Set";
            this.btn_rxset.UseVisualStyleBackColor = true;
            this.btn_rxset.Click += new System.EventHandler(this.btn_rxset_Click);
            // 
            // comboBox_rx_atten
            // 
            this.comboBox_rx_atten.FormattingEnabled = true;
            this.comboBox_rx_atten.Items.AddRange(new object[] {
            "0 dB",
            "6 dB",
            "12 dB",
            "18 dB"});
            this.comboBox_rx_atten.Location = new System.Drawing.Point(71, 17);
            this.comboBox_rx_atten.Name = "comboBox_rx_atten";
            this.comboBox_rx_atten.Size = new System.Drawing.Size(55, 21);
            this.comboBox_rx_atten.TabIndex = 54;
            this.comboBox_rx_atten.Text = "0 dB";
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(10, 20);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(59, 13);
            this.label43.TabIndex = 54;
            this.label43.Text = "Attenuator:";
            // 
            // RFIDForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(968, 764);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.btn_reset);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.tb_status);
            this.Controls.Add(this.lb_elapsed);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.btn_clear_info);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_info);
            this.Controls.Add(this.groupBox1);
            this.Name = "RFIDForm";
            this.Text = "RFID";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_rfid_clear;
        private System.Windows.Forms.Button btn_rfid_poweroff;
        private System.Windows.Forms.Button btn_rfid_poweron;
        private System.Windows.Forms.TextBox tb_info;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_inventory;
        private System.Windows.Forms.Button btn_stop_inventory;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tb_power;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_channel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        //private System.Windows.Forms.ListView lv_tag;
        private TListView lv_tag;
        private System.Windows.Forms.ColumnHeader col_index;
        private System.Windows.Forms.ColumnHeader col_pc;
        private System.Windows.Forms.ColumnHeader col_epc;
        private System.Windows.Forms.ColumnHeader col_rssi;
        private System.Windows.Forms.ColumnHeader col_count;
        private System.Windows.Forms.Button btn_clear_info;
        private System.Windows.Forms.TextBox tb_version;
        private System.Windows.Forms.Button btn_version;
        private System.Windows.Forms.Timer timer_rate;
        private System.Windows.Forms.Label lb_rate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lb_total;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lb_elapsed;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button btn_read;
        private System.Windows.Forms.CheckBox cb_reconnect;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox tb_status;
        private System.Windows.Forms.ComboBox comboBox_region;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Timer timer_reset;
        private System.Windows.Forms.Button btn_reset;
        private System.Windows.Forms.ColumnHeader col_phasebegin;
        private System.Windows.Forms.TextBox tb_min_q_cycle;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox tb_maxq;
        private System.Windows.Forms.TextBox tb_minq;
        private System.Windows.Forms.TextBox tb_startq;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Button btn_invset;
        private System.Windows.Forms.ComboBox comboBox_algorithm;
        private System.Windows.Forms.ComboBox comboBox_session;
        private System.Windows.Forms.ComboBox comboBox_target;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.CheckBox cb_tagfocus;
        private System.Windows.Forms.CheckBox cb_fastid;
        private System.Windows.Forms.CheckBox cb_q_decrease_use_query;
        private System.Windows.Forms.CheckBox cb_q_increase_use_query;
        private System.Windows.Forms.Button btn_read_register;
        private System.Windows.Forms.TextBox tb_length;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tb_address;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_deinterval;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox cb_tagcaching;
        private System.Windows.Forms.ColumnHeader col_phaseend;
        private System.Windows.Forms.ColumnHeader col_timestamp;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox tb_mask;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox comboBox_sel;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.ComboBox comboBox_bank;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TextBox tb_offset;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox tb_selLength;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.ComboBox comboBox_action;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.ComboBox comboBox_selTarget;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Button btn_selset;
        private System.Windows.Forms.CheckBox cb_selenable;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox cb_bank2_enable;
        private System.Windows.Forms.TextBox tb_bank2_length;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.TextBox tb_bank2_offset;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.ComboBox comboBox_bank2;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.CheckBox cb_bank1_enable;
        private System.Windows.Forms.Button btn_mbset;
        private System.Windows.Forms.TextBox tb_bank1_length;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.TextBox tb_bank1_offset;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.ComboBox comboBox_bank1;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.ColumnHeader col_mb;
        private System.Windows.Forms.CheckBox cb_compact_mode;
        private System.Windows.Forms.ComboBox comboBox_rfmode;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btn_write_register;
        private System.Windows.Forms.TextBox tb_register_value;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btn_rxset;
        private System.Windows.Forms.ComboBox comboBox_rx_atten;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.ComboBox comboBox_rx_mixer;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.ComboBox comboBox_rx_pga3;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.ComboBox comboBox_rx_pga2;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.ComboBox comboBox_rx_pga1;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Button btn_write_oem;
        private System.Windows.Forms.Button btn_read_oem;
        private System.Windows.Forms.CheckBox cb_set_default;
        private System.Windows.Forms.Button btn_reset_factory;
        private System.Windows.Forms.TextBox tb_max_query;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.ColumnHeader col_xpc1;
        private System.Windows.Forms.ColumnHeader col_xpc2;
    }
}


namespace CS710_PC_Client
{
    partial class BtHidConfigForm
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
            this.tb_status = new System.Windows.Forms.TextBox();
            this.cb_bt_mode = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_set = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rb_raw = new System.Windows.Forms.RadioButton();
            this.rb_tlv = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_prefix2 = new System.Windows.Forms.ComboBox();
            this.cb_prefix2_enable_shift = new System.Windows.Forms.CheckBox();
            this.cb_suffix1_enable_shift = new System.Windows.Forms.CheckBox();
            this.comboBox_suffix1 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cb_prefix1_enable_shift = new System.Windows.Forms.CheckBox();
            this.comboBox_prefix1 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cb_suffix2_enable_shift = new System.Windows.Forms.CheckBox();
            this.comboBox_suffix2 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_reset = new System.Windows.Forms.Button();
            this.tb_report_per_interval = new System.Windows.Forms.TextBox();
            this.tb_interval_delay = new System.Windows.Forms.TextBox();
            this.btn_inv_config = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.rb_fast_mode = new System.Windows.Forms.RadioButton();
            this.rb_basic_mode = new System.Windows.Forms.RadioButton();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tb_status
            // 
            this.tb_status.Location = new System.Drawing.Point(12, 357);
            this.tb_status.Multiline = true;
            this.tb_status.Name = "tb_status";
            this.tb_status.ReadOnly = true;
            this.tb_status.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_status.Size = new System.Drawing.Size(524, 59);
            this.tb_status.TabIndex = 5;
            // 
            // cb_bt_mode
            // 
            this.cb_bt_mode.AutoSize = true;
            this.cb_bt_mode.Location = new System.Drawing.Point(15, 13);
            this.cb_bt_mode.Name = "cb_bt_mode";
            this.cb_bt_mode.Size = new System.Drawing.Size(158, 17);
            this.cb_bt_mode.TabIndex = 30;
            this.cb_bt_mode.Text = "Enable Bluetooth HID mode";
            this.cb_bt_mode.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 340);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Status:";
            // 
            // btn_set
            // 
            this.btn_set.Location = new System.Drawing.Point(440, 94);
            this.btn_set.Name = "btn_set";
            this.btn_set.Size = new System.Drawing.Size(78, 29);
            this.btn_set.TabIndex = 25;
            this.btn_set.Text = "Save";
            this.btn_set.UseVisualStyleBackColor = true;
            this.btn_set.Click += new System.EventHandler(this.btn_set_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rb_raw);
            this.groupBox2.Controls.Add(this.rb_tlv);
            this.groupBox2.Location = new System.Drawing.Point(12, 36);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(524, 74);
            this.groupBox2.TabIndex = 29;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Output Text Format";
            // 
            // rb_raw
            // 
            this.rb_raw.AutoSize = true;
            this.rb_raw.Location = new System.Drawing.Point(7, 43);
            this.rb_raw.Name = "rb_raw";
            this.rb_raw.Size = new System.Drawing.Size(266, 17);
            this.rb_raw.TabIndex = 27;
            this.rb_raw.Text = "RAW: <Prefix1><Prefix2><EPC><Suffix1><Suffix2>";
            this.rb_raw.UseVisualStyleBackColor = true;
            // 
            // rb_tlv
            // 
            this.rb_tlv.AutoSize = true;
            this.rb_tlv.Checked = true;
            this.rb_tlv.Location = new System.Drawing.Point(7, 20);
            this.rb_tlv.Name = "rb_tlv";
            this.rb_tlv.Size = new System.Drawing.Size(458, 17);
            this.rb_tlv.TabIndex = 26;
            this.rb_tlv.TabStop = true;
            this.rb_tlv.Text = "TLV :  T,<Length>,<Prefix1><Prefix2><EPC><Suffix1><Suffix2><Enter Key> (factory d" +
    "efault)";
            this.rb_tlv.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = "Prefix2: ";
            // 
            // comboBox_prefix2
            // 
            this.comboBox_prefix2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_prefix2.FormattingEnabled = true;
            this.comboBox_prefix2.Items.AddRange(new object[] {
            "(empty)",
            "a",
            "b",
            "c",
            "d",
            "e",
            "f",
            "g",
            "h",
            "i",
            "j",
            "k",
            "l",
            "m",
            "n",
            "o",
            "p",
            "q",
            "r",
            "s",
            "t",
            "u",
            "v",
            "w",
            "x",
            "y",
            "z",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "0",
            "Enter",
            "Space",
            "-",
            "=",
            "[",
            "]",
            "\\",
            ";",
            "\'",
            ",",
            ".",
            "/"});
            this.comboBox_prefix2.Location = new System.Drawing.Point(50, 44);
            this.comboBox_prefix2.Name = "comboBox_prefix2";
            this.comboBox_prefix2.Size = new System.Drawing.Size(151, 21);
            this.comboBox_prefix2.TabIndex = 55;
            this.comboBox_prefix2.Text = "(empty)";
            // 
            // cb_prefix2_enable_shift
            // 
            this.cb_prefix2_enable_shift.AutoSize = true;
            this.cb_prefix2_enable_shift.Location = new System.Drawing.Point(213, 47);
            this.cb_prefix2_enable_shift.Name = "cb_prefix2_enable_shift";
            this.cb_prefix2_enable_shift.Size = new System.Drawing.Size(104, 17);
            this.cb_prefix2_enable_shift.TabIndex = 56;
            this.cb_prefix2_enable_shift.Text = "Enable Shift Key";
            this.cb_prefix2_enable_shift.UseVisualStyleBackColor = true;
            this.cb_prefix2_enable_shift.CheckedChanged += new System.EventHandler(this.cb_prefix2_enable_shift_CheckedChanged);
            // 
            // cb_suffix1_enable_shift
            // 
            this.cb_suffix1_enable_shift.AutoSize = true;
            this.cb_suffix1_enable_shift.Location = new System.Drawing.Point(213, 74);
            this.cb_suffix1_enable_shift.Name = "cb_suffix1_enable_shift";
            this.cb_suffix1_enable_shift.Size = new System.Drawing.Size(104, 17);
            this.cb_suffix1_enable_shift.TabIndex = 59;
            this.cb_suffix1_enable_shift.Text = "Enable Shift Key";
            this.cb_suffix1_enable_shift.UseVisualStyleBackColor = true;
            this.cb_suffix1_enable_shift.CheckedChanged += new System.EventHandler(this.cb_suffix1_enable_shift_CheckedChanged);
            // 
            // comboBox_suffix1
            // 
            this.comboBox_suffix1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_suffix1.FormattingEnabled = true;
            this.comboBox_suffix1.Items.AddRange(new object[] {
            "(empty)",
            "a",
            "b",
            "c",
            "d",
            "e",
            "f",
            "g",
            "h",
            "i",
            "j",
            "k",
            "l",
            "m",
            "n",
            "o",
            "p",
            "q",
            "r",
            "s",
            "t",
            "u",
            "v",
            "w",
            "x",
            "y",
            "z",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "0",
            "Enter",
            "Space",
            "-",
            "=",
            "[",
            "]",
            "\\",
            ";",
            "\'",
            ",",
            ".",
            "/"});
            this.comboBox_suffix1.Location = new System.Drawing.Point(50, 71);
            this.comboBox_suffix1.Name = "comboBox_suffix1";
            this.comboBox_suffix1.Size = new System.Drawing.Size(151, 21);
            this.comboBox_suffix1.TabIndex = 58;
            this.comboBox_suffix1.Text = "(empty)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 57;
            this.label3.Text = "Suffix1: ";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cb_prefix1_enable_shift);
            this.groupBox1.Controls.Add(this.comboBox_prefix1);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cb_suffix2_enable_shift);
            this.groupBox1.Controls.Add(this.comboBox_suffix2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.btn_reset);
            this.groupBox1.Controls.Add(this.cb_suffix1_enable_shift);
            this.groupBox1.Controls.Add(this.comboBox_suffix1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btn_set);
            this.groupBox1.Controls.Add(this.cb_prefix2_enable_shift);
            this.groupBox1.Controls.Add(this.comboBox_prefix2);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 211);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(524, 127);
            this.groupBox1.TabIndex = 60;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Prefix and Suffix";
            // 
            // cb_prefix1_enable_shift
            // 
            this.cb_prefix1_enable_shift.AutoSize = true;
            this.cb_prefix1_enable_shift.Location = new System.Drawing.Point(213, 20);
            this.cb_prefix1_enable_shift.Name = "cb_prefix1_enable_shift";
            this.cb_prefix1_enable_shift.Size = new System.Drawing.Size(104, 17);
            this.cb_prefix1_enable_shift.TabIndex = 66;
            this.cb_prefix1_enable_shift.Text = "Enable Shift Key";
            this.cb_prefix1_enable_shift.UseVisualStyleBackColor = true;
            this.cb_prefix1_enable_shift.CheckedChanged += new System.EventHandler(this.cb_prefix1_enable_shift_CheckedChanged);
            // 
            // comboBox_prefix1
            // 
            this.comboBox_prefix1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_prefix1.FormattingEnabled = true;
            this.comboBox_prefix1.Items.AddRange(new object[] {
            "(empty)",
            "a",
            "b",
            "c",
            "d",
            "e",
            "f",
            "g",
            "h",
            "i",
            "j",
            "k",
            "l",
            "m",
            "n",
            "o",
            "p",
            "q",
            "r",
            "s",
            "t",
            "u",
            "v",
            "w",
            "x",
            "y",
            "z",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "0",
            "Enter",
            "Space",
            "-",
            "=",
            "[",
            "]",
            "\\",
            ";",
            "\'",
            ",",
            ".",
            "/"});
            this.comboBox_prefix1.Location = new System.Drawing.Point(50, 17);
            this.comboBox_prefix1.Name = "comboBox_prefix1";
            this.comboBox_prefix1.Size = new System.Drawing.Size(151, 21);
            this.comboBox_prefix1.TabIndex = 65;
            this.comboBox_prefix1.Text = "(empty)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 13);
            this.label5.TabIndex = 64;
            this.label5.Text = "Prefix1: ";
            // 
            // cb_suffix2_enable_shift
            // 
            this.cb_suffix2_enable_shift.AutoSize = true;
            this.cb_suffix2_enable_shift.Location = new System.Drawing.Point(213, 101);
            this.cb_suffix2_enable_shift.Name = "cb_suffix2_enable_shift";
            this.cb_suffix2_enable_shift.Size = new System.Drawing.Size(104, 17);
            this.cb_suffix2_enable_shift.TabIndex = 63;
            this.cb_suffix2_enable_shift.Text = "Enable Shift Key";
            this.cb_suffix2_enable_shift.UseVisualStyleBackColor = true;
            this.cb_suffix2_enable_shift.CheckedChanged += new System.EventHandler(this.cb_suffix2_enable_shift_CheckedChanged);
            // 
            // comboBox_suffix2
            // 
            this.comboBox_suffix2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_suffix2.FormattingEnabled = true;
            this.comboBox_suffix2.Items.AddRange(new object[] {
            "(empty)",
            "a",
            "b",
            "c",
            "d",
            "e",
            "f",
            "g",
            "h",
            "i",
            "j",
            "k",
            "l",
            "m",
            "n",
            "o",
            "p",
            "q",
            "r",
            "s",
            "t",
            "u",
            "v",
            "w",
            "x",
            "y",
            "z",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "0",
            "Enter",
            "Space",
            "-",
            "=",
            "[",
            "]",
            "\\",
            ";",
            "\'",
            ",",
            ".",
            "/"});
            this.comboBox_suffix2.Location = new System.Drawing.Point(50, 98);
            this.comboBox_suffix2.Name = "comboBox_suffix2";
            this.comboBox_suffix2.Size = new System.Drawing.Size(151, 21);
            this.comboBox_suffix2.TabIndex = 62;
            this.comboBox_suffix2.Text = "(empty)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 61;
            this.label4.Text = "Suffix2: ";
            // 
            // btn_reset
            // 
            this.btn_reset.Location = new System.Drawing.Point(329, 94);
            this.btn_reset.Name = "btn_reset";
            this.btn_reset.Size = new System.Drawing.Size(105, 29);
            this.btn_reset.TabIndex = 60;
            this.btn_reset.Text = "Reset Factory";
            this.btn_reset.UseVisualStyleBackColor = true;
            this.btn_reset.Click += new System.EventHandler(this.btn_reset_Click);
            // 
            // tb_report_per_interval
            // 
            this.tb_report_per_interval.Enabled = false;
            this.tb_report_per_interval.Location = new System.Drawing.Point(480, 64);
            this.tb_report_per_interval.Name = "tb_report_per_interval";
            this.tb_report_per_interval.Size = new System.Drawing.Size(38, 20);
            this.tb_report_per_interval.TabIndex = 69;
            // 
            // tb_interval_delay
            // 
            this.tb_interval_delay.Enabled = false;
            this.tb_interval_delay.Location = new System.Drawing.Point(480, 39);
            this.tb_interval_delay.Name = "tb_interval_delay";
            this.tb_interval_delay.Size = new System.Drawing.Size(38, 20);
            this.tb_interval_delay.TabIndex = 68;
            // 
            // btn_inv_config
            // 
            this.btn_inv_config.Location = new System.Drawing.Point(179, 6);
            this.btn_inv_config.Name = "btn_inv_config";
            this.btn_inv_config.Size = new System.Drawing.Size(123, 29);
            this.btn_inv_config.TabIndex = 64;
            this.btn_inv_config.Text = "Inventory Settings";
            this.btn_inv_config.UseVisualStyleBackColor = true;
            this.btn_inv_config.Click += new System.EventHandler(this.btn_inv_config_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tb_report_per_interval);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.tb_interval_delay);
            this.groupBox3.Controls.Add(this.rb_fast_mode);
            this.groupBox3.Controls.Add(this.rb_basic_mode);
            this.groupBox3.Location = new System.Drawing.Point(12, 116);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(524, 91);
            this.groupBox3.TabIndex = 30;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "HID Speed";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(7, 62);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(400, 17);
            this.label6.TabIndex = 65;
            this.label6.Text = "Remark: Please use Basic Mode if the output text is corrupted.";
            // 
            // rb_fast_mode
            // 
            this.rb_fast_mode.AutoSize = true;
            this.rb_fast_mode.Location = new System.Drawing.Point(10, 39);
            this.rb_fast_mode.Name = "rb_fast_mode";
            this.rb_fast_mode.Size = new System.Drawing.Size(188, 17);
            this.rb_fast_mode.TabIndex = 27;
            this.rb_fast_mode.Text = "Fast Mode (for iPhone, Oppo, etc.)";
            this.rb_fast_mode.UseVisualStyleBackColor = true;
            // 
            // rb_basic_mode
            // 
            this.rb_basic_mode.AutoSize = true;
            this.rb_basic_mode.Checked = true;
            this.rb_basic_mode.Location = new System.Drawing.Point(10, 19);
            this.rb_basic_mode.Name = "rb_basic_mode";
            this.rb_basic_mode.Size = new System.Drawing.Size(157, 17);
            this.rb_basic_mode.TabIndex = 26;
            this.rb_basic_mode.TabStop = true;
            this.rb_basic_mode.Text = "Basic Mode (factory default)";
            this.rb_basic_mode.UseVisualStyleBackColor = true;
            // 
            // BtHidConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 425);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btn_inv_config);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cb_bt_mode);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_status);
            this.Name = "BtHidConfigForm";
            this.Text = "Bluetooth HID mode configuration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tb_status;
        private System.Windows.Forms.CheckBox cb_bt_mode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_set;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rb_raw;
        private System.Windows.Forms.RadioButton rb_tlv;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_prefix2;
        private System.Windows.Forms.CheckBox cb_prefix2_enable_shift;
        private System.Windows.Forms.CheckBox cb_suffix1_enable_shift;
        private System.Windows.Forms.ComboBox comboBox_suffix1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_reset;
        private System.Windows.Forms.CheckBox cb_suffix2_enable_shift;
        private System.Windows.Forms.ComboBox comboBox_suffix2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_inv_config;
        private System.Windows.Forms.CheckBox cb_prefix1_enable_shift;
        private System.Windows.Forms.ComboBox comboBox_prefix1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb_report_per_interval;
        private System.Windows.Forms.TextBox tb_interval_delay;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rb_fast_mode;
        private System.Windows.Forms.RadioButton rb_basic_mode;
        private System.Windows.Forms.Label label6;
    }
}


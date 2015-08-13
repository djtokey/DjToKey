namespace Ktos.DjToKey
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.lbDevice = new System.Windows.Forms.Label();
            this.cbMidiDevices = new System.Windows.Forms.ComboBox();
            this.gbBindings = new System.Windows.Forms.GroupBox();
            this.tlpBindings = new System.Windows.Forms.TableLayoutPanel();
            this.btnSave = new System.Windows.Forms.Button();
            this.lbAbout = new System.Windows.Forms.LinkLabel();
            this.lbPlugins = new System.Windows.Forms.LinkLabel();
            this.gbBindings.SuspendLayout();
            this.SuspendLayout();
            // 
            // trayIcon
            // 
            this.trayIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
            this.trayIcon.Text = "DjToKey";
            this.trayIcon.DoubleClick += new System.EventHandler(this.TrayIcon_DoubleClick);
            // 
            // lbDevice
            // 
            this.lbDevice.AutoSize = true;
            this.lbDevice.Location = new System.Drawing.Point(17, 16);
            this.lbDevice.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbDevice.Name = "lbDevice";
            this.lbDevice.Size = new System.Drawing.Size(115, 17);
            this.lbDevice.TabIndex = 0;
            this.lbDevice.Text = "Urządzenie MIDI:";
            // 
            // cbMidiDevices
            // 
            this.cbMidiDevices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbMidiDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMidiDevices.FormattingEnabled = true;
            this.cbMidiDevices.Location = new System.Drawing.Point(145, 16);
            this.cbMidiDevices.Margin = new System.Windows.Forms.Padding(4);
            this.cbMidiDevices.Name = "cbMidiDevices";
            this.cbMidiDevices.Size = new System.Drawing.Size(945, 24);
            this.cbMidiDevices.TabIndex = 1;
            this.cbMidiDevices.SelectedIndexChanged += new System.EventHandler(this.cbMidiDevices_SelectedIndexChanged);
            // 
            // gbBindings
            // 
            this.gbBindings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbBindings.Controls.Add(this.tlpBindings);
            this.gbBindings.Location = new System.Drawing.Point(17, 49);
            this.gbBindings.Margin = new System.Windows.Forms.Padding(4);
            this.gbBindings.Name = "gbBindings";
            this.gbBindings.Padding = new System.Windows.Forms.Padding(4);
            this.gbBindings.Size = new System.Drawing.Size(1075, 495);
            this.gbBindings.TabIndex = 2;
            this.gbBindings.TabStop = false;
            this.gbBindings.Text = "Konfiguracja klawiszy";
            // 
            // tlpBindings
            // 
            this.tlpBindings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpBindings.AutoScroll = true;
            this.tlpBindings.ColumnCount = 2;
            this.tlpBindings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpBindings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tlpBindings.Location = new System.Drawing.Point(9, 25);
            this.tlpBindings.Margin = new System.Windows.Forms.Padding(4);
            this.tlpBindings.Name = "tlpBindings";
            this.tlpBindings.RowCount = 1;
            this.tlpBindings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBindings.Size = new System.Drawing.Size(1057, 434);
            this.tlpBindings.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(992, 556);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 28);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Zapisz";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lbAbout
            // 
            this.lbAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbAbout.AutoSize = true;
            this.lbAbout.Location = new System.Drawing.Point(17, 576);
            this.lbAbout.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbAbout.Name = "lbAbout";
            this.lbAbout.Size = new System.Drawing.Size(45, 17);
            this.lbAbout.TabIndex = 4;
            this.lbAbout.TabStop = true;
            this.lbAbout.Text = "About";
            this.lbAbout.Click += new System.EventHandler(this.linkLabel1_Click);
            // 
            // lbPlugins
            // 
            this.lbPlugins.AutoSize = true;
            this.lbPlugins.Location = new System.Drawing.Point(70, 576);
            this.lbPlugins.Name = "lbPlugins";
            this.lbPlugins.Size = new System.Drawing.Size(54, 17);
            this.lbPlugins.TabIndex = 5;
            this.lbPlugins.TabStop = true;
            this.lbPlugins.Text = "Plugins";
            this.lbPlugins.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1109, 607);
            this.Controls.Add(this.lbPlugins);
            this.Controls.Add(this.lbAbout);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gbBindings);
            this.Controls.Add(this.cbMidiDevices);
            this.Controls.Add(this.lbDevice);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(1125, 644);
            this.Name = "MainForm";
            this.Text = "DjToKey";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.gbBindings.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.Label lbDevice;
        private System.Windows.Forms.ComboBox cbMidiDevices;
        private System.Windows.Forms.GroupBox gbBindings;
        private System.Windows.Forms.TableLayoutPanel tlpBindings;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.LinkLabel lbAbout;
        private System.Windows.Forms.LinkLabel lbPlugins;
    }
}
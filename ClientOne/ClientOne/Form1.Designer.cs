namespace ClientOne
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            NewGameButton = new Button();
            SurrenderButton = new Button();
            SendButton = new Button();
            ChatTextBox = new TextBox();
            MessageTextBox6 = new TextBox();
            SymbolO = new Button();
            SymbolX = new Button();
            NickName = new TextBox();
            pictureBox1 = new PictureBox();
            StartButton = new Button();
            ServerPortTextBox = new TextBox();
            ServerIPtextBox = new TextBox();
            btnTic1 = new Button();
            btnTic2 = new Button();
            btnTic3 = new Button();
            btnTic4 = new Button();
            btnTic5 = new Button();
            btnTic6 = new Button();
            btnTic7 = new Button();
            btnTic8 = new Button();
            btnTic9 = new Button();
            btnTic10 = new Button();
            btnTic11 = new Button();
            btnTic12 = new Button();
            btnTic13 = new Button();
            btnTic14 = new Button();
            btnTic15 = new Button();
            btnTic16 = new Button();
            btnTic17 = new Button();
            btnTic18 = new Button();
            btnTic19 = new Button();
            btnTic20 = new Button();
            btnTic21 = new Button();
            btnTic22 = new Button();
            btnTic23 = new Button();
            btnTic24 = new Button();
            btnTic25 = new Button();
            btnTic26 = new Button();
            btnTic27 = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            panel1 = new Panel();
            panel2 = new Panel();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // NewGameButton
            // 
            NewGameButton.BackColor = Color.DarkTurquoise;
            NewGameButton.Font = new Font("Calibri", 12.75F, FontStyle.Bold, GraphicsUnit.Point);
            NewGameButton.ForeColor = Color.Snow;
            NewGameButton.Location = new Point(384, 432);
            NewGameButton.Name = "NewGameButton";
            NewGameButton.Size = new Size(107, 92);
            NewGameButton.TabIndex = 22;
            NewGameButton.Text = "NEW GAME";
            NewGameButton.UseVisualStyleBackColor = false;
            NewGameButton.Click += NewGameButton_Click;
            // 
            // SurrenderButton
            // 
            SurrenderButton.BackColor = Color.IndianRed;
            SurrenderButton.Font = new Font("Calibri", 12.75F, FontStyle.Bold, GraphicsUnit.Point);
            SurrenderButton.ForeColor = Color.Snow;
            SurrenderButton.Location = new Point(542, 432);
            SurrenderButton.Name = "SurrenderButton";
            SurrenderButton.Size = new Size(107, 92);
            SurrenderButton.TabIndex = 21;
            SurrenderButton.Text = "SURRENDER";
            SurrenderButton.UseVisualStyleBackColor = false;
            SurrenderButton.Click += SurrenderButton_Click;
            // 
            // SendButton
            // 
            SendButton.BackColor = Color.Gold;
            SendButton.Font = new Font("Calibri", 12.75F, FontStyle.Bold, GraphicsUnit.Point);
            SendButton.Location = new Point(636, 300);
            SendButton.Name = "SendButton";
            SendButton.Size = new Size(59, 91);
            SendButton.TabIndex = 8;
            SendButton.Text = "SEND";
            SendButton.UseVisualStyleBackColor = false;
            SendButton.Click += SendButton_Click;
            // 
            // ChatTextBox
            // 
            ChatTextBox.BackColor = Color.Snow;
            ChatTextBox.Location = new Point(332, 24);
            ChatTextBox.Multiline = true;
            ChatTextBox.Name = "ChatTextBox";
            ChatTextBox.Size = new Size(362, 256);
            ChatTextBox.TabIndex = 1;
            // 
            // MessageTextBox6
            // 
            MessageTextBox6.BackColor = Color.Snow;
            MessageTextBox6.Location = new Point(332, 300);
            MessageTextBox6.Multiline = true;
            MessageTextBox6.Name = "MessageTextBox6";
            MessageTextBox6.Size = new Size(298, 91);
            MessageTextBox6.TabIndex = 0;
            // 
            // SymbolO
            // 
            SymbolO.BackColor = Color.Gold;
            SymbolO.Font = new Font("Calibri", 12.75F, FontStyle.Bold, GraphicsUnit.Point);
            SymbolO.ForeColor = Color.Black;
            SymbolO.Location = new Point(173, 249);
            SymbolO.Name = "SymbolO";
            SymbolO.Size = new Size(103, 88);
            SymbolO.TabIndex = 12;
            SymbolO.Text = "O";
            SymbolO.UseVisualStyleBackColor = false;
            SymbolO.Click += Symbol_Click;
            // 
            // SymbolX
            // 
            SymbolX.BackColor = Color.Gold;
            SymbolX.Font = new Font("Calibri", 12.75F, FontStyle.Bold, GraphicsUnit.Point);
            SymbolX.ForeColor = Color.Black;
            SymbolX.Location = new Point(35, 249);
            SymbolX.Name = "SymbolX";
            SymbolX.Size = new Size(103, 88);
            SymbolX.TabIndex = 11;
            SymbolX.Text = "X";
            SymbolX.UseVisualStyleBackColor = false;
            SymbolX.Click += Symbol_Click;
            // 
            // NickName
            // 
            NickName.BackColor = Color.Snow;
            NickName.Location = new Point(35, 209);
            NickName.Multiline = true;
            NickName.Name = "NickName";
            NickName.Size = new Size(241, 30);
            NickName.TabIndex = 4;
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImage = Properties.Resources.tic_tac_toe__5_;
            pictureBox1.Location = new Point(84, 410);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(128, 127);
            pictureBox1.TabIndex = 14;
            pictureBox1.TabStop = false;
            // 
            // StartButton
            // 
            StartButton.BackColor = Color.Gold;
            StartButton.Font = new Font("Calibri", 12.75F, FontStyle.Bold, GraphicsUnit.Point);
            StartButton.Location = new Point(12, 77);
            StartButton.Name = "StartButton";
            StartButton.Size = new Size(298, 34);
            StartButton.TabIndex = 7;
            StartButton.Text = "START";
            StartButton.UseVisualStyleBackColor = false;
            StartButton.Click += StartButton_Click;
            // 
            // ServerPortTextBox
            // 
            ServerPortTextBox.BackColor = Color.Snow;
            ServerPortTextBox.Location = new Point(168, 30);
            ServerPortTextBox.Multiline = true;
            ServerPortTextBox.Name = "ServerPortTextBox";
            ServerPortTextBox.Size = new Size(142, 30);
            ServerPortTextBox.TabIndex = 6;
            // 
            // ServerIPtextBox
            // 
            ServerIPtextBox.BackColor = Color.Snow;
            ServerIPtextBox.Location = new Point(12, 30);
            ServerIPtextBox.Multiline = true;
            ServerIPtextBox.Name = "ServerIPtextBox";
            ServerIPtextBox.Size = new Size(150, 30);
            ServerIPtextBox.TabIndex = 5;
            // 
            // btnTic1
            // 
            btnTic1.BackColor = Color.Snow;
            btnTic1.Location = new Point(728, 24);
            btnTic1.Name = "btnTic1";
            btnTic1.Size = new Size(68, 54);
            btnTic1.TabIndex = 9;
            btnTic1.UseVisualStyleBackColor = false;
            btnTic1.Click += btnTic1_Click;
            // 
            // btnTic2
            // 
            btnTic2.BackColor = Color.Snow;
            btnTic2.Location = new Point(802, 24);
            btnTic2.Name = "btnTic2";
            btnTic2.Size = new Size(68, 54);
            btnTic2.TabIndex = 13;
            btnTic2.UseVisualStyleBackColor = false;
            btnTic2.Click += btnTic2_Click;
            // 
            // btnTic3
            // 
            btnTic3.BackColor = Color.Snow;
            btnTic3.Location = new Point(876, 24);
            btnTic3.Name = "btnTic3";
            btnTic3.Size = new Size(68, 54);
            btnTic3.TabIndex = 14;
            btnTic3.UseVisualStyleBackColor = false;
            btnTic3.Click += btnTic3_Click;
            // 
            // btnTic4
            // 
            btnTic4.BackColor = Color.Snow;
            btnTic4.Location = new Point(728, 84);
            btnTic4.Name = "btnTic4";
            btnTic4.Size = new Size(68, 54);
            btnTic4.TabIndex = 15;
            btnTic4.UseVisualStyleBackColor = false;
            btnTic4.Click += btnTic4_Click;
            // 
            // btnTic5
            // 
            btnTic5.BackColor = Color.Snow;
            btnTic5.Location = new Point(802, 84);
            btnTic5.Name = "btnTic5";
            btnTic5.Size = new Size(68, 54);
            btnTic5.TabIndex = 16;
            btnTic5.UseVisualStyleBackColor = false;
            btnTic5.Click += btnTic5_Click;
            // 
            // btnTic6
            // 
            btnTic6.BackColor = Color.Snow;
            btnTic6.Location = new Point(876, 84);
            btnTic6.Name = "btnTic6";
            btnTic6.Size = new Size(68, 54);
            btnTic6.TabIndex = 17;
            btnTic6.UseVisualStyleBackColor = false;
            btnTic6.Click += btnTic6_Click;
            // 
            // btnTic7
            // 
            btnTic7.BackColor = Color.Snow;
            btnTic7.Location = new Point(728, 144);
            btnTic7.Name = "btnTic7";
            btnTic7.Size = new Size(68, 54);
            btnTic7.TabIndex = 18;
            btnTic7.UseVisualStyleBackColor = false;
            btnTic7.Click += btnTic7_Click;
            // 
            // btnTic8
            // 
            btnTic8.BackColor = Color.Snow;
            btnTic8.Location = new Point(802, 144);
            btnTic8.Name = "btnTic8";
            btnTic8.Size = new Size(68, 54);
            btnTic8.TabIndex = 19;
            btnTic8.UseVisualStyleBackColor = false;
            btnTic8.Click += btnTic8_Click;
            // 
            // btnTic9
            // 
            btnTic9.BackColor = Color.Snow;
            btnTic9.Location = new Point(876, 144);
            btnTic9.Name = "btnTic9";
            btnTic9.Size = new Size(68, 54);
            btnTic9.TabIndex = 20;
            btnTic9.UseVisualStyleBackColor = false;
            btnTic9.Click += btnTic9_Click;
            // 
            // btnTic10
            // 
            btnTic10.BackColor = Color.Snow;
            btnTic10.Location = new Point(728, 210);
            btnTic10.Name = "btnTic10";
            btnTic10.Size = new Size(68, 54);
            btnTic10.TabIndex = 23;
            btnTic10.UseVisualStyleBackColor = false;
            btnTic10.Click += btnTic10_Click;
            // 
            // btnTic11
            // 
            btnTic11.BackColor = Color.Snow;
            btnTic11.Location = new Point(802, 210);
            btnTic11.Name = "btnTic11";
            btnTic11.Size = new Size(68, 54);
            btnTic11.TabIndex = 24;
            btnTic11.UseVisualStyleBackColor = false;
            btnTic11.Click += btnTic11_Click;
            // 
            // btnTic12
            // 
            btnTic12.BackColor = Color.Snow;
            btnTic12.Location = new Point(876, 210);
            btnTic12.Name = "btnTic12";
            btnTic12.Size = new Size(68, 54);
            btnTic12.TabIndex = 25;
            btnTic12.UseVisualStyleBackColor = false;
            btnTic12.Click += btnTic12_Click;
            // 
            // btnTic13
            // 
            btnTic13.BackColor = Color.Snow;
            btnTic13.Location = new Point(728, 270);
            btnTic13.Name = "btnTic13";
            btnTic13.Size = new Size(68, 54);
            btnTic13.TabIndex = 26;
            btnTic13.UseVisualStyleBackColor = false;
            btnTic13.Click += btnTic13_Click;
            // 
            // btnTic14
            // 
            btnTic14.BackColor = Color.Snow;
            btnTic14.Location = new Point(802, 270);
            btnTic14.Name = "btnTic14";
            btnTic14.Size = new Size(68, 54);
            btnTic14.TabIndex = 27;
            btnTic14.UseVisualStyleBackColor = false;
            btnTic14.Click += btnTic14_Click;
            // 
            // btnTic15
            // 
            btnTic15.BackColor = Color.Snow;
            btnTic15.Location = new Point(876, 270);
            btnTic15.Name = "btnTic15";
            btnTic15.Size = new Size(68, 54);
            btnTic15.TabIndex = 28;
            btnTic15.UseVisualStyleBackColor = false;
            btnTic15.Click += btnTic15_Click;
            // 
            // btnTic16
            // 
            btnTic16.BackColor = Color.Snow;
            btnTic16.Location = new Point(728, 330);
            btnTic16.Name = "btnTic16";
            btnTic16.Size = new Size(68, 54);
            btnTic16.TabIndex = 29;
            btnTic16.UseVisualStyleBackColor = false;
            btnTic16.Click += btnTic16_Click;
            // 
            // btnTic17
            // 
            btnTic17.BackColor = Color.Snow;
            btnTic17.Location = new Point(802, 330);
            btnTic17.Name = "btnTic17";
            btnTic17.Size = new Size(68, 54);
            btnTic17.TabIndex = 30;
            btnTic17.UseVisualStyleBackColor = false;
            btnTic17.Click += btnTic17_Click;
            // 
            // btnTic18
            // 
            btnTic18.BackColor = Color.Snow;
            btnTic18.Location = new Point(876, 330);
            btnTic18.Name = "btnTic18";
            btnTic18.Size = new Size(68, 54);
            btnTic18.TabIndex = 31;
            btnTic18.UseVisualStyleBackColor = false;
            btnTic18.Click += btnTic18_Click;
            // 
            // btnTic19
            // 
            btnTic19.BackColor = Color.Snow;
            btnTic19.Location = new Point(728, 395);
            btnTic19.Name = "btnTic19";
            btnTic19.Size = new Size(68, 54);
            btnTic19.TabIndex = 32;
            btnTic19.UseVisualStyleBackColor = false;
            btnTic19.Click += btnTic19_Click;
            // 
            // btnTic20
            // 
            btnTic20.BackColor = Color.Snow;
            btnTic20.Location = new Point(802, 395);
            btnTic20.Name = "btnTic20";
            btnTic20.Size = new Size(68, 54);
            btnTic20.TabIndex = 33;
            btnTic20.UseVisualStyleBackColor = false;
            btnTic20.Click += btnTic20_Click;
            // 
            // btnTic21
            // 
            btnTic21.BackColor = Color.Snow;
            btnTic21.Location = new Point(876, 395);
            btnTic21.Name = "btnTic21";
            btnTic21.Size = new Size(68, 54);
            btnTic21.TabIndex = 34;
            btnTic21.UseVisualStyleBackColor = false;
            btnTic21.Click += btnTic21_Click;
            // 
            // btnTic22
            // 
            btnTic22.BackColor = Color.Snow;
            btnTic22.Location = new Point(728, 455);
            btnTic22.Name = "btnTic22";
            btnTic22.Size = new Size(68, 54);
            btnTic22.TabIndex = 35;
            btnTic22.UseVisualStyleBackColor = false;
            btnTic22.Click += btnTic22_Click;
            // 
            // btnTic23
            // 
            btnTic23.BackColor = Color.Snow;
            btnTic23.Location = new Point(802, 455);
            btnTic23.Name = "btnTic23";
            btnTic23.Size = new Size(68, 54);
            btnTic23.TabIndex = 36;
            btnTic23.UseVisualStyleBackColor = false;
            btnTic23.Click += btnTic23_Click;
            // 
            // btnTic24
            // 
            btnTic24.BackColor = Color.Snow;
            btnTic24.Location = new Point(876, 455);
            btnTic24.Name = "btnTic24";
            btnTic24.Size = new Size(68, 54);
            btnTic24.TabIndex = 37;
            btnTic24.UseVisualStyleBackColor = false;
            btnTic24.Click += btnTic24_Click;
            // 
            // btnTic25
            // 
            btnTic25.BackColor = Color.Snow;
            btnTic25.Location = new Point(728, 515);
            btnTic25.Name = "btnTic25";
            btnTic25.Size = new Size(68, 54);
            btnTic25.TabIndex = 38;
            btnTic25.UseVisualStyleBackColor = false;
            btnTic25.Click += btnTic25_Click;
            // 
            // btnTic26
            // 
            btnTic26.BackColor = Color.Snow;
            btnTic26.Location = new Point(802, 515);
            btnTic26.Name = "btnTic26";
            btnTic26.Size = new Size(68, 54);
            btnTic26.TabIndex = 39;
            btnTic26.UseVisualStyleBackColor = false;
            btnTic26.Click += btnTic26_Click;
            // 
            // btnTic27
            // 
            btnTic27.BackColor = Color.Snow;
            btnTic27.Location = new Point(876, 515);
            btnTic27.Name = "btnTic27";
            btnTic27.Size = new Size(68, 54);
            btnTic27.TabIndex = 40;
            btnTic27.UseVisualStyleBackColor = false;
            btnTic27.Click += btnTic27_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Calibri", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = Color.Snow;
            label1.Location = new Point(74, 9);
            label1.Name = "label1";
            label1.Size = new Size(20, 18);
            label1.TabIndex = 41;
            label1.Text = "IP";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Calibri", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            label2.ForeColor = Color.Snow;
            label2.Location = new Point(222, 9);
            label2.Name = "label2";
            label2.Size = new Size(41, 18);
            label2.TabIndex = 42;
            label2.Text = "PORT";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Calibri", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            label3.ForeColor = Color.Snow;
            label3.Location = new Point(122, 188);
            label3.Name = "label3";
            label3.Size = new Size(70, 18);
            label3.TabIndex = 43;
            label3.Text = "Nickname";
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.Fixed3D;
            panel1.Location = new Point(313, 11);
            panel1.Name = "panel1";
            panel1.Size = new Size(11, 570);
            panel1.TabIndex = 44;
            // 
            // panel2
            // 
            panel2.BorderStyle = BorderStyle.Fixed3D;
            panel2.Location = new Point(700, 12);
            panel2.Name = "panel2";
            panel2.Size = new Size(11, 570);
            panel2.TabIndex = 45;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.SkyBlue;
            ClientSize = new Size(956, 598);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(SymbolO);
            Controls.Add(pictureBox1);
            Controls.Add(SymbolX);
            Controls.Add(NickName);
            Controls.Add(StartButton);
            Controls.Add(ServerPortTextBox);
            Controls.Add(NewGameButton);
            Controls.Add(ServerIPtextBox);
            Controls.Add(btnTic27);
            Controls.Add(btnTic26);
            Controls.Add(SurrenderButton);
            Controls.Add(btnTic25);
            Controls.Add(btnTic24);
            Controls.Add(btnTic23);
            Controls.Add(MessageTextBox6);
            Controls.Add(btnTic22);
            Controls.Add(SendButton);
            Controls.Add(btnTic21);
            Controls.Add(ChatTextBox);
            Controls.Add(btnTic20);
            Controls.Add(btnTic1);
            Controls.Add(btnTic19);
            Controls.Add(btnTic2);
            Controls.Add(btnTic18);
            Controls.Add(btnTic3);
            Controls.Add(btnTic17);
            Controls.Add(btnTic4);
            Controls.Add(btnTic16);
            Controls.Add(btnTic5);
            Controls.Add(btnTic15);
            Controls.Add(btnTic6);
            Controls.Add(btnTic14);
            Controls.Add(btnTic7);
            Controls.Add(btnTic13);
            Controls.Add(btnTic8);
            Controls.Add(btnTic12);
            Controls.Add(btnTic9);
            Controls.Add(btnTic11);
            Controls.Add(btnTic10);
            Name = "Form1";
            Text = "Tic-Tac-Toe";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox MessageTextBox6;
        private TextBox ChatTextBox;
        private Button SendButton;
        private Button SurrenderButton;
        private Button NewGameButton;
        private Button SymbolO;
        private Button SymbolX;
        private TextBox NickName;
        private TextBox ServerIPtextBox;
        private TextBox ServerPortTextBox;
        private Button StartButton;
        private PictureBox pictureBox1;
        private Button btnTic1;
        private Button btnTic2;
        private Button btnTic3;
        private Button btnTic4;
        private Button btnTic5;
        private Button btnTic6;
        private Button btnTic7;
        private Button btnTic8;
        private Button btnTic9;
        private Button btnTic10;
        private Button btnTic11;
        private Button btnTic12;
        private Button btnTic13;
        private Button btnTic14;
        private Button btnTic15;
        private Button btnTic16;
        private Button btnTic17;
        private Button btnTic18;
        private Button btnTic19;
        private Button btnTic20;
        private Button btnTic21;
        private Button btnTic22;
        private Button btnTic23;
        private Button btnTic24;
        private Button btnTic25;
        private Button btnTic26;
        private Button btnTic27;
        private Label label1;
        private Label label2;
        private Label label3;
        private Panel panel1;
        private Panel panel2;
    }
}
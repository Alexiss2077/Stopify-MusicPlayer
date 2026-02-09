namespace Stopify
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.DataGridView dgvCanciones;
        private System.Windows.Forms.Button btnAbrirCarpeta;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnGuardarPlaylist;
        private System.Windows.Forms.Button btnCargarPlaylist;
        private System.Windows.Forms.Button btnAbrirWMP;
        private System.Windows.Forms.Button btnLetra;
        private System.Windows.Forms.PictureBox pbCover;
        private System.Windows.Forms.RichTextBox rtbLyrics;
        private System.Windows.Forms.Label lblCanciones;
        private System.Windows.Forms.Label lblCover;
        private System.Windows.Forms.Label lblLyrics;

        private System.Windows.Forms.TrackBar tbProgreso;
        private System.Windows.Forms.TrackBar tbVolumen;
        private System.Windows.Forms.Label lblTiempoActual;
        private System.Windows.Forms.Label lblTiempoTotal;
        private System.Windows.Forms.Label lblVolumen;

        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;

        private System.Windows.Forms.Timer timer1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            dgvCanciones = new DataGridView();
            btnAbrirCarpeta = new Button();
            btnPlay = new Button();
            btnPause = new Button();
            btnStop = new Button();
            btnGuardarPlaylist = new Button();
            btnCargarPlaylist = new Button();
            btnAbrirWMP = new Button();
            btnLetra = new Button();
            pbCover = new PictureBox();
            rtbLyrics = new RichTextBox();
            lblCanciones = new Label();
            lblCover = new Label();
            lblLyrics = new Label();
            tbProgreso = new TrackBar();
            tbVolumen = new TrackBar();
            lblTiempoActual = new Label();
            lblTiempoTotal = new Label();
            lblVolumen = new Label();
            btnNext = new Button();
            btnPrev = new Button();
            timer1 = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)dgvCanciones).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbCover).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tbProgreso).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tbVolumen).BeginInit();
            SuspendLayout();
            // 
            // dgvCanciones
            // 
            dgvCanciones.AllowUserToAddRows = false;
            dgvCanciones.AllowUserToDeleteRows = false;
            dgvCanciones.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvCanciones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCanciones.BackgroundColor = Color.FromArgb(64, 64, 64);
            dgvCanciones.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCanciones.Location = new Point(12, 80);
            dgvCanciones.MultiSelect = false;
            dgvCanciones.Name = "dgvCanciones";
            dgvCanciones.ReadOnly = true;
            dgvCanciones.RowHeadersVisible = false;
            dgvCanciones.RowHeadersWidth = 51;
            dgvCanciones.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCanciones.Size = new Size(670, 270);
            dgvCanciones.TabIndex = 0;
            dgvCanciones.CellDoubleClick += dgvCanciones_CellDoubleClick;
            // 
            // btnAbrirCarpeta
            // 
            btnAbrirCarpeta.BackColor = Color.LightGray;
            btnAbrirCarpeta.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAbrirCarpeta.Location = new Point(12, 12);
            btnAbrirCarpeta.Name = "btnAbrirCarpeta";
            btnAbrirCarpeta.Size = new Size(150, 40);
            btnAbrirCarpeta.TabIndex = 1;
            btnAbrirCarpeta.Text = "Abrir Carpeta 📂";
            btnAbrirCarpeta.UseVisualStyleBackColor = false;
            btnAbrirCarpeta.Click += btnAbrirCarpeta_Click;
            // 
            // btnPlay
            // 
            btnPlay.BackColor = Color.LightGray;
            btnPlay.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnPlay.Location = new Point(180, 12);
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new Size(90, 40);
            btnPlay.TabIndex = 2;
            btnPlay.Text = "Play ▶";
            btnPlay.UseVisualStyleBackColor = false;
            btnPlay.Click += btnPlay_Click;
            // 
            // btnPause
            // 
            btnPause.BackColor = Color.LightGray;
            btnPause.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnPause.Location = new Point(280, 12);
            btnPause.Name = "btnPause";
            btnPause.Size = new Size(90, 40);
            btnPause.TabIndex = 3;
            btnPause.Text = "Pause ⏸";
            btnPause.UseVisualStyleBackColor = false;
            btnPause.Click += btnPause_Click;
            // 
            // btnStop
            // 
            btnStop.BackColor = Color.LightGray;
            btnStop.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnStop.Location = new Point(380, 12);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(90, 40);
            btnStop.TabIndex = 4;
            btnStop.Text = "Stop ⏹";
            btnStop.UseVisualStyleBackColor = false;
            btnStop.Click += btnStop_Click;
            // 
            // btnGuardarPlaylist
            // 
            btnGuardarPlaylist.BackColor = Color.LightGray;
            btnGuardarPlaylist.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnGuardarPlaylist.Location = new Point(12, 485);
            btnGuardarPlaylist.Name = "btnGuardarPlaylist";
            btnGuardarPlaylist.Size = new Size(150, 40);
            btnGuardarPlaylist.TabIndex = 13;
            btnGuardarPlaylist.Text = "Guardar Playlist 💾";
            btnGuardarPlaylist.UseVisualStyleBackColor = false;
            btnGuardarPlaylist.Click += btnGuardarPlaylist_Click;
            // 
            // btnCargarPlaylist
            // 
            btnCargarPlaylist.BackColor = Color.LightGray;
            btnCargarPlaylist.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnCargarPlaylist.Location = new Point(180, 485);
            btnCargarPlaylist.Name = "btnCargarPlaylist";
            btnCargarPlaylist.Size = new Size(150, 40);
            btnCargarPlaylist.TabIndex = 14;
            btnCargarPlaylist.Text = "Cargar Playlist 📂";
            btnCargarPlaylist.UseVisualStyleBackColor = false;
            btnCargarPlaylist.Click += btnCargarPlaylist_Click;
            // 
            // btnAbrirWMP
            // 
            btnAbrirWMP.BackColor = Color.LightGray;
            btnAbrirWMP.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnAbrirWMP.Location = new Point(350, 485);
            btnAbrirWMP.Name = "btnAbrirWMP";
            btnAbrirWMP.Size = new Size(150, 40);
            btnAbrirWMP.TabIndex = 15;
            btnAbrirWMP.Text = "Abrir en WMP 🎵";
            btnAbrirWMP.UseVisualStyleBackColor = false;
            btnAbrirWMP.Click += btnAbrirWMP_Click;
            // 
            // btnLetra
            // 
            btnLetra.BackColor = Color.LightGray;
            btnLetra.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnLetra.Location = new Point(512, 485);
            btnLetra.Name = "btnLetra";
            btnLetra.Size = new Size(170, 40);
            btnLetra.TabIndex = 16;
            btnLetra.Text = "Buscar Letra 🎤";
            btnLetra.UseVisualStyleBackColor = false;
            btnLetra.Click += btnLetra_Click;
            // 
            // pbCover
            // 
            pbCover.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pbCover.BackColor = Color.GreenYellow;
            pbCover.BorderStyle = BorderStyle.FixedSingle;
            pbCover.Location = new Point(700, 80);
            pbCover.Name = "pbCover";
            pbCover.Size = new Size(250, 250);
            pbCover.SizeMode = PictureBoxSizeMode.StretchImage;
            pbCover.TabIndex = 17;
            pbCover.TabStop = false;
            // 
            // rtbLyrics
            // 
            rtbLyrics.BackColor = Color.Chartreuse;
            rtbLyrics.Font = new Font("Yu Gothic UI", 13.8F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            rtbLyrics.Location = new Point(700, 363);
            rtbLyrics.Name = "rtbLyrics";
            rtbLyrics.Size = new Size(250, 162);
            rtbLyrics.TabIndex = 20;
            rtbLyrics.Text = "";
            // 
            // lblCanciones
            // 
            lblCanciones.AutoSize = true;
            lblCanciones.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblCanciones.Location = new Point(12, 57);
            lblCanciones.Name = "lblCanciones";
            lblCanciones.Size = new Size(141, 20);
            lblCanciones.TabIndex = 7;
            lblCanciones.Text = "Lista de Canciones:";
            // 
            // lblCover
            // 
            lblCover.AutoSize = true;
            lblCover.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblCover.Location = new Point(700, 57);
            lblCover.Name = "lblCover";
            lblCover.Size = new Size(68, 20);
            lblCover.TabIndex = 18;
            lblCover.Text = "Portada:";
            // 
            // lblLyrics
            // 
            lblLyrics.AutoSize = true;
            lblLyrics.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblLyrics.Location = new Point(700, 340);
            lblLyrics.Name = "lblLyrics";
            lblLyrics.Size = new Size(49, 20);
            lblLyrics.TabIndex = 19;
            lblLyrics.Text = "Letra:";
            // 
            // tbProgreso
            // 
            tbProgreso.Location = new Point(12, 360);
            tbProgreso.Name = "tbProgreso";
            tbProgreso.Size = new Size(670, 45);
            tbProgreso.TabIndex = 8;
            tbProgreso.MouseDown += tbProgreso_MouseDown;
            tbProgreso.MouseUp += tbProgreso_MouseUp;
            // 
            // tbVolumen
            // 
            tbVolumen.Location = new Point(90, 430);
            tbVolumen.Maximum = 100;
            tbVolumen.Name = "tbVolumen";
            tbVolumen.Size = new Size(250, 45);
            tbVolumen.TabIndex = 12;
            tbVolumen.Value = 50;
            tbVolumen.Scroll += tbVolumen_Scroll;
            // 
            // lblTiempoActual
            // 
            lblTiempoActual.AutoSize = true;
            lblTiempoActual.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTiempoActual.Location = new Point(12, 405);
            lblTiempoActual.Name = "lblTiempoActual";
            lblTiempoActual.Size = new Size(45, 19);
            lblTiempoActual.TabIndex = 9;
            lblTiempoActual.Text = "00:00";
            // 
            // lblTiempoTotal
            // 
            lblTiempoTotal.AutoSize = true;
            lblTiempoTotal.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTiempoTotal.Location = new Point(638, 405);
            lblTiempoTotal.Name = "lblTiempoTotal";
            lblTiempoTotal.Size = new Size(45, 19);
            lblTiempoTotal.TabIndex = 10;
            lblTiempoTotal.Text = "00:00";
            // 
            // lblVolumen
            // 
            lblVolumen.AutoSize = true;
            lblVolumen.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblVolumen.Location = new Point(12, 435);
            lblVolumen.Name = "lblVolumen";
            lblVolumen.Size = new Size(71, 19);
            lblVolumen.TabIndex = 11;
            lblVolumen.Text = "Volumen:";
            // 
            // btnNext
            // 
            btnNext.BackColor = Color.LightGray;
            btnNext.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnNext.Location = new Point(580, 12);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(90, 40);
            btnNext.TabIndex = 6;
            btnNext.Text = "Next ⏭";
            btnNext.UseVisualStyleBackColor = false;
            btnNext.Click += btnNext_Click;
            // 
            // btnPrev
            // 
            btnPrev.BackColor = Color.LightGray;
            btnPrev.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnPrev.Location = new Point(480, 12);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(90, 40);
            btnPrev.TabIndex = 5;
            btnPrev.Text = "Prev ⏮";
            btnPrev.UseVisualStyleBackColor = false;
            btnPrev.Click += btnPrev_Click;
            // 
            // timer1
            // 
            timer1.Tick += timer1_Tick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(970, 545);
            Controls.Add(rtbLyrics);
            Controls.Add(lblLyrics);
            Controls.Add(lblCover);
            Controls.Add(pbCover);
            Controls.Add(btnLetra);
            Controls.Add(btnAbrirWMP);
            Controls.Add(btnCargarPlaylist);
            Controls.Add(btnGuardarPlaylist);
            Controls.Add(tbVolumen);
            Controls.Add(lblVolumen);
            Controls.Add(lblTiempoTotal);
            Controls.Add(lblTiempoActual);
            Controls.Add(tbProgreso);
            Controls.Add(lblCanciones);
            Controls.Add(btnNext);
            Controls.Add(btnPrev);
            Controls.Add(btnStop);
            Controls.Add(btnPause);
            Controls.Add(btnPlay);
            Controls.Add(btnAbrirCarpeta);
            Controls.Add(dgvCanciones);
            DoubleBuffered = true;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "Stopify Music Player";
            ((System.ComponentModel.ISupportInitialize)dgvCanciones).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbCover).EndInit();
            ((System.ComponentModel.ISupportInitialize)tbProgreso).EndInit();
            ((System.ComponentModel.ISupportInitialize)tbVolumen).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion
    }
}

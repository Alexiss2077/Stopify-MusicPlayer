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
            this.components = new System.ComponentModel.Container();
            this.dgvCanciones = new System.Windows.Forms.DataGridView();
            this.btnAbrirCarpeta = new System.Windows.Forms.Button();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnGuardarPlaylist = new System.Windows.Forms.Button();
            this.btnCargarPlaylist = new System.Windows.Forms.Button();
            this.btnAbrirWMP = new System.Windows.Forms.Button();
            this.btnLetra = new System.Windows.Forms.Button();
            this.pbCover = new System.Windows.Forms.PictureBox();
            this.rtbLyrics = new System.Windows.Forms.RichTextBox();
            this.lblCanciones = new System.Windows.Forms.Label();
            this.lblCover = new System.Windows.Forms.Label();
            this.lblLyrics = new System.Windows.Forms.Label();
            this.tbProgreso = new System.Windows.Forms.TrackBar();
            this.tbVolumen = new System.Windows.Forms.TrackBar();
            this.lblTiempoActual = new System.Windows.Forms.Label();
            this.lblTiempoTotal = new System.Windows.Forms.Label();
            this.lblVolumen = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCanciones)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCover)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbProgreso)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbVolumen)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvCanciones
            // 
            this.dgvCanciones.AllowUserToAddRows = false;
            this.dgvCanciones.AllowUserToDeleteRows = false;
            this.dgvCanciones.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCanciones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCanciones.Location = new System.Drawing.Point(12, 80);
            this.dgvCanciones.MultiSelect = false;
            this.dgvCanciones.Name = "dgvCanciones";
            this.dgvCanciones.ReadOnly = true;
            this.dgvCanciones.RowHeadersVisible = false;
            this.dgvCanciones.RowTemplate.Height = 25;
            this.dgvCanciones.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCanciones.Size = new System.Drawing.Size(670, 270);
            this.dgvCanciones.TabIndex = 0;
            this.dgvCanciones.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCanciones_CellDoubleClick);
            // 
            // btnAbrirCarpeta
            // 
            this.btnAbrirCarpeta.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnAbrirCarpeta.Location = new System.Drawing.Point(12, 12);
            this.btnAbrirCarpeta.Name = "btnAbrirCarpeta";
            this.btnAbrirCarpeta.Size = new System.Drawing.Size(150, 40);
            this.btnAbrirCarpeta.TabIndex = 1;
            this.btnAbrirCarpeta.Text = "Abrir Carpeta 📂";
            this.btnAbrirCarpeta.UseVisualStyleBackColor = true;
            this.btnAbrirCarpeta.Click += new System.EventHandler(this.btnAbrirCarpeta_Click);
            // 
            // btnPlay
            // 
            this.btnPlay.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnPlay.Location = new System.Drawing.Point(180, 12);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(90, 40);
            this.btnPlay.TabIndex = 2;
            this.btnPlay.Text = "Play ▶";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnPause
            // 
            this.btnPause.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnPause.Location = new System.Drawing.Point(280, 12);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(90, 40);
            this.btnPause.TabIndex = 3;
            this.btnPause.Text = "Pause ⏸";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnStop
            // 
            this.btnStop.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnStop.Location = new System.Drawing.Point(380, 12);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(90, 40);
            this.btnStop.TabIndex = 4;
            this.btnStop.Text = "Stop ⏹";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnPrev.Location = new System.Drawing.Point(480, 12);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(90, 40);
            this.btnPrev.TabIndex = 5;
            this.btnPrev.Text = "Prev ⏮";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnNext
            // 
            this.btnNext.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnNext.Location = new System.Drawing.Point(580, 12);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(90, 40);
            this.btnNext.TabIndex = 6;
            this.btnNext.Text = "Next ⏭";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // lblCanciones
            // 
            this.lblCanciones.AutoSize = true;
            this.lblCanciones.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblCanciones.Location = new System.Drawing.Point(12, 57);
            this.lblCanciones.Name = "lblCanciones";
            this.lblCanciones.Size = new System.Drawing.Size(155, 20);
            this.lblCanciones.TabIndex = 7;
            this.lblCanciones.Text = "Lista de Canciones:";
            // 
            // tbProgreso
            // 
            this.tbProgreso.Location = new System.Drawing.Point(12, 360);
            this.tbProgreso.Name = "tbProgreso";
            this.tbProgreso.Size = new System.Drawing.Size(670, 45);
            this.tbProgreso.TabIndex = 8;
            this.tbProgreso.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tbProgreso_MouseDown);
            this.tbProgreso.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tbProgreso_MouseUp);
            // 
            // lblTiempoActual
            // 
            this.lblTiempoActual.AutoSize = true;
            this.lblTiempoActual.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTiempoActual.Location = new System.Drawing.Point(12, 405);
            this.lblTiempoActual.Name = "lblTiempoActual";
            this.lblTiempoActual.Size = new System.Drawing.Size(44, 19);
            this.lblTiempoActual.TabIndex = 9;
            this.lblTiempoActual.Text = "00:00";
            // 
            // lblTiempoTotal
            // 
            this.lblTiempoTotal.AutoSize = true;
            this.lblTiempoTotal.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTiempoTotal.Location = new System.Drawing.Point(638, 405);
            this.lblTiempoTotal.Name = "lblTiempoTotal";
            this.lblTiempoTotal.Size = new System.Drawing.Size(44, 19);
            this.lblTiempoTotal.TabIndex = 10;
            this.lblTiempoTotal.Text = "00:00";
            // 
            // lblVolumen
            // 
            this.lblVolumen.AutoSize = true;
            this.lblVolumen.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblVolumen.Location = new System.Drawing.Point(12, 435);
            this.lblVolumen.Name = "lblVolumen";
            this.lblVolumen.Size = new System.Drawing.Size(73, 19);
            this.lblVolumen.TabIndex = 11;
            this.lblVolumen.Text = "Volumen:";
            // 
            // tbVolumen
            // 
            this.tbVolumen.Location = new System.Drawing.Point(90, 430);
            this.tbVolumen.Maximum = 100;
            this.tbVolumen.Name = "tbVolumen";
            this.tbVolumen.Size = new System.Drawing.Size(250, 45);
            this.tbVolumen.TabIndex = 12;
            this.tbVolumen.Value = 50;
            this.tbVolumen.Scroll += new System.EventHandler(this.tbVolumen_Scroll);
            // 
            // btnGuardarPlaylist
            // 
            this.btnGuardarPlaylist.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnGuardarPlaylist.Location = new System.Drawing.Point(12, 485);
            this.btnGuardarPlaylist.Name = "btnGuardarPlaylist";
            this.btnGuardarPlaylist.Size = new System.Drawing.Size(150, 40);
            this.btnGuardarPlaylist.TabIndex = 13;
            this.btnGuardarPlaylist.Text = "Guardar Playlist 💾";
            this.btnGuardarPlaylist.UseVisualStyleBackColor = true;
            this.btnGuardarPlaylist.Click += new System.EventHandler(this.btnGuardarPlaylist_Click);
            // 
            // btnCargarPlaylist
            // 
            this.btnCargarPlaylist.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnCargarPlaylist.Location = new System.Drawing.Point(180, 485);
            this.btnCargarPlaylist.Name = "btnCargarPlaylist";
            this.btnCargarPlaylist.Size = new System.Drawing.Size(150, 40);
            this.btnCargarPlaylist.TabIndex = 14;
            this.btnCargarPlaylist.Text = "Cargar Playlist 📂";
            this.btnCargarPlaylist.UseVisualStyleBackColor = true;
            this.btnCargarPlaylist.Click += new System.EventHandler(this.btnCargarPlaylist_Click);
            // 
            // btnAbrirWMP
            // 
            this.btnAbrirWMP.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnAbrirWMP.Location = new System.Drawing.Point(350, 485);
            this.btnAbrirWMP.Name = "btnAbrirWMP";
            this.btnAbrirWMP.Size = new System.Drawing.Size(150, 40);
            this.btnAbrirWMP.TabIndex = 15;
            this.btnAbrirWMP.Text = "Abrir en WMP 🎵";
            this.btnAbrirWMP.UseVisualStyleBackColor = true;
            this.btnAbrirWMP.Click += new System.EventHandler(this.btnAbrirWMP_Click);
            // 
            // btnLetra
            // 
            this.btnLetra.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnLetra.Location = new System.Drawing.Point(512, 485);
            this.btnLetra.Name = "btnLetra";
            this.btnLetra.Size = new System.Drawing.Size(170, 40);
            this.btnLetra.TabIndex = 16;
            this.btnLetra.Text = "Buscar Letra 🎤";
            this.btnLetra.UseVisualStyleBackColor = true;
            this.btnLetra.Click += new System.EventHandler(this.btnLetra_Click);
            // 
            // pbCover
            // 
            this.pbCover.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbCover.Location = new System.Drawing.Point(700, 80);
            this.pbCover.Name = "pbCover";
            this.pbCover.Size = new System.Drawing.Size(250, 250);
            this.pbCover.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbCover.TabIndex = 17;
            this.pbCover.TabStop = false;
            // 
            // lblCover
            // 
            this.lblCover.AutoSize = true;
            this.lblCover.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblCover.Location = new System.Drawing.Point(700, 57);
            this.lblCover.Name = "lblCover";
            this.lblCover.Size = new System.Drawing.Size(74, 20);
            this.lblCover.TabIndex = 18;
            this.lblCover.Text = "Portada:";
            // 
            // lblLyrics
            // 
            this.lblLyrics.AutoSize = true;
            this.lblLyrics.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblLyrics.Location = new System.Drawing.Point(700, 340);
            this.lblLyrics.Name = "lblLyrics";
            this.lblLyrics.Size = new System.Drawing.Size(51, 20);
            this.lblLyrics.TabIndex = 19;
            this.lblLyrics.Text = "Letra:";
            // 
            // rtbLyrics
            // 
            this.rtbLyrics.Location = new System.Drawing.Point(700, 363);
            this.rtbLyrics.Name = "rtbLyrics";
            this.rtbLyrics.Size = new System.Drawing.Size(250, 162);
            this.rtbLyrics.TabIndex = 20;
            this.rtbLyrics.Text = "";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(970, 545);
            this.Controls.Add(this.rtbLyrics);
            this.Controls.Add(this.lblLyrics);
            this.Controls.Add(this.lblCover);
            this.Controls.Add(this.pbCover);
            this.Controls.Add(this.btnLetra);
            this.Controls.Add(this.btnAbrirWMP);
            this.Controls.Add(this.btnCargarPlaylist);
            this.Controls.Add(this.btnGuardarPlaylist);
            this.Controls.Add(this.tbVolumen);
            this.Controls.Add(this.lblVolumen);
            this.Controls.Add(this.lblTiempoTotal);
            this.Controls.Add(this.lblTiempoActual);
            this.Controls.Add(this.tbProgreso);
            this.Controls.Add(this.lblCanciones);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.btnAbrirCarpeta);
            this.Controls.Add(this.dgvCanciones);
            this.Name = "Form1";
            this.Text = "Reproductor MP3 - Proyecto";
            ((System.ComponentModel.ISupportInitialize)(this.dgvCanciones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCover)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbProgreso)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbVolumen)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}

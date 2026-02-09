namespace Stopify
{
    partial class PortadaStopify
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.Button btnStopify;
        private System.Windows.Forms.Label lblTitulo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PortadaStopify));
            pbLogo = new PictureBox();
            btnStopify = new Button();
            lblTitulo = new Label();
            ((System.ComponentModel.ISupportInitialize)pbLogo).BeginInit();
            SuspendLayout();
            // 
            // pbLogo
            // 
            pbLogo.BackColor = Color.Transparent;
            pbLogo.BackgroundImage = (Image)resources.GetObject("pbLogo.BackgroundImage");
            pbLogo.BackgroundImageLayout = ImageLayout.Zoom;
            pbLogo.Location = new Point(219, 80);
            pbLogo.Margin = new Padding(4, 3, 4, 3);
            pbLogo.Name = "pbLogo";
            pbLogo.Size = new Size(256, 254);
            pbLogo.SizeMode = PictureBoxSizeMode.Zoom;
            pbLogo.TabIndex = 0;
            pbLogo.TabStop = false;
            // 
            // btnStopify
            // 
            btnStopify.BackColor = Color.Black;
            btnStopify.FlatAppearance.BorderSize = 0;
            btnStopify.FlatStyle = FlatStyle.Flat;
            btnStopify.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnStopify.ForeColor = Color.LawnGreen;
            btnStopify.Location = new Point(242, 375);
            btnStopify.Margin = new Padding(4, 3, 4, 3);
            btnStopify.Name = "btnStopify";
            btnStopify.Size = new Size(210, 52);
            btnStopify.TabIndex = 1;
            btnStopify.Text = "ENTRAR";
            btnStopify.UseVisualStyleBackColor = false;
            btnStopify.Click += btnStopify_Click;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.BackColor = Color.Transparent;
            lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.LawnGreen;
            lblTitulo.Location = new Point(224, 23);
            lblTitulo.Margin = new Padding(4, 0, 4, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(247, 32);
            lblTitulo.TabIndex = 2;
            lblTitulo.Text = "Stopify Music Player";
            // 
            // PortadaStopify
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(700, 462);
            Controls.Add(lblTitulo);
            Controls.Add(btnStopify);
            Controls.Add(pbLogo);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4, 3, 4, 3);
            Name = "PortadaStopify";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SplashForm";
            ((System.ComponentModel.ISupportInitialize)pbLogo).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }
    }
}

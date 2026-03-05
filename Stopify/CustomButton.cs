using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Stopify
{
    /// <summary>
    /// Botón personalizado con esquinas redondeadas y efecto hover,
    /// diseñado para combinarse con el tema oscuro de Stopify.
    /// </summary>
    public class CustomButton : Control
    {
        // ── PROPIEDADES ───────────────────────────────────────────────────────
        private bool _isHover = false;
        private bool _isPressed = false;
        private int _radius = 8;

        /// <summary>Radio de las esquinas redondeadas (default 8).</summary>
        public int CornerRadius
        {
            get => _radius;
            set { _radius = value; Invalidate(); }
        }

        /// <summary>Color de fondo normal.</summary>
        public Color NormalColor { get; set; } = Color.FromArgb(40, 40, 40);

        /// <summary>Color de fondo al pasar el mouse.</summary>
        public Color HoverColor { get; set; } = Color.FromArgb(65, 65, 65);

        /// <summary>Color de fondo al hacer click.</summary>
        public Color PressColor { get; set; } = Color.FromArgb(20, 20, 20);

        /// <summary>Color del borde.</summary>
        public Color BorderColor { get; set; } = Color.FromArgb(75, 75, 75);

        /// <summary>Grosor del borde (0 = sin borde).</summary>
        public int BorderWidth { get; set; } = 1;

        // ── CONSTRUCTOR ───────────────────────────────────────────────────────
        public CustomButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.StandardClick |
                     ControlStyles.StandardDoubleClick, true);

            ForeColor = Color.White;
            Font = new Font("Segoe UI", 9.5f);
            Cursor = Cursors.Hand;
            Height = 46;
        }

        // ── FONDO (transparencia igual que CustomTrackBar) ────────────────────
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (Parent == null) { base.OnPaintBackground(e); return; }
            e.Graphics.TranslateTransform(-Left, -Top);
            using (var pe = new PaintEventArgs(e.Graphics,
                new Rectangle(Left, Top, Width, Height)))
                InvokePaintBackground(Parent, pe);
            e.Graphics.TranslateTransform(Left, Top);
        }

        // ── DIBUJO ────────────────────────────────────────────────────────────
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Color bg = _isPressed ? PressColor
                     : _isHover ? HoverColor
                     : NormalColor;

            using (var path = RoundedRect(new Rectangle(0, 0, Width - 1, Height - 1), _radius))
            {
                // Fondo
                using (var brush = new SolidBrush(bg))
                    g.FillPath(brush, path);

                // Borde
                if (BorderWidth > 0)
                {
                    using (var pen = new Pen(BorderColor, BorderWidth))
                        g.DrawPath(pen, path);
                }

                // Sutil degradado superior (sensación de profundidad)
                using (var grad = new LinearGradientBrush(
                    new Point(0, 0), new Point(0, Height / 3),
                    Color.FromArgb(30, Color.White), Color.Transparent))
                    g.FillPath(grad, path);
            }

            // Texto + icono centrado
            TextRenderer.DrawText(g, Text, Font,
                new Rectangle(0, 0, Width, Height),
                ForeColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                TextFormatFlags.SingleLine);
        }

        // ── MOUSE ─────────────────────────────────────────────────────────────
        protected override void OnMouseEnter(EventArgs e)
        {
            _isHover = true; Invalidate();
            base.OnMouseEnter(e);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            _isHover = false; _isPressed = false; Invalidate();
            base.OnMouseLeave(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) { _isPressed = true; Invalidate(); }
            base.OnMouseDown(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            _isPressed = false; Invalidate();
            base.OnMouseUp(e);
        }

        // ── TECLADO (Enter/Space dispara Click) ───────────────────────────────
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space)
                OnClick(EventArgs.Empty);
            base.OnKeyDown(e);
        }

        // ── HELPER ───────────────────────────────────────────────────────────
        private GraphicsPath RoundedRect(Rectangle r, int radius)
        {
            int d = radius * 2;
            var path = new GraphicsPath();
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Stopify
{
    public class CustomTrackBar : Control
    {
        // ── PROPIEDADES ───────────────────────────────────────────────────────
        private int _minimum = 0;
        private int _maximum = 100;
        private int _value = 0;
        private bool _isDragging = false;

        public int Minimum
        {
            get => _minimum;
            set { _minimum = value; Invalidate(); }
        }

        public int Maximum
        {
            get => _maximum;
            set { _maximum = value; Invalidate(); }
        }

        public int Value
        {
            get => _value;
            set
            {
                _value = Math.Max(_minimum, Math.Min(_maximum, value));
                Invalidate();
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public Color TrackColor { get; set; } = Color.FromArgb(80, 80, 80);
        public Color FillColor { get; set; } = Color.LimeGreen;
        public Color ThumbColor { get; set; } = Color.White;
        public Color ThumbHoverColor { get; set; } = Color.LimeGreen;

        private bool _isHovering = false;

        public event EventHandler ValueChanged;

        // ── CONSTRUCTOR ───────────────────────────────────────────────────────
        public CustomTrackBar()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);

            Height = 22;
            Cursor = Cursors.Hand;
        }

        // Simula transparencia pintando SOLO el fondo del padre (sin sus controles hijos)
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (Parent == null) { base.OnPaintBackground(e); return; }

            e.Graphics.TranslateTransform(-Left, -Top);
            using (var pe = new PaintEventArgs(e.Graphics,
                new Rectangle(Left, Top, Width, Height)))
            {
                // Solo fondo del padre — NO InvokePaint para no sobreescribir labels hermanos
                InvokePaintBackground(Parent, pe);
            }
            e.Graphics.TranslateTransform(Left, Top);
        }

        // ── DIBUJO ────────────────────────────────────────────────────────────
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int trackH = 4;
            int trackY = (Height - trackH) / 2;
            int thumbR = _isHovering ? 8 : 6;
            int trackLeft = thumbR + 2;
            int trackRight = Width - thumbR - 2;
            int trackW = trackRight - trackLeft;

            double ratio = (_maximum > _minimum)
                ? (double)(_value - _minimum) / (_maximum - _minimum)
                : 0;
            int thumbX = trackLeft + (int)(ratio * trackW);

            // 1. Track fondo
            using (var path = RoundedRect(new Rectangle(trackLeft, trackY, trackW, trackH), 2))
            using (var brush = new SolidBrush(TrackColor))
                g.FillPath(brush, path);

            // 2. Track relleno (verde)
            if (thumbX > trackLeft)
            {
                using (var path = RoundedRect(new Rectangle(trackLeft, trackY, thumbX - trackLeft, trackH), 2))
                using (var brush = new SolidBrush(FillColor))
                    g.FillPath(brush, path);
            }

            // 3. Sombra del thumb
            int tx = thumbX - thumbR;
            int ty = (Height / 2) - thumbR;
            using (var brush = new SolidBrush(Color.FromArgb(60, 0, 0, 0)))
                g.FillEllipse(brush, tx + 1, ty + 1, thumbR * 2, thumbR * 2);

            // 4. Thumb
            using (var brush = new SolidBrush(_isHovering ? ThumbHoverColor : ThumbColor))
                g.FillEllipse(brush, tx, ty, thumbR * 2, thumbR * 2);
        }

        // ── INTERACCIÓN ───────────────────────────────────────────────────────
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) { _isDragging = true; SetValueFromX(e.X); }
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            _isHovering = true;
            if (_isDragging) SetValueFromX(e.X);
            Invalidate();
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _isDragging = false;
            base.OnMouseUp(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _isHovering = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        private void SetValueFromX(int x)
        {
            int thumbR = 8;
            int trackLeft = thumbR + 2;
            int trackRight = Width - thumbR - 2;
            double ratio = (double)(x - trackLeft) / (trackRight - trackLeft);
            Value = _minimum + (int)(Math.Max(0, Math.Min(1, ratio)) * (_maximum - _minimum));
        }

        private GraphicsPath RoundedRect(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
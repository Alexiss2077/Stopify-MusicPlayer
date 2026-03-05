using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Stopify
{
    /// <summary>
    /// Scrollbar vertical personalizado con tema oscuro para Stopify.
    /// Reemplaza el VScrollBar estándar del DataGridView mediante
    /// un panel superpuesto al borde derecho.
    /// </summary>
    public class CustomScrollBar : Control
    {
        // ── COLORES ────────────────────────────────────────────────────────────
        public Color TrackColor { get; set; } = Color.FromArgb(28, 28, 28);
        public Color ThumbColor { get; set; } = Color.FromArgb(80, 80, 80);
        public Color ThumbHover { get; set; } = Color.FromArgb(120, 120, 120);
        public Color ThumbActive { get; set; } = Color.LimeGreen;

        // ── ESTADO ─────────────────────────────────────────────────────────────
        private int _minimum = 0;
        private int _maximum = 100;
        private int _value = 0;
        private int _largeChange = 10;

        private bool _hover = false;
        private bool _drag = false;
        private int _dragStartY;
        private int _dragStartVal;

        public int Minimum { get => _minimum; set { _minimum = value; Invalidate(); } }
        public int Maximum { get => _maximum; set { _maximum = value; Invalidate(); } }
        public int LargeChange { get => _largeChange; set { _largeChange = value; Invalidate(); } }

        public int Value
        {
            get => _value;
            set
            {
                _value = Math.Max(_minimum, Math.Min(_maximum - _largeChange, value));
                Invalidate();
                Scroll?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler Scroll;

        // ── CONSTRUCTOR ───────────────────────────────────────────────────────
        public CustomScrollBar()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);
            Width = 8;
            Cursor = Cursors.Default;
        }

        // Fondo transparente (igual que los demás controles custom)
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (Parent == null) { base.OnPaintBackground(e); return; }
            e.Graphics.TranslateTransform(-Left, -Top);
            using (var pe = new PaintEventArgs(e.Graphics, new Rectangle(Left, Top, Width, Height)))
                InvokePaintBackground(Parent, pe);
            e.Graphics.TranslateTransform(Left, Top);
        }

        // ── DIBUJO ────────────────────────────────────────────────────────────
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Track
            using (var b = new SolidBrush(TrackColor))
                g.FillRoundedRectangle(b, new Rectangle(0, 0, Width, Height), 4);

            // Thumb
            var (ty, th) = GetThumbRect();
            Color tc = _drag ? ThumbActive : _hover ? ThumbHover : ThumbColor;

            using (var b = new SolidBrush(tc))
                g.FillRoundedRectangle(b, new Rectangle(1, ty, Width - 2, th), 3);
        }

        private (int top, int height) GetThumbRect()
        {
            int range = _maximum - _minimum;
            if (range <= 0) return (0, Height);
            float ratio = (float)_largeChange / range;
            int th = Math.Max(20, (int)(Height * ratio));
            float pos = (float)(_value - _minimum) / (range - _largeChange);
            int ty = (int)(pos * (Height - th));
            return (Math.Max(0, Math.Min(Height - th, ty)), th);
        }

        // ── INTERACCIÓN ───────────────────────────────────────────────────────
        protected override void OnMouseEnter(EventArgs e)
        { _hover = true; Invalidate(); base.OnMouseEnter(e); }

        protected override void OnMouseLeave(EventArgs e)
        { _hover = false; Invalidate(); base.OnMouseLeave(e); }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            var (ty, th) = GetThumbRect();
            if (e.Y >= ty && e.Y <= ty + th)
            {
                _drag = true; _dragStartY = e.Y; _dragStartVal = _value;
                Capture = true;
            }
            else
            {
                // Click en el track → saltar
                Value = e.Y < ty
                    ? Value - _largeChange
                    : Value + _largeChange;
            }
            Invalidate();
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_drag)
            {
                int range = _maximum - _minimum - _largeChange;
                var (_, th) = GetThumbRect();
                int trackH = Height - th;
                if (trackH <= 0) return;
                int delta = e.Y - _dragStartY;
                Value = _dragStartVal + (int)((float)delta / trackH * range);
                Invalidate();
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        { _drag = false; Capture = false; Invalidate(); base.OnMouseUp(e); }

        protected override void OnMouseWheel(MouseEventArgs e)
        { Value += e.Delta < 0 ? _largeChange / 2 : -_largeChange / 2; base.OnMouseWheel(e); }
    }

    // ── EXTENSIÓN: FillRoundedRectangle ──────────────────────────────────────
    internal static class GraphicsExtensions
    {
        public static void FillRoundedRectangle(this Graphics g, Brush brush, Rectangle rect, int radius)
        {
            using var path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            g.FillPath(brush, path);
        }
    }
}
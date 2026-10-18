using Microsoft.Maui.Graphics;

namespace Dalla.Controls;

/// <summary>Couleurs dépendantes du thème pour les graphiques dessinés à la main.</summary>
static class ChartTheme
{
	public static bool IsDark => Application.Current?.RequestedTheme == AppTheme.Dark;
	public static Color Text => IsDark ? Color.FromArgb("#F1F5F9") : Color.FromArgb("#1F2937");
	public static Color Text2 => IsDark ? Color.FromArgb("#94A3B8") : Color.FromArgb("#6B7280");
	public static Color Grid => IsDark ? Color.FromArgb("#334155") : Color.FromArgb("#E5E7EB");
	public static Color Card => IsDark ? Color.FromArgb("#1E293B") : Color.FromArgb("#FFFFFF");
	public static Color Primary => IsDark ? Color.FromArgb("#3A74C4") : Color.FromArgb("#1E5AA8");

	public static string Short(double v) => v >= 1000 ? $"{Math.Round(v / 1000):0}k" : $"{v:0}";
}

/// <summary>Diagramme en anneau (répartition par catégorie).</summary>
public class DonutDrawable : IDrawable
{
	public List<(double Value, Color Color)> Slices { get; set; } = new();
	public string CenterTop { get; set; } = "";
	public string CenterBottom { get; set; } = "";

	public void Draw(ICanvas canvas, RectF rect)
	{
		float cx = rect.Center.X, cy = rect.Center.Y;
		float outer = Math.Min(rect.Width, rect.Height) / 2f - 4f;
		float inner = outer * 0.62f;
		double total = Slices.Sum(s => s.Value);

		if (total <= 0)
		{
			Sector(canvas, cx, cy, outer, inner, -90, 359.9, ChartTheme.Grid);
		}
		else
		{
			double start = -90;
			foreach (var s in Slices.Where(s => s.Value > 0))
			{
				double sweep = s.Value / total * 360.0;
				Sector(canvas, cx, cy, outer, inner, start, sweep, s.Color);
				start += sweep;
			}
		}

		canvas.Font = Microsoft.Maui.Graphics.Font.DefaultBold;
		canvas.FontColor = ChartTheme.Text;
		canvas.FontSize = 15;
		canvas.DrawString(CenterTop, cx - inner, cy - 20, inner * 2, 22, HorizontalAlignment.Center, VerticalAlignment.Center);
		canvas.Font = Microsoft.Maui.Graphics.Font.Default;
		canvas.FontColor = ChartTheme.Text2;
		canvas.FontSize = 12;
		canvas.DrawString(CenterBottom, cx - inner, cy + 2, inner * 2, 18, HorizontalAlignment.Center, VerticalAlignment.Center);
	}

	static void Sector(ICanvas canvas, float cx, float cy, float outer, float inner, double startDeg, double sweepDeg, Color color)
	{
		var path = new PathF();
		int steps = Math.Max(2, (int)Math.Ceiling(sweepDeg / 3.0));
		for (int i = 0; i <= steps; i++)
		{
			double a = (startDeg + sweepDeg * i / steps) * Math.PI / 180.0;
			float x = cx + outer * (float)Math.Cos(a), y = cy + outer * (float)Math.Sin(a);
			if (i == 0) path.MoveTo(x, y); else path.LineTo(x, y);
		}
		for (int i = steps; i >= 0; i--)
		{
			double a = (startDeg + sweepDeg * i / steps) * Math.PI / 180.0;
			path.LineTo(cx + inner * (float)Math.Cos(a), cy + inner * (float)Math.Sin(a));
		}
		path.Close();
		canvas.FillColor = color;
		canvas.FillPath(path);
		canvas.StrokeColor = ChartTheme.Card;
		canvas.StrokeSize = 2;
		canvas.DrawPath(path);
	}
}

/// <summary>Histogramme des dépenses par membre.</summary>
public class BarsDrawable : IDrawable
{
	public List<(string Label, double Value, Color Color)> Bars { get; set; } = new();

	public void Draw(ICanvas canvas, RectF rect)
	{
		if (Bars.Count == 0) return;
		float top = 22, bottom = 24;
		float h = rect.Height - top - bottom;
		double max = Math.Max(1, Bars.Max(b => b.Value));
		float slot = rect.Width / Bars.Count;
		float barW = Math.Min(44, slot * 0.55f);

		canvas.StrokeColor = ChartTheme.Grid;
		canvas.StrokeSize = 1;
		canvas.DrawLine(0, top + h, rect.Width, top + h);

		for (int i = 0; i < Bars.Count; i++)
		{
			var b = Bars[i];
			float bh = (float)(b.Value / max) * h;
			float x = i * slot + (slot - barW) / 2f;
			float y = top + h - bh;
			canvas.FillColor = b.Color;
			canvas.FillRoundedRectangle(x, y, barW, Math.Max(bh, 2), 6);

			canvas.FontColor = ChartTheme.Text;
			canvas.FontSize = 12;
			canvas.DrawString(ChartTheme.Short(b.Value), i * slot, y - 20, slot, 18, HorizontalAlignment.Center, VerticalAlignment.Center);
			canvas.FontColor = ChartTheme.Text2;
			canvas.DrawString(b.Label, i * slot, top + h + 4, slot, 18, HorizontalAlignment.Center, VerticalAlignment.Center);
		}
	}
}

/// <summary>Courbe d'évolution sur 6 mois.</summary>
public class LineDrawable : IDrawable
{
	public List<(string Label, double Value)> Points { get; set; } = new();

	public void Draw(ICanvas canvas, RectF rect)
	{
		if (Points.Count < 2) return;
		float left = 8, right = 8, top = 26, bottom = 24;
		float w = rect.Width - left - right;
		float h = rect.Height - top - bottom;
		double max = Math.Max(1, Points.Max(p => p.Value)) * 1.1;

		canvas.StrokeColor = ChartTheme.Grid;
		canvas.StrokeSize = 1;
		for (int g = 0; g <= 3; g++)
		{
			float gy = top + h * g / 3f;
			canvas.DrawLine(left, gy, left + w, gy);
		}

		var pts = new List<PointF>();
		for (int i = 0; i < Points.Count; i++)
		{
			float x = left + w * i / (Points.Count - 1);
			float y = top + h - (float)(Points[i].Value / max) * h;
			pts.Add(new PointF(x, y));
		}

		canvas.StrokeColor = ChartTheme.Primary;
		canvas.StrokeSize = 3;
		canvas.StrokeLineCap = LineCap.Round;
		canvas.StrokeLineJoin = LineJoin.Round;
		var path = new PathF();
		path.MoveTo(pts[0]);
		for (int i = 1; i < pts.Count; i++) path.LineTo(pts[i]);
		canvas.DrawPath(path);

		for (int i = 0; i < pts.Count; i++)
		{
			canvas.FillColor = ChartTheme.Card;
			canvas.FillCircle(pts[i], 5);
			canvas.StrokeColor = ChartTheme.Primary;
			canvas.StrokeSize = 2.5f;
			canvas.DrawCircle(pts[i], 5);

			canvas.FontSize = 12;
			canvas.FontColor = ChartTheme.Text;
			canvas.DrawString(ChartTheme.Short(Points[i].Value), pts[i].X - 24, pts[i].Y - 24, 48, 16, HorizontalAlignment.Center, VerticalAlignment.Center);
			canvas.FontColor = ChartTheme.Text2;
			canvas.DrawString(Points[i].Label, pts[i].X - 24, top + h + 6, 48, 16, HorizontalAlignment.Center, VerticalAlignment.Center);
		}
	}
}

/// <summary>Logo Dalla : une maison associée à une pièce (version couleur ou monochrome).</summary>
public class LogoDrawable : IDrawable
{
	public bool Monochrome { get; set; }
	public Color? MonoColor { get; set; }

	public void Draw(ICanvas canvas, RectF rect)
	{
		float s = Math.Min(rect.Width, rect.Height);
		float ox = rect.Center.X - s / 2f, oy = rect.Center.Y - s / 2f;
		float U(float v) => v * s / 100f;
		var main = Monochrome ? (MonoColor ?? ChartTheme.Text) : ChartTheme.Primary;
		var coin = Monochrome ? main : Color.FromArgb("#F5A623");

		var house = new PathF();
		house.MoveTo(ox + U(16), oy + U(48));
		house.LineTo(ox + U(50), oy + U(18));
		house.LineTo(ox + U(84), oy + U(48));
		house.LineTo(ox + U(84), oy + U(82));
		house.LineTo(ox + U(16), oy + U(82));
		house.Close();

		canvas.StrokeColor = main;
		canvas.StrokeSize = U(7);
		canvas.StrokeLineJoin = LineJoin.Round;
		canvas.StrokeLineCap = LineCap.Round;
		canvas.DrawPath(house);

		canvas.FillColor = coin;
		canvas.FillCircle(ox + U(50), oy + U(58), U(14));
		canvas.StrokeColor = Monochrome ? Colors.Transparent : ChartTheme.Card;
		canvas.StrokeSize = U(3);
		canvas.DrawCircle(ox + U(50), oy + U(58), U(14));
	}
}

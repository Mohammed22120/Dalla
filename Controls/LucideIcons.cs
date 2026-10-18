using System.Globalization;
using Microsoft.Maui.Graphics;

namespace Dalla.Controls;

/// <summary>
/// Jeu d'icônes Lucide (https://lucide.dev, licence ISC) embarqué dans le code.
/// Chaque icône est définie sur une grille 24 × 24, trait de 2 px, extrémités arrondies,
/// exactement comme les SVG officiels. Dessinées en vectoriel : aucune image, aucun package,
/// teinte libre (clair / sombre) → cohérent avec l'écoconception du projet.
/// Syntaxe : "p:" chemin SVG, "c:cx,cy,r" cercle, "r:x,y,w,h,rx" rectangle arrondi.
/// Pour ajouter une icône : copier les éléments du SVG Lucide dans ce dictionnaire.
/// </summary>
public static class LucideIcons
{
	static readonly Dictionary<string, string[]> Map = new()
	{
		// Navigation
		["house"] = new[] {
			"p:M15 21v-8a1 1 0 0 0-1-1h-4a1 1 0 0 0-1 1v8",
			"p:M3 10a2 2 0 0 1 .709-1.528l7-5.999a2 2 0 0 1 2.582 0l7 5.999A2 2 0 0 1 21 10v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z" },
		["receipt"] = new[] {
			"p:M4 2v20l2-1 2 1 2-1 2 1 2-1 2 1 2-1 2 1V2l-2 1-2-1-2 1-2-1-2 1-2-1-2 1Z",
			"p:M16 8h-6a2 2 0 1 0 0 4h4a2 2 0 1 1 0 4H8",
			"p:M12 17.5v-11" },
		["chart-column"] = new[] {
			"p:M3 3v16a2 2 0 0 0 2 2h16",
			"p:M18 17V9",
			"p:M13 17V5",
			"p:M8 17v-3" },
		["users"] = new[] {
			"p:M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2",
			"c:9,7,4",
			"p:M22 21v-2a4 4 0 0 0-3-3.87",
			"p:M16 3.13a4 4 0 0 1 0 7.75" },
		["plus"] = new[] { "p:M5 12h14", "p:M12 5v14" },

		// Actions et flèches
		["arrow-left"] = new[] { "p:m12 19-7-7 7-7", "p:M19 12H5" },
		["chevron-left"] = new[] { "p:m15 18-6-6 6-6" },
		["chevron-right"] = new[] { "p:m9 18 6-6-6-6" },
		["search"] = new[] { "p:m21 21-4.34-4.34", "c:11,11,8" },

		// Menu Foyer
		["wallet"] = new[] {
			"p:M19 7V4a1 1 0 0 0-1-1H5a2 2 0 0 0 0 4h15a1 1 0 0 1 1 1v4h-3a2 2 0 0 0 0 4h3a1 1 0 0 0 1-1v-2a1 1 0 0 0-1-1",
			"p:M3 5v14a2 2 0 0 0 2 2h15a1 1 0 0 0 1-1v-4" },
		["tag"] = new[] {
			"p:M12.586 2.586A2 2 0 0 0 11.172 2H4a2 2 0 0 0-2 2v7.172a2 2 0 0 0 .586 1.414l8.704 8.704a2.426 2.426 0 0 0 3.42 0l6.58-6.58a2.426 2.426 0 0 0 0-3.42z",
			"c:7.5,7.5,0.5" },
		["settings"] = new[] {
			"p:M9.671 4.136a2.34 2.34 0 0 1 4.659 0 2.34 2.34 0 0 0 3.319 1.915 2.34 2.34 0 0 1 2.33 4.033 2.34 2.34 0 0 0 0 3.831 2.34 2.34 0 0 1-2.33 4.033 2.34 2.34 0 0 0-3.319 1.915 2.34 2.34 0 0 1-4.659 0 2.34 2.34 0 0 0-3.32-1.915 2.34 2.34 0 0 1-2.33-4.033 2.34 2.34 0 0 0 0-3.831A2.34 2.34 0 0 1 6.35 6.051a2.34 2.34 0 0 0 3.319-1.915",
			"c:12,12,3" },

		// Catégories de dépenses
		["shopping-cart"] = new[] {
			"c:8,21,1", "c:19,21,1",
			"p:M2.05 2.05h2l2.66 12.42a2 2 0 0 0 2 1.58h9.78a2 2 0 0 0 1.95-1.57l1.65-7.43H5.12" },
		["bus"] = new[] {
			"p:M8 6v6", "p:M15 6v6", "p:M2 12h19.6",
			"p:M18 18h3s.5-1.7.8-2.8c.1-.4.2-.8.2-1.2 0-.4-.1-.8-.2-1.2l-1.4-5C20.1 6.8 19.1 6 18 6H4a2 2 0 0 0-2 2v10h3",
			"c:7,18,2", "p:M9 18h5", "c:16,18,2" },
		["heart-pulse"] = new[] {
			"p:M19 14c1.49-1.46 3-3.21 3-5.5A5.5 5.5 0 0 0 16.5 3c-1.76 0-3 .5-4.5 2-1.5-1.5-2.74-2-4.5-2A5.5 5.5 0 0 0 2 8.5c0 2.3 1.5 4.05 3 5.5l7 7Z",
			"p:M3.22 12H9.5l.5-1 2 4.5 2-7 1.5 3.5h5.27" },
		["ticket"] = new[] {
			"p:M2 9a3 3 0 0 1 0 6v2a2 2 0 0 0 2 2h16a2 2 0 0 0 2-2v-2a3 3 0 0 1 0-6V7a2 2 0 0 0-2-2H4a2 2 0 0 0-2 2Z",
			"p:M13 5v2", "p:M13 17v2", "p:M13 11v2" },
		["graduation-cap"] = new[] {
			"p:M21.42 10.922a1 1 0 0 0-.019-1.838L12.83 5.18a2 2 0 0 0-1.66 0L2.6 9.08a1 1 0 0 0 0 1.832l8.57 3.908a2 2 0 0 0 1.66 0z",
			"p:M22 10v6",
			"p:M6 12.5V16a6 3 0 0 0 12 0v-3.5" },
		["ellipsis"] = new[] { "c:12,12,1", "c:19,12,1", "c:5,12,1" },

		// États et messages
		["lock"] = new[] { "r:3,11,18,11,2", "p:M7 11V7a5 5 0 0 1 10 0v4" },
		["triangle-alert"] = new[] {
			"p:m21.73 18-8-14a2 2 0 0 0-3.48 0l-8 14A2 2 0 0 0 4 21h16a2 2 0 0 0 1.73-3",
			"p:M12 9v4", "p:M12 17h.01" },
		["circle-check"] = new[] { "c:12,12,10", "p:m9 12 2 2 4-4" },
		["wifi-off"] = new[] {
			"p:M12 20h.01", "p:M8.5 16.429a5 5 0 0 1 7 0", "p:M5 12.859a10 10 0 0 1 5.17-2.69",
			"p:M19 12.859a10 10 0 0 0-2.007-1.523", "p:M2 8.82a15 15 0 0 1 4.177-2.643",
			"p:M22 8.82a15 15 0 0 0-11.288-3.764", "p:m2 2 20 20" },
		["inbox"] = new[] {
			"p:M22 12h-6l-2 3h-4l-2-3H2",
			"p:M5.45 5.11 2 12v6a2 2 0 0 0 2 2h16a2 2 0 0 0 2-2v-6l-3.45-6.89A2 2 0 0 0 16.76 4H7.24a2 2 0 0 0-1.79 1.11z" },
	};

	public static bool Has(string name) => !string.IsNullOrEmpty(name) && Map.ContainsKey(name);

	/// <summary>Dessine l'icône dans le carré (x, y, size) avec la couleur et l'épaisseur voulues.</summary>
	public static void Draw(ICanvas canvas, string name, float x, float y, float size, Color color, float strokeWidth = 2f)
	{
		if (string.IsNullOrEmpty(name) || !Map.TryGetValue(name, out var parts)) return;

		float k = size / 24f;
		canvas.SaveState();
		canvas.StrokeColor = color;
		canvas.StrokeSize = strokeWidth * k;
		canvas.StrokeLineCap = LineCap.Round;
		canvas.StrokeLineJoin = LineJoin.Round;

		foreach (var part in parts)
		{
			var kind = part[0];
			var body = part.Substring(2);
			switch (kind)
			{
				case 'p':
					canvas.DrawPath(SvgPath.Parse(body, k, x, y));
					break;
				case 'c':
				{
					var v = Nums(body);
					canvas.DrawCircle(x + v[0] * k, y + v[1] * k, v[2] * k);
					break;
				}
				case 'r':
				{
					var v = Nums(body);
					canvas.DrawRoundedRectangle(x + v[0] * k, y + v[1] * k, v[2] * k, v[3] * k, v[4] * k);
					break;
				}
			}
		}
		canvas.RestoreState();
	}

	static float[] Nums(string s)
		=> s.Split(',').Select(t => float.Parse(t, CultureInfo.InvariantCulture)).ToArray();
}

/// <summary>Mini-parseur de chemins SVG (M L H V C S Q A Z, absolus et relatifs) → PathF mis à l'échelle.</summary>
static class SvgPath
{
	public static PathF Parse(string d, float k, float ox, float oy)
	{
		var path = new PathF();
		int i = 0;
		char cmd = ' ';
		float cx = 0, cy = 0;      // point courant (unités 24 × 24)
		float sx = 0, sy = 0;      // début du sous-chemin
		float lx = 0, ly = 0;      // dernier point de contrôle (pour S)
		char prev = ' ';

		PointF P(float x, float y) => new PointF(ox + x * k, oy + y * k);

		while (true)
		{
			SkipSep(d, ref i);
			if (i >= d.Length) break;

			if (char.IsLetter(d[i])) { cmd = d[i++]; }
			else if (cmd == ' ') break;
			else if (cmd == 'M') cmd = 'L';
			else if (cmd == 'm') cmd = 'l';

			bool rel = char.IsLower(cmd);
			switch (char.ToUpperInvariant(cmd))
			{
				case 'M':
				{
					float x = Num(d, ref i), y = Num(d, ref i);
					if (rel) { x += cx; y += cy; }
					cx = sx = x; cy = sy = y;
					path.MoveTo(P(cx, cy));
					break;
				}
				case 'L':
				{
					float x = Num(d, ref i), y = Num(d, ref i);
					if (rel) { x += cx; y += cy; }
					cx = x; cy = y;
					path.LineTo(P(cx, cy));
					break;
				}
				case 'H':
				{
					float x = Num(d, ref i);
					cx = rel ? cx + x : x;
					path.LineTo(P(cx, cy));
					break;
				}
				case 'V':
				{
					float y = Num(d, ref i);
					cy = rel ? cy + y : y;
					path.LineTo(P(cx, cy));
					break;
				}
				case 'C':
				{
					float x1 = Num(d, ref i), y1 = Num(d, ref i), x2 = Num(d, ref i), y2 = Num(d, ref i), x = Num(d, ref i), y = Num(d, ref i);
					if (rel) { x1 += cx; y1 += cy; x2 += cx; y2 += cy; x += cx; y += cy; }
					path.CurveTo(P(x1, y1), P(x2, y2), P(x, y));
					lx = x2; ly = y2; cx = x; cy = y;
					break;
				}
				case 'S':
				{
					float x2 = Num(d, ref i), y2 = Num(d, ref i), x = Num(d, ref i), y = Num(d, ref i);
					if (rel) { x2 += cx; y2 += cy; x += cx; y += cy; }
					float x1 = cx, y1 = cy;
					if (char.ToUpperInvariant(prev) is 'C' or 'S') { x1 = 2 * cx - lx; y1 = 2 * cy - ly; }
					path.CurveTo(P(x1, y1), P(x2, y2), P(x, y));
					lx = x2; ly = y2; cx = x; cy = y;
					break;
				}
				case 'Q':
				{
					float x1 = Num(d, ref i), y1 = Num(d, ref i), x = Num(d, ref i), y = Num(d, ref i);
					if (rel) { x1 += cx; y1 += cy; x += cx; y += cy; }
					path.QuadTo(P(x1, y1), P(x, y));
					cx = x; cy = y;
					break;
				}
				case 'A':
				{
					float rx = Num(d, ref i), ry = Num(d, ref i), rot = Num(d, ref i);
					bool large = Flag(d, ref i), sweep = Flag(d, ref i);
					float x = Num(d, ref i), y = Num(d, ref i);
					if (rel) { x += cx; y += cy; }
					Arc(path, cx, cy, rx, ry, rot, large, sweep, x, y, P);
					cx = x; cy = y;
					break;
				}
				case 'Z':
					path.Close();
					cx = sx; cy = sy;
					break;
				default:
					return path;
			}
			prev = cmd;
		}
		return path;
	}

	static void SkipSep(string s, ref int i)
	{
		while (i < s.Length && (s[i] == ' ' || s[i] == ',' || s[i] == '\t' || s[i] == '\n' || s[i] == '\r')) i++;
	}

	static float Num(string s, ref int i)
	{
		SkipSep(s, ref i);
		int start = i;
		if (i < s.Length && (s[i] == '-' || s[i] == '+')) i++;
		bool dot = false;
		while (i < s.Length)
		{
			char c = s[i];
			if (char.IsDigit(c)) i++;
			else if (c == '.' && !dot) { dot = true; i++; }
			else break;
		}
		return float.Parse(s.AsSpan(start, i - start), NumberStyles.Float, CultureInfo.InvariantCulture);
	}

	// Les indicateurs d'arc sont un seul caractère (0 ou 1), parfois collés au nombre suivant.
	static bool Flag(string s, ref int i)
	{
		SkipSep(s, ref i);
		return s[i++] == '1';
	}

	/// <summary>Arc SVG (conversion point d'arrivée → centre) approché par des courbes de Bézier cubiques.</summary>
	static void Arc(PathF path, float x1, float y1, float rx, float ry, float rotDeg, bool large, bool sweep,
		float x2, float y2, Func<float, float, PointF> P)
	{
		if (rx == 0 || ry == 0) { path.LineTo(P(x2, y2)); return; }
		if (x1 == x2 && y1 == y2) return;

		rx = Math.Abs(rx); ry = Math.Abs(ry);
		double phi = rotDeg * Math.PI / 180.0, cos = Math.Cos(phi), sin = Math.Sin(phi);
		double dx = (x1 - x2) / 2.0, dy = (y1 - y2) / 2.0;
		double x1p = cos * dx + sin * dy, y1p = -sin * dx + cos * dy;

		double lambda = (x1p * x1p) / (rx * rx) + (y1p * y1p) / (ry * ry);
		if (lambda > 1) { double sl = Math.Sqrt(lambda); rx = (float)(rx * sl); ry = (float)(ry * sl); }

		double num = rx * rx * ry * ry - rx * rx * y1p * y1p - ry * ry * x1p * x1p;
		double den = rx * rx * y1p * y1p + ry * ry * x1p * x1p;
		double coef = (large == sweep ? -1 : 1) * Math.Sqrt(Math.Max(0, num / den));
		double cxp = coef * (rx * y1p / ry), cyp = coef * -(ry * x1p / rx);
		double cx = cos * cxp - sin * cyp + (x1 + x2) / 2.0;
		double cy = sin * cxp + cos * cyp + (y1 + y2) / 2.0;

		double Angle(double ux, double uy, double vx, double vy)
		{
			double a = Math.Atan2(ux * vy - uy * vx, ux * vx + uy * vy);
			return a;
		}
		double theta1 = Angle(1, 0, (x1p - cxp) / rx, (y1p - cyp) / ry);
		double delta = Angle((x1p - cxp) / rx, (y1p - cyp) / ry, (-x1p - cxp) / rx, (-y1p - cyp) / ry);
		if (!sweep && delta > 0) delta -= 2 * Math.PI;
		else if (sweep && delta < 0) delta += 2 * Math.PI;

		int segs = (int)Math.Ceiling(Math.Abs(delta) / (Math.PI / 2) - 1e-9);
		segs = Math.Max(1, segs);
		double step = delta / segs;
		double t = 4.0 / 3.0 * Math.Tan(step / 4);

		for (int s = 0; s < segs; s++)
		{
			double a1 = theta1 + s * step, a2 = a1 + step;
			double c1 = Math.Cos(a1), s1 = Math.Sin(a1), c2 = Math.Cos(a2), s2 = Math.Sin(a2);

			(float, float) Pt(double ux, double uy)
			{
				double px = rx * ux, py = ry * uy;
				return ((float)(cos * px - sin * py + cx), (float)(sin * px + cos * py + cy));
			}
			var p1 = Pt(c1 - t * s1, s1 + t * c1);
			var p2 = Pt(c2 + t * s2, s2 - t * c2);
			var p3 = s == segs - 1 ? (x2, y2) : Pt(c2, s2);
			path.CurveTo(P(p1.Item1, p1.Item2), P(p2.Item1, p2.Item2), P(p3.Item1, p3.Item2));
		}
	}
}

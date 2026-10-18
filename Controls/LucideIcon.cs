using Microsoft.Maui.Graphics;

namespace Dalla.Controls;

/// <summary>
/// Icône Lucide teintable. Usage : &lt;controls:LucideIcon Name="house" Size="24" IconColor="White" /&gt;
/// Sans IconColor : couleur du texte du thème (clair / sombre). Muted="True" : texte secondaire.
/// </summary>
public class LucideIcon : GraphicsView
{
	public static readonly BindableProperty NameProperty = BindableProperty.Create(
		nameof(Name), typeof(string), typeof(LucideIcon), null,
		propertyChanged: (b, o, n) => ((LucideIcon)b).Invalidate());

	public static readonly BindableProperty IconColorProperty = BindableProperty.Create(
		nameof(IconColor), typeof(Color), typeof(LucideIcon), null,
		propertyChanged: (b, o, n) => ((LucideIcon)b).Invalidate());

	public static readonly BindableProperty MutedProperty = BindableProperty.Create(
		nameof(Muted), typeof(bool), typeof(LucideIcon), false,
		propertyChanged: (b, o, n) => ((LucideIcon)b).Invalidate());

	public static readonly BindableProperty SizeProperty = BindableProperty.Create(
		nameof(Size), typeof(double), typeof(LucideIcon), 24.0,
		propertyChanged: (b, o, n) => ((LucideIcon)b).ApplySize((double)n));

	public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create(
		nameof(StrokeWidth), typeof(double), typeof(LucideIcon), 2.0,
		propertyChanged: (b, o, n) => ((LucideIcon)b).Invalidate());

	public string Name { get => (string)GetValue(NameProperty); set => SetValue(NameProperty, value); }
	public Color IconColor { get => (Color)GetValue(IconColorProperty); set => SetValue(IconColorProperty, value); }
	public bool Muted { get => (bool)GetValue(MutedProperty); set => SetValue(MutedProperty, value); }
	public double Size { get => (double)GetValue(SizeProperty); set => SetValue(SizeProperty, value); }
	public double StrokeWidth { get => (double)GetValue(StrokeWidthProperty); set => SetValue(StrokeWidthProperty, value); }

	public LucideIcon()
	{
		Drawable = new IconDrawable(this);
		InputTransparent = true;               // les taps passent au parent (lignes, boutons…)
		HorizontalOptions = LayoutOptions.Center;
		VerticalOptions = LayoutOptions.Center;
		ApplySize(24);
	}

	void ApplySize(double size)
	{
		WidthRequest = size;
		HeightRequest = size;
		Invalidate();
	}

	protected override void OnHandlerChanged()
	{
		base.OnHandlerChanged();
		if (Application.Current != null)
		{
			Application.Current.RequestedThemeChanged -= OnThemeChanged;
			if (Handler != null) Application.Current.RequestedThemeChanged += OnThemeChanged;
		}
	}

	void OnThemeChanged(object sender, AppThemeChangedEventArgs e) => Invalidate();

	class IconDrawable : IDrawable
	{
		readonly LucideIcon _owner;
		public IconDrawable(LucideIcon owner) => _owner = owner;

		public void Draw(ICanvas canvas, RectF rect)
		{
			var color = _owner.IconColor ?? (_owner.Muted ? ChartTheme.Text2 : ChartTheme.Text);
			float size = Math.Min(rect.Width, rect.Height);
			float x = rect.Center.X - size / 2f, y = rect.Center.Y - size / 2f;
			LucideIcons.Draw(canvas, _owner.Name, x, y, size, color, (float)_owner.StrokeWidth);
		}
	}
}

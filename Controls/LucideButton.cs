namespace Dalla.Controls;

/// <summary>Bouton-icône (zone tactile 48 × 48) : retour, mois précédent / suivant, etc.</summary>
public class LucideButton : ContentView
{
	readonly LucideIcon _icon = new() { Size = 24 };

	public static readonly BindableProperty IconProperty = BindableProperty.Create(
		nameof(Icon), typeof(string), typeof(LucideButton), null,
		propertyChanged: (b, o, n) => ((LucideButton)b)._icon.Name = n as string);

	public static readonly BindableProperty IconColorProperty = BindableProperty.Create(
		nameof(IconColor), typeof(Color), typeof(LucideButton), null,
		propertyChanged: (b, o, n) => ((LucideButton)b)._icon.IconColor = n as Color);

	public string Icon { get => (string)GetValue(IconProperty); set => SetValue(IconProperty, value); }
	public Color IconColor { get => (Color)GetValue(IconColorProperty); set => SetValue(IconColorProperty, value); }

	public event EventHandler Clicked;

	public LucideButton()
	{
		WidthRequest = 48;
		HeightRequest = 48;
		HorizontalOptions = LayoutOptions.Start;
		VerticalOptions = LayoutOptions.Center;
		Content = _icon;

		var tap = new TapGestureRecognizer();
		tap.Tapped += (_, _) => Clicked?.Invoke(this, EventArgs.Empty);
		GestureRecognizers.Add(tap);
	}
}

namespace Dalla.Controls;

/// <summary>Avatar simple : initiale sur fond coloré (aucune image → écoconception).</summary>
public class AvatarView : Border
{
	readonly Label _label = new()
	{
		HorizontalOptions = LayoutOptions.Center,
		VerticalOptions = LayoutOptions.Center,
		TextColor = Colors.White,
		FontAttributes = FontAttributes.Bold,
	};

	public static readonly BindableProperty InitialProperty = BindableProperty.Create(
		nameof(Initial), typeof(string), typeof(AvatarView), "?",
		propertyChanged: (b, o, n) => ((AvatarView)b)._label.Text = n as string);

	public static readonly BindableProperty AvatarColorProperty = BindableProperty.Create(
		nameof(AvatarColor), typeof(Color), typeof(AvatarView), Colors.Gray,
		propertyChanged: (b, o, n) => ((AvatarView)b).BackgroundColor = (Color)n);

	public static readonly BindableProperty SizeProperty = BindableProperty.Create(
		nameof(Size), typeof(double), typeof(AvatarView), 40.0,
		propertyChanged: (b, o, n) => ((AvatarView)b).ApplySize((double)n));

	public string Initial { get => (string)GetValue(InitialProperty); set => SetValue(InitialProperty, value); }
	public Color AvatarColor { get => (Color)GetValue(AvatarColorProperty); set => SetValue(AvatarColorProperty, value); }
	public double Size { get => (double)GetValue(SizeProperty); set => SetValue(SizeProperty, value); }

	public AvatarView()
	{
		StrokeThickness = 0;
		Padding = 0;
		Content = _label;
		_label.Text = "?";
		ApplySize(40);
		HorizontalOptions = LayoutOptions.Start;
		VerticalOptions = LayoutOptions.Center;
	}

	void ApplySize(double size)
	{
		WidthRequest = size;
		HeightRequest = size;
		StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = new CornerRadius(size / 2) };
		_label.FontSize = size * 0.42;
	}
}

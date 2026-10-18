namespace Dalla.Controls;

/// <summary>État vide / erreur : pictogramme + phrase explicative + bouton d'action.</summary>
public partial class EmptyState : ContentView
{
	public static readonly BindableProperty IconProperty = BindableProperty.Create(
		nameof(Icon), typeof(string), typeof(EmptyState), "inbox",
		propertyChanged: (b, o, n) => ((EmptyState)b).IconView.Name = n as string);

	public static readonly BindableProperty MessageProperty = BindableProperty.Create(
		nameof(Message), typeof(string), typeof(EmptyState), string.Empty,
		propertyChanged: (b, o, n) => ((EmptyState)b).MessageLabel.Text = n as string);

	public static readonly BindableProperty ButtonTextProperty = BindableProperty.Create(
		nameof(ButtonText), typeof(string), typeof(EmptyState), string.Empty,
		propertyChanged: (b, o, n) =>
		{
			var e = (EmptyState)b;
			e.ActionButton.Text = n as string;
			e.ActionButton.IsVisible = !string.IsNullOrEmpty(n as string);
		});

	public string Icon { get => (string)GetValue(IconProperty); set => SetValue(IconProperty, value); }
	public string Message { get => (string)GetValue(MessageProperty); set => SetValue(MessageProperty, value); }
	public string ButtonText { get => (string)GetValue(ButtonTextProperty); set => SetValue(ButtonTextProperty, value); }

	public event EventHandler ActionClicked;

	public EmptyState()
	{
		InitializeComponent();
		IconView.Name = Icon;
		ActionButton.IsVisible = false;
	}

	void OnActionClicked(object sender, EventArgs e) => ActionClicked?.Invoke(this, EventArgs.Empty);
}

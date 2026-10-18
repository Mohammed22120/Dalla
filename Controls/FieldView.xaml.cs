namespace Dalla.Controls;

/// <summary>
/// Champ de saisie réutilisable avec ses 4 états : normal, actif (bordure bleue 2 px),
/// désactivé (fond gris) et erreur (bordure corail + message sous le champ).
/// </summary>
public partial class FieldView : ContentView
{
	public static readonly BindableProperty TitleProperty = BindableProperty.Create(
		nameof(Title), typeof(string), typeof(FieldView), string.Empty,
		propertyChanged: (b, o, n) =>
		{
			var f = (FieldView)b;
			f.TitleLabel.Text = n as string;
			SemanticProperties.SetDescription(f.Input, n as string);
		});

	public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
		nameof(Placeholder), typeof(string), typeof(FieldView), string.Empty,
		propertyChanged: (b, o, n) => ((FieldView)b).Input.Placeholder = n as string);

	public static readonly BindableProperty TextProperty = BindableProperty.Create(
		nameof(Text), typeof(string), typeof(FieldView), string.Empty, BindingMode.TwoWay,
		propertyChanged: (b, o, n) =>
		{
			var f = (FieldView)b;
			if (f.Input.Text != (string)n) f.Input.Text = n as string;
		});

	public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create(
		nameof(IsPassword), typeof(bool), typeof(FieldView), false,
		propertyChanged: (b, o, n) =>
		{
			var f = (FieldView)b;
			f.Input.IsPassword = (bool)n;
			f.ToggleButton.IsVisible = (bool)n;
		});

	public static readonly BindableProperty KeyboardTypeProperty = BindableProperty.Create(
		nameof(KeyboardType), typeof(Keyboard), typeof(FieldView), Keyboard.Default,
		propertyChanged: (b, o, n) => ((FieldView)b).Input.Keyboard = (Keyboard)n);

	public static readonly BindableProperty ErrorTextProperty = BindableProperty.Create(
		nameof(ErrorText), typeof(string), typeof(FieldView), string.Empty,
		propertyChanged: (b, o, n) => ((FieldView)b).UpdateState());

	public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
	public string Placeholder { get => (string)GetValue(PlaceholderProperty); set => SetValue(PlaceholderProperty, value); }
	public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }
	public bool IsPassword { get => (bool)GetValue(IsPasswordProperty); set => SetValue(IsPasswordProperty, value); }
	public Keyboard KeyboardType { get => (Keyboard)GetValue(KeyboardTypeProperty); set => SetValue(KeyboardTypeProperty, value); }
	public string ErrorText { get => (string)GetValue(ErrorTextProperty); set => SetValue(ErrorTextProperty, value); }

	public event EventHandler<TextChangedEventArgs> TextChanged;
	public event EventHandler Blurred;

	bool _focused;

	public FieldView()
	{
		InitializeComponent();
		UpdateState();
	}

	public new bool Focus() => Input.Focus();

	void OnTextChanged(object sender, TextChangedEventArgs e)
	{
		Text = e.NewTextValue;
		TextChanged?.Invoke(this, e);
	}

	void OnFocused(object sender, FocusEventArgs e) { _focused = true; UpdateState(); }

	void OnUnfocused(object sender, FocusEventArgs e)
	{
		_focused = false;
		UpdateState();
		Blurred?.Invoke(this, EventArgs.Empty);
	}

	void OnToggleClicked(object sender, EventArgs e)
	{
		Input.IsPassword = !Input.IsPassword;
		ToggleButton.Text = Input.IsPassword ? "Afficher" : "Masquer";
	}

	protected override void OnPropertyChanged(string propertyName = null)
	{
		base.OnPropertyChanged(propertyName);
		if (propertyName == nameof(IsEnabled)) UpdateState();
	}

	void UpdateState()
	{
		if (Frame == null) return;
		bool hasError = !string.IsNullOrEmpty(ErrorText);
		ErrorLabel.Text = ErrorText;
		ErrorLabel.IsVisible = hasError;

		string state = !IsEnabled ? "Disabled"
			: hasError ? "Error"
			: _focused ? "Focus"
			: "Normal";
		VisualStateManager.GoToState(Frame, state);
	}
}

using Microsoft.Maui.Controls.Shapes;

namespace Dalla.Controls;

/// <summary>
/// Barre de navigation inférieure flottante : Accueil · Dépenses · [+] · Statistiques · Foyer.
/// À placer en bas de chacune des 4 pages principales : &lt;controls:BottomNavBar Active="home" /&gt;.
/// La barre native de Shell est masquée (voir AppShell.xaml) ; la navigation passe par les routes //main/….
/// </summary>
public class BottomNavBar : ContentView
{
	record Tab(string Route, string Title, string Icon);

	static readonly Tab[] Tabs =
	{
		new("home", "Accueil", "house"),
		new("expenses", "Dépenses", "receipt"),
		new("stats", "Stats", "chart-column"),
		new("foyer", "Foyer", "users"),
	};

	public static readonly BindableProperty ActiveProperty = BindableProperty.Create(
		nameof(Active), typeof(string), typeof(BottomNavBar), "home",
		propertyChanged: (b, o, n) => ((BottomNavBar)b).Build());

	/// <summary>Route de l'onglet courant : home, expenses, stats ou foyer.</summary>
	public string Active { get => (string)GetValue(ActiveProperty); set => SetValue(ActiveProperty, value); }

	public BottomNavBar()
	{
		VerticalOptions = LayoutOptions.End;
		Margin = new Thickness(16, 0, 16, 12);
		Build();
	}

	protected override void OnHandlerChanged()
	{
		base.OnHandlerChanged();
		if (Application.Current == null) return;
		Application.Current.RequestedThemeChanged -= OnThemeChanged;
		if (Handler != null) Application.Current.RequestedThemeChanged += OnThemeChanged;
	}

	void OnThemeChanged(object sender, AppThemeChangedEventArgs e) => Build();

	void Build()
	{
		var primary = ChartTheme.Primary;
		var muted = ChartTheme.Text2;

		var grid = new Grid
		{
			ColumnDefinitions =
			{
				new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Star),
				new ColumnDefinition(GridLength.Auto),
				new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Star),
			},
			ColumnSpacing = 0,
			Padding = new Thickness(8, 0),
			HeightRequest = 68,
		};

		grid.Add(TabItem(Tabs[0], primary, muted), 0, 0);
		grid.Add(TabItem(Tabs[1], primary, muted), 1, 0);
		grid.Add(AddButton(primary), 2, 0);
		grid.Add(TabItem(Tabs[2], primary, muted), 3, 0);
		grid.Add(TabItem(Tabs[3], primary, muted), 4, 0);

		Content = new Border
		{
			BackgroundColor = ChartTheme.Card,
			Stroke = new SolidColorBrush(ChartTheme.Grid),
			StrokeThickness = 1,
			StrokeShape = new RoundRectangle { CornerRadius = 34 },
			Shadow = new Shadow { Brush = Brush.Black, Opacity = ChartTheme.IsDark ? 0.5f : 0.14f, Radius = 18, Offset = new Point(0, 6) },
			Padding = 0,
			Content = grid,
		};
	}

	View TabItem(Tab tab, Color primary, Color muted)
	{
		bool active = tab.Route == Active;
		var color = active ? primary : muted;

		// Pastille teintée derrière l'icône de l'onglet actif
		var pill = new Border
		{
			WidthRequest = 56,
			HeightRequest = 32,
			StrokeThickness = 0,
			StrokeShape = new RoundRectangle { CornerRadius = 16 },
			BackgroundColor = active ? primary.WithAlpha(0.14f) : Colors.Transparent,
			HorizontalOptions = LayoutOptions.Center,
			Content = new LucideIcon { Name = tab.Icon, Size = 22, IconColor = color, StrokeWidth = active ? 2.2 : 1.9 },
		};

		var label = new Label
		{
			Text = tab.Title,
			FontSize = 11,
			FontAttributes = active ? FontAttributes.Bold : FontAttributes.None,
			TextColor = color,
			HorizontalTextAlignment = TextAlignment.Center,
			LineBreakMode = LineBreakMode.NoWrap,
		};

		var item = new VerticalStackLayout
		{
			Spacing = 2,
			VerticalOptions = LayoutOptions.Center,
			BackgroundColor = Colors.Transparent,
			Children = { pill, label },
		};
		SemanticProperties.SetDescription(item, active ? $"{tab.Title}, onglet actif" : tab.Title);

		var tap = new TapGestureRecognizer();
		tap.Tapped += async (_, _) =>
		{
			if (tab.Route == Active) return;
			await Shell.Current.GoToAsync($"//main/{tab.Route}");
		};
		item.GestureRecognizers.Add(tap);
		return item;
	}

	View AddButton(Color primary)
	{
		var btn = new Border
		{
			WidthRequest = 52,
			HeightRequest = 52,
			BackgroundColor = primary,
			StrokeThickness = 0,
			StrokeShape = new RoundRectangle { CornerRadius = 26 },
			Margin = new Thickness(6, 0),
			VerticalOptions = LayoutOptions.Center,
			Shadow = new Shadow { Brush = new SolidColorBrush(primary), Opacity = 0.35f, Radius = 10, Offset = new Point(0, 4) },
			Content = new LucideIcon { Name = "plus", Size = 26, IconColor = Colors.White, StrokeWidth = 2.4 },
		};
		SemanticProperties.SetDescription(btn, "Ajouter une dépense");

		var tap = new TapGestureRecognizer();
		tap.Tapped += async (_, _) => await Shell.Current.GoToAsync("add");
		btn.GestureRecognizers.Add(tap);
		return btn;
	}
}

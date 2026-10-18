using Dalla.Controls;
using Dalla.Models;

namespace Dalla.Views;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
		Empty.ActionClicked += async (_, _) => await Shell.Current.GoToAsync("add");
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		Reload();
	}

	void Reload()
	{
		var month = AppData.MonthStart;
		long spent = AppData.TotalSpent(month);
		long budget = AppData.GlobalBudget;
		long balance = budget - spent;

		Greeting.Text = $"Bonjour, {AppData.UserName.Split(' ')[0]}";
		HouseLabel.Text = AppData.HouseholdName;
		var me = AppData.GetMember(AppData.CurrentMemberId);
		UserAvatar.Initial = me.Initial;
		UserAvatar.AvatarColor = me.Color;
		MonthLabel.Text = AppData.MonthLabel(month).ToUpperInvariant();

		// Montant : vert = il reste, corail = dépassé ; toujours avec un signe
		bool over = balance < 0;
		BalanceLabel.Text = over ? $"−{AppData.Money(-balance)}" : $"+{AppData.Money(balance)}";
		BalanceLabel.TextColor = Color.FromArgb(over ? "#E4572E" : "#2E9E6B");

		double ratio = budget > 0 ? (double)spent / budget : 0;
		BudgetProgress.Progress = Math.Min(ratio, 1);
		BudgetProgress.ProgressColor = Color.FromArgb(ratio >= 1 ? "#E4572E" : ratio >= 0.8 ? "#F5A623" : "#2E9E6B");
		ProgressText.Text = $"{AppData.FormatNumber(spent)} dépensés sur {AppData.Money(budget)} ({(int)Math.Round(ratio * 100)} %)";

		// Alerte : corail = dépassé, ambre = seuil de 80 % proche (fond teinté + bordure + icône + texte)
		var alert = AppData.CurrentAlert(month);
		if (alert is { } a) ShowAlert(a.Text, a.Exceeded);
		else if (ratio >= 1) ShowAlert($"Attention : budget global dépassé de {AppData.Money(spent - budget)}", true);
		else if (ratio >= 0.8) ShowAlert($"Budget global presque atteint ({(int)Math.Round(ratio * 100)} %).", false);
		else AlertBanner.IsVisible = false;

		// Raccourcis catégories
		Shortcuts.Children.Clear();
		foreach (var c in AppData.Categories)
		{
			var tile = new Border
			{
				WidthRequest = 76,
				HeightRequest = 76,
				StrokeThickness = 1,
				Padding = 4,
				StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 12 },
			};
			tile.SetAppThemeColor(Border.BackgroundColorProperty, Color.FromArgb("#FFFFFF"), Color.FromArgb("#1E293B"));
			tile.SetAppThemeColor(Border.StrokeProperty, Color.FromArgb("#E5E7EB"), Color.FromArgb("#334155"));
			var badge = new Border
			{
				WidthRequest = 32, HeightRequest = 32, StrokeThickness = 0, BackgroundColor = c.Color,
				HorizontalOptions = LayoutOptions.Center,
				StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 16 },
				Content = new LucideIcon { Name = c.Icon, IconColor = Colors.White, Size = 20 },
			};
			tile.Content = new VerticalStackLayout
			{
				Spacing = 4, VerticalOptions = LayoutOptions.Center,
				Children = { badge, new Label { Text = c.Name, FontSize = 11, HorizontalTextAlignment = TextAlignment.Center, LineBreakMode = LineBreakMode.TailTruncation, MaxLines = 1 } },
			};
			SemanticProperties.SetDescription(tile, $"Ajouter une dépense en {c.Name}");
			var id = c.Id;
			tile.GestureRecognizers.Add(new TapGestureRecognizer
			{
				Command = new Command(async () => await Shell.Current.GoToAsync($"add?cat={id}")),
			});
			Shortcuts.Children.Add(tile);
		}

		// Dernières dépenses (3 à 5)
		var monthRows = AppData.InMonth(month).OrderByDescending(e => e.Date).Take(5).Select(AppData.ToRow).ToList();
		BindableLayout.SetItemsSource(RecentList, monthRows);
		bool empty = monthRows.Count == 0;
		RecentCard.IsVisible = !empty;
		EmptyCard.IsVisible = empty;
	}

	void ShowAlert(string text, bool exceeded)
	{
		AlertBanner.IsVisible = true;
		AlertLabel.Text = "⚠ " + text;
		var accent = Color.FromArgb(exceeded ? "#E4572E" : "#F5A623");
		AlertBanner.Stroke = accent;
		if (exceeded)
			AlertBanner.SetAppThemeColor(Border.BackgroundColorProperty, Color.FromArgb("#FDECE7"), Color.FromArgb("#3B2019"));
		else
			AlertBanner.SetAppThemeColor(Border.BackgroundColorProperty, Color.FromArgb("#FEF3DC"), Color.FromArgb("#3A2B0F"));
	}

	async void OnSeeAllTapped(object sender, TappedEventArgs e) => await Shell.Current.GoToAsync("//main/expenses");
}

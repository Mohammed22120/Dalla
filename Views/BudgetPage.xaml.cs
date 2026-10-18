using Dalla.Controls;
using Dalla.Models;
using Microsoft.Maui.Controls.Shapes;

namespace Dalla.Views;

public partial class BudgetPage : ContentPage
{
	class Row
	{
		public Category Cat;
		public FieldView Field;
		public ProgressBar Bar;
		public Label Status;
		public Label Amounts;
		public Border Card;
	}

	readonly List<Row> _rows = new();

	public BudgetPage()
	{
		InitializeComponent();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		Build();
	}

	void Build()
	{
		bool admin = AppData.IsAdmin;
		LockedBanner.IsVisible = !admin;
		SaveButton.IsVisible = admin;

		GlobalField.Text = AppData.FormatNumber(AppData.GlobalBudget);
		GlobalField.IsEnabled = admin;      // état désactivé pour un simple membre

		CategoryRows.Children.Clear();
		_rows.Clear();
		foreach (var cat in AppData.Categories)
		{
			var row = new Row { Cat = cat };

			var badge = new Border
			{
				WidthRequest = 36, HeightRequest = 36, StrokeThickness = 0, BackgroundColor = cat.Color,
				StrokeShape = new RoundRectangle { CornerRadius = 18 },
				Content = new LucideIcon { Name = cat.Icon, IconColor = Colors.White, Size = 22 },
			};
			row.Amounts = new Label { FontSize = 14, HorizontalTextAlignment = TextAlignment.End, VerticalOptions = LayoutOptions.Center };
			var header = new Grid
			{
				ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) },
				ColumnSpacing = 12,
			};
			header.Add(badge, 0, 0);
			header.Add(new Label { Text = cat.Name, FontAttributes = FontAttributes.Bold, VerticalOptions = LayoutOptions.Center }, 1, 0);
			header.Add(row.Amounts, 2, 0);

			row.Bar = new ProgressBar { HeightRequest = 8 };
			row.Status = new Label { FontSize = 14 };
			row.Field = new FieldView
			{
				Title = "Budget",
				KeyboardType = Keyboard.Numeric,
				Text = AppData.FormatNumber(cat.Budget),
				IsEnabled = admin,
			};
			row.Field.TextChanged += (_, _) => Refresh(row);

			row.Card = new Border
			{
				Style = GetStyle("Card"),
				Content = new VerticalStackLayout { Spacing = 8, Children = { header, row.Bar, row.Status, row.Field } },
			};
			if (!admin) row.Card.Opacity = 0.7; // carte grisée pour un simple membre

			_rows.Add(row);
			CategoryRows.Children.Add(row.Card);
			Refresh(row);
		}
	}

	static Style GetStyle(string key)
		=> Application.Current.Resources.TryGetValue(key, out var v) ? (Style)v : null;

	/// <summary>Barre : vert → ambre (80 %) → corail (100 %), toujours accompagnée d'un texte.</summary>
	void Refresh(Row row)
	{
		Ui.TryParseAmount(row.Field.Text, out var budget);
		var spent = AppData.SpentIn(row.Cat.Id, AppData.MonthStart);
		double ratio = budget > 0 ? (double)spent / budget : 0;

		row.Amounts.Text = $"{AppData.FormatNumber(spent)} / {AppData.Money(budget)}";
		row.Bar.Progress = Math.Min(ratio, 1);
		if (budget > 0 && ratio >= 1)
		{
			row.Bar.ProgressColor = Color.FromArgb("#E4572E");
			row.Status.Text = $"⚠ Dépassé de {AppData.Money(spent - budget)}";
		}
		else if (budget > 0 && ratio >= 0.8)
		{
			row.Bar.ProgressColor = Color.FromArgb("#F5A623");
			row.Status.Text = $"⚠ Bientôt atteint ({(int)Math.Round(ratio * 100)} %)";
		}
		else
		{
			row.Bar.ProgressColor = Color.FromArgb("#2E9E6B");
			row.Status.Text = "✓ Dans le budget";
		}
	}

	async void OnSaveClicked(object sender, EventArgs e)
	{
		if (!Ui.TryParseAmount(GlobalField.Text, out var global) || global <= 0)
		{
			GlobalField.ErrorText = "Saisis un budget supérieur à 0.";
			return;
		}
		GlobalField.ErrorText = "";

		bool ok = true;
		foreach (var r in _rows)
		{
			if (!Ui.TryParseAmount(r.Field.Text, out var b) || b < 0) { r.Field.ErrorText = "Montant invalide."; ok = false; }
			else r.Field.ErrorText = "";
		}
		if (!ok) return;

		AppData.GlobalBudget = global;
		foreach (var r in _rows)
		{
			Ui.TryParseAmount(r.Field.Text, out var b);
			r.Cat.Budget = b;
		}
		await Ui.ShowSnackAsync(Snack, "✓ Budgets enregistrés");
	}

	async void OnBackClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("..");
}

using Dalla.Controls;
using Dalla.Models;
using Microsoft.Maui.Controls.Shapes;

namespace Dalla.Views;

/// <summary>Ajout ET modification d'une dépense (même écran, en passant ?id=...).</summary>
[QueryProperty(nameof(ExpenseId), "id")]
[QueryProperty(nameof(PresetCategory), "cat")]
public partial class AddExpensePage : ContentPage
{
	static string _lastCategory = "alim"; // valeur par défaut intelligente : dernière catégorie utilisée

	string _expenseId;
	string _categoryId = _lastCategory;
	string _memberId = AppData.CurrentMemberId;
	bool _saving;

	public string ExpenseId { get => _expenseId; set { _expenseId = value; LoadExisting(); } }
	public string PresetCategory { set { if (!string.IsNullOrEmpty(value)) { _categoryId = value; BuildCategories(); } } }

	public AddExpensePage()
	{
		InitializeComponent();
		DatePick.Date = AppData.Today;      // date du jour par défaut
		DatePick.MaximumDate = AppData.Today;
		BuildCategories();
		BuildMembers();

		Amount.TextChanged += (_, _) =>
		{
			Amount.ErrorText = "";
			SaveButton.IsEnabled = !string.IsNullOrWhiteSpace(Amount.Text);
		};
		Amount.Blurred += (_, _) => ValidateAmount(requireNonEmpty: false);
	}

	void LoadExisting()
	{
		var e = AppData.GetExpense(_expenseId);
		if (e == null) return;
		PageTitle.Text = "Modifier la dépense";
		Amount.Text = AppData.FormatNumber(e.Amount);
		_categoryId = e.CategoryId;
		_memberId = e.MemberId;
		DatePick.Date = e.Date;
		NoteEditor.Text = e.Note;
		BuildCategories();
		BuildMembers();
		SaveButton.IsEnabled = true;
	}

	// ----- Sélecteurs à icônes -----
	void BuildCategories()
	{
		CategoryGrid.Children.Clear();
		foreach (var c in AppData.Categories)
		{
			bool selected = c.Id == _categoryId;
			var badge = new Border
			{
				WidthRequest = 36, HeightRequest = 36, StrokeThickness = 0, BackgroundColor = c.Color,
				HorizontalOptions = LayoutOptions.Center,
				StrokeShape = new RoundRectangle { CornerRadius = 18 },
				Content = new LucideIcon { Name = c.Icon, IconColor = Colors.White, Size = 22 },
			};
			var tile = new Border
			{
				WidthRequest = 80, HeightRequest = 80, Padding = 4, Margin = new Thickness(0, 0, 8, 8),
				StrokeThickness = selected ? 2 : 1,
				StrokeShape = new RoundRectangle { CornerRadius = 12 },
				Content = new VerticalStackLayout
				{
					Spacing = 4, VerticalOptions = LayoutOptions.Center,
					Children =
					{
						badge,
						new Label { Text = c.Name, FontSize = 12, HorizontalTextAlignment = TextAlignment.Center, LineBreakMode = LineBreakMode.TailTruncation, MaxLines = 1 },
					},
				},
			};
			if (selected)
			{
				tile.SetAppThemeColor(Border.StrokeProperty, Color.FromArgb("#1E5AA8"), Color.FromArgb("#3A74C4"));
				tile.SetAppThemeColor(Border.BackgroundColorProperty, Color.FromArgb("#E8F0FA"), Color.FromArgb("#243B5C"));
			}
			else
			{
				tile.SetAppThemeColor(Border.StrokeProperty, Color.FromArgb("#E5E7EB"), Color.FromArgb("#334155"));
				tile.SetAppThemeColor(Border.BackgroundColorProperty, Color.FromArgb("#FFFFFF"), Color.FromArgb("#1E293B"));
			}
			SemanticProperties.SetDescription(tile, selected ? $"{c.Name}, sélectionnée" : c.Name);
			var id = c.Id;
			tile.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { _categoryId = id; BuildCategories(); }) });
			CategoryGrid.Children.Add(tile);
		}
	}

	void BuildMembers()
	{
		MemberRow.Children.Clear();
		foreach (var m in AppData.Members)
		{
			bool selected = m.Id == _memberId;
			var chip = new Border
			{
				Padding = new Thickness(8, 6, 14, 6), MinimumHeightRequest = 48, Margin = new Thickness(0, 0, 8, 8),
				StrokeThickness = selected ? 2 : 1,
				StrokeShape = new RoundRectangle { CornerRadius = 24 },
				Content = new HorizontalStackLayout
				{
					Spacing = 8, VerticalOptions = LayoutOptions.Center,
					Children =
					{
						new AvatarView { Initial = m.Initial, AvatarColor = m.Color, Size = 32 },
						new Label { Text = m.Name, VerticalOptions = LayoutOptions.Center, FontAttributes = selected ? FontAttributes.Bold : FontAttributes.None },
					},
				},
			};
			if (selected)
			{
				chip.SetAppThemeColor(Border.StrokeProperty, Color.FromArgb("#1E5AA8"), Color.FromArgb("#3A74C4"));
				chip.SetAppThemeColor(Border.BackgroundColorProperty, Color.FromArgb("#E8F0FA"), Color.FromArgb("#243B5C"));
			}
			else
			{
				chip.SetAppThemeColor(Border.StrokeProperty, Color.FromArgb("#E5E7EB"), Color.FromArgb("#334155"));
				chip.SetAppThemeColor(Border.BackgroundColorProperty, Color.FromArgb("#FFFFFF"), Color.FromArgb("#1E293B"));
			}
			SemanticProperties.SetDescription(chip, selected ? $"{m.Name}, sélectionné" : m.Name);
			var id = m.Id;
			chip.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { _memberId = id; BuildMembers(); }) });
			MemberRow.Children.Add(chip);
		}
	}

	bool ValidateAmount(bool requireNonEmpty = true, bool showError = true)
	{
		if (string.IsNullOrWhiteSpace(Amount.Text))
		{
			if (requireNonEmpty && showError) Amount.ErrorText = "Saisis un montant.";
			return false;
		}
		if (!Ui.TryParseAmount(Amount.Text, out var v) || v <= 0)
		{
			if (showError) Amount.ErrorText = "Le montant doit être supérieur à 0.";
			return false;
		}
		Amount.ErrorText = "";
		return true;
	}

	async void OnSaveClicked(object sender, EventArgs e)
	{
		if (_saving) return;
		if (!ValidateAmount()) return;
		Ui.TryParseAmount(Amount.Text, out var amount);

		_saving = true;
		SaveButton.IsEnabled = false;

		var cat = AppData.GetCategory(_categoryId);
		var chosenDate = (DatePick.Date ?? AppData.Today).Date;
		var month = new DateTime(chosenDate.Year, chosenDate.Month, 1);
		long before = AppData.SpentIn(cat.Id, month);
		var existing = AppData.GetExpense(_expenseId);
		if (existing != null && existing.CategoryId == cat.Id && existing.Date.Month == month.Month) before -= existing.Amount;

		var expense = existing ?? new Expense();
		expense.Amount = amount;
		expense.CategoryId = cat.Id;
		expense.MemberId = _memberId;
		expense.Date = chosenDate;
		expense.Note = NoteEditor.Text?.Trim();
		expense.Label = string.IsNullOrWhiteSpace(expense.Note) ? cat.Name : expense.Note.Split('\n')[0];
		AppData.Save(expense);
		_lastCategory = cat.Id;

		long after = before + amount;
		bool nowOver = cat.Budget > 0 && after > cat.Budget;

		// Confirmation courte qui disparaît seule
		await Ui.ShowSnackAsync(Snack, "✓ Dépense enregistrée", 1200);

		// Alerte si la saisie fait dépasser le budget de la catégorie
		if (nowOver)
		{
			await DisplayAlert("Alerte budget",
				$"Attention : budget {cat.Name} dépassé de {AppData.Money(after - cat.Budget)}", "OK");
		}
		else if (cat.Budget > 0 && (double)after / cat.Budget >= 0.8)
		{
			await DisplayAlert("Budget bientôt atteint",
				$"Tu as utilisé {(int)Math.Round((double)after / cat.Budget * 100)} % du budget {cat.Name}.", "OK");
		}

		_saving = false;
		await Shell.Current.GoToAsync("..");
	}

	async void OnCancelClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("..");
}

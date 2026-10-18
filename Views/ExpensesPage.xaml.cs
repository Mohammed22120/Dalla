using Dalla.Controls;
using Dalla.Models;

namespace Dalla.Views;

public partial class ExpensesPage : ContentPage
{
	const string AllMembers = "Tous les membres";
	const string AllCategories = "Toutes les catégories";
	static readonly string[] Periods = { "Ce mois-ci", "Mois dernier", "Tout" };

	bool _ready;
	int _loadId;

	public ExpensesPage()
	{
		InitializeComponent();

		MemberPicker.ItemsSource = new[] { AllMembers }.Concat(AppData.Members.Select(m => m.Name)).ToList();
		CategoryPicker.ItemsSource = new[] { AllCategories }.Concat(AppData.Categories.Select(c => c.Name)).ToList();
		PeriodPicker.ItemsSource = Periods;
		MemberPicker.SelectedIndex = 0;
		CategoryPicker.SelectedIndex = 0;
		PeriodPicker.SelectedIndex = 0;
		_ready = true;

		EmptyView.ActionClicked += async (_, _) => await Shell.Current.GoToAsync("add");
		ErrorView.ActionClicked += async (_, _) => await LoadAsync(simulateDelay: true);

		ApplySkeletonColors();
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		ApplySkeletonColors();
		await LoadAsync(simulateDelay: true);
	}

	void ApplySkeletonColors()
	{
		foreach (var name in new[] { "Sk1", "Sk1b", "Sk1c", "Sk2", "Sk2b", "Sk2c", "Sk3", "Sk3b", "Sk3c", "Sk4", "Sk4b", "Sk4c" })
		{
			if (FindByName(name) is Border b)
				b.SetAppThemeColor(Border.BackgroundColorProperty, Color.FromArgb("#E5E7EB"), Color.FromArgb("#334155"));
		}
	}

	async void OnFilterChanged(object sender, EventArgs e)
	{
		if (!_ready) return;
		// pas de délai de chargement quand on filtre : réponse immédiate
		await LoadAsync(simulateDelay: false);
	}

	async Task LoadAsync(bool simulateDelay)
	{
		int id = ++_loadId;
		List.IsVisible = EmptyView.IsVisible = ErrorView.IsVisible = false;
		Skeleton.IsVisible = true;
		if (simulateDelay) await Task.Delay(700);
		if (id != _loadId) return; // un chargement plus récent a pris le relais
		Skeleton.IsVisible = false;

		if (AppData.SimulateNetworkError)
		{
			ErrorView.IsVisible = true;
			return;
		}

		var now = AppData.Today;
		IEnumerable<Expense> q = AppData.Expenses;

		switch (PeriodPicker.SelectedIndex)
		{
			case 0: q = q.Where(e => e.Date.Year == now.Year && e.Date.Month == now.Month); break;
			case 1:
				var prev = now.AddMonths(-1);
				q = q.Where(e => e.Date.Year == prev.Year && e.Date.Month == prev.Month); break;
		}
		if (MemberPicker.SelectedIndex > 0)
		{
			var m = AppData.Members[MemberPicker.SelectedIndex - 1];
			q = q.Where(e => e.MemberId == m.Id);
		}
		if (CategoryPicker.SelectedIndex > 0)
		{
			var c = AppData.Categories[CategoryPicker.SelectedIndex - 1];
			q = q.Where(e => e.CategoryId == c.Id);
		}
		var text = SearchEntry.Text?.Trim();
		if (!string.IsNullOrEmpty(text))
			q = q.Where(e => (e.Label ?? "").Contains(text, StringComparison.CurrentCultureIgnoreCase)
			              || (e.Note ?? "").Contains(text, StringComparison.CurrentCultureIgnoreCase));

		var groups = q.OrderByDescending(e => e.Date)
			.GroupBy(e => e.Date.Date)
			.Select(g => new ExpenseGroup(AppData.LongDate(g.Key), g.Select(AppData.ToRow)))
			.ToList();

		List.ItemsSource = groups;
		bool empty = groups.Count == 0;
		List.IsVisible = !empty;
		EmptyView.IsVisible = empty;
	}

}

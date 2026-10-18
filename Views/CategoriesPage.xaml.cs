using Dalla.Models;

namespace Dalla.Views;

public partial class CategoriesPage : ContentPage
{
	public CategoriesPage()
	{
		InitializeComponent();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		Reload();
		AddButton.IsEnabled = AppData.IsAdmin;
		AdminNote.IsVisible = !AppData.IsAdmin;
	}

	void Reload()
		=> BindableLayout.SetItemsSource(CategoryList, AppData.BudgetRows(AppData.MonthStart));

	async void OnAddClicked(object sender, EventArgs e)
	{
		var name = await DisplayPromptAsync("Nouvelle catégorie", "Quel nom veux-tu lui donner ?",
			"Ajouter", "Annuler", "Ex. : Animaux", 24);
		if (string.IsNullOrWhiteSpace(name)) return;

		AppData.Categories.Add(new Category
		{
			Id = Guid.NewGuid().ToString(),
			Name = name.Trim(),
			Icon = "tag",
			ColorHex = "#A3A3A3",
			Budget = 0,
		});
		Reload();
	}

	async void OnBackClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("..");
}

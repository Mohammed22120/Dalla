using Dalla.Models;

namespace Dalla.Controls;

/// <summary>Ligne de dépense : icône catégorie, libellé, avatar du membre, montant en corail.</summary>
public partial class ExpenseRowView : ContentView
{
	public ExpenseRowView()
	{
		InitializeComponent();
	}

	async void OnTapped(object sender, TappedEventArgs e)
	{
		if (BindingContext is ExpenseRow row)
			await Shell.Current.GoToAsync($"detail?id={row.Id}");
	}
}

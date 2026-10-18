using Dalla.Controls;
using Dalla.Models;

namespace Dalla.Views;

[QueryProperty(nameof(ExpenseId), "id")]
public partial class ExpenseDetailPage : ContentPage
{
	string _id;
	public string ExpenseId { get => _id; set { _id = value; } }

	public ExpenseDetailPage()
	{
		InitializeComponent();
	}

	// Recharge au retour de l'écran « Modifier »
	protected override void OnAppearing()
	{
		base.OnAppearing();
		var e = AppData.GetExpense(_id);
		if (e == null) return;
		var c = AppData.GetCategory(e.CategoryId);
		var m = AppData.GetMember(e.MemberId);

		CatBadge.BackgroundColor = c.Color;
		CatIcon.Name = c.Icon;
		LabelText.Text = e.Label;
		AmountLabel.Text = $"−{AppData.Money(e.Amount)}";
		CatName.Text = c.Name;
		MemberAvatar.Initial = m.Initial;
		MemberAvatar.AvatarColor = m.Color;
		MemberName.Text = m.Name;
		DateLabel.Text = AppData.ShortDate(e.Date);
		NoteLabel.Text = string.IsNullOrWhiteSpace(e.Note) ? "Aucune note." : e.Note;
	}

	async void OnEditClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync($"add?id={_id}");

	async void OnDeleteClicked(object sender, EventArgs e)
	{
		bool ok = await DisplayAlert("Supprimer cette dépense ?",
			"Cette action est définitive.", "Supprimer", "Annuler");
		if (!ok) return;

		AppData.Delete(_id);
		await Ui.ShowSnackAsync(Snack, "✓ Dépense supprimée", 1000);
		await Shell.Current.GoToAsync("..");
	}

	async void OnBackClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("..");
}

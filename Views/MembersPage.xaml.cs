using Dalla.Controls;
using Dalla.Models;

namespace Dalla.Views;

public partial class MembersPage : ContentPage
{
	string Link => $"https://dalla.app/rejoindre/{AppData.InviteCode}";

	public MembersPage()
	{
		InitializeComponent();
		CodeLabel.Text = AppData.InviteCode;
		LinkLabel.Text = Link;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		var rows = AppData.MemberRows(AppData.MonthStart);
		BindableLayout.SetItemsSource(MemberList, rows);
		bool solo = rows.Count <= 1;
		SoloCard.IsVisible = solo;
		InviteButton.IsVisible = !solo;                  // en état vide, le bouton est dans la carte
	}

	void OnInviteClicked(object sender, EventArgs e) => InvitePanel.IsVisible = true;

	async void OnCopyClicked(object sender, EventArgs e)
	{
		await Clipboard.Default.SetTextAsync(AppData.InviteCode);
		await Ui.ShowSnackAsync(Snack, "✓ Code copié");
	}

	async void OnShareClicked(object sender, EventArgs e)
	{
		await Share.Default.RequestAsync(new ShareTextRequest
		{
			Title = "Rejoins mon foyer sur Dalla",
			Text = $"Rejoins le foyer « {AppData.HouseholdName} » sur Dalla avec le code {AppData.InviteCode} ou ce lien : {Link}",
		});
	}

	async void OnBackClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("..");
}

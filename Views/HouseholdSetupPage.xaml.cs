using Dalla.Controls;
using Dalla.Models;

namespace Dalla.Views;

public partial class HouseholdSetupPage : ContentPage
{
	public HouseholdSetupPage()
	{
		InitializeComponent();
		CurrencyPicker.ItemsSource = new List<string> { "FCFA", "EUR", "USD", "MAD", "GNF" };
		CurrencyPicker.SelectedIndex = 0;

		HouseName.TextChanged += (_, _) => { HouseName.ErrorText = ""; RefreshCreate(); };
		InitialBudget.TextChanged += (_, _) => { InitialBudget.ErrorText = ""; RefreshCreate(); };
		Code.TextChanged += (_, _) =>
		{
			Code.ErrorText = "";
			JoinButton.IsEnabled = !string.IsNullOrWhiteSpace(Code.Text);
		};
	}

	void RefreshCreate()
		=> CreateButton.IsEnabled = !string.IsNullOrWhiteSpace(HouseName.Text)
		                            && !string.IsNullOrWhiteSpace(InitialBudget.Text);

	void OnCreateTab(object sender, EventArgs e) => ShowPanel(create: true);
	void OnJoinTab(object sender, EventArgs e) => ShowPanel(create: false);

	void ShowPanel(bool create)
	{
		CreatePanel.IsVisible = create;
		JoinPanel.IsVisible = !create;
		CreateTab.Style = GetStyle(create ? "PrimaryButton" : "SecondaryButton");
		JoinTab.Style = GetStyle(create ? "SecondaryButton" : "PrimaryButton");
	}

	static Style GetStyle(string key)
		=> Application.Current.Resources.TryGetValue(key, out var value) ? (Style)value : null;

	async void OnCreateClicked(object sender, EventArgs e)
	{
		if (string.IsNullOrWhiteSpace(HouseName.Text)) { HouseName.ErrorText = "Donne un nom à ton foyer."; return; }
		if (!Ui.TryParseAmount(InitialBudget.Text, out var budget) || budget <= 0)
		{
			InitialBudget.ErrorText = "Saisis un montant supérieur à 0.";
			return;
		}

		await Busy();
		AppData.HouseholdName = HouseName.Text.Trim();
		AppData.Currency = CurrencyPicker.SelectedItem?.ToString() ?? "FCFA";
		AppData.GlobalBudget = budget;
		AppData.IsAdmin = true;
		await Shell.Current.GoToAsync("//main");
	}

	async void OnJoinClicked(object sender, EventArgs e)
	{
		await Busy();
		if (!string.Equals(Code.Text?.Trim(), AppData.InviteCode, StringComparison.OrdinalIgnoreCase))
		{
			Code.ErrorText = "Code introuvable. Vérifie-le auprès de l'administrateur.";
			return;
		}
		AppData.IsAdmin = false;
		await Shell.Current.GoToAsync("//main");
	}

	async Task Busy()
	{
		Loader.IsVisible = Loader.IsRunning = true;
		CreateButton.IsEnabled = JoinButton.IsEnabled = false;
		await Task.Delay(800);
		Loader.IsVisible = Loader.IsRunning = false;
		CreateButton.IsEnabled = !string.IsNullOrWhiteSpace(HouseName.Text) && !string.IsNullOrWhiteSpace(InitialBudget.Text);
		JoinButton.IsEnabled = !string.IsNullOrWhiteSpace(Code.Text);
	}
}

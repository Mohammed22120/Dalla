using Dalla.Controls;
using Dalla.Models;

namespace Dalla.Views;

public partial class SettingsPage : ContentPage
{
	static readonly string[] Themes = { "Automatique (système)", "Clair", "Sombre" };
	static readonly string[] Currencies = { "FCFA", "EUR", "USD", "MAD", "GNF" };
	bool _loading = true;

	public SettingsPage()
	{
		InitializeComponent();
		CurrencyPicker.ItemsSource = Currencies;
		ThemePicker.ItemsSource = Themes;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		_loading = true;
		var me = AppData.GetMember(AppData.CurrentMemberId);
		Avatar.Initial = me.Initial;
		Avatar.AvatarColor = me.Color;
		NameLabel.Text = AppData.UserName;
		EmailLabel.Text = AppData.UserEmail;
		RoleLabel.Text = AppData.IsAdmin ? "ADMINISTRATEUR" : "MEMBRE";

		CurrencyPicker.SelectedIndex = Math.Max(0, Array.IndexOf(Currencies, AppData.Currency));
		ThemePicker.SelectedIndex = Application.Current.UserAppTheme switch
		{
			AppTheme.Light => 1,
			AppTheme.Dark => 2,
			_ => 0,
		};
		AlertSwitch.IsToggled = AppData.AlertNotifications;
		NetSwitch.IsToggled = AppData.SimulateNetworkError;
		SoloSwitch.IsToggled = AppData.SimulateSoloHousehold;
		AdminSwitch.IsToggled = AppData.IsAdmin;
		_loading = false;
	}

	void OnCurrencyChanged(object sender, EventArgs e)
	{
		if (_loading || CurrencyPicker.SelectedIndex < 0) return;
		AppData.Currency = Currencies[CurrencyPicker.SelectedIndex];
	}

	void OnThemeChanged(object sender, EventArgs e)
	{
		if (_loading) return;
		Application.Current.UserAppTheme = ThemePicker.SelectedIndex switch
		{
			1 => AppTheme.Light,
			2 => AppTheme.Dark,
			_ => AppTheme.Unspecified,
		};
	}

	void OnAlertToggled(object sender, ToggledEventArgs e) { if (!_loading) AppData.AlertNotifications = e.Value; }
	void OnNetToggled(object sender, ToggledEventArgs e) { if (!_loading) AppData.SimulateNetworkError = e.Value; }
	void OnSoloToggled(object sender, ToggledEventArgs e) { if (!_loading) AppData.SimulateSoloHousehold = e.Value; }
	void OnAdminToggled(object sender, ToggledEventArgs e)
	{
		if (_loading) return;
		AppData.IsAdmin = e.Value;
		RoleLabel.Text = e.Value ? "ADMINISTRATEUR" : "MEMBRE";
	}

	async void OnClearClicked(object sender, EventArgs e)
	{
		AppData.ClearExpenses();
		await Ui.ShowSnackAsync(Snack, "✓ Dépenses supprimées");
	}

	async void OnRestoreClicked(object sender, EventArgs e)
	{
		AppData.RestoreSample();
		await Ui.ShowSnackAsync(Snack, "✓ Données d'exemple restaurées");
	}

	async void OnLogoutClicked(object sender, EventArgs e)
	{
		bool ok = await DisplayAlert("Se déconnecter ?", "Tu devras te reconnecter pour retrouver ton foyer.",
			"Se déconnecter", "Annuler");
		if (ok) await Shell.Current.GoToAsync("//login");
	}

	async void OnBackClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("..");
}

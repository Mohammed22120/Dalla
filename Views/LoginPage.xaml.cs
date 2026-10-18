using Dalla.Controls;
using Dalla.Models;

namespace Dalla.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
		Logo.Drawable = new LogoDrawable();
		Email.TextChanged += (_, _) => OnFormChanged();
		Password.TextChanged += (_, _) => OnFormChanged();
		Email.Blurred += (_, _) => ValidateEmail();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		Logo.Invalidate();
	}

	void OnFormChanged()
	{
		// Bouton désactivé tant que le formulaire est incomplet
		LoginButton.IsEnabled = !string.IsNullOrWhiteSpace(Email.Text) && !string.IsNullOrEmpty(Password.Text);
		ErrorBanner.IsVisible = false;
		Password.ErrorText = "";
		if (!string.IsNullOrEmpty(Email.ErrorText)) Email.ErrorText = "";
	}

	bool ValidateEmail()
	{
		if (string.IsNullOrWhiteSpace(Email.Text)) { Email.ErrorText = "Saisis ton e-mail."; return false; }
		if (!Ui.IsValidEmail(Email.Text.Trim())) { Email.ErrorText = "Cette adresse e-mail n'est pas valide."; return false; }
		Email.ErrorText = "";
		return true;
	}

	void SetLoading(bool loading)
	{
		Loader.IsVisible = loading;
		Loader.IsRunning = loading;
		LoginButton.IsEnabled = !loading;
		LoginButton.Text = loading ? "Connexion…" : "Se connecter";
		Email.IsEnabled = !loading;
		Password.IsEnabled = !loading;
	}

	async void OnLoginClicked(object sender, EventArgs e)
	{
		if (!ValidateEmail()) return;
		if (string.IsNullOrEmpty(Password.Text)) { Password.ErrorText = "Saisis ton mot de passe."; return; }

		SetLoading(true);
		await Task.Delay(900); // simulation du chargement
		SetLoading(false);

		if (AppData.SimulateNetworkError)
		{
			ErrorBannerLabel.Text = "Connexion impossible. Vérifie ton réseau.";
			ErrorBanner.IsVisible = true;
			return;
		}

		if (Password.Text != "dalla123")
		{
			Password.ErrorText = "E-mail ou mot de passe incorrect. Vérifie et réessaie.";
			return;
		}

		await Shell.Current.GoToAsync("//main");
	}

	async void OnForgotTapped(object sender, TappedEventArgs e) => await Shell.Current.GoToAsync("forgot");
	async void OnRegisterTapped(object sender, TappedEventArgs e) => await Shell.Current.GoToAsync("register");
}

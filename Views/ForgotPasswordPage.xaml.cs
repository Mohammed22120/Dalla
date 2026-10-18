using Dalla.Controls;
using Dalla.Models;

namespace Dalla.Views;

public partial class ForgotPasswordPage : ContentPage
{
	public ForgotPasswordPage()
	{
		InitializeComponent();
		Email.TextChanged += (_, _) =>
		{
			SendButton.IsEnabled = !string.IsNullOrWhiteSpace(Email.Text);
			Email.ErrorText = "";
			SuccessBox.IsVisible = false;
		};
	}

	async void OnSendClicked(object sender, EventArgs e)
	{
		var email = Email.Text?.Trim() ?? "";
		if (!Ui.IsValidEmail(email))
		{
			Email.ErrorText = "Cette adresse e-mail n'est pas valide.";
			return;
		}

		Loader.IsVisible = Loader.IsRunning = true;
		SendButton.IsEnabled = false;
		await Task.Delay(900);
		Loader.IsVisible = Loader.IsRunning = false;
		SendButton.IsEnabled = true;

		if (!AppData.KnownEmails.Any(k => string.Equals(k, email, StringComparison.OrdinalIgnoreCase)))
		{
			Email.ErrorText = "Aucun compte ne correspond à cette adresse. Vérifie-la ou crée un compte.";
			return;
		}

		SuccessLabel.Text = $"✓ C'est envoyé ! Regarde ta boîte mail ({email}) et clique sur le lien reçu.";
		SuccessBox.IsVisible = true;
	}

	async void OnBackClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("..");
	async void OnBackTapped(object sender, TappedEventArgs e) => await Shell.Current.GoToAsync("..");
}

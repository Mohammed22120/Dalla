using Dalla.Controls;
using Dalla.Models;

namespace Dalla.Views;

public partial class RegisterPage : ContentPage
{
	static readonly Color Grey = Color.FromArgb("#D1D5DB");
	static readonly Color Coral = Color.FromArgb("#E4572E");
	static readonly Color Amber = Color.FromArgb("#F5A623");
	static readonly Color Green = Color.FromArgb("#2E9E6B");

	public RegisterPage()
	{
		InitializeComponent();
		Seg1.Color = Seg2.Color = Seg3.Color = Grey;

		FullName.TextChanged += (_, _) => { if (!string.IsNullOrEmpty(FullName.ErrorText)) ValidateName(); Refresh(); };
		FullName.Blurred += (_, _) => ValidateName();

		Email.TextChanged += (_, _) => { ValidateEmail(liveOnly: true); Refresh(); };
		Email.Blurred += (_, _) => ValidateEmail();

		Password.TextChanged += (_, _) => { UpdateStrength(); if (!string.IsNullOrEmpty(Confirm.Text)) ValidateConfirm(); Refresh(); };
		Password.Blurred += (_, _) => ValidatePassword();

		Confirm.TextChanged += (_, _) => { ValidateConfirm(); Refresh(); };
		Confirm.Blurred += (_, _) => ValidateConfirm();
	}

	// ----- Validations en temps réel -----
	bool ValidateName()
	{
		if (string.IsNullOrWhiteSpace(FullName.Text)) { FullName.ErrorText = "Ce champ est obligatoire."; return false; }
		FullName.ErrorText = ""; return true;
	}

	bool ValidateEmail(bool liveOnly = false)
	{
		var t = Email.Text?.Trim() ?? "";
		if (t.Length == 0)
		{
			if (!liveOnly) Email.ErrorText = "Ce champ est obligatoire.";
			return false;
		}
		if (!Ui.IsValidEmail(t))
		{
			// On n'affiche l'erreur en direct qu'une fois le « @ » saisi
			if (!liveOnly || t.Contains('@')) Email.ErrorText = "Cette adresse e-mail n'est pas valide.";
			return false;
		}
		Email.ErrorText = ""; return true;
	}

	bool ValidatePassword()
	{
		var p = Password.Text ?? "";
		if (p.Length == 0) { Password.ErrorText = "Ce champ est obligatoire."; return false; }
		if (p.Length < 8) { Password.ErrorText = "8 caractères minimum."; return false; }
		Password.ErrorText = ""; return true;
	}

	bool ValidateConfirm()
	{
		if (string.IsNullOrEmpty(Confirm.Text)) { Confirm.ErrorText = "Ce champ est obligatoire."; return false; }
		if (Confirm.Text != Password.Text) { Confirm.ErrorText = "Les mots de passe sont différents."; return false; }
		Confirm.ErrorText = ""; return true;
	}

	int Strength(string p)
	{
		if (string.IsNullOrEmpty(p)) return 0;
		int score = 0;
		if (p.Length >= 8) score++;
		if (p.Any(char.IsLetter) && p.Any(char.IsDigit)) score++;
		if (p.Any(char.IsUpper) && p.Any(c => !char.IsLetterOrDigit(c))) score++;
		return Math.Max(score, 1);
	}

	void UpdateStrength()
	{
		int s = Strength(Password.Text);
		var color = s switch { 1 => Coral, 2 => Amber, 3 => Green, _ => Grey };
		Seg1.Color = s >= 1 ? color : Grey;
		Seg2.Color = s >= 2 ? color : Grey;
		Seg3.Color = s >= 3 ? color : Grey;
		StrengthLabel.Text = s switch
		{
			1 => "Force du mot de passe : faible",
			2 => "Force du mot de passe : moyenne",
			3 => "Force du mot de passe : forte",
			_ => "Force du mot de passe : —",
		};
	}

	bool IsFormValid()
		=> !string.IsNullOrWhiteSpace(FullName.Text)
		&& Ui.IsValidEmail(Email.Text?.Trim() ?? "")
		&& (Password.Text?.Length ?? 0) >= 8
		&& Confirm.Text == Password.Text
		&& Terms.IsChecked;

	void Refresh() => CreateButton.IsEnabled = IsFormValid();

	void OnTermsChanged(object sender, CheckedChangedEventArgs e)
	{
		TermsError.IsVisible = false;
		Refresh();
	}

	void OnTermsLabelTapped(object sender, TappedEventArgs e) => Terms.IsChecked = !Terms.IsChecked;

	async void OnCreateClicked(object sender, EventArgs e)
	{
		bool ok = ValidateName() & ValidateEmail() & ValidatePassword() & ValidateConfirm();
		if (!Terms.IsChecked) { TermsError.IsVisible = true; ok = false; }
		if (!ok) return;

		Loader.IsVisible = Loader.IsRunning = true;
		CreateButton.IsEnabled = false;
		await Task.Delay(900);
		Loader.IsVisible = Loader.IsRunning = false;

		AppData.UserName = FullName.Text.Trim();
		AppData.UserEmail = Email.Text.Trim();
		if (!AppData.KnownEmails.Contains(AppData.UserEmail)) AppData.KnownEmails.Add(AppData.UserEmail);

		await Shell.Current.GoToAsync("household");
	}

	async void OnBackClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("..");
	async void OnLoginLinkTapped(object sender, TappedEventArgs e) => await Shell.Current.GoToAsync("..");
}

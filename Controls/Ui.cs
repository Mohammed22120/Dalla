namespace Dalla.Controls;

public static class Ui
{
	/// <summary>Snackbar vert qui disparaît seul. 'snack' est un Border contenant un Label.</summary>
	public static async Task ShowSnackAsync(Border snack, string message, int visibleMs = 2200)
	{
		if (snack.Content is Label label) label.Text = message;
		snack.IsVisible = true;
		snack.Opacity = 0;
		await snack.FadeTo(1, 150);
		await Task.Delay(visibleMs);
		await snack.FadeTo(0, 150);
		snack.IsVisible = false;
	}

	public static bool IsValidEmail(string email)
	{
		if (string.IsNullOrWhiteSpace(email)) return false;
		var at = email.IndexOf('@');
		return at > 0 && email.IndexOf('.', at) > at + 1 && !email.EndsWith('.') && !email.Contains(' ');
	}

	public static bool TryParseAmount(string text, out long amount)
	{
		var cleaned = new string((text ?? "").Where(char.IsDigit).ToArray());
		return long.TryParse(cleaned, out amount);
	}
}

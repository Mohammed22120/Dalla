using Dalla.Controls;
using Dalla.Models;

namespace Dalla.Views;

public partial class HouseholdHubPage : ContentPage
{
	public HouseholdHubPage()
	{
		InitializeComponent();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		var members = AppData.VisibleMembers;
		HouseName.Text = AppData.HouseholdName;
		HouseInfo.Text = $"{members.Count} membre{(members.Count > 1 ? "s" : "")} · Budget {AppData.Money(AppData.GlobalBudget)} / mois";

		AvatarRow.Children.Clear();
		foreach (var m in members)
			AvatarRow.Children.Add(new AvatarView { Initial = m.Initial, AvatarColor = m.Color, Size = 36, StrokeThickness = 2 });
	}

	async void OnMembersTapped(object sender, TappedEventArgs e) => await Shell.Current.GoToAsync("members");
	async void OnBudgetTapped(object sender, TappedEventArgs e) => await Shell.Current.GoToAsync("budget");
	async void OnCategoriesTapped(object sender, TappedEventArgs e) => await Shell.Current.GoToAsync("categories");
	async void OnSettingsTapped(object sender, TappedEventArgs e) => await Shell.Current.GoToAsync("settings");
}

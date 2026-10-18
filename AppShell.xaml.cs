using Dalla.Views;

namespace Dalla;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		// Écrans ouverts « par-dessus » (pile de navigation)
		Routing.RegisterRoute("register", typeof(RegisterPage));
		Routing.RegisterRoute("forgot", typeof(ForgotPasswordPage));
		Routing.RegisterRoute("household", typeof(HouseholdSetupPage));
		Routing.RegisterRoute("add", typeof(AddExpensePage));
		Routing.RegisterRoute("detail", typeof(ExpenseDetailPage));
		Routing.RegisterRoute("members", typeof(MembersPage));
		Routing.RegisterRoute("budget", typeof(BudgetPage));
		Routing.RegisterRoute("categories", typeof(CategoriesPage));
		Routing.RegisterRoute("settings", typeof(SettingsPage));
	}
}

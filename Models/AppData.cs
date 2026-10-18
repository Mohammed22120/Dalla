using System.Globalization;

namespace Dalla.Models;

public class Member
{
	public string Id { get; set; }
	public string Name { get; set; }
	public string Role { get; set; }
	public string ColorHex { get; set; }
	public string Initial => string.IsNullOrWhiteSpace(Name) ? "?" : Name.Trim()[..1].ToUpperInvariant();
	public Color Color => Color.FromArgb(ColorHex);
}

public class Category
{
	public string Id { get; set; }
	public string Name { get; set; }
	public string Icon { get; set; }
	public string ColorHex { get; set; }
	public long Budget { get; set; }
	public Color Color => Color.FromArgb(ColorHex);
}

public class Expense
{
	public string Id { get; set; } = Guid.NewGuid().ToString();
	public string Label { get; set; }
	public long Amount { get; set; }
	public string CategoryId { get; set; }
	public string MemberId { get; set; }
	public DateTime Date { get; set; }
	public string Note { get; set; }
}

// ---------- Lignes affichées dans les listes (view-models simples) ----------

public class ExpenseRow
{
	public string Id { get; set; }
	public string Icon { get; set; }
	public string Label { get; set; }
	public string Sub { get; set; }
	public string AmountText { get; set; }
	public string Initial { get; set; }
	public Color MemberColor { get; set; }
	public Color CatColor { get; set; }
	public DateTime Date { get; set; }
}

public class ExpenseGroup : List<ExpenseRow>
{
	public string Title { get; }
	public ExpenseGroup(string title, IEnumerable<ExpenseRow> rows) : base(rows) => Title = title;
}

public class BudgetRow
{
	public string Id { get; set; }
	public string Icon { get; set; }
	public string Name { get; set; }
	public Color CatColor { get; set; }
	public long Spent { get; set; }
	public long Budget { get; set; }
	public double Progress { get; set; }
	public Color BarColor { get; set; }
	public string AmountText { get; set; }
	public string StatusText { get; set; }
	public string PercentText { get; set; }
}

public class MemberRow
{
	public string Id { get; set; }
	public string Name { get; set; }
	public string Role { get; set; }
	public string TotalText { get; set; }
	public string Initial { get; set; }
	public Color Color { get; set; }
}

public class LegendRow
{
	public string Name { get; set; }
	public Color Dot { get; set; }
	public string AmountText { get; set; }
	public string PercentText { get; set; }
}

// ---------- Données d'exemple + règles simples (front-end uniquement) ----------

public static class AppData
{
	static readonly CultureInfo Fr = new("fr-FR");

	public static string HouseholdName = "Famille Ismaïla";
	public static string Currency = "FCFA";
	public static long GlobalBudget = 700_000;
	public static string InviteCode = "DALLA-7K2P";
	public static bool IsAdmin = true;
	public static bool SimulateNetworkError = false;
	public static bool SimulateSoloHousehold = false;
	public static bool AlertNotifications = true;
	public static string UserName = "Ismaïla Bello";
	public static string UserEmail = "ismaila@exemple.cm";
	public static List<string> KnownEmails = new() { "ismaila@exemple.cm" };
	public static string CurrentMemberId = "m1";

	public static DateTime Today => DateTime.Today;
	public static DateTime MonthStart => new(Today.Year, Today.Month, 1);

	public static List<Member> Members = new()
	{
		new Member { Id = "m1", Name = "Ismaïla", Role = "Administrateur", ColorHex = "#1E5AA8" },
		new Member { Id = "m2", Name = "Aïcha",   Role = "Membre",         ColorHex = "#2E9E6B" },
		new Member { Id = "m3", Name = "Yanis",   Role = "Membre",         ColorHex = "#7C5CBF" },
		new Member { Id = "m4", Name = "Fatou",   Role = "Membre",         ColorHex = "#E8740C" },
	};

	public static List<Category> Categories = new()
	{
		new Category { Id = "alim", Name = "Alimentation", Icon = "shopping-cart", ColorHex = "#4C9A82", Budget = 150_000 },
		new Category { Id = "log",  Name = "Logement",     Icon = "house", ColorHex = "#1E5AA8", Budget = 200_000 },
		new Category { Id = "trans",Name = "Transport",    Icon = "bus", ColorHex = "#F5A623", Budget = 60_000 },
		new Category { Id = "sante",Name = "Santé",        Icon = "heart-pulse", ColorHex = "#D9689B", Budget = 40_000 },
		new Category { Id = "loi",  Name = "Loisirs",      Icon = "ticket", ColorHex = "#7C5CBF", Budget = 50_000 },
		new Category { Id = "edu",  Name = "Éducation",    Icon = "graduation-cap", ColorHex = "#2CB1BC", Budget = 70_000 },
		new Category { Id = "fact", Name = "Factures",     Icon = "receipt", ColorHex = "#6B7280", Budget = 80_000 },
		new Category { Id = "aut",  Name = "Autres",       Icon = "ellipsis", ColorHex = "#A3A3A3", Budget = 30_000 },
	};

	public static readonly string[] MemberPalette =
		{ "#1E5AA8", "#2E9E6B", "#7C5CBF", "#D9689B", "#E8740C", "#2CB1BC" };

	public static List<Expense> Expenses = BuildSample();

	static List<Expense> BuildSample()
	{
		DateTime D(int day) => new(Today.Year, Today.Month, day);
		return new List<Expense>
		{
			new() { Label = "Marché de Mokolo",  Amount = 45_000,  CategoryId = "alim",  MemberId = "m1", Date = D(3),  Note = "Courses de la semaine" },
			new() { Label = "Supermarché",       Amount = 38_000,  CategoryId = "alim",  MemberId = "m2", Date = D(8) },
			new() { Label = "Poisson et légumes",Amount = 42_000,  CategoryId = "alim",  MemberId = "m4", Date = D(12) },
			new() { Label = "Courses du week-end",Amount = 37_000, CategoryId = "alim",  MemberId = "m1", Date = D(15) },
			new() { Label = "Loyer d'octobre",   Amount = 150_000, CategoryId = "log",   MemberId = "m1", Date = D(1),  Note = "Virement au propriétaire" },
			new() { Label = "Taxi / moto",       Amount = 15_000,  CategoryId = "trans", MemberId = "m3", Date = D(5) },
			new() { Label = "Carburant",         Amount = 12_000,  CategoryId = "trans", MemberId = "m1", Date = D(9) },
			new() { Label = "Abonnement bus",    Amount = 21_500,  CategoryId = "trans", MemberId = "m2", Date = D(13) },
			new() { Label = "Pharmacie",         Amount = 12_000,  CategoryId = "sante", MemberId = "m2", Date = D(9),  Note = "Médicaments" },
			new() { Label = "Cinéma",            Amount = 18_000,  CategoryId = "loi",   MemberId = "m3", Date = D(11) },
			new() { Label = "Sortie en famille", Amount = 7_000,   CategoryId = "loi",   MemberId = "m4", Date = D(14) },
			new() { Label = "Frais de scolarité",Amount = 35_000,  CategoryId = "edu",   MemberId = "m3", Date = D(2) },
			new() { Label = "Électricité",       Amount = 27_500,  CategoryId = "fact",  MemberId = "m1", Date = D(6) },
			new() { Label = "Internet",          Amount = 18_000,  CategoryId = "fact",  MemberId = "m2", Date = D(7) },
			new() { Label = "Cadeau d'anniversaire", Amount = 5_000, CategoryId = "aut",MemberId = "m4", Date = D(16) },
		};
	}

	// ----- Utilitaires -----
	public static string FormatNumber(long value)
		=> value.ToString("N0", CultureInfo.InvariantCulture).Replace(",", " ");

	public static string Money(long value) => $"{FormatNumber(value)} {Currency}";

	public static string ShortDate(DateTime d) => d.ToString("d MMM yyyy", Fr);

	public static string LongDate(DateTime d)
		=> d.Date == Today ? "Aujourd'hui"
		 : d.Date == Today.AddDays(-1) ? "Hier"
		 : Fr.TextInfo.ToTitleCase(d.ToString("dddd d MMMM", Fr));

	public static string MonthLabel(DateTime d) => Fr.TextInfo.ToTitleCase(d.ToString("MMMM yyyy", Fr));

	public static Category GetCategory(string id) => Categories.FirstOrDefault(c => c.Id == id) ?? Categories.Last();
	public static Member GetMember(string id) => Members.FirstOrDefault(m => m.Id == id) ?? Members.First();
	public static Expense GetExpense(string id) => Expenses.FirstOrDefault(e => e.Id == id);

	public static IEnumerable<Expense> InMonth(DateTime month)
		=> Expenses.Where(e => e.Date.Year == month.Year && e.Date.Month == month.Month);

	public static long TotalSpent(DateTime month) => InMonth(month).Sum(e => e.Amount);
	public static long SpentIn(string categoryId, DateTime month) => InMonth(month).Where(e => e.CategoryId == categoryId).Sum(e => e.Amount);
	public static long SpentBy(string memberId, DateTime month) => InMonth(month).Where(e => e.MemberId == memberId).Sum(e => e.Amount);

	// ----- Lignes -----
	public static ExpenseRow ToRow(Expense e)
	{
		var c = GetCategory(e.CategoryId);
		var m = GetMember(e.MemberId);
		return new ExpenseRow
		{
			Id = e.Id,
			Icon = c.Icon,
			Label = e.Label,
			Sub = $"{c.Name} · {m.Name} · {ShortDate(e.Date)}",
			AmountText = $"−{Money(e.Amount)}",
			Initial = m.Initial,
			MemberColor = m.Color,
			CatColor = c.Color,
			Date = e.Date,
		};
	}

	public static List<ExpenseRow> RecentRows(int count)
		=> Expenses.OrderByDescending(e => e.Date).Take(count).Select(ToRow).ToList();

	public static BudgetRow ToBudgetRow(Category c, DateTime month)
	{
		var spent = SpentIn(c.Id, month);
		double ratio = c.Budget > 0 ? (double)spent / c.Budget : 0;
		string status;
		Color bar;
		if (ratio >= 1)
		{
			bar = Color.FromArgb("#E4572E");
			status = $"⚠ Dépassé de {Money(spent - c.Budget)}";
		}
		else if (ratio >= 0.8)
		{
			bar = Color.FromArgb("#F5A623");
			status = $"⚠ Bientôt atteint ({(int)Math.Round(ratio * 100)} %)";
		}
		else
		{
			bar = Color.FromArgb("#2E9E6B");
			status = "✓ Dans le budget";
		}
		return new BudgetRow
		{
			Id = c.Id, Icon = c.Icon, Name = c.Name, CatColor = c.Color,
			Spent = spent, Budget = c.Budget,
			Progress = Math.Min(ratio, 1),
			BarColor = bar,
			AmountText = $"{FormatNumber(spent)} / {Money(c.Budget)}",
			StatusText = status,
			PercentText = $"{(int)Math.Round(ratio * 100)} %",
		};
	}

	public static List<BudgetRow> BudgetRows(DateTime month)
		=> Categories.Select(c => ToBudgetRow(c, month)).ToList();

	public static List<MemberRow> MemberRows(DateTime month)
		=> VisibleMembers.Select(m => new MemberRow
		{
			Id = m.Id, Name = m.Name, Role = m.Role, Initial = m.Initial, Color = m.Color,
			TotalText = $"{Money(SpentBy(m.Id, month))} ce mois-ci",
		}).ToList();

	/// <summary>Texte d'alerte pour le tableau de bord (null = aucune alerte).</summary>
	public static (string Text, bool Exceeded)? CurrentAlert(DateTime month)
	{
		var rows = BudgetRows(month);
		var over = rows.FirstOrDefault(r => r.Budget > 0 && r.Spent > r.Budget);
		if (over != null)
			return ($"Attention : budget {over.Name} dépassé de {Money(over.Spent - over.Budget)}", true);
		var near = rows.FirstOrDefault(r => r.Budget > 0 && (double)r.Spent / r.Budget >= 0.8);
		if (near != null)
			return ($"Budget {near.Name} presque atteint ({near.PercentText}).", false);
		return null;
	}

	public static void Save(Expense e)
	{
		var existing = GetExpense(e.Id);
		if (existing == null) Expenses.Add(e);
		else
		{
			existing.Label = e.Label; existing.Amount = e.Amount; existing.CategoryId = e.CategoryId;
			existing.MemberId = e.MemberId; existing.Date = e.Date; existing.Note = e.Note;
		}
	}

	public static void Delete(string id) => Expenses.RemoveAll(e => e.Id == id);

	public static void ClearExpenses() => Expenses = new List<Expense>();
	public static void RestoreSample() => Expenses = BuildSample();

	/// <summary>Membres affichés (un seul si on simule un foyer vide).</summary>
	public static List<Member> VisibleMembers => SimulateSoloHousehold ? Members.Take(1).ToList() : Members;

	public static readonly string[] MonthsLabels = { "mai", "juin", "juil.", "août", "sept.", "oct." };
	public static readonly double[] SixMonthsTotals = { 410_000, 455_000, 520_000, 498_000, 441_000, 0 };
}

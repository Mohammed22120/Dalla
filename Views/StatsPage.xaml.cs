using System.Globalization;
using Dalla.Controls;
using Dalla.Models;

namespace Dalla.Views;

public partial class StatsPage : ContentPage
{
	static readonly CultureInfo Fr = new("fr-FR");

	readonly DonutDrawable _donut = new();
	readonly BarsDrawable _bars = new();
	readonly LineDrawable _line = new();
	DateTime _month = AppData.MonthStart;

	public StatsPage()
	{
		InitializeComponent();
		DonutView.Drawable = _donut;
		BarsView.Drawable = _bars;
		LineView.Drawable = _line;
		Empty.ActionClicked += async (_, _) => await Shell.Current.GoToAsync("add");
		Application.Current.RequestedThemeChanged += (_, _) => InvalidateAll();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		Reload();
	}

	void InvalidateAll()
	{
		DonutView.Invalidate();
		BarsView.Invalidate();
		LineView.Invalidate();
	}

	void OnPrevClicked(object sender, EventArgs e) { _month = _month.AddMonths(-1); Reload(); }
	void OnNextClicked(object sender, EventArgs e) { if (_month < AppData.MonthStart) { _month = _month.AddMonths(1); Reload(); } }

	void Reload()
	{
		MonthLabel.Text = AppData.MonthLabel(_month);
		NextButton.IsEnabled = _month < AppData.MonthStart;
		NextButton.Opacity = NextButton.IsEnabled ? 1 : 0.35;

		long total = AppData.TotalSpent(_month);
		bool empty = total <= 0;
		StatsContent.IsVisible = !empty;
		EmptyCard.IsVisible = empty;
		if (empty) return;

		// Anneau + légende textuelle
		var cats = AppData.Categories
			.Select(c => (Cat: c, Spent: AppData.SpentIn(c.Id, _month)))
			.Where(x => x.Spent > 0)
			.OrderByDescending(x => x.Spent)
			.ToList();
		_donut.Slices = cats.Select(x => ((double)x.Spent, x.Cat.Color)).ToList();
		_donut.CenterTop = AppData.FormatNumber(total);
		_donut.CenterBottom = AppData.Currency;
		BindableLayout.SetItemsSource(LegendList, cats.Select(x => new LegendRow
		{
			Name = x.Cat.Name,
			Dot = x.Cat.Color,
			AmountText = AppData.FormatNumber(x.Spent),
			PercentText = $"{(int)Math.Round((double)x.Spent / total * 100)} %",
		}).ToList());

		// Histogramme par membre
		_bars.Bars = AppData.VisibleMembers
			.Select(m => (m.Name, (double)AppData.SpentBy(m.Id, _month), m.Color))
			.ToList();

		// Courbe sur 6 mois (les 5 mois passés sont des données d'exemple)
		var pts = new List<(string, double)>();
		for (int i = 0; i < 6; i++)
		{
			var d = AppData.MonthStart.AddMonths(i - 5);
			double v = i == 5 ? AppData.TotalSpent(AppData.MonthStart) : AppData.SixMonthsTotals[i];
			pts.Add((d.ToString("MMM", Fr).TrimEnd('.'), v));
		}
		_line.Points = pts;
		var last = pts[5].Item2; var prev = pts[4].Item2;
		if (prev > 0)
		{
			int diff = (int)Math.Round((last - prev) / prev * 100);
			LineSummary.Text = diff >= 0
				? $"▲ +{diff} % par rapport au mois dernier"
				: $"▼ −{Math.Abs(diff)} % par rapport au mois dernier";
		}

		InvalidateAll();
	}
}

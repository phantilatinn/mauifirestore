using MauiFirestore;

namespace MauiFireStore;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
		MainPage=  new SamplePage();
	}
}
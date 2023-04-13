namespace MauiFlyoutBug;

public partial class App : Application
{
public App()
{
	InitializeComponent();

	var flyout = new FlyoutPage() { Title = "Navigation bug" };
	flyout.Flyout = new NavigationPage(new FlyoutContentPage()){Title="Needs a title"};
	flyout.Detail = new MainPage() { Title = "Another title" };
	MainPage = flyout;
	flyout.IsPresented = true;
}
}

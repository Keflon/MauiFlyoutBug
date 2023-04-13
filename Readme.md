# MauiFlyoutBug

Putting a navigation stack into a FlyoutPage Flyout property causes a few problems.

## Repro:

1. Set a root-level flyout page
1. Set the Flyout property to a navigationPage
1. Push some ContentPage instances into the navigation page

 ## iOS and WinUI
 Both platforms add a 'back button' within the flyout and things work as expected.
![image](https://user-images.githubusercontent.com/16598898/231740856-b6827e6a-97d3-42dd-8e54-167c84655f42.png)


## Android

1. Pushing pages onto the nav stack renders the 'back button' in the `FlyoutPage.Content` page  
   -  It ought to be in the `FlyoutPage.Flyout` page but it is not.
1. Tapping the back button (on the wrong ContentPage) does not pop from FlyoutPage.Flyout.
1. Setting `flyout.IsPresented = true;` is not respected.
1. The flyout is reluctant to emerge using gestures. (using Pixel 5 emulator, debug build.)
   - In the sample, I cannot get the flyout to show until I have clicked the button on the Flyout.Content page.  
![image](https://user-images.githubusercontent.com/16598898/231741947-35621e39-cd19-48cc-b166-f67a716478cb.png)  

Found it!
![image](https://user-images.githubusercontent.com/16598898/231742410-feeeff6e-cf86-48f8-8258-64d9f7d577d4.png)




1. Create a File->New NET MAUI app.
2. Create a FlyoutPage.
3. Set FlyoutPage.Flyout to a new NavigationPage and push some ContentPage instances onto it.
4. Set FlyoutPage.Content to something.
5. Set App.MainPage to the FlyoutPage.
6. Set FlyoutPage.IsPresented to true.
7. Observe on Android the FlyoutPage.Flyout
    - IsPresented is not respected.
    - The Flyout cannot be coaxed into view until a button has been pressed on FlyoutPage.Content.
    - The Flyout does not have a back button when it should.
    - The Flyout Content does have a back button when it should not.

```csharp
public App()
{
	InitializeComponent();

	var flyout = new FlyoutPage() { Title = "Navigation bug" };
	flyout.Flyout = new NavigationPage(new FlyoutContentPage()){Title="Needs a title"};
	flyout.Detail = new MainPage() { Title = "Another title" };
	MainPage = flyout;
	flyout.IsPresented = true;
}
```
```xaml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MauiFlyoutBug.FlyoutContentPage"
             Title="FlyoutContentPage"
             BackgroundColor="Cyan">
    <VerticalStackLayout>
        <Label 
            Text="Flyout Content"
            VerticalOptions="Center" 
            HorizontalOptions="Center" />

        <Label x:Name="StackDepthLabel"
               Text="Stack depth: 0"
            VerticalOptions="Center" 
            HorizontalOptions="Center" />

        <Button Text="Push a page" Clicked="Button_Clicked"/>
    </VerticalStackLayout>
</ContentPage>
```
```csharp
public partial class FlyoutContentPage : ContentPage
{
    public FlyoutContentPage()
    {
        InitializeComponent();
    }

    string LabelText { get => StackDepthLabel.Text; set => StackDepthLabel.Text = value; }

    private void Button_Clicked(object sender, EventArgs e)
    {
        // Why doesn't the following line compile?
        //this.Navigation.PushAsync(new FlyoutContentPage() { this.StackDepthLabel.Text = "" });

        this.Navigation.PushAsync(new FlyoutContentPage() { LabelText = $"Stack depth: {this.Navigation.NavigationStack.Count}" });
    }
}
```
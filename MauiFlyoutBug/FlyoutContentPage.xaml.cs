namespace MauiFlyoutBug;

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

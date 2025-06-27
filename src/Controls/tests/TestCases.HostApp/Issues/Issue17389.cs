namespace Maui.Controls.Sample.Issues;

[Issue(IssueTracker.Github, 21331, "InputTransparent should not affect background color on Windows layouts", PlatformAffected.UWP)]
public class Issue17389 : TestContentPage
{
    Grid redGrid;
    Grid greenGrid;
    StackLayout blueStack;
    ContentView purpleContent;
    Label tapCountLabel;
    Label redGridLabel;
    Label greenGridLabel;
    Label blueStackLabel;
    Label purpleContentLabel;
    int tapCount;

    protected override void Init()
    {
        tapCountLabel = new Label { Text = "Tap count: 0", HorizontalOptions = LayoutOptions.Center };

        redGrid = CreateBackgroundTestGrid(Colors.Red, true, "RedGrid", out redGridLabel);
        greenGrid = CreateBackgroundTestGrid(Colors.Green, true, "GreenGrid", out greenGridLabel);

        blueStackLabel = new Label { Text = "Blue Stack (InputTransparent=True)", HorizontalOptions = LayoutOptions.Center, AutomationId = "BlueStack" };
        blueStack = new StackLayout
        {
            BackgroundColor = Colors.Blue,
            InputTransparent = true,
            WidthRequest = 200,
            HeightRequest = 100,
            Children = { blueStackLabel }
        };
        AddTapGesture(blueStack);

        purpleContentLabel = new Label { Text = "Purple Content (InputTransparent=True)", HorizontalOptions = LayoutOptions.Center, AutomationId = "PurpleContent" };
        purpleContent = new ContentView
        {
            BackgroundColor = Colors.Purple,
            InputTransparent = true,
            WidthRequest = 200,
            HeightRequest = 100,
            Content = purpleContentLabel
        };
        AddTapGesture(purpleContent);

        Content = CreateMainContent();
    }

    ScrollView CreateMainContent()
    {
        return new ScrollView
        {
            Content = new StackLayout
            {
                Spacing = 20,
                Children =
                {
                    new Label
                    {
                        Text = "InputTransparent Background Test",
                        FontSize = 18,
                        FontAttributes = FontAttributes.Bold,
                        HorizontalOptions = LayoutOptions.Center
                    },
                    tapCountLabel,
                    new Button
                    {
                        Text = "Toggle InputTransparent",
                        Command = new Command(ToggleInputTransparent),
                        AutomationId = "ToggleInputTransparentButton"
                    },
                    new Button
                    {
                        Text = "Toggle Background Colors",
                        Command = new Command(ToggleBackgroundColors),
                        AutomationId = "ToggleBackgroundColorsButton"
                    },
                    redGrid,
                    greenGrid,
                    blueStack,
                    purpleContent
                }
            }
        };
    }

    Grid CreateBackgroundTestGrid(Color bgColor, bool inputTransparent, string labelText, out Label label)
    {
        label = new Label { Text = $"{labelText} (InputTransparent=True)", HorizontalOptions = LayoutOptions.Center, AutomationId = $"{labelText}" };

        Grid childGrid = new Grid
        {
            BackgroundColor = bgColor,
            InputTransparent = inputTransparent,
            Children = { label }
        };

        AddTapGesture(childGrid);

        Grid parentGrid = new Grid
        {
            WidthRequest = 200,
            HeightRequest = 100,
            InputTransparent = inputTransparent,
            BackgroundColor = Colors.LightGray,
            Children = { childGrid }
        };

        AddTapGesture(parentGrid);
        return parentGrid;
    }

    void AddTapGesture(View view)
    {
        view.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(() =>
            {
                tapCount++;
                tapCountLabel.Text = $"Tap count: {tapCount}";
            })
        });
    }

    void ToggleInputTransparent()
    {
        redGrid.InputTransparent = !redGrid.InputTransparent;
        greenGrid.InputTransparent = !greenGrid.InputTransparent;
        blueStack.InputTransparent = !blueStack.InputTransparent;
        purpleContent.InputTransparent = !purpleContent.InputTransparent;

        redGridLabel.Text = $"Red Grid (InputTransparent={redGrid.InputTransparent})";
        greenGridLabel.Text = $"Green Grid (InputTransparent={greenGrid.InputTransparent})";
        blueStackLabel.Text = $"Blue Stack (InputTransparent={blueStack.InputTransparent})";
        purpleContentLabel.Text = $"Purple Content (InputTransparent={purpleContent.InputTransparent})";
    }

    void ToggleBackgroundColors()
    {
        redGrid.BackgroundColor = Colors.Orange;
        greenGrid.BackgroundColor = Colors.LightGreen;
        blueStack.BackgroundColor = Colors.LightBlue;
        purpleContent.BackgroundColor = Colors.Pink;
    }
}
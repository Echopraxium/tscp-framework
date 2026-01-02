// uuid: a2b3c4d5-e6f7-4890-b1c2-d3e4f5a6b7c8
namespace TSCP.GUI

open Avalonia
open Avalonia.Controls
open Avalonia.Layout
open Avalonia.Media
open Avalonia.Media.Imaging
open Avalonia.Themes.Fluent
open Avalonia.Controls.ApplicationLifetimes
open System
open System.IO
open System.Diagnostics
open TSCP.Core
open TSCP.Session

/// <summary>
/// Optimized TSCP IDE for Laptop: Compact Tab Fonts (50% reduction), 
/// Horizontal Toolbar layout, and Corrected Dialog sizing.
/// </summary>
type MainWindow(state: SessionState) as self =
    inherit Window()
    
    // UI Constants
    let beigeBg = Color.Parse("#FFFFF0") |> SolidColorBrush
    let lightGraySplitter = Color.Parse("#D3D3D3") |> SolidColorBrush
    let tabFontSize = 6.0 // 50% smaller than standard
    let splitterWidth = 2.6
    
    let loadIcon (name: string) =
        let path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "icons", name)
        if File.Exists(path) then 
            let bitmap = new Bitmap(path)
            new Image(Source = bitmap, Width = 16.0, Height = 16.0) :> Control
        else new TextBlock(Text = "□") :> Control

    do
        self.Title <- "TSCP Framework"
        self.Width <- 1024.0
        self.Height <- 680.0
        self.Background <- beigeBg
        self.WindowStartupLocation <- WindowStartupLocation.CenterScreen

        let rootDock = new DockPanel()

        // --- TOP SECTION: MENU + TOOLBAR (Horizontal & Full Width) ---
        let topContainer = new StackPanel(Orientation = Orientation.Vertical)
        
        // Menu Bar
        let menuBar = new Menu(Background = Brushes.WhiteSmoke)
        let fileMenu = new MenuItem(Header = "_File")
        let quitItem = new MenuItem(Header = "Quit")
        quitItem.Click.Add(fun _ -> self.Close())
        fileMenu.Items.Add(quitItem) |> ignore
        
        let editMenu = new MenuItem(Header = "_Edit")
        
        let helpMenu = new MenuItem(Header = "_Help")
        let aboutItem = new MenuItem(Header = "About..")
        aboutItem.Click.Add(fun _ -> 
            // Corrected Dialog Size: Much smaller
            let aboutWin = new Window(Title="About TSCP", Width=320.0, Height=180.0, 
                                      WindowStartupLocation=WindowStartupLocation.CenterOwner,
                                      Background=beigeBg, CanResize=false,
                                      SystemDecorations=SystemDecorations.BorderOnly)
            
            let contentStack = new StackPanel(Margin=Thickness(15.0), Spacing=8.0)
            let titleLabel = new TextBlock(Text="TSCP (Transdisciplinary System Construction Principles)", 
                                           FontSize=10.0, FontWeight=FontWeight.Bold, TextWrapping=TextWrapping.Wrap)
            let authorsLabel = new TextBlock(Text="Authors: Echopraxium with the support of Google Gemini Search", 
                                             FontSize=7.0, TextWrapping=TextWrapping.Wrap)
            let linkBtn = new Button(Content="TSCP-Framework on GitHub", Foreground=Brushes.Blue, 
                                     Background=Brushes.Transparent, BorderThickness=Thickness(0.0), FontSize=7.0)
            linkBtn.Click.Add(fun _ -> Process.Start(new ProcessStartInfo("https://github.com/echopraxium/TSCP-Framework", UseShellExecute=true)) |> ignore)
            let okBtn = new Button(Content="OK", Width=50.0, HorizontalAlignment=HorizontalAlignment.Right)
            okBtn.Click.Add(fun _ -> aboutWin.Close())

            contentStack.Children.Add(titleLabel) |> ignore
            contentStack.Children.Add(authorsLabel) |> ignore
            contentStack.Children.Add(linkBtn) |> ignore
            contentStack.Children.Add(okBtn) |> ignore
            aboutWin.Content <- contentStack
            aboutWin.ShowDialog(self) |> ignore
        )
        helpMenu.Items.Add(aboutItem) |> ignore
        
        menuBar.Items.Add(fileMenu) |> ignore
        menuBar.Items.Add(editMenu) |> ignore
        menuBar.Items.Add(helpMenu) |> ignore
        
        // Toolbar: Under Menu, Full Horizontal Width
        let toolbar = new DockPanel(Background = Brushes.AliceBlue, Height = 32.0)
        let buttonStack = new StackPanel(Orientation = Orientation.Horizontal)
        let toolBtn (ico: string) = new Button(Content = loadIcon ico, Margin = Thickness(2.0, 0.0), Background = Brushes.Transparent)
        buttonStack.Children.Add(toolBtn "file_new_24px_icn.png") |> ignore
        buttonStack.Children.Add(toolBtn "file_open_24px_icn.png") |> ignore
        buttonStack.Children.Add(toolBtn "save_24px_icn.png") |> ignore
        buttonStack.Children.Add(toolBtn "save_as_24px_icn.png") |> ignore
        
        let userLogo = new Border(Width=24.0, Height=24.0, CornerRadius=CornerRadius(12.0), Background=Brushes.RoyalBlue, Margin=Thickness(0.0, 0.0, 10.0, 0.0))
        userLogo.Child <- new TextBlock(Text="G", Foreground=Brushes.White, HorizontalAlignment=HorizontalAlignment.Center, VerticalAlignment=VerticalAlignment.Center, FontSize=10.0)
        DockPanel.SetDock(userLogo, Dock.Right)

        toolbar.Children.Add(userLogo) |> ignore
        toolbar.Children.Add(buttonStack) |> ignore

        topContainer.Children.Add(menuBar) |> ignore
        topContainer.Children.Add(toolbar) |> ignore
        DockPanel.SetDock(topContainer, Dock.Top)
        rootDock.Children.Add(topContainer) |> ignore

        // --- MAIN WORKSPACE GRID ---
        let mainGrid = new Grid()
        mainGrid.ColumnDefinitions.Add(new ColumnDefinition(Width = GridLength(180.0))) 
        mainGrid.ColumnDefinitions.Add(new ColumnDefinition(Width = GridLength(splitterWidth))) 
        mainGrid.ColumnDefinitions.Add(new ColumnDefinition(Width = new GridLength(1.0, GridUnitType.Star))) 
        mainGrid.ColumnDefinitions.Add(new ColumnDefinition(Width = GridLength(splitterWidth))) 
        mainGrid.ColumnDefinitions.Add(new ColumnDefinition(Width = GridLength(220.0))) 

        // Splitters
        let splitL = new GridSplitter(Background=lightGraySplitter, ResizeBehavior=GridResizeBehavior.PreviousAndNext)
        Grid.SetColumn(splitL, 1)
        let splitR = new GridSplitter(Background=lightGraySplitter, ResizeBehavior=GridResizeBehavior.PreviousAndNext)
        Grid.SetColumn(splitR, 3)

        // Ontology Explorer
        let explorer = new Border(Child=new TextBlock(Text="Ontology Explorer", FontSize=tabFontSize, Margin=Thickness(5.0)))
        Grid.SetColumn(explorer, 0)
        
        // Right Panel
        let rightTabs = new TabControl(FontSize=tabFontSize)
        rightTabs.Items.Add(new TabItem(Header="Properties", Content=new TextBlock(Text="Metadata View", FontSize=8.0))) |> ignore
        rightTabs.Items.Add(new TabItem(Header="ChatBot", Content=new TextBlock(Text="M3 AI Assistant", FontSize=8.0))) |> ignore
        Grid.SetColumn(rightTabs, 4)

        // Center Workspace
        let centerGrid = new Grid()
        centerGrid.RowDefinitions.Add(new RowDefinition(Height = new GridLength(1.0, GridUnitType.Star)))
        centerGrid.RowDefinitions.Add(new RowDefinition(Height = GridLength(splitterWidth))) 
        centerGrid.RowDefinitions.Add(new RowDefinition(Height = GridLength(140.0)))

        let workspaceTabs = new TabControl(FontSize=tabFontSize)
        workspaceTabs.Items.Add(new TabItem(Header="3D View", Content=new Border(Background=Brushes.Black))) |> ignore
        workspaceTabs.Items.Add(new TabItem(Header="Text Editor", Content=new TextBox(FontSize=8.0, Background=beigeBg))) |> ignore
        workspaceTabs.Items.Add(new TabItem(Header="GraphViz", Content=new TextBlock(Text="Graph", FontSize=8.0))) |> ignore
        Grid.SetRow(workspaceTabs, 0)

        let splitB = new GridSplitter(Height=splitterWidth, HorizontalAlignment=HorizontalAlignment.Stretch, Background=lightGraySplitter)
        Grid.SetRow(splitB, 1)

        let bottomTabs = new TabControl(FontSize=tabFontSize)
        let console = new ListBox(Background=Brushes.Black, Foreground=Brushes.White, FontFamily=FontFamily("Consolas"), FontSize=8.0)
        state.History |> List.iter (fun e -> 
            let t = match e with | Log m -> sprintf "[LOG] %s" m | ActiveConcept c -> sprintf "[CONCEPT] %s" c.Name
            console.Items.Add(t :> obj) |> ignore)
        bottomTabs.Items.Add(new TabItem(Header="TSCP-CLI", Content=console)) |> ignore
        bottomTabs.Items.Add(new TabItem(Header="Statistics", Content=new TextBlock(Text="Stats", FontSize=8.0))) |> ignore
        Grid.SetRow(bottomTabs, 2)

        centerGrid.Children.Add(workspaceTabs) |> ignore
        centerGrid.Children.Add(splitB) |> ignore
        centerGrid.Children.Add(bottomTabs) |> ignore
        Grid.SetColumn(centerGrid, 2)

        mainGrid.Children.Add(explorer) |> ignore
        mainGrid.Children.Add(splitL) |> ignore
        mainGrid.Children.Add(centerGrid) |> ignore
        mainGrid.Children.Add(splitR) |> ignore
        mainGrid.Children.Add(rightTabs) |> ignore

        rootDock.Children.Add(mainGrid) |> ignore
        self.Content <- rootDock

type App() =
    inherit Application()
    override this.Initialize() = this.Styles.Add(FluentTheme())
    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as d -> d.MainWindow <- MainWindow(SessionManager.loadSession())
        | _ -> ()
        base.OnFrameworkInitializationCompleted()

module GuiMain =
    [<EntryPoint>]
    let main args = AppBuilder.Configure<App>().UsePlatformDetect().StartWithClassicDesktopLifetime(args)
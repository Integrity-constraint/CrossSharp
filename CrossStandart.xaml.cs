﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Runtime.InteropServices;


namespace CrossSharp
{
    /// <summary>
    /// Логика взаимодействия для CrossStandart.xaml
    /// </summary>
    public partial class CrossStandart : Window
    {
        const int WS_EX_TRANSPARENT = 0x00000020;
        const int GWL_EXSTYLE = (-20);

        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        
        Line verticalLine;
        Line horizontalLine;
        public CrossStandart()
        {
            InitializeComponent();
            this.Topmost = true;
            RoutedCommand hotkeyCommand = new RoutedCommand();
            RoutedCommand hotkeyCommandcolor = new RoutedCommand();
            RoutedCommand hotkeyCommandExit = new RoutedCommand();
            hotkeyCommandcolor.InputGestures.Add(new KeyGesture(Key.F1, ModifierKeys.Shift));
            hotkeyCommand.InputGestures.Add(new KeyGesture(Key.F1, ModifierKeys.Control));
            hotkeyCommandExit.InputGestures.Add(new KeyGesture(Key.F2, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(hotkeyCommand, HotkeyCommandExecuted));
            CommandBindings.Add(new CommandBinding(hotkeyCommandcolor, HotkeyCommandColorChange));
            CommandBindings.Add(new CommandBinding(hotkeyCommandExit, HotkeyCommandexitd));

            this.WindowStyle = WindowStyle.None;
            this.WindowState = WindowState.Maximized;
            this.Topmost = true;
            this.Background = Brushes.Transparent;
            this.AllowsTransparency = true;

            verticalLine = new Line
            {
                Stroke = Brushes.Red,
                StrokeThickness = 2
            };

            horizontalLine = new Line
            {
                Stroke = Brushes.Red,
                StrokeThickness = 2
            };

            this.Content = new Canvas();
            ((Canvas)this.Content).Children.Add(verticalLine);
            ((Canvas)this.Content).Children.Add(horizontalLine);

            this.SizeChanged += MainWindow_SizeChanged;
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            int crosshairSize = 50; // Размер прицела

            verticalLine.X1 = this.Width / 2;
            verticalLine.X2 = this.Width / 2;
            verticalLine.Y1 = this.Height / 2 - crosshairSize / 2;
            verticalLine.Y2 = this.Height / 2 + crosshairSize / 2;

            horizontalLine.X1 = this.Width / 2 - crosshairSize / 2;
            horizontalLine.X2 = this.Width / 2 + crosshairSize / 2;
            horizontalLine.Y1 = this.Height / 2;
            horizontalLine.Y2 = this.Height / 2;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

           
            IntPtr hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;

            int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);

            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
        }

        private void HotkeyCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
           
            this.Close();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void HotkeyCommandexitd(object sender, ExecutedRoutedEventArgs e)
        {
           App.Current.Shutdown();
        }

        private void HotkeyCommandColorChange(object sender, ExecutedRoutedEventArgs e)
        {
            Brush[] colors = { Brushes.Red, Brushes.Blue, Brushes.Green, Brushes.Yellow, Brushes.Purple };

           
            Random random = new Random();
            int index = random.Next(colors.Length);

            verticalLine.Stroke = colors[index];
            horizontalLine.Stroke = colors[index];
        }
    }


    }


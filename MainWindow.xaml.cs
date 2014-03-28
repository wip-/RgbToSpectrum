using System;
using System.Windows;
using System.Windows.Media;
using Helpers;

namespace RgbToSpectrum
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool initialized = false;

        public MainWindow()
        {
            InitializeComponent();
            initialized = true;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!initialized)
                return;

            byte R = SliderR.Value.ToByte();
            byte G = SliderG.Value.ToByte();
            byte B = SliderB.Value.ToByte();

            LabelR.Content = String.Format("R={0:000}", R);
            LabelG.Content = String.Format("G={0:000}", G);
            LabelB.Content = String.Format("B={0:000}", B);

            var brush = new SolidColorBrush();
            brush.Color = Color.FromArgb(255, R, G, B);
            Background = brush;

            // TODO

            // spectrum = new Spectrum(R, G, B);
            // bitmap = new SpectrumCurve(spectrum);




















        }
    }
}

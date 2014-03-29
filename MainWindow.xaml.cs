using System;
using System.Windows;
using System.Windows.Media;
using WilCommon;

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
            Slider_ValueChanged(null, null);
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!initialized)
                return;

            var R = SliderR.Value;
            var G = SliderG.Value;
            var B = SliderB.Value;

            LabelR.Content = String.Format("R={0:000}", R);
            LabelG.Content = String.Format("G={0:000}", G);
            LabelB.Content = String.Format("B={0:000}", B);

            var brush = new SolidColorBrush();
            brush.Color = Color.FromArgb(255, R.ToByte(), G.ToByte(), B.ToByte());
            Background = brush;

            SimpleSpectrum spectrum = new SimpleSpectrum( R / 255, G / 255, B / 255);
            Image.Source = spectrum.ToBitmap().ToBitmapSource();

            // TODO more precision in RGBCYM spectra
        }
    }
}

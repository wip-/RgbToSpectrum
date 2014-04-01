using System;
using System.Windows;
using System.Windows.Media;
using System.Linq;
using WilCommon;

namespace RgbToSpectrum
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool initialized = false;
        SimpleSpectrum spectrum;
        Filter filter;
        FilteredSpectrum filtered;

        public enum FilterType : int
        {
            Warming85,
            Custom
        }

        public enum DisplayType : int
        {
            SnakeCurves,
            CatmullRomSplines
        }

        public MainWindow()
        {
            InitializeComponent();
            ComboBoxFilter.ItemsSource = Enum.GetValues(typeof(FilterType)).Cast<FilterType>();
            ComboBoxFilter.SelectedIndex = 0;
            ComboBoxDisplay.ItemsSource = Enum.GetValues(typeof(DisplayType)).Cast<DisplayType>();
            ComboBoxDisplay.SelectedIndex = 0;

            initialized = true;

            UpdateInputSpectrum();
            UpdateFilter();
            UpdateFilteredSpectrum();
        }

        private void SliderInputColor_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateInputSpectrum();
            UpdateFilteredSpectrum();
        }

        private void ComboBoxFilter_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            UpdateFilter();
            UpdateFilteredSpectrum();
        }

        private void ComboBoxDisplay_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            UpdateInputSpectrum();
            UpdateFilter();
            UpdateFilteredSpectrum();
        }

        void UpdateInputSpectrum()
        {
            if (!initialized)
                return;

            var R = SliderR.Value;
            var G = SliderG.Value;
            var B = SliderB.Value;

            LabelRin.Content = String.Format("R={0:000}", R);
            LabelGin.Content = String.Format("G={0:000}", G);
            LabelBin.Content = String.Format("B={0:000}", B);

            var brush = new SolidColorBrush();
            brush.Color = Color.FromArgb(255, R.ToByte(), G.ToByte(), B.ToByte());
            InputColorGrid.Background = brush;

            spectrum = new SimpleSpectrum(R / 255, G / 255, B / 255);
            ImageInput.Source = spectrum.ToBitmap(ComboBoxDisplay.SelectedIndex==(int)DisplayType.CatmullRomSplines).ToBitmapSource();

            // TODO more precision in RGBCYM spectra (lux render has data for 32 bins)
        }

        void UpdateFilter()
        {
            if (!initialized)
                return;

            filter = new Filter();
            ImageFilter.Source = filter.ToBitmap(ComboBoxDisplay.SelectedIndex == (int)DisplayType.CatmullRomSplines).ToBitmapSource();
        }

        void UpdateFilteredSpectrum()
        {
            if (!initialized || filter==null || spectrum==null)
                return;

            filtered = new FilteredSpectrum(spectrum, filter);
            ImageOutput.Source = filtered.ToBitmap(ComboBoxDisplay.SelectedIndex == (int)DisplayType.CatmullRomSplines).ToBitmapSource();

            XYZColor xyz = new XYZColor(filtered);
            System.Drawing.Color rgb = xyz.ToRGB();
            var brush = new SolidColorBrush();
            brush.Color = Color.FromArgb(rgb.A, rgb.R, rgb.G, rgb.B);
            OutputColorGrid.Background = brush;

            LabelRout.Content = String.Format("R={0:000}", rgb.R);
            LabelGout.Content = String.Format("G={0:000}", rgb.G);
            LabelBout.Content = String.Format("B={0:000}", rgb.B);
        }


    }
}

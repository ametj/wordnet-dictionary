using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace WordNet.Wpf.Controls
{
    public class LoadingAnimation
    {
        public const string Continuous = "ContinuousAnimation";
        public const string WithPause = "WithPauseAnimation";
    }

    public partial class Loading : UserControl
    {
        public static readonly DependencyProperty HideDelayProperty =
            DependencyProperty.Register(nameof(HideDelay), typeof(int), typeof(Loading), new FrameworkPropertyMetadata(0));

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register(nameof(IsLoading), typeof(bool), typeof(Loading), new PropertyMetadata(false, OnIsLoadingChanged));

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(nameof(StrokeThickness), typeof(int), typeof(Loading), new PropertyMetadata(3, OnPropertyChanged));

        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register(nameof(Stroke), typeof(Brush), typeof(Loading), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register(nameof(Radius), typeof(int), typeof(Loading), new PropertyMetadata(10, OnPropertyChanged));

        public static readonly DependencyProperty ResetAfterProperty =
            DependencyProperty.Register(nameof(ResetAfter), typeof(int), typeof(Loading), new PropertyMetadata(0));

        public static readonly DependencyProperty ShowOverlayProperty =
            DependencyProperty.Register(nameof(ShowOverlay), typeof(bool), typeof(Loading), new PropertyMetadata(false));

        public static readonly DependencyProperty StoryboardKeyProperty =
            DependencyProperty.Register(nameof(StoryboardKey), typeof(string), typeof(Loading), new PropertyMetadata(LoadingAnimation.Continuous));

        private DispatcherTimer _timer;
        private Storyboard _storyboard;
        private DateTime _stopTime;

        public Loading()
        {
            InitializeComponent();
        }

        public int HideDelay
        {
            get => (int)GetValue(HideDelayProperty);
            set => SetValue(HideDelayProperty, value);
        }

        public bool IsLoading
        {
            get => (bool)GetValue(IsLoadingProperty);
            set => SetValue(IsLoadingProperty, value);
        }

        public int Radius
        {
            get => (int)GetValue(RadiusProperty);
            set => SetValue(RadiusProperty, value);
        }

        public int ResetAfter
        {
            get => (int)GetValue(ResetAfterProperty);
            set => SetValue(ResetAfterProperty, value);
        }

        public Brush Stroke
        {
            get => (Brush)GetValue(StrokeProperty);
            set => SetValue(StrokeProperty, value);
        }

        public int StrokeThickness
        {
            get => (int)GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }

        public bool ShowOverlay
        {
            get => (bool)GetValue(ShowOverlayProperty);
            set => SetValue(ShowOverlayProperty, value);
        }

        public string StoryboardKey
        {
            get => (string)GetValue(StoryboardKeyProperty);
            set => SetValue(StoryboardKeyProperty, value);
        }

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Loading)?.Render();
        }

        private static void OnIsLoadingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue && d is Loading loading) loading.UpdateVisibility();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsLoading)
            {
                Show();
            }
            else
            {
                Hide();
            }

            Render();
        }

        private void Render()
        {
            var thickness = StrokeThickness / 2d;

            Path.Margin = new Thickness(thickness, thickness, -thickness, -thickness);
            Path.Height = Path.Width = Radius * 2;

            var adjRadius = Radius - thickness;
            var size = new Size(adjRadius, adjRadius);

            FirstStart.StartPoint = new Point(adjRadius * 2, adjRadius);
            FirstEnd.Point = new Point(adjRadius, adjRadius * 2);
            FirstEnd.Size = size;

            SecondStart.StartPoint = new Point(0, adjRadius);
            SecondEnd.Point = new Point(adjRadius, 0);
            SecondEnd.Size = size;

            LoadingRotateTransform.CenterX = adjRadius;
            LoadingRotateTransform.CenterY = adjRadius;
        }

        private void InitializeAnimation()
        {
            _timer = new(DispatcherPriority.Background);
            _timer.Interval = TimeSpan.FromMilliseconds(HideDelay);
            _timer.Tick += (_, _) => PauseAnimation();

            _storyboard = FindResource(StoryboardKey) as Storyboard;
            Path.BeginStoryboard(_storyboard);
        }

        private void PauseAnimation()
        {
            Hide();

            _stopTime = DateTime.Now;
            _timer.IsEnabled = false;
            _storyboard.Pause();
        }

        private void UpdateVisibility()
        {
            if (!IsLoaded) return;

            if (_storyboard is null) InitializeAnimation();

            if (IsLoading)
            {
                _timer.IsEnabled = false;

                // Reset animation if specified amount of time passed since it stopped
                if (DateTime.Now.Subtract(_stopTime).TotalMilliseconds > ResetAfter)
                    _storyboard.Begin();
                else
                    _storyboard.Resume();

                Show();
            }
            else
            {
                _timer.IsEnabled = true;
            }
        }

        private void Hide()
        {
            Overlay.Visibility = Visibility.Hidden;
            Path.Visibility = Visibility.Hidden;
        }

        private void Show()
        {
            if (_storyboard is null) InitializeAnimation();

            Path.Visibility = Visibility.Visible;
            Overlay.Visibility = ShowOverlay ? Visibility.Visible : Visibility.Hidden;
        }
    }
}
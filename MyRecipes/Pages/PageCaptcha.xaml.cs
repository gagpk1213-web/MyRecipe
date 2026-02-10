using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MyRecipes.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageCaptcha.xaml
    /// </summary>
    public partial class PageCaptcha : Page
    {
        private string _captchaText;

        public PageCaptcha(string captchaText)
        {
            InitializeComponent();
            _captchaText = captchaText;
            DrawCaptcha();
        }

        private void DrawCaptcha()
        {
            CaptchaCanvas.Children.Clear();

            var random = new System.Random();
            double x = 10;

            foreach (char c in _captchaText)
            {
                // Создаем TextBlock для каждого символа
                var textBlock = new TextBlock
                {
                    Text = c.ToString(),
                    FontSize = 24,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Color.FromRgb(
                        (byte)random.Next(100, 200),
                        (byte)random.Next(100, 200),
                        (byte)random.Next(100, 200))),
                    RenderTransform = new RotateTransform(random.Next(-20, 20))
                };

                // Добавляем шум (линии)
                for (int i = 0; i < 3; i++)
                {
                    var line = new System.Windows.Shapes.Line
                    {
                        X1 = random.Next(0, 200),
                        Y1 = random.Next(0, 60),
                        X2 = random.Next(0, 200),
                        Y2 = random.Next(0, 60),
                        Stroke = new SolidColorBrush(Color.FromRgb(
                            (byte)random.Next(150, 255),
                            (byte)random.Next(150, 255),
                            (byte)random.Next(150, 255))),
                        StrokeThickness = 1
                    };
                    CaptchaCanvas.Children.Add(line);
                }

                // Позиционируем символ
                Canvas.SetLeft(textBlock, x);
                Canvas.SetTop(textBlock, random.Next(5, 25));
                CaptchaCanvas.Children.Add(textBlock);

                x += 35;
            }
        }
    }
}

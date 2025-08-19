using System;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InventoryManager
{
    /// <summary>
    /// Lógica de interacción para LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            // Establecer el foco en el campo de usuario
            Loaded += (s, e) => UsernameTextBox.Focus();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            await PerformLogin();
        }

        private async void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await PerformLogin();
            }
        }

        private async Task PerformLogin()
        {
            // Validar campos vacíos
            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text))
            {
                ShowError("Por favor ingresa tu nombre de usuario");
                UsernameTextBox.Focus();
                return;
            }

            if (string.IsNullOrEmpty(PasswordBox.Password))
            {
                ShowError("Por favor ingresa tu contraseña");
                PasswordBox.Focus();
                return;
            }

            // Mostrar indicador de carga
            ShowLoading(true);
            HideError();

            try
            {
                // Simular autenticación (reemplazar con lógica real)
                await SimulateAuthentication(UsernameTextBox.Text, PasswordBox.Password);

                // Si la autenticación es exitosa, abrir ventana principal
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();

                // Cerrar ventana de login
                this.Close();
            }
            catch (UnauthorizedAccessException)
            {
                ShowError("Usuario o contraseña incorrectos");
                PasswordBox.Clear();
                PasswordBox.Focus();
            }
            catch (Exception ex)
            {
                ShowError($"Error de conexión: {ex.Message}");
            }
            finally
            {
                ShowLoading(false);
            }
        }

        private async Task SimulateAuthentication(string username, string password)
        {
            // Simular demora de autenticación
            await Task.Delay(2000);

            // Lógica de autenticación básica (reemplazar con tu implementación)
            if (username.ToLower() == "admin" && password == "admin123")
            {
                // Autenticación exitosa
                return;
            }
            else if (username.ToLower() == "usuario" && password == "user123")
            {
                // Autenticación exitosa
                return;
            }
            else
            {
                throw new UnauthorizedAccessException("Credenciales inválidas");
            }
        }

        private void ShowLoading(bool show)
        {
            if (show)
            {
                LoadingPanel.Visibility = Visibility.Visible;
                LoginButton.IsEnabled = false;

                // Iniciar animación de rotación
                var storyboard = (Storyboard)FindResource("LoadingAnimation");
                storyboard.Begin();
            }
            else
            {
                LoadingPanel.Visibility = Visibility.Collapsed;
                LoginButton.IsEnabled = true;

                // Detener animación
                var storyboard = (Storyboard)FindResource("LoadingAnimation");
                storyboard.Stop();
            }
        }

        private void ShowError(string message)
        {
            ErrorMessage.Text = message;
            ErrorPanel.Visibility = Visibility.Visible;

            // Auto-ocultar error después de 5 segundos
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += (s, e) =>
            {
                HideError();
                timer.Stop();
            };
            timer.Start();
        }

        private void HideError()
        {
            ErrorPanel.Visibility = Visibility.Collapsed;
        }

        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Para recuperar tu contraseña, contacta al administrador del sistema.\n\n" +
                "Email: admin@empresa.com\n" +
                "Teléfono: (81) 1234-5678",
                "Recuperar Contraseña",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            // Mostrar confirmación antes de cerrar
            var result = MessageBox.Show(
                "¿Estás seguro que deseas salir?",
                "Confirmar Salida",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        // Permitir arrastrar la ventana
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }
    }
}

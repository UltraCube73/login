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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        bool l_password_hidden = false;
        bool r_password_hidden = false;

        private void b_l_goto_register_Click(object sender, RoutedEventArgs e)
        {
            g_login.Visibility = Visibility.Hidden;
            g_register.Visibility = Visibility.Visible;
        }

        private void b_r_goto_login_Click(object sender, RoutedEventArgs e)
        {
            g_login.Visibility = Visibility.Visible;
            g_register.Visibility = Visibility.Hidden;
        }

        private void b_r_show_password_Click(object sender, RoutedEventArgs e)
        {
            r_password_hidden = !r_password_hidden;
            if (r_password_hidden)
            {
                pb_r_password.Visibility = Visibility.Hidden;
                tb_r_password.Visibility = Visibility.Visible;
                pb_r_password_repeat.Visibility = Visibility.Hidden;
                tb_r_password_repeat.Visibility = Visibility.Visible;
                i_r_open.Visibility = Visibility.Visible;
                i_r_closed.Visibility = Visibility.Hidden;
                tb_r_password.Text = pb_r_password.Password;
                tb_r_password_repeat.Text = pb_r_password_repeat.Password;
            }
            else
            {
                pb_r_password.Visibility = Visibility.Visible;
                tb_r_password.Visibility = Visibility.Hidden;
                pb_r_password_repeat.Visibility = Visibility.Visible;
                tb_r_password_repeat.Visibility = Visibility.Hidden;
                i_r_open.Visibility = Visibility.Hidden;
                i_r_closed.Visibility = Visibility.Visible;
                pb_r_password_repeat.Password = tb_r_password_repeat.Text;
            }
        }

        private void b_r_register_Click(object sender, RoutedEventArgs e)
        {
            string pattern = "[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}";
            Match isMatch = Regex.Match(tb_r_mail.Text, pattern, RegexOptions.IgnoreCase);
            if (isMatch.Success)
            {
                tb_r_mail.BorderBrush = Brushes.DarkGray;
                tb_r_error.Text = "";
                pattern = "^[a-zA-Z0-9_]*$";
                isMatch = Regex.Match(tb_r_login.Text, pattern, RegexOptions.IgnoreCase);
                if (isMatch.Success)
                {
                    tb_r_login.BorderBrush = Brushes.DarkGray;
                    tb_r_error.Text = "";
                    isMatch = Regex.Match(tb_r_password.Text, pattern, RegexOptions.IgnoreCase);
                    if (isMatch.Success)
                    {
                        if (!r_password_hidden)
                        {
                            tb_r_password.Text = pb_r_password.Password;
                            tb_r_password_repeat.Text = pb_r_password_repeat.Password;
                        }
                        tb_r_password.BorderBrush = Brushes.DarkGray;
                        tb_r_error.Text = "";
                        if (tb_r_password.Text == tb_r_password_repeat.Text)
                        {
                            tb_r_password_repeat.BorderBrush = Brushes.DarkGray;
                            tb_r_error.Text = "";


                            var context = new AppDbContext();
                            if(context.Users.FirstOrDefault(x => x.Login == tb_r_login.Text) is not null)
                            {
                                tb_r_login.BorderBrush = Brushes.Red;
                                tb_r_error.Text = "Логин занят!";
                            }
                            else if(context.Users.FirstOrDefault(x => x.Mail == tb_r_mail.Text) is not null)
                            {
                                tb_r_mail.BorderBrush = Brushes.Red;
                                tb_r_error.Text = "Почта уже зарегистрирована!";
                            }
                            else
                            {
                                var user = new User { Login = tb_r_login.Text, Password = tb_r_password.Text, Mail = tb_r_mail.Text };
                                context.Users.Add(user);
                                context.SaveChanges();
                                g_login.Visibility = Visibility.Visible;
                                g_register.Visibility = Visibility.Hidden;
                                tb_l_login.Text = user.Login;
                                tb_l_password.Text = user.Password;
                                pb_l_password.Password = user.Password;
                            }
                        }
                        else
                        {
                            tb_r_password_repeat.BorderBrush = Brushes.Red;
                            tb_r_error.Text = "Пароли не совпадают!";
                        }
                    }
                    else
                    {
                        tb_r_password.BorderBrush = Brushes.Red;
                        tb_r_error.Text = "Пароль не может содержать пробелы и спецсимволы!";
                    }
                }
                else
                {
                    tb_r_login.BorderBrush = Brushes.Red;
                    tb_r_error.Text = "Логин не может содержать пробелы и спецсимволы!";
                }
            }
            else
            {
                tb_r_mail.BorderBrush = Brushes.Red;
                tb_r_error.Text = "Некорректный почтовый адрес!";
            }
        }

        private void b_l_show_password_Click(object sender, RoutedEventArgs e)
        {
            l_password_hidden = !l_password_hidden;
            if (l_password_hidden)
            {
                pb_l_password.Visibility = Visibility.Hidden;
                tb_l_password.Visibility = Visibility.Visible;
                i_l_open.Visibility = Visibility.Visible;
                i_l_closed.Visibility = Visibility.Hidden;
                tb_l_password.Text = pb_l_password.Password;
            }
            else
            {
                pb_l_password.Visibility = Visibility.Visible;
                tb_l_password.Visibility = Visibility.Hidden;
                i_l_open.Visibility = Visibility.Hidden;
                i_l_closed.Visibility = Visibility.Visible;
                pb_l_password.Password = tb_l_password.Text;
            }
        }

        private void b_l_login_Click(object sender, RoutedEventArgs e)
        {
            tb_l_login.BorderBrush = Brushes.Black;
            tb_l_password.BorderBrush = Brushes.Black;
            tb_l_error.Text = "";
            var context = new AppDbContext();
            User user = context.Users.FirstOrDefault(x => x.Mail == tb_l_login.Text);
            if (user is null) user = context.Users.FirstOrDefault(x => x.Login == tb_l_login.Text);
            if (user is not null)
            {
                if (!l_password_hidden)
                {
                    tb_l_password.Text = pb_l_password.Password;
                }
                if (user.Password == tb_l_password.Text)
                {
                    g_login.Visibility = Visibility.Hidden;
                    g_main.Visibility = Visibility.Visible;
                    tb_m_username.Text = $"Здравствуйте, {user.Login}!";
                }
                else
                {
                    tb_l_password.BorderBrush = Brushes.Black;
                    tb_l_error.Text = "Неверный пароль!";
                }
            }
            else
            {
                tb_l_login.BorderBrush = Brushes.Red;
                tb_l_error.Text = "Логин или почта не найдена!";
            }
        }

        private void b_m_goto_login_Click(object sender, RoutedEventArgs e)
        {
            g_login.Visibility = Visibility.Visible;
            g_main.Visibility = Visibility.Hidden;
        }
    }
}

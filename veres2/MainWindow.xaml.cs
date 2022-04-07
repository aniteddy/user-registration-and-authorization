using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;


namespace veres2
{
    public class Autdata
    {
        public string L { get; set; }
        public string P { get; set; }
    }


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Autdata> data = new List<Autdata>();
        //неправильный логин и пароль при входе
        public bool ErrorLP = false;
        //существует ли такой же логин при регистрации
        public bool Loginexist = false;
        //кнопка забыл пароль при условии что логин существует
        public bool ForgotP = false;


        public MainWindow()
        {
            
            InitializeComponent();
            //чтение из json
            using (StreamReader r = new StreamReader(@"person.json"))
            {
                string json = r.ReadToEnd();
                data = System.Text.Json.JsonSerializer.Deserialize<List<Autdata>>(json);
            }
            ok.Visibility = Visibility.Hidden;
            Forgot_OK.Visibility = Visibility.Hidden;
        }


        /// <summary>
        /// вход в систему
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].L == login.Text.ToString() && data[i].P == password.Text.ToString())
                {
                    MessageBox.Show("Вы вошли в систему",
                        "Message",
                        MessageBoxButton.OK);
                    ErrorLP = false;
                    signup.Visibility = Visibility.Visible;
                    ok.Visibility = Visibility.Hidden;
                    break;
                }
                else
                {
                    ErrorLP = true;
                    //MessageBox.Show("Error login or password",
                    //"Error!",
                    //MessageBoxButton.OK);
                    //break;
                }
            }
            
            if (ErrorLP == true)
            {
                MessageBox.Show("Неверный логин или пароль",
                    "Error!",
                    MessageBoxButton.OK);
                ErrorLP = false;
            }
        }


        /// <summary>
        /// регистрация пользователей
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            signup.Visibility = Visibility.Hidden;
            ok.Visibility = Visibility.Visible;
            MessageBox.Show("Введите логин и пароль",
                "Message",
                        MessageBoxButton.OK);

        }
        /// <summary>
        /// кнопка забыл пароль
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void forgot_Click(object sender, RoutedEventArgs e)
        {
            Forgot_OK.Visibility = Visibility.Visible;
            forgot.Visibility = Visibility.Hidden;

            MessageBox.Show("Введите логин и новый пароль",
                "Message",
                        MessageBoxButton.OK);

        }

        private void WritingToFile()
        {
            //добавление нового зарегистрированного пользователя
            data.Add(new Autdata { L = login.Text.ToString(), P = password.Text.ToString() });

            //запись логина с паролем в json
            string jsonString = System.Text.Json.JsonSerializer.Serialize(data, new JsonSerializerOptions() { WriteIndented = true });
            using (StreamWriter outputFile = new StreamWriter("person.json"))
            {
                outputFile.WriteLine(jsonString);
            }
        }


        /// <summary>
        /// ok button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void signup_Copy_Click(object sender, RoutedEventArgs e)
        {
            //проверка всех элементов существует ли такой логин
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].L == login.Text.ToString())
                {
                    Loginexist = true;
                }
                else
                {
                    Loginexist = false;
                }
            }

            //если логин существует, то сообщение, если не существует, то запись в файл
            if (Loginexist == true)
            {
                MessageBox.Show("Такой логин уже существует",
                        "Attention!",
                        MessageBoxButton.OK);
                Loginexist = false;
            }
            else
            {
                Loginexist = false;
                MessageBox.Show("Вы успешно зарегистрировались",
                       "Message",
                       MessageBoxButton.OK);
                WritingToFile();
                signup.Visibility = Visibility.Visible;
                ok.Visibility = Visibility.Hidden;
            }
        }

        private void Forgot_OK_Click(object sender, RoutedEventArgs e)
        {

            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].L == login.Text.ToString())
                {
                    data[i].P = password.Text.ToString();

                    //запись логина с паролем в json
                    string jsonString = System.Text.Json.JsonSerializer.Serialize(data, new JsonSerializerOptions() { WriteIndented = true });
                    using (StreamWriter outputFile = new StreamWriter("person.json"))
                    {
                        outputFile.WriteLine(jsonString);
                    }

                    ForgotP = false;
                    Forgot_OK.Visibility = Visibility.Hidden;
                    forgot.Visibility = Visibility.Visible;
                    MessageBox.Show("Пароль изменён, пожалуйста авторизуйтесь",
                        "Message",
                        MessageBoxButton.OK);
                    break;
                }
                else
                {
                    ForgotP = true;
                }
            }

            if (ForgotP == true)
            {
                MessageBox.Show("Неправильный логин",
                        "Attention!",
                        MessageBoxButton.OK);
                ForgotP = false;
            }
        }
    }
}

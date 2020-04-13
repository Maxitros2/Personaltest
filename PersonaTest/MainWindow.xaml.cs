using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace PersonaTest
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Question> scrambled;
        public Config cfg;
        
        public MainWindow()
        {
            try
            {
               
                InitializeComponent();
                OtchetCanvas.Visibility = Visibility.Hidden;
                cfg = new Config();
                _loginPage.DownloadBaseClick.PreviewMouseDown += DownloadBaseClick_PreviewMouseDown;
                _loginPage.StartTest.Click += StartTest_Click;
                /*
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    ExcelHelper.OpenFile(openFileDialog.FileName);
                }
                foreach (Question question in Questions._alllist)
                {
                    string s = "Вопрос " + question.text + " Ответы ";
                    foreach (Answer answer in question.answers)
                    {

                        if (answer.right)
                            s = s + "(Правильный)";


                    }
                    */
            }catch
            (Exception e)
            { MessageBox.Show(e.ToString()); }
            }

        private void DownloadBaseClick_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            LoadTest();
        }

        int nowtest = 1;
        int rightanswes = 0;
        List<Answer> currentAnswers;
        List<FailedQuestion> otchet = new List<FailedQuestion>();
        List<Question> otchet2 = new List<Question>();
        public void pickQuestion()
        {
            if(nowtest-1== scrambled.Count())
            {
                finish();
                return;
            }
            QuestionCount1.Content = "Вопрос "+ nowtest + "/" + scrambled.Count();
            Question question = scrambled.ElementAt(nowtest - 1);
            if (question.text.Contains(".") && question.text.IndexOf('.') < 5)

                Question_textblock.Text = question.text.Remove(0, question.text.IndexOf('.') + 2);
            else
                Question_textblock.Text = question.text;
            currentAnswers = new List<Answer>();
            Random r = new Random();
            Answer[] toadd1 = new Answer[question.answers.Count]; ;
            question.answers.CopyTo(toadd1);
            List<Answer> toadd = toadd1.ToList();
          
            for (int i =0;i< question.answers.Count();i++)
            {
               
                int n = r.Next(toadd.Count());
                currentAnswers.Add(toadd.ElementAt(n));
                toadd.RemoveAt(n);
            }
            
            AnswerPanel.Children.Clear();
            foreach (Answer answer in currentAnswers)
            {
                UserControl1 user = new UserControl1 { Margin = new Thickness(10, 0, 10, 5) };
                user.Answertext.Text = (AnswerPanel.Children.Count+1)+". "+answer.text;
                AnswerPanel.Children.Add(user);
            }
            nowtest++;
        }

        private void finish()
        {
            OtchetCanvas.Visibility = Visibility.Visible;
            FIOLAST.Content = FIOLABEL.Content;
            OtchetCount.Content = "Вы ответили правильно на" + " "+rightanswes.ToString() + "/" + scrambled.Count;
        }

        public void ScrableAndClass()
        {
            Random r = new Random();
            double k = 1;
            switch (_loginPage.selectedclass) { 
                

            case "A": k = 1; break;
            case "B": k = 0.75d; break;
            case "C": k = 0.4d; break;
            }
            Question[] toadd1 = new Question[Questions._alllist.Count];
            Questions._alllist.CopyTo(toadd1);
            

            List<Question> toadd = toadd1.ToList(); 
            int count = Convert.ToInt32(Math.Ceiling(Questions._alllist.Count()*k));
            scrambled = new List<Question>();
            otchet = new List<FailedQuestion>();
            for (int i = 0;i<count;i++)
            {
                int n = r.Next(toadd.Count());
                scrambled.Add(toadd.ElementAt(n));
                toadd.RemoveAt(n);

            }


        }
        public void LoadTest()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                ExcelHelper.OpenFile(openFileDialog.FileName);
                cfg.Save();
            }
            
                
            
          
        }
        private void StartTest_Click(object sender, RoutedEventArgs e)
        {
           
                if (_loginPage.Name.Text == "")
                {
                    MessageBox.Show("Введите имя!");
                    return;
                }
                if (_loginPage.SerName.Text == "")
                {
                    MessageBox.Show("Введите фамилию!");
                    return;
               }
            
            if(Questions._alllist.Count()==0)
            {
                MessageBox.Show("Тест не загружен! Перед началом теста загрузите его!");
                LoadTest();
                return;
            }
            nowtest = 1;
            rightanswes = 0;
            ScrableAndClass();
            SetupTest();
            pickQuestion();
            LoginGrid.Visibility = Visibility.Hidden;
        }

        private void SetupTest()
        {
            FIOLABEL.Content = _loginPage.SerName.Text + " " + _loginPage.Name.Text + " " + _loginPage.ser2.Text;
          
        }

        private void StartTest_Click_1(object sender, RoutedEventArgs e)
        {
            int[] n, k = new int[currentAnswers.Count()];
            bool flag = true;
            for(int i =0;i< currentAnswers.Count();i++)
            {
                if (currentAnswers.ElementAt(i).right != AnswerPanel.Children.Cast<UserControl1>().ToArray()[i].isSelected)
                    flag = false;
            }
            if(flag)
            {
                rightanswes++;
                otchet2.Add(new Question() { text = Question_textblock.Text, answers = currentAnswers });
            }
            else
            {
                List<Answer> correct = currentAnswers.Where(x => x.right == true).ToList();
                List<Answer> failed = new List<Answer>();
                foreach(UserControl1 user in AnswerPanel.Children.Cast<UserControl1>().Where(x => x.isSelected == true))
                {
                    failed.Add(currentAnswers.ElementAt(AnswerPanel.Children.IndexOf(user)));
                }
                otchet.Add(new FailedQuestion() { text = Question_textblock.Text, CorrectAnswers = correct, FailedAnswers = failed });
            }
           
            pickQuestion();
        }

        private void StartTest_Copy_Click(object sender, RoutedEventArgs e)
        {
            string pathDocument = AppDomain.CurrentDomain.BaseDirectory + "Отчет " + FIOLABEL.Content + ".docx";
            
            // создаём документ
            DocX document = DocX.Create(pathDocument);


            Xceed.Document.NET.Paragraph paragraph = document.InsertParagraph();
            // выравниваем параграф по правой стороне
            paragraph.Alignment = Alignment.center;
            
            // добавляем отдельную строку со своим форматированием
            paragraph.AppendLine("Служба персонала").Bold().Font("Times New Roman").FontSize(14);
            paragraph.AppendLine("ООО \"Эрнис\"").Bold().Font("Times New Roman").FontSize(14);
            paragraph.AppendLine("ПРОТОКОЛ ТЕСТИРОВАНИЯ").Bold().Font("Times New Roman").FontSize(14);
            Xceed.Document.NET.Paragraph paragraph2 = document.InsertParagraph();
            paragraph2.Font("Times New Roman");
            paragraph2.FontSize(14);
            paragraph2.Alignment = Alignment.left;
            paragraph2.AppendLine("Фамилия: " + _loginPage.SerName.Text).Font("Times New Roman").FontSize(14);
            paragraph2.AppendLine("Имя: " + _loginPage.Name.Text).Font("Times New Roman").FontSize(14);
            paragraph2.AppendLine("Отчество: " + _loginPage.ser2.Text).Font("Times New Roman").FontSize(14);
            paragraph2.AppendLine();
            paragraph2.AppendLine("Всего вопросов: " + scrambled.Count()).Font("Times New Roman").FontSize(14);
            paragraph2.AppendLine("Правильных ответов: " + rightanswes+" ("+ (Math.Truncate((double)rightanswes*100 / (double)scrambled.Count()))+"%)").Font("Times New Roman").FontSize(14);
            Xceed.Document.NET.Paragraph paragraph3 = document.InsertParagraph().Font("Times New Roman").FontSize(14);
            paragraph3.Alignment = Alignment.right;
            paragraph3.Font("Times New Roman");
            paragraph3.FontSize(14);
            paragraph3.AppendLine();
            DateTime time = DateTime.Today;
            if (time.Month < 10)
                paragraph3.AppendLine("Дата: " + time.Day + ".0" + time.Month + "." + time.Year).Font("Times New Roman").FontSize(14);
            else
                paragraph3.AppendLine("Дата: " + time.Day + "." + time.Month + "." + time.Year).Font("Times New Roman").FontSize(14);
            paragraph3.AppendLine("ФИО контролирующего:___________________________________").Font("Times New Roman").FontSize(14);
            paragraph3.AppendLine("Подпись тестируемого:__________________________________").Font("Times New Roman").FontSize(14);
            paragraph3.InsertPageBreakAfterSelf();
            document.InsertSectionPageBreak();
            Xceed.Document.NET.Paragraph paragraph4 = document.InsertParagraph();
           
            paragraph4.Alignment = Alignment.left;

            paragraph4.AppendLine("Неверные ответы:").Font("Times New Roman").FontSize(14).Alignment=Alignment.center;
            paragraph4.AppendLine().Alignment = Alignment.left; ;
            foreach(FailedQuestion failed in otchet)
            {

                paragraph4.AppendLine(failed.text).Font("Times New Roman").FontSize(14);
                paragraph4.AppendLine("Ответ тестируемого:").Font("Times New Roman").FontSize(14);
                foreach(Answer answer in failed.FailedAnswers)
                    paragraph4.AppendLine("-"+answer.text).Font("Times New Roman").FontSize(14);
                paragraph4.AppendLine("Правильный ответ:").Font("Times New Roman").FontSize(14);
                foreach (Answer answer in failed.CorrectAnswers)
                    paragraph4.AppendLine("-"+answer.text).Font("Times New Roman").FontSize(14);
                paragraph4.AppendLine();

            }
           
            paragraph4.AppendLine("Верные ответы:").Font("Times New Roman").FontSize(14).Alignment = Alignment.center; ;
            paragraph4.AppendLine().Alignment = Alignment.left; ;
            foreach (Question failed in otchet2)
            {
                paragraph4.AppendLine(failed.text).Font("Times New Roman").FontSize(14);
                paragraph4.AppendLine("Ответ тестируемого:").Font("Times New Roman").FontSize(14);
                foreach (Answer answer in failed.answers.Where(x=>x.right==true))
                    paragraph4.AppendLine("-"+answer.text).Font("Times New Roman").FontSize(14);
                paragraph4.AppendLine();



            }









            // сохраняем документ
            document.Save();
            Process.Start(AppDomain.CurrentDomain.BaseDirectory + "Отчет " + FIOLABEL.Content + ".docx");
        }

        private void StartTest_Copy1_Click(object sender, RoutedEventArgs e)
        {
            OtchetCanvas.Visibility = Visibility.Hidden;
            LoginGrid.Visibility = Visibility.Visible;
        }
    }
}

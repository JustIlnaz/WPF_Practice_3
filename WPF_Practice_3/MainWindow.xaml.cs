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
using System.Windows.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window

    {

        private DispatcherTimer alarmTimer;
        private DispatcherTimer countdownTimer;
        private DateTime alarmTime;
        private TimeSpan timeLeft;

        public MainWindow()

        {
            InitializeComponent();
            alarmTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            alarmTimer.Tick += AlarmTimer_Tick;
            countdownTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            countdownTimer.Tick += CountdownTimer_Tick;
        }

        private void CbSetDate_Checked(object sender, RoutedEventArgs e)
        {
            datePickerAlarm.Visibility = Visibility.Visible;
        }
        private void CbSetDate_Unchecked(object sender, RoutedEventArgs e)
        {
            datePickerAlarm.Visibility = Visibility.Collapsed;
        }
        private void BtnSetAlarm_Click(object sender, RoutedEventArgs e) 
        {
            if (!int.TryParse(tbHours.Text, out int hours) || hours < 0 || hours > 23)
            {
                MessageBox.Show("Invalid hours!");
                return;
            }

            if (!int.TryParse(tbMinutes.Text, out int minutes) || minutes < 0 || minutes > 59)
            {
                MessageBox.Show("Invalid minutes!");
                return;
            }
            
            DateTime selectedDate = cbSetDate.IsChecked == true ? datePickerAlarm.SelectedDate ?? DateTime.Today : DateTime.Today;
            alarmTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, hours, minutes, 0);

            if (alarmTime < DateTime.Now)
            {
                if (cbSetDate.IsChecked == true)
                {
                    MessageBox.Show("Selected date/time is in the past!");
                    return;
                }

                alarmTime = alarmTime.AddDays(1);
            }

            alarmTimer.Start();
            tbTimeRemaining.Text = $"Alarm set for {alarmTime}";
        }
        private void AlarmTimer_Tick(object sender, EventArgs e)
        {
            var remaining = alarmTime - DateTime.Now;
            if (remaining <= TimeSpan.Zero)
            {
                alarmTimer.Stop();
                MessageBox.Show("ВСТАВАЙ(GET UPPPP)!!!!!!");
                tbTimeRemaining.Text = "Alarm triggered!";
            }
            else
                tbTimeRemaining.Text = $"Time remaining: {remaining:hh\\:mm\\:ss}";
        }


        private void BtnStartTimer_Click(object sender, RoutedEventArgs e)
        {

            if (!int.TryParse(tbTimerHours.Text, out int hours) || hours < 0 ||
                !int.TryParse(tbTimerMinutes.Text, out int minutes) || minutes < 0 || minutes > 59 ||
                !int.TryParse(tbTimerSeconds.Text, out int seconds) || seconds < 0 || seconds > 59)
            {
                MessageBox.Show("Invalid input!");
                return;
            }

            timeLeft = new TimeSpan(hours, minutes, seconds);

            if (timeLeft.TotalSeconds <= 0)
            {
                MessageBox.Show("Please enter a positive time.");
                return;
            }

            countdownTimer.Start();
            UpdateTimerDisplay();
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)

        {
            timeLeft = timeLeft.Subtract(TimeSpan.FromSeconds(1));
            if (timeLeft.TotalSeconds <= 0)
            {
                countdownTimer.Stop();
                tbTimerCountdown.Text = "Время вышло!";
                MessageBox.Show("Время вышло!(Time's up!)");
            }
            else
                UpdateTimerDisplay();
        }

        private void UpdateTimerDisplay() =>
            tbTimerCountdown.Text = $"{timeLeft:hh\\:mm\\:ss}";

    }
}

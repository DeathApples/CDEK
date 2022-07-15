using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using System.Windows;

using Logic.Controllers;
using Logic.Services;
using Logic.Models;

namespace Interface
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Organization CurrentOrganization;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OrganizationControl.Update();
            OrganizationDataGrid.ItemsSource = OrganizationControl.Organizations;
        }

        private void OrganizationDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (OrganizationDataGrid.SelectedIndex < 0)
            {
                return;
            }

            CurrentOrganization = (Organization)OrganizationDataGrid.SelectedItem;

            NameOrganizationTextBox.Text = CurrentOrganization.Name;
            CityTextBox.Text = CurrentOrganization.City;
            INNTextBox.Text = CurrentOrganization.INN;
            ContractNumberTextBox.Text = CurrentOrganization.ContractNumber;
            ContractDateTextBox.Text = CurrentOrganization.ContractDate;
            AddressTextBox.Text = CurrentOrganization.Address;
            WebsiteTextBox.Text = CurrentOrganization.Website;
            EmailOrganizationTextBox.Text = CurrentOrganization.Email;
            EDMTextBox.Text = CurrentOrganization.EDM;

            PostTextBox.Text = string.Empty;
            NamePersonTextBox.Text = string.Empty;
            EmailPersonTextBox.Text = string.Empty;
            NumberPhoneTextBox.Text = string.Empty;

            ComboReport.ItemsSource = null;
            ContactDataGrid.ItemsSource = null;
            ComboReport.ItemsSource = ReportControl.Search(CurrentOrganization.Id);
            ContactDataGrid.ItemsSource = ContactControl.Search(CurrentOrganization.Id);

            ContactAndReportTabControl.SelectedIndex = 0;
            ContactAndReportTabControl.IsEnabled = true;
            ComboReport.SelectedIndex = 0;

            CancelOrganizationButton.Content = "Назад";

            if ((ReportControl.Reports?.Count ?? 0) < 1)
            {
                ComboReport.IsEnabled = false;
                SaveReportButton.Content = "Добавить";
            }
            else
            {
                SaveReportButton.Content = "Сохранить";
            }

            CancelReportButton.Visibility = Visibility.Collapsed;

            MainTabControl.SelectedIndex = 1;
        }

        private void AddOrganizationButton_Click(object sender, RoutedEventArgs e)
        {
            CancelOrganizationButton.Content = "Отмена";

            NameOrganizationTextBox.Text = string.Empty;
            CityTextBox.Text = string.Empty;
            INNTextBox.Text = string.Empty;
            ContractNumberTextBox.Text = string.Empty;
            ContractDateTextBox.Text = string.Empty;
            AddressTextBox.Text = string.Empty;
            WebsiteTextBox.Text = string.Empty;
            EmailOrganizationTextBox.Text = string.Empty;
            EDMTextBox.Text = string.Empty;

            PostTextBox.Text = string.Empty;
            NamePersonTextBox.Text = string.Empty;
            EmailPersonTextBox.Text = string.Empty;
            NumberPhoneTextBox.Text = string.Empty;

            DateReport.Text = string.Empty;
            InfoBox.Document.Blocks.Clear();

            ComboReport.IsEnabled = false;
            SaveReportButton.Content = "Добавить";
            CancelReportButton.Visibility = Visibility.Collapsed;

            ContactDataGrid.ItemsSource = null;
            ComboReport.ItemsSource = null;

            ContactAndReportTabControl.SelectedIndex = 0;
            ContactAndReportTabControl.IsEnabled = false;

            MainTabControl.SelectedIndex = 1;
        }

        private void DeleteOrganizationButton_Click(object sender, RoutedEventArgs e)
        {
            if (OrganizationDataGrid.SelectedIndex >= 0)
            {
                if (MessageBoxResult.Yes == MessageBox.Show("Вы уверены, что хотите удалить информацию о данной организации?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Hand))
                {
                    ReportControl.RemoveAll((Organization)OrganizationDataGrid.SelectedItem);
                    ContactControl.RemoveAll((Organization)OrganizationDataGrid.SelectedItem);
                    OrganizationControl.Remove((Organization)OrganizationDataGrid.SelectedItem);

                    OrganizationDataGrid.ItemsSource = null;
                    OrganizationDataGrid.ItemsSource = OrganizationControl.Organizations;
                }
            }
            else
            {
                MessageBox.Show("Для удаления нужно выбрать организацию", "Оповещение", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            Interaction.OpenFile(((Organization)OrganizationDataGrid.SelectedItem).INN);
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            Export(true);
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                OrganizationDataGrid.ItemsSource = OrganizationControl.Search(SearchTextBox.Text.ToLower());
            }
        }

        private void SaveOrganizationButton_Click(object sender, RoutedEventArgs e)
        {
            if ((CancelOrganizationButton.Content.ToString() ?? "") == "Назад")
            {
                OrganizationControl.Remove(CurrentOrganization);
                OrganizationControl.Add(NameOrganizationTextBox.Text, CityTextBox.Text, INNTextBox.Text, EDMTextBox.Text, EmailOrganizationTextBox.Text, AddressTextBox.Text, WebsiteTextBox.Text, ContractNumberTextBox.Text, ContractDateTextBox.Text, CurrentOrganization.Id);
                CurrentOrganization = OrganizationControl.Organizations[OrganizationControl.Organizations.Count - 1];

                return;
            }

            if (!OrganizationControl.Add(NameOrganizationTextBox.Text, CityTextBox.Text, INNTextBox.Text, EDMTextBox.Text, EmailOrganizationTextBox.Text, AddressTextBox.Text, WebsiteTextBox.Text, ContractNumberTextBox.Text, ContractDateTextBox.Text))
            {
                MessageBox.Show("Информация о данной организации уже существует", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Hand);
                return;
            }

            CurrentOrganization = OrganizationControl.Organizations[OrganizationControl.Organizations.Count - 1];
            ContactAndReportTabControl.IsEnabled = true;
            CancelOrganizationButton.Content = "Назад";
            ReportControl.Reports.Clear();
        }

        private void CancelOrganizationButton_Click(object sender, RoutedEventArgs e)
        {
            OrganizationControl.Update();

            OrganizationDataGrid.ItemsSource = null;
            OrganizationDataGrid.ItemsSource = OrganizationControl.Organizations;

            MainTabControl.SelectedIndex = 0;
        }

        private void AddContactButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ContactControl.Add(CurrentOrganization.Id, NamePersonTextBox.Text, PostTextBox.Text, EmailPersonTextBox.Text, NumberPhoneTextBox.Text))
            {
                MessageBox.Show("Информация о данном сотруднике уже существует", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Hand);
                return;
            }

            PostTextBox.Text = string.Empty;
            NamePersonTextBox.Text = string.Empty;
            EmailPersonTextBox.Text = string.Empty;
            NumberPhoneTextBox.Text = string.Empty;

            ContactDataGrid.ItemsSource = null;
            ContactDataGrid.ItemsSource = ContactControl.Contacts;
        }

        private void DeleteContactButton_Click(object sender, RoutedEventArgs e)
        {
            if (ContactDataGrid.SelectedIndex >= 0)
            {
                if (MessageBoxResult.Yes == MessageBox.Show("Вы уверены, что хотите удалить информацию о данном сотруднике?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Hand))
                {
                    ContactControl.Remove((Contact)ContactDataGrid.SelectedItem);

                    ContactDataGrid.ItemsSource = null;
                    ContactDataGrid.ItemsSource = ContactControl.Contacts;
                }
            }
            else
            {
                MessageBox.Show("Для удаления нужно выбрать сотрудника", "Оповещение", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void AddReportImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DateReport.Text = string.Empty;
            InfoBox.Document.Blocks.Clear();

            ComboReport.SelectedIndex = -1;
            SaveReportButton.Content = "Добавить";

            ComboReport.IsEnabled = false;
            CancelReportButton.Visibility = Visibility.Visible;
        }

        private void ExportReportsImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Export(false);
        }

        private void ComboReport_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboReport.SelectedIndex >= 0)
            {
                DateReport.Text = string.Empty;
                InfoBox.Document.Blocks.Clear();

                DateReport.Text = ReportControl.Reports[ComboReport.SelectedIndex].DateReport;
                InfoBox.Document.Blocks.Add(new Paragraph(new Run(ReportControl.Reports[ComboReport.SelectedIndex].Information)));
            }
        }

        private void SaveReportButton_Click(object sender, RoutedEventArgs e)
        {
            if (ComboReport.IsEnabled)
            {
                ReportControl.Remove((Report)ComboReport.SelectedItem);
            }

            string information = new TextRange(InfoBox.Document.ContentStart, InfoBox.Document.ContentEnd).Text;

            if (!ReportControl.Add(CurrentOrganization.Id, information, DateReport.Text))
            {
                MessageBox.Show("Отчёт на данную дату уже существует", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Hand);
                return;
            }

            SaveReportButton.Content = "Сохранить";

            ComboReport.ItemsSource = null;
            ComboReport.ItemsSource = ReportControl.Reports;

            ComboReport.IsEnabled = true;
            ComboReport.SelectedIndex = 0;

            CancelReportButton.Visibility = Visibility.Collapsed;
        }

        private void CancelReportButton_Click(object sender, RoutedEventArgs e)
        {
            ComboReport.IsEnabled = true;
            ComboReport.SelectedIndex = 0;
            CancelReportButton.Visibility = Visibility.Collapsed;
        }

        private void Export(bool isOrganization = false)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.DefaultExt = "xlsx";

            if (saveFileDialog.ShowDialog() == true)
            {
                string path = saveFileDialog.FileName;

                if (!isOrganization)
                    Interaction.Export(path, CurrentOrganization);
                else
                    Interaction.Export(path);
            }
        }
    }
}

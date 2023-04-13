using Humongous.Healthcare.MAUI.Services;
using System.Collections.ObjectModel;

namespace Humongous.Healthcare.MAUI;

public partial class MainPage : ContentPage
{
	int count = 0;
	SubmissionService svc = new SubmissionService();
	ObservableCollection<string> symptoms = new ObservableCollection<string>();
	string healthStatus = "";
	public MainPage()
	{
		InitializeComponent();
		symptomList.ItemsSource = symptoms;
	}

	private void OnSymptomButtonClicked(object sender, EventArgs e)
	{
		symptoms.Add(symptomEntry.Text);
		symptomEntry.Text = "";
	}

	private async void OnCounterClicked(object sender, EventArgs e)
	{
		if (healthStatus == "")
		{
			await DisplayAlert("Health Status Required", "Please specify a health status", "OK");
		}

        var submission = new Models.Submission()
		{
			id = Guid.NewGuid().ToString(),
			PatientID = 77,
			Date = DateTime.Today,
			HealthStatus = healthStatus,
			Symptoms = symptoms.ToArray(),
		};
        bool ret = await svc.AddSubmission(submission);
		if (ret)
		{
			await DisplayAlert("Submission", "Submitted successfully", "OK");
			symptoms.Clear();
		} 
		else
		{
            await DisplayAlert("Submission", "Submission failed", "OK");
        }
    }

    private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
		healthStatus = (sender as RadioButton).Value.ToString();
    }

    private void OnClearButtonClicked(object sender, EventArgs e)
    {
		symptoms.Clear();
    }
}


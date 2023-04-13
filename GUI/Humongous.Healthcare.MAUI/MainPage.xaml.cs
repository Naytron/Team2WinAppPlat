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
		string[] symptoms = { "Congestion", "Cough", "Fever" };

        var submission = new Models.Submission()
		{
			PatientID = 1,
			Date = DateTime.Today,
			HealthStatus = "I feel unwell",
			Symptoms = symptoms,
		};
        bool ret = await svc.AddSubmission(submission);
		if (ret)
		{
			await DisplayAlert("Submission", "Submitted successfully", "OK");
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


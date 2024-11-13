using System;
using Google.Cloud.Firestore;
using MauiFirestore.Models;

namespace MauiFirestore.Services;

public class FirestoreService
{
    private FirestoreDb db;
    public string StatusMessage;

    public FirestoreService()
    {
        this.SetupFireStore();
    }
    private async Task SetupFireStore()
    {
        if (db == null)
        {
            var stream = await FileSystem.OpenAppPackageFileAsync("dx212-2024-a7b0e-firebase-adminsdk-7v235-4933ea8e9b.json");
            var reader = new StreamReader(stream);
            var contents = reader.ReadToEnd();
            db = new FirestoreDbBuilder
            {
                ProjectId = "dx212-2024-a7b0e",

                JsonCredentials = contents
            }.Build();
        }
    }

    public async Task<List<SampleModel>> GetAllSample()
    {
        try
        {
            await SetupFireStore();
            var data = await db.Collection("Samples").GetSnapshotAsync();
            var samples = data.Documents.Select(doc =>
            {
                var sample = new SampleModel();
                sample.Id = doc.Id;
                sample.Code = doc.GetValue<string>("Code");
                sample.Name = doc.GetValue<string>("Name");
                return sample;
            }).ToList();
            return samples;
        }
        catch (Exception ex)
        {

            StatusMessage = $"Error: {ex.Message}";
        }
        return null;
    }

    public async Task InsertSample(SampleModel sample)
    {
        try
        {
            await SetupFireStore();
            var sampleData = new Dictionary<string, object>
            {
                { "Code", sample.Code },
                { "Name", sample.Name }
                // Add more fields as needed
            };

            await db.Collection("Samples").AddAsync(sampleData);
        }
        catch (Exception ex)
        {

            StatusMessage = $"Error: {ex.Message}";
        }
    }

    public async Task UpdateSample(SampleModel sample)
    {
        try
        {
            await SetupFireStore();

            // Manually create a dictionary for the updated data
            var sampleData = new Dictionary<string, object>
            {
                { "Code", sample.Code },
                { "Name", sample.Name }
                // Add more fields as needed
            };

            // Reference the document by its Id and update it
            var docRef = db.Collection("Samples").Document(sample.Id);
            await docRef.SetAsync(sampleData, SetOptions.Overwrite);

            StatusMessage = "Sample successfully updated!";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error: {ex.Message}";
        }
    }

    public async Task DeleteSample(string id)
    {
        try
        {
            await SetupFireStore();

            // Reference the document by its Id and delete it
            var docRef = db.Collection("Samples").Document(id);
            await docRef.DeleteAsync();

            StatusMessage = "Sample successfully deleted!";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error: {ex.Message}";
        }
    }




}
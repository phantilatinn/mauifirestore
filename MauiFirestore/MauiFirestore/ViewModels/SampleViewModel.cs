using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using MauiFirestore.Models;
using MauiFirestore.Services;
using PropertyChanged;

namespace MauiFirestore.ViewModels;

[AddINotifyPropertyChangedInterface]
public partial class SampleViewModel
{
    FirestoreService _firestoreService;

    public ObservableCollection<SampleModel> Samples { get; set; } = [];
    public SampleModel CurrentSample { get; set; }

    public ICommand Reset { get; set; }
    public ICommand AddOrUpdateCommand { get; set; }
    public ICommand DeleteCommand { get; set; }

    public SampleViewModel(FirestoreService firestoreService)
    {
        this._firestoreService = firestoreService;
        this.Refresh();
        Reset = new Command( async () =>
        {
            CurrentSample = new SampleModel();
            await this.Refresh();
        }
        );
        AddOrUpdateCommand = new Command(async () =>
        {
            await this.Save();
            await this.Refresh();
        });
        DeleteCommand = new Command(async () =>
        {
            await this.Delete();
            await this.Refresh();
        });

    }

    public async Task GetAll()
    {
        Samples = [];
        var items = await _firestoreService.GetAllSample();
        foreach (var item in items)
        {
            Samples.Add(item);
        }
    }

    public async Task Save()
    {
       if(string.IsNullOrEmpty(CurrentSample.Id))
       {
            await _firestoreService.InsertSample(this.CurrentSample);
       }
       else{
            await _firestoreService.UpdateSample(this.CurrentSample);
       }
    }

    private async Task Refresh()
    {
        CurrentSample = new SampleModel();
        await this.GetAll();
    }

    private async Task Delete()
    {
        await _firestoreService.DeleteSample(this.CurrentSample.Id);
    }

}
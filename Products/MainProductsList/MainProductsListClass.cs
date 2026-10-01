using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Communication.Contacts;
using MauiApp1.Communication.SMS;
using MauiApp1.Communication.SMS.Android;
using MauiApp1.ViewerModels;
using System.Collections.ObjectModel;

namespace MauiApp1.Products.MainProductsList;

public partial class MainProductsListClass : ObservableObject
{


	public MainProductsListClass(MainViewModel _mainViewModel)
	{
		//TO_DO async constructor
		mainViewModel = _mainViewModel;

		receiveSMS = new ReceiveSMS(mainViewModel);

		Task.Run(() => ItemListUpdater.LoadListFromJson(this)).Wait();

		ReceiveListUpdate.AddThisList(Products);
	}

	ReceiveSMS receiveSMS;

	[ObservableProperty] public partial ObservableCollection<MainListProduct> Products { get; set; } = new();

	MainViewModel mainViewModel;

	public void SortProductsByStatus(MainListProduct product, bool toCart)
	{

		int lastOutOfCartProduct = Products.Count(e => e.IsInCart == false);

		int currentIndex = Products.IndexOf(product);

		if (!toCart)
		{
			Products.Move(currentIndex, 0);
			return;
		}

		Products.Move(currentIndex, lastOutOfCartProduct);

	}

	public async Task SaveList()
	{
		await ItemListUpdater.SaveListToJson(this);
	}
}

public partial class MainProductsListClass
{
	public async Task Add(string name)
	{

		if (!BaseProduct.GetTrimmedProductNameIfValidString(out string trimmedName, name)) return;

		float productCount = 1; //TO_DO parsuj czy nie ma dopisanej ilosci produktu

		Products.AddProductToList(trimmedName, productCount, this);
		await SaveList();
	}

	public async Task Update()
	{
		if (mainViewModel.ContactsLogicClass.SelectedContactPerson is null) return;
		SendSMS sendSMS = new SendSMS(this);
		await sendSMS.Send(mainViewModel.ContactsLogicClass.SelectedContactPerson.PersonSMSData.PhoneNumber, Products);
	}

	public async Task DeleteAll()
	{

		bool answer = await Shell.Current.DisplayAlert(
		"Confirmation",
		"Are you sure you want to delete all products?",
		"Yes",
		"No");

		if (!answer)
			return;

		Products.Clear();
		await SaveList();
	}




	[RelayCommand]
	private async Task Delete(MainListProduct product)
	{
		Products.Remove(product);
		await SaveList();
	}

	[RelayCommand]
	private async Task Increment(MainListProduct product)
	{
		product.Increment();
		await SaveList();
	}

	[RelayCommand]
	private async Task ChangeName(MainListProduct product)
	{
		await product.ChangeName(Products);
		await SaveList();
	}

	[RelayCommand]
	private async Task SetCustomCount(MainListProduct product)
	{

		string? result = await Shell.Current.DisplayPromptAsync(
		"Enter custom value",
		"Enter a number:",
		"OK",
		"Cancel",
		keyboard: Keyboard.Numeric);

		if (float.TryParse(result, out float value))
			product.Count = value;


		await SaveList();
	}

	[RelayCommand]
	private async Task Decrement(MainListProduct product)
	{
		product.Decrement();
		await SaveList();
	}
	[RelayCommand]
	private async Task ChangeStatus(MainListProduct product)
	{
		product.IsInCart = !product.IsInCart;

		SortProductsByStatus(product, product.IsInCart);

		await SaveList();
	}

}

using AppRpgEtec.ViewModels.Disputas;

namespace AppRpgEtec.Views.Disputas;

public partial class ListagemView : ContentPage
{
	public ListagemView()
	{
		DisputaViewModel viewModel;

		InitializeComponent();

		viewModel = new DisputaViewModel();
		BindingContext = viewModel;

	}
}
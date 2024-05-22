using AppRpgEtec.ViewModels.Armas;

namespace AppRpgEtec.Views.Armas;

public partial class CadastroArmaView : ContentPage
{
	private CadastroArmaViewModel cadviewModel;

	public CadastroArmaView()
	{
		InitializeComponent();

		cadviewModel = new CadastroArmaViewModel();
		BindingContext = cadviewModel;
		Title = "Nova Arma";

	}
}
using AppRpgEtec.ViewModels.Personagens;

namespace AppRpgEtec.Views.Personagens;

public partial class CadastroPesonagemView : ContentPage
{
    private CadastroPersonagemViewModel cadastroPersonagemViewModel;

    public CadastroPesonagemView()
    {
        cadastroPersonagemViewModel = new CadastroPersonagemViewModel();
        BindingContext = cadastroPersonagemViewModel;
        InitializeComponent();
        Title = "Novo personagem";
    }
}
using AppRpgEtec.Models;
using AppRpgEtec.Services.Personagens;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AppRpgEtec.ViewModels.Personagens
{
    class ListagemPersonagemViewModel : BaseViewModel
    {
        private PersonagemService _personagemService;
        public ObservableCollection<Personagem> Personagens { get; set; }
        public ListagemPersonagemViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            _personagemService = new PersonagemService(token);
            Personagens = new ObservableCollection<Personagem>();

            // descarte do retorno
            _ = ObterPesonagens();

            NovoPersonagem = new Command(async () => { await ExibirCadastroPersonagem(); });
            RemoverPersonagemCommand = new Command<Personagem>(async (Personagem p) => { await RemoverPersonagem(p); });
            ZerarRankingRestaurarVidasGeralCommand = new Command(async () => { await ZerarRankingRestaurarVidasGeral(); });
        }

        #region Commands
        public ICommand NovoPersonagem { get; set; }
        public ICommand RemoverPersonagemCommand { get; set; }
        public ICommand ZerarRankingRestaurarVidasGeralCommand { get; set; }
        #endregion

        #region Properties
        private Personagem personagemSelecionado;
        public Personagem PersonagemSelecionado
        {
            get { return personagemSelecionado; }
            set
            {
                if (value != null)
                {
                    personagemSelecionado = value;
                    // Shell.Current.GoToAsync($"cadPersonagemView?pId={personagemSelecionado.Id}");
                    _ = ExibirOpcoesAsync(personagemSelecionado);
                }
            }
        }
        #endregion

        #region Methods
        public async Task ObterPesonagens()
        {
            try
            {
                Personagens = await _personagemService.GetPersonagensAsync();
                OnPropertyChanged(nameof(Personagens));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                        .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

        public async Task ExibirCadastroPersonagem()
        {
            try
            {
                await Shell.Current.GoToAsync("cadPersonagemView");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                        .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

        public async Task RemoverPersonagem(Personagem p)
        {
            try
            {
                if (await Application.Current.MainPage.DisplayAlert("Confirmação", $"Confirma a remoção de {p.Nome}?", "Sim", "Não"))
                {
                    await _personagemService.DeletePersonagemAsync(p.Id);

                    await Application.Current.MainPage.DisplayAlert("Mensagem", "Personagem removido com sucesso!", "Ok");

                    _ = ObterPesonagens();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                        .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

        public async Task ExecutarRestaurarPontosPersonagem(Personagem personagem)
        {
            await _personagemService.PutRestaurarPontosAsync(personagem);
        }

        public async Task ExecutarZerarRankingPersonagem(Personagem personagem)
        {
            await _personagemService.PutZerarRankingAsync(personagem);
        }

        public async Task ExecutarZerarRankingRestaurarVidasGeral()
        {
            await _personagemService.PutZerarRankingRestaurarVidasGeralAsync();
        }

        public async void ProcessarOpcaoRespondidaAsync(Personagem personagem, string result)
        {
            if (result.Equals("Editar Personagem"))
            {
                await Shell.Current
                .GoToAsync($"cadPersonagemView?pId={personagem.Id}");
            }
            else if (result.Equals("Remover Personagem"))
            {
                if (await Application.Current.MainPage.DisplayAlert("Confirmação", $"Deseja realmente remover o personagem {personagem.Nome.ToUpper()}?", "Yes", "No"))
                {
                    await RemoverPersonagem(personagem);
                    await Application.Current.MainPage.DisplayAlert("Informação", "Personagem removido com sucesso!", "Ok");
                    await ObterPesonagens();
                }
            }
            else if (result.Equals("Restaurar Pontos de Vida"))
            {
                if (await Application.Current.MainPage.DisplayAlert("Confirmação", $"Restaurar os pontos de vida de {personagem.Nome.ToUpper()}?", "Yes", "No"))
                {
                    await ExecutarRestaurarPontosPersonagem(personagem); await Application.Current.MainPage.DisplayAlert("Informação", "Os pontos foram restaurados com sucesso.", "Ok"); await ObterPesonagens();
                }
            }
            else if (result.Equals("Zerar Ranking do Personagem"))
            {
                if (await Application.Current.MainPage.DisplayAlert("Confirmação", $"Zerar o ranking de {personagem.Nome.ToUpper()}?", "Yes", "No"))
                {
                    await ExecutarZerarRankingPersonagem(personagem); await Application.Current.MainPage.DisplayAlert("Informação", "O ranking foi zerado com sucesso.", "Ok");
                    await ObterPesonagens();
                }
            }
        }

        public async Task ExibirOpcoesAsync(Personagem personagem)
        {
            try
            {
                personagemSelecionado = null;
                string result = string.Empty;
                if (personagem.PontosVida > 0)
                {

                    result = await Application.Current.MainPage
                            .DisplayActionSheet("Opções para o personagem " + personagem.Nome,
                            "Cancelar",
                            "Editar Personagem",
                            "Restaurar Pontos de Vida",
                            "Zerar Ranking do Personagem",
                            "Remover Personagem");
                }
                else
                {
                    result = await Application.Current.MainPage
                      .DisplayActionSheet("Opções para o personagem " + personagem.Nome,
                      "Cancelar",
                      "Restaurar Pontos de Vida");
                }

                if (result != null)
                    ProcessarOpcaoRespondidaAsync(personagem, result);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                        .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

        public async Task ZerarRankingRestaurarVidasGeral()
        {
            try
            {
                if (await Application.Current.MainPage
                        .DisplayAlert("Comfirmação", $"Deseja realmente zerar todo o ranking?", "Yes", "No"))
                {
                    await ExecutarZerarRankingRestaurarVidasGeral();
                    await Application.Current.MainPage
                       .DisplayAlert("informações", "Rankibg zerado com sucesso.", "Ok");

                    await ObterPesonagens();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                        .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }
        #endregion
    }
}

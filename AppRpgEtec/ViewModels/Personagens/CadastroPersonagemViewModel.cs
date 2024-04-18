using AppRpgEtec.Models;
using AppRpgEtec.Models.Enuns;
using AppRpgEtec.Services.Personagens;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AppRpgEtec.ViewModels.Personagens
{
    public class CadastroPersonagemViewModel : BaseViewModel
    {
        private PersonagemService pService;

        public CadastroPersonagemViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            pService = new PersonagemService(token);
            _ = ObterClasses();

            SalvarCommand = new Command(async () => { await SalvarPersonagem(); });

            CancelarCommand = new Command(async => { CancelarCadastro(); });
        }

        #region Propriedades

        private int id;
        private string nome;
        private int pontosVida;
        private int forca;
        private int defesa;
        private int inteligencia;
        private int disputas;
        private int vitorias;
        private int derrotas;
        private ObservableCollection<TipoClasse> listaTiposClasse;
        private TipoClasse tipoClasseSelecionado;


        public int Id 
        { 
            get => id;
            set 
            { 
                id = value;
                OnPropertyChanged(nameof(Id));
            } 
        }
        public string Nome 
        { 
            get => nome; 
            set
            { 
                nome = value;
                OnPropertyChanged(nameof(Nome)); 
            }
        }
        public int PontosVida 
        { 
            get => pontosVida; 
            set
            { 
                pontosVida = value; 
                OnPropertyChanged(nameof(PontosVida)); 
            } 
        }
        public int Forca 
        { 
            get => forca; 
            set 
            { 
                forca = value; 
                OnPropertyChanged(nameof(Forca)); 
            } 
        }
        public int Defesa 
        { get => defesa; 
            set 
            { 
                defesa = value; 
                OnPropertyChanged(nameof(Defesa)); 
            } 
        }
        public int Inteligencia 
        { 
            get => inteligencia; 
            set 
            { 
                inteligencia = value; 
                OnPropertyChanged(nameof(Inteligencia)); 
            } 
        }
        public int Disputas 
        { 
            get => disputas; 
            set 
            { 
                disputas = value; 
                OnPropertyChanged(nameof(Disputas)); 
            } 
        }
        public int Vitorias 
        { 
            get => vitorias; 
            set 
            { 
                vitorias = value; 
                OnPropertyChanged(nameof(Vitorias)); 
            } 
        }
        public int Derrotas 
        { 
            get => derrotas; 
            set 
            { 
                derrotas = value; 
                OnPropertyChanged(nameof(Derrotas)); 
            } 
        }
        public ObservableCollection<TipoClasse> ListaTiposClasse
        {
            get { return listaTiposClasse; }
            set
            {
                if (value != null)
                {
                    listaTiposClasse = value;
                    OnPropertyChanged(nameof(ListaTiposClasse));
                }
            }
        }

        public TipoClasse TipoClasseSelecionado
        {
            get { return tipoClasseSelecionado; }
            set
            {
                if (value != null)
                {
                    tipoClasseSelecionado = value;
                    OnPropertyChanged(nameof(ListaTiposClasse));
                }
            }
        }

        public async Task ObterClasses()
        {
            try
            {
                listaTiposClasse = new ObservableCollection<TipoClasse>();
                ListaTiposClasse.Add(new TipoClasse() { Id = 1, Descricao = "Cavaleiro" });
                ListaTiposClasse.Add(new TipoClasse() { Id = 2, Descricao = "Mago" });
                ListaTiposClasse.Add(new TipoClasse() { Id = 3, Descricao = "Clerigo" });
                OnPropertyChanged(nameof(ListaTiposClasse));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                        .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

        public ICommand SalvarCommand { get; }

        public ICommand CancelarCommand { get; }

        public async Task SalvarPersonagem() 
        {
            try
            {
                Personagem model = new Personagem()
                {
                    Nome = nome,
                    PontosVida = pontosVida,
                    Defesa = defesa,
                    Derrotas = derrotas,
                    Disputas = disputas,
                    Forca = forca,
                    Inteligencia = inteligencia,
                    Vitorias = vitorias,
                    Id = id,
                    Classe = (ClasseEnum)tipoClasseSelecionado.Id
                };
                if (model.Id == 0)
                    await pService.PostPersonagemAsync(model);

                await Application.Current.MainPage.DisplayAlert("Mensagem", "Dados salvos com sucesso!", "OK");

                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "OK");
            }
        }

        private async void CancelarCadastro() 
        {
            await Shell.Current.GoToAsync("..");
        }

        #endregion

    }
}

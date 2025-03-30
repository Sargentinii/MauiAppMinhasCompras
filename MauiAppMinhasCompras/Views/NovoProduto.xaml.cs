using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class NovoProduto : ContentPage
{
    public NovoProduto()
    {
        InitializeComponent();
        PreencherCategorias();
    }

    // Método responsável por preencher o Picker com as categorias disponíveis
    private void PreencherCategorias()
    {
        // Obtém todas as opções do Enum "Categoria", transforma em lista e atribui ao Picker
        categoriaPicker.ItemsSource = Enum.GetValues(typeof(Categoria)) // Pega todas as categorias do Enum
                                          .Cast<Categoria>()  // Converte para uma lista de categorias
                                          .ToList(); // Transforma em uma lista para exibição
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Verifica se a categoria foi selecionada
            if (categoriaPicker.SelectedItem == null)
            {
                await DisplayAlert("Erro", "Por favor, selecione uma categoria.", "OK");
                return; // Impede a criação do produto se não tiver categoria selecionada
            }

            if (categoriaPicker.SelectedItem.ToString() == "Limpar")
            {
                await DisplayAlert("Erro", "Você não pode criar um produto com a categoria 'Limpar'.", "OK");
                return; // Impede a criação do produto se a categoria for "Limpar"
            }

            // Criação do produto
            Produto p = new Produto
            {
                Descricao = txt_descricao.Text,
                Quantidade = Convert.ToDouble(txt_qnt.Text),
                Preco = Convert.ToDouble(txt_preco.Text),
                Categoria = (Categoria)categoriaPicker.SelectedItem
            };

            // Inserir o produto no banco
            await App.Db.Insert(p);

            // Exibe o sucesso na operação
            await DisplayAlert("Sucesso", "Produto criado com sucesso!", "OK");

            // Navega para a tela anterior
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops!", ex.Message, "OK");
        }
    }
}
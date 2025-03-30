using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class NovoProduto : ContentPage
{
    public NovoProduto()
    {
        InitializeComponent();
        PreencherCategorias();
    }

    // M�todo respons�vel por preencher o Picker com as categorias dispon�veis
    private void PreencherCategorias()
    {
        // Obt�m todas as op��es do Enum "Categoria", transforma em lista e atribui ao Picker
        categoriaPicker.ItemsSource = Enum.GetValues(typeof(Categoria)) // Pega todas as categorias do Enum
                                          .Cast<Categoria>()  // Converte para uma lista de categorias
                                          .ToList(); // Transforma em uma lista para exibi��o
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Verifica se a categoria foi selecionada
            if (categoriaPicker.SelectedItem == null)
            {
                await DisplayAlert("Erro", "Por favor, selecione uma categoria.", "OK");
                return; // Impede a cria��o do produto se n�o tiver categoria selecionada
            }

            if (categoriaPicker.SelectedItem.ToString() == "Limpar")
            {
                await DisplayAlert("Erro", "Voc� n�o pode criar um produto com a categoria 'Limpar'.", "OK");
                return; // Impede a cria��o do produto se a categoria for "Limpar"
            }

            // Cria��o do produto
            Produto p = new Produto
            {
                Descricao = txt_descricao.Text,
                Quantidade = Convert.ToDouble(txt_qnt.Text),
                Preco = Convert.ToDouble(txt_preco.Text),
                Categoria = (Categoria)categoriaPicker.SelectedItem
            };

            // Inserir o produto no banco
            await App.Db.Insert(p);

            // Exibe o sucesso na opera��o
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
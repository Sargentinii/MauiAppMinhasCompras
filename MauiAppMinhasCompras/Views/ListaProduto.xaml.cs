using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

    public ListaProduto()
    {
        InitializeComponent();

        // Preencher o Picker com as categorias
        filtroCategoriaPicker.ItemsSource = Enum.GetValues(typeof(Categoria)).Cast<Categoria>().ToList();


        lst_produtos.ItemsSource = lista;
    }

    protected async override void OnAppearing()
    {
        try
        {
            lista.Clear();

            List<Produto> tmp = await App.Db.GetALL();

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops!", ex.Message, "OK");
        }
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Views.NovoProduto());
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops!", ex.Message, "OK");
        }
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            string q = e.NewTextValue;

            lst_produtos.IsRefreshing = true;

            lista.Clear();

            List<Produto> tmp = await App.Db.Search(q);

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops!", ex.Message, "OK");
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        try
        {
            // Agrupa os produtos por categoria e calcula o total gasto em cada categoria
            var totaisPorCategoria = lista.GroupBy(i => i.Categoria)// Agrupa os produtos por categoria
                                          .Select(grupo => new
                                          {
                                              Categoria = grupo.Key, // Pega o nome da categoria
                                              Total = grupo.Sum(i => i.Total) // Soma os valores dos produtos dessa categoria
                                          })
                                          .ToList();

            // Cria a mensagem do alerta que mostrará o total gasto por categoria
            string msg = "Total por Categoria:\n";

            // Percorre cada categoria e adiciona ao resumo
            foreach (var item in totaisPorCategoria)
            {
                msg += $"{item.Categoria}: {item.Total:C}\n"; // Adiciona o nome da categoria e o total formatado em moeda
            }

            // Calcula o total geral (soma de todas as categorias)
            double totalGeral = lista.Sum(i => i.Total);
            msg += $"\nTotal Geral: {totalGeral:C}"; // Adiciona o total geral ao resumo

            // Exibe um alerta mostrando o total gasto por categoria e o total geral
            DisplayAlert("Resumo de Compras", msg, "OK");
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops!", ex.Message, "OK");
        }
    }

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            MenuItem? selecionado = sender as MenuItem;

            Produto? p = selecionado.BindingContext as Produto;

            bool confirm = await DisplayAlert("Tem certeza?", $"Deseja excluir {p.Descricao}?", "Sim", "Não");

            if (confirm)
            {
                await App.Db.Delete(p.Id);
                lista.Remove(p);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops!", ex.Message, "OK");
        }
    }

    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            Produto? p = e.SelectedItem as Produto;

            Navigation.PushAsync(new Views.EditarProduto
            {
                BindingContext = p,
            });
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops!", ex.Message, "OK");
        }
    }

    private async void lst_produtos_Refreshing(object sender, EventArgs e)
    {
        try
        {
            lista.Clear();

            List<Produto> tmp = await App.Db.GetALL();

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops!", ex.Message, "OK");
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }

    private void FiltroCategoriaPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            // Verifica se "Sem Filtro" foi selecionado
            if (filtroCategoriaPicker.SelectedItem.ToString() == "Limpar")
            {
                // Mostra todos os produtos
                lst_produtos.ItemsSource = lista;
                return;
            }

            // Caso contrário, filtra os produtos pela categoria selecionada
            Categoria categoriaSelecionada = (Categoria)filtroCategoriaPicker.SelectedItem;
            var produtosFiltrados = lista.Where(p => p.Categoria == categoriaSelecionada).ToList();

            lst_produtos.ItemsSource = produtosFiltrados;
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops!", ex.Message, "OK");
        }
    }
}
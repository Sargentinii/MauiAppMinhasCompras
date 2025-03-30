using SQLite;

namespace MauiAppMinhasCompras.Models
{
    public class Produto
    {
        string _descricao;
        double _quantidade;
        double _preco;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        // Propriedade para armazenar a categoria do produto
        // O tipo "Categoria" é um enum (definido abaixo), o que significa que o usuário 
        // só poderá escolher entre as opções pré-definidas (Alimentos, Higiene, etc.)
        public Categoria Categoria { get; set; }
        public string Descricao
        {
            get => _descricao;
            set
            {
                if (value == null)
                {
                    throw new Exception("Descrição não pode ser vazia");
                }

                _descricao = value;
            }
        }
        public double Quantidade
        {
            get => _quantidade;
            set
            {
                if (value == 0)
                {
                    throw new Exception("A quantidade não pode ser 0");
                }

                _quantidade = value;
            }
        }
        public double Preco
        {
            get => _preco;
            set
            {
                if (value == 0)
                {
                    throw new Exception("O preço não pode ser 0");
                }

                _preco = value;
            }
        }
        public double Total { get => Quantidade * Preco; }

    }

    // Enum que define as categorias disponíveis para os produtos
    // Isso garante que o usuário escolha uma categoria válida, em vez de digitar livremente
    public enum Categoria
    {
        Alimentos,
        Higiene,
        Limpeza,
        Bebidas,
        Eletronicos,
        Outros,
        Limpar
    }
}

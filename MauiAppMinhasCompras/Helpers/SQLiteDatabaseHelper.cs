using MauiAppMinhasCompras.Models;
using SQLite;
using static SQLite.SQLite3;

namespace MauiAppMinhasCompras.Helpers
{
    public class SQLiteDatabaseHelper
    {
        readonly SQLiteAsyncConnection _conn;

        public SQLiteDatabaseHelper(string path)
        {
            _conn = new SQLiteAsyncConnection(path);
            _conn.CreateTableAsync<Produto>().Wait();
        }

        public Task<int> Insert(Produto p) 
        {
            return _conn.InsertAsync(p) ;
        }
        public Task<List<Produto>> Update(Produto p) 
        {
            string sql = "UPDATE Produto SET Descricao=?, Quantidade=?, Preco=? WHERE Id=?";
            
            return _conn.QueryAsync<Produto>(
                sql, p.Descricao, p.Quantidade, p.Preco, p.Id
                );
        }
        public async Task<int> Delete(int id)
        {
            int result = await _conn.Table<Produto>().DeleteAsync(i => i.Id == id);

            var count = await _conn.Table<Produto>().CountAsync();
            if (count == 0)
            {
                await _conn.ExecuteAsync("DELETE FROM sqlite_sequence WHERE name='Produto'");
            }

            return result;
        }
        public Task<List<Produto>> GetALL() 
        {
            return _conn.Table<Produto>().ToListAsync();
        }
        public Task<List<Produto>> Search(string q) 
        {
            string sql = "SELECT * FROM Produto WHERE Descricao LIKE '%"+q+"%'";

            return _conn.QueryAsync<Produto>(sql);
        }

    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using loja.data;
using loja.models;

namespace loja.services
{
    public class FornecedorService
    {
        private readonly LojaDbContext _dbContext;
        public FornecedorService(LojaDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        // Método para consultar todos os produtos
        public async Task<List<Fornecedor>> GetAllFornecedoresAsync()
        {
            return await _dbContext.Fornecedores.ToListAsync();
        }
        // Método para consultar um produto a partir do seu Id
        public async Task<Fornecedor> GetFornecedorByIdAsync(int id)
        {
            return await _dbContext.Fornecedores.FindAsync(id);
        }
        // Método para gravar um novo produto
        public async Task AddFornecedorAsync(Fornecedor fornecedor)
        {
            _dbContext.Fornecedores.Add(fornecedor);
            await _dbContext.SaveChangesAsync();
        }
        // Método para atualizar os dados de um produto
        public async Task UpdateFornecedorAsync(Fornecedor fornecedor)
        {
            _dbContext.Entry(fornecedor).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
        // Método para excluir um produto
        public async Task DeleteFornecedorAsync(int id)
        {
            var fornecedor = await _dbContext.Fornecedores.FindAsync(id);
            if (fornecedor != null)
            {
            _dbContext.Fornecedores.Remove(fornecedor);
            await _dbContext.SaveChangesAsync();
            }
        }
    }
}
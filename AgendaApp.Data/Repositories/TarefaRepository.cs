using AgendaApp.Data.Contexts;
using AgendaApp.Data.Entities;
using AgendaApp.Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaApp.Data.Repositories
{
    /// <summary>
    /// Classe para operações de repositório de dados de Tarefa.
    /// </summary>
    public class TarefaRepository
    {
        /// <summary>
        /// Método para inserir uma tarefa na tabela do banco de dados.
        /// </summary>
        /// <param name="tarefa">Objeto da classe de entidade</param>
        public void Save(Tarefa tarefa)
        {
            //abrindo conexão com o banco de dados
            using (var dataContext = new DataContext())
            {
                //gravar os dados da tarefa na tabela do banco
                dataContext.Add(tarefa);
                dataContext.SaveChanges();
            }
        }

        /// <summary>
        /// Método para atualizar uma tarefa na tabela do banco de dados.
        /// </summary>
        /// <param name="tarefa">Objeto da classe de entidade</param>
        public void Update(Tarefa tarefa)
        {
            //abrindo conexão com o banco de dados
            using (var dataContext = new DataContext())
            {
                //atualizar os dados da tarefa na tabela do banco
                dataContext.Update(tarefa);
                dataContext.SaveChanges();
            }
        }

        /// <summary>
        /// Método para excluir uma tarefa na tabela do banco de dados
        /// </summary>
        /// <param name="tarefa">Objeto da classe de entidade</param>
        public void Delete(Tarefa tarefa)
        {
            //abrindo conexão com o banco de dados
            using (var dataContext = new DataContext())
            {
                //excluindo os dados da tarefa na tabela do banco
                dataContext.Remove(tarefa);
                dataContext.SaveChanges();
            }
        }

        /// <summary>
        /// Método para consultar tarefas dentro de um período de datas informado
        /// </summary>
        /// <param name="dataMin">Data de início do período</param>
        /// <param name="dataMax">Data de término do período</param>
        /// <returns>Lista com todas as tarefas obtidas</returns>
        public List<Tarefa> GetByDatas(DateTime dataMin, DateTime dataMax)
        {
            //abrindo conexão com o banco de dados
            using (var dataContext = new DataContext())
            {
                return dataContext
                    .Set<Tarefa>() //Entidade que iremos consultar
                    .Include(t => t.Categoria) //Junção com a entidade Categoria (LEFT JOIN)
                    .Where(t => t.Data >= dataMin && t.Data <= dataMax) //filtro de pesquisa
                    .OrderBy(t => t.Data) //critério de ordenação
                    .ToList(); //Retornar uma lista com todos os resultados
            }
        }

        /// <summary>
        /// Método para consultar 1 tarefa no banco de dados através do ID
        /// </summary>
        /// <param name="id">Identificador da tarefa (chave primária)</param>
        /// <returns>Objeto contendo os dados da tarefa</returns>
        public Tarefa? GetById(Guid id)
        {
            //abrindo conexão com o banco de dados
            using (var dataContext = new DataContext())
            {
                return dataContext
                    .Set<Tarefa>() //Entidade que iremos consultar
                    .Include(t => t.Categoria) //Junção com a entidade Categoria (LEFT JOIN)
                    .Where(t => t.Id == id) //filtrando a tarefa pelo id
                    .FirstOrDefault(); //retornar o primeiro registro obtido ou vazio (null)
            }
        }

        public List<TarefaCategoriaModel> GroupByCategoria(DateTime dataMin, DateTime dataMax)
        {
            //abrindo conexao
            using (var dataContext = new DataContext())
            {
                return dataContext
                    .Set<Tarefa>()
                    .Include(t => t.Categoria)
                    .Where(t => t.Data >= dataMin && t.Data <= dataMax)
                    .GroupBy(g => g.Categoria.Descricao)
                    .Select(group => new TarefaCategoriaModel
                    {
                        Categoria = group.Key,
                        QtdTarefas = group.Count()
                    })
                    .ToList();
            }
        }

        public List<TarefaPrioridadeModel> GroupByPrioridade(DateTime dataMin, DateTime dataMax)
        {

            using (var dataContext = new DataContext())
            {
                return dataContext
                    .Set<Tarefa>()
                    .Where(t => t.Data >= dataMin && t.Data < dataMax)
                    .GroupBy(g => g.Prioridade)
                    .Select(group => new TarefaPrioridadeModel
                    {
                        Prioridade = group.Key.ToString(),
                        QtdTarefas = group.Count()
                    })
                    .ToList();
            }
        }

    }
}




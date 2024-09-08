using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_MySQL
{
    internal class CRUD_class
    {
        private MySqlConnection _connection;
        public CRUD_class(string server, string db, string user, string ps) {
            string connectionString = $"server={server};database={db};user={user};password={ps};";
            Connection = new MySqlConnection(connectionString);

            try
            {
                Connection.Open();
                Console.WriteLine("Conexão com banco de dados foi efetuada com sucesso!\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Connection.Close();
            }
        }
        public void Create(string tableName, List<string> columns, List<object> values)
        {
            if(columns.Count != values.Count)
            {
                throw new ArgumentException("O número de colunas e valores não coincide.");
            }
            // Construir a query dinamicamente
            string query = $"INSERT INTO {tableName} ({string.Join(", ", columns)}) VALUES ({string.Join(", ", columns.Select(c => "@" + c))})";
            try
            {
                Connection.Open();
                MySqlCommand cmd = new MySqlCommand(query, Connection);

                for(int i = 0; i < columns.Count; i++)
                {
                    cmd.Parameters.AddWithValue("@"+columns[i], values[i]);
                }
                cmd.ExecuteNonQuery();
                Console.WriteLine("Registro inserido com sucesso.");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally { Connection.Close(); }
        }

        public List<Dictionary<string, object>> Select(string query)
        {
            List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();

            
            try
            {
                Connection.Open();
                MySqlCommand cmd = new MySqlCommand(query, Connection);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    // Criar um dicionário para armazenar os valores de cada linha
                    Dictionary<string, object> row = new Dictionary<string, object>();

                    // Loop através das colunas retornadas
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string columnName = reader.GetName(i); // Nome da coluna
                        object value = reader.GetValue(i); // Valor da coluna
                        row[columnName] = value; // Adiciona a coluna e seu valor ao dicionário
                    }

                    // Adicionar a linha à lista de resultados
                    results.Add(row);
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally { Connection.Close(); }
            return results;
        }

        public void Update(string tableName, List<string> columns, List<object> values, string conditionColumn, object conditionValue)
        {
            if (columns.Count != values.Count)
            {
                throw new ArgumentException("O numero de colunas não corresponde á quantidade de valores!");
            }
            // Construir a parte SET da query dinamicamente
            string setClause = string.Join(", ", columns.Select(c => $"{c} = @{c}"));
            string query = $"UPDATE {tableName} SET {setClause} WHERE {conditionColumn} = @{conditionColumn}";

            try
            {
                Connection.Open();
                MySqlCommand cmd = new MySqlCommand(query, Connection);

                // Adicionar os parâmetros das colunas a serem atualizadas
                for (int i = 0; i < columns.Count; i++)
                {
                    cmd.Parameters.AddWithValue("@" + columns[i], values[i]);
                }

                // Adicionar o parâmetro da condição (ex.: ID)
                cmd.Parameters.AddWithValue("@" + conditionColumn, conditionValue);

                // Executar o comando de atualização
                cmd.ExecuteNonQuery();
                Console.WriteLine("Registro atualizado com sucesso.");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally { Connection.Close(); }
        }

        public void Delete(string tabela, string tabelaCondicao, object valorCondi)
        {
            string query = $"DELETE FROM {tabela} WHERE {tabelaCondicao} = @{tabelaCondicao};";
            try
            {
                Connection.Open();
                MySqlCommand cmd = new MySqlCommand(query, Connection);
                cmd.Parameters.AddWithValue("@"+tabelaCondicao, valorCondi);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Registro deletado com sucesso!");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }finally { Connection.Close(); }
        }

        //get e set da variável encapsulada de conexão ao banco de dados
        public MySqlConnection Connection { get => _connection; set => _connection = value; }

        
    }
}

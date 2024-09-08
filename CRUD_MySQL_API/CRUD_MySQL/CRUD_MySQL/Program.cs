namespace CRUD_MySQL
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("testando aqui uau wow legal\n");
            CRUD_class cursor = new CRUD_class("localhost", "geral", "root", "admin");

            string tabela = "usuario";
            List<string> columns = new List<string> { "nome", "email", "senha" };
            List<object> values = new List<object> { "Dante", "enzo@gmail.com", "12345" };

            cursor.Create(tabela, columns, values);
            //===============================================================================
            
            //read
            string query = "SELECT * FROM usuario";
            List<Dictionary<string, object>> result = cursor.Select(query);

            // Exibindo os resultados
            foreach (var row in result)
            {
                foreach (var column in row)
                {
                    Console.WriteLine($"{column.Key}: {column.Value}");
                }
                Console.WriteLine(); // Nova linha para separar os registros
            }
            //===============================================================================
            //update
            // Colunas e novos valores
            columns = new List<string> { "nome", "email" };
            values = new List<object> { "Carlos silva", "carlos.silva@email.com" };
            string tableName = "usuario";
            // Condição (WHERE id = 1)
            string conditionColumn = "id";
            object conditionValue = 1;

            // Executar a atualização
            cursor.Update(tableName, columns, values, conditionColumn, conditionValue);
            //===================================================================================
            //delete
            tabela = "usuario";
            conditionColumn = "id";
            conditionValue = 1;
            cursor.Delete(tabela, conditionColumn, conditionValue);
        }
    }
}
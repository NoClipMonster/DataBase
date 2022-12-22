using System.Collections.Generic;
using System.Data;
using System.Windows;

using Microsoft.Data.SqlClient;

namespace DBHandler {
    public class BaseHandler {
        SqlConnection connection;
        public bool isConnected { get { return connection.State == ConnectionState.Open; } }

        public delegate void StateChangedHandler();
        public event StateChangedHandler Connected;
        public event StateChangedHandler Disconected;

        public BaseHandler() {
            connection = new SqlConnection();
            connection.StateChange += Connection_StateChange;
        }

        private void Connection_StateChange(object sender, StateChangeEventArgs e) {
            if ( e.CurrentState == ConnectionState.Open && Connected != null )
                Connected();
            else if ( e.CurrentState == ConnectionState.Closed && Disconected != null )
                Disconected();
        }


        public async void Connect() {
            string connectionString = "Server=(LocalDB)\\MSSQLLocalDB;DataBase=TestBase; Integrated Security=True;Connect Timeout=30";
            connection.ConnectionString = connectionString;
            try {
                await connection.OpenAsync();
            }
            catch ( SqlException ex ) {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }
        public async void Disconnect() {
            try {
                await connection.CloseAsync();
            }
            catch ( SqlException ex ) {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }
        public List<string> GetFields() {
            List<string> fields = new List<string>();
            string sqlExpression = "SELECT DISTINCT (SELECT Name FROM Products WHERE Products.Id = Pairs.ProductId), (SELECT Name FROM Categories WHERE Categories.Id = Pairs.CategoryId) FROM Pairs";
            //"(SELECT Name FROM Categories WHERE Categories.Id = Pairs.CategoryId) AS C" +
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            SqlDataReader reader = command.ExecuteReader();

            if ( reader.HasRows ) // если есть данные
            {
                while ( reader.Read() ) // построчно считываем данные
                {
                    fields.Add(reader.GetValue(0).ToString() + reader.GetValue(1).ToString());
                }
            }
            return fields;

        }
    }
}

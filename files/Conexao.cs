using System;
using System.Net;
using System.Data.SQLite;

namespace Main{
	public class Conexao{
		public static SQLiteConnection 	sql_con;
		public static SQLiteCommand 	sql_cmd;
		public static SQLiteDataAdapter DB;

		public static string Base64Encode(string plainText) {
  			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
  			return System.Convert.ToBase64String(plainTextBytes);
		}

		public static void SetConnection(){ 
			sql_con = new SQLiteConnection(@"Data Source=res\database.db;Version=3;New=False;Compress=True;");
		}

		public static void ExecuteQuery(string txtQuery){
			try{
				SetConnection();
				sql_con.Open();

				sql_cmd = sql_con.CreateCommand();
				sql_cmd.CommandText = txtQuery; 
				sql_cmd.ExecuteNonQuery(); 

				sql_con.Close();
			}

			catch(FormatException e)			{Console.WriteLine(e);}
			catch(NotSupportedException e)		{Console.WriteLine(e);}
			catch(InvalidOperationException e)	{Console.WriteLine(e);}
			catch(SQLiteException e)			{Console.WriteLine(e);}
			catch(InvalidCastException e)		{Console.WriteLine(e);}
			catch(ArgumentException e)			{Console.WriteLine(e);}
			catch(Exception e)					{Console.WriteLine(e);}
		}

		public static SQLiteDataReader LoadData(string txtQuery){
			SetConnection();
			sql_con.Open(); 
			sql_cmd = sql_con.CreateCommand();
			sql_cmd.CommandText = txtQuery;

			return sql_cmd.ExecuteReader();
		}

		public static bool Login(string email, string senha){
			string Reply = new WebClient().DownloadString(new Uri("http://nicode.esy.es/044267a365f264ebcd2ba31b278500df55a48f96.php?email="+Base64Encode(email)+"&pswd="+Base64Encode(senha)+"&jogo="+Base64Encode("Cybership")));

			if(Reply == "Email ou senha incorretos!")
				return false;
			else{
				Data.SetUser(email, Reply);
				return true;
			}
		}
	}
}
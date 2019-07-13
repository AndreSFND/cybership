using System;
using System.Linq;
using System.Collections.Generic;
using SFML;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System.Data.SQLite;
using System.Text.RegularExpressions;

namespace Main{
	public class Data{
		private static string user_email;
		private static string user_nick; 

		public static void SetUser(string user_email, string user_nick){
			Data.user_email 	= user_email;
			Data.user_nick 		= user_nick;
		}

		public static string Nick(){
			return user_nick;
		}
	}
}
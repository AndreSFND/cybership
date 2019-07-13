using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using SFML;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace Main{
	public partial class Eventos{
		
		public static void NPC_5_OnAction(Geral x){
			Interacoes.Falar(x, "Boa sorte em sua jornada!");
		}

		public static void NPC_6_OnAction(Geral x){
			if(Interacoes.Falar(x, "Use este item para ganhar a batalha"))
				Interacoes.DarItem(x, 3);
		}

		public static void NPC_7_OnAction(Geral x){
			Interacoes.Falar(x, "Eu sou max, é um prazer te conhecer :D");
		}
	}
}
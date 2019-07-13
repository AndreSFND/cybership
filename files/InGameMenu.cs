using System;
using System.Linq;
using System.Collections.Generic;
using SFML;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace Main{
	public partial class F{
		public static Playable x 			= Configuracoes.controle;
		public static int[,] menuArray 		= new int[3,3];
		public static int mY 				= -1;
		public static int mX, mA, rN, sY, hY= 0;
		public static byte cX, cY, cZ, cA 	= 0;


		public static void Menu(){
			x = Configuracoes.controle;

			for(int m=0;m<3;m++){
				for(int n=0;n<3;n++)
					menuArray[m,n] = -1;
			}
			
			if(F.Key("esc") && !x.menu && !x.pausado && !F.TeclaDesativada("esc")){
				x.menu = true;
				x.menuType = 0;
				F.DesativarTecla("esc", 300);
				Efeitos.Backdrop("fade-in", 125, 25);
			}
			else if(F.Key("esc") && x.menu && !F.TeclaDesativada("esc")){
				x.menu = false;
				F.DesativarTecla("esc", 300);
				F.BloquearMov(x, false);
				Efeitos.Backdrop("fade-out", 0, 25);
			}
			
			if(F.Key("enter") && !x.menu && !x.pausado && !F.TeclaDesativada("enter")){
				x.menu = true;
				x.menuType = 1;
				F.DesativarTecla("enter", 300);
				Efeitos.Backdrop("fade-in", 125, 25);
			}
			else if(F.Key("enter") && x.menu && !F.TeclaDesativada("enter")){
				x.menu = false;
				F.DesativarTecla("enter", 300);
				F.BloquearMov(x, false);
				Efeitos.Backdrop("fade-out", 0, 25);
			}

			if(x.menu && x.menuType == 0){
				V.window.SetView(V.hud);
				
				F.BloquearMov(x, true);
				
				string[] e 				= new string[3];
				e[0] = "Continuar";
				e[1] = "Mapa";
				e[2] = "Menu Principal";
				
				if(F.Key("up") && sY > 0 && !F.TeclaDesativada("up") && !F.MouseIn(((Screen.width - 240) / 2), ((Screen.height - 400) / 2), 240, 240)){
					sY--;
					F.DesativarTecla("up", 125);
				}
				if(F.Key("down") && sY < 2 && !F.TeclaDesativada("down") && !F.MouseIn(((Screen.width - 240) / 2), ((Screen.height - 400) / 2), 240, 240)){
					sY++;
					F.DesativarTecla("down", 125);
				}

				for(int m=0;m<3;m++){
					cA = (m == mA && mY == -1) ? (byte)35 : (byte)19;
					
					if(sY == m){
						cX = 35;
						cY = 255;
						if(F.Key("x")){
							switch(m){
								case 0:
									x.menu = false;
									F.BloquearMov(x, false);
									F.DesativarTecla("esc", 300);
									F.DesativarTecla("x", 300);
									Efeitos.Backdrop("fade-out", 0, 25);
								break;

								case 1:
									CurrentScreen.Change("worldmap");
								break;
								
								case 2:
									Idk.DeletarMapa();
									CurrentScreen.Change("mainmenu");

									x.menu = false;
									F.BloquearMov(x, false);
									F.DesativarTecla("esc", 300);
									F.DesativarTecla("x", 300);
									Efeitos.Backdrop("fade-out", 0, 25);
								break;
							}
						}
					}
					else{
						cX = 19;
						cY = 130;
					}
					
					F.DesenharShape(((Screen.width - 240) / 2), ((Screen.height - 400) / 2)+80*m, 240, 80, cX, cX, cX, 255);
					F.Escrever(e[m], true, ((Screen.width - 240) / 2)+120-(F.TxtWidth(e[m], 32, true)/2), ((Screen.height - 400) / 2)+16+(80*m), 32, cY, cY, cY, 230);
				}
			}
			
			if(x.menu && x.menuType == 1){
				F.BloquearMov(x, true);
				F.DesenharShape(((Screen.width - 720) / 2), ((Screen.height - 400) / 2), 720, 400, 210, 210, 210, 230);

				string[] e 				= new string[3];
				e[0] = "Itens";
				e[1] = "Recursos";
				e[2] = "Habilidades";

				for(int m=0;m<3;m++){
					cA = (m == mA && mY == -1) ? (byte)35 : (byte)19;
					F.DesenharShape(((Screen.width - 720) / 2)+(240*m), ((Screen.height - 400) / 2), 240, 80, cA, cA, cA, 255);

					cA = (m == mA) ? (byte)255 : (byte)130;
					F.Escrever(e[m], true, ((Screen.width - 720) / 2)+120-(F.TxtWidth(e[m], 32, true)/2)+(240*m), ((Screen.height - 400) / 2)+16, 32, cA, cA, cA, 230);
				}

				if(mY == -1)
					mX = 0;

				if(mA < 2){
					for(int m=0;m<3;m++)
						for(int n=0;n<3;n++){
							cA = (m == mY && n == mX) ? (byte)35 : (byte)19;
							F.DesenharShape(((Screen.width - 720) / 2)+20+(100*n), ((Screen.height - 400) / 2)+100+(100*m), 80, 80, cA, cA, cA, 255);

							if(mA == 0 && x.itens.Count > (m*3)+n){
								menuArray[m,n] 			= V.itens.IndexOf(x.itens[(m*3)+n]);
								V.img[1].Texture 		= V.IMG_CAT[5][x.itens[(m*3)+n].sprite];
								V.img[1].Scale 			= new Vector2f((float)80/V.IMG_CAT[5][x.itens[(m*3)+n].sprite].Size.X, (float)80/V.IMG_CAT[5][x.itens[(m*3)+n].sprite].Size.Y);
								V.img[1].Position 		= new Vector2f(((Screen.width - 720) / 2)+20+(100*n), ((Screen.height - 400) / 2)+100+(100*m));
								V.img[1].TextureRect	= new IntRect(0, 0, (int)V.IMG_CAT[5][x.itens[(m*3)+n].sprite].Size.X, (int)V.IMG_CAT[5][x.itens[(m*3)+n].sprite].Size.Y);
								V.window.Draw(V.img[1]);

								F.Escrever(""+x.itens[(m*3)+n].quantidade, true, ((Screen.width - 720) / 2)+96+(100*n)-F.TxtWidth(""+x.itens[(m*3)+n].quantidade, 20, true), ((Screen.height - 400) / 2)+156+(100*m), 18, 255, 255, 255, 255, 000, 000, 000, 255, 2);
							}
							if(mA == 1 && x.recursos.Count > (m*3)+n){
								menuArray[m,n] 			= (m*3)+n;
								V.img[1].Texture 		= V.IMG_CAT[6][x.recursos[menuArray[m,n]].sprite];
								V.img[1].Scale 			= new Vector2f((float)80/V.IMG_CAT[6][x.recursos[menuArray[m,n]].sprite].Size.X, (float)80/V.IMG_CAT[6][x.recursos[menuArray[m,n]].sprite].Size.Y);
								V.img[1].Position 		= new Vector2f(((Screen.width - 720) / 2)+20+(100*n), ((Screen.height - 400) / 2)+100+(100*m));
								V.img[1].TextureRect	= new IntRect(0, 0, (int)V.IMG_CAT[6][x.recursos[menuArray[m,n]].sprite].Size.X, (int)V.IMG_CAT[6][x.recursos[menuArray[m,n]].sprite].Size.Y);
								V.window.Draw(V.img[1]);

								F.Escrever(""+x.recursos[menuArray[m,n]].quantidade, true, ((Screen.width - 720) / 2)+96+(100*n)-F.TxtWidth(""+x.recursos[menuArray[m,n]].quantidade, 20, true), ((Screen.height - 400) / 2)+156+(100*m), 18, 255, 255, 255, 255, 000, 000, 000, 255, 2);
							}

							rN++;
						}

					F.DesenharShape(((Screen.width - 720) / 2)+320, ((Screen.height - 400) / 2)+100, 380, 280, 000, 000, 000, 230);

					if(mA == 0 && mY > -1 && menuArray[mY,mX] != -1){
						V.img[1].Texture 		= V.IMG_CAT[5][x.itens[(mY*3)+mX].sprite];
						V.img[1].Scale 			= new Vector2f((float)80/V.IMG_CAT[5][x.itens[(mY*3)+mX].sprite].Size.X, (float)80/V.IMG_CAT[5][x.itens[(mY*3)+mX].sprite].Size.Y);
						V.img[1].Position 		= new Vector2f(((Screen.width - 720) / 2)+320, ((Screen.height - 400) / 2)+100);
						V.img[1].TextureRect	= new IntRect(0, 0, (int)V.IMG_CAT[5][x.itens[(mY*3)+mX].sprite].Size.X, (int)V.IMG_CAT[5][x.itens[(mY*3)+mX].sprite].Size.Y);
						V.window.Draw(V.img[1]);

						F.Escrever(V.itens[menuArray[mY,mX]].nome, true, ((Screen.width - 720) / 2)+420, ((Screen.height - 400) / 2)+95, 42, 255, 255, 255, 230);
						F.Escrever(V.itens[menuArray[mY,mX]].descricao, true, ((Screen.width - 720) / 2)+335, ((Screen.height - 400) / 2)+185, 28, 255, 255, 255, 230);
						
						if(F.Key("x") && !F.TeclaDesativada("x") && !F.TeclaDesativada("enter")){
							x.item = V.itens[menuArray[mY,mX]];
						
							x.menu = false;
							F.DesativarTecla("enter", 300);
							F.DesativarTecla("x", 300);
							F.BloquearMov(x, false);
							Efeitos.Backdrop("fade-out", 0, 25);
						}
					}

					else if(mA == 1 && mY > -1 && menuArray[mY,mX] != -1){
						V.img[1].Texture 		= V.IMG_CAT[6][x.recursos[menuArray[mY,mX]].sprite];
						V.img[1].Scale 			= new Vector2f((float)80/V.IMG_CAT[6][x.recursos[menuArray[mY,mX]].sprite].Size.X, (float)80/V.IMG_CAT[6][x.recursos[menuArray[mY,mX]].sprite].Size.Y);
						V.img[1].Position 		= new Vector2f(((Screen.width - 720) / 2)+320, ((Screen.height - 400) / 2)+100);
						V.img[1].TextureRect	= new IntRect(0, 0, (int)V.IMG_CAT[6][x.recursos[menuArray[mY,mX]].sprite].Size.X, (int)V.IMG_CAT[6][x.recursos[menuArray[mY,mX]].sprite].Size.Y);
						V.window.Draw(V.img[1]);

						F.Escrever(x.recursos[menuArray[mY,mX]].nome, true, ((Screen.width - 720) / 2)+420, ((Screen.height - 400) / 2)+95, 42, 255, 255, 255, 230);
						F.Escrever(x.recursos[menuArray[mY,mX]].descricao, true, ((Screen.width - 720) / 2)+335, ((Screen.height - 400) / 2)+185, 28, 255, 255, 255, 230);
						F.Escrever("Quantidade: "+x.recursos[menuArray[mY,mX]].quantidade, true, ((Screen.width - 720) / 2)+685-F.TxtWidth("Quantidade: "+x.recursos[menuArray[mY,mX]].quantidade, 28, true), ((Screen.height - 400) / 2)+335, 28, 255, 255, 255, 230);
					}
					
					if(F.Key("c") && !F.TeclaDesativada("c") && !F.TeclaDesativada("enter")){
						x.item = null;
					
						x.menu = false;
						F.DesativarTecla("enter", 300);
						F.DesativarTecla("c", 300);
						F.BloquearMov(x, false);
						Efeitos.Backdrop("fade-out", 0, 25);
					}

					if(F.Key("right") && !F.TeclaDesativada("menuMove")){
						if(mY == -1 && mA < 2)
							mA++;
						else if(mY > -1  && mX < 2)
							mX++;

						F.DesativarTecla("menuMove", 175);
					}
					else if(F.Key("left") && !F.TeclaDesativada("menuMove")){
						if(mY == -1 && mA > 0)
							mA--;
						else if(mY > -1  && mX > 0)
							mX--;

						F.DesativarTecla("menuMove", 175);
					}
					else if(F.Key("up") && !F.TeclaDesativada("menuMove") && mY > -1){
						mY--;
						F.DesativarTecla("menuMove", 175);
					}
					else if(F.Key("down") && !F.TeclaDesativada("menuMove") && mY < 2){
						mY++;
						F.DesativarTecla("menuMove", 175);
					}
				}
				else{
					int m;
					if(mX > 1) mX = 1;
					if(mX == 0) hY = 0;

					for(m=0;m<3;m++){
						cA = (mX == 0 && m == mY) ? (byte)35 : (byte)19;
						F.DesenharShape(((Screen.width - 720) / 2)+20, ((Screen.height - 400) / 2)+100+(100*m), 80, 80, cA, cA, cA, 255);

						byte c = (mY == m) ? (byte)255 : (byte)135;
						F.Escrever(""+(m+1), true, ((Screen.width - 720) / 2)+60-(F.TxtWidth(""+(m+1), 34, true)/2), ((Screen.height - 400) / 2)+113.5f+(100*m), 34, c, c, c, 255);
					}

					F.DesenharShape(((Screen.width - 720) / 2)+220, ((Screen.height - 400) / 2)+100, 480, 280, 000, 000, 000, 230);

					if(mY > -1){
						F.Escrever(Configuracoes.controle.skills[mY].Nome(), true, ((Screen.width - 720) / 2)+265, ((Screen.height - 400) / 2)+105, 34, 255, 255, 255, 255);

						for(m=3;m<Configuracoes.controle.skills.Count();m++){
							byte c = (Configuracoes.controle.skills[m] == Configuracoes.controle.skills[mY]) ? (byte)255 : (byte)135;
							F.Escrever(Configuracoes.controle.skills[m].Nome(), true, ((Screen.width - 720) / 2)+265, ((Screen.height - 400) / 2)+105+(45*(m-2)), 34, c, c, c, 255);
						}

						if(mX == 1){
							V.img[0].Texture 		= V.IMG_CAT[8][0];
							V.img[0].TextureRect	= new IntRect(0, 0, 27, 15);
							V.img[0].Position 		= new Vector2f(((Screen.width - 720) / 2)+230, ((Screen.height - 400) / 2)+120+(45*hY));
							V.window.Draw(V.img[0]);
						}
					}

					if(F.Key("x") && !F.TeclaDesativada("x")){
						if(mX == 1 && hY > 0){
							Skill tmp = Configuracoes.controle.skills[mY];
							Configuracoes.controle.skills[mY] = Configuracoes.controle.skills[hY+2];
							Configuracoes.controle.skills[hY+2] = tmp;
						}

						mX = mX == 0 ? 1 : 0;
						F.DesativarTecla("x", 300);
					}

					if(F.Key("c") && !F.TeclaDesativada("c") && mX == 1){
						mX = 0;
						F.DesativarTecla("x", 300);
					}


					if(F.Key("right") && !F.TeclaDesativada("menuMove") && mY == -1 && mA < 2){
						mA++;
						F.DesativarTecla("menuMove", 175);
					}
					else if(F.Key("left") && !F.TeclaDesativada("menuMove") && mY == -1 && mA > 0){
						mA--;
						F.DesativarTecla("menuMove", 175);
					}
					else if(F.Key("up") && !F.TeclaDesativada("menuMove") && mY > -1){
						if(mX == 0) mY--;
						if(mX == 1 && hY > 0) hY--;
						F.DesativarTecla("menuMove", 175);
					}
					else if(F.Key("down") && !F.TeclaDesativada("menuMove")){
						if(mX == 0 && mY < 2) mY++;
						if(mX == 1 && hY < Configuracoes.controle.skills.Count()-3) hY++;
						F.DesativarTecla("menuMove", 175);
					}
				}
			}

			F.DesenharShape(0, Screen.height - 25, Screen.width, 25, 239, 239, 239, 230);
			
			//XP
			F.DesenharShape(((Screen.width - (Screen.width / 3)) / 2), Screen.height - 20, Screen.width / 3, 15, 000, 000, 000, 230);
			F.DesenharShape(((Screen.width - (Screen.width / 3)) / 2), Screen.height - 20, ((x.xpA  - V.nivelArray[V.personagem.nivel]) / (V.nivelArray[V.personagem.nivel+1] - V.nivelArray[V.personagem.nivel])) * (Screen.width / 3), 15, 000, 255, 000, 230);
			F.Escrever("XP", true, ((Screen.width - (Screen.width / 3))/2)-30, Screen.height-30, 24, 000, 000, 000, 230);
			
			//VIDA
			F.DesenharShape(Screen.width - 30, 80, 20, 125, 239, 239, 239, 230);
			F.DesenharShape(Screen.width - 30, 205, 20, (float)-(x.vidaA*1.25), 000, 255, 000, 230);

			//ATALHO MENU
			F.DesenharShape(Screen.width-70, 10, 60, 60, 239, 239, 239, 230);
			
			Item i = Configuracoes.controle.item;
			
			if(i != null){
				V.img[1].Texture 		= V.IMG_CAT[5][i.sprite];
				V.img[1].Scale 			= new Vector2f((float)60/V.IMG_CAT[5][i.sprite].Size.X, (float)60/V.IMG_CAT[5][i.sprite].Size.Y);
				V.img[1].TextureRect	= new IntRect(0, 0, (int)V.IMG_CAT[5][i.sprite].Size.X, (int)V.IMG_CAT[5][i.sprite].Size.Y);
				V.img[1].Position 		= new Vector2f(Screen.width-70, 10);
				V.window.Draw(V.img[1]);
			}
		}
	}
}
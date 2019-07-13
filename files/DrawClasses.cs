using System;
using System.Linq;
using System.Collections.Generic;
using SFML;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace Main{
	public class Efeitos{
		public static string 	efeito 		 = "";
		public static float 	opacidade 	 = 0;
		public static float 	opacidade2 	 = 0;
		public static float 	s 			 = 0;
		public static float 	q 			 = 0;
		public static bool		decisao		 = false;

		public static float		cX 			 = 0;
		public static string	word 		 = "";
		public static bool		respawn 	 = false;
		public static bool		allowRespawn = false;

		public static bool 		efeitoTela	 = false;
		public static float		efeitoTelaN  = 0;

		public static void LimiteMapa(){
			if(V.cameraY < 0){
				V.view.Move(new Vector2f(0, -V.cameraY)); 
				V.cameraY = 0;
			}
			if(V.cameraY+Screen.height > V.mapHeight){
				V.view.Move(new Vector2f(0, V.mapHeight-(V.cameraY+Screen.height))); 
				V.cameraY = V.mapHeight-Screen.height;
			}
			if(V.cameraX+Screen.width > V.mapWidth){
				V.view.Move(new Vector2f(V.mapWidth-(V.cameraX+Screen.width),0)); 
				V.cameraX = V.mapWidth-Screen.width;
			}
			if(V.cameraX < 0){
				V.view.Move(new Vector2f(-V.cameraX ,0)); 
				V.cameraX = 0;
			}
		}
		public static void AjustarCamera(Geral x){
			if(x.camera){
				V.view.Center = new Vector2f(x.x+(x.imgWidth/2), x.y);
				V.cameraX = V.view.Center.X - ((Screen.width/V.Delta)/2);
				V.cameraY = V.view.Center.Y - ((Screen.height/V.Delta)/2);
			}
			if(efeitoTela){
				V.view.Move(new Vector2f(2.5f, 0));
				efeitoTelaN -= 2.5f;

				if(efeitoTelaN <= 0){
					efeitoTela 	= false;
					x.camera 	= true;
				}
			}
			if(!Configuracoes.controle.interior)
				Efeitos.LimiteMapa();
		}
		public static void DesenharXp(){
			if(Math.Ceiling(V.personagem.xpA) == V.personagem.xp)		V.personagem.xpA = V.personagem.xp;
			if(V.personagem.xpA >= V.nivelArray[V.personagem.nivel+1]) 	V.personagem.nivel++;
			if(V.personagem.xpA < V.personagem.xp)						V.personagem.xpA += (V.personagem.xp - V.personagem.xpA) / 10;
		}
		public static void Camera(Geral x, bool y){
			x.camera = y;
		}
		public static bool Render(float x, float y, float width, float height){
			float sW = Screen.width/V.Delta;
			float sH = Screen.height/V.Delta;

			float cW	= sW + (sW/5);
			float cH	= sH + (sH/5);
			
			float catX = (x + (width / 2)) - (V.cameraX-(sW/10) + (cW / 2));
			float catY = (y + (height / 2)) - (V.cameraY-(sH/10) + (cH / 2));
									
			float sumHalfWidth = (width / 2) + (cW / 2);
			float sumHalfHeight = (height / 2) + (cH / 2);
						
			return (Math.Abs(catX) < sumHalfWidth && Math.Abs(catY) < sumHalfHeight) ? true : false;
		}
		public static void AtualizarQtdItens(){
			
		}
		public static void AtualizarVida(Playable x){
			if(x.vida < 0)x.vida = 0;
			if(x.vida > x.vidaLimite)x.vida = x.vidaLimite;

			if(Math.Floor(x.vidaA) == x.vida)x.vidaA = x.vida;
			
			if((x.vida - x.vidaA)/10 < 0)
				x.vidaA += (x.vida - x.vidaA)/10;
			else
				x.vidaA += (x.vida - x.vidaA)/10;
		}
		
		public static void AtualizarEnergia(Playable x){
			if(x.energia < 0)x.energia = 0;
			if(x.energia > 100)x.energia = 100;
			if(Math.Floor(x.energiaA) == x.energia)x.energiaA = x.energia;
			
			if((x.energia - x.energiaA)/10 < 0)
				x.energiaA += (x.energia - x.energiaA)/10;
			else
				x.energiaA += (x.energia - x.energiaA)/10;
		}
		
		public static void Backdrop(string e_, float q_, float s_){
			efeito 	= e_;
			q 		= q_;
			s 		= s_;
		}
		public static void BackdropRun(){
			if(opacidade > 255) opacidade = 255;
			if(opacidade < 0)	opacidade = 0;

			if(efeito == "sem-efeito")
				F.DesenharShape(0, 0, Screen.width, Screen.height, 000, 000, 000, q);
			else if(efeito == "fade-in"){
				if(opacidade < q) opacidade += s;
				else opacidade = q;
				
				F.DesenharShape(0, 0, Screen.width, Screen.height, 000, 000, 000, opacidade);
			}
			else if(efeito == "fade-out"){			
				if(opacidade > q) opacidade -= s;
				else opacidade = q;
				
				F.DesenharShape(0, 0, Screen.width, Screen.height, 000, 000, 000, opacidade);
			}
			else if(efeito == "fade-in-white"){
				if(opacidade < q) opacidade += s;
				else opacidade = q;
				
				F.DesenharShape(0, 0, Screen.width, Screen.height, 255, 255, 255, opacidade);
			}
			else if(efeito == "fade-out-white"){
				if(opacidade > q) opacidade -= s;
				else opacidade = q;
				
				F.DesenharShape(0, 0, Screen.width, Screen.height, 255, 255, 255, opacidade);
			}
		}
		public static bool GameOver(Playable x){
			if(respawn){
				if(allowRespawn && opacidade == 125)
					Backdrop("fade-in", 255, 3.25f);

				else if(F.TeclaDesativada("delayRespawn"))
					F.DesenharShape(0, 0, Screen.width, Screen.height, 000, 000, 000, 255);

				else if(!allowRespawn && !F.TeclaDesativada("delayRespawn") && opacidade >= 255)
					Backdrop("fade-out", 0, 12.75f);
				

				if(opacidade >= 255 && allowRespawn){
					allowRespawn = false;

					x.x			= V.random.Next(1, V.mapWidth-x.imgWidth);
					x.y			= V.random.Next(1, V.mapHeight-x.imgHeight);
					LocalMapState.Menu(false);
					CurrentScreen.Change("localmap");
					
					F.DesativarTecla("delayRespawn", 1000);
				}
				else if(opacidade <= 0){
					x.vida 			= 50;
					opacidade 		= 0;
					opacidade2 		= 0;
					respawn 		= false;
					F.BloquearMov(x, false);
					LocalMapState.Menu(true);
				}

				return false;
			}

			else{
				if(!x.gameOver){
					opacidade 		= 0;
					opacidade2		= 0;
					decisao			= false;
					x.vida 			= 0;
					x.vidaA 		= 0;
					allowRespawn 	= true;

					Backdrop("fade-in", 125f, 3.125f);
					F.BloquearMov(x, true);
				}
				
				if(opacidade >= 125){
					if(!F.TeclaDesativada("left") && (F.Key("left") || F.Key("right"))){
						decisao = !decisao;
						F.DesativarTecla("left", 250);
					}
					else if(F.Key("x")){
						if(decisao){
							Idk.DeletarMapa();
							CurrentScreen.Change("mainmenu");
							F.DesativarTecla("x", 300);
						}
						else respawn = true;
					}
					
					if(opacidade2 < 255)
						opacidade2 += 3.1875f;

					if(!decisao){
						F.DesenharShape((Screen.width / 2) - (F.TxtWidth("Main Menu", 26, true))+4, (Screen.height/2)+3, F.TxtWidth("Respawn", 26, true)+12, 30, 255, 255, 255, (byte)opacidade2);
						F.Escrever("Respawn", true, (Screen.width/2)-F.TxtWidth("Respawn", 26, true)-20, Screen.height/2, 26, 000, 000, 000, (byte)opacidade2);
						F.Escrever("Main Menu", true, (Screen.width/2)+20, Screen.height/2, 26, 255, 255, 255, (byte)opacidade2);
					}
					else{
						F.DesenharShape((Screen.width / 2) + 14, (Screen.height/2)+3, F.TxtWidth("Main Menu", 26, true)+12, 30, 255, 255, 255, (byte)opacidade2);
						F.Escrever("Respawn", true, (Screen.width/2)-F.TxtWidth("Respawn", 26, true)-20, Screen.height/2, 26, 255, 255, 255, (byte)opacidade2);
						F.Escrever("Main Menu", true, (Screen.width/2)+20, Screen.height/2, 26, 000, 000, 000, (byte)opacidade2);
					}
					
					F.Escrever("Game Over", true, (Screen.width / 2) - (F.TxtWidth("Game Over", 36, true) / 2), (Screen.height / 2) - 100, 36, 255, 255, 255, (byte)(opacidade*2));
				}

				return true;
			}
		}

		public static void TremerTela(Geral x){
			x.camera 	= false;
			efeitoTela 	= true;

			V.view.Move(new Vector2f(-10, 0));
			efeitoTelaN = 10;
		}
	} 


	public class Interacoes{
		public static int letraAtual 			= 0;
		public static bool decisao 				= false;
		public static int[,] rightSide 			= new int[3,3];
		public static int[,] leftSide 			= new int[3,3];
		public static int[,] bottom 			= new int[2,4];
		public static int[] top 				= new int[4];
		public static bool[] lights 			= new bool[4];
		public static int mX, mY, mA 			= 0;
		public static byte cA 					= 0;
		public static int puzzleSide			= 0;
		public static int puzzleVar 			= 0;

		public static void Puzzle1(Geral x){
			if(Configuracoes.controle.pausado){
				if(F.Key("enter") && !F.TeclaDesativada("enter")){
					F.BloquearMov(x, false);
					F.BloquearMov(Configuracoes.controle, false);
					InteracaoContinua(x, Configuracoes.controle, false);
					F.DesativarTecla("enter", 500);
					F.DesativarTecla("x", 500);
				}

				if(leftSide[0, 0] == 4 && leftSide[0, 1] == 6 && leftSide[0, 2] == 0 && leftSide[1, 0] == 3 && leftSide[1, 2] == 5 && leftSide[2, 0] == 1 && leftSide[2, 1] == 7 && leftSide[2, 2] == 2){
					InteracaoContinua(x, Configuracoes.controle, false);
					F.DesativarTecla("enter", 500);
					F.DesativarTecla("x", 500);

					V.lockedObjects[23] = false;
					F.Alerta("Um baú foi aberto!");
				}

				F.DesenharShape(((Screen.width - 780) / 2), ((Screen.height - 440) / 2), 780, 440, 210, 210, 210, 230);

				cA = (mA == 0 && mY == -1) ? (byte)35 : (byte)19;
				F.DesenharShape(((Screen.width - 780) / 2), ((Screen.height - 440) / 2), 390, 80, cA, cA, cA, 255);
				cA = (mA == 1 && mY == -1) ? (byte)35 : (byte)19;
				F.DesenharShape(((Screen.width - 780) / 2)+390, ((Screen.height - 440) / 2), 390, 80, cA, cA, cA, 255);

				F.Escrever("Enigma", true, ((Screen.width - 780) / 2)+195-(F.TxtWidth("Enigma", 32, true)/2), ((Screen.height - 440) / 2)+16, 32, 255, 255, 255, 230);
				F.Escrever("Dicas", true, ((Screen.width - 780) / 2)+390+195-(F.TxtWidth("Dicas", 32, true)/2), ((Screen.height - 440) / 2)+16, 32, 255, 255, 255, 230);

				if(F.Key("down") && !F.TeclaDesativada("menuMove") && mY < 2 && !(mX == 1 && mY == 0)){
					mY++;
					F.DesativarTecla("menuMove", 175);
				}
				else if(F.Key("up") && !F.TeclaDesativada("menuMove") && mY > -1 && !(mX == 1 && mY == 2)){
					mY--;
					F.DesativarTecla("menuMove", 175);
				}
				else if(F.Key("right") && !F.TeclaDesativada("menuMove")){
					if(mY == -1 && mA < 1)
						mA++;
					else if(mY > -1 && mX < 2 && mY != 1)
						mX++;

					F.DesativarTecla("menuMove", 175);
				}
				else if(F.Key("left") && !F.TeclaDesativada("menuMove")){
					if(mY == -1 && mA > 0)
						mA--;
					else if(mY > -1 && mX > 0 && mY != 1)
						mX--;

					F.DesativarTecla("menuMove", 175);
				}
				else if(F.Key("x") && !F.TeclaDesativada("x") && mY > -1){
					if(puzzleSide == 1){
						if(rightSide[mY, mX] != -1){
							puzzleVar 			= rightSide[mY, mX];
							rightSide[mY, mX] 	= -1;

							puzzleSide 	= puzzleSide == 1 ? 0 : 1;
							F.DesativarTecla("x", 175);
						}
					}
					else{
						if(leftSide[mY, mX] != -1){
							for(int m=0;m<3;m++){
								for(int n=0;n<3;n++){
									if(!(m == 1 && n == 1) && rightSide[m,n] == -1){
										rightSide[m,n] = leftSide[mY, mX];
										m = n = 3;
										break;
									}
								}
							}
						}
						leftSide[mY, mX] = puzzleVar;
						puzzleSide 	= puzzleSide == 1 ? 0 : 1;
						F.DesativarTecla("x", 175);
					}
				}

				if(mA == 0){
					int o = 0;

					for(int m=0;m<3;m++){
						for(int n=0;n<3;n++){
							if(!(m == 1 && n == 1)){
								o++;
								cA = (m == mY && n == mX && puzzleSide == 1) ? (byte)50 : (byte)19;
								F.DesenharShape(((Screen.width - 780) / 2)+20+(114*n)+400, ((Screen.height - 440) / 2)+100+(114*m), 94, 94, cA, cA, cA, 255);

								cA = (m == mY && n == mX && puzzleSide == 0) ? (byte)50 : (byte)19;
								F.DesenharShape(((Screen.width - 780) / 2)+20+(114*n), ((Screen.height - 440) / 2)+100+(114*m), 94, 94, cA, cA, cA, 255);
								F.Escrever(o+"", true, ((Screen.width - 780) / 2)+20+(114*n)+47-(F.TxtWidth(o+"", 32, true)/2), ((Screen.height - 440) / 2)+100+(114*m)+20, 32, 255, 255, 255, 230);

								if(rightSide[m, n] != -1){
									V.img[1].Texture 		= V.IMG_CAT[9][0];
									V.img[1].Scale 			= new Vector2f(1, 1);
									V.img[1].Position 		= new Vector2f(((Screen.width - 780) / 2)+21+(114*n)+400, ((Screen.height - 440) / 2)+101+(114*m));
									V.img[1].TextureRect	= new IntRect(90*rightSide[m, n], 0, 90, 90);
									V.window.Draw(V.img[1]);
								}

								if(leftSide[m, n] != -1){
									V.img[1].Texture 		= V.IMG_CAT[9][0];
									V.img[1].Scale 			= new Vector2f(1, 1);
									V.img[1].Position 		= new Vector2f(((Screen.width - 780) / 2)+21+(114*n), ((Screen.height - 440) / 2)+101+(114*m));
									V.img[1].TextureRect	= new IntRect(0, 0, 57, 57);
									V.img[1].TextureRect	= new IntRect(90*leftSide[m, n], 0, 90, 90);
									V.window.Draw(V.img[1]);
								}

								if(puzzleSide == 0 && m == mY && n == mX){
									V.img[1].Texture 		= V.IMG_CAT[9][0];
									V.img[1].Scale 			= new Vector2f(1, 1);
									V.img[1].Position 		= new Vector2f(((Screen.width - 780) / 2)+21+(114*n), ((Screen.height - 440) / 2)+101+(114*m));
									V.img[1].TextureRect	= new IntRect(90*puzzleVar, 0, 90, 90);
									V.window.Draw(V.img[1]);
								}
							}
						}
					}
				}
				else if(mA == 1){
					int o = 0;

					F.DesenharShape(((Screen.width - 780) / 2)+360, ((Screen.height - 440) / 2)+100, 400, 320, 000, 000, 000, 230);

					for(int m=0;m<3;m++){
						for(int n=0;n<3;n++){
							if(!(m == 1 && n == 1)){
								o++;
								
								cA = (m == mY && n == mX) ? (byte)50 : (byte)19;
								F.DesenharShape(((Screen.width - 780) / 2)+20+(114*n), ((Screen.height - 440) / 2)+100+(114*m), 94, 94, cA, cA, cA, 255);
								F.Escrever(o+"", true, ((Screen.width - 780) / 2)+20+(114*n)+47-(F.TxtWidth(o+"", 32, true)/2), ((Screen.height - 440) / 2)+100+(114*m)+20, 32, 255, 255, 255, 230);
							
								if(m == mY && n == mX){
									F.Escrever("Forma:  "	+V.puzzleObjects[o-1].forma, true, 	((Screen.width - 720) / 2)+345, ((Screen.height - 400) / 2)+85, 32, 255, 255, 255, 230);
									F.Escrever("Número: "	+V.puzzleObjects[o-1].numero, true, ((Screen.width - 720) / 2)+345, ((Screen.height - 400) / 2)+120, 32, 255, 255, 255, 230);
									F.Escrever("Cor:    "	+V.puzzleObjects[o-1].cor, true, 	((Screen.width - 720) / 2)+345, ((Screen.height - 400) / 2)+155, 32, 255, 255, 255, 230);
								}
							}
						}
					}
				}
			}
			else{
				int q=0;
				mX = mY = mA = 0;

				for(int m=0;m<3;m++){
					for(int n=0;n<3;n++){
						if(!(m == 1 && n == 1)){
							rightSide[m,n] 	= q;
							leftSide[m,n] 	= -1;
							q++;
						}
					}
				}

				puzzleSide 	= 1;
				F.BloquearMov(x, true);
				F.BloquearMov(Configuracoes.controle, true);
				InteracaoContinua(x, Configuracoes.controle, true);
				F.DesativarTecla("x", 500);
				F.DesativarTecla("enter", 500);
			}
		}

		public static void Puzzle2(Geral x){
			if(Configuracoes.controle.pausado){
				if(F.Key("enter") && !F.TeclaDesativada("enter")){
					F.BloquearMov(x, false);
					F.BloquearMov(Configuracoes.controle, false);
					InteracaoContinua(x, Configuracoes.controle, false);
					F.DesativarTecla("enter", 500);
					F.DesativarTecla("x", 500);
				}

				if(lights[0] && lights[1] && lights[2] && !lights[3]){
					F.BloquearMov(x, false);
					F.BloquearMov(Configuracoes.controle, false);
					InteracaoContinua(x, Configuracoes.controle, false);
					F.DesativarTecla("enter", 500);
					F.DesativarTecla("x", 500);

					CurrentScreen.Set("fimdemo");
				}

				F.DesenharShape(((Screen.width - 600) / 2), ((Screen.height - 500) / 2), 600, 500, 210, 210, 210, 230);

				cA = (mA == 0 && mY == -2) ? (byte)35 : (byte)19;
				F.DesenharShape(((Screen.width - 600) / 2), ((Screen.height - 500) / 2), 300, 80, cA, cA, cA, 255);
				cA = (mA == 1 && mY == -2) ? (byte)35 : (byte)19;
				F.DesenharShape(((Screen.width - 600) / 2)+300, ((Screen.height - 500) / 2), 300, 80, cA, cA, cA, 255);

				F.Escrever("Enigma", true, ((Screen.width - 600) / 2)+150-(F.TxtWidth("Enigma", 32, true)/2), ((Screen.height - 500) / 2)+16, 32, 255, 255, 255, 230);
				F.Escrever("Instruções", true, ((Screen.width - 600) / 2)+300+150-(F.TxtWidth("Instruções", 32, true)/2), ((Screen.height - 500) / 2)+16, 32, 255, 255, 255, 230);

				if(F.Key("down") && !F.TeclaDesativada("menuMove") && mY < 1){
					mY++;
					F.DesativarTecla("menuMove", 175);
				}
				else if(F.Key("up") && !F.TeclaDesativada("menuMove") && mY > -2){
					mY--;
					F.DesativarTecla("menuMove", 175);
				}
				else if(F.Key("right") && !F.TeclaDesativada("menuMove")){
					if(mY == -2 && mA < 1)
						mA++;
					else if(mY > -2 && mX < 4)
						mX++;

					F.DesativarTecla("menuMove", 175);
				}
				else if(F.Key("left") && !F.TeclaDesativada("menuMove")){
					if(mY == -2 && mA > 0)
						mA--;
					else if(mY > -2 && mX > 0)
						mX--;

					F.DesativarTecla("menuMove", 175);
				}
				else if(F.Key("x") && !F.TeclaDesativada("x") && mY > -2){
					if(mY == -1 && top[mX] != -1){
						for(int m=0;m<2;m++){
							for(int n=0;n<4;n++){
								if(bottom[m,n] == -1){
									bottom[m, n] 	= top[mX];
									top[mX] = -1;

									if(V.puzzle2[bottom[m, n]].cor == "Amarelo")
										lights[0] = !lights[0];
									if(V.puzzle2[bottom[m, n]].cor == "Vermelho")
										lights[1] = !lights[1];
									if(V.puzzle2[bottom[m, n]].cor == "Azul")
										lights[2] = !lights[2];
									if(V.puzzle2[bottom[m, n]].numero == "1" || V.puzzle2[bottom[m, n]].numero == "4" || V.puzzle2[bottom[m, n]].forma == "Triângulo")
										lights[3] = !lights[3];

									break;
								}
							}
						}
					}
					else if(mY > -1 && bottom[mY, mX] != -1){
						for(int m=0;m<4;m++){
							if(top[m] == -1){
								top[m] = bottom[mY, mX];
								bottom[mY, mX] 	= -1;

								//forma, numero, cor
								if(V.puzzle2[top[m]].cor == "Amarelo")
									lights[0] = !lights[0];
								if(V.puzzle2[top[m]].cor == "Vermelho")
									lights[1] = !lights[1];
								if(V.puzzle2[top[m]].cor == "Azul")
									lights[2] = !lights[2];
								if(V.puzzle2[top[m]].numero == "1" || V.puzzle2[top[m]].numero == "4" || V.puzzle2[top[m]].forma == "Triângulo")
									lights[3] = !lights[3];


								break;
							}
						}
					}

					F.DesativarTecla("x", 500);
				}

				if(mA == 0){
					int o = 0;

					for(int m=0;m<2;m++){
						for(int n=0;n<4;n++){
							o++;
							cA = (m == mY && n == mX) ? (byte)50 : (byte)19;
							F.DesenharShape(((Screen.width - 780) / 2)+20+(114*n)+150, ((Screen.height - 500) / 2)+260+(114*m), 94, 94, cA, cA, cA, 255);

							if(bottom[m, n] != -1){
								V.img[1].Texture 		= V.IMG_CAT[9][0];
								V.img[1].Scale 			= new Vector2f(1, 1);
								V.img[1].Position 		= new Vector2f(((Screen.width - 780) / 2)+21+(114*n)+150, ((Screen.height - 500) / 2)+261+(114*m));
								V.img[1].TextureRect	= new IntRect(90*bottom[m, n], 0, 90, 90);
								V.window.Draw(V.img[1]);
							}
						}
					}

					for(int m=0;m<4;m++){
						cA = (mY == -1 && m == mX) ? (byte)50 : (byte)19;
						F.DesenharShape(((Screen.width - 780) / 2)+20+(114*m)+150, ((Screen.height - 500) / 2)+100, 94, 94, cA, cA, cA, 255);

						if(lights[m])
							F.DesenharShape(((Screen.width - 780) / 2)+20+(114*m)+150, ((Screen.height - 500) / 2)+210, 94, 30, 255, 000, 000, 255);
						else	
							F.DesenharShape(((Screen.width - 780) / 2)+20+(114*m)+150, ((Screen.height - 500) / 2)+210, 94, 30, 000, 255, 000, 255);


						if(puzzleVar != -1){
							V.img[1].Texture 		= V.IMG_CAT[9][0];
							V.img[1].Scale 			= new Vector2f(1, 1);
							V.img[1].Position 		= new Vector2f(((Screen.width - 780) / 2)+21+(114*mX)+150, ((Screen.height - 500) / 2)+261+(114*mY));
							V.img[1].TextureRect	= new IntRect(90*puzzleVar, 0, 90, 90);
							V.window.Draw(V.img[1]);
						}

						if(top[m] != -1){
							V.img[1].Texture 		= V.IMG_CAT[9][0];
							V.img[1].Scale 			= new Vector2f(1, 1);
							V.img[1].Position 		= new Vector2f(((Screen.width - 780) / 2)+21+(114*m)+150, ((Screen.height - 500) / 2)+101);
							V.img[1].TextureRect	= new IntRect(90*top[m], 0, 90, 90);
							V.window.Draw(V.img[1]);
						}
					}
				}
				else if(mA == 1){
					F.DesenharShape(((Screen.width - 600) / 2)+20, ((Screen.height - 500) / 2)+100, 560, 380, 000, 000, 000, 230);

					F.Escrever("Formas de N.1:       Portão", true, 					((Screen.width - 600) / 2)+30, ((Screen.height - 500) / 2)+110, 26, 255, 255, 255, 230);
					F.Escrever("Formas de N.4:       Portão", true, 					((Screen.width - 600) / 2)+30, ((Screen.height - 500) / 2)+132, 26, 255, 255, 255, 230);
					F.Escrever("Formas Amarelas:     1. SS", true, 						((Screen.width - 600) / 2)+30, ((Screen.height - 500) / 2)+154, 26, 255, 255, 255, 230);
					F.Escrever("Formas Azuis:        Abertura do Portão", true, 		((Screen.width - 600) / 2)+30, ((Screen.height - 500) / 2)+176, 26, 255, 255, 255, 230);
					F.Escrever("Formas Vermelhas:    2. SS", true, 						((Screen.width - 600) / 2)+30, ((Screen.height - 500) / 2)+198, 26, 255, 255, 255, 230);
					F.Escrever("Formas Triangulares: Portão", true, 					((Screen.width - 600) / 2)+30, ((Screen.height - 500) / 2)+220, 26, 255, 255, 255, 230);

					F.Escrever("*SS = Sistema de Segurança", true, 						((Screen.width - 600) / 2)+30, ((Screen.height - 500) / 2)+264, 26, 255, 255, 255, 230);
				}
			}
			else{
				int q=0;
				mX = mY = mA = 0;

				for(int m=0;m<2;m++){
					for(int n=0;n<4;n++){
						bottom[m,n] = q;
						q++;
					}
				}

				for(int m=0;m<4;m++){
					top[m] 		= -1;
				}				

				lights[0] = lights[1] = lights[2] = false;
				lights[3] = true;

				puzzleVar = -1;
				F.BloquearMov(x, true);
				F.BloquearMov(Configuracoes.controle, true);
				InteracaoContinua(x, Configuracoes.controle, true);
				F.DesativarTecla("x", 500);
				F.DesativarTecla("enter", 500);
			}
		}

		public static void Video(Geral x, int video){
			if(!(CurrentScreen.Screen() is VideoState)){
				VideoState.Set(x, video);
				CurrentScreen.Change("video");
			}
		}

		public static bool Falar(Geral x, string txt){
			if(!Configuracoes.controle.interagindo){
				V.window.SetView(V.hud);

				if(txt == "")txt = x.dialog;

				if(!Configuracoes.controle.pausado && !F.TeclaDesativada("x")){
					letraAtual = 1;
					F.BloquearMov(x, true);
					F.BloquearMov(Configuracoes.controle, true);
					InteracaoContinua(x, Configuracoes.controle, true);
					F.DesativarTecla("x", 500);
				}
				else if(Configuracoes.controle.pausado && F.Key("x") && !F.TeclaDesativada("x") && letraAtual >= txt.Length && Configuracoes.letraTempo == 50){
					letraAtual = 1;
					F.BloquearMov(x, false);
					F.BloquearMov(Configuracoes.controle, false);
					InteracaoContinua(x, Configuracoes.controle, false);
					F.DesativarTecla("x", 500);

					return true;
				}
				
				if(!F.TeclaDesativada("letraAtual") && letraAtual < txt.Length){
					letraAtual++;
					F.DesativarTecla("letraAtual", Configuracoes.letraTempo);
				}

				if(txt.Substring(letraAtual-1, 1) == "#")
					letraAtual += 6;

				Configuracoes.letraTempo = F.Key("x") ? 10 : 50;
				
				F.DesenharShape(10, Screen.height-210, Screen.width-20, 180, 000, 000, 000, 200);
				new RichText(20, Screen.height-210, 22, true, txt.Substring(0, letraAtual));
				F.Escrever("Aperte X para continuar", true, Screen.width - F.TxtWidth("Aperte X para continuar", 20, true)-20, Screen.height-60, 20, 255, 255, 255, 255);
			
				V.window.SetView(V.view);
			}

			return false;
		}

		public static bool FalarOpcional(Geral x, string txt){
			if(!Configuracoes.controle.interagindo){
				V.window.SetView(V.hud);

				if(txt == "")txt = x.dialog;

				if(!Configuracoes.controle.pausado && F.Key("x") && !F.TeclaDesativada("x")){
					letraAtual = 1;
					F.BloquearMov(x, true);
					F.BloquearMov(Configuracoes.controle, true);
					InteracaoContinua(x, Configuracoes.controle, true);
					F.DesativarTecla("x", 500);
					decisao = false;
				}
				if(Configuracoes.controle.pausado && F.Key("x") && !F.TeclaDesativada("x") && letraAtual >= txt.Length && Configuracoes.letraTempo == 50){
					F.BloquearMov(x, false);
					F.BloquearMov(Configuracoes.controle, false);
					InteracaoContinua(x, Configuracoes.controle, false);
					F.DesativarTecla("x", 500);
					return decisao;
				}
				
				if(!F.TeclaDesativada("letraAtual") && letraAtual < txt.Length){
					letraAtual++;
					F.DesativarTecla("letraAtual", Configuracoes.letraTempo);
				}
				Configuracoes.letraTempo = F.Key("x") ? 10 : 50;
				
				if((F.Key("left") || F.Key("right")) && !F.TeclaDesativada("left")){
					F.DesativarTecla("left", 250);
					F.DesativarTecla("right", 250);
				
					decisao = !decisao;
				}
			
				F.DesenharShape(10, Screen.height-210, Screen.width-20, 180, 000, 000, 000, 200);
				F.Escrever(txt.Substring(0, letraAtual), true, 20, Screen.height-210, 22, 255, 255, 255, 255);
				
				if(decisao){
					F.DesenharShape(Screen.width - (F.TxtWidth("Aceitar", 22, true)*2)-41, Screen.height-70, F.TxtWidth("Aceitar", 22, true)+10, 30, 255, 255, 255, 255);
					F.Escrever("Aceitar", true, Screen.width-(F.TxtWidth("Aceitar", 22, true)*2)-37, Screen.height-70, 22, 000, 000, 000, 255);
					F.Escrever("Recusar", true, Screen.width-F.TxtWidth("Recusar", 22, true)-22, Screen.height-70, 22, 255, 255, 255, 255);
				}
				else{
					F.DesenharShape(Screen.width - (F.TxtWidth("Recusar", 22, true)*2)+47, Screen.height-70, F.TxtWidth("Recusar", 22, true)+10, 30, 255, 255, 255, 255);
					F.Escrever("Aceitar", true, Screen.width-(F.TxtWidth("Aceitar", 22, true)*2)-37, Screen.height-70, 22, 255, 255, 255, 255);
					F.Escrever("Recusar", true, Screen.width-F.TxtWidth("Recusar", 22, true)-22, Screen.height-70, 22, 000, 000, 000, 255);
				}

				V.window.SetView(V.view);
			}
			return false;
		}
		public static void DarItem(Geral x, int y){
			if(!x.activated){
				V.popups.Add(new PopUp(V.itens[y]));
				F.AddItem(Configuracoes.controle, V.itens[y]);
				x.activated = true;
			}
		}
		public static void TrocarItem(Geral x, bool recurso){
			if(!x.activated){
				if(recurso){
					Recurso it = Configuracoes.controle.recursos.Find(item => item.id == x.troca[1]);
					
					if (it != null && it.quantidade >= x.troca[2]){
						V.popups.Add(new PopUp(x.itens[0]));

						it.quantidade -= x.troca[2];

						F.AddItem(Configuracoes.controle, x.itens[0]);
						x.itens.RemoveAt(0);

						x.activated = true;
					}
					else{
						F.Alerta("Você não possui os recursos necessários!");
						foreach(Recurso r in Configuracoes.controle.recursos)
							Console.WriteLine(r.nome +" - "+ x.troca[1]);
					}
				}
				else{
					if(Configuracoes.controle.itens.Count() != 0 && V.itens.IndexOf(Configuracoes.controle.itens[0]) == x.troca[1]){
						V.popups.Add(new PopUp(x.itens[0]));
						
						F.AddItem(Configuracoes.controle, x.itens[0]);
						Configuracoes.controle.itens.RemoveAt(0);

						x.activated = true;
					}
					else
						F.Alerta("Você não está segurando o item necessário!");
				}
			}
			else
				F.Alerta("Você já realizou esta troca!");
		}

		public static void Danificar(Materia x){
			if(Configuracoes.controle.item != null && Configuracoes.controle.item.id == x.itemDestroi && !F.TeclaDesativada("danificar")){
				Efeitos.TremerTela(Configuracoes.controle);

				x.vida -= 25;
				F.DesativarTecla("danificar", 500);
				
				if(x.vida <= 0)
					x.Give(Configuracoes.controle);
			}
		}

		public static void Transformar(Geral x, float vida, int item, float xp){
			
		}

		public static void Batalhar(List<Playable> x1, List<Playable> x2){
			if(!(CurrentScreen.Screen() is BattleState)){
				BattleState.Set(x1, x2);
				CurrentScreen.Change("battle");
			}
		}

		public static void Andar(Playable x, int dir, float qtd){
			x.direcao 		= dir;
			x.movimentar 	= qtd;
		}

		public static void Movimentar(Playable x){
			if(x.movimentar > 0){
				F.BloquearMov(x, true);
				x.fps = 75;
				
				if(x.direcao == 0){
					F.Andar(0, x);
					x.y += Configuracoes.DefaultSpeed()*2;
				}
				if(x.direcao == 1){
					F.Andar(1, x);
					x.y -= Configuracoes.DefaultSpeed()*2;
				}
				if(x.direcao == 2){
					F.Andar(2, x);
					x.x += Configuracoes.DefaultSpeed()*2;
				}
				if(x.direcao == 3){
					F.Andar(3, x);
					x.x -= Configuracoes.DefaultSpeed()*2;
				}

				x.movimentar -= Configuracoes.DefaultSpeed()*2;

				if(x.movimentar <= 0){
					x.frame = 0;

					x.direcao += (x.direcao == 0 || x.direcao == 2) ? 1 : -1;
					x.andando = false;
					x.speed = Configuracoes.defaultSpeed;
					x.fps = 300;
					F.BloquearMov(x, false);
				}
			}
		}

		public static bool mapa, mapaDelay, mapaFim = false;

		public static void MudarMapa(Geral x, string type){
			if(!mapa){
				mapa 		= true;
				mapaDelay 	= false;
				mapaFim 	= false;
				F.BloquearMov(x, true);
				F.BloquearMov(Configuracoes.controle, true);
				InteracaoContinua(x, Configuracoes.controle, true);
				Efeitos.Backdrop("fade-in", 255, 12.75f);
			}

			if(Efeitos.opacidade >= 255 && !F.TeclaDesativada("delayMudarMapa") && !mapaDelay){
				Configuracoes.controle.interior = (type == "interior") ? true : false;
				mapaDelay 	= true;
				Idk.LoadMap(x.mapa, true);
				F.DesativarTecla("delayMudarMapa", 1000);
			}

			if(mapaDelay && !F.TeclaDesativada("delayMudarMapa") && Efeitos.opacidade >= 255)
				Efeitos.Backdrop("fade-out", 000, 12.75f);

			if(mapaDelay && Efeitos.opacidade <= 0)
				mapaFim = true;

			if(mapaFim){
				F.BloquearMov(x, false);
				F.BloquearMov(Configuracoes.controle, false);
				InteracaoContinua(x, Configuracoes.controle, false);
				mapa 	= false;
			}
		}
		
		public static int saveOption = 0, saveStep = 0;
		public static bool saveConfirm;

		public static void Salvar(Geral x){
			if(!Interagindo(x, Configuracoes.controle)){
				InteracaoContinua(x, Configuracoes.controle, true);
				F.BloquearMov(Configuracoes.controle, true);
				Efeitos.Backdrop("fade-in", 125, 25);
				F.DesativarTecla("x", 500);
			}
			else{
				switch(saveStep){
					case 0:
						for(int m=0;m<4;m++){
							byte shapeColor = m == saveOption ? (byte)180 : (byte)15; 
							byte fontColor 	= m == saveOption ? (byte)255 : (byte)112;
							string txt 		= !LoadFileState.saveFiles[m].Empty() ? LoadFileState.saveFiles[m].mapaNome : "-Empty-";
							
							F.DesenharShape(Screen.width/2-150, 110+(105*m), 300, 80, shapeColor, shapeColor, shapeColor, 230);
							F.Escrever(txt, true, Screen.width/2-(F.TxtWidth(txt, 30, true)/2), 125+(105*m), 30, fontColor, fontColor, fontColor, 230);
						}
						
						if(F.Key("up") && !F.TeclaDesativada("saveOption") && saveOption > 0){
							saveOption--;
							F.DesativarTecla("saveOption", 175);
						}
						else if(F.Key("down") && !F.TeclaDesativada("saveOption") && saveOption < 3){
							saveOption++;
							F.DesativarTecla("saveOption", 175);
						}
						else if(F.Key("x") && !F.TeclaDesativada("x")){
							F.DesativarTecla("x", 175);
							saveStep++;
						}
						else if(F.Key("c") && !F.TeclaDesativada("c")){
							F.DesativarTecla("c", 175);
							saveStep = 5;
						}
					break;
					
					case 1:
						if(!LoadFileState.saveFiles[saveOption].Empty()){
							F.Escrever("Deseja substituir o arquivo?", true, Screen.width/2-(F.TxtWidth("Deseja substituir o arquivo?", 28, true)/2), Screen.height/2 - 100, 28, 255, 255, 255, 255, 000, 000, 000, 255, 2);

							byte shapeSim = saveConfirm ? (byte)180 : (byte)15, 	shapeNao = !saveConfirm ? (byte)180 : (byte)15;
							byte fonteSim = saveConfirm ? (byte)255 : (byte)112, 	fonteNao = !saveConfirm ? (byte)255 : (byte)112;

							F.DesenharShape(Screen.width/2-162.5f, Screen.height/2-35, 150, 70, shapeSim, shapeSim, shapeSim, 230);
							F.DesenharShape(Screen.width/2+12.5f, Screen.height/2-35, 150, 70, shapeNao, shapeNao, shapeNao, 230);

							F.Escrever("Sim", true, Screen.width/2-87.5f-(F.TxtWidth("Sim", 28, true)/2), Screen.height/2-22, 28, fonteSim, fonteSim, fonteSim, 255);
							F.Escrever("Não", true, Screen.width/2+87.5f-(F.TxtWidth("Não", 28, true)/2), Screen.height/2-22, 28, fonteNao, fonteNao, fonteNao, 255);

							if((F.Key("left") || F.Key("right")) && !F.TeclaDesativada("saveConfirm")){
								saveConfirm = !saveConfirm;
								F.DesativarTecla("saveConfirm", 175);
							}

							if(F.Key("x") && !F.TeclaDesativada("x")){
								saveStep += saveConfirm ? 1 : -1;
								F.DesativarTecla("x", 175);
							}
						}
						else
							saveStep++;
					break;
					
					case 2:
						F.DesenharShape(Screen.width/2-150, Screen.height/2-50, 300, 100, 000, 000, 000, 200);
						F.Escrever("Salvando...", true, Screen.width/2-(F.TxtWidth("Salvando...", 26, true)/2), Screen.height/2-17.5f, 26, 255, 255, 255, 255);
						saveStep++;
					break;

					case 3:
						Conexao.ExecuteQuery("update saveFile set savePoint = '"+x.id+"' where id = '"+(saveOption+1)+"'");
						Conexao.ExecuteQuery("delete from objeto_item where objeto_id = '"+Configuracoes.controle.id+"'");
						Conexao.ExecuteQuery("delete from objeto_recurso where objeto_id = '"+Configuracoes.controle.id+"'");
						
						foreach(Item i in Configuracoes.controle.itens)
							Conexao.ExecuteQuery("insert into objeto_item (item_id, objeto_id, quantidade, saveFile_id) values ('"+i.id+"', '"+Configuracoes.controle.id+"', '"+i.quantidade+"', '"+(saveOption+1)+"')");

						foreach(Recurso i in Configuracoes.controle.recursos)
							Conexao.ExecuteQuery("insert into objeto_recurso (recurso_id, objeto_id, quantidade, saveFile_id) values ('"+i.id+"', '"+Configuracoes.controle.id+"', '"+i.quantidade+"', '"+(saveOption+1)+"')");

						foreach(KeyValuePair<int, bool> o in V.tempObjects)
							Conexao.ExecuteQuery("insert or ignore into objeto_savefile(objeto_id, saveFile_id) values('"+o.Key+"', '"+(saveOption+1)+"')");

						foreach(KeyValuePair<int, bool> o in V.lockedObjects)
							Conexao.ExecuteQuery("insert or ignore into objeto_locked(objeto_id, saveFile_id) values('"+o.Key+"', '"+(saveOption+1)+"')");

						LoadFileState.LoadSaveFiles();
						
						F.DesativarTecla("delaySalvar", 750);
						saveStep++;
					break;
					
					case 4:
						F.DesenharShape(Screen.width/2-150, Screen.height/2-50, 300, 100, 000, 000, 000, 200);
						F.Escrever("Salvo!", true, Screen.width/2-(F.TxtWidth("Salvo!", 26, true)/2), Screen.height/2-17.5f, 26, 255, 255, 255, 255);

						if(!F.TeclaDesativada("delaySalvar"))
							saveStep++;
					break;
					
					case 5:
						InteracaoContinua(x, Configuracoes.controle, false);
						F.BloquearMov(Configuracoes.controle, false);
						Efeitos.Backdrop("fade-out", 0, 25);
						F.DesativarTecla("x", 500);
						saveStep = 0;
					break;
				}
			}
		}

		public static void InteracaoContinua(Geral x1, Geral x2, bool o){
			if(o)
				V.interacaoContinua.Add(new IC(x1, x2));
			else{
				IC it 	= V.interacaoContinua.Find(item => item.x1 == x1 && item.x2 == x2);
				int result 	= V.interacaoContinua.IndexOf(it);
				V.interacaoContinua.RemoveAt(result);
			}
		}

		public static bool Interagindo(Geral x1, Geral x2){
			IC it = V.interacaoContinua.Find(item => item.x1 == x1 && item.x2 == x2);
			int result 	= V.interacaoContinua.IndexOf(it);
			return result > -1 ? true : false;
		}
	}
}
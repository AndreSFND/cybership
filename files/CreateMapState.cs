using System;
using System.Linq;
using System.Collections.Generic;
using SFML;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Main{
	public class CreateMapState : Screen{

		public override void Draw(){
			if(V.window.Size.X < 800 || V.window.Size.Y < 600) V.window.Size = new Vector2u(800, 600);

			LocalMapDraw();
			TelaPrincipal();
		}

		public override void OnExit(){
			V.view.Zoom(V.Delta);
			V.Delta 		= Screen.width/V.view.Size.X;
			V.view.Center 	= new Vector2f(Screen.width/2, Screen.height/2);
		}

		public override void OnStart(){
			Idk.windowColor = new Color(255, 255, 255);
			V.view.Center 		= new Vector2f(Screen.width/2, Screen.height/2);
		}

		public static int 	menuCreatorPos 		= 250;
		public static int 	spriteTemp 			= 0;
		public static int 	paddingTop			= 0;
		public static int 	menuCreatorTab 		= -1;
		public static bool 	menuCreator 		= true;
		
		public static int 	criarMapaX 			= 0;
		public static int 	criarMapaY 			= 0;

		public static int 	oldMouseX 			= 0;
		public static int 	oldMouseY 			= 0;
		
		public static float colisaoX 			= 0;
		public static float colisaoY 			= 0;
		public static float colisaoW 			= 0;
		public static float colisaoH 			= 0;

		public static bool 	editarMateria1 		= false;
		public static Geral editarMateria2;
		
		public static void TelaPrincipal(){
		
			V.window.SetView(V.view);
			
					V.cameraX = V.view.Center.X - ((Screen.width/V.Delta)/2);
					V.cameraY = V.view.Center.Y - ((Screen.height/V.Delta)/2);

					if(menuCreatorTab > -1){
						criarMapaX = (int)((V.mouseX+((V.view.Center.X-(V.view.Size.X/2))*V.Delta))/(V.IMG_CAT[menuCreatorTab][spriteTemp].Size.X*V.Delta));
						criarMapaY = (int)((V.mouseY+((V.view.Center.Y-(V.view.Size.Y/2))*V.Delta))/(V.IMG_CAT[menuCreatorTab][spriteTemp].Size.Y*V.Delta));
					}
					
					if(F.Key("esc") && !F.TeclaDesativada("esc")){
						if(menuCreatorTab == -1 || !menuCreator)
							menuCreator = !menuCreator;
						else
							menuCreatorTab = -1;
							
						F.DesativarTecla("esc", 200);
					}
					if(V.mouseButton == "Right")
						V.view.Move(new Vector2f(-((V.mouseX-oldMouseX)/V.Delta), -((V.mouseY-oldMouseY)/V.Delta)));

					if(F.Key("enter")){
						V.personagem = new Personagem(1, Data.Nick(), 50, 40, 130, 50, 0, 100, false, 0, new List<Item>(), new List<Recurso>());
						V.personagem.recursos[0].Add(20);
						V.personagem.recursos[1].Add(20);

						Configuracoes.Set(V.personagem);

						CurrentScreen.Change("localmap");
					}
					
					if(criarMapaX >= 0 && criarMapaY >= 0 && !menuCreator && menuCreatorPos == 0){
						if(F.Key("lcontrol"))
							V.img[0].Color = (V.mouseButton == "Left") ? new Color(255, 0, 0, 255) : new Color(255, 0, 0, 120);
						else
							V.img[0].Color = (V.mouseButton == "Left") ? new Color(255, 255, 255, 255) : new Color(255, 255, 255, 120);
					
						if(V.mouseButton == "Left" && !F.TeclaDesativada("mouseLeft")){
							Geral geral;
							Chao criarchao;
						
							switch(menuCreatorTab){
								case -1:
									geral = V.objetos.Find(item => F.MouseIn((item.x-(V.view.Center.X-(V.view.Size.X/2)))*V.Delta, (item.y-(V.view.Center.Y-(V.view.Size.Y/2)))*V.Delta, item.imgWidth*V.Delta, item.imgHeight*V.Delta));
									if(geral != null) EditarMateria(geral);
								break;
								
								case 1:
									geral = V.objetos.Find(item => item.x == criarMapaX*V.IMG_CAT[menuCreatorTab][spriteTemp].Size.X && item.y == criarMapaY*V.IMG_CAT[menuCreatorTab][spriteTemp].Size.Y);
									if(geral != null && F.Key("lcontrol")) V.objetos.RemoveAt(V.objetos.IndexOf(geral));
									if(geral == null && !F.Key("lcontrol")) V.objetos.Add(new Npc(0, spriteTemp, criarMapaX*V.IMG_CAT[menuCreatorTab][spriteTemp].Size.X, criarMapaY*V.IMG_CAT[menuCreatorTab][spriteTemp].Size.Y, 120, 120, 0, 0, 100, 5, false, false, false, "", new int[]{}, false));
								break;
								
								case 2:
									criarchao = V.chao.Find(item => item.x == criarMapaX*V.IMG_CAT[menuCreatorTab][spriteTemp].Size.X && item.y == criarMapaY*V.IMG_CAT[menuCreatorTab][spriteTemp].Size.Y);
									if(criarchao != null && F.Key("lcontrol")) V.chao.RemoveAt(V.chao.IndexOf(criarchao));
									if(criarchao == null && !F.Key("lcontrol")) V.chao.Add(new Chao(spriteTemp, criarMapaX, criarMapaY));
								break;
							}
						}
						
						if(menuCreatorTab >= 0){
							V.img[0].Position 		= new Vector2f(V.IMG_CAT[menuCreatorTab][spriteTemp].Size.X*criarMapaX, V.IMG_CAT[menuCreatorTab][spriteTemp].Size.Y*criarMapaY);
							V.img[0].TextureRect	= new IntRect(0, 0, (int)V.IMG_CAT[menuCreatorTab][spriteTemp].Size.X, (int)V.IMG_CAT[menuCreatorTab][spriteTemp].Size.Y);
							V.img[0].Texture 		= V.IMG_CAT[menuCreatorTab][spriteTemp];
							V.window.Draw(V.img[0]);
						}
						
						V.img[0].Color = new Color(255, 255, 255, 255);
					}

			V.window.SetView(V.hud);

					if(menuCreator && menuCreatorPos < 250)
						menuCreatorPos+= 25;
					else if(!menuCreator && !editarMateria1 && menuCreatorPos > 0){
						menuCreatorPos-= 25;
					}

					F.DesenharShape(Screen.width-menuCreatorPos, 0, 250, Screen.height, 19, 19, 19, 230);
					
					
					if(menuCreatorTab == -1 && !editarMateria1){
						int m = 0;

						foreach(var tab in V.mapCreatorTabs) if(tab.Value){
							F.DesenharShape(Screen.width-menuCreatorPos, 110*m, menuCreatorPos, 55, 35, 35, 35, 230);

							F.Escrever(tab.Key, false, Screen.width-menuCreatorPos+125-(F.TxtWidth(tab.Key, 32, false)/2), 5+(55*m), 32, 255, 255, 255, 255);

							if(V.mouseButton == "Left" && F.MouseIn(Screen.width-menuCreatorPos, 55*m, menuCreatorPos, 55) && tab.Value && menuCreator){
								F.DesativarTecla("mouseLeft", 500);
								menuCreatorTab = m;
							}

							m++;
						}
					}
					
					if(menuCreatorTab != -1){
						F.Escrever(V.mapCreatorTabs.ElementAt(menuCreatorTab).Key, false, Screen.width-menuCreatorPos+125-(F.TxtWidth(V.mapCreatorTabs.ElementAt(menuCreatorTab).Key, 32, false)/2), 5, 32, 255, 255, 255, 255);
			
						if(F.Button("Upload", 32, Screen.width-menuCreatorPos+25, Screen.height-75, 200, 50, 221, 66, 82, 255) && !F.TeclaDesativada("mouseLeft") && menuCreator){
							int m = 0;

							OpenFileDialog theDialog = new OpenFileDialog();
							theDialog.Title = "Selecione uma imagem";
							theDialog.Filter = "PNG|*.png";
							V.mouseButton = "";
							if(theDialog.ShowDialog() == DialogResult.OK){
								while(System.IO.File.Exists(@"res\img\chao\"+m+".png")) m++;
								string destFile = System.IO.Path.Combine(@"res\img\chao", m+".png");

								System.IO.File.Copy(theDialog.FileName, destFile, true);

								V.IMG_CAT[menuCreatorTab].Clear();
								V.ImgLoad();
							}
						}
					}
			

			V.window.SetView(V.menuCreator);
			
					if(menuCreatorTab != -1){
						paddingTop = 0;
						for(int m=0;m<V.IMG_CAT[menuCreatorTab].Count;m++)
							paddingTop += (int)V.IMG_CAT[menuCreatorTab][m].Size.Y+10;
					
						if(V.menuCreator.Center.Y >= 215 && V.menuCreator.Center.Y <= paddingTop-225 && menuCreator){
							V.menuCreator.Move(new Vector2f(0, V.smoothScroll));

							if(V.mouseWheel > 0 || V.mouseWheel < 0)
								V.smoothScroll += -V.mouseWheel*10;
						}

						if(V.menuCreator.Center.Y <= 215){
							V.smoothScroll = 0;
							V.menuCreator.Move(new Vector2f(0, 216-V.menuCreator.Center.Y));
						}
						if(V.menuCreator.Center.Y >= paddingTop-225){
							V.smoothScroll = 0;
							V.menuCreator.Move(new Vector2f(0, -(V.menuCreator.Center.Y-(paddingTop-226))));
						}

						
						if(V.smoothScroll > 0)V.smoothScroll--;
						if(V.smoothScroll < 0)V.smoothScroll++;

						paddingTop = 0;
						
						for(int m=0;m<V.IMG_CAT[menuCreatorTab].Count;m++){
							if(V.IMG_CAT[menuCreatorTab][m].Size.X > 115){
								V.img[0].Scale 		= new Vector2f((float)(115/(float)V.IMG_CAT[menuCreatorTab][m].Size.X), (float)(115/(float)V.IMG_CAT[menuCreatorTab][m].Size.X));
								V.img[0].Position 	= new Vector2f(250-menuCreatorPos + 125-((V.IMG_CAT[menuCreatorTab][m].Size.X*(float)(115/(float)V.IMG_CAT[menuCreatorTab][m].Size.X))/2), paddingTop);
							}
							else{
								V.img[0].Position 	= new Vector2f(250-menuCreatorPos + 125-(V.IMG_CAT[menuCreatorTab][m].Size.X/2), paddingTop);
							}
							
							V.img[0].TextureRect	= new IntRect(0, 0, 115, (int)V.IMG_CAT[menuCreatorTab][m].Size.Y);
							V.img[0].Texture 		= V.IMG_CAT[menuCreatorTab][m];
							V.window.Draw(V.img[0]);

							if(V.mouseButton == "Left" && F.MouseIn(Screen.width-menuCreatorPos+70, (100*m)+275-V.menuCreator.Center.Y, 115, 90) && !F.TeclaDesativada("mouseLeft") && menuCreator){
								spriteTemp 	= m;
								menuCreator = false;
								F.DesativarTecla("mouseLeft", 500);
							}
							
							
							paddingTop += V.IMG_CAT[menuCreatorTab][m].Size.X > 115 ? (int)(V.IMG_CAT[menuCreatorTab][m].Size.Y*(float)(115/(float)V.IMG_CAT[menuCreatorTab][m].Size.X))+10 : (int)V.IMG_CAT[menuCreatorTab][m].Size.Y+10;
							
							V.img[0].Scale = new Vector2f(1, 1);
						}
					}


			V.window.SetView(V.hud);

					if(!menuCreator){
						float zoom 	= 1;
						zoom 		-= (float)V.mouseWheel/10;
						V.view.Zoom(zoom);
						V.Delta = Screen.width/V.view.Size.X;
					
						F.Escrever(V.Delta+"x", true, Screen.width-F.TxtWidth(V.Delta+"x", 32, true)-10, Screen.height-40, 32, 255, 255, 255, 255, 0, 0, 0, 255, 2);
					}
			
					if(editarMateria1) EditarMateriaF();

					oldMouseX = V.mouseX;
					oldMouseY = V.mouseY; 
		}

		public static void LocalMapDraw(){
			V.window.SetView(V.view);

				foreach(Chao x in V.chao.ToList())
					x.Draw();

				foreach(Limite x in V.limites.ToList())
					x.Draw();

				V.objetos.Sort((x, y) => x.zindex.CompareTo(y.zindex));
				foreach(Geral x in V.objetos.ToList())
					x.Draw();

			V.window.SetView(V.hud);

				foreach(IC x in V.interacaoContinua.ToList())
					x.x1.Interagir(1);		
		}

		public static void EditarMateria(Geral x){
			editarMateria1 	= true;
			editarMateria2 	= x;
			
			colisaoX 		= editarMateria2.x;
			colisaoY 		= editarMateria2.y;
			colisaoW 		= editarMateria2.imgWidth;
			colisaoH 		= editarMateria2.imgHeight;
		}
		
		public static void EditarMateriaF(){
			List<string> itens = new List<string>();
			List<string> recursos = new List<string>();
		
			if(menuCreatorPos < 250)
				menuCreatorPos+= 25;

			if(F.Key("esc")){
				editarMateria1 = false;
				F.Limpar(new string[]{"objeto_nome", "objeto_dialog"}, new string[]{"objeto_acao", "objeto_itemDar", "objeto_itemReceber", "objeto_recursosReceber"});
			}
			
			//COLISAO
			//F.DesenharShape(colisaoX, colisaoY, colisaoW, colisaoH, 255, 000, 255, 120, 255, 000, 255, 255, 1);
			
			//F.DesenharShape(colisaoX-3, 			colisaoY-3, 			6, 6, 255, 000, 255, 255);
			//F.DesenharShape(colisaoX+colisaoW-3, 	colisaoY-3, 			6, 6, 255, 000, 255, 255);
			//F.DesenharShape(colisaoX-3, 			colisaoY+colisaoH-3, 	6, 6, 255, 000, 255, 255);
			//F.DesenharShape(colisaoX+colisaoW-3, 	colisaoY+colisaoH-3,	6, 6, 255, 000, 255, 255);
			
			//if(F.MouseIn(colisaoX+colisaoW-3, colisaoY-3, 6, 6) && V.mouseButton == "Left"){
				//colisaoW += V.mouseX-oldMouseX;
				//colisaoY += V.mouseY-oldMouseY;
				//colisaoH -= V.mouseY-oldMouseY;
			//}

			F.DesenharShape(Screen.width-menuCreatorPos, 0, 250, Screen.height, 19, 19, 19, 230);

			F.Escrever("Nome:", false, Screen.width-menuCreatorPos+10, 0, 24, 255, 255, 255, 255);
			F.TextBox("objeto_nome", "", "", Screen.width-menuCreatorPos+20+F.TxtWidth("Nome:", 24, false), 5, 240-(F.TxtWidth("Nome:", 24, false)+20), 25, 255, 255, 255, 255);

			F.Escrever("Diálogo:", false, Screen.width-menuCreatorPos+10, 30, 24, 255, 255, 255, 255);
			F.TextArea("objeto_dialog", "", Screen.width-menuCreatorPos+10, 65, 230, 100, 255, 255, 255, 255);
			
			
			for(int m=0;m<V.itens.Count();m++) itens.Add(V.itens[m].nome);
			for(int m=0;m<V.recursos.Count();m++) recursos.Add(V.recursos[m].nome);
			
			CmbBox cmbBox = V.comboBoxes.Find(item => item.id == "objeto_acao");

			if(cmbBox != null && cmbBox.valor == "Trocar item por recursos"){
				F.Escrever("Quantidade:", false, Screen.width-menuCreatorPos+10, 365, 24, 255, 255, 255, 255);
				F.TextBox("objeto_recursosQuantidade", "Numeros", "", Screen.width-menuCreatorPos+20+F.TxtWidth("Quantidade:", 24, false), 370, 240-(F.TxtWidth("Quantidade:", 24, false)+20), 25, 255, 255, 255, 255);
			}

			if(cmbBox != null && cmbBox.valor != "Somente falar" && cmbBox.valor != "Dar Item")
				F.Escrever("Receber:", false, Screen.width-menuCreatorPos+10, 300, 24, 255, 255, 255, 255);
			if(cmbBox != null && cmbBox.valor == "Trocar item por item")
				F.ComboBox("objeto_itemReceber", itens, Screen.width-menuCreatorPos+10, 335, 230, 25, 255, 255, 255, 255);
			if(cmbBox != null && cmbBox.valor == "Trocar item por recursos")
				F.ComboBox("objeto_recursosReceber", recursos, Screen.width-menuCreatorPos+10, 335, 230, 25, 255, 255, 255, 255);

			if(cmbBox != null && cmbBox.valor != "Somente falar"){
				F.Escrever("Dar:", false, Screen.width-menuCreatorPos+10, 230, 24, 255, 255, 255, 255);
				F.ComboBox("objeto_itemDar", itens, Screen.width-menuCreatorPos+10, 265, 230, 25, 255, 255, 255, 255);
			}
				
				
			F.ComboBox("objeto_acao", new List<string>(){"Somente falar", "Dar Item", "Trocar item por item", "Trocar item por recursos"}, Screen.width-menuCreatorPos+10, 185, 230, 25, 255, 255, 255, 255);
			
			if(F.Button("Salvar", 32, Screen.width-menuCreatorPos+25, Screen.height-75, 200, 50, 221, 66, 82, 255)){
				TxtBox txtBox;
			
				txtBox = V.txtBoxes.Find(item => item.id == "objeto_nome");
				editarMateria2.nome = txtBox.txt;

				txtBox = V.txtBoxes.Find(item => item.id == "objeto_dialog");
				editarMateria2.dialog = txtBox.txt;

				cmbBox = V.comboBoxes.Find(item => item.id == "objeto_acao");
				if(cmbBox.valor == "Dar Item"){
					cmbBox = V.comboBoxes.Find(item => item.id == "objeto_itemDar");
					editarMateria2.itens.Add(V.itens[cmbBox.valorIndex]);
					editarMateria2.troca = new int[]{cmbBox.valorIndex};
				}
				if(cmbBox.valor == "Trocar item por item"){
					int item1, item2;
				
					cmbBox = V.comboBoxes.Find(item => item.id == "objeto_itemDar");
					editarMateria2.itens.Add(V.itens[cmbBox.valorIndex]);
					item1 = cmbBox.valorIndex;
					
					cmbBox = V.comboBoxes.Find(item => item.id == "objeto_itemReceber");
					editarMateria2.itens.Add(V.itens[cmbBox.valorIndex]);
					item2 = cmbBox.valorIndex;
					
					editarMateria2.troca = new int[]{item1, item2};
				}
				if(cmbBox.valor == "Trocar item por recursos"){
					int i1, i2;
				
					cmbBox = V.comboBoxes.Find(item => item.id == "objeto_itemDar");
					editarMateria2.itens.Add(V.itens[cmbBox.valorIndex]);
					i1 = cmbBox.valorIndex;
					
					cmbBox = V.comboBoxes.Find(item => item.id == "objeto_recursosReceber");
					editarMateria2.itens.Add(V.itens[cmbBox.valorIndex]);
					i2 = cmbBox.valorIndex;
					
					txtBox = V.txtBoxes.Find(item => item.id == "objeto_recursosQuantidade");
					editarMateria2.itens.Add(V.itens[int.Parse(txtBox.txt)]);
					
					editarMateria2.troca = new int[]{i1, i2, int.Parse(txtBox.txt)};
				}


				editarMateria1 = false;
				F.Limpar(new string[]{"objeto_nome", "objeto_dialog"}, new string[]{"objeto_acao", "objeto_itemDar", "objeto_itemReceber", "objeto_recursosReceber"});
			}
		}
	}
}
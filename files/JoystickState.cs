using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Threading;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Collections.Generic;
using SFML;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;


namespace Main{
	public class JoystickState : Screen{

		public static bool connected 	= false;
		public static bool delay		= false;
		public static Texture t;
		public static Thread thread;

		public override void Draw(){
			V.window.SetView(V.hud);
			if(V.window.Size.X < 800 || V.window.Size.Y < 600) V.window.Size = new Vector2u(800, 600);

			TelaPrincipal();
		}

		public override void OnExit(){

		}

		public override void OnStart(){
			thread = new Thread(() => {
			    Thread.CurrentThread.IsBackground = true;
			    Joystick.Start();
			    Joystick.Run();
			});
			thread.Start();
			
			qrCode(Joystick.GetLocalIPAddress());
		}

		public static void TelaPrincipal(){
			if((F.Key("esc") || F.Key("c")) && !connected && !F.TeclaDesativada("voltar")){
				thread.Abort();
				Joystick.Stop();
				F.DesativarTecla("voltar", 200);
				CurrentScreen.Change("config");
			}

			F.DesenharShape(0, 0, Screen.width, Screen.height, 19, 19, 19, 255);
		
			F.Escrever("Conectar Joystick", true, Screen.width-F.TxtWidth("Conectar Joystick", 50, true)-30, 20, 50, 255, 255, 255, 255);
			F.Escrever("v0.5.2 Alpha", false, Screen.width-F.TxtWidth("v0.5.2 Alpha", 32, false)-30, Screen.height-50, 32, 255, 255, 255, 255);
			
			F.DesenharShape(Screen.width/2-130, Screen.height/2-130, 260, 260, 255, 255, 255, 255);
			
			V.img[1].Texture = t;
			V.img[1].Position = new Vector2f(Screen.width/2-120, Screen.height/2-120);
			V.window.Draw(V.img[1]);
			
			
			if(!connected)
				F.Escrever("Aguardando...", false, Screen.width/2-(F.TxtWidth("Aguardando...", 32, false)/2), Screen.height/2+140, 32, 255, 255, 255, 255);
			else
				F.Escrever("Controle Conectado!", false, Screen.width/2-(F.TxtWidth("Controle Conectado!", 32, false)/2), Screen.height/2+140, 32, 000, 255, 000, 255);


			if(!delay && connected){
				F.DesativarTecla("delayControle", 1500);
				delay = true;
			}

			if(!F.TeclaDesativada("delayControle") && connected && delay)
				CurrentScreen.Change("config");
		}
		
		public static SFML.Graphics.Image ToSFMLImage(System.Drawing.Bitmap bmp) {
            SFML.Graphics.Color[,] sfmlcolorarray = new SFML.Graphics.Color[bmp.Height, bmp.Width];
            SFML.Graphics.Image newimage = null;
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    System.Drawing.Color csharpcolor = bmp.GetPixel(x, y);
                    sfmlcolorarray[y,x] = new SFML.Graphics.Color(csharpcolor.R, csharpcolor.G, csharpcolor.B, csharpcolor.A);
                }
            }
            newimage = new SFML.Graphics.Image(sfmlcolorarray);
			
            return newimage;
        }
		
		public static void qrCode(string content){
			QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
			QrCode qrCode = new QrCode();
			qrEncoder.TryEncode(content, out qrCode);

			GraphicsRenderer renderer = new GraphicsRenderer(
				new FixedCodeSize(400, QuietZoneModules.Zero), 
				Brushes.Black, 
				Brushes.White);
			MemoryStream ms = new MemoryStream();
			renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, ms);
			Bitmap imageTemp = new Bitmap(ms);
			Bitmap image = new Bitmap(imageTemp, new Size(new Point(240, 240)));
			
			t = new Texture(ToSFMLImage(image));
		}
	}
}
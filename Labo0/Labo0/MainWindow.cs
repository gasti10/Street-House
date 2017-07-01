using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenTK; //La matematica
using OpenTK.Graphics.OpenGL;
using gl = OpenTK.Graphics.OpenGL.GL;
using CGUNS.Shaders;
using CGUNS.Cameras;
using CGUNS.Meshes;
using CGUNS.Parsers;
using CGUNS;
using System.Drawing.Imaging;


namespace Labo0
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Declaracion de variables

        private ShaderProgram sProgram; //Nuestro programa de shaders.
        private ShaderProgram sProgramBump; 

        //Aca se almacenan las texturas
        private Dictionary<string, int> programTextures;

        private Camera myCamera;  //Camara
        private Rectangle viewport; //Viewport a utilizar (Porcion del glControl en donde voy a dibujar).
        private ObjetoGrafico miPiram,mapa;
        private Light myLight;
        private Light[] allLight;
        private Material material;
        #endregion

        private void glControl3_Load(object sender, EventArgs e)
        {            
            logContextInfo(); //Mostramos info de contexto.
            SetupShaders("vPhong.glsl", "fPhong.glsl", out sProgram); //Creamos los shaders y el programa de shader
            //SetupShaders("vBumpedPhong.glsl", "fBumpedPhong.glsl", out sProgram); //Creamos los shaders y el programa de shader
            SetupShaders("vBumpedPhong.glsl", "fBumpedPhong.glsl", out sProgramBump); //Creamos los shaders y el programa de shader

            //Creo el contenedor de texturas
            programTextures = new Dictionary<string, int>();

            SetupTextures();
           // miPiram = new ObjetoGrafico("CGUNS/ModelosOBJ/casa.obj");
            miPiram = new ObjetoGrafico("CGUNS/ModelosOBJ/Street2.obj");
            //miPiram.AddTextureToAllMeshes(GetTextureID("prueba1"));
            //miPiram.Build(sProgram);
            //miPiram.setMaterial(Material.Default);

            mapa = new ObjetoGrafico("CGUNS/ModelosOBJ/plano.obj");
            mapa.Build(sProgramBump);
           
            mapa.setMaterial(Material.Default);

            foreach (Mesh m in miPiram.Meshes)
            {
                Char[] separator = { '.' };
                string prefijo = m.Name.Split(separator)[0];
                switch (prefijo)
                {

                    case "mesa_Object_1":
                        m.AddTexture(GetTextureID("mesa"));
                        m.material = Material.Wood;
                        m.Build(sProgram);
                        break;
                    case "banco1_ParkBench":
                        m.AddTexture(GetTextureID("banco"));
                        m.material = Material.Wood;
                        m.Build(sProgram);
                        break;
                    case "banco2_ParkBench":
                        m.AddTexture(GetTextureID("banco"));
                        m.material = Material.Wood;
                        m.Build(sProgram);
                        break;
                    case "banco3_ParkBench":
                        m.AddTexture(GetTextureID("banco"));
                        m.material = Material.Wood;
                        m.Build(sProgram);
                        break;
                    case "banco4_ParkBench":
                        m.AddTexture(GetTextureID("banco"));
                        m.material = Material.Wood;
                        m.Build(sProgram);
                        break;
                    case "banco5_ParkBench":
                        m.AddTexture(GetTextureID("banco"));
                        m.material = Material.Wood;
                        m.Build(sProgram);
                        break;
                    case "banco6_ParkBench":
                        m.AddTexture(GetTextureID("banco"));
                        m.material = Material.Wood;
                        m.Build(sProgram);
                        break;
                    case "banco7_ParkBench":
                        m.AddTexture(GetTextureID("banco"));
                        m.material = Material.Wood;
                        m.Build(sProgram);
                        break;
                    case "banco8_ParkBench":
                        m.AddTexture(GetTextureID("banco"));
                        m.material = Material.Wood;
                        m.Build(sProgram);
                        break;


                    case "castillo_PROP_Amusement_Park_Jumping_Castle":
                        m.AddTexture(GetTextureID("castillo"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;
                     case "casa1_tipo1":
                         m.AddTexture(GetTextureID("casa_tipo1"));
                         m.material = Material.Default;
                         m.Build(sProgram);
                         break;
                     case "casa2_tipo1":
                         m.AddTexture(GetTextureID("casa_tipo1"));
                         m.material = Material.Default;
                         m.Build(sProgram);
                         break;
                     case "casa3_tipo1":
                         m.AddTexture(GetTextureID("casa_tipo1"));
                         m.material = Material.Default;
                         m.Build(sProgram);
                         break;
                     case "casa4_tipo1":
                         m.AddTexture(GetTextureID("casa_tipo1"));
                         m.material = Material.Default;
                         m.Build(sProgram);
                         break;
                     case "casa5_tipo1":
                         m.AddTexture(GetTextureID("casa_tipo1"));
                         m.material = Material.Default;
                         m.Build(sProgram);
                         break;
                     case "casa6_tipo1":
                         m.AddTexture(GetTextureID("casa_tipo1"));
                         m.material = Material.Default;
                         m.Build(sProgram);
                         break;
                     case "casa7_tipo2":
                         m.AddTexture(GetTextureID("casa_tipo2"));
                         m.material = Material.Default;
                         m.Build(sProgram);
                         break;
                     case "casa8_tipo2":
                         m.AddTexture(GetTextureID("casa_tipo2"));
                         m.material = Material.Default;
                         m.Build(sProgram);
                         break;
                     
                    case "casa9_tipo2":
                        m.AddTexture(GetTextureID("casa_tipo2"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;
                     
                    case "casa10_tipo3":
                        m.AddTexture(GetTextureID("casa_tipo3"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;
                   case "casa11_tipo4":
                        m.AddTexture(GetTextureID("casa_tipo3"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;
                    case "casa12_tipo5":
                        m.AddTexture(GetTextureID("casa_tipo3"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;

                    case "casa13_tipo6":
                        m.AddTexture(GetTextureID("casa_tipo3"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;
                    case "casa14_tipo7":
                        m.AddTexture(GetTextureID("casa_tipo7"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;
                    case "casa15_tipo7":
                        m.AddTexture(GetTextureID("casa_tipo7"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;
                    case "casa16_tipo8":
                        m.AddTexture(GetTextureID("casa_tipo8"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;
                    case "casa17_tipo8":
                        m.AddTexture(GetTextureID("casa_tipo8"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;
                    case "casa18_tipo9":
                        m.AddTexture(GetTextureID("casa_tipo9"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;
                    case "casa19_tipo10":
                        m.AddTexture(GetTextureID("casa_tipo10"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;
                    
                   
                    case "plano_cruece_Plane001_Plane001_20___Default":
                        m.AddTexture(GetTextureID("calle"));//plano
                        m.AddTexture(GetTextureID("calle_n"));//plano_n
                        m.material = Material.Default;
                        m.Build(sProgramBump);
                        break;
                    case "tronco":
                        m.AddTexture(GetTextureID("tronco"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;
                    case "hojas":
                        m.AddTexture(GetTextureID("hojas"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;
                    case "contenedordeluces":
                        m.AddTexture(GetTextureID("contenedordeluces"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;
                    case "pilar":
                        m.AddTexture(GetTextureID("pilar"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;
                    case "sustentor":
                        m.AddTexture(GetTextureID("sustentor"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;
                    case "luzverde1":
                        m.AddTexture(GetTextureID("verde"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;
                    case "luzverde2":
                        m.AddTexture(GetTextureID("verde"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;
                    case "luzamarilla1":
                        m.AddTexture(GetTextureID("amarillo"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;
                    case "luzamarilla2":
                        m.AddTexture(GetTextureID("amarillo"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;
                    case "luzroja1":
                        m.AddTexture(GetTextureID("rojo"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;
                    case "luzroja2":
                        m.AddTexture(GetTextureID("rojo"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;
                    case "auto":
                       
                        m.AddTexture(GetTextureID("auto"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;
                    //arenero1
                    case "arenero1_parte1":
                        m.AddTexture(GetTextureID("arenero_parte1"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;
                    case "arenero1_parte2":
                        m.AddTexture(GetTextureID("arenero_parte2"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;
                    case "arenero1_parte3":
                        m.AddTexture(GetTextureID("arenero_parte3"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;
                    case "arenero1_parte4":
                        m.AddTexture(GetTextureID("arenero_parte4"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;
                    case "arenero1_parte5":
                        m.AddTexture(GetTextureID("arenero_parte5"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;

                    //arenero2
                    case "arenero2_parte1":
                        m.AddTexture(GetTextureID("arenero_parte1"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;
                    case "arenero2_parte2":
                        m.AddTexture(GetTextureID("arenero_parte2"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;
                    case "arenero2_parte3":
                        m.AddTexture(GetTextureID("arenero_parte3"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;
                    case "arenero2_parte4":
                        m.AddTexture(GetTextureID("arenero_parte4"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;
                    case "arenero2_parte5":
                        m.AddTexture(GetTextureID("arenero_parte5"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;

                    //arenero3
                    case "arenero3_parte1":
                        m.AddTexture(GetTextureID("arenero_parte1"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;
                    case "arenero3_parte2":
                        m.AddTexture(GetTextureID("arenero_parte2"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;
                    case "arenero3_parte3":
                        m.AddTexture(GetTextureID("arenero_parte3"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;
                    case "arenero3_parte4":
                        m.AddTexture(GetTextureID("arenero_parte4"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;
                    case "arenero3_parte5":
                        m.AddTexture(GetTextureID("arenero_parte5"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;

                    case "luz":
                        m.AddTexture(GetTextureID("luz"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;
                    case "pilarluz":
                        m.AddTexture(GetTextureID("pilarluz"));
                        m.material = Material.Default;
                        m.Build(sProgram);
                        break;

                }
            }


            allLight = new Light[7];

            allLight[0] = new Light();
            allLight[0].Position = new Vector4(4.0f, 4.0f, 4.0f, 0.0f);//simula ser el SOL
            allLight[0].Iambient = new Vector3(0.1f, 0.1f, 0.1f);
            allLight[0].Idiffuse = new Vector3(0.7f, 0.7f, 0.7f);
           // allLight[0].Ispecular = new Vector3(0.8f, 0.8f, 0.8f);
            allLight[0].ConeAngle = 180.0f;
            allLight[0].ConeDirection = new Vector3(0.0f, -1.0f, 0.0f);
            allLight[0].Enabled = 1;            
                       
            
            //Luz tipo de auto 1
            allLight[1] = new Light();
            allLight[1].Position = new Vector4(1.5f, 0.20f, -3.40f, 1.0f);
            allLight[1].Iambient = new Vector3(0.0f, 0.0f, 0.0f); //239, 127, 26
            allLight[1].Idiffuse = new Vector3(1.0f, 1.0f, 1.0f); //0.243f, 0.165f, 0.005f
            allLight[1].Ispecular = new Vector3(0.8f, 0.8f, 0.8f);
            allLight[1].ConeAngle = 15.0f;
            allLight[1].ConeDirection = new Vector3(-0.95f, -0.25f, 0.0f);
            allLight[1].Enabled =1 ;
            
            //Luz tipo de auto 2
            allLight[2] = new Light();
            allLight[2].Position = new Vector4(1.5f, 0.20f, -0.40f, 1.0f);
            allLight[2].Iambient = new Vector3(0.0f, 0.0f, 0.0f);
            allLight[2].Idiffuse = new Vector3(1f, 1f, 1f);
            allLight[2].Ispecular = new Vector3(0.8f, 0.8f, 0.8f);
            allLight[2].ConeAngle = 15.0f;
            allLight[2].ConeDirection = new Vector3(-0.95f, -0.25f, 0.0f);
            allLight[2].Enabled = 1;
            
            //Luz tipo Farol
            allLight[3] = new Light();
            allLight[3].Position = new Vector4(12.0f, 2.00f, -13.00f, 1.0f);
            allLight[3].Iambient = new Vector3(0f, 0f, 0f);
            allLight[3].Idiffuse = new Vector3(1f, 1f, 1f); //0.243f, 0.165f, 0.005f
           // allLight[3].Ispecular = new Vector3(0.8f, 0.8f, 0.8f);
            allLight[3].ConeAngle = 70.0f;
            allLight[3].ConeDirection = new Vector3(0.0f, -1.0f, 0.0f);
            allLight[3].Enabled = 1;
            
            
            //Luz tipo Faro2
            allLight[4] = new Light();
            allLight[4].Position = new Vector4(-12.0f, 1.00f, -10.00f, 1.0f);
            allLight[4].Iambient = new Vector3(0f, 0f, 0f); //239, 127, 26
            allLight[4].Idiffuse = new Vector3(1f, 1f, 1f);
            allLight[4].Ispecular = new Vector3(0.8f, 0.8f, 0.8f);
            allLight[4].ConeAngle = 70.0f;
            allLight[4].ConeDirection = new Vector3(0.0f, -1.0f, 0.0f);
            allLight[4].Enabled = 1;
            
            //Luz tipo Faro3
            allLight[5] = new Light();
            allLight[5].Position = new Vector4(-13.0f, 1.00f, 13.00f, 1.0f);
            allLight[5].Iambient = new Vector3(0f, 0f, 0f);
            allLight[5].Idiffuse = new Vector3(1f, 1f, 1f);
            allLight[5].Ispecular = new Vector3(0.8f, 0.8f, 0.8f);
            allLight[5].ConeAngle = 70.0f;
            allLight[5].ConeDirection = new Vector3(0.0f, -1.0f, 0.0f);
            allLight[5].Enabled = 1;
            
            
            allLight[6] = new Light();
            allLight[6].Position = new Vector4(13.0f, 1.00f, 13.00f, 1.0f);
            allLight[6].Iambient = new Vector3(0f, 0f, 0f);
            allLight[6].Idiffuse = new Vector3(1f, 1f, 1f);
            allLight[6].Ispecular = new Vector3(0.8f, 0.8f, 0.8f);
            allLight[6].ConeAngle = 70.0f;
            allLight[6].ConeDirection = new Vector3(0.0f, -1.0f, 0.0f);
            allLight[6].Enabled = 1;
            
            
            /*

            //allLight = new Light[1];
            myLight = new Light();
            myLight.Position = new Vector4(4.0f, 4.0f, 4.0f,0.0f);//simula ser el SOL
            myLight.Iambient = new Vector3(0.9f, 0.9f, 0.9f);
            myLight.Idiffuse = new Vector3(1.0f, 1.0f, 0.8f);
            myLight.Ispecular = new Vector3(0.8f, 0.8f, 0.8f);
            myLight.ConeAngle = 10.0f;
            myLight.ConeDirection = new Vector3(0.0f, -1.0f, 0.0f);
            myLight.Enabled = 0;
            allLight[1] = myLight;
            */

            myCamera = new CamaraSimple(new Vector3(2.0f,99.0f,0),myCamera.aspect); //Creo una camara.
      
            gl.Enable(EnableCap.DepthTest);

            gl.ClearColor(Color.FloralWhite); //Configuro el Color de borrado.

             gl.PolygonMode(MaterialFace.Front, PolygonMode.Fill);
   
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

            
        }

        private void SetupTextures()
        {
            //Texturas de los tipos de casas
            CargarTextura("files/Texturas/casas/casa_tipo1.png", "casa_tipo1");
            CargarTextura("files/Texturas/casas/casa_tipo2.png", "casa_tipo2");
            CargarTextura("files/Texturas/casas/casa_tipo3.png", "casa_tipo3");
            CargarTextura("files/Texturas/casas/casa_tipo4.png", "casa_tipo4");
            CargarTextura("files/Texturas/casas/casa_tipo5.png", "casa_tipo5");
            CargarTextura("files/Texturas/casas/casa_tipo6.png", "casa_tipo6");
            CargarTextura("files/Texturas/casas/casa_tipo7.png", "casa_tipo7");
            CargarTextura("files/Texturas/casas/casa_tipo8.png", "casa_tipo8");
            CargarTextura("files/Texturas/casas/casa_tipo9.png", "casa_tipo9");
            CargarTextura("files/Texturas/casas/casa_tipo10.png", "casa_tipo10");
            //Textura del castillo
            CargarTextura("files/Texturas/castillo/castillo.png", "castillo");

            //textura del auto
            CargarTextura("files/Texturas/auto/autoford.jpg", "auto");
            //texturas de los bancos
            CargarTextura("files/Texturas/banco/banco.png", "banco");
            //textura de las hojas del arbol
         
            //textura de las hojas del arbol
            CargarTextura("files/Texturas/arbol/tronco.jpg", "mesa");
            
            //textura del plano
            CargarTextura("files/Texturas/plano/cruce.jpg", "calle");
            CargarTextura("files/Texturas/plano/cruce_NRM.jpg", "calle_n");

            //textura de los arboles
            CargarTextura("files/Texturas/arbol/hojas.jpg", "hojas");
            CargarTextura("files/Texturas/arbol/tronco.jpg", "tronco");


           
            //textura del cruce
            CargarTextura("files/Texturas/cruce/plano.jpg", "plano");
            CargarTextura("files/Texturas/cruce/line.png", "line");
            CargarTextura("files/Texturas/cruce/road.jpg", "road");

            //textura de los semaforos
            CargarTextura("files/Texturas/semaforo/amarillo.jpg", "amarillo");
            CargarTextura("files/Texturas/semaforo/verde.jpg", "verde");
            CargarTextura("files/Texturas/semaforo/rojo.jpg", "rojo");
            CargarTextura("files/Texturas/semaforo/pilar.jpg", "pilar");
            CargarTextura("files/Texturas/semaforo/sustentor.jpg", "sustentor");
            CargarTextura("files/Texturas/semaforo/contenedordeluces.jpg", "contenedordeluces");

            //textura de los areneros
            CargarTextura("files/Texturas/arenero/arenero_parte1.png", "arenero_parte1");
            CargarTextura("files/Texturas/arenero/arenero_parte2.png", "arenero_parte2");
            CargarTextura("files/Texturas/arenero/arenero_parte3.png", "arenero_parte3");
            CargarTextura("files/Texturas/arenero/arenero_parte4.png", "arenero_parte4");
            CargarTextura("files/Texturas/arenero/arenero_parte5.png", "arenero_parte5");

            //textura de los faroles
            CargarTextura("files/Texturas/farol/luz.png", "luz");
            CargarTextura("files/Texturas/farol/pilarluz.jpg", "pilarluz");

        }

        private int CargarTextura(String imagenTex,String nombre)
        {
            //Selecciono como textura activa la que corresponda segun el orden que se agregue.
            gl.ActiveTexture(TextureUnit.Texture0 + programTextures.Count);
            int texId = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, texId);
            Bitmap bitmap = new Bitmap(Image.FromFile(imagenTex));
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                             ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bitmap.UnlockBits(data);

            //Guardo en mi diccionario el ID correspondiente a la textura activa.
            programTextures.Add(nombre, texId - 1);
            return texId;

        }
        //Retorna la ID de una textura
        private int GetTextureID(string nombre)
        {
            int value;
            if (!programTextures.TryGetValue(nombre, out value))
                throw new NullReferenceException("La textura " + nombre + " no se encuentra en el diccionario");
            return value;
        }

        private void glControl3_Paint(object sender, PaintEventArgs e)
        {
            Matrix4 modelMatrix = Matrix4.Identity; //Por ahora usamos la identidad.
            Matrix4 viewMatrix = myCamera.getViewMatrix();
            Matrix4 projMatrix = myCamera.getProjectionMatrix();
            
            gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit); //Borramos el contenido del glControl.

            gl.Viewport(viewport); //Especificamos en que parte del glControl queremos dibujar.
            sProgram.Activate(); //Activamos el programa de shaders
            //Seteamos los valores uniformes.
            sProgram.SetUniformValue("projMat", projMatrix);
            sProgram.SetUniformValue("viewMatrix", viewMatrix);

            //Dibujamos el objeto.
            //Matrix4 scale = Matrix4.CreateRotationY(0.0f);
            //Matrix4 traslation = Matrix4.CreateTranslation(0, 0.0f, 75.0f);
            //Matrix4.Mult( ref traslation, ref scale ,out modelMatrix );

            sProgram.SetUniformValue("modelMatrix", modelMatrix);

            foreach (Mesh m in miPiram.Meshes)
            {
                Char[] separator = { '.' };
                string prefijo = m.Name.Split(separator)[0];
               
                if ((prefijo != "plano_cruece_Plane001_Plane001_20___Default") && (prefijo != "cruce_cruce_Line002_Line002_20___Default") && (prefijo != "line_cruce_Object010_Object010_21___Default"))
                    m.Dibujar(sProgram);
             
            }
          



            sProgram.SetUniformValue("A", 0.3f);
            sProgram.SetUniformValue("B", 0.007f);
            sProgram.SetUniformValue("C", 0.00008f);
            sProgram.SetUniformValue("numLights", allLight.Length);
            for (int i = 0; i < allLight.Length; i++)
            {
                sProgram.SetUniformValue("allLights[" + i + "].position", allLight[i].Position);
                sProgram.SetUniformValue("allLights[" + i + "].Ia", allLight[i].Iambient);
                sProgram.SetUniformValue("allLights[" + i + "].Ip", allLight[i].Idiffuse);
               // sProgram.SetUniformValue("allLights[" + i + "].Is", allLight[i].Ispecular);
                sProgram.SetUniformValue("allLights[" + i + "].coneAngle", allLight[i].ConeAngle);
                sProgram.SetUniformValue("allLights[" + i + "].coneDirection", allLight[i].ConeDirection);
                sProgram.SetUniformValue("allLights[" + i + "].enabled", allLight[i].Enabled);
            }
            
            sProgram.SetUniformValue("cameraPosition", myCamera.getPosition());
            sProgram.Deactivate(); //Desactivamos el programa de shader.

            //SHADER para el piso
            sProgramBump.Activate(); //Activamos el programa de shaders


            /// BUMPED PHONG
            //Configuracion de los valores uniformes del shader compartidos por todos
            material = Material.Obsidian;

            sProgramBump.SetUniformValue("modelMatrix", modelMatrix);
            sProgramBump.SetUniformValue("projMatrix", myCamera.getProjectionMatrix());
            sProgramBump.SetUniformValue("viewMatrix", myCamera.getViewMatrix());
            // sProgramBump.SetUniformValue("normalMatrix", normalMatrix);
            sProgramBump.SetUniformValue("A", 0.3f);
            sProgramBump.SetUniformValue("B", 0.007f);
            sProgramBump.SetUniformValue("C", 0.00008f);
            sProgramBump.SetUniformValue("material.Ka", material.Kambient);
            //sProgramBump.SetUniformValue("material.Kd", material.Kdiffuse);
            sProgramBump.SetUniformValue("material.Ks", material.Kspecular);
            sProgramBump.SetUniformValue("material.Shininess", material.Shininess);
            sProgramBump.SetUniformValue("ColorTex", GetTextureID("calle"));
            sProgramBump.SetUniformValue("NormalMapTex", GetTextureID("calle_n"));


            sProgramBump.SetUniformValue("numLights", allLight.Length);
            for (int i = 0; i < allLight.Length; i++)
            {
                sProgramBump.SetUniformValue("allLights[" + i + "].position", allLight[i].Position);
                sProgramBump.SetUniformValue("allLights[" + i + "].Ia", allLight[i].Iambient);
                sProgramBump.SetUniformValue("allLights[" + i + "].Ip", allLight[i].Idiffuse);
               // sProgramBump.SetUniformValue("allLights[" + i + "].Is", allLight[i].Ispecular);
                sProgramBump.SetUniformValue("allLights[" + i + "].coneAngle", allLight[i].ConeAngle);
                sProgramBump.SetUniformValue("allLights[" + i + "].coneDirection", allLight[i].ConeDirection);
                sProgramBump.SetUniformValue("allLights[" + i + "].enabled", allLight[i].Enabled);
             }
            //Dibujamos el mapa
            Matrix4 scale = Matrix4.CreateScale(15.5f);
            Matrix4 traslacion = Matrix4.CreateTranslation(-3f, 0.1f, 0.0f);
            Matrix4 modelPlano = scale * traslacion ;


            sProgramBump.SetUniformValue("modelMatrix", modelMatrix);
             //mapa.Dibujar(sProgramBump);

            foreach (Mesh m in miPiram.Meshes)
            {
                Char[] separator = { '.' };
                string prefijo = m.Name.Split(separator)[0];

                if (prefijo == "plano_cruece_Plane001_Plane001_20___Default")
                   m.Dibujar(sProgramBump);
              // if (prefijo == "cruce_cruce_Line002_Line002_20___Default")
               //    m.Dibujar(sProgramBump);
              // if (prefijo == "line_cruce_Object010_Object010_21___Default")
                //    m.Dibujar(sProgramBump);
            }

            sProgramBump.Deactivate(); //Desactivamos el programa de shader.

            glControl3.SwapBuffers(); //Intercambiamos buffers frontal y trasero, para evitar flickering.
        }

        private void glControl3_Resize(object sender, EventArgs e)
        {   //Actualizamos el viewport para que dibuje en el centro de la pantalla.
            Size size = glControl3.Size;
            int w = size.Width;
            int h = size.Height;
            float aspecto = 1;

            // Calculate aspect ratio, checking for divide by zero
            if (h > 0)
            {
                aspecto = (float)w / (float)h;
            }
            if (myCamera == null ) myCamera = new QuatCamera(aspecto);
            myCamera.aspect = aspecto;
            //Configuro el tamaño del viewport
            viewport.X = 0;
            viewport.Y = 0;
            viewport.Width = w;
            viewport.Height = h;
            glControl3.Invalidate(); //Invalidamos el glControl para que se redibuje.(llama al metodo Paint)
        }

        private void SetupShaders(String vShaderFile,String fShaderFile, out ShaderProgram sProgram)
        {
            //Lo hago con mis clases, que encapsulan el manejo de shaders.
            //1. Creamos los shaders, a partir de archivos.
            Shader vShader = new Shader(ShaderType.VertexShader, "files/shaders/" + vShaderFile);
            Shader fShader = new Shader(ShaderType.FragmentShader, "files/shaders/" + fShaderFile);
            //2. Los compilamos
            vShader.Compile();
            fShader.Compile();
            //3. Creamos el Programa de shader con ambos.
            sProgram = new ShaderProgram();
            sProgram.AddShader(vShader);
            sProgram.AddShader(fShader);
            //4. Construimos (linkeamos) el programa.
            sProgram.Build();
            //5. Ya podemos eliminar los shaders compilados. (Si no los vamos a usar en otro programa)
            vShader.Delete();
            fShader.Delete();
        }

        private void logContextInfo()
        {
            String version, renderer, shaderVer, vendor;//, extensions;
            version = gl.GetString(StringName.Version);
            renderer = gl.GetString(StringName.Renderer);
            shaderVer = gl.GetString(StringName.ShadingLanguageVersion);
            vendor = gl.GetString(StringName.Vendor);
            //extensions = gl.GetString(StringName.Extensions);
            log("========= CONTEXT INFORMATION =========");
            log("Renderer:       {0}", renderer);
            log("Vendor:         {0}", vendor);
            log("OpenGL version: {0}", version);
            log("GLSL version:   {0}", shaderVer);
            //log("Extensions:" + extensions);
            log("===== END OF CONTEXT INFORMATION =====");

        }
        private void log(String format, params Object[] args)
        {
            System.Diagnostics.Debug.WriteLine(String.Format(format, args), "[CGUNS]");
        }

        private void glControl3_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
                switch (e.KeyChar) {
                    case 'j':
                      myCamera.Acercar(1f);
                      break;
                    case 'k':
                      myCamera.Alejar(1f);
                      break;
                    case 'w':
                      myCamera.Arriba();
                      break;
                    case 's':
                      myCamera.Abajo();
                      break;
                    case 'a':
                      myCamera.Izquierda();
                      break;
                    case 'd':
                      myCamera.Derecha();
                      break;
                    case 'c':
                        myCamera = new QuatCamera(myCamera.aspect);
                        break;
                    case 'v':
                        myCamera = new CamaraSimple(new Vector3(2.0f, 99.0f, 0),myCamera.aspect);
                        break;                
                    case 'g':
                      myLight.Position = new Vector4(0,0,2.0f,1.0f);
                      myLight.ConeDirection = new Vector3(0,0,-1.0f);    
                    break;           
      }
      glControl3.Invalidate();
    }
  }
        }   
            
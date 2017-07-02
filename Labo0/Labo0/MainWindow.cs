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

        private Camera[] myCameras;
        private int indexCamaras;
        private Camera myCamera;  //Camara
        private Rectangle viewport; //Viewport a utilizar (Porcion del glControl en donde voy a dibujar).
        private ObjetoGrafico miPiram, auto, auto2;
        private Light[] allLight;
        private Material material;

        //Variables utilizadas para la animacion
        private float LuzAmbiente = 0.3f;
        private bool flagAmbienteSuma = true;
        private Matrix4 modelAuto1 = Matrix4.Identity;
        private Matrix4 modelAuto2 = Matrix4.Identity;
        private Matrix4 modelAuto3 = Matrix4.Identity;
        private bool animacionAuto1 = true;
        private bool animacionAuto2 = true;
        private int contadorAuto1 = 0; // Contador para tiempo de frenado
        private int contadorAuto2 = 0;
        private int posXAuto1 = 15;


        private const float DEG2RAD = (float)(Math.PI / 180.0);
        #endregion

        private void glControl3_Load(object sender, EventArgs e)
        {
            logContextInfo(); //Mostramos info de contexto.
            SetupShaders("vPhong.glsl", "fPhong.glsl", out sProgram); //Creamos los shaders y el programa de shader
            SetupShaders("vBumpedPhong.glsl", "fBumpedPhong.glsl", out sProgramBump); //Creamos los shaders y el programa de shader

            //Creo el contenedor de texturas
            programTextures = new Dictionary<string, int>();

            SetupTextures();

            #region Creacion de Objetos
            miPiram = new ObjetoGrafico("CGUNS/ModelosOBJ/Street3.obj");
            auto = new ObjetoGrafico("CGUNS/ModelosOBJ/Ford2.obj");
            auto2 = new ObjetoGrafico("CGUNS/ModelosOBJ/Ford2.obj");
            
            auto.Build(sProgram);
            auto.setMaterial(Material.Bronze);
            auto.AddTextureToAllMeshes(GetTextureID("auto"));

            auto2.Build(sProgram);
            auto2.setMaterial(Material.Default);
            auto2.AddTextureToAllMeshes(GetTextureID("auto"));
#endregion

            #region Build de mapa

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
                        m.material = Material.YellowPlastic;
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

            #endregion

            #region Luces

            allLight = new Light[11];

            allLight[0] = new Light();
            allLight[0].Position = new Vector4(4.0f, 4.0f, 4.0f, 0.0f);//simula ser el SOL
            allLight[0].Iambient = new Vector3(0.1f, 0.1f, 0.1f);
            allLight[0].Idiffuse = new Vector3(0.7f, 0.7f, 0.7f);
            allLight[0].ConeAngle = 180.0f;
            allLight[0].ConeDirection = new Vector3(0.0f, -1.0f, 0.0f);
            allLight[0].Enabled = 1;

            //farol6
            allLight[1] = new Light();
            allLight[1].Position = new Vector4(-16.69486f, 24.33494f, -28.28732f, 1.0f);
            allLight[1].Iambient = new Vector3(0.0f, 0.0f, 0.0f); //239, 127, 26
            allLight[1].Idiffuse = new Vector3(1.0f, 1.0f, 1.0f); //0.243f, 0.165f, 0.005f            
            allLight[1].ConeAngle = 30.0f;
            allLight[1].ConeDirection = new Vector3(0.0f, -1.0f, 0.0f);
            allLight[1].Enabled = 0;

            //farol2
            allLight[2] = new Light();
            allLight[2].Position = new Vector4(19.69486f, 24.33494f, -37.28732f, 1.0f);
            allLight[2].Iambient = new Vector3(0.0f, 0.0f, 0.0f);
            allLight[2].Idiffuse = new Vector3(1f, 1f, 1f);
            allLight[2].ConeAngle = 30.0f;
            allLight[2].ConeDirection = new Vector3(0.0f, -1.0f, 0.0f);
            allLight[2].Enabled = 0;

            //farol1
            allLight[3] = new Light();
            allLight[3].Position = new Vector4(19.69486f, 24.33494f, -23.28732f, 1.0f);
            allLight[3].Iambient = new Vector3(0f, 0f, 0f);
            allLight[3].Idiffuse = new Vector3(1f, 1f, 1f); //0.243f, 0.165f, 0.005f            
            allLight[3].ConeAngle = 30.0f;
            allLight[3].ConeDirection = new Vector3(0.0f, -1.0f, 0.0f);
            allLight[3].Enabled = 0;

            //farol3
            allLight[4] = new Light();
            allLight[4].Position = new Vector4(25.69486f, 24.33494f, -41.28732f, 1.0f);
            allLight[4].Iambient = new Vector3(0f, 0f, 0f); //239, 127, 26
            allLight[4].Idiffuse = new Vector3(1f, 1f, 1f);
            allLight[4].ConeAngle = 30.0f;
            allLight[4].ConeDirection = new Vector3(0.0f, -1.0f, 0.0f);
            allLight[4].Enabled = 0;

            //farol4
            allLight[5] = new Light();
            allLight[5].Position = new Vector4(30.69486f, 24.33494f, -23.28732f, 1.0f);
            allLight[5].Iambient = new Vector3(0f, 0f, 0f);
            allLight[5].Idiffuse = new Vector3(1f, 1f, 1f);
            allLight[5].ConeAngle = 30.0f;
            allLight[5].ConeDirection = new Vector3(0.0f, -1.0f, 0.0f);
            allLight[5].Enabled = 0;

            //farol5
            allLight[6] = new Light();
            allLight[6].Position = new Vector4(42.69486f, 24.33494f, -23.28732f, 1.0f);
            allLight[6].Iambient = new Vector3(0f, 0f, 0f);
            allLight[6].Idiffuse = new Vector3(1f, 1f, 1f);
            allLight[6].ConeAngle = 30.0f;
            allLight[6].ConeDirection = new Vector3(0.0f, -1.0f, 0.0f);
            allLight[6].Enabled = 0;

            //farol6
            allLight[7] = new Light();
            allLight[7].Position = new Vector4(42.69486f, 24.33494f, -30.28732f, 1.0f);
            allLight[7].Iambient = new Vector3(0f, 0f, 0f);
            allLight[7].Idiffuse = new Vector3(1f, 1f, 1f);
            allLight[7].ConeAngle = 30.0f;
            allLight[7].ConeDirection = new Vector3(0.0f, -1.0f, 0.0f);
            allLight[7].Enabled = 0;

            //farol7
            allLight[8] = new Light();
            allLight[8].Position = new Vector4(-16.69486f, 24.33494f, -37.28732f, 1.0f);
            allLight[8].Iambient = new Vector3(0.0f, 0.0f, 0.0f); //239, 127, 26
            allLight[8].Idiffuse = new Vector3(1.0f, 1.0f, 1.0f); //0.243f, 0.165f, 0.005f            
            allLight[8].ConeAngle = 30.0f;
            allLight[8].ConeDirection = new Vector3(0.0f, -1.0f, 0.0f);
            allLight[8].Enabled = 0;

            //Luz tipo de auto 1
            allLight[9] = new Light();
            allLight[9].Position = new Vector4(42.0f, 0.20f, 1.00f, 1.0f);
            allLight[9].Iambient = new Vector3(0.0f, 0.0f, 0.0f); //239, 127, 26
            allLight[9].Idiffuse = new Vector3(2.0f, 2.0f, 2.0f); //0.243f, 0.165f, 0.005f            
            allLight[9].ConeAngle = 15.0f;
            allLight[9].ConeDirection = new Vector3(-0.95f, -0.25f, 0.0f);
            allLight[9].Enabled = 1;

            //Luz tipo de auto 2
            allLight[10] = new Light();
            allLight[10].Position = new Vector4(42.0f, 0.20f, 3.80f, 1.0f); //-8.0f, 0.20f, 3.80f, 1.0f Posicion (0,0,0) del auto
            allLight[10].Iambient = new Vector3(0.0f, 0.0f, 0.0f);
            allLight[10].Idiffuse = new Vector3(2.0f, 2.0f, 2.0f);
            allLight[10].ConeAngle = 15.0f;
            allLight[10].ConeDirection = new Vector3(-0.95f, -0.25f, 0.0f);
            allLight[10].Enabled = 1;

            #endregion       

            #region Inicializo Posicion de los Autos

            //Inicializo la Posicion del auto1
            Matrix4 scale = Matrix4.CreateScale(2f);
            modelAuto1 = Matrix4.CreateTranslation(25f, -1.0f, 0f);
            modelAuto1 = modelAuto1 * scale;

            //Inicializo la Posicion del auto2              
            modelAuto2 = Matrix4.CreateRotationY(DEG2RAD * 90f);
            modelAuto2 = modelAuto2 * Matrix4.CreateTranslation(-4.5f, -1.0f, -20f);
            modelAuto2 = modelAuto2 * scale;

            /*
            //Inicializo la Posicion del auto3             
            modelAuto3 = Matrix4.CreateRotationY(DEG2RAD * 90f);
            modelAuto3 = modelAuto3 * Matrix4.CreateTranslation(0.5f, -1.0f, -20f);
            modelAuto3 = modelAuto3 * scale;
            */

            #endregion

            #region Camaras
            myCamera = new CamaraSimple(new Vector3(2.0f, 99.0f, 0), myCamera.aspect); //Creo una camara.

            indexCamaras = 0;

            myCameras = new CamaraSimple[5];
            myCameras[0] = new CamaraSimple(new Vector3(2.0f, 99.0f, 0), myCamera.aspect);
            myCameras[1] = new CamaraSimple(new Vector3(20.0f, 20.0f, 20f), myCamera.aspect);
            myCameras[2] = new CamaraSimple(new Vector3(50.0f, 20.0f, -50), myCamera.aspect); // camara desde la plaza
            myCameras[3] = new CamaraSimple(new Vector3(-10f, 8f, 10f), myCamera.aspect);
            myCameras[4] = new CamaraSimple(new Vector3(45f, 3.0f, 2f), new Vector3(-50f, 1.0f, 0f), myCamera.aspect); // camara primera persona auto1
            #endregion

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

            #region Dibujado de la Ciudad y los Autos con Phong
            sProgram.Activate(); //Activamos el programa de shaders
            //Seteamos los valores uniformes.
            sProgram.SetUniformValue("projMat", projMatrix);
            sProgram.SetUniformValue("viewMatrix", viewMatrix);
                      
            sProgram.SetUniformValue("modelMatrix", modelMatrix);
            sProgram.SetUniformValue("A", 0.3f);
            sProgram.SetUniformValue("B", 0.007f);
            sProgram.SetUniformValue("C", 0.00008f);
            sProgram.SetUniformValue("numLights", allLight.Length);
            for (int i = 0; i < allLight.Length; i++)
            {
                sProgram.SetUniformValue("allLights[" + i + "].position", allLight[i].Position);
                sProgram.SetUniformValue("allLights[" + i + "].Ia", allLight[i].Iambient);
                sProgram.SetUniformValue("allLights[" + i + "].Ip", allLight[i].Idiffuse);             
                sProgram.SetUniformValue("allLights[" + i + "].coneAngle", allLight[i].ConeAngle);
                sProgram.SetUniformValue("allLights[" + i + "].coneDirection", allLight[i].ConeDirection);
                sProgram.SetUniformValue("allLights[" + i + "].enabled", allLight[i].Enabled);
            }

            foreach (Mesh m in miPiram.Meshes)
            {
                Char[] separator = { '.' };
                string prefijo = m.Name.Split(separator)[0];

                if ((prefijo != "plano_cruece_Plane001_Plane001_20___Default") && (prefijo != "cruce_cruce_Line002_Line002_20___Default") && (prefijo != "line_cruce_Object010_Object010_21___Default"))
                    m.Dibujar(sProgram);

            }

            //Dibujamos el auto 1
            sProgram.SetUniformValue("modelMatrix", modelAuto1);            
            auto.Dibujar(sProgram);

            //Dibujamos el auto 2
            sProgram.SetUniformValue("modelMatrix", modelAuto2);            
            auto2.Dibujar(sProgram);
            
            sProgram.SetUniformValue("cameraPosition", myCamera.getPosition());
            sProgram.Deactivate(); //Desactivamos el programa de shader.
            #endregion

            #region Dibujado del piso con BumpMapping

            //SHADER para el piso
            sProgramBump.Activate(); //Activamos el programa de shaders
                        
            //Configuracion de los valores uniformes
            material = Material.Obsidian;

            sProgramBump.SetUniformValue("modelMatrix", modelMatrix);
            sProgramBump.SetUniformValue("projMatrix", myCamera.getProjectionMatrix());
            sProgramBump.SetUniformValue("viewMatrix", myCamera.getViewMatrix());            
            sProgramBump.SetUniformValue("A", 0.3f);
            sProgramBump.SetUniformValue("B", 0.007f);
            sProgramBump.SetUniformValue("C", 0.00008f);
            sProgramBump.SetUniformValue("material.Ka", material.Kambient);            
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
                sProgramBump.SetUniformValue("allLights[" + i + "].coneAngle", allLight[i].ConeAngle);
                sProgramBump.SetUniformValue("allLights[" + i + "].coneDirection", allLight[i].ConeDirection);
                sProgramBump.SetUniformValue("allLights[" + i + "].enabled", allLight[i].Enabled);
             }
            
            //Dibujamos el piso            
            sProgramBump.SetUniformValue("modelMatrix", modelMatrix);             

            foreach (Mesh m in miPiram.Meshes)
            {
                Char[] separator = { '.' };
                string prefijo = m.Name.Split(separator)[0];

                if (prefijo == "plano_cruece_Plane001_Plane001_20___Default")
                   m.Dibujar(sProgramBump);              
            }

            sProgramBump.Deactivate(); //Desactivamos el programa de shader.
            #endregion

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
            //SETEO DE ASPECTO A LAS CAMARAS
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

        #region Animación

        private void timer1_Tick(object sender, EventArgs e)
        {
            AnimacionLuces();
           
            glControl3.Invalidate();
        }

        private void AnimacionLuces()
        {
            Console.Write(LuzAmbiente);
            allLight[0].Iambient = new Vector3(LuzAmbiente, LuzAmbiente, LuzAmbiente);
            
            if (flagAmbienteSuma)
            {
                LuzAmbiente = LuzAmbiente + 0.05f;
                if (LuzAmbiente > 2f)
                    flagAmbienteSuma = false;
            }
            else
            {
                LuzAmbiente = LuzAmbiente - 0.05f;
                if (LuzAmbiente < 0.37f)
                    flagAmbienteSuma = true;
            }

            //Prendo las luces de Noche
            if (LuzAmbiente < 1.2f)
            {
                Console.Write("prendo");
                for (int i = 1; i < allLight.Length; i++)
                    allLight[i].Enabled = 1;
            }else
                {
                Console.Write("apago");
                for (int i = 1; i < allLight.Length; i++)
                    allLight[i].Enabled = 0;
                }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            contadorAuto1++;
            //Animacion Auto1
            if ((animacionAuto1) && (contadorAuto1 < 750))
            {
                modelAuto1 = modelAuto1 * Matrix4.CreateTranslation(-0.15f, 0f, 0f);
                allLight[9].Position -= new Vector4(0.15f, 0f, 0f, 0f);
                allLight[10].Position -= new Vector4(0.15f, 0f, 0f, 0f);
                myCameras[4].setPosition(myCameras[4].getPosition() + new Vector3(-0.15f, 0f, 0f));

                if (allLight[9].Position.X < posXAuto1)
                    animacionAuto1 = false;
            }
            else {
                posXAuto1 = -100;                
                if (contadorAuto1 > 370)                
                    animacionAuto1 = true;    
             
            }

            contadorAuto2++;
            if (contadorAuto2 > 500)
                animacionAuto2 = false;

            //Animacion Auto2
            if (animacionAuto2)
                modelAuto2 = modelAuto2 * Matrix4.CreateTranslation(0f, 0f, 0.15f);          
                      

            glControl3.Invalidate();
        }

        private void reinicioAuto1()
        {
            //Reinicio Posicion Auto1
            Matrix4 scale = Matrix4.CreateScale(2f);
            modelAuto1 = Matrix4.CreateTranslation(25f, -1.0f, 0f);
            modelAuto1 = modelAuto1 * scale;
            myCameras[4].setPosition(new Vector3(45f, 3.0f, 2f)); //reinicio camara desde el auto

            //Reinicio Posicion de Luces del Auto
            allLight[9].Position = new Vector4(42.0f, 0.20f, 1.00f, 1.0f);
            allLight[10].Position = new Vector4(42.0f, 0.20f, 3.80f, 1.0f);
            
            //Reinicio contadores Auto1
            contadorAuto1 = 0;
            animacionAuto1 = true;
            posXAuto1 = 15;

        }

        private void reinicioAuto2()
        {
            //Reinicio Posicion Auto2  
            Matrix4 scale = Matrix4.CreateScale(2f);
            modelAuto2 = Matrix4.CreateRotationY(DEG2RAD * 90f);
            modelAuto2 = modelAuto2 * Matrix4.CreateTranslation(-4.5f, -1.0f, -20f);
            modelAuto2 = modelAuto2 * scale;           

            //Reinicio contadores Auto1
            contadorAuto2 = 0;
            animacionAuto2 = true;           

        }

        #endregion

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
                    myCamera = new CamaraSimple(new Vector3(2.0f, 99.0f, 0), myCamera.aspect);
                    break;
                case 'r':
                    reinicioAuto1();
                    reinicioAuto2();
                    break;

                case 'q':
                    indexCamaras = (indexCamaras + 1) % (myCameras.Length - 1);
                    myCameras[indexCamaras].aspect = myCamera.aspect;
                    myCamera = myCameras[indexCamaras];

                    break;
                case 'f':

                    myCameras[myCameras.Length - 1].aspect = myCamera.aspect;
                    myCamera = myCameras[myCameras.Length - 1];

                    break;
                case 'p':
                    //Pauso la animacion
                    timer1.Enabled = !timer1.Enabled;
                    
                    break;

            }
      glControl3.Invalidate();
    }
  }
        }   
            
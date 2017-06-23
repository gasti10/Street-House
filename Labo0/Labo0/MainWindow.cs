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

        //Aca se almacenan las texturas
        private Dictionary<string, int> programTextures;

        private SphericalCamera myCamera;  //Camara
        private Rectangle viewport; //Viewport a utilizar (Porcion del glControl en donde voy a dibujar).
        private ObjetoGrafico miPiram;
        private Light myLight;
        private Light[] allLight;
        #endregion

        private void glControl3_Load(object sender, EventArgs e)
        {            
            logContextInfo(); //Mostramos info de contexto.
            SetupShaders("vPhong.glsl","fPhong.glsl",out sProgram); //Creamos los shaders y el programa de shader

            //Creo el contenedor de texturas
            programTextures = new Dictionary<string, int>();

            SetupTextures();
            miPiram = new ObjetoGrafico("CGUNS/ModelosOBJ/banco1.obj");
            //miPiram.AddTextureToAllMeshes(GetTextureID("prueba1"));
            //miPiram.Build(sProgram);
            //miPiram.setMaterial(Material.Gold);
            
            foreach (Mesh m in miPiram.Meshes)
            {
                Char[] separator = { '.' };
                string prefijo = m.Name.Split(separator)[0];
                switch (prefijo)
                {
                    case "Gun_Object_1":
                        m.AddTexture(GetTextureID("prueba1"));
                        m.material = Material.Gold;
                        m.Build(sProgram);
                        break;
                    default://case "Wheels_Right_Object_4":
                        m.AddTexture(GetTextureID("prueba2"));
                        m.material = Material.CyanPlastic;
                        m.Build(sProgram);
                        break;
                }
            }
            
            allLight = new Light[1];
            myLight = new Light();
            myLight.Position = new Vector4(0.0f, 1.0f, 0.0f,1.0f);//spot desde arriba
            myLight.Iambient = new Vector3(0.8f, 0.65f, 0.2f);
            myLight.Idiffuse = new Vector3(0.38f, 0.15f, 0.72f);
            myLight.Ispecular = new Vector3(0.8f, 0.8f, 0.8f);
            myLight.ConeAngle = 10.0f;
            myLight.ConeDirection = new Vector3(0.0f, -1.0f, 0.0f);
            myLight.Enabled = 1;
            allLight[0] = myLight;

            
            myCamera = new SphericalCamera(); //Creo una camara.
      
            gl.Enable(EnableCap.DepthTest);

            gl.ClearColor(Color.FloralWhite); //Configuro el Color de borrado.

             gl.PolygonMode(MaterialFace.Front, PolygonMode.Fill);
   
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

            
        }

        private void SetupTextures()
        {
            CargarTextura("files/Texturas/modern1.jpg", "prueba1");
            CargarTextura("files/Texturas/modern3.jpg", "prueba2");
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
           // Matrix4.CreateTranslation(0,-1.0f,0,out modelMatrix);
            sProgram.SetUniformValue("modelMatrix", modelMatrix);
    
            miPiram.Dibujar(sProgram);
            
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
            
            sProgram.SetUniformValue("cameraPosition", myCamera.getPosition());
            sProgram.Deactivate(); //Desactivamos el programa de shader.

            glControl3.SwapBuffers(); //Intercambiamos buffers frontal y trasero, para evitar flickering.
        }

        private void glControl3_Resize(object sender, EventArgs e)
        {   //Actualizamos el viewport para que dibuje en el centro de la pantalla.
            Size size = glControl3.Size;
            if (size.Width < size.Height)
            {
                viewport.X = 0;
                viewport.Y = (size.Height - size.Width) / 2;
                viewport.Width = size.Width;
                viewport.Height = size.Width;
            }
            else
            {
                viewport.X = (size.Width - size.Height) / 2;
                viewport.Y = 0;
                viewport.Width = size.Height;
                viewport.Height = size.Height;
            }
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
        case 't':
          myLight.ConeAngle += 10.0f;
          break;
       case 'y':
          myLight.ConeAngle -= 10.0f;
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
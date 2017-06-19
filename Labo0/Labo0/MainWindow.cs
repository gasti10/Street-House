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
using CGUNS.Primitives;
using System.Drawing.Imaging;


namespace Labo0
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private ShaderProgram sProgram; //Nuestro programa de shaders.
        
        private Piramide miPiram;//Nuestro objeto a dibujar.
        private int tex1;

 
        private QuatCamera myCamera;  //Camara
        private Rectangle viewport; //Viewport a utilizar (Porcion del glControl en donde voy a dibujar).

        private void glControl3_Load(object sender, EventArgs e)
        {            
            logContextInfo(); //Mostramos info de contexto.
            SetupShaders(); //Creamos los shaders y el programa de shader
 
            miPiram = new Piramide();
            miPiram.Build(sProgram);
 

            myCamera = new QuatCamera(); //Creo una camara.
      
            gl.ClearColor(Color.LightGray); //Configuro el Color de borrado.

            gl.Enable(EnableCap.DepthTest);

            gl.ClearColor(Color.WhiteSmoke); //Configuro el Color de borrado.

             gl.PolygonMode(MaterialFace.Front, PolygonMode.Fill);
   
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

            tex1 = CargarTextura("files/Texturas/maggie.png");
    
        }

        private int CargarTextura(String imagenTex)
        {
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
            return texId;

        }
        private void glControl3_Paint(object sender, PaintEventArgs e)
        {
            Matrix4 modelMatrix = Matrix4.Identity; //Por ahora usamos la identidad.
            Matrix4 viewMatrix = myCamera.getViewMatrix();
            Matrix4 projMatrix = myCamera.getProjectionMatrix();
            Matrix4 mvMatrix = Matrix4.Mult(viewMatrix, modelMatrix);
            Vector4 figColor = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);

            gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit); //Borramos el contenido del glControl.

            gl.Viewport(viewport); //Especificamos en que parte del glControl queremos dibujar.
            sProgram.Activate(); //Activamos el programa de shaders
            //Seteamos los valores uniformes.
               sProgram.SetUniformValue("projMat", projMatrix);
      //      sProgram.SetUniformValue("mvMat", mvMatrix);
            //Dibujamos el objeto.
            Matrix4 bajar = Matrix4.Mult(Matrix4.CreateTranslation(0.0f, -0.5f, 0.0f), mvMatrix);
            sProgram.SetUniformValue("mvMat", bajar);
    
            miPiram.Dibujar(sProgram);
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

        private void SetupShaders()
        {
            //Lo hago con mis clases, que encapsulan el manejo de shaders.
            //1. Creamos los shaders, a partir de archivos.
            String vShaderFile = "files/shaders/vTextu.glsl";
            String fShaderFile = "files/shaders/fTextu.glsl";
            Shader vShader = new Shader(ShaderType.VertexShader, vShaderFile);
            Shader fShader = new Shader(ShaderType.FragmentShader, fShaderFile);
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
          myCamera.Acercar(0.25f);
          break;
        case 'k':
          myCamera.Alejar(0.25f);
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
      }
      glControl3.Invalidate();
    }
  }
        }


    
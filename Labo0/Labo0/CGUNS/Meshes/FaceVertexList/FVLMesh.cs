using System;
using System.Collections.Generic;
using OpenTK;
using System.Text;
using CGUNS.Shaders;
using OpenTK.Graphics.OpenGL;
using gl = OpenTK.Graphics.OpenGL.GL;


namespace CGUNS.Meshes.FaceVertexList {
  public class FVLMesh {
    private List<FVLFace> faceList;
    private List<Vector3> vertexList;
    private List<Vector2> texCordList;
    private List<Vector3> vertexNormalList;

    //private int[] Indices;  //Los indices para formar las caras.

    public FVLMesh() {
      faceList = new List<FVLFace>();
      vertexList = new List<Vector3>();
      vertexNormalList = new List<Vector3>();
      texCordList = new List<Vector2>();

    }

    public List<Vector3> VertexList {
      get { return vertexList; }
    }

    public List<FVLFace> FaceList {
      get { return faceList; }
    }

    public List<Vector3> VertexNormalList
    {
        get { return vertexNormalList; }
    }

    public List<Vector2> TexCordList
    {
        get { return texCordList; }
    }

    public int VertexCount {
      get { return vertexList.Count; }
    }

    public int FaceCount {
      get { return faceList.Count; }
    }

    public int AddVertex(Vector3 vertex) {      
      vertexList.Add(vertex);
      return vertexList.Count - 1;
    }

    public int AddVertexNormal(Vector3 normal)
    {
        vertexNormalList.Add(normal);
        return vertexNormalList.Count - 1;
    }

    public int AddTexCord(Vector2 texCord)
    {
        texCordList.Add(texCord);
        return texCordList.Count - 1;
    }

    public int AddFace(FVLFace face) {
      faceList.Add(face);
      return faceList.Count - 1;
    }

    public void PrintLists() {
      String sender = "FVLMesh.printLists: ";
      FVLFace face;
      List<int> faceVertexes;
      log(sender, "Vertex List has {0} items.", vertexList.Count);
      for (int i = 0; i < vertexList.Count; i++) {
        log("", "V[{0}] = ({1}, {2}, {3})", i, vertexList[i].X, vertexList[i].Y, vertexList[i].Z);
      }
      int cantFaces = faceList.Count;
      log(sender, "Face List has {0} items.", cantFaces);
      for (int i = 0; i < cantFaces; i++) {
        face = faceList[i];
        faceVertexes = face.VertexIndexes;
        String format = "F[{0}] = ";
        for (int j = 0; j < faceList[i].VertexCount; j++) {
          format = format + " V[" + faceVertexes[j] + "],";
        }
        log("", format, i);
      }
      log(sender, "End!");
    }

    private void log(String sender, String format, params Object[] args) {
      Console.Out.WriteLine(sender + format, args);
    }

    public void Build(ShaderProgram sProgram)
    {
        CrearVBOs();
        CrearVAO(sProgram);
    }
    private int h_VBO; //Handle del Vertex Buffer Object (posiciones de los vertices)
    private int h_EBO; //Handle del Elements Buffer Object (indices)
    private int h_VAO; //Handle del Vertex Array Object (Configuracion de los dos anteriores)

      //las tres líneas que siguen son para el dibujado de normales
    private int n_VBO; //Handle del Vertex Buffer Object (normales de los vertices)
  //  private int n_EBO; //Handle del Elements Buffer Object (normIndices)
 //   private int n_VAO; //Handle del Vertex Array Object (Configuracion de los dos anteriores)
   
    private void CrearVBOs()
    {
        BufferTarget bufferType; //Tipo de buffer (Array: datos, Element: indices)
        IntPtr size;             //Tamanio (EN BYTES!) del buffer.
        //Hint para que OpenGl almacene el buffer en el lugar mas adecuado.
        //Por ahora, usamos siempre StaticDraw (buffer solo para dibujado, que no se modificara)
        BufferUsageHint hint = BufferUsageHint.StaticDraw;

        //VBO con el atributo "posicion" de los vertices.
        bufferType = BufferTarget.ArrayBuffer;
             

        h_VBO = gl.GenBuffer();  //Le pido un Id de buffer a OpenGL
        gl.BindBuffer(bufferType, h_VBO); //Lo selecciono como buffer de Datos actual.


        int cantCaras = faceList.Count;
        int[] Indices;
        Vector3[] posiciones;
        Vector3[] coorTex ;
        Vector3[] normales ;

// los parametros booleanos indican si se cargan coord. Text y normales respectivamente
        OrdenarDatos(false, true, out posiciones, out coorTex, out normales, out Indices);//prepara los arreglos en el orden que los necesita el VBO/EBO

        size = new IntPtr(posiciones.Length * Vector3.SizeInBytes);
        gl.BufferData<Vector3>(bufferType, size, posiciones, hint); //Lo lleno con la info.
        gl.BindBuffer(bufferType, 0); // Lo deselecciono (0: ninguno)

        //VBO con otros atributos de los vertices (color, normal, textura, etc).
        bufferType = BufferTarget.ArrayBuffer;
        n_VBO = gl.GenBuffer();  //Le pido un Id de buffer a OpenGL
        gl.BindBuffer(bufferType, n_VBO); //Lo selecciono como buffer de Datos actual.

        size = new IntPtr(normales.Length * Vector3.SizeInBytes);
        gl.BufferData<Vector3>(bufferType, size, normales, hint); //Lo lleno con la info.
        gl.BindBuffer(bufferType, 0); // Lo deselecciono (0: ninguno)
        //Se pueden hacer en distintos VBOs o en el mismo.

        //EBO, buffer con los indices.
        bufferType = BufferTarget.ElementArrayBuffer;

        h_EBO = gl.GenBuffer();

         size = new IntPtr(Indices.Length * sizeof(int));
        gl.BindBuffer(bufferType, h_EBO); //Lo selecciono como buffer de elementos actual.
        gl.BufferData<int>(bufferType, size, Indices, hint);
        gl.BindBuffer(bufferType, 0);

        //Y ahora todo lo necesario para dibujado de normales

    }


    void OrdenarDatos(bool siText, bool siNormal, out Vector3[] posiciones, out Vector3[] coorTex, out Vector3[] normales, out int[] Indices )
    {
        int k =0;
        int cantCaras = faceList.Count;
        Indices = new int[cantCaras * 3]; //OJO solo si  TODAS las caras son triágulos
        posiciones = new Vector3[cantCaras *3];
        coorTex = new Vector3[cantCaras * 3];
        normales = new Vector3[cantCaras * 3];

        for (int i = 0; i < cantCaras; i++)
        {
            FVLFace Cara = faceList[i];
            int[] indPosF = Cara.VertexIndexes.ToArray();
            int[] indTexF = Cara.TexCordIndexes.ToArray();
            int[] indNormF = Cara.NormalIndexes.ToArray();

            for (int j = 0; j < 3; j++)
            {
               posiciones[k] = VertexList[indPosF[j]];

  //              if (siText)
 //                 coorTex[k] = cordTexList[indTexF[j]];

                if (siNormal)
                normales[k] = VertexNormalList[indNormF[j]];

                Indices[k] = k;
                k = k + 1;
            }
            
        }
    
    }

    private void CrearVAO(ShaderProgram sProgram)
    {
        // Indice del atributo a utilizar. Este indice se puede obtener de tres maneras:
        // Supongamos que en nuestro shader tenemos un atributo: "in vec3 vPos";
        // 1. Dejar que OpenGL le asigne un indice cualquiera al atributo, y para consultarlo hacemos:
        //    attribIndex = gl.GetAttribLocation(programHandle, "vPos") DESPUES de haberlo linkeado.
        // 2. Nosotros le decimos que indice queremos que le asigne, utilizando:
        //    gl.BindAttribLocation(programHandle, desiredIndex, "vPos"); ANTES de linkearlo.
        // 3. Nosotros de decimos al preprocesador de shader que indice queremos que le asigne, utilizando
        //    layout(location = xx) in vec3 vPos;
        //    En el CODIGO FUENTE del shader (Solo para #version 330 o superior)      
        int attribIndex;
        int cantComponentes; //Cantidad de componentes de CADA dato.
        VertexAttribPointerType attribType; // Tipo de CADA una de las componentes del dato.
        int stride; //Cantidad de BYTES que hay que saltar para llegar al proximo dato. (0: Tightly Packed, uno a continuacion del otro)
        int offset; //Offset en BYTES del primer dato.
        BufferTarget bufferType; //Tipo de buffer.

        // 1. Creamos el VAO
        h_VAO = gl.GenVertexArray(); //Pedimos un identificador de VAO a OpenGL.
        gl.BindVertexArray(h_VAO);   //Lo seleccionamos para trabajar/configurar.

        //2. Configuramos el VBO de posiciones.
        attribIndex = sProgram.GetVertexAttribLocation("vPos"); //Yo lo saco de mi clase ProgramShader.
        cantComponentes = 3;   // 3 componentes (x, y, z)
        attribType = VertexAttribPointerType.Float; //Cada componente es un Float.
        stride = 0;  //Los datos estan uno a continuacion del otro.
        offset = 0;  //El primer dato esta al comienzo. (no hay offset).
        bufferType = BufferTarget.ArrayBuffer; //Buffer de Datos.

        gl.EnableVertexAttribArray(attribIndex); //Habilitamos el indice de atributo.
        gl.BindBuffer(bufferType, h_VBO); //Seleccionamos el buffer a utilizar.
        gl.VertexAttribPointer(attribIndex, cantComponentes, attribType, false, stride, offset);//Configuramos el layout (como estan organizados) los datos en el buffer.

        // 2.a.El bloque anterior se repite para cada atributo del vertice (color, normal, textura..)
        attribIndex = sProgram.GetVertexAttribLocation("vNormal"); //Yo lo saco de mi clase ProgramShader.
        cantComponentes = 3;   // 3 componentes (x, y, z)
        attribType = VertexAttribPointerType.Float; //Cada componente es un Float.
        stride = 0;  //Los datos estan uno a continuacion del otro.
        offset = 0;  //El primer dato esta al comienzo. (no hay offset).
        bufferType = BufferTarget.ArrayBuffer; //Buffer de Datos.

        gl.EnableVertexAttribArray(attribIndex); //Habilitamos el indice de atributo.
        gl.BindBuffer(bufferType, n_VBO); //Seleccionamos el buffer a utilizar.
        gl.VertexAttribPointer(attribIndex, cantComponentes, attribType, false, stride, offset);//Configuramos el layout (como estan organizados) los datos en el buffer.


        // 3. Configuramos el EBO a utilizar. (como son indices, no necesitan info de layout)
        bufferType = BufferTarget.ElementArrayBuffer;
        gl.BindBuffer(bufferType, h_EBO);

        // 4. Deseleccionamos el VAO.
        gl.BindVertexArray(0);
    }



    public void Dibujar(ShaderProgram sProgram)
    {

        PrimitiveType primitive; //Tipo de Primitiva a utilizar (triangulos, strip, fan, quads, ..)
        int offset; // A partir de cual indice dibujamos?
        int count;  // Cuantos?
        DrawElementsType indexType; //Tipo de los indices.

        primitive = PrimitiveType.Triangles;  //Usamos trianglos.
        offset = 0;  // A partir del primer indice.
 
        count = faceList.Count * 3; // Todos los indices.
        indexType = DrawElementsType.UnsignedInt; //Los indices son enteros sin signo.

        gl.BindVertexArray(h_VAO); //Seleccionamos el VAO a utilizar.
        gl.DrawElements(primitive, count, indexType, offset); //Dibujamos utilizando los indices del VAO.
        gl.BindVertexArray(0); //Deseleccionamos el VAO
    }

  }
}

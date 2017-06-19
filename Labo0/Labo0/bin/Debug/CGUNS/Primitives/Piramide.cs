using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using gl = OpenTK.Graphics.OpenGL.GL;
using CGUNS.Shaders;

namespace CGUNS.Primitives
{
    class Piramide
    {
        private Vector3[] vPos; //Las posiciones de los vertices.
        private Vector2[] TexCoord;// las coordenadas de textura de los vertices.
        private uint[] indices;  //Los indices para formar las caras.

        public Piramide(float s = 0.3f)
        {
            vPos = new Vector3[18];
            //adelante
            vPos[0] = new Vector3(s, 0.0f, s);
            vPos[1] = new Vector3(0.0f, 4 * s, 0.0f);
            vPos[2] = new Vector3(-s, 0.0f, s);
            //izquierda
            vPos[3] = new Vector3(-s, 0.0f, s);
            vPos[4] = new Vector3(0.0f, 4 * s, 0.0f);
            vPos[5] = new Vector3(-s, 0.0f, -s);
            //atras
            vPos[6] = new Vector3(-s, 0.0f, -s);
            vPos[7] = new Vector3(0.0f, 4 * s, 0.0f);
            vPos[8] = new Vector3(s, 0.0f, -s);
            //derecha
            vPos[9] = new Vector3(s, 0.0f, -s);
            vPos[10] = new Vector3(0.0f, 4 * s, 0.0f);
            vPos[11] = new Vector3(s, 0.0f, s);
            //abajo
            vPos[12] = new Vector3(s, 0.0f, s);
            vPos[13] = new Vector3(-s, 0.0f, -s);
            vPos[14] = new Vector3(s, 0.0f, -s);

            vPos[15] = new Vector3(s, 0.0f, s);
            vPos[16] = new Vector3(-s, 0.0f, s);
            vPos[17] = new Vector3(-s, 0.0f, -s);

            TexCoord = new Vector2[18];
            //adelante
            TexCoord[0] = new Vector2(0.75f, 0.0f);
            TexCoord[1] = new Vector2(0.5f, 1.0f);
            TexCoord[2] = new Vector2(0.25f, 0.0f);
           //izquierda
            TexCoord[3] = new Vector2(0.75f, 0.0f);
            TexCoord[4] = new Vector2(0.5f, 1.0f);
            TexCoord[5] = new Vector2(0.25f, 0.0f);
            //atras
            TexCoord[6] = new Vector2(0.75f, 0.0f);
            TexCoord[7] = new Vector2(0.5f, 1.0f);
            TexCoord[8] = new Vector2(0.25f, 0.0f);
            //derecha
            TexCoord[9] = new Vector2(0.75f, 0.0f);
            TexCoord[10] = new Vector2(0.5f, 1.0f);
            TexCoord[11] = new Vector2(0.25f, 0.0f);
            //abajo
            TexCoord[12] = new Vector2(0.25f, 0.0f);
            TexCoord[13] = new Vector2(0.75f, 1.0f);
            TexCoord[14] = new Vector2(0.25f, 1.0f);

            TexCoord[15] = new Vector2(0.25f, 0.0f);
            TexCoord[16] = new Vector2(0.75f, 0.0f);
            TexCoord[17] = new Vector2(0.75f, 1.0f);

            indices = new uint[]{
                0, 1, 2,
                3, 4, 5,
                6, 7, 8,
                9, 10, 11,
                12, 13, 14,
                15, 16, 17
             };

        }
        // Construye los Buffers correspondientes de OpenGL para dibujar este objeto.
        public void Build(ShaderProgram sProgram)
        {
            CrearVBOs();
            CrearVAO(sProgram);
        }
        /// Dibuja el contenido de los Buffers de este objeto.
        /// </summary>
        /// <param name="sProgram"></param>
        public void Dibujar(ShaderProgram sProgram)
        {
            PrimitiveType primitive; //Tipo de Primitiva a utilizar (triangulos, strip, fan, quads, ..)
            int offset; // A partir de cual indice dibujamos?
            int count;  // Cuantos?
            DrawElementsType indexType; //Tipo de los indices.

            primitive = PrimitiveType.Triangles;  //Usamos trianglos.
            offset = 0;  // A partir del primer indice.
            count = indices.Length; // Todos los indices.
            indexType = DrawElementsType.UnsignedInt; //Los indices son enteros sin signo.

            gl.BindVertexArray(h_VAO); //Seleccionamos el VAO a utilizar.
            gl.DrawElements(primitive, count, indexType, offset); //Dibujamos utilizando los indices del VAO.
        //    gl.DrawArrays(primitive, offset, count);
            gl.BindVertexArray(0); //Deseleccionamos el VAO
        }

        private int h_VBO; //Handle del Vertex Buffer Object (posiciones de los vertices)
        private int t_VBO; //Handle del Vertex Buffer Object (color de los vertices)
        private int h_EBO; //Handle del Elements Buffer Object (indices)
        private int h_VAO; //Handle del Vertex Array Object (Configuracion de los dos anteriores)

        private void CrearVBOs()
        {
            BufferTarget bufferType; //Tipo de buffer (Array: datos, Element: indices)
            IntPtr size;             //Tamanio (EN BYTES!) del buffer.
            //Hint para que OpenGl almacene el buffer en el lugar mas adecuado.
            ///Por ahora, usamos siempre StaticDraw (buffer solo para dibujado, que no se modificara)
            BufferUsageHint hint = BufferUsageHint.StaticDraw;

            //VBO con el atributo "posicion" de los vertices.
            bufferType = BufferTarget.ArrayBuffer;
            size = new IntPtr(vPos.Length * Vector3.SizeInBytes);
            h_VBO = gl.GenBuffer();  //Le pido un Id de buffer a OpenGL
            gl.BindBuffer(bufferType, h_VBO); //Lo selecciono como buffer de Datos actual.
            gl.BufferData<Vector3>(bufferType, size, vPos, hint); //Lo lleno con la info.
            gl.BindBuffer(bufferType, 0); // Lo deselecciono (0: ninguno)

            //VBO con el atributo "coorTex" de los vertices.
            bufferType = BufferTarget.ArrayBuffer;
            size = new IntPtr(TexCoord.Length * Vector2.SizeInBytes);
            t_VBO = gl.GenBuffer();  //Le pido un Id de buffer a OpenGL
            gl.BindBuffer(bufferType, t_VBO); //Lo selecciono como buffer de Datos actual.
            gl.BufferData<Vector2>(bufferType, size, TexCoord, hint); //Lo lleno con la info.
            gl.BindBuffer(bufferType, 0); // Lo deselecciono (0: ninguno)

            //VBO con otros atributos de los vertices (color, normal, textura, etc).
            //Se pueden hacer en distintos VBOs o en el mismo.

            //EBO, buffer con los indices.
            bufferType = BufferTarget.ElementArrayBuffer;
            size = new IntPtr(indices.Length * sizeof(int));
            h_EBO = gl.GenBuffer();
            gl.BindBuffer(bufferType, h_EBO); //Lo selecciono como buffer de elementos actual.
            gl.BufferData<uint>(bufferType, size, indices, hint);
            gl.BindBuffer(bufferType, 0);
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
          //  attribIndex = 0;
            cantComponentes = 3;   // 3 componentes (x, y, z)
            attribType = VertexAttribPointerType.Float; //Cada componente es un Float.
            stride = 0;  //Los datos estan uno a continuacion del otro.
            offset = 0;  //El primer dato esta al comienzo. (no hay offset).
            bufferType = BufferTarget.ArrayBuffer; //Buffer de Datos.

            gl.EnableVertexAttribArray(attribIndex); //Habilitamos el indice de atributo.
            gl.BindBuffer(bufferType, h_VBO); //Seleccionamos el buffer a utilizar.
            gl.VertexAttribPointer(attribIndex, cantComponentes, attribType, false, stride, offset);//Configuramos el layout (como estan organizados) los datos en el buffer.

            //2a. Configuramos el VBO de ccord. de Textura.
            attribIndex = sProgram.GetVertexAttribLocation("TexCoord"); //Yo lo saco de mi clase ProgramShader.
           // attribIndex = 1;
            cantComponentes = 2;   // 3 componentes (s, t)
            attribType = VertexAttribPointerType.Float; //Cada componente es un Float.
            stride = 0;  //Los datos estan uno a continuacion del otro.
            offset = 0;  //El primer dato esta al comienzo. (no hay offset).
            bufferType = BufferTarget.ArrayBuffer; //Buffer de Datos.

            gl.EnableVertexAttribArray(attribIndex); //Habilitamos el indice de atributo.
            gl.BindBuffer(bufferType, t_VBO); //Seleccionamos el buffer a utilizar.
            gl.VertexAttribPointer(attribIndex, cantComponentes, attribType, false, stride, offset);//Configuramos el layout (como estan organizados) los datos en el buffer.

            // 3. Configuramos el EBO a utilizar. (como son indices, no necesitan info de layout)
            bufferType = BufferTarget.ElementArrayBuffer;
            gl.BindBuffer(bufferType, h_EBO);

            // 4. Deseleccionamos el VAO.
            gl.BindVertexArray(0);
        }
    }
}
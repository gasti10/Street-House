using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace CGUNS.Cameras
{

    /// <summary>
    /// Representa una Camara fija. En el constructor se le indica la posicion y target y luego no pueden modificarse
    /// </summary>
    class CamaraSimple : Camera
    {
        
        //Valores necesarios para calcular la Matriz de Vista.
        private Vector3 eye = new Vector3(0.0f, 0.0f, 0.0f);
        private Vector3 target = new Vector3(0, 0, 0);
        private Vector3 up = Vector3.UnitY;


        
        /// <summary>
        /// Constructor con la posicion. Target se asume en el origen y up como el versor en Y
        /// </summary>
        /// <returns></returns>
        public CamaraSimple(Vector3 posicion,float asp)
        {
            aspect = asp;
            CrearProjMatrix(aspect);
            eye = posicion;

        }

        /// <summary>
        /// Constructor con la posicion y target. El vector up se asume como el versor en Y
        /// </summary>
        /// <returns></returns>
        public CamaraSimple(Vector3 posicion, Vector3 objetivo,float asp)
        {
            aspect = asp;
            CrearProjMatrix(aspect);
            eye = posicion;
            target = objetivo;

        }

        
        /// <summary>
        /// Constructor con la posicion y target. El vector up se asume como el versor en Y
        /// </summary>
        /// <returns></returns>
        public CamaraSimple(Vector3 posicion, Vector3 objetivo, Vector3 Up,float asp)
        {
            aspect = asp;
            CrearProjMatrix(aspect);
            eye = posicion;
            target = objetivo;
            up = Up;

        }

        private void CrearProjMatrix(float a) {

            float fovy = 50 * DEG2RAD; //50 grados de angulo.
            float aspectRadio = a; //Cuadrado
            float zNear = 0.1f; //Plano Near
            float zFar = 100f;  //Plano Far
            projMatrix = Matrix4.CreatePerspectiveFieldOfView(fovy, aspectRadio, zNear, zFar);

        }

        /// <summary>
        /// Retorna la Matriz de Projeccion que esta utilizando esta camara.
        /// </summary>
        /// <returns></returns>
        public override Matrix4 getProjectionMatrix()
        {
            return projMatrix;
        }
        /// <summary>
        /// Retorna la Matriz de Vista que representa esta camara.
        /// </summary>
        /// <returns></returns>
        public Matrix4 ViewMatrix()
        {
            //Construimos la matriz y la devolvemos.
            return Matrix4.LookAt(eye, target, up);
        }

        public override Matrix4 getViewMatrix()
        {
            //Construimos la matriz y la devolvemos.
            return Matrix4.LookAt(eye, target, up);
        }

        /// <summary>
        /// Retorna la Posicion de la camara.
        /// </summary>
        /// <returns></returns>
        public override Vector3 getPosition()
        {
            return eye;
        }

        public override void Arriba()
        {
            eye.X += 2.0f;
        }

        public override void Abajo()
        {
            eye.X -= 2.0f;
        }

        public override void Izquierda()
        {
           eye.Z-=2.0f;
        }

        public override void Derecha()
        {
            eye.Z+=2.0f;
        }

        public override void Acercar(float v)
        {
            eye.Y -= v;
        }

        public override void Alejar(float v)
        {
            eye.Y += v;
        }
        public override void setPosition(Vector3 Pos)
        {
            eye = Pos;
        }
    }
}

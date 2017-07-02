using System;
using OpenTK;

namespace CGUNS.Cameras
{
    public abstract class Camera
    {
        protected const float DEG2RAD = (float)(Math.PI / 180.0); //Para pasar de grados a radianes
        protected Matrix4 projMatrix; //Matriz de Proyeccion.

        public abstract Vector3 getPosition();

        public abstract Matrix4 getProjectionMatrix();
        
        public abstract Matrix4 getViewMatrix();

        protected float aspecto;

        public float aspect
        {
            get
            {
                return aspecto;
            }

            set
            {
                aspecto = value;
                float fovy = 50 * DEG2RAD; //50 grados de angulo.
                float zNear = 0.1f; //Plano Near
                float zFar = 250f;  //Plano Far
                projMatrix = Matrix4.CreatePerspectiveFieldOfView(fovy, aspecto, zNear, zFar);
            }
        }


        public abstract void Acercar(float v);

        public abstract void Alejar(float v);

        public abstract void Arriba();

        public abstract void Abajo();

        public abstract void Izquierda();

        public abstract void Derecha();

        public abstract void setPosition(Vector3 Pos);
    }
}

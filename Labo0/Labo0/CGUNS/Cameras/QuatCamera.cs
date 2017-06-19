using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;

namespace CGUNS.Cameras
{
    /// <summary>
    /// Representa una Camara en coordenadas esfericas.
    /// La camara apunta y orbita alrededor del origen de coordenadas (0,0,0).
    /// El vector "up" de la camara es esl eje "Y" (0,1,0).
    /// La posicion de la camara esta dada por 3 valores: Radio, Theta, Phi.
    /// </summary>
    class QuatCamera
    {
        private const float DEG2RAD = (float)(Math.PI / 180.0); //Para pasar de grados a radianes

        private Matrix4 projMatrix; //Matriz de Proyeccion.

        private float radius; //Distancia al origen.
 
        private Quaternion cameraRot;
        private Vector3 cameraPos;

        public QuatCamera()
        {
            //Por ahora la matriz de proyeccion queda fija. :)
            float fovy = 50 * DEG2RAD; //50 grados de angulo.
            float aspectRadio = 1; //Cuadrado
            float zNear = 0.1f; //Plano Near
            float zFar = 100f;  //Plano Far
            projMatrix = Matrix4.CreatePerspectiveFieldOfView(fovy, aspectRadio, zNear, zFar);

            //Posicion inicial de la camara.
            radius = 2.5f;
 
            cameraRot = Quaternion.FromAxisAngle(new Vector3(0, 0, -1), 0);
        }

        /// <summary>
        /// Retorna la Matriz de Projeccion que esta utilizando esta camara.
        /// </summary>
        /// <returns></returns>
        public Matrix4 getProjectionMatrix()
        {
            return projMatrix;
        }
        /// <summary>
        /// Retorna la Matriz de Vista que representa esta camara.
        /// </summary>
        /// <returns></returns>
        public Matrix4 getViewMatrix()
        {   
            //Construimos la matriz y la devolvemos.
            cameraPos = new Vector3(0, 0, radius);
            return Matrix4.Mult(Matrix4.CreateFromQuaternion(cameraRot), Matrix4.CreateTranslation(-cameraPos));
        }


        public void Acercar(float distance)
        {
            if ((distance > 0) && (distance < radius))
            {
                radius = radius - distance;
            }
        }

        public void Alejar(float distance)
        {
            if (distance > 0)
            {
                radius = radius + distance;
            }
        }

        private float deltaTheta = 0.5f;
        private float deltaPhi = 0.1f;

        public void Arriba()
        {
            Quaternion tmpQuat = Quaternion.FromAxisAngle(new Vector3(1, 0, 0), -deltaPhi);
            cameraRot = tmpQuat * cameraRot;
            cameraRot.Normalize();
        }

        public void Abajo()
        {
            Quaternion tmpQuat = Quaternion.FromAxisAngle(new Vector3(1, 0, 0), deltaPhi);
            cameraRot = tmpQuat * cameraRot;
            cameraRot.Normalize();
        }

        public void Izquierda()
        {
            Quaternion tmpQuat = Quaternion.FromAxisAngle(new Vector3(0, 1, 0), deltaTheta);
            cameraRot = tmpQuat * cameraRot;
            cameraRot.Normalize();
        }

        public void Derecha()
        {
            Quaternion tmpQuat = Quaternion.FromAxisAngle(new Vector3(0, 1, 0), -deltaTheta);
            cameraRot = tmpQuat * cameraRot;
            cameraRot.Normalize();
        }


    }
}

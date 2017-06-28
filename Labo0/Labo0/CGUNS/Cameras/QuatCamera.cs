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
    class QuatCamera: Camera
    {
        private float radius; //Distancia al origen.
 
        private Quaternion cameraRot;
        private Vector3 cameraPos;

        public QuatCamera(float aspecto)
        {
            //Por ahora la matriz de proyeccion queda fija. :)
            float fovy = 50 * DEG2RAD; //50 grados de angulo.
            float zNear = 0.1f; //Plano Near
            float zFar = 250f;  //Plano Far
            if (aspecto == 0) aspecto = 1;
            aspect = aspecto;
            projMatrix = Matrix4.CreatePerspectiveFieldOfView(fovy, aspecto, zNear, zFar);

            //Posicion inicial de la camara.
            radius = 5.0f;
 
            cameraRot = //Quaternion.FromAxisAngle(new Vector3(1, 0, 0), MathHelper.DegreesToRadians(180.0f)) *
                Quaternion.FromAxisAngle(new Vector3(0, 1, 0), MathHelper.DegreesToRadians(-90.0f)) *
                Quaternion.FromAxisAngle(new Vector3(0, 0, -1), 0);
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
        public override Matrix4 getViewMatrix()
        {   
            //Construimos la matriz y la devolvemos.
            cameraPos = new Vector3(0, 0, radius);
            return Matrix4.Mult(Matrix4.CreateFromQuaternion(cameraRot), Matrix4.CreateTranslation(-cameraPos));
        }

        public override Vector3 getPosition() {
            //Matriz de Transformacion del Espacio del Ojo al espacio del Mundo
            Matrix4 viewToWorld = ViewMatrix().Inverted();
            //Posicion de la camara en el espacio del ojo (Osea el origen)
            Vector4 eyePos = new Vector4(0, 0, 0, 1);
            //Transformo el origen de la camara en espacio del ojo al espacio del mundo
            return new Vector3(
                  Vector4.Dot(viewToWorld.Column0, eyePos),
                  Vector4.Dot(viewToWorld.Column1, eyePos),
                  Vector4.Dot(viewToWorld.Column2, eyePos)
                );
        }

        /// <summary>
        /// Retorna la Matriz de Vista que representa esta camara.
        /// </summary>
        /// <returns></returns>
        public  Matrix4 ViewMatrix()
        {
            //Construimos la matriz y la devolvemos.
            Matrix4 posicion = Matrix4.CreateTranslation(cameraPos);
            Matrix4 rotacion = Matrix4.CreateFromQuaternion(cameraRot);
            //return Matrix4.LookAt(eye, target, up);
            return Matrix4.Mult(rotacion, posicion);
        }

        public override void Acercar(float distance)
        {
            if ((distance > 0) && (distance < radius))
            {
                radius = radius - distance;
            }
        }

        public override void Alejar(float distance)
        {
            if (distance > 0)
            {
                radius = radius + distance;
            }
        }

        private float deltaTheta = 0.3f;
        private float deltaPhi = 0.1f;
        
        public override void Arriba()
        {
            Quaternion tmpQuat = Quaternion.FromAxisAngle(new Vector3(1, 0, 0), -deltaPhi);
            cameraRot = tmpQuat * cameraRot;
            cameraRot.Normalize();
        }

        public override void Abajo()
        {
            Quaternion tmpQuat = Quaternion.FromAxisAngle(new Vector3(1, 0, 0), deltaPhi);
            cameraRot = tmpQuat * cameraRot;
            cameraRot.Normalize();
        }

        public override void Izquierda()
        {
            Quaternion tmpQuat = Quaternion.FromAxisAngle(new Vector3(0, 1, 0), deltaTheta);
            cameraRot = tmpQuat * cameraRot;
            cameraRot.Normalize();
        }

        public override void Derecha()
        {
            Quaternion tmpQuat = Quaternion.FromAxisAngle(new Vector3(0, 1, 0), -deltaTheta);
            cameraRot = tmpQuat * cameraRot;
            cameraRot.Normalize();
        }


    }
}

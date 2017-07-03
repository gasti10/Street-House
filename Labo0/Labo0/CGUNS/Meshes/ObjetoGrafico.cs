﻿using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using CGUNS.Shaders;
using System.Linq;
using CGUNS.Parsers;

namespace CGUNS.Meshes
{
    public class ObjetoGrafico
    {
        //Submeshes
        List<Mesh> meshes;
        //Materiales
        List<Material> materiales;

        public ObjetoGrafico()
        {
            meshes = new List<Mesh>();
            materiales = new List<Material>();
        }

        public ObjetoGrafico(string path)
        {
            meshes = CGUNS.Parsers.ObjFileParser.parseFile(path).Cast<Mesh>().ToList();
        }

        public List<Mesh> Meshes
        {
            get { return meshes; }
            set { meshes = value; }
        }

        public void AddMesh(Mesh m)
        {
            meshes.Add(m);
        }

        public void RemoveMesh(Mesh m)
        {
            meshes.Remove(m);
        }

        public void AddTextureToAllMeshes(int id)
        {
            foreach (Mesh m in Meshes)
                m.AddTexture(id);
        }

        public void ClearMeshes()
        {
            meshes.Clear();
        }

        public void Dibujar(ShaderProgram sProgram)
        {
            foreach (Mesh m in meshes)
            {
                m.Dibujar(sProgram);
            }
        }
     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sProgram1">Shader para dibujar objeto</param>
        /// <param name="sProgram2">Corresponde al shader de sombras</param>
        public void Build(ShaderProgram sProgram1)//, ShaderProgram sProgram2)
        {
            foreach (Mesh m in meshes)
                m.Build(sProgram1);
                    //m.Build(sProgram1, sProgram2);
        }
        
        public List<Vector3> getMeshVertices(int index){
            List<Vector3> aux = new List<Vector3>();
            int i=0;
            foreach (Mesh m in meshes)
            {
                if (i== index)
                    foreach (Vector3 vertex in m.getVertices())
                    {
                        aux.Add(vertex);
                    }
                i++;
            }
            return aux;
        }

        public List<Vector3> getMeshVertices(String name)
        {
            List<Vector3> aux = new List<Vector3>();
            foreach (Mesh m in meshes)
            {
                if (m.Name==name)
                    foreach (Vector3 vertex in m.getVertices())
                    {
                        aux.Add(vertex);
                    }
            }
            return aux;
        }


        public List<int> getIndicesDeMesh(int index) {
            int i=0;
            foreach (Mesh m in meshes)
            {
                if (i==index)
                    return m.IndicesDeMesh();
                i++;
            }
            return new List<int>();
        }
        public List<int> getIndicesDeMesh(String name)
        {
            foreach (Mesh m in meshes)
            {
                if (m.Name==name)
                    return m.IndicesDeMesh();
            }
            return new List<int>();
        }
    
        public void addMaterial(String name, Material mat)
        {
            foreach (Mesh m in meshes)
            {
                if (m.Name == name)
                {
                    m.material = mat;
                    break;                    
                }
            }
        }

        public void setMaterial(Material mat) {
            foreach (Mesh m in meshes)
                m.material = mat;
        }

        public Material getMaterial(String name) {
            foreach (Mesh m in meshes)
                if (m.Name == name)
                    return m.material;
            return Material.Default;
        }

        public Material getMaterial(int index)
        {
            int i = 0;
            foreach (Mesh m in meshes)
            {
                if (i == index)
                    return m.material;
                i++;
            }
            return Material.Default;
        }
    }
}

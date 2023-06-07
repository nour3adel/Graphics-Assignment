using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;
using GlmNet;
using System.IO;
using System.Diagnostics;
using static System.Windows.Forms.AxHost;

namespace Graphics
{
    class Renderer
    {
        Shader sh;

        uint HouseBufferID;
        uint WallBufferID;
        uint RamadanBufferID;
    
        mat4 modelMatrix; // SCALING
        mat4 ViewMatrix;
        mat4 ProjectionMatrix;

        int ShadertransMatrixID;
        int ShaderViewMatrixID;
        int ShaderProjectionMatrixID;


        public Camera cam;
        public void Initialize()
        {
            string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            sh = new Shader(projectPath + "\\Shaders\\SimpleVertexShader.vertexshader", projectPath + "\\Shaders\\SimpleFragmentShader.fragmentshader");
            Gl.glClearColor(0, 0, 0.2f, 1);

            float[] Houseverts = {

                //House Base
                0.0f,  0.0f, 0.0f, 0.0f, 1.0f, 1.0f,
                0.0f,  0.0f, 40.0f, 0.0f, 1.0f, 1.0f,
                40.0f,  0.0f, 40.0f, 0.0f, 1.0f, 1.0f,
                40.0f,  0.0f, 0.0f, 0.0f, 1.0f, 1.0f,   
                //Back Side
                40.0f,  0.0f, 0.0f, 0.92f, 0.0f, 0.54f,
                0.0f,  0.0f, 0.0f, 0.92f, 0.0f, 0.54f,
                0.0f,  40.0f, 0.0f, 0.92f, 0.0f, 0.54f,
                40.0f,  40.0f, 0.0f, 0.92f, 0.0f, 0.54f,  
                //Left Side
                0.0f,  0.0f, 0.0f, 0.72f, 0.58f, 0.39f,
                0.0f,  0.0f, 40.0f, 0.72f, 0.58f, 0.39f,
                0.0f,  40.0f, 40.0f, 0.72f, 0.58f, 0.39f,
                0.0f,  40.0f, 0.0f, 0.72f, 0.58f, 0.39f,
                 //right side
                40.0f,  40.0f, 0.0f, 0.72f, 0.58f, 0.39f,
                40.0f,  40.0f, 40.0f, 0.72f, 0.58f, 0.39f,
                40.0f,  0.0f, 40.0f, 0.72f, 0.58f, 0.39f,
                40.0f,  0.0f, 0.0f, 0.72f, 0.58f, 0.39f, 
                //Front Side
                40.0f,  0.0f, 40.0f, 1.0f, 0.88f, 0.71f,
                0.0f,  0.0f, 40.0f, 0.72f, 0.58f, 0.39f,
                0.0f,  40.0f, 40.0f, 0.72f, 0.58f, 0.39f,
                40.0f,  40.0f, 40.0f, 1.0f, 0.88f, 0.71f,   
                //Top Base
                0.0f,  40.0f, 0.0f, 1.0f, 0.0f, 0.0f,
                40.0f,  40.0f, 0.0f, 1.0f, 0.0f, 0.0f,
                40.0f,  40.0f, 40.0f, 1.0f, 0.0f, 0.0f,
                0.0f,  40.0f, 40.0f, 1.0f, 0.0f, 0.0f,  //23
                //Door 
                15.0f,  0.0f, 40.0f, 0.45f, 0.38f, 0.34f, //24
                15.0f,  20.0f, 40.0f, 0.45f, 0.38f, 0.34f, //25
                25.0f,  20.0f, 40.0f, 0.45f, 0.38f, 0.34f, //26
                25.0f,  0.0f, 40.0f, 0.45f, 0.38f, 0.34f,   //27      
                //Door Open
                23.0f,  13.0f, 40.0f, 0.0f, 0.0f, 0.0f, //28
                //Rough BACK
                0.0f,  40.0f, 0.0f,  0.61f, 0.04f, 0.05f,  //29
                20.0f, 60.0f, 0.0f,  0.61f, 0.04f, 0.05f,  //30
                40.0f,  40.0f, 0.0f,  0.61f, 0.04f, 0.05f,  //31
                //Rough Left
                0.0f,  40.0f, 40.0f, 0.47f, 0.0f, 0.0f,  //32
                20.0f,  60.0f, 40.0f, 0.47f, 0.0f, 0.0f,   //3
                20.0f,  60.0f, 0.0f, 0.47f, 0.0f, 0.0f,    //34
                0.0f,  40.0f, 0.0f, 0.47f, 0.0f, 0.0f,    //35
                //Rough Right
                40.0f,  40.0f, 40.0f,  0.47f, 0.0f, 0.0f,  //36
                20.0f,  60.0f, 40.0f,  0.47f, 0.0f, 0.0f,  //37
                20.0f,  60.0f, 0.0f,  0.47f, 0.0f, 0.0f,   //38
                40.0f,  40.0f, 0.0f,  0.47f, 0.0f, 0.0f,   //39
                //Rough FRONT
                0.0f,  40.0f, 40.0f,  0.61f, 0.04f, 0.05f,  //40
                20.0f,  60.0f, 40.0f,  0.61f, 0.04f, 0.05f,  //41
                40.0f,  40.0f, 40.0f,  0.61f, 0.04f, 0.05f,  //42
                // Left Windows
                5.0f,  20.0f, 40.0f,  0.3f, 0.22f, 0.08f, //43
                5.0f,  30.0f, 40.0f,  0.3f, 0.22f, 0.08f, //44
                12.0f,  30.0f, 40.0f, 0.3f, 0.22f, 0.08f, //45
                12.0f,  20.0f, 40.0f,  0.3f, 0.22f, 0.08f,  //46
                // Right Windows
                28.0f,  20.0f, 40.0f, 0.3f, 0.22f, 0.08f, //47
                28.0f,  30.0f, 40.0f,  0.3f, 0.22f, 0.08f,  //48
                35.0f,  30.0f, 40.0f,  0.3f, 0.22f, 0.08f,   //49
                35.0f,  20.0f, 40.0f,  0.3f, 0.22f, 0.08f,   //50

            };

            float[] RamadanAxesVertices = {

                 //Base BACK
                -10.0f,  0.0f, 70.0f,  0.61f, 0.04f, 0.05f,
                1.0f, 0.0f, 70.0f,  0.61f, 0.04f, 0.05f,
                0.0f,  10.0f, 71.0f,  0.61f, 0.04f, 0.05f,
                -9.0f,  10.0f, 71.0f,  0.61f, 0.04f, 0.05f,  
                //Base Left
                -10.0f,  0.0f, 70.0f, 0.47f, 1.0f, 0.0f,
                -9.0f,  10.0f, 71.0f, 0.47f, 1.0f, 0.0f,
                -8.0f,  10.0f, 78.0f, 0.47f, 1.0f, 0.0f,
                -10.0f,  0.0f, 79.0f, 0.47f, 1.0f, 0.0f,    
                //Base Right
                1.0f,   0.0f, 70.0f,  0.03f, 0.78f, 0.95f,
                0.0f,   10.0f, 70.0f,  0.03f, 0.78f, 0.95f,
                0.0f,  10.0f, 78.0f,  0.03f, 0.78f, 0.95f,
                1.0f,  0.0f, 79.0f,  0.03f, 0.78f, 0.95f,
                //Base FRONT
                -10.0f,  0.0f, 79.0f,  0.95f, 0.03f, 0.5f,
                -8.0f,  10.0f, 78.0f,  0.95f, 0.03f, 0.5f,
                0.0f,  10.0f, 78.0f,  0.95f, 0.03f, 0.5f,
                1.0f,  0.0f,  79.0f,  0.95f, 0.03f, 0.5f,
                //Mid BACK
                -9.0f,  10.0f, 71.0f,  0.61f, 0.04f, 0.05f,
                 0.0f,  10.0f, 71.0f,  0.61f, 0.04f, 0.05f,
                 2.0f,  25.0f, 69.0f,  0.61f, 0.04f, 0.05f,
                -11.0f,  25.0f, 69.0f,  0.61f, 0.04f, 0.05f,  
                //Mid Left
                -8.0f,  10.0f, 78.0f, 0.47f, 1.0f, 0.0f,
                -9.0f,  10.0f, 71.0f, 0.47f, 1.0f, 0.0f,
                -11.0f,  25.0f, 69.0f, 0.47f, 1.0f, 0.0f,
                -11.0f,  25.0f, 79.0f, 0.47f, 1.0f, 0.0f,    
                //Mid Right
                0.0f,  10.0f, 78.0f,  0.47f, 0.0f, 1.0f,
                0.0f,   10.0f, 70.0f,  0.47f, 0.0f, 1.0f,
                2.0f,  25.0f, 69.0f,  0.47f, 0.0f, 1.0f,
                2.0f,  25.0f, 79.0f,  0.47f, 0.0f, 1.0f,   
                //Mid FRONT
               -8.0f,  10.0f, 78.0f, 1.0f, 0.04f, 0.05f,
                0.0f,  10.0f, 78.0f,  1.0f, 0.04f, 0.05f,
                2.0f,  25.0f, 79.0f,  1.0f, 0.04f, 0.05f,
               -11.0f,  25.0f, 79.0f,  1.0f, 0.04f, 0.05f,             
		        //TOP Back
	           -11.0f,  25.0f, 69.0f,  0.95f, 0.33f, 0.03f,
                2.0f,  25.0f, 69.0f, 0.95f, 0.33f, 0.03f,
               -6.0f,  35.0f, 72.0f,  0.95f, 0.33f, 0.03f, 
		       //TOP Left
               -11.0f,  25.0f, 69.0f, 0.35f, 0.95f, 0.03f,
               -11.0f,  25.0f, 79.0f, 0.35f, 0.95f, 0.03f,
               -6.0f,  35.0f, 72.0f,  0.35f, 0.95f, 0.03f,                    
		       //TOP Right
                2.0f,  25.0f, 69.0f, 0.35f, 0.95f, 0.03f,
                2.0f,  25.0f, 79.0f,  0.35f, 0.95f, 0.03f,
               -6.0f,  35.0f, 72.0f,  0.35f, 0.95f, 0.03f,                     
               //TOP Front
               -11.0f,  25.0f, 79.0f,  0.95f, 0.33f, 0.03f,
                2.0f,  25.0f, 79.0f,  0.95f, 0.33f, 0.03f,
               -6.0f,  35.0f, 72.0f,  0.95f, 0.33f, 0.03f,

            };

            float[] WallVertices = {

                //GROUND
                -20.0f, 0.0f,  -20.0f, 0.15f, 0.44f, 0.13f,  //0
                -20.0f, 0.0f,100.0f, 0.0f, 0.87f, 0.07f,  //1
                100.0f, 0.0f,100.0f, 0.33f, 0.7f, 0.16f,  //2
                100.0f, 0.0f, -20.0f, 0.35f, 0.78f, 0.25f,  //3      
		        // back wall
		        -20.0f, 0.0f, -20.0f, 0.55f, 0.49f, 0.37f,  //4
               -20.0f, 20.0f, -20.0f, 0.55f, 0.49f, 0.37f,  //5
               100.0f, 20.0f, -20.0f, 0.55f, 0.49f, 0.37f,  //6
                100.0f, 0.0f, -20.0f, 0.55f, 0.49f, 0.37f,  //7
                //Left Wall
               -20.0f, 0.0f,  -20.0f, 0.22f, 0.17f, 0.07f,  //8
               -20.0f, 20.0f, -20.0f, 0.22f, 0.17f, 0.07f,  //9
               -20.0f, 20.0f, 100.0f, 0.45f, 0.44f, 0.44f,  //10
               -20.0f, 0.0f,  100.0f, 0.45f, 0.44f, 0.44f,  //11
                //Right Wall
                100.0f, 0.0f, -20.0f, 0.22f, 0.17f, 0.07f,  //12
                100.0f,20.0f, -20.0f, 0.22f, 0.17f, 0.07f,   //13
                100.0f, 20.0f, 100.0f, 0.22f, 0.17f, 0.07f,  //14
                100.0f, 0.0f,  100.0f, 0.22f, 0.17f, 0.07f, //15
                //street
                 15.0f, 0.0f,  40.0f, 0.0f, 0.0f, 0.0f,  //16
                 25.0f, 0.0f,  40.0f, 0.0f, 0.0f, 0.0f,  //17
                 25.0f, 0.0f, 100.0f, 0.0f, 0.0f, 0.0f,  //18
                 15.0f, 0.0f,  100.0f, 0.0f, 0.0f, 0.0f,  //19
                 //Front Left Wall
                -20.0f, 0.0f, 100.0f, 0.55f, 0.49f, 0.37f,  //20
                -20.0f,20.0f, 100.0f, 0.55f, 0.49f, 0.37f,  //21
                 15.0f, 20.0f, 100.0f, 0.55f, 0.49f, 0.37f,  //22
                 15.0f, 0.0f,  100.0f, 0.55f, 0.49f, 0.37f,  //23
                 //Swimming pool
                90.0f, 0.0f, 0.0f, 0.03f, 0.1f, 0.95f,  //24
                60.0f, 0.0f, 0.0f, 0.19f, 0.64f, 0.78f, //25
                60.0f, 0.0f, 70.0f, 0.03f, 0.8f, 0.95f,  //26
                90.0f, 0.0f,  70.0f, 0.03f, 0.3f, 0.95f,   //27
                //Swimming pool border
                90.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f,  //28
                60.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, //29
                60.0f, 0.0f, 70.0f, 0.0f, 0.0f, 0.0f,  //30
                90.0f, 0.0f,  70.0f, 0.0f, 0.0f, 0.0f,  //31
                //Front Right Wall
                100.0f, 0.0f, 100.0f, 0.55f, 0.49f, 0.37f,  //32
                100.0f,20.0f, 100.0f, 0.55f, 0.49f, 0.37f, //33
                25.0f, 20.0f, 100.0f, 0.55f, 0.49f, 0.37f,  //34
                25.0f, 0.0f,  100.0f, 0.55f, 0.49f, 0.37f,  //35

            };

            HouseBufferID = GPU.GenerateBuffer(Houseverts);
            WallBufferID = GPU.GenerateBuffer(WallVertices);
            RamadanBufferID = GPU.GenerateBuffer(RamadanAxesVertices);


            // SCALE MATRIX
            modelMatrix = glm.scale(new mat4(1), new vec3(1f, 1f, 1.0f));

            cam = new Camera();

            ProjectionMatrix = cam.GetProjectionMatrix();
            ViewMatrix = cam.GetViewMatrix();

            //Get a handle for our "MVP" uniform (the holder we created in the vertex shader)
            ShadertransMatrixID = Gl.glGetUniformLocation(sh.ID, "modelMatrix");
            ShaderViewMatrixID = Gl.glGetUniformLocation(sh.ID, "viewMatrix");
            ShaderProjectionMatrixID = Gl.glGetUniformLocation(sh.ID, "projectionMatrix");
        }

        public void Draw()
        {
            sh.UseShader();
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);

            Gl.glUniformMatrix4fv(ShadertransMatrixID, 1, Gl.GL_FALSE, modelMatrix.to_array());
            Gl.glUniformMatrix4fv(ShaderViewMatrixID, 1, Gl.GL_FALSE, ViewMatrix.to_array());
            Gl.glUniformMatrix4fv(ShaderProjectionMatrixID, 1, Gl.GL_FALSE, ProjectionMatrix.to_array());

            #region Wall & Swimming Pool

            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, WallBufferID);
            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)0);
            Gl.glEnableVertexAttribArray(1);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)(3 * sizeof(float)));

            // GOUND
            Gl.glDrawArrays(Gl.GL_POLYGON, 0, 4);
            // Back Wall
            Gl.glDrawArrays(Gl.GL_POLYGON, 4, 4);
            // Left  Wall
            Gl.glDrawArrays(Gl.GL_POLYGON, 8, 4);
            // Street Wall
            Gl.glDrawArrays(Gl.GL_POLYGON, 16, 4);
            // Front Left Wall
            Gl.glDrawArrays(Gl.GL_POLYGON, 20, 4);
            // Swimming pool
            Gl.glDrawArrays(Gl.GL_POLYGON, 24, 4);
            // Swimming pool border
            Gl.glDrawArrays(Gl.GL_LINE_LOOP, 28, 4);
            // Right  Wall
            Gl.glDrawArrays(Gl.GL_POLYGON, 12, 4);
            // Front Right Wall
            Gl.glDrawArrays(Gl.GL_POLYGON, 32, 4);


            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);

            #endregion

            #region House

            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, HouseBufferID);
            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)0);
            Gl.glEnableVertexAttribArray(1);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)(3 * sizeof(float)));

            //House
            Gl.glDrawArrays(Gl.GL_QUADS, 0, 6 * 4);
            //DOOR
            Gl.glDrawArrays(Gl.GL_POLYGON, 24, 4);
            //DOOR OPEN
            Gl.glPointSize(4);
            Gl.glDrawArrays(Gl.GL_POINTS, 28, 1);
            //ROUPH BACK
            Gl.glDrawArrays(Gl.GL_POLYGON, 29, 3);
            //ROUPH LEFT
            Gl.glDrawArrays(Gl.GL_POLYGON, 32, 4);
            //ROUPH Right
            Gl.glDrawArrays(Gl.GL_POLYGON, 36, 4);
            //ROUPH FRONT
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 40, 3);
            //Left   Windows
            Gl.glDrawArrays(Gl.GL_TRIANGLE_FAN, 43, 4);
            //RIGHT   Windows
            Gl.glDrawArrays(Gl.GL_TRIANGLE_FAN, 47, 4);

            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);
            #endregion

            #region Ramadan axis

            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, RamadanBufferID);
            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)0);
            Gl.glEnableVertexAttribArray(1);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)(3 * sizeof(float)));

            Gl.glDrawArrays(Gl.GL_QUADS, 0, 32);
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 32, 3);
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 35, 3);
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 38, 3);
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 41, 3);


            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);

            #endregion
        }
        public void Update()
        {
            cam.UpdateViewMatrix();
            ProjectionMatrix = cam.GetProjectionMatrix();
            ViewMatrix = cam.GetViewMatrix();
        }

        public void CleanUp()
        {
            sh.DestroyShader();
        }
    }
}
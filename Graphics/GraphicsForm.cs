﻿using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.Reflection.Emit;

namespace Graphics
{
    public partial class GraphicsForm : Form
    {
        Renderer renderer = new Renderer();
        Thread MainLoopThread;

        public GraphicsForm()
        {
            InitializeComponent();
            simpleOpenGlControl1.InitializeContexts();
            initialize();
            MainLoopThread = new Thread(MainLoop);
            MainLoopThread.Start();
        }
        void initialize()
        {
            renderer.Initialize();   
        }
        void MainLoop()
        {
            while (true)
            {
                renderer.Update();
                simpleOpenGlControl1.Invoke(new MethodInvoker(() => {
                    renderer.Draw();
                    simpleOpenGlControl1.Refresh();
                }));
                Thread.Sleep(1); //add a short delay to reduce CPU usage
            }
        }
        private void GraphicsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            renderer.CleanUp();
            MainLoopThread.Abort();
        }

        private void simpleOpenGlControl1_Paint(object sender, PaintEventArgs e)
        {
            renderer.Draw();
        }

   
        private void simpleOpenGlControl1_KeyPress(object sender, KeyPressEventArgs e)
        {
            float speed = 0.3f;
            if (e.KeyChar == 'a')
                renderer.cam.Strafe(-speed*3);
            if (e.KeyChar == 'd')
                renderer.cam.Strafe(speed*3);
            if (e.KeyChar == 's')
                renderer.cam.Walk(-speed*3);
            if (e.KeyChar == 'w')
                renderer.cam.Walk(speed * 3);
            if (e.KeyChar == 'z')
                renderer.cam.Fly(-speed);
            if (e.KeyChar == 'c')
                renderer.cam.Fly(speed);
            if (e.KeyChar == 'x')
                renderer.cam.Pitch(-speed);
            if (e.KeyChar == 'v')
                renderer.cam.Pitch(speed);
            if (e.KeyChar == 'e')
                renderer.cam.Yaw(-speed);
            if (e.KeyChar == 'q')
                renderer.cam.Yaw(speed);
        }

        private void simpleOpenGlControl1_Load(object sender, System.EventArgs e)
        {        

        }
        float prevX, prevY;
        private void simpleOpenGlControl1_MouseMove(object sender, MouseEventArgs e)
        {
   /*         float speed = 0.01f;
            float delta = e.X - prevX;
            if (delta > 2)
                renderer.cam.Yaw(-speed);
            else if (delta < -2)
                renderer.cam.Yaw(speed);
            delta = e.Y - prevY;
            if (delta > 2)
                renderer.cam.Pitch(-speed);
            else if (delta < -2)
                renderer.cam.Pitch(speed);
            MoveCursor();*/
        }

  

        private void MoveCursor()
        {
            this.Cursor = new Cursor(Cursor.Current.Handle);
            Point p = PointToScreen(simpleOpenGlControl1.Location);
            Cursor.Position = new Point(simpleOpenGlControl1.Size.Width / 2 + p.X, simpleOpenGlControl1.Size.Height / 2 + p.Y);
            Cursor.Clip = new Rectangle(this.Location, this.Size);
            prevX = simpleOpenGlControl1.Location.X + simpleOpenGlControl1.Size.Width / 2;
            prevY = simpleOpenGlControl1.Location.Y + simpleOpenGlControl1.Size.Height / 2;
        }
 
    
    }
}
